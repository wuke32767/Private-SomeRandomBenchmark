using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SomeRandomBenchmark
{

    public class Depth
    {
        class A;
        class B : A;
        class C : B;
        class D : C;
        class E : D;
        [GlobalSetup]
        public void Setup()
        {
        }
        A a = new D();
        A b = new E();
        [MethodImpl(MethodImplOptions.NoInlining)]
        static int GetDepth(Type t)
        {
            int i = 0;
            while (t.BaseType is not null)
            {
                i++;
                t = t.BaseType;
            }
            return i;
        }
        [Benchmark]
        public int Padding()
        {
            return 0;
        }
        [Benchmark]
        public int D4()
        {
            return GetDepth(a.GetType());
        }
        [Benchmark]
        public int D5()
        {
            return GetDepth(b.GetType());
        }
    }
}
