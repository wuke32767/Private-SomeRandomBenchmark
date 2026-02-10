using BenchmarkDotNet.Attributes;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SomeRandomBenchmark
{
    public class MonoModBenchmark50
    {
        List<Hook> a = [];
        List<ILHook> b = [];
        void ApplyOn(MethodInfo m)
        {
            static int target(Func<int, int, int, int, int, int, int, int> orig,
                int a, int b, int c, int d, int e, int f, int g)
            {
                return orig(a, b, c, d, e, f, g);
            }
            a.Add(new(m,
                 target
                ));
        }
        void ApplyIL(MethodInfo m)
        {
            b.Add(new(m, il => { }));
        }
        [GlobalSetup]
        public void Setup()
        {
            var a = typeof(MonoModBenchmark50).GetMethod(nameof(_OnHook1));
            var b = typeof(MonoModBenchmark50).GetMethod(nameof(_OnHook50));
            var c = typeof(MonoModBenchmark50).GetMethod(nameof(_ILHook1));
            var d = typeof(MonoModBenchmark50).GetMethod(nameof(_ILHook50));
            for (int i = 0; i < 50; i++)
            {
                ApplyOn(b);
                ApplyIL(d);
            }
            ApplyOn(a);
            ApplyIL(c);
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
        public int OnHook1()
        {
            return _OnHook1(1, 9, 1, 9, 8, 1, 0);
        }
        [Benchmark]
        public int OnHook50()
        {
            return _OnHook50(1, 9, 1, 9, 8, 1, 0);
        }
        [Benchmark]
        public int ILHook1()
        {
            return _ILHook1(1, 9, 1, 9, 8, 1, 0);
        }
        [Benchmark]
        public int ILHook50()
        {
            return _ILHook50(1, 9, 1, 9, 8, 1, 0);
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
        public static int _OnHook1(int a, int b, int c, int d, int e, int f, int g)
        {
            return e;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _ILHook1(int a, int b, int c, int d, int e, int f, int g)
        {
            return e;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _OnHook50(int a, int b, int c, int d, int e, int f, int g)
        {
            return e;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _ILHook50(int a, int b, int c, int d, int e, int f, int g)
        {
            return e;
        }
    }
}
