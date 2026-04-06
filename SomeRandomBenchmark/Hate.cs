using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SomeRandomBenchmark
{
    public class Hate
    {
        class A;
        class B : A;
        class C : A;
        class D_ : A;
        class D : D_;
        class E : A;

        [GlobalSetup]
        public void Setup()
        {
            complist = [new B(), new C(), new D()];
        }
        List<A> complist = [];
        [MethodImpl(MethodImplOptions.NoInlining)]
        T? GetObj<T>()
        {
            return complist.OfType<T>().FirstOrDefault();
        }
        T? GetObjFine<T>()
        {
            foreach (var comp in complist)
            {
                if (comp is T u)
                {
                    return u;
                }
            }
            return default;
        }
        [Benchmark]
        public int Padding()
        {
            return 0;
        }
        [Benchmark]
        public object Dx()
        {
            return GetObj<D>();
        }
        [Benchmark]
        public object Ex()
        {
            return GetObj<E>();
        }
        [Benchmark]
        public object Dy()
        {
            return GetObjFine<D>();
        }
        [Benchmark]
        public object Ey()
        {
            return GetObjFine<E>();
        }
        [Benchmark]
        public object Dz()
        {
            return complist[2];
        }
    }
}
