using BenchmarkDotNet.Attributes;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SomeRandomBenchmark
{
    public class TrustJIT_MyEmitDelegate
    {
        [Benchmark]
        public int Padding()
        {
            return 0;
        }

        ILHook a, b, c, d, e, f;
        static Basetype wtf = Wtf.Instance.wtf;
        sealed class Wtf
        {
            public static Wtf Instance = new();
            public int wtf(int a, int b, int c, int d, int e, int f, int g)
            {
                return a + b + c + d + e + f + g;
            }

            public static int wtfs(int a, int b, int c, int d, int e, int f, int g)
            {
                return a + b + c + d + e + f + g;
            }
        }
        [GlobalSetup]
        public void Setup()
        {
            ILContext.Manipulator manip(Action<ILCursor> u) => i =>
            {
                ILCursor ic = new(i);
                ic.GotoNext(MoveType.After, i => i.MatchLdarg(4));
                ic.GotoNext(MoveType.Before, i => i.MatchLdarg(4));
                ic.Remove();
                for (var x = 0; x != 7; x++)
                {
                    ic.EmitLdarg(x);
                }
                u(ic);
            };
            f = new(typeof(TrustJIT_MyEmitDelegate).GetMethod(nameof(_StandardStatic)), i =>
            {
            });
            a = new(typeof(TrustJIT_MyEmitDelegate).GetMethod(nameof(_Standard)), i =>
            {
                ILCursor ic = new(i);
                ic.EmitReference(wtf);
                ic.EmitPop();
            });
            b = new(typeof(TrustJIT_MyEmitDelegate).GetMethod(nameof(_Standard2)), i =>
            {
                ILCursor ic = new(i);
                ic.EmitReference(wtf);
                ic.EmitPop();
            });
            c = new(typeof(TrustJIT_MyEmitDelegate).GetMethod(nameof(_Official)),
                manip(ic => ic.EmitDelegate(wtf)));
            d = new(typeof(TrustJIT_MyEmitDelegate).GetMethod(nameof(_Mine)),
                manip(ic =>
                {
                    var l = Enumerable.Range(0, 7)
                        .Select(x => new VariableDefinition(ic.Context.Import(typeof(int))))
                        .ToList();
                    foreach (var item in l)
                    {
                        ic.Body.Variables.Add(item);
                        ic.EmitStloc(item);
                    }
                    ic.EmitReference(wtf.Target);
                    foreach (var item in l)
                    {
                        ic.EmitLdloc(item);
                    }
                    ic.EmitCallvirt(wtf.Method);
                }));
            e = new(typeof(TrustJIT_MyEmitDelegate).GetMethod(nameof(_MineStatic)),
                manip(ic =>
                {
                    var l = Enumerable.Range(0, 7)
                        .Select(x => new VariableDefinition(ic.Context.Import(typeof(int))))
                        .ToList();
                    foreach (var item in l)
                    {
                        ic.Body.Variables.Add(item);
                        ic.EmitStloc(item);
                    }
                    ic.EmitLdnull();
                    foreach (var item in l)
                    {
                        ic.EmitLdloc(item);
                    }
                    ic.EmitCall(wtf.Method);
                }));
        }

        [Benchmark]
        public int Standard()
        {
            return _Standard(1, 9, 1, 9, 8, 1, 0);
        }
        [Benchmark]
        public int StandardStatic()
        {
            return _StandardStatic(1, 9, 1, 9, 8, 1, 0);
        }
        [Benchmark]
        public int Standard2()
        {
            return _Standard2(1, 9, 1, 9, 8, 1, 0);
        }
        [Benchmark]
        public int Official()
        {
            return _Official(1, 9, 1, 9, 8, 1, 0);
        }
        [Benchmark]
        public int Mine()
        {
            return _Mine(1, 9, 1, 9, 8, 1, 0);
        }
        [Benchmark]
        public int MineStatic()
        {
            return _MineStatic(1, 9, 1, 9, 8, 1, 0);
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _Standard(int a, int b, int c, int d, int e, int f, int g)
        {
            var ax = 5 * a;
            var bx = b + c;
            var cx = d - e;
            var dx = f * g;
            var ex = a + b + c;
            var fx = d - wtf(a, b, c, d, e, f, g) + f;
            var gx = g * a - b;
            return ax + bx - cx * dx == ex - fx + gx ? ax * bx + cx : dx - ex + fx;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _StandardStatic(int a, int b, int c, int d, int e, int f, int g)
        {
            var ax = 5 * a;
            var bx = b + c;
            var cx = d - e;
            var dx = f * g;
            var ex = a + b + c;
            var fx = d - Wtf.wtfs(a, b, c, d, e, f, g) + f;
            var gx = g * a - b;
            return ax + bx - cx * dx == ex - fx + gx ? ax * bx + cx : dx - ex + fx;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _Standard2(int a, int b, int c, int d, int e, int f, int g)
        {
            var ax = 5 * a;
            var bx = b + c;
            var cx = d - e;
            var dx = f * g;
            var ex = a + b + c;
            var fx = d - Wtf.Instance.wtf(a, b, c, d, e, f, g) + f;
            var gx = g * a - b;
            return ax + bx - cx * dx == ex - fx + gx ? ax * bx + cx : dx - ex + fx;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _Official(int a, int b, int c, int d, int e, int f, int g)
        {
            var ax = 5 * a;
            var bx = b + c;
            var cx = d - e;
            var dx = f * g;
            var ex = a + b + c;
            var fx = d - e + f;
            var gx = g * a - b;
            return ax + bx - cx * dx == ex - fx + gx ? ax * bx + cx : dx - ex + fx;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _Mine(int a, int b, int c, int d, int e, int f, int g)
        {
            var ax = 5 * a;
            var bx = b + c;
            var cx = d - e;
            var dx = f * g;
            var ex = a + b + c;
            var fx = d - e + f;
            var gx = g * a - b;
            return ax + bx - cx * dx == ex - fx + gx ? ax * bx + cx : dx - ex + fx;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _MineStatic(int a, int b, int c, int d, int e, int f, int g)
        {
            var ax = 5 * a;
            var bx = b + c;
            var cx = d - e;
            var dx = f * g;
            var ex = a + b + c;
            var fx = d - e + f;
            var gx = g * a - b;
            return ax + bx - cx * dx == ex - fx + gx ? ax * bx + cx : dx - ex + fx;
        }
    }
}
