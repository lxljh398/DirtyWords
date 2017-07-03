
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace KudyStudioFilteringTest
{
    /// <summary>
    /// The <see cref="T:CodeTimer"/> helps to time code testing in console for convenience.
    /// </summary>
    /// <remarks>code by Jeffrey Zhao</remarks>
    public static class CodeTimer
    {
        #region Utils

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentThread();

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

        private static ulong GetCycleCount()
        {
            ulong cycleCount = 0;
            QueryThreadCycleTime(GetCurrentThread(), ref cycleCount);
            return cycleCount;
        }

        #endregion

        /// <summary>
        /// Initializes the <see cref="T:CodeTimer"/>.
        /// </summary>
        public static void Initialize()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            Time(string.Empty, 1, () => { });
        }

        /// <summary>
        /// Times a testing.
        /// </summary>
        /// <param name="iteration">The iteration to run the specified action.</param>
        /// <param name="action">The action to run.</param>
        public static void Time(int iteration, Action action)
        {
            Time(string.Empty, iteration, i => action());
        }

        /// <summary>
        /// Times a testing.
        /// </summary>
        /// <param name="iteration">The iteration to run the specified action.</param>
        /// <param name="action">The action to run.</param>
        public static void Time(int iteration, Action<int> action)
        {
            Time(string.Empty, iteration, action);
        }

        /// <summary>
        /// Times a named testing.
        /// </summary>
        /// <param name="name">The name of the current testing.</param>
        /// <param name="iteration">The iteration to run the specified action.</param>
        /// <param name="action">The action to run.</param>
        public static void Time(string name, int iteration, Action action)
        {
            Time(name, iteration, i => action());
        }

        /// <summary>
        /// Times a named testing.
        /// </summary>
        /// <param name="name">The name of the current testing.</param>
        /// <param name="iteration">The iteration to run the specified action.</param>
        /// <param name="action">The action to run.</param>
        public static void Time(string name, int iteration, Action<int> action)
        {
            Time(name, false, iteration, action);
        }

        /// <summary>
        /// Times a named testing.
        /// </summary>
        /// <param name="name">The name of the current testing.</param>
        /// <param name="averageTime"></param>
        /// <param name="iteration">The iteration to run the specified action.</param>
        /// <param name="action">The action to run.</param>
        public static void Time(string name, bool averageTime, int iteration, Action<int> action)
        {
            if (name == null || iteration <= 0 || action == null)
                return;

            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(name);

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            int[] gcCounts = new int[GC.MaxGeneration + 1];
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                gcCounts[i] = GC.CollectionCount(i);
            }

            var watch = new Stopwatch();
            watch.Start();
            ulong cycleCount = GetCycleCount();
            for (int i = 0; i < iteration; i++)
            {
                action(i);
            }
            ulong cpuCycles = GetCycleCount() - cycleCount;
            watch.Stop();

            Console.ForegroundColor = currentForeColor;

            if (averageTime)
                Console.WriteLine("\tTime Elapsed:\t" + (watch.ElapsedMilliseconds / iteration).ToString("N0") + "ms");
            else
                Console.WriteLine("\tTime Elapsed:\t" + watch.ElapsedMilliseconds.ToString("N0") + "ms");

            Console.WriteLine("\tCPU Cycles:\t" + cpuCycles.ToString("N0"));

            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                int count = GC.CollectionCount(i) - gcCounts[i];
                Console.WriteLine("\tGen " + i + ": \t\t" + count);
            }

            Console.WriteLine();
        }
    }
}
