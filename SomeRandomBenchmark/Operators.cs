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
    public class Operators
    {
        [GlobalSetup]
        public void Setup()
        {
            DynamicMethod dmd = new("", typeof(bool), [typeof(Operators)]);
            var il = dmd.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, typeof(Operators).GetField(nameof(tester)));
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, typeof(Operators).GetMethod(nameof(expensive)));
            il.Emit(OpCodes.Or);
            il.Emit(OpCodes.Ret);
            bitwise = dmd.CreateDelegate<Func<bool>>(this);

            or = () => tester || expensive();

            dict = [];
            for (int i = 0; i < 10000; i++)
            {
                dict.Add(i.ToString() + "eswtgyhujiedrcxftg", 114);
            }
        }
        [Params([true, false])]
        public bool tester;
        Dictionary<string, int> dict;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool expensive()
        {
            return dict.TryGetValue("""ghyjcfvrtyh""", out _);
        }
        [Benchmark]
        public int Padding()
        {
            return 0;
        }
        [Benchmark]
        public bool Bitwise()
        {
            return bitwise();
        }
        Func<bool> bitwise;
        [Benchmark]
        public bool Or()
        {
            return or();
        }
        Func<bool> or;
    }
}
