using System;

/*
 * Centralizes all debug output.
 * Eventually this can be configured with a setting so users can generate debug logs
 * Which are output to a text box (or file?) that can be used for debugging.
 */

namespace MacroCoach
{
    class DebugOutput
    {
        private static bool ENABLE_DEBUG = true;

        public static void WriteLine(string line)
        {
            if (ENABLE_DEBUG) {
                System.Console.WriteLine(line);

            }
        }
    }
}
