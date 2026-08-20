namespace SharpHook.Internal;

[ExcludeFromCodeCoverage]
internal static class PlatformDetector
{
#if !NET40_OR_GREATER && !NET6_0_OR_GREATER
    private static readonly OSPlatform MacCatalystPlatform = OSPlatform.Create("MACCATALYST");
#endif

    internal static bool IsWindows =>
#if NET40_OR_GREATER
        Environment.OSVersion.Platform == PlatformID.Win32NT;
#elif NET6_0_OR_GREATER
        OperatingSystem.IsWindows();
#else
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif

    internal static bool IsMacOS =>
#if NET40_OR_GREATER
        false;
#elif NET6_0_OR_GREATER
        OperatingSystem.IsMacOS() || OperatingSystem.IsMacCatalyst();
#else
        RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(MacCatalystPlatform);
#endif

    internal static bool IsLinux =>
#if NET40_OR_GREATER
        Environment.OSVersion.Platform == PlatformID.Unix;
#elif NET6_0_OR_GREATER
        OperatingSystem.IsLinux();
#else
        RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
#endif
}
