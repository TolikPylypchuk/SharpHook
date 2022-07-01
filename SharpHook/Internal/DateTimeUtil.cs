namespace SharpHook.Internal;

using System;

internal static class DateTimeUtil
{
    private static readonly DateTimeOffset Epoch =
#if NETCOREAPP3_0_OR_GREATER
        DateTimeOffset.UnixEpoch;
#else
        new(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
#endif

    internal static DateTimeOffset ToDateTimeOffset(this ulong timestamp) =>
        Epoch.AddMilliseconds(timestamp);
}
