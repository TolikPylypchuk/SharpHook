# Library Status

SharpHook is under active development. The code itself is basically done at this point. Now I need to configure CI to
build it along with libuiohook itself, and publish the NuGet package to nuget.org when I release a new version.

Afterwards, I will maintain it to keep up with the releases of libuiohook. It uses a rolling release model - every
commit to its `1.2` branch is considered stable. That said, libuiohook itself is not really active anymore, but is still
maintained, so this library will most probably receive updates quite rarely.
