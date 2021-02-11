using System;
using System.IO;

namespace lox
{
    class Lox
    {
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
            BufferedStream buffered = new BufferedStream(Console.OpenStandardInput());
            using (StreamReader input = new StreamReader(buffered)) {
                for (; ; ) {
                    Console.Write("> ");
                    string line = input.ReadLine();
                    if (line == null) break;
                    run(line);
                }
            }

        }

        /* Read an execute a Lox source code file */
        public static void runFile(string path) {
            byte[] bytes = File.ReadAllBytes(Path.GetFullPath(path));
            run(System.Text.Encoding.UTF8.GetString(bytes));
        }

        /* Run Lox code */
        public static void run(string code) { 
        }
    }
}
