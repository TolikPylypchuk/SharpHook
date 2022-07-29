namespace SharpHook.Logging;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using SharpHook.Internal;
using SharpHook.Native;

/// <summary>
/// Creates log entries from native log formats and arguments.
/// </summary>
/// <remarks>
/// The log text is formatted using the <c>vsprintf</c> function from the C runtime and then the result is parsed to
/// extract the arguments. This means that on Windows this class requires the Visual C++ Redistributable package to be
/// installed.
/// </remarks>
public sealed class LogEntryParser
{
    private static readonly Regex ArgumentRegex = new(
        @"%\+?\-? ?#?0?(?:[1-9][0-9]*|\*)?(?:\.(?:[1-9][0-9]*|\*))?(?:hh|ll|[hljztL])?[diuoxXfFeEgGaAcspn]",
        RegexOptions.Compiled);

    /// <summary>
    /// Parses a native log format and arguments to create a log entry.
    /// </summary>
    /// <param name="level">The log level.</param>
    /// <param name="format">A pointer to the native format of the log message.</param>
    /// <param name="args">A pointer to the native arguments of the log message.</param>
    /// <returns>A log entry represented by the level, format, and argumets.</returns>
    public LogEntry ParseNativeLogEntry(LogLevel level, IntPtr format, IntPtr args)
    {
        string rawFormat = format.ToStringFromUtf8().Trim();
        string message = this.GetLogMessage(format, args).Trim();
        var result = this.ExtractArguments(message, rawFormat);

        return new(
            level,
            message,
            result.Format,
            rawFormat,
            result.Arguments,
            result.RawArguments,
            result.ArgumentPlaceholders);
    }

    private string GetLogMessage(IntPtr format, IntPtr args)
    {
        if (PlatformDetector.IsMacOS)
        {
            return this.GetMacString(format, args);
        }

        if (PlatformDetector.IsLinux && Environment.Is64BitProcess)
        {
            return this.GetLinuxX64String(format, args);
        }

        return this.GetDefaultString(format, args);
    }

    private FormatAndArguments ExtractArguments(string message, string rawFormat)
    {
        var matches = ArgumentRegex.Matches(rawFormat);
        var rawParts = ArgumentRegex.Split(rawFormat);

        if (rawParts.Length <= 1)
        {
            return new(
                this.NormalizeFormat(rawFormat),
                Array.Empty<object>(),
                Array.Empty<string>(),
                Array.Empty<string>());
        }

        var arguments = new List<object>();
        var rawArguments = new List<string>();
        var placeholders = new List<string>();

        for (int i = 0, index = message.IndexOf(rawParts[i]), nextIndex = index;
            i < rawParts.Length - 1;
            i++, index = nextIndex + rawParts[i].Length)
        {
            nextIndex = message.IndexOf(rawParts[i + 1], nextIndex + rawParts[i].Length);

            var placeholder = matches[i].Value;
            var argument = message.Substring(index, nextIndex - index);

            arguments.Add(this.ParseArgument(placeholder, argument));
            rawArguments.Add(argument);
            placeholders.Add(placeholder);
        }

        var format = Enumerable.Range(1, rawParts.Length - 1).Aggregate(
            this.NormalizeFormat(rawParts[0]),
            (acc, index) => $"{acc}{{{index - 1}}}{this.NormalizeFormat(rawParts[index])}");

        return new(format, arguments.AsReadOnly(), rawArguments.AsReadOnly(), placeholders.AsReadOnly());
    }

    private object ParseArgument(string format, string argument)
    {
        char specifier = format[format.Length - 1];

        return specifier switch
        {
            'd' or 'i' => this.ParseArgumentLength(format) switch
            {
                ArgumentLength.Byte when SByte.TryParse(argument, out sbyte result) => result,
                ArgumentLength.Short when Int16.TryParse(argument, out short result) => result,
                ArgumentLength.None when Int32.TryParse(argument, out int result) => result,
                ArgumentLength.Long when Int64.TryParse(argument, out long result) => result,
                _ => argument
            },
            'u' => this.ParseArgumentLength(format) switch
            {
                ArgumentLength.Byte when Byte.TryParse(argument, out byte result) => result,
                ArgumentLength.Short when UInt16.TryParse(argument, out ushort result) => result,
                ArgumentLength.None when UInt32.TryParse(argument, out uint result) => result,
                ArgumentLength.Long when UInt64.TryParse(argument, out ulong result) => result,
                _ => argument
            },
            'o' => this.ParseArgumentLength(format) switch
            {
                ArgumentLength.Byte when
                    TryParseOctal(argument, Convert.ToByte, () => (byte)0, out byte result) => result,
                ArgumentLength.Short when
                    TryParseOctal(argument, Convert.ToUInt16, () => (ushort)0, out ushort result) => result,
                ArgumentLength.None when
                    TryParseOctal(argument, Convert.ToUInt32, () => (uint)0, out uint result) => result,
                ArgumentLength.Long when
                    TryParseOctal(argument, Convert.ToUInt64, () => (ulong)0, out ulong result) => result,
                _ => argument
            },
            'x' or 'X' => this.ParseArgumentLength(format) switch
            {
                ArgumentLength.Byte when
                    Byte.TryParse(argument, NumberStyles.AllowHexSpecifier, null, out byte result) => result,
                ArgumentLength.Short when
                    UInt16.TryParse(argument, NumberStyles.AllowHexSpecifier, null, out ushort result) => result,
                ArgumentLength.None when
                    UInt32.TryParse(argument, NumberStyles.AllowHexSpecifier, null, out uint result) => result,
                ArgumentLength.Long when
                    UInt64.TryParse(argument, NumberStyles.AllowHexSpecifier, null, out ulong result) => result,
                _ => argument
            },
            'f' or 'F' or 'e' or 'E' or 'g' or 'G' => this.ParseArgumentLength(format) switch
            {
                ArgumentLength.None when Double.TryParse(argument, out double result) => result,
                ArgumentLength.Long when Decimal.TryParse(argument, out decimal result) => result,
                _ => argument
            },
            'c' when Char.TryParse(argument, out char result) => result,
            'p' when Int64.TryParse(argument, NumberStyles.AllowHexSpecifier, null, out long result) => (IntPtr)result,
            _ => argument
        };
    }

