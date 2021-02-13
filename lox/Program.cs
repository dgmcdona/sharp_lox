using System;
using System.IO;
using System.Collections.Generic;

namespace lox
{
    class Lox
    {
        static bool hadError;

        public static void Main(string[] args)
        {
            if (args.Length > 1) {
                Console.WriteLine("Usage: lox [script]");
                Environment.Exit(64);
            } else if (args.Length == 1) {
                RunFile(args[0]);
            } else {
                RunPrompt();
            }
        }

        /* Wrap the Run() method in a REPL */
        public static void RunPrompt() 
        {
            BufferedStream buffered = 
                new BufferedStream(Console.OpenStandardInput());
            using (StreamReader input = new StreamReader(buffered)) {
                for (; ; ) {
                    Console.Write("> ");
                    string line = input.ReadLine();
                    // Ctrl-D will return null, stopping the interpreter.
                    if (line == null) break;
                    Run(line);
                }
            }

        }

        /* Read an execute a Lox source code file */
        public static void RunFile(string path) 
        {
            byte[] bytes = File.ReadAllBytes(Path.GetFullPath(path));
            Run(System.Text.Encoding.UTF8.GetString(bytes));
            // Exit if Error Reported in source code
            if (hadError) Environment.Exit(65);
        }

        /* Run Lox code */
        public static void Run(string source)
        {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.ScanTokens();
            foreach (Token token in tokens) {
                Console.WriteLine(token);
            }
        }

        public static void Error(int line, string message)
        {
            Report(line, "", message);
        }

        private static void Report(int line, string where, string message) 
        {
            Console.Error.WriteLine(
                "[line " + line + "] Error" + where + ": " + message);
            hadError = true;
        }
    }
}
