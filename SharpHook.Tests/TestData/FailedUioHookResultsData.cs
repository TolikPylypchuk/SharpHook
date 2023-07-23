namespace SharpHook.TestData;

using System.Collections;

public sealed class FailedUioHookResultsData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator() =>
        Enum.GetValues<UioHookResult>()
            .Where(result => result != UioHookResult.Success)
            .Select(result => new object[] { result })
            .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        this.GetEnumerator();
}
