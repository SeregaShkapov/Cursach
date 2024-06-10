using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursWork
{
    public class BauerZamelzon
    {
        List<Token> tokens;
        int index = 0;
        int Ecount = 0;
        int count = 0;
        Stack<Token> temp = new Stack<Token>();
        Stack<Token> E = new Stack<Token>();
        Stack<Token> T = new Stack<Token>();
        string str = "";
        bool start = true;

        public BauerZamelzon(List<Token> tokens)
        {
            this.tokens = tokens;
            while (index < tokens.Count - 1)
            {
                index++;
                if (tokens[index].Type == TokenType.EQUAL)
                {
                    index++;
                    Expression();
                    start = true;
                    str += Matrix() + "\r\n";
                    count = 0;
                    Ecount = 0;
                }
            }
        }
        void Expression()
        {
            while (start)
            {
                switch (tokens[index].Type)
                {
                    case TokenType.ID:
                        E.Push(tokens[index++]);
                        Ecount++;
                        break;
                    case TokenType.LITERAL:
                        E.Push(tokens[index++]);
                        Ecount++;
                        break;

                    case TokenType.SEMICOLON:
                        if (T.Count == 0)
                        {
                            start = false;
                        }
                        else if (T.Peek().Type == TokenType.LPAR)
                        {
                            throw new Exception("Ошибка в операции выражения!");
                        }
                        else if (T.Peek().Type == TokenType.PLUS || T.Peek().Type == TokenType.MINUS
                            || T.Peek().Type == TokenType.MULTIPLICATION || T.Peek().Type == TokenType.DIV)
                        {
                            temp.Push(T.Pop());
                            K();
                        }
                        else
                        {
                            throw new Exception("Ошибка в выражении!");
                        }
                        break;
                    case TokenType.LPAR:
                        if (T.Count == 0 || T.Peek().Type == TokenType.PLUS || T.Peek().Type == TokenType.MINUS || T.Peek().Type == TokenType.MULTIPLICATION || T.Peek().Type == TokenType.DIV || T.Peek().Type == TokenType.LPAR)
                        {
                            T.Push(tokens[index++]);
                        }
                        else
                        {
                            throw new Exception("Ошибка в операции выражения!");
                        }
                        break;
                    case TokenType.RPAR:
                        if (T.Count == 0)
                        {
                            throw new Exception("Ошибка в операции выражения!");
                        }
                        else if (T.Peek().Type == TokenType.LPAR)
                        {
                            T.Pop();
                            index++;
                        }
                        else if (T.Peek().Type == TokenType.PLUS || T.Peek().Type == TokenType.MINUS
                            || T.Peek().Type == TokenType.MULTIPLICATION || T.Peek().Type == TokenType.DIV)
                        {
                            temp.Push(T.Pop());
                            K();
                        }
                        else
                        {
                            throw new Exception("Ошибка в выражении!");
                        }
                        break;
                    default:
                        if (tokens[index].Type == TokenType.PLUS || tokens[index].Type == TokenType.MINUS)
                        {
                            if (T.Count == 0 || T.Peek().Type == TokenType.LPAR)
                            {
                                T.Push(tokens[index++]);
                            }
                            else if (T.Peek().Type == TokenType.PLUS || T.Peek().Type == TokenType.MINUS)
                            {
                                temp.Push(T.Pop());
                                K();
                                T.Push(tokens[index++]);
                            }
                            else if (T.Peek().Type == TokenType.MULTIPLICATION || T.Peek().Type == TokenType.DIV)
                            {
                                temp.Push(T.Pop());
                                K();
                            }
                            else
                            {
                                throw new Exception("Ошибка в операции выражения!");
                            }
                        }
                        else if (tokens[index].Type == TokenType.MULTIPLICATION
                        || tokens[index].Type == TokenType.DIV)
                        {
                            if (T.Count == 0 || T.Peek().Type == TokenType.PLUS
                            || T.Peek().Type == TokenType.MINUS || T.Peek().Type == TokenType.LPAR)
                            {
                                T.Push(tokens[index++]);
                            }
                            else if (T.Peek().Type == TokenType.MULTIPLICATION || T.Peek().Type == TokenType.DIV)
                            {
                                temp.Push(T.Pop());
                                K();
                                T.Push(tokens[index++]);
                            }
                            else
                            {
                                throw new Exception("Ошибка в операции выражения!");
                            }
                        }
                        else
                        {
                            throw new Exception("Ошибка в выражении!");
                        }
                        break;
                }
            }
        }
        void K()
        {
            if (E.Count <= 1)
            {
                throw new Exception("Отсутствует операнд!");
            }
            temp.Push(E.Pop());
            temp.Push(E.Pop());
            while (temp.Count > 0)
            {
                E.Push(temp.Pop());
            }
        }
        public string MatrixShow()
        {
            return str;
        }
        string Matrix()
        {
            while (E.Count > 0)
            {
                temp.Push(E.Pop());
                if (temp.Peek().Type != TokenType.ID && temp.Peek().Type != TokenType.LITERAL) { count++; }
            }
            if (Ecount - count != 1)
            {
                throw new Exception("Отсутствует операнд!");
            }
            string str = "";
            string str2 = "";
            string[] s = new string[temp.Count];
            Token token;
            int i = 1, j = 0;
            while (temp.Count > 0)
            {
                token = temp.Pop();
                if (token.Type == TokenType.PLUS || token.Type == TokenType.MINUS || token.Type == TokenType.MULTIPLICATION
                    || token.Type == TokenType.DIV)
                {
                    if (j == 1)
                    {
                        throw new Exception("Отсутствует операнд!");
                    }
                    if (token.Type == TokenType.PLUS)
                    {
                        str2 = "M" + i.ToString() + ": " + "+" + s[j - 2] + s[j - 1];
                    }
                    else if (token.Type == TokenType.MINUS)
                    {
                        str2 = "M" + i.ToString() + ": " + "-" + s[j - 2] + s[j - 1];
                    }
                    else if (token.Type == TokenType.MULTIPLICATION)
                    {
                        str2 = "M" + i.ToString() + ": " + "*" + s[j - 2] + s[j - 1];
                    }
                    else if (token.Type == TokenType.DIV)
                    {
                        str2 = "M" + i.ToString() + ": " + "/" + s[j - 2] + s[j - 1];
                    }
                    s[j - 2] = "M" + i++.ToString();
                    s[j - 1] = null;
                    j--;
                    str += str2 + "\r\n";
                }
                else
                { s[j++] = token.Value; }
            }
            return str;
        }
    }
}
