namespace SharpHook.TestData;

public record struct DateTimeAfterEpoch(DateTimeOffset Value);

public record struct FailedUioHookResult(UioHookResult Value);

public record struct KeyboardEvent(UioHookEvent Value);

public record struct MouseEvent(UioHookEvent Value);
