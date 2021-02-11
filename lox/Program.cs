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
                runFile(args[0]);
            } else {
                runPrompt();
            }
        }

        /* Wrap the run() method in a REPL */
        public static void runPrompt() {
            BufferedStream buffered = 
                new BufferedStream(Console.OpenStandardInput());
            using (StreamReader input = new StreamReader(buffered)) {
                for (; ; ) {
                    Console.Write("> ");
                    string line = input.ReadLine();
                    // Ctrl-D will return null, stopping the interpreter.
                    if (line == null) break;
                    run(line);
                }
            }

        }

        /* Read an execute a Lox source code file */
        public static void runFile(string path) {
            byte[] bytes = File.ReadAllBytes(Path.GetFullPath(path));
            run(System.Text.Encoding.UTF8.GetString(bytes));
            // Exit if error reported in source code
            if (hadError) Environment.Exit(65);
        }

        /* Run Lox code */
        public static void run(string source) {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.scanTokens();
            foreach (Token token in tokens) {
                Console.WriteLine(token);
            }
        }

        static void error(int line, string message) {
            report(line, "", message);
        }

        private static void report(int line, string where, string message) {
            Console.Error.WriteLine(
                "[line " + line + "] Error" + where + ": " + message);
            hadError = true;
        }
    }
}
