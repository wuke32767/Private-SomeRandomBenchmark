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
    public class TrustMiniJIT
    {
        Hook a, b, c;
        [GlobalSetup]
        public void Setup()
        {
            a = new(typeof(TrustMiniJIT).GetMethod(nameof(_Instanced)),
                (Func<int, int> orig, int e) =>
            {
                return orig(e);
            });
            b = new(typeof(TrustMiniJIT).GetMethod(nameof(_Static)), target);
            static int target(Func<int, int> orig, int e)
                => orig(e);
            c = new(typeof(TrustMiniJIT).GetMethod(nameof(_Aggressive)),
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
            (Func<int, int> orig, int e) =>
            {
                return orig(e);
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
            return _Instanced(114514);
        }
        [Benchmark]
        public int Static()
        {
            return _Static(114514);
        }
        [Benchmark]
        public int Aggressive()
        {
            return _Aggressive(114514);
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _Instanced(int e)
        {
            return e;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _Static(int e)
        {
            return e;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _Aggressive(int e)
        {
            return e;
        }

        class For
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            public static int Method(int e)
            {
                return e;
            }
            public static For Instance = new();
        }
        class Simulator
        {
            public static Simulator Instance = new();
            [MethodImpl(MethodImplOptions.NoInlining)]
            public int NoInlining(int e)
            {
                return For.Method(e);
            }
            public static int Static(int e)
            {
                return For.Method(e);
            }
            public int Instanced(int e)
            {
                return For.Method(e);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Aggressive(int e)
            {
                return For.Method(e);
            }
        }
        [Benchmark]
        public int Simulate()
        {
            return Simulator.Instance.Instanced(114514);
        }
        [Benchmark]
        public int SimulateAggressive()
        {
            return Simulator.Instance.Aggressive(114514);
        }
        [Benchmark]
        public int SimulateStatic()
        {
            return Simulator.Static(114514);
        }
        [Benchmark]
        public int SimulateNo()
        {
            return Simulator.Instance.NoInlining(114514);
        }
    }
}
