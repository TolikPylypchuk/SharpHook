namespace SharpHook.Internal;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

[ExcludeFromCodeCoverage]
internal static class StringUtil
{
    internal static string ToStringFromUtf8(this IntPtr nativeString)
    {
        if (nativeString == IntPtr.Zero)
        {
            return String.Empty;
        }

        var length = 0;

        while (Marshal.ReadByte(nativeString, length) != 0)
        {
            length++;
        }

        var buffer = new byte[length];
        Marshal.Copy(nativeString, buffer, 0, buffer.Length);

        return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
    }
}
