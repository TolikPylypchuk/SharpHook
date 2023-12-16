namespace SharpHook.Internal;

[ExcludeFromCodeCoverage]
internal static partial class NativeStringFunctions
{
    private const string Msvcrt = "msvcrt";
    private const string Libc = "libc";
    private const string LibSystem = "libSystem";

#if NET7_0_OR_GREATER
    [LibraryImport(Msvcrt, EntryPoint = "vsprintf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int WindowsVsprintf(IntPtr buffer, IntPtr format, IntPtr args);
#else
    [DllImport(Msvcrt, EntryPoint = "vsprintf", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int WindowsVsprintf(IntPtr buffer, IntPtr format, IntPtr args);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(Libc, EntryPoint = "vsprintf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int LinuxVsprintf(IntPtr buffer, IntPtr format, IntPtr args);
#else
    [DllImport(Libc, EntryPoint = "vsprintf", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int LinuxVsprintf(IntPtr buffer, IntPtr format, IntPtr args);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibSystem, EntryPoint = "vasprintf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int MacVasprintf(ref IntPtr buffer, IntPtr format, IntPtr args);
#else
    [DllImport(LibSystem, EntryPoint = "vasprintf", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int MacVasprintf(ref IntPtr buffer, IntPtr format, IntPtr args);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(Msvcrt, EntryPoint = "vsnprintf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int WindowsVsnprintf(IntPtr buffer, UIntPtr size, IntPtr format, IntPtr args);
#else
    [DllImport(Msvcrt, EntryPoint = "vsnprintf", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int WindowsVsnprintf(IntPtr buffer, UIntPtr size, IntPtr format, IntPtr args);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(Libc, EntryPoint = "vsnprintf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int LinuxVsnprintf(IntPtr buffer, UIntPtr size, IntPtr format, IntPtr args);
#else
    [DllImport(Libc, EntryPoint = "vsnprintf", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int LinuxVsnprintf(IntPtr buffer, UIntPtr size, IntPtr format, IntPtr args);
#endif
}
