namespace SharpHook.Internal;

[ExcludeFromCodeCoverage]
internal static partial class NativeStringFunctions
{
    private const string Msvcrt = "msvcrt";
    private const string LibC = "libc";
    private const string LibSystem = "libSystem";

#if NET7_0_OR_GREATER
    [LibraryImport(Msvcrt, EntryPoint = "vsprintf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int WindowsVsprintf(nint buffer, nint format, nint args);
#else
    [DllImport(Msvcrt, EntryPoint = "vsprintf", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int WindowsVsprintf(nint buffer, nint format, nint args);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibC, EntryPoint = "vsprintf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int LinuxVsprintf(nint buffer, nint format, nint args);
#else
    [DllImport(LibC, EntryPoint = "vsprintf", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int LinuxVsprintf(nint buffer, nint format, nint args);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibSystem, EntryPoint = "vasprintf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int MacVasprintf(ref nint buffer, nint format, nint args);
#else
    [DllImport(LibSystem, EntryPoint = "vasprintf", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int MacVasprintf(ref nint buffer, nint format, nint args);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(Msvcrt, EntryPoint = "vsnprintf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int WindowsVsnprintf(nint buffer, nuint size, nint format, nint args);
#else
    [DllImport(Msvcrt, EntryPoint = "vsnprintf", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int WindowsVsnprintf(nint buffer, nuint size, nint format, nint args);
#endif

#if NET7_0_OR_GREATER
    [LibraryImport(LibC, EntryPoint = "vsnprintf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int LinuxVsnprintf(nint buffer, nuint size, nint format, nint args);
#else
    [DllImport(LibC, EntryPoint = "vsnprintf", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int LinuxVsnprintf(nint buffer, nuint size, nint format, nint args);
#endif
}
