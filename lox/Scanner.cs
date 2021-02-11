using System;
using System.Collections.Generic;

namespace lox
{
    public class Scanner
    {
        private string source;
        public Scanner(string source)
        {
            this.source = source;
        }

        public List<Token> scanTokens() {
            List<Token> tokens = new List<Token>();
            return tokens;
        }

    }
}
