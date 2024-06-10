using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursWork
{
    public class LR
    {
        List<Token> tokens = new List<Token>();
        Stack<Token> lexemStack = new Stack<Token>();
        Stack<int> stateStack = new Stack<int>();
        private static List<KeyValuePair<string, int>> inputValues = new List<KeyValuePair<string, int>>();
        int nextLex = 0;
        int state = 0;
        bool isEnd = false;
        int count = 0;
        int i = 0;
        public LR(List<Token> inputtoken)
        {
            tokens = inputtoken;
        }
        private Token GetLexeme(int nextLex)
        {
            return tokens[nextLex];
        }

        private void Shift()
        {
            lexemStack.Push(GetLexeme(nextLex));
            nextLex++;
        }
        private void GoToState(int state)
        {
            stateStack.Push(state);
            this.state = state;
        }
        private void Reduce(int num, string value)
        {
            for (int i = 0; i < num; i++)
            {
                lexemStack.Pop();
                stateStack.Pop();
            }
            state = stateStack.Peek();
            Token k = new Token(TokenType.NETERMINAL);
            k.Value = value;
            lexemStack.Push(k);
        }
        private void ReduceEXPR(int num, string value)
        {
            for (int i = 0; i < num; i++)
            {
                lexemStack.Pop();

            }
            stateStack.Pop();
            stateStack.Pop();
            stateStack.Pop();
            state = stateStack.Peek();
            Token k = new Token(TokenType.NETERMINAL);
            k.Value = value;
            lexemStack.Push(k);
        }
        public void Programm()
        {
            Start();
        }


        void State0()
        {
            if (lexemStack.Count == 0)
                Shift();
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<программа>":
                            isEnd = true;
                            break;
                    }
                    break;
                case TokenType.VOID:
                    GoToState(1);
                    break;
                default:
                    throw new Exception($"Ожидалось void, но было получено {lexemStack.Peek().ToString()}. State: 0");
            }
        }
        void State1()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.VOID:
                    if (GetLexeme(nextLex).Type != TokenType.MAIN) { throw new Exception($"Ожидалось main, но было получено {GetLexeme(nextLex).ToString()}. State: 1"); }
                    else { Shift(); }
                    break;
                case TokenType.MAIN:
                    if (GetLexeme(nextLex).Type == TokenType.MAIN) { throw new Exception($"Ожидалось 1 main, но было получено несколько main. State: 1"); }
                    else { GoToState(2); }
                    
                    break;
                default:
                    throw new Exception($"Ожидалось main, но было получено {lexemStack.Peek().ToString()}. State: 1");
            }
        }
        void State2()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.MAIN:
                    Shift();
                    break;
                case TokenType.LPAR:
                    GoToState(3);
                    break;
                default:
                    throw new Exception($"Ожидалось (, но было получено {lexemStack.Peek().ToString()}. State: 2");
            }
        }
        void State3()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.LPAR:
                    Shift();
                    break;
                case TokenType.RPAR:
                    GoToState(4);
                    break;
                default:
                    throw new Exception($"Ожидалось (, но было получено {lexemStack.Peek().ToString()}. State: 3");
            }
        }
        void State4()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.RPAR:
                    Shift();
                    break;
                case TokenType.LBRACE:
                    GoToState(5);
                    break;
                default:
                    throw new Exception("Ожидалось {, но было получено " + $"{lexemStack.Peek().ToString()}. State: 4");
            }
        }
        void State5()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<инструкция>":
                            GoToState(6);
                            break;
                        case "<список_объявлений>":
                            GoToState(7);
                            break;
                        case "<объявление>":
                            GoToState(8);
                            break;
                        case "<тип>":
                            GoToState(9);
                            break;
                    }
                    break;
                case TokenType.LBRACE:
                    if (GetLexeme(nextLex).Type == TokenType.LBRACE) { throw new Exception($"Ожидались одни фигурные скобки, но было получено несколько фигурных скобок. State: 5"); }
                    else {  Shift(); }
                    break;
                case TokenType.INT:
                    GoToState(10);
                    break;
                case TokenType.BOOL:
                    GoToState(11);
                    break;
                case TokenType.BYTE:
                    GoToState(12);
                    break;
                default:
                    throw new Exception($"Ожидалось int, bool или byte, но было получено {lexemStack.Peek().ToString()}. State: 5");
            }
        }
        void State6()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<инструкция>":
                            Shift();
                            break;
                    }
                    break;
                case TokenType.RBRACE:
                    GoToState(13);
                    break;
                default:
                    throw new Exception("Ожидалось }, но было получено " + $"{lexemStack.Peek().ToString()}. State: 6");
            }
        }
        void State7()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<оператор>":
                            GoToState(15);
                            break;
                        case "<список_объявлений>":
                            Shift();
                            break;
                        case "<услов_опер>":
                            GoToState(16);
                            break;
                        case "<присваивание>":
                            GoToState(17);
                            break;
                        case "<список_операторов>":
                            GoToState(14);
                            break;
                    }
                    break;
                case TokenType.SWITCH:
                    GoToState(18);
                    break;
                case TokenType.ID:
                    GoToState(19);
                    break;
                default:
                    throw new Exception($"Ожидалось switch или id, но было получено {lexemStack.Peek().ToString()}. State: 7");
            }
        }
        void State8()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<объявление>":

                            if (GetLexeme(nextLex).Type == TokenType.INT || GetLexeme(nextLex).Type == TokenType.BOOL || GetLexeme(nextLex).Type == TokenType.BYTE)
                            {
                                Shift();
                            }
                            else
                            {
                                Reduce(1, "<список_объявлений>");
                                break;
                            }
                            break;
                        case "<список_объявлений>":
                            GoToState(20);
                            break;
                        case "<тип>":
                            GoToState(9);
                            break;
                    }
                    break;
                case TokenType.INT:
                    GoToState(10);
                    break;
                case TokenType.BOOL:
                    GoToState(11);
                    break;
                case TokenType.BYTE:
                    GoToState(12);
                    break;
                default:
                    throw new Exception($"Ожидалось int, bool или byte, но было получено {lexemStack.Peek().ToString()}. State: 8");
            }
        }
        void State9()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список_переменных>":
                            GoToState(21);
                            break;
                        case "<тип>":
                            Shift();
                            break;
                    }
                    break;
                case TokenType.ID:
                    GoToState(22);
                    break;
                default:
                    throw new Exception($"Ожидалось id, но было получено {lexemStack.Peek().ToString()}. State: 9");
            }
        }
        void State10()
        {
            if (lexemStack.Peek().Type == TokenType.INT)
                Reduce(1, "<тип>");
            else
                throw new Exception($"Ожидалось int, но было получено {lexemStack.Peek().ToString()}. State: 10");
        }
        void State11()
        {
            if (lexemStack.Peek().Type == TokenType.BOOL)
                Reduce(1, "<тип>");
            else
                throw new Exception($"Ожидалось bool, но было получено {lexemStack.Peek().ToString()}. State: 11");
        }
        void State12()
        {
            if (lexemStack.Peek().Type == TokenType.BYTE)
                Reduce(1, "<тип>");
            else
                throw new Exception($"Ожидалось byte, но было получено {lexemStack.Peek().ToString()}. State: 12");
        }
        void State13()
        {
            if (lexemStack.Peek().Type == TokenType.RBRACE) {Reduce(7, "<программа>"); }
                
            else
                throw new Exception($"Ожидались фигурные скобки, но было получено {lexemStack.Peek().ToString()}. State: 13");
        }
        void State14()
        {
            if (lexemStack.Peek().Type == TokenType.NETERMINAL && lexemStack.Peek().Value == "<список_операторов>")
                Reduce(2, "<инструкция>");
            else
                throw new Exception($"Ожидалось правило <список_операторов>, но было получено {lexemStack.Peek().ToString()}. State: 14");
        }
        void State15()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<оператор>":

                            if (GetLexeme(nextLex).Type == TokenType.SWITCH || GetLexeme(nextLex).Type == TokenType.ID)
                            {
                                Shift();
                            }
                            else
                            {
                                Reduce(1, "<список_операторов>");
                                break;
                            }
                            break;
                        case "<список_операторов>":
                            GoToState(23);
                            break;
                        case "<услов_опер>":
                            GoToState(16);
                            break;
                        case "<присваивание>":
                            GoToState(17);
                            break;
                    }
                    break;
                case TokenType.SWITCH:
                    GoToState(18);
                    break;
                case TokenType.ID:
                    GoToState(19);
                    break;
                default:
                    throw new Exception($"Ожидалось switch или id, но было получено {lexemStack.Peek().ToString()}. State: 15");
            }
        }
        void State16()
        {
            if (lexemStack.Peek().Type == TokenType.NETERMINAL && lexemStack.Peek().Value == "<услов_опер>")
                Reduce(1, "<оператор>");
            else
                throw new Exception($"Ожидалось правило <услов_опер>, но было получено {lexemStack.Peek().ToString()}. State: 16");
        }
        void State17()
        {
            if (lexemStack.Peek().Type == TokenType.NETERMINAL && lexemStack.Peek().Value == "<присваивание>")
                Reduce(1, "<оператор>");
            else
                throw new Exception($"Ожидалось правило <присваивание>, но было получено {lexemStack.Peek().ToString()}. State: 17");
        }
        void State18()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.SWITCH:
                    Shift();
                    break;
                case TokenType.LPAR:
                    GoToState(24);
                    break;
                default:
                    throw new Exception($"Ожидалось (, но было получено {lexemStack.Peek().ToString()}. State: 18");
            }
        }
        void State19()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.ID:
                    if (GetLexeme(nextLex).Type == TokenType.ID) { throw new Exception($"Ожидалось =, но было получено {GetLexeme(nextLex).ToString()}. State: 19"); }
                    else { Shift(); }
                    break;
                case TokenType.EQUAL:
                    GoToState(25);
                    break;
                default:
                    throw new Exception($"Ожидалось =, но было получено {lexemStack.Peek().ToString()}. State: 19");
            }
        }
        void State20()
        {
            if (lexemStack.Peek().Type == TokenType.NETERMINAL && lexemStack.Peek().Value == "<список_объявлений>")
                Reduce(2, "<список_объявлений>");
            else
                throw new Exception($"Ожидалось правило <список_объявлений>, но было получено {lexemStack.Peek().ToString()}. State: 20");
        }
        void State21()
        {
            if (lexemStack.Peek().Type == TokenType.NETERMINAL && lexemStack.Peek().Value == "<список_переменных>")
                Reduce(2, "<объявление>");
            else
                throw new Exception($"Ожидалось правило <список_переменных>, но было получено {lexemStack.Peek().ToString()}. State: 21");
        }
        void State22()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.ID:
                    if (GetLexeme(nextLex).Type == TokenType.ID) { throw new Exception($"Ожидалась запятая, но было получено {GetLexeme(nextLex).ToString()}. State: 22"); }
                    else { Shift(); }
                    break;
                case TokenType.SEMICOLON:
                    GoToState(26);
                    break;
                case TokenType.COMMA:
                    GoToState(27);
                    break;
                default:
                    throw new Exception($"Ожидалось запятая или точка с запятой, но было получено {lexemStack.Peek().ToString()}. State: 22");
            }
        }
        void State23()
        {
            if (lexemStack.Peek().Type == TokenType.NETERMINAL && lexemStack.Peek().Value == "<список_операторов>")
                Reduce(2, "<список_операторов>");
            else
                throw new Exception($"Ожидалось правило <список_операторов>, но было получено {lexemStack.Peek().ToString()}. State: 23");
        }
        void State24()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.LPAR:
                    Shift();
                    break;
                case TokenType.ID:
                    GoToState(28);
                    break;
                default:
                    throw new Exception($"Ожидалось id, но было получено {lexemStack.Peek().ToString()}. State: 24");
            }
        }
        void State25()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.EQUAL:
                    Shift();
                    break;
                case TokenType.LPAR:
                    while (lexemStack.Peek().Type != TokenType.SEMICOLON) { count++; Shift(); if (lexemStack.Peek().Type == TokenType.CASE || lexemStack.Peek().Type == TokenType.RBRACE) { throw new Exception($"Ожидалось ;, но было получено {lexemStack.Peek().ToString()}. State: 25"); } }
                    GoToState(29);
                    break;
                case TokenType.ID:
                    while (lexemStack.Peek().Type != TokenType.SEMICOLON) { count++; Shift(); if (lexemStack.Peek().Type == TokenType.CASE || lexemStack.Peek().Type == TokenType.RBRACE) { throw new Exception($"Ожидалось ;, но было получено {lexemStack.Peek().ToString()}. State: 25"); } }
                    GoToState(29);
                    break;
                case TokenType.LITERAL:
                    while (lexemStack.Peek().Type != TokenType.SEMICOLON) { count++; Shift(); if (lexemStack.Peek().Type == TokenType.CASE || lexemStack.Peek().Type == TokenType.RBRACE) { throw new Exception($"Ожидалось ;, но было получено {lexemStack.Peek().ToString()}. State: 25"); } }
                    GoToState(29);
                    break;
                case TokenType.RPAR:
                    while (lexemStack.Peek().Type != TokenType.SEMICOLON) { count++; Shift(); if (lexemStack.Peek().Type == TokenType.CASE || lexemStack.Peek().Type == TokenType.RBRACE) { throw new Exception($"Ожидалось ;, но было получено {lexemStack.Peek().ToString()}. State: 25"); } }
                    GoToState(29);
                    break;
                case TokenType.PLUS:
                    while (lexemStack.Peek().Type != TokenType.SEMICOLON) { count++; Shift(); if (lexemStack.Peek().Type == TokenType.CASE || lexemStack.Peek().Type == TokenType.RBRACE) { throw new Exception($"Ожидалось ;, но было получено {lexemStack.Peek().ToString()}. State: 25"); } }
                    GoToState(29);
                    break;
                case TokenType.MINUS:
                    while (lexemStack.Peek().Type != TokenType.SEMICOLON) { count++; Shift(); if (lexemStack.Peek().Type == TokenType.CASE || lexemStack.Peek().Type == TokenType.RBRACE) { throw new Exception($"Ожидалось ;, но было получено {lexemStack.Peek().ToString()}. State: 25"); } }
                    GoToState(29);
                    break;
                case TokenType.MULTIPLICATION:
                    while (lexemStack.Peek().Type != TokenType.SEMICOLON) { count++; Shift(); if (lexemStack.Peek().Type == TokenType.CASE || lexemStack.Peek().Type == TokenType.RBRACE) { throw new Exception($"Ожидалось ;, но было получено {lexemStack.Peek().ToString()}. State: 25"); } }
                    GoToState(29);
                    break;
                case TokenType.DIV:
                    while (lexemStack.Peek().Type != TokenType.SEMICOLON) { count++; Shift(); if (lexemStack.Peek().Type == TokenType.CASE || lexemStack.Peek().Type == TokenType.RBRACE) { throw new Exception($"Ожидалось ;, но было получено {lexemStack.Peek().ToString()}. State: 25"); } }
                    GoToState(29);
                    break;
                default:
                    throw new Exception($"Ожидалось expr, но было получено {lexemStack.Peek().ToString()}. State: 25");
            }
        }
        void State26()
        {
            if (lexemStack.Peek().Type == TokenType.SEMICOLON)
                Reduce(2, "<список_переменных>");
            else
                throw new Exception($"Ожидалось точка с запятой, но было получено {lexemStack.Peek().ToString()}. State: 26");
        }
        void State27()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список_переменных>":
                            GoToState(30);
                            break;
                    }
                    break;
                case TokenType.ID:
                    GoToState(22);
                    break;
                case TokenType.COMMA:
                    Shift();
                    break;
                default:
                    throw new Exception($"Ожидалось id, но было получено {lexemStack.Peek().ToString()}. State: 27");
            }
        }
        void State28()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.RPAR:
                    GoToState(31);
                    break;
                case TokenType.ID:
                    if (GetLexeme(nextLex).Type == TokenType.ID) { throw new Exception($"Ожидалось ), но было получено {GetLexeme(nextLex).ToString()}. State: 28"); }
                    else { Shift(); }
                    break;
                default:
                    throw new Exception($"Ожидалось ), но было получено {lexemStack.Peek().ToString()}. State: 28");
            }
        }
        void State29()
        {
            if (lexemStack.Peek().Type == TokenType.SEMICOLON)
            {
                ReduceEXPR(3 + count, "<присваивание>");
                count = 0;
            }
            else
            {
                throw new Exception($"Ожидалось expr, но было получено {lexemStack.Peek().ToString()}. State: 29");
            }

        }
        void State30()
        {
            if (lexemStack.Peek().Type == TokenType.NETERMINAL && lexemStack.Peek().Value == "<список_переменных>")
                Reduce(3, "<список_переменных>");
            else
                throw new Exception($"Ожидалось правило <список_переменных>, но было получено {lexemStack.Peek().ToString()}. State: 30");
        }
        void State31()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.RPAR:
                    Shift();
                    break;
                case TokenType.LBRACE:
                    GoToState(32);
                    break;
                default:
                    throw new Exception("Ожидалось {, но было получено " + $"{lexemStack.Peek().ToString()}. State: 31");
            }
        }
        void State32()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список_case>":
                            GoToState(33);
                            break;
                    }
                    break;
                case TokenType.LBRACE:
                    if (GetLexeme(nextLex).Type == TokenType.LBRACE) { throw new Exception($"Ожидались одни фигурные скобки, но было получено несколько фигурных скобок. State: 32"); }
                    else { Shift(); }
                    break;
                case TokenType.CASE:
                    GoToState(34);
                    break;
                default:
                    throw new Exception($"Ожидалось case, но было получено {lexemStack.Peek().ToString()}. State: 32");
            }
        }
        void State33()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список_case>":
                            Shift();
                            break;
                    }
                    break;
                case TokenType.RBRACE:
                    GoToState(35);
                    break;
                default:
                    throw new Exception("Ожидалось }, но было получено " + $"{lexemStack.Peek().ToString()}. State: 33");
            }
        }
        void State34()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.CASE:
                    Shift();
                    break;
                case TokenType.LITERAL:
                    GoToState(36);
                    break;
                default:
                    throw new Exception($"Ожидалось lit, но было получено {lexemStack.Peek().ToString()}. State: 34");
            }
        }
        void State35()
        {
            i++;
            if (lexemStack.Peek().Type == TokenType.RBRACE) {Reduce(7, "<услов_опер>"); }
            else
                throw new Exception("Ожидалось }, но было получено " + $"{lexemStack.Peek().ToString()}. State: 35");
        }
        void State36()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.COLON:
                    GoToState(37);
                    break;
                case TokenType.LITERAL:
                    if (GetLexeme(nextLex).Type != TokenType.COLON) { throw new Exception($"Ожидалось двоеточие, но было получено {GetLexeme(nextLex).ToString()}. State: 36"); }
                    else { Shift(); }
                    break;
                default:
                    throw new Exception($"Ожидалось :, но было получено {lexemStack.Peek().ToString()}. State: 36");
            }
        }
        void State37()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список_операторов>":
                            GoToState(38);
                            break;
                        case "<оператор>":
                            GoToState(15);
                            break;
                        case "<услов_опер>":
                            GoToState(16);
                            break;
                        case "<присваивание>":
                            GoToState(17);
                            break;
                    }
                    break;
                case TokenType.COLON:
                    Shift();
                    break;
                case TokenType.SWITCH:
                    GoToState(18);
                    break;
                case TokenType.ID:
                    GoToState(19);
                    break;
                default:
                    throw new Exception($"Ожидалось id или switch, но было получено {lexemStack.Peek().ToString()}. State: 37");
            }
        }
        void State38()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERMINAL:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<список_операторов>":

                            if (GetLexeme(nextLex).Type == TokenType.CASE)
                            {
                                Shift();
                            }
                            else
                            {
                                Reduce(4, "<список_case>");
                                break;
                            }
                            break;
                        case "<список_case>":
                            GoToState(39);
                            break;
                    }
                    break;
                case TokenType.CASE:
                    GoToState(34);
                    break;
                default:
                    throw new Exception($"Ожидалось case, но было получено {lexemStack.Peek().ToString()}. State: 38");
            }
        }
        void State39()
        {
            if (lexemStack.Peek().Type == TokenType.NETERMINAL && lexemStack.Peek().Value == "<список_case>")
                Reduce(5, "<список_case>");
            else
                throw new Exception($"Ожидалось правило <список_case>, но было получено {lexemStack.Peek().ToString()}. State: 39");
        }
        public void Start()
        {
            stateStack.Push(0);
            while (isEnd != true)
            {
                switch (state)
                {
                    case 0:
                        State0();
                        break;
                    case 1:
                        State1();
                        break;
                    case 2:
                        State2();
                        break;
                    case 3:
                        State3();
                        break;
                    case 4:
                        State4();
                        break;
                    case 5:
                        State5();
                        break;
                    case 6:
                        State6();
                        break;
                    case 7:
                        State7();
                        break;
                    case 8:
                        State8();
                        break;
                    case 9:
                        State9();
                        break;
                    case 10:
                        State10();
                        break;
                    case 11:
                        State11();
                        break;
                    case 12:
                        State12();
                        break;
                    case 13:
                        State13();
                        break;
                    case 14:
                        State14();
                        break;
                    case 15:
                        State15();
                        break;
                    case 16:
                        State16();
                        break;
                    case 17:
                        State17();
                        break;
                    case 18:
                        State18();
                        break;
                    case 19:
                        State19();
                        break;
                    case 20:
                        State20();
                        break;
                    case 21:
                        State21();
                        break;
                    case 22:
                        State22();
                        break;
                    case 23:
                        State23();
                        break;
                    case 24:
                        State24();
                        break;
                    case 25:
                        State25();
                        break;
                    case 26:
                        State26();
                        break;
                    case 27:
                        State27();
                        break;
                    case 28:
                        State28();
                        break;
                    case 29:
                        State29();
                        break;
                    case 30:
                        State30();
                        break;
                    case 31:
                        State31();
                        break;
                    case 32:
                        State32();
                        break;
                    case 33:
                        State33();
                        break;
                    case 34:
                        State34();
                        break;
                    case 35:
                        State35();
                        break;
                    case 36:
                        State36();
                        break;
                    case 37:
                        State37();
                        break;
                    case 38:
                        State38();
                        break;
                    case 39:
                        State39();
                        break;
                }
            }
        }

    }
}
