using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Shared
{
    public static class StopwatchExtensions
    {
        public static Stopwatch CreateStartSW()
        {
            var sw = new Stopwatch();
            sw.Start();
            return sw;
        }

        public static string LogTimeToMessage(this Stopwatch sw, string message)
        {
            sw.Stop();
            return $"{message} [{sw.ElapsedMilliseconds} ms]";
        }

        public static async Task LogTimeToConsoleAsync(this Stopwatch sw, string message)
        {
            await Console.Out.WriteLineAsync(LogTimeToMessage(sw, message));
        }

        public static void LogTimeToConsole(this Stopwatch sw, string message)
        {
            Console.WriteLine(LogTimeToMessage(sw, message));
        }
    }
}
