// See https://aka.ms/new-console-template for more information
using SharpHook;

using SharpHookCore.Sample;

Console.WriteLine("Hello, World!");

var test = new TestHook();
await test.StartTestHook();

while(true)
{
    Console.WriteLine(".");
    Thread.Sleep(1000);
}
