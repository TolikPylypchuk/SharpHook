# Limitations

## Working Directory

You have to remember that libuiohook binaries should be present in the curent working directory. This is how P/Invoke
works, and it can cause the 'library not found' issues when running your client app from a different directory.

## Missing libuiohook Feature

libuiohook supports hooking into its logging capabilities so that you can get its logs. SharpHook doesn't support this
feature. The reason is that you should call `hook_set_logger_proc` and pass your callback for logging. This is
similar to `hook_set_dispatch_proc`, but this time the callback should accept a variable number of arguments
(using C's `...` syntax) and the client decides how to format the log message. Supporting native variable arguments in
callbacks is next to impossible in C#, and the payoff is not worth spending a lot of effort to implement this feature.

Next article: [Migration Guide](migration.md).
