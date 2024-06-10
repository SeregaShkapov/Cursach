using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static CursWork.Token;

namespace CursWork
{
    class LexemAnalysis
    {
        string[,] buff = new string[2, 255];
        string alf = @"=<>(){}+-*/,:;";
        int j = 0;
        string error = "Недопустимый символ";
        public string[,] Buff { get { return buff; } }

        bool Chek()
        {
            for (int i = 0; i <= j; i++)
            {
                bool resi = int.TryParse(buff[0, i], out _);
                bool resd = double.TryParse(buff[0, i], out _);
                if (buff[0, i] == null) { break; }
                if (buff[0, i].Length > 8) return false;
                else if (resi || resd) buff[1, i] = "литерал";
                else if (alf.Contains(buff[0, i]) || buff[0, i] == @"\n") buff[1, i] = "разделитель";
                else if (Regex.IsMatch(buff[0, i], @"^[a-z1-9]+$") && Regex.IsMatch(buff[0, i][0].ToString(), @"^[a-z]+$")) buff[1, i] = "идентификатор";
                else return false;
            }
            return true;
        }
        bool Add(string str)
        {
            string ch = "";
            for (int i = 0; str.Length > i; i++)
            {
                if (str[i] == ' ' || str[i] == '\r')
                {

                    AddElem(ch);
                    ch = "";
                    i++;
                }
                else if (alf.Contains(str[i]))
                {
                    AddElem(ch);
                    AddElem(str[i].ToString());
                    ch = "";
                    i++;

                }
                else if (alf.Contains(ch) && ch != "")
                {
                    AddElem(ch);
                    ch = "";
                }
                if (str[i] == '\n') { i++; if (i >= str.Length) break; }
                if (str[i] == '\t') { i++; if (i >= str.Length) break; }
                ch += str[i];
                if (ch == " " || ch == "\r" || ch == "\t") { ch = ""; }
            }
            AddElem(ch);
            return Chek();
        }
        void AddElem(string v)
        {
            if (v == "")
            {
                return;
            }
            buff[0, j] += v;
            j++;
            return;
        }
        public string Info(string str)
        {
            str = str.ToLower();
            if (!Add(str)) { buff = null; throw new Exception(string.Format(error)); }
            string info = "";
            for (int i = 0; buff[1, i] != null; i++)
            {
                info += buff[0, i] + ", " + buff[1, i] + "\r" + "\n";
            }
            return info;
        }
    }
}
