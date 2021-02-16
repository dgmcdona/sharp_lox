using System;
using System.Collections.Generic;
using static lox.TokenType;

namespace lox
{
    public class Scanner
    {
        private readonly string source;
        private readonly List<Token> tokens = new List<Token>();
        private int start = 0;
        private int current = 0;
        private int line = 1;

        public Scanner(string source)
        {
            this.source = source;
        }

        public List<Token> ScanTokens()
        {
            while (!IsAtEnd()) {
                start = current;
                ScanToken();
            }

            tokens.Add(new Token(EOF, "", null, line));
            return tokens;
        }

        private Boolean IsAtEnd()
        {
            return current >= source.Length; 
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c) {
                case '(': AddToken(LEFT_PAREN); break;
                case ')': AddToken(RIGHT_PAREN); break;
                case '{': AddToken(LEFT_BRACE); break;
                case '}': AddToken(RIGHT_BRACE); break;
                case '.': AddToken(DOT); break;
                case ',': AddToken(COMMA); break;
                case '+': AddToken(PLUS); break;
                case '-': AddToken(MINUS); break;
                case ';': AddToken(SEMICOLON); break;
                case '*': AddToken(STAR); break;
                // Check for two-character lexemes using Match('=')
                case '!': AddToken(Match('=') ? BANG_EQUAL : BANG); break;
                case '=': AddToken(Match('=') ? EQUAL_EQUAL : EQUAL); break;
                case '<': AddToken(Match('=') ? LESS_EQUAL : LESS); break;
                case '>': AddToken(Match('=') ? GREATER_EQUAL : GREATER); break;
                case '/': 
                    if (Match('/')) {
                        // A comment goes until the end of the line.
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    } else {
                        AddToken(SLASH);
                    }
                    break;

                // Skip whitespace and newlines (increment line for newlines)
                case ' ':
                case '\r':
                case '\t':
                    break;
                case '\n':
                    line++;
                    break;
                case '"': String(); break;
                default: 
                    if (Char.IsDigit(c)) {
                        Number();
                    } else { 
                        Lox.Error(line, "Unexpected character."); 
                    }
                    break;
            }
        }

        private void Number()
        {
            while (Char.IsDigit(Peek())) _ = Advance();

            // Look for a fractional part.
            if (Peek() == '.' && Char.IsDigit(PeekNext())) {
                // Consume the '.'
                _ = Advance();

                while (Char.IsDigit(Peek())) _ = Advance();
            }

            AddToken(NUMBER,
                Double.Parse(source.Substring(start, current)));
        }

        private void String()
        { 
            while (Peek() != '"' && !IsAtEnd()) {
                if (Peek() == '\n') line++;
                Advance();
            }

            if (IsAtEnd()) {
                Lox.Error(line, "Unterminated string.");
                return;
            }

            _ = Advance();

            string value = source.Substring(start + 1, source.Length - current - 1);
            AddToken(STRING, value);
        }

        private Boolean Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (source[current] != expected) return false;

            // Consume if current character matches expected.
            current++;
            return true;
        }

        /* Implement lookahead (like Advance, but don't consume the char) */
        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return source[current];
        }

        private char PeekNext() {
            if (current + 1 >= source.Length) return '\0';
            return source[current + 1];
        }

        private char Advance()
        {
            current++;
            return source[current - 1];
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, Object literal)
        {
            string text = source.Substring(start, current - start);
            tokens.Add(new Token(type, text, literal, line));
        }

    }

}