    private ArgumentLength ParseArgumentLength(string format) =>
        format[format.Length - 2] switch
        {
            'h' when format.Length >= 3 && format[format.Length - 3] == 'h' => ArgumentLength.Byte,
            'h' => ArgumentLength.Short,
            'l' or 'L' or 'j' => ArgumentLength.Long,
            _ => ArgumentLength.None
        };

    private bool TryParseOctal<T>(string num, Func<string, int, T> convert, Func<T> zero, out T result)
    {
        try
        {
            result = convert(num, 8);
            return true;
        } catch
        {
            result = zero();
            return false;
        }
    }

    private string NormalizeFormat(string part) =>
        part.Replace("%%", "%").Replace("{", "{{").Replace("}", "}}");

    private string GetDefaultString(IntPtr format, IntPtr args)
    {
        var byteLength = Vsnprintf(IntPtr.Zero, UIntPtr.Zero, format, args) + 1;

        if (byteLength <= 1)
        {
            return String.Empty;
        }

        var buffer = IntPtr.Zero;

        try
        {
            buffer = Marshal.AllocHGlobal(byteLength);
            Vsprintf(buffer, format, args);
            return buffer.ToStringFromUtf8();
        } finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    private string GetMacString(IntPtr format, IntPtr args)
    {
        var buffer = IntPtr.Zero;
        try
        {
            var count = Native.MacVasprintf(ref buffer, format, args);
            return count != -1 ? buffer.ToStringFromUtf8() : String.Empty;
        } finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    private string GetLinuxX64String(IntPtr format, IntPtr args)
    {
        var listStructure = Marshal.PtrToStructure<VaListLinuxX64>(args);

        var byteLength = 0;

        UseStructurePointer(listStructure, listPointer =>
        {
            byteLength = Native.LinuxVsnprintf(IntPtr.Zero, UIntPtr.Zero, format, listPointer) + 1;
        });

        var utf8Buffer = IntPtr.Zero;

        try
        {
            utf8Buffer = Marshal.AllocHGlobal(byteLength);

            return UseStructurePointer(listStructure, listPointer =>
            {
                int result = Native.LinuxVsprintf(utf8Buffer, format, listPointer);
                return result >= 0 ? utf8Buffer.ToStringFromUtf8() : String.Empty;
            });
        } finally
        {
            Marshal.FreeHGlobal(utf8Buffer);
        }
    }

    private R UseStructurePointer<T, R>(T structure, Func<IntPtr, R> action) where T : notnull
    {
        var structurePointer = IntPtr.Zero;
        try
        {
            structurePointer = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
            Marshal.StructureToPtr(structure, structurePointer, false);
            return action(structurePointer);
        } finally
        {
            Marshal.FreeHGlobal(structurePointer);
        }
    }

    private void UseStructurePointer<T>(T structure, Action<IntPtr> action) where T : notnull
    {
        var structurePointer = IntPtr.Zero;
        try
        {
            structurePointer = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
            Marshal.StructureToPtr(structure, structurePointer, false);
            action(structurePointer);
        } finally
        {
            Marshal.FreeHGlobal(structurePointer);
        }
    }

    private int Vsprintf(IntPtr buffer, IntPtr format, IntPtr args) =>
        PlatformDetector.IsWindows
            ? Native.WindowsVsprintf(buffer, format, args)
            : PlatformDetector.IsLinux ? Native.LinuxVsprintf(buffer, format, args) : -1;

    private int Vsnprintf(IntPtr buffer, UIntPtr size, IntPtr format, IntPtr args) =>
        PlatformDetector.IsWindows
            ? Native.WindowsVsnprintf(buffer, size, format, args)
            : PlatformDetector.IsLinux ? Native.LinuxVsnprintf(buffer, size, format, args) : -1;

    private struct FormatAndArguments
    {
        internal FormatAndArguments(
            string format,
            IReadOnlyList<object> arguments,
            IReadOnlyList<string> rawArguments,
            IReadOnlyList<string> argumentPlaceholders)
        {
            this.Format = format;
            this.Arguments = arguments;
            this.RawArguments = rawArguments;
            this.ArgumentPlaceholders = argumentPlaceholders;
        }

        internal string Format { get; }
        internal IReadOnlyList<object> Arguments { get; }
        internal IReadOnlyList<string> RawArguments { get; }
        internal IReadOnlyList<string> ArgumentPlaceholders { get; }
    }

    private enum ArgumentLength
    {
        None,
        Byte,
        Short,
        Long
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    private struct VaListLinuxX64
    {
        internal uint GpOffset;
        internal uint FpOffset;
        internal IntPtr OverflowArgArea;
        internal IntPtr RegSaveArea;
    }
}
