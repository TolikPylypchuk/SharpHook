namespace SharpHook.Internal;

using System;
using System.Runtime.CompilerServices;

internal static class HashCodeUtil
{
#if !NETCOREAPP3_0_OR_GREATER
    private const int HashStart = unchecked((int)2166136261);
    private const int HashMultiplier = 16777619;
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int GetHashCode<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
    {
#if NETCOREAPP3_0_OR_GREATER
        return HashCode.Combine(value1, value2, value3);
#else
        unchecked
        {
            int hash = HashStart;
            hash = (hash * HashMultiplier) ^ value1?.GetHashCode() ?? 0;
            hash = (hash * HashMultiplier) ^ value2?.GetHashCode() ?? 0;
            hash = (hash * HashMultiplier) ^ value3?.GetHashCode() ?? 0;
            return hash;
        }
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int GetHashCode<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
    {
#if NETCOREAPP3_0_OR_GREATER
        return HashCode.Combine(value1, value2, value3, value4);
#else
        unchecked
        {
            int hash = HashStart;
            hash = (hash * HashMultiplier) ^ value1?.GetHashCode() ?? 0;
            hash = (hash * HashMultiplier) ^ value2?.GetHashCode() ?? 0;
            hash = (hash * HashMultiplier) ^ value3?.GetHashCode() ?? 0;
            hash = (hash * HashMultiplier) ^ value4?.GetHashCode() ?? 0;
            return hash;
        }
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int GetHashCode<T1, T2, T3, T4, T5>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
    {
#if NETCOREAPP3_0_OR_GREATER
        return HashCode.Combine(value1, value2, value3, value4, value5);
#else
        unchecked
        {
            int hash = HashStart;
            hash = (hash * HashMultiplier) ^ value1?.GetHashCode() ?? 0;
            hash = (hash * HashMultiplier) ^ value2?.GetHashCode() ?? 0;
            hash = (hash * HashMultiplier) ^ value3?.GetHashCode() ?? 0;
            hash = (hash * HashMultiplier) ^ value4?.GetHashCode() ?? 0;
            hash = (hash * HashMultiplier) ^ value5?.GetHashCode() ?? 0;
            return hash;
        }
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int GetHashCode<T1, T2, T3, T4, T5, T6>(
        T1 value1,
        T2 value2,
        T3 value3,
        T4 value4,
        T5 value5,
        T6 value6)
    {
#if NETCOREAPP3_0_OR_GREATER
        return HashCode.Combine(value1, value2, value3, value4, value5, value6);
#else
        unchecked
        {
            int hash = HashStart;
            hash = (hash * HashMultiplier) ^ value1?.GetHashCode() ?? 0;
            hash = (hash * HashMultiplier) ^ value2?.GetHashCode() ?? 0;
            hash = (hash * HashMultiplier) ^ value3?.GetHashCode() ?? 0;
            hash = (hash * HashMultiplier) ^ value4?.GetHashCode() ?? 0;
            hash = (hash * HashMultiplier) ^ value5?.GetHashCode() ?? 0;
            hash = (hash * HashMultiplier) ^ value6?.GetHashCode() ?? 0;
            return hash;
        }
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int GetHashCode<T1, T2, T3, T4, T5, T6, T7>(
        T1 value1,
        T2 value2,
        T3 value3,
        T4 value4,
        T5 value5,
        T6 value6,
        T7 value7)
    {
#if NETCOREAPP3_0_OR_GREATER
        return HashCode.Combine(value1, value2, value3, value4, value5, value6, value7);
#else
        unchecked
        {
            int hash = HashStart;
            hash = (hash * HashMultiplier) ^ value1?.GetHashCode() ?? 0;
            hash = (hash * HashMultiplier) ^ value2?.GetHashCode() ?? 0;
            hash = (hash * HashMultiplier) ^ value3?.GetHashCode() ?? 0;
            hash = (hash * HashMultiplier) ^ value4?.GetHashCode() ?? 0;
            hash = (hash * HashMultiplier) ^ value5?.GetHashCode() ?? 0;
            hash = (hash * HashMultiplier) ^ value6?.GetHashCode() ?? 0;
            hash = (hash * HashMultiplier) ^ value7?.GetHashCode() ?? 0;
            return hash;
        }
#endif
    }
}
