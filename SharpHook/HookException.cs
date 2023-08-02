namespace SharpHook;

using System.Runtime.Serialization;

/// <summary>
/// Represents an exception related to the global hook.
/// </summary>
[Serializable]
[ExcludeFromCodeCoverage]
public sealed class HookException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HookException" /> class.
    /// </summary>
    public HookException()
        : this(UioHookResult.Failure)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="HookException" /> class.
    /// </summary>
    /// <param name="result">The result of an opetaion which caused this exception.</param>
    public HookException(UioHookResult result)
        : base($"Hook exception based on result: {result}") =>
        this.Result = result;

    /// <summary>
    /// Initializes a new instance of the <see cref="HookException" /> class.
    /// </summary>
    /// <param name="message">The message of the exception.</param>
    public HookException(string message)
        : this(UioHookResult.Failure, message)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="HookException" /> class.
    /// </summary>
    /// <param name="result">The result of an opetaion which caused this exception.</param>
    /// <param name="message">The message of the exception.</param>
    public HookException(UioHookResult result, string message)
        : base(message) =>
        this.Result = result;

    /// <summary>
    /// Initializes a new instance of the <see cref="HookException" /> class.
    /// </summary>
    /// <param name="result">The result of an opetaion which caused this exception.</param>
    /// <param name="innerException">The exception which caused this exception.</param>
    public HookException(UioHookResult result, Exception innerException)
        : this(result, $"Hook exception based on result: {result}", innerException) =>
        this.Result = result;

    /// <summary>
    /// Initializes a new instance of the <see cref="HookException" /> class.
    /// </summary>
    /// <param name="message">The message of the exception.</param>
    /// <param name="innerException">The exception which caused this exception.</param>
    public HookException(string message, Exception innerException)
        : this(UioHookResult.Failure, message, innerException)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="HookException" /> class.
    /// </summary>
    /// <param name="result">The result of an opetaion which caused this exception.</param>
    /// <param name="message">The message of the exception.</param>
    /// <param name="innerException">The exception which caused this exception.</param>
    public HookException(UioHookResult result, string message, Exception innerException)
        : base(message, innerException) =>
        this.Result = result;

    private HookException(SerializationInfo info, StreamingContext context)
        : base(info, context) =>
        this.Result = (UioHookResult)
            (info.GetValue(nameof(this.Result), typeof(UioHookResult)) ?? UioHookResult.Failure);

    /// <summary>
    /// Sets the <see cref="SerializationInfo" /> with information about this exception.
    /// </summary>
    /// <param name="info">The serialization info to set.</param>
    /// <param name="context">The streaming context.</param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(Result), this.Result);
    }

    /// <summary>
    /// Gets the result of an opetaion which caused this exception.
    /// </summary>
    /// <value>The result of an opetaion which caused this exception.</value>
    public UioHookResult Result { get; }
}
