using SomeRandomBenchmark;
using BenchmarkDotNet.Running;

#pragma warning disable CS0162
if (false)
{
    BenchmarkRunner.Run<MonoModBenchmark>();
    BenchmarkRunner.Run<MonoModBenchmark2>();
    BenchmarkRunner.Run<MonoModEmitReference>();
}
if (true)
{
    BenchmarkRunner.Run<GetTypeBenchmark>();
    BenchmarkRunner.Run<TrustJIT>();
    BenchmarkRunner.Run<TrustMiniJIT>();
}