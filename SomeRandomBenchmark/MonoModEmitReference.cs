using BenchmarkDotNet.Attributes;
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
    public class MonoModEmitReference
    {
        ILHook a, b, c, d;
        [GlobalSetup]
        public void Setup()
        {
            a = new(typeof(MonoModEmitReference).GetMethod(nameof(_Empty)), i => { });
            d = new(typeof(MonoModEmitReference).GetMethod(nameof(_EmptyO)), i => { });
            b = new(typeof(MonoModEmitReference).GetMethod(nameof(_EmitHook)), i =>
            {
                ILCursor ic = new(i);
                ic.EmitReference(1);
                ic.EmitRet();
            });
            c = new(typeof(MonoModEmitReference).GetMethod(nameof(_EmitObjHook)), i =>
            {
                ILCursor ic = new(i);
                ic.EmitReference<int[]>([1]);
                ic.EmitRet();
            });
        }

        Basetype empty = _Empty;
        Objtype emptyo = _EmptyO;
        Basetype emitint = _EmitHook;
        Objtype emitobj = _EmitObjHook;
        [Benchmark]
        public int Standard()
        {
            return empty(1, 9, 1, 9, 8, 1, 0);
        }
        [Benchmark]
        public int[] StandardO()
        {
            return emptyo(1, 9, 1, 9, 8, 1, 0);
        }

        [Benchmark]
        public int EmitIntReference()
        {
            return emitint(1, 9, 1, 9, 8, 1, 0);
        }
        [Benchmark]
        public int[] EmitObjReference()
        {
            return emitobj(1, 9, 1, 9, 8, 1, 0);
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _Empty(int a, int b, int c, int d, int e, int f, int g)
        {
            return e;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int[] _EmptyO(int a, int b, int c, int d, int e, int f, int g)
        {
            return [e];
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int _EmitHook(int a, int b, int c, int d, int e, int f, int g)
        {
            return e;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int[] _EmitObjHook(int a, int b, int c, int d, int e, int f, int g)
        {
            return [e];
        }
    }
}
