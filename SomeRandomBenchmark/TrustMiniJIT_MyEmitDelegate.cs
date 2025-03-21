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
    public class TrustMiniJIT_MyEmitDelegate
    {
        [Benchmark]
        public int Padding()
        {
            return 0;
        }

        ILHook a, b, c, d, e, f;
        static Func<int, int> wtf = Wtf.Instance.wtf;
        sealed class Wtf
        {
            public static Wtf Instance = new();
            public int wtf(int a)
            {
                return a * 3 + 2;
            }
            public static int wtfs(int a)
            {
                return a * 3 + 2;
            }
        }
        [GlobalSetup]
        public void Setup()
        {
            ILContext.Manipulator manip(Action<ILCursor> u) => i =>
            {
                ILCursor ic = new(i);
                ic.GotoNext(MoveType.Before, i => i.MatchLdcI4(114514));
                ic.Remove();
                ic.EmitLdarg0();
                u(ic);
            };
            f = new(typeof(TrustMiniJIT_MyEmitDelegate).GetMethod(nameof(_StandardStatic)), i =>
            {
            });
            a = new(typeof(TrustMiniJIT_MyEmitDelegate).GetMethod(nameof(_Standard)), i =>
            {
                ILCursor ic = new(i);
                ic.EmitReference(wtf);
                ic.EmitPop();
            });
            b = new(typeof(TrustMiniJIT_MyEmitDelegate).GetMethod(nameof(_Standard2)), i =>
            {
                ILCursor ic = new(i);
                ic.EmitReference(wtf);
                ic.EmitPop();
            });
            c = new(typeof(TrustMiniJIT_MyEmitDelegate).GetMethod(nameof(_Official)),
                manip(ic => ic.EmitDelegate(wtf)));
            d = new(typeof(TrustMiniJIT_MyEmitDelegate).GetMethod(nameof(_Mine)),
                manip(ic =>
                {
                    var item = new VariableDefinition(ic.Context.Import(typeof(int)));
                    ic.Body.Variables.Add(item);
                    ic.EmitStloc(item);
                    ic.EmitReference(wtf.Target);
                    ic.EmitLdloc(item);
                    ic.EmitCallvirt(wtf.Method);
                }));
            e = new(typeof(TrustMiniJIT_MyEmitDelegate).GetMethod(nameof(_MineStatic)),
                manip(ic =>
                {
                    var item = new VariableDefinition(ic.Context.Import(typeof(int)));
                    ic.Body.Variables.Add(item);
                    ic.EmitStloc(item);
                    ic.EmitLdnull();
                    ic.EmitLdloc(item);
                    ic.EmitCall(wtf.Method);
                }));
        }

        [Benchmark]
        public int Standard()
        {
            return _Standard(1919810);
        }
        [Benchmark]
        public int StandardStatic()
        {
            return _StandardStatic(1919810);
        }
        [Benchmark]
        public int Standard2()
        {
            return _Standard2(1919810);
        }
        [Benchmark]
        public int Official()
        {
            return _Official(1919810);
        }
        [Benchmark]
        public int Mine()
        {
            return _Mine(1919810);
        }
        [Benchmark]
        public int MineStatic()
        {
            return _MineStatic(1919810);
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _Standard(int a)
        {
            var ax = 5 + a;
            var dx = 3 & a;
            var bx = 7 * a;
            var cx = dx % 25;
            var fx = wtf(a) - ax;
            var ex = 6 ^ a;
            var gx = ax + dx + ex;
            return ax * bx - cx % dx == gx - ex - bx ? fx + cx * dx : ax + gx - 4;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _StandardStatic(int a)
        {
            var ax = 5 + a;
            var dx = 3 & a;
            var bx = 7 * a;
            var cx = dx % 25;
            var fx = Wtf.wtfs(a) - ax;
            var ex = 6 ^ a;
            var gx = ax + dx + ex;
            return ax * bx - cx % dx == gx - ex - bx ? fx + cx * dx : ax + gx - 4;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _Standard2(int a)
        {
            var ax = 5 + a;
            var dx = 3 & a;
            var bx = 7 * a;
            var cx = dx % 25;
            var fx = Wtf.Instance.wtf(a) - ax;
            var ex = 6 ^ a;
            var gx = ax + dx + ex;
            return ax * bx - cx % dx == gx - ex - bx ? fx + cx * dx : ax + gx - 4;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _Official(int a)
        {
            var ax = 5 + a;
            var dx = 3 & a;
            var bx = 7 * a;
            var cx = dx % 25;
            var fx = 114514 - ax;
            var ex = 6 ^ a;
            var gx = ax + dx + ex;
            return ax * bx - cx % dx == gx - ex - bx ? fx + cx * dx : ax + gx - 4;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _Mine(int a)
        {
            var ax = 5 + a;
            var dx = 3 & a;
            var bx = 7 * a;
            var cx = dx % 25;
            var fx = 114514 - ax;
            var ex = 6 ^ a;
            var gx = ax + dx + ex;
            return ax * bx - cx % dx == gx - ex - bx ? fx + cx * dx : ax + gx - 4;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _MineStatic(int a)
        {
            var ax = 5 + a;
            var dx = 3 & a;
            var bx = 7 * a;
            var cx = dx % 25;
            var fx = 114514 - ax;
            var ex = 6 ^ a;
            var gx = ax + dx + ex;
            return ax * bx - cx % dx == gx - ex - bx ? fx + cx * dx : ax + gx - 4;
        }
    }
}
