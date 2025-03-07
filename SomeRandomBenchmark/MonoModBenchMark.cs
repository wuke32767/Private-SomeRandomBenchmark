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
    public class MonoModBenchmark
    {
        Hook a;
        ILHook b;
        [GlobalSetup]
        public void Setup()
        {
            a = new(typeof(MonoModBenchmark).GetMethod(nameof(_Hook)), (//Func<int, int, int, int, int, int, int, int> orig,
                int a, int b, int c, int d, int e, int f, int g) =>
            {
                return e;
            });
            b = new ILHook(typeof(MonoModBenchmark).GetMethod(nameof(_ILHook)), il => { });
            using (new ILHook(typeof(MonoModBenchmark).GetMethod(nameof(_Hooked)), il => { }))
            {
                using (new Hook(typeof(MonoModBenchmark).GetMethod(nameof(_Hooked)), (//Func<int, int, int, int, int, int, int, int> orig,
                            int a, int b, int c, int d, int e, int f, int g) =>
                {
                    return e;
                }))
                {
                    _Hooked(0, 0, 0, 0, 0, 0, 0);
                }
            }
        }

        [Benchmark]
        public int Inlining()
        {
            return _Inlining(1, 9, 1, 9, 8, 1, 0);
        }
        [Benchmark]
        public int NoInlining()
        {
            return _NoInlining(1, 9, 1, 9, 8, 1, 0);
        }
        [Benchmark]
        public int Hook0()
        {
            return _Hook(1, 9, 1, 9, 8, 1, 0);
        }
        [Benchmark]
        public int ILHook()
        {
            return _ILHook(1, 9, 1, 9, 8, 1, 0);
        }
        [Benchmark]
        public int Hooked()
        {
            return _Hooked(1, 9, 1, 9, 8, 1, 0);
        }
        public static int _Inlining(int a, int b, int c, int d, int e, int f, int g)
        {
            return e;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _NoInlining(int a, int b, int c, int d, int e, int f, int g)
        {
            return e;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _Hook(int a, int b, int c, int d, int e, int f, int g)
        {
            return e;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _ILHook(int a, int b, int c, int d, int e, int f, int g)
        {
            return e;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _Hooked(int a, int b, int c, int d, int e, int f, int g)
        {
            return e;
        }

    }
    public class MonoModBenchmark2
    {
        Hook a, b, c, d;
        ILHook e;
        [GlobalSetup]
        public void Setup()
        {

            static int target(Basetype orig,
                int a, int b, int c, int d, int e, int f, int g) => orig(a, b, c, d, e, f, g);

            a = new(typeof(MonoModBenchmark2).GetMethod(nameof(_Hook1)), target);
            b = new(typeof(MonoModBenchmark2).GetMethod(nameof(_Hook2)), target);
            c = new(typeof(MonoModBenchmark2).GetMethod(nameof(_Hook2)), target);
            d = new(typeof(MonoModBenchmark2).GetMethod(nameof(_Hook11)), target);
            e = new(typeof(MonoModBenchmark2).GetMethod(nameof(_Hook11)), i => { });


            hook1d = (a, b, c, d, e, f, g) => hook1(a, b, c, d, e, f, g);
        }

        Basetype hook1 = _Hook1;
        Basetype hook1d;
        [Benchmark]
        public int Hook1d()
        {
            return hook1d(1, 9, 1, 9, 8, 1, 0);
        }
        [Benchmark]
        public int Hook1()
        {
            return hook1(1, 9, 1, 9, 8, 1, 0);
        }
        [Benchmark]
        public int Hook11()
        {
            return _Hook11(1, 9, 1, 9, 8, 1, 0);
        }

        [Benchmark]
        public int Hook2()
        {
            return _Hook2(1, 9, 1, 9, 8, 1, 0);
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _Hook1(int a, int b, int c, int d, int e, int f, int g)
        {
            return e;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _Hook11(int a, int b, int c, int d, int e, int f, int g)
        {
            return e;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _Hook2(int a, int b, int c, int d, int e, int f, int g)
        {
            return e;
        }
    }
}
