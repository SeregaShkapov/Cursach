using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursWork
{
    public enum TokenType
    {
        VOID, INT, BOOL, BYTE, MAIN, CASE, SWITCH, LITERAL, LPAR, RPAR, PLUS,
        MINUS, EQUAL,  MULTIPLICATION, DIV, ID, COMMA, COLON, SEMICOLON, LBRACE, RBRACE, NETERMINAL, END
    }
    public class Token
    {
        public TokenType Type;
        public string Value;
        public Token(TokenType type)
        {
            Type = type;
        }
        public override string ToString()
        {
            return string.Format("{1}, {0}", Type, Value);
        }
    }
}
