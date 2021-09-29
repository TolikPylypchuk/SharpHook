using System;
using System.Runtime.Serialization;

using SharpHook.Native;

namespace SharpHook
{
    [Serializable]
    public sealed class HookException : Exception
    {
        public HookException()
            : this(UioHookResult.Failure)
        { }

        public HookException(UioHookResult result)
            : base($"Hook exception based on result: {result}") =>
            this.Result = result;

        public HookException(string message)
            : this(UioHookResult.Failure, message)
        { }

        public HookException(UioHookResult result, string message)
            : base(message) =>
            this.Result = result;

        public HookException(string message, Exception innerException)
            : this(UioHookResult.Failure, message, innerException)
        { }

        public HookException(UioHookResult result, string message, Exception innerException)
            : base(message, innerException) =>
            this.Result = result;

        private HookException(SerializationInfo info, StreamingContext context)
            : base(info, context) =>
            this.Result = (UioHookResult)
                (info.GetValue(nameof(this.Result), typeof(UioHookResult)) ?? UioHookResult.Failure);

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(Result), this.Result);
        }

        public UioHookResult Result { get; }
    }
}
