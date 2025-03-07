using SomeRandomBenchmark;
using BenchmarkDotNet.Running;

#pragma warning disable CS0162
if (false)
{
    BenchmarkRunner.Run<MonoModBenchmark>();
    BenchmarkRunner.Run<MonoModBenchmark2>();
}
else
{
    BenchmarkRunner.Run<MonoModEmitReference>();
}