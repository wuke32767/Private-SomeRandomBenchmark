using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SomeRandomBenchmark
{
    public class Eliminated
    {
        [GlobalSetup]
        public void Setup()
        {
            MyImportA.ImportState = ImportState.NotImported;
            MyImportB.ImportState = ImportState.Failed;
            MyImportC.ImportState = ImportState.Ok;
            MyImportD.ImportState = ImportState.NotImported;
            Ignore(HelperD.ImportState);
            MyImportD.ImportState = ImportState.Ok;
            MyImportE.ImportState = ImportState.NotImported;
            gchandle = Task.Run(() =>
            {
                while (Console.ReadKey().KeyChar != '好') ;
                MyImportA.ImportState = (ImportState)Console.Read();
                MyImportB.ImportState = (ImportState)Console.Read();
                MyImportC.ImportState = (ImportState)Console.Read();
                MyImportD.ImportState = (ImportState)Console.Read();
                MyImportE.ImportState = (ImportState)Console.Read();
                Console.WriteLine("UNREACHABLE!");
                Debugger.Launch();
            });
        }
        public Task gchandle;
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Ignore<T>(T t)
        {
        }

        [Benchmark]
        public void _Padding()
        {
        }
        [Benchmark]
        public void RunNotImported()
        {
            MyImportA.ImportedMethod();
        }
        [Benchmark]
        public void RunFailed()
        {
            MyImportB.ImportedMethod();
        }
        [Benchmark]
        public void RunOk()
        {
            MyImportC.ImportedMethod();
        }
        [Benchmark]
        public void RunLateOk()
        {
            MyImportD.ImportedMethod();
        }
        [Benchmark]
        public void RunNotImported_ColdPath()
        {
            MyImportE.ImportedMethod();
        }
    }

    internal enum ImportState
    {
        /// <summary>
        ///   The import state is unknown, because <c>Load()</c> has not yet been invoked.
        /// </summary>
        NotImported,

        /// <summary>
        ///   The import completed successfully.
        /// </summary>
        Ok,

        /// <summary>
        ///   The import was unsuccessful, because the dependency is not present.
        /// </summary>
        /// <remarks>
        ///   Previously this state was used when none of the import methods were imported successfully.
        ///   However, this would not inherently mean that the dependency was not present - it is possible that
        ///   the dependency is present, but none of the import methods' names and signatures were compatible with
        ///   any of the available export methods.<br/>
        ///   As there is no way to know for sure at this time, this state is currently unused.
        /// </remarks>
        [Obsolete("This state is currently unused. See the remarks for more information. Use FailedImport instead.")]
        DependencyNotPresent,

        /// <summary>
        ///   The import was unsuccessful, either because the dependency was not present, or none of the
        ///   import methods' names and signatures were compatible with any of the available export methods.
        /// </summary>
        FailedImport,

        /// <summary>
        ///   The import was partially successful; one or more methods has not been imported.
        /// </summary>
        PartialImport,

        /// <summary>
        ///   The import was unsuccessful for an unknown reason.
        /// </summary>
        UnknownFailure,
        Failed,
    }
    public partial class MyImportA
    {
        internal static ImportState ImportState { get; set; }
        public static partial void ImportedMethod();
        public static partial void ImportedMethod()
        {
            if (HelperA.ImportState == ImportState.NotImported)
            {
                if (ImportState == ImportState.NotImported)
                {
                    DoError("b");
                    return;
                }
                if (ImportState == ImportState.Ok)
                {
                    DoThing();
                    return;
                }
                else if (ImportState == ImportState.Failed)
                {
                    DoError("a");
                    return;
                }
                DoError("c");
            }
            else
            {
                if (HelperA.ImportState == ImportState.Ok)
                {
                    DoThing();
                    return;
                }
                else if (HelperA.ImportState == ImportState.Failed)
                {
                    DoError("a");
                    return;
                }
                if (HelperA.ImportState == ImportState.NotImported)
                {
                    DoError("b");
                    return;
                }
                DoError("c");
            }
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DoThing()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DoError(string v)
        {
        }
    }
    static class HelperA
    {
        public static readonly ImportState ImportState = MyImportA.ImportState;
    }
    public partial class MyImportB
    {
        internal static ImportState ImportState { get; set; }
        public static partial void ImportedMethod();
        public static partial void ImportedMethod()
        {
            if (HelperB.ImportState == ImportState.NotImported)
            {
                if (ImportState == ImportState.NotImported)
                {
                    DoError("b");
                    return;
                }
                if (ImportState == ImportState.Ok)
                {
                    DoThing();
                    return;
                }
                else if (ImportState == ImportState.Failed)
                {
                    DoError("a");
                    return;
                }
                DoError("c");
            }
            else
            {
                if (HelperB.ImportState == ImportState.Ok)
                {
                    DoThing();
                    return;
                }
                else if (HelperB.ImportState == ImportState.Failed)
                {
                    DoError("a");
                    return;
                }
                if (HelperB.ImportState == ImportState.NotImported)
                {
                    DoError("b");
                    return;
                }
                DoError("c");
            }
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DoThing()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DoError(string v)
        {
        }
    }
    static class HelperB
    {
        public static readonly ImportState ImportState = MyImportB.ImportState;
    }
    public partial class MyImportC
    {
        internal static ImportState ImportState { get; set; }
        public static partial void ImportedMethod();
        public static partial void ImportedMethod()
        {
            if (HelperC.ImportState == ImportState.NotImported)
            {
                if (ImportState == ImportState.NotImported)
                {
                    DoError("b");
                    return;
                }
                if (ImportState == ImportState.Ok)
                {
                    DoThing();
                    return;
                }
                else if (ImportState == ImportState.Failed)
                {
                    DoError("a");
                    return;
                }
                DoError("c");
            }
            else
            {
                if (HelperC.ImportState == ImportState.Ok)
                {
                    DoThing();
                    return;
                }
                else if (HelperC.ImportState == ImportState.Failed)
                {
                    DoError("a");
                    return;
                }
                if (HelperC.ImportState == ImportState.NotImported)
                {
                    DoError("b");
                    return;
                }
                DoError("c");
            }
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DoThing()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DoError(string v)
        {
        }
    }
    static class HelperC
    {
        public static readonly ImportState ImportState = MyImportC.ImportState;
    }
    public partial class MyImportD
    {
        internal static ImportState ImportState { get; set; }
        public static partial void ImportedMethod();
        public static partial void ImportedMethod()
        {
            if (HelperD.ImportState == ImportState.NotImported)
            {
                if (ImportState == ImportState.NotImported)
                {
                    DoError("b");
                    return;
                }
                if (ImportState == ImportState.Ok)
                {
                    DoThing();
                    return;
                }
                else if (ImportState == ImportState.Failed)
                {
                    DoError("a");
                    return;
                }
                DoError("c");
            }
            else
            {
                if (HelperD.ImportState == ImportState.Ok)
                {
                    DoThing();
                    return;
                }
                else if (HelperD.ImportState == ImportState.Failed)
                {
                    DoError("a");
                    return;
                }
                if (HelperD.ImportState == ImportState.NotImported)
                {
                    DoError("b");
                    return;
                }
                DoError("c");
            }
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DoThing()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DoError(string v)
        {
        }
    }
    static class HelperD
    {
        public static readonly ImportState ImportState = MyImportD.ImportState;
    }
    public partial class MyImportE
    {
        internal static ImportState ImportState { get; set; }
        public static partial void ImportedMethod();
        public static partial void ImportedMethod()
        {
            if (HelperE.ImportState == ImportState.NotImported)
            {
                if (ImportState == ImportState.Ok)
                {
                    DoThing();
                    return;
                }
                else if (ImportState == ImportState.Failed)
                {
                    DoError("a");
                    return;
                }
                if (ImportState == ImportState.NotImported)
                {
                    DoError("b");
                    return;
                }
                DoError("c");
            }
            else
            {
                if (HelperE.ImportState == ImportState.Ok)
                {
                    DoThing();
                    return;
                }
                else if (HelperE.ImportState == ImportState.Failed)
                {
                    DoError("a");
                    return;
                }
                if (HelperE.ImportState == ImportState.NotImported)
                {
                    DoError("b");
                    return;
                }
                DoError("c");
            }
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DoThing()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DoError(string v)
        {
        }
    }
    static class HelperE
    {
        public static readonly ImportState ImportState = MyImportE.ImportState;
    }
}
