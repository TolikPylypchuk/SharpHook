namespace SharpHook.Internal;

using System;
using System.Runtime.InteropServices;

internal static class Native
{
    private const string Msvcrt = "msvcrt";
    private const string Libc = "libc";
    private const string LibSystem = "libSystem";

    [DllImport(Msvcrt, EntryPoint = "vsprintf", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int WindowsVsprintf(IntPtr buffer, IntPtr format, IntPtr args);

    [DllImport(Libc, EntryPoint = "vsprintf", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int LinuxVsprintf(IntPtr buffer, IntPtr format, IntPtr args);

    [DllImport(LibSystem, EntryPoint = "vasprintf", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int MacVasprintf(ref IntPtr buffer, IntPtr format, IntPtr args);

    [DllImport(Msvcrt, EntryPoint = "vsnprintf", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int WindowsVsnprintf(IntPtr buffer, UIntPtr size, IntPtr format, IntPtr args);

    [DllImport(Libc, EntryPoint = "vsnprintf", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int LinuxVsnprintf(IntPtr buffer, UIntPtr size, IntPtr format, IntPtr args);
}
