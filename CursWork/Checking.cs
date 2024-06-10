using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursWork
{
    class Checking
    {
        public static Dictionary<string, TokenType> KeyWords = new Dictionary<string, TokenType>()
        {
            { "int", TokenType.INT },
            { "void", TokenType.VOID },
            { "bool", TokenType.BOOL },
            { "byte", TokenType.BYTE },
            { "main", TokenType.MAIN },
            { "case", TokenType.CASE },
            { "switch", TokenType.SWITCH }
        };
        public static bool IsSpecialWord(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return false;
            }
            return (KeyWords.ContainsKey(word));
        }
        public static Dictionary<string, TokenType> SpecialSymbols = new Dictionary<string, TokenType>()
        {
            { "=", TokenType.EQUAL },
            { "{", TokenType.LBRACE },
            { "}", TokenType.RBRACE },
            { ")", TokenType.RPAR },
            { "(", TokenType.LPAR },
            { "+", TokenType.PLUS },
            { "-", TokenType.MINUS },
            { "*", TokenType.MULTIPLICATION },
            { "/", TokenType.DIV },
            //{ @"\n", TokenType.NEWSTRING },
            { ",", TokenType.COMMA },
            { ":", TokenType.COLON },
            { ";", TokenType.SEMICOLON }
        };
        public static bool IsSpecialSymbol(string ch)
        {
            return SpecialSymbols.ContainsKey(ch);
        }
    }
}
