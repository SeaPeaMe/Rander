using System;
using System.Diagnostics;

namespace Rander
{
    public static class Debug
    {

        public static bool ShowButtonBounds = false;

        public static void Log(string Text)
        {
            Console.WriteLine(Text);
        }

        public static void LogWarning(string Text, bool MakeBold = false)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (MakeBold)
            {
                Console.WriteLine("\n-----! WARNING !-----");
                Console.WriteLine(Text);
                Console.WriteLine("---------------------");
            }
            else
            {
                Console.WriteLine(Text);
            }
            Console.ResetColor();
        }

        public static void LogSuccess(string Text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(Text);
            Console.ResetColor();
        }

        public static void Clear()
        {
            Console.Clear();
        }

        public static void LogError(string Text, bool CloseWindow = false, int traceBackAmount = 2)
        {
            StackFrame frame = new StackFrame(traceBackAmount, true);
            string MethodName = frame.GetMethod().Name;
            string ScriptName = frame.GetFileName();
            int Line = frame.GetFileLineNumber();

            // Writes to the console
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n-----Error Report-----");
            Console.WriteLine(Text + "\n");
            Console.WriteLine("Method: " + MethodName);
            Console.WriteLine("Line: " + Line);
            Console.WriteLine("Script Path: " + ScriptName);
            Console.WriteLine("----------------------");
            Console.ResetColor();

            if (CloseWindow == true)
            {
                // Closes the game window and keeps the console open
                Game.Close();
                if (Console.ReadKey() != null)
                {
                    // Waits for the console window to close, then terminates the program
                }
            }
        }
    }
}