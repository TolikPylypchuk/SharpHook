namespace SharpHook.Data;

/// <summary>
/// Contains extension methods for <see cref="EventMask" />.
/// </summary>
public static class EventMaskExtensions
{
    extension(EventMask mask)
    {
        /// <summary>
        /// Returns <see langword="true" /> if the event mask contains either <see cref="EventMask.LeftShift" /> or
        /// <see cref="EventMask.RightShift"/>. Otherwise, returns <see langword="false" />.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the event mask contains either <see cref="EventMask.LeftShift" /> or
        /// <see cref="EventMask.RightShift"/>. Otherwise, <see langword="false" />.
        /// </returns>
        public bool HasShift() =>
            mask.HasAny(EventMask.Shift);

        /// <summary>
        /// Returns <see langword="true" /> if the event mask contains either <see cref="EventMask.LeftCtrl" /> or
        /// <see cref="EventMask.RightCtrl"/>. Otherwise, returns <see langword="false" />.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the event mask contains either <see cref="EventMask.LeftCtrl" /> or
        /// <see cref="EventMask.RightCtrl"/>. Otherwise, <see langword="false" />.
        /// </returns>
        public bool HasCtrl() =>
            mask.HasAny(EventMask.Ctrl);

        /// <summary>
        /// Returns <see langword="true" /> if the event mask contains either <see cref="EventMask.LeftAlt" /> or
        /// <see cref="EventMask.RightAlt"/>. Otherwise, returns <see langword="false" />.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the event mask contains either <see cref="EventMask.LeftAlt" /> or
        /// <see cref="EventMask.RightAlt"/>. Otherwise, <see langword="false" />.
        /// </returns>
        public bool HasAlt() =>
            mask.HasAny(EventMask.Alt);

        /// <summary>
        /// Returns <see langword="true" /> if the event mask contains either <see cref="EventMask.LeftMeta" /> or
        /// <see cref="EventMask.RightMeta"/>. Otherwise, returns <see langword="false" />.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the event mask contains either <see cref="EventMask.LeftMeta" /> or
        /// <see cref="EventMask.RightMeta"/>. Otherwise, <see langword="false" />.
        /// </returns>
        public bool HasMeta() =>
            mask.HasAny(EventMask.Meta);

        /// <summary>
        /// Returns <see langword="true" /> if the event mask contains any of the specified flags.
        /// Otherwise, returns <see langword="false" />.
        /// </summary>
        /// <param name="flagsToCheck">The mask flags to check against.</param>
        /// <returns>
        /// <see langword="true" /> if the event mask contains any of the specified flags.
        /// Otherwise, <see langword="false" />.
        /// </returns>
        /// <remarks>
        /// This method returns <see langword="false" /> if <paramref name="flagsToCheck" /> equals
        /// <see cref="EventMask.None" />.
        /// </remarks>
        public bool HasAny(EventMask flagsToCheck) =>
            (mask & flagsToCheck) != EventMask.None;

        /// <summary>
        /// Returns <see langword="true" /> if the event mask contains all of the specified flags.
        /// Otherwise, returns <see langword="false" />.
        /// </summary>
        /// <param name="flagsToCheck">The mask flags to check against.</param>
        /// <returns>
        /// <see langword="true" /> if the event mask contains all of the specified flags.
        /// Otherwise, <see langword="false" />.
        /// </returns>
        /// <remarks>
        /// This method returns <see langword="true" /> if <paramref name="flagsToCheck" /> equals
        /// <see cref="EventMask.None" />.
        /// </remarks>
        public bool HasAll(EventMask flagsToCheck) =>
            (mask & flagsToCheck) == flagsToCheck;

        /// <summary>
        /// Splits an event mask into an array of individual flags.
        /// </summary>
        /// <returns>
        /// An array of individual flags contained in the mask, or an empty array if the mask is
        /// <see cref="EventMask.None" />.
        /// </returns>
        public EventMask[] Split() =>
#if NET5_0_OR_GREATER
        Enum.GetValues<EventMask>()
#else
            Enum.GetValues(typeof(EventMask))
                .Cast<EventMask>()
#endif
                .Where(m =>
                    m != EventMask.None &&
                    m != EventMask.Shift &&
                    m != EventMask.Ctrl &&
                    m != EventMask.Alt &&
                    m != EventMask.Meta)
                .Where(m => mask.HasFlag(m))
                .ToArray();
    }

    extension(EventMask[] flags)
    {
        /// <summary>
        /// Merges an array of event mask flags into a single event mask.
        /// </summary>
        /// <returns>An event mask which contains the specified mask flags.</returns>
        /// <exception cref="ArgumentNullException">
        /// The flags array is <see langword="null" />.
        /// </exception>
        public EventMask Merge() =>
            flags != null
                ? flags.Aggregate(EventMask.None, (mask, flag) => mask | flag)
                : throw new ArgumentNullException(nameof(flags));
    }
}
