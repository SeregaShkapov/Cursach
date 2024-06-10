using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursWork
{
    public class Tab
    {
        public List<Token> tokens = new List<Token>();
        Token token;
        public List<Token> Tokens { get { return tokens; } }
        public Tab(string[,] buff)
        {
            Add(buff);
        }
        void Add(string[,] buff)
        {
            int i = 0;
            while (buff[0, i] != null)
            {
                if (buff[1, i] == "литерал")
                {
                    token = new Token(TokenType.LITERAL);
                    token.Value = buff[0, i];
                    tokens.Add(token);
                    i++;
                }
                else if (buff[1, i] == "разделитель")
                {
                    token = new Token(Checking.SpecialSymbols[buff[0, i]]);
                    tokens.Add(token);
                    i++;
                }
                else if (buff[1, i] == "идентификатор")
                {
                    if (Checking.IsSpecialWord(buff[0, i]))
                    {
                        token = new Token(Checking.KeyWords[buff[0, i]]);
                        tokens.Add(token);
                        i++;
                    }
                    else
                    {
                        token = new Token(TokenType.ID);
                        token.Value = buff[0, i];
                        tokens.Add(token);
                        i++;
                    }
                }
            }
            return;
        }
        public string Info()
        {
            string info = "";
            for (int i = 0; i < tokens.Count; i++)
            {
                info += tokens[i].ToString() + '\r' + '\n';
            }
            return info;
        }
    }
}
