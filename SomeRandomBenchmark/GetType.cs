using BenchmarkDotNet.Attributes;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeRandomBenchmark
{
    public class GetTypeBenchmark
    {
        [GlobalSetup]
        public void Setup()
        {
            // i dont know if jit can inline the gettype()
            // and i dont know if this can work
            object[] os = [new List<int>(), 1145, new GetTypeBenchmark(), Random.Shared];
            o = os[Random.Shared.Next(os.Length)];
        }
        public object o;
        [Benchmark]
        public new Type GetType()
        {
            return o.GetType();
        }
        [Benchmark]
        public new Type GetTypeLiteral()
        {
            return typeof(GetTypeBenchmark);
        }
    }
}
