using SomeRandomBenchmark;
using BenchmarkDotNet.Running;
using MiscTests;

#pragma warning disable CS0162
if (false)
{
    BenchmarkRunner.Run<MonoModEmitReference>();
    BenchmarkRunner.Run<GetTypeBenchmark>();
    BenchmarkRunner.Run<TrustJIT>();
    BenchmarkRunner.Run<TrustMiniJIT>();
    BenchmarkRunner.Run<TrustJIT_MyEmitDelegate>();
    BenchmarkRunner.Run<TrustMiniJIT_MyEmitDelegate>();
    BenchmarkRunner.Run<Operators>();
    BenchmarkRunner.Run<TrustJIT_Unbox>();
    BenchmarkRunner.Run<Hitbox>();
    BenchmarkRunner.Run<Eliminated>();
}
if (true)
{
    BenchmarkRunner.Run<MonoModBenchmark50>();
}