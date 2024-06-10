using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CursWork
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                    {
                        textBox1.Text = sr.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
            textBox2.Clear();
            try
            {
                if (textBox1.Text == "") { throw new Exception("Введите или загрузите код"); }
                LexemAnalysis betaTab = new LexemAnalysis();
                richTextBox1.Text = betaTab.Info(textBox1.Text);
                Tab tab = new Tab(betaTab.Buff);
                richTextBox2.Text = tab.Info();
                LR rule = new LR(tab.tokens);
                rule.Programm();
                BauerZamelzon rule1 = new BauerZamelzon(tab.tokens);
                textBox2.Text = rule1.MatrixShow();
                MessageBox.Show("Разбор успешно завершён");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                MessageBox.Show($"Error! {ex.Message}");
            }
        }
    }
}
