﻿using SomeRandomBenchmark;
using BenchmarkDotNet.Running;

#pragma warning disable CS0162
if (false)
{
    BenchmarkRunner.Run<MonoModBenchmark>();
    BenchmarkRunner.Run<MonoModBenchmark2>();
    BenchmarkRunner.Run<MonoModEmitReference>();
    BenchmarkRunner.Run<GetTypeBenchmark>();
}
if (true)
{
    BenchmarkRunner.Run<TrustJIT>();
    BenchmarkRunner.Run<TrustMiniJIT>();
}