namespace SharpHook.Logging;

using System;
using System.Runtime.InteropServices;

using SharpHook.Internal;
using SharpHook.Native;

/// <summary>
/// Creates log entries from log formats and native varargs.
/// </summary>
public sealed class LogEntryParser
{
    /// <summary>
    /// Parses a log format and native varargs to create a log entry.
    /// </summary>
    /// <param name="level">The log level.</param>
    /// <param name="format">The format of the log message.</param>
    /// <param name="args">The native varargs of the log message.</param>
    /// <returns>A log entry represented by the level, format, and argumets.</returns>
    public LogEntry ParseNativeLogEntry(LogLevel level, IntPtr format, IntPtr args) =>
        new(level, this.GetLogMessage(format, args), String.Empty, String.Empty);

    internal string GetLogMessage(IntPtr format, IntPtr args)
    {
        if (PlatformDetector.IsMacOS)
        {
            return GetMacString(format, args);
        }

        if (PlatformDetector.IsLinux && Environment.Is64BitProcess)
        {
            return GetLinuxX64String(format, args);
        }

        return this.GetDefaultString(format, args);
    }

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
                Native.LinuxVsprintf(utf8Buffer, format, listPointer);
                return utf8Buffer.ToStringFromUtf8();
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

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    private struct VaListLinuxX64
    {
        internal uint GpOffset;
        internal uint FpOffset;
        internal IntPtr OverflowArgArea;
        internal IntPtr RegSaveArea;
    }
}
