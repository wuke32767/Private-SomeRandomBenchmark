using SomeRandomBenchmark;
using BenchmarkDotNet.Running;
using MiscTests;

#pragma warning disable CS0162
if (false)
{
    BenchmarkRunner.Run<MonoModBenchmark>();
    BenchmarkRunner.Run<MonoModBenchmark2>();
    BenchmarkRunner.Run<MonoModEmitReference>();
    BenchmarkRunner.Run<GetTypeBenchmark>();
    BenchmarkRunner.Run<TrustJIT>();
    BenchmarkRunner.Run<TrustMiniJIT>();
    BenchmarkRunner.Run<TrustJIT_MyEmitDelegate>();
    BenchmarkRunner.Run<TrustMiniJIT_MyEmitDelegate>();
    BenchmarkRunner.Run<Operators>();
    BenchmarkRunner.Run<TrustJIT_Unbox>();
}
if (true)
{
    BenchmarkRunner.Run<Hitbox>();
}