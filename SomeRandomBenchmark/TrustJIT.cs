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
    public class TrustJIT
    {
        Hook a, b, c;
        [GlobalSetup]
        public void Setup()
        {
            a = new(typeof(TrustJIT).GetMethod(nameof(_Instanced)),
                (Basetype orig, int a, int b, int c, int d, int e, int f, int g) =>
            {
                return orig(a, b, c, d, e, f, g);
            });
            b = new(typeof(TrustJIT).GetMethod(nameof(_Static)), target);
            static int target(Basetype orig, int a, int b, int c, int d, int e, int f, int g)
                => orig(a, b, c, d, e, f, g);
            c = new(typeof(TrustJIT).GetMethod(nameof(_Aggressive)),
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
            (Basetype orig, int a, int b, int c, int d, int e, int f, int g) =>
            {
                return orig(a, b, c, d, e, f, g);
            });
        }

        [Benchmark]
        public int Padding()
        {
            return 0;
        }
        [Benchmark]
        public int Padding2()
        {
            return 0;
        }
        [Benchmark]
        public int Instanced()
        {
            return _Instanced(1, 9, 1, 9, 8, 1, 0);
        }
        [Benchmark]
        public int Static()
        {
            return _Static(1, 9, 1, 9, 8, 1, 0);
        }
        [Benchmark]
        public int Aggressive()
        {
            return _Aggressive(1, 9, 1, 9, 8, 1, 0);
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _Instanced(int a, int b, int c, int d, int e, int f, int g)
        {
            return e;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _Static(int a, int b, int c, int d, int e, int f, int g)
        {
            return e;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _Aggressive(int a, int b, int c, int d, int e, int f, int g)
        {
            return e;
        }

        class For
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static int Method(int a, int b, int c, int d, int e, int f, int g)
            {
                return e;
            }
            public static For Instance = new();
        }
        class Simulator
        {
            public static Simulator Instance = new();
            [MethodImpl(MethodImplOptions.NoInlining)]
            public int NoInlining(int a, int b, int c, int d, int e, int f, int g)
            {
                return For.Method(a, b, c, d, e, f, g);
            }
            public static int Static(int a, int b, int c, int d, int e, int f, int g)
            {
                return For.Method(a, b, c, d, e, f, g);
            }
            public int Instanced(int a, int b, int c, int d, int e, int f, int g)
            {
                return For.Method(a, b, c, d, e, f, g);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Aggressive(int a, int b, int c, int d, int e, int f, int g)
            {
                return For.Method(a, b, c, d, e, f, g);
            }
        }
        [Benchmark]
        public int Simulate()
        {
            return Simulator.Instance.Instanced(1, 9, 1, 9, 8, 1, 0);
        }
        [Benchmark]
        public int SimulateAggressive()
        {
            return Simulator.Instance.Aggressive(1, 9, 1, 9, 8, 1, 0);
        }
        [Benchmark]
        public int SimulateStatic()
        {
            return Simulator.Static(1, 9, 1, 9, 8, 1, 0);
        }
        [Benchmark]
        public int SimulateNo()
        {
            return Simulator.Instance.NoInlining(1, 9, 1, 9, 8, 1, 0);
        }
    }
}
