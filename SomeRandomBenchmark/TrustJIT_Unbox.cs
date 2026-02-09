using BenchmarkDotNet.Attributes;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SomeRandomBenchmark
{
    public class TrustJIT_Unbox
    {
        [GlobalSetup]
        public void Setup()
        {
            str = new(10);
        }
        RandomStruct str;
        public interface RandomInterface
        {
            public int RandomMethod();
        }
        public struct RandomStruct(int v) : RandomInterface
        {
            public int RandomMethod()
            {
                return v;
            }
        }
        [Benchmark]
        public int Padding()
        {
            return 0;
        }
        [Benchmark]
        public int Unbox()
        {
            return str.RandomMethod();
        }
        [Benchmark]
        public int Box()
        {
            RandomInterface i = str;
            return i.RandomMethod();
        }
    }
}
