namespace SharpHook.Internal;

[ExcludeFromCodeCoverage]
internal static class PlatformDetector
{
    internal static bool IsWindows =>
#if NET40_OR_GREATER
        Environment.OSVersion.Platform == PlatformID.Win32NT;
#elif NET5_0_OR_GREATER
        OperatingSystem.IsWindows();
#else
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif

    internal static bool IsMacOS =>
#if NET40_OR_GREATER
        false;
#elif NET5_0_OR_GREATER
        OperatingSystem.IsMacOS();
#else
        RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#endif

    internal static bool IsLinux =>
#if NET40_OR_GREATER
        Environment.OSVersion.Platform == PlatformID.Unix;
#elif NET5_0_OR_GREATER
        OperatingSystem.IsLinux();
#else
        RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
#endif
}
