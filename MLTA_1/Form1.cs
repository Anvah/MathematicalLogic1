using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MLTA_1
{
    public partial class Form1 : Form
    {
        NormalFormConvertor expression;
        TextBox[,] tex;
        Label[] vars;
        string[] operands = { "⊕", "*", "∨", "∧", "¬", "→", "⟷" };
        int sdnfCount = 0;
        int sknfCount = 0;
        int spnfCount = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox7.Hide();
            textBox6.Hide();
            textBox5.Hide();
            label8.Hide();
            label7.Hide();
            label6.Hide();
        }
        void CreateTrustTable()
        {
            LabelEffect.Text = "";
            Duality.Checked = false;
            textBox7.Text = "";
            textBox6.Text = "";
            textBox5.Text = "";
            textBox4.Text = "";
            textBox3.Text = "";
            textBox2.Text = "";
            int top = 5;
            int left = 20;
            vars = new Label[expression.trustTable.varCount + 2];
            tex = new TextBox[expression.trustTable.trustTableRowCount, expression.trustTable.varCount + 2];
            for (int i = 0; i < expression.trustTable.varCount + 2; i++)
            {
                vars[i] = new Label();
                vars[i].Left = left;
                vars[i].Top = top;
                vars[i].Font = new Font("", 16);
                vars[i].Size = new Size(30, 30);
                if (i == expression.trustTable.varCount)
                    vars[i].Text = "F";
                else if (i == expression.trustTable.varCount + 1)
                    vars[i].Text = "F'";
                else
                {
                    vars[i].Text = expression.trustTable.varArray[i];
                }

                panel1.Controls.Add(vars[i]);

                left += 50 + 20;
                top = 5;
            }
            top = 40;
            left = 20;
            for (int i = 0; i < expression.trustTable.trustTableRowCount; i++)
            {
                for (int j = 0; j < expression.trustTable.varCount + 2; j++)
                {

                    tex[i, j] = new TextBox();
                    if (j < expression.trustTable.varCount)
                        tex[i, j].ReadOnly = true;
                    tex[i, j].Left = left;
                    tex[i, j].Top = top;
                    tex[i, j].Multiline = true;
                    tex[i, j].Font = new Font("", 16);
                    tex[i, j].Size = new Size(30, 30);
                    tex[i, j].Text = 0.ToString();
                    panel1.Controls.Add(tex[i, j]);
                    left += tex[i, j].Height + 40;

                }
                left = 20;
                top += 40;
            }
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            labelSelfDuality.Text = "";
            for (int i = 0; i < expression.trustTable.trustTableRowCount; i++)
            {
                tex[i, expression.trustTable.varCount + 1].Hide();
            }
            vars[expression.trustTable.varCount + 1].Hide();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (expression != null)
            {
                for (int i = 0; i < expression.trustTable.varCount + 2; i++)
                {
                    panel1.Controls.Remove(vars[i]);
                    panel1.Controls.Remove(vars[i]);
                    for (int j = 0; j < expression.trustTable.trustTableRowCount; j++)
                    {
                        panel1.Controls.Remove(tex[j, i]);

                    }
                }
            }
            try
            {
                int count = Convert.ToInt32(textBox1.Text);
                if (count > 7) count = 7;
                expression = new NormalFormConvertor(count);
                CreateTrustTable();
                Fill();
                SetResults();
            }
            catch
            {
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                labelSelfDuality.Text = "";
                MessageBox.Show("Некорректное значение", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        private void Solve()
        {
            for (int i = 0; i < expression.trustTable.trustTableRowCount; i++)
            {
                if(expression.trustTable.result[i])
                    tex[i, expression.trustTable.varCount].Text = "1";
                else
                    tex[i, expression.trustTable.varCount].Text = "0";
            }
        }
        void SetResults()
        {
            for (int i = 0; i < expression.trustTable.trustTableRowCount; i++)
            {
                expression.trustTable.result[i] = Convert.ToInt32(tex[i, expression.trustTable.varCount].Text) != 0;
            }
        }
        private void Fill()
        {
            labelSelfDuality.Text = "";
            for (int i = 0; i < expression.trustTable.trustTableRowCount; i++)
            {
                for (int j = 0; j < expression.trustTable.varCount; j++)
                {
                    if (expression.trustTable.trustTable[i, j])
                    {
                        tex[i, j].Text = "1";
                    }
                    else
                    {
                        tex[i, j].Text = "0";
                    }


                }
            }
        }
        private void Read()
        {
            LabelEffect.Text = "";
            labelSelfDuality.Text = "";
            for (int j = 0; j < expression.trustTable.trustTableRowCount; j++)
            {

                expression.trustTable.result[j] = Convert.ToInt32(tex[j, expression.trustTable.varCount].Text) != 0;
            }
            if (Duality.Checked)
            {
                bool isSelfDuality = expression.trustTable.IsSelfDuality();
                if (isSelfDuality)
                    labelSelfDuality.Text = "Самодвойсвенна";
                else
                    labelSelfDuality.Text = "Не самодвойсвенна";
                expression.trustTable.Duality();
                for (int i = 0; i < expression.trustTable.trustTableRowCount; i++)
                {
                    if (expression.trustTable.dualityResult[i])
                    {
                        tex[i, expression.trustTable.varCount + 1].Text = "1";
                    }
                    else
                    {
                        tex[i, expression.trustTable.varCount + 1].Text = "0";
                    }
                    tex[i, expression.trustTable.varCount + 1].Show();
                }
                vars[expression.trustTable.varCount + 1].Show();
            }
            else
            {
                for (int i = 0; i < expression.trustTable.trustTableRowCount; i++)
                {
                    tex[i, expression.trustTable.varCount + 1].Hide();
                }
                vars[expression.trustTable.varCount + 1].Hide();
            }
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Read();
            string ans = expression.SDNF();
            if (Duality.Checked)
            {
                textBox7.Show();
                label8.Show();
                string ansd = expression.SDNF(true);
                if (ansd.Length > 0)
                {
                    textBox7.Text = ansd;
                }
                else
                {
                    textBox7.Text = "Не существует";
                }
            }
            else
            {
                textBox7.Hide();
                textBox6.Hide();
                textBox5.Hide();
                label8.Hide();
                label7.Hide();
                label6.Hide();
            }
            if (ans.Length > 0)
            {
               
                textBox2.Text = ans;
                int count = 0;
                for (int i = 0; i < textBox2.Text.Length; i++)
                {
                    if (operands.Contains(textBox2.Text[i].ToString()) && textBox2.Text[i] != '¬')
                        count++;
                }
                sdnfCount = count;
                label2.Text = "кол-во операций " + count;
            }
            else
            {
                sdnfCount = 9999;
                textBox2.Text = "Не существует";
            }
            MinOperations();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Read();
            string ans = expression.SKNF();
            if (Duality.Checked)
            {
                textBox6.Show();
                label7.Show();
                string ansd = expression.SKNF(true);
                if (ansd.Length > 0)
                {
                    textBox6.Text = ansd;
                }
                else
                {
                    textBox6.Text = "Не существует";
                }
            }
            else
            {
                textBox7.Hide();
                textBox6.Hide();
                textBox5.Hide();
                label8.Hide();
                label7.Hide();
                label6.Hide();
            }
            if (ans.Length > 0)
            {
                textBox3.Text = ans;
                int count = 0;
                for (int i = 0; i < textBox3.Text.Length; i++)
                {
                    if (operands.Contains(textBox3.Text[i].ToString()) && textBox3.Text[i] != '¬')
                        count++;
                }
                label3.Text = "кол-во операций " + count;
                sknfCount = count;
            }
            else
            {
                sknfCount = 9999;
                textBox3.Text = "Не существует";
            }
            MinOperations();
        }
        void MinOperations()
        {
            if (sdnfCount != 0 && sknfCount != 0 && spnfCount != 0)
            {
                if (sdnfCount <= sknfCount && sdnfCount <= spnfCount)
                {
                    LabelEffect.Text = "В СДНФ операций меньше";
                }
                if (sknfCount <= sdnfCount && sknfCount <= sdnfCount)
                {
                    LabelEffect.Text = "В СКНФ операций меньше";
                }
                if (spnfCount <= sknfCount && spnfCount <= sdnfCount)
                {
                    LabelEffect.Text = "В СПНФ операций меньше";
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Read();
            string ans = expression.SPNF();
            if (Duality.Checked)
            {
                textBox5.Show();
                label6.Show();
                string ansd = expression.SPNF(true);
                if (ansd.Length > 0)
                {
                    textBox5.Text = ansd;
                }
                else
                {
                    textBox5.Text = "Не существует";
                }
            }
            else
            {
                textBox7.Hide();
                textBox6.Hide();
                textBox5.Hide();
                label8.Hide();
                label7.Hide();
                label6.Hide();
            }
            if (ans.Length > 0)
            {
                
                textBox4.Text = ans;
                int count = 0;
                for (int i = 0; i < textBox4.Text.Length; i++)
                {
                    if (operands.Contains(textBox4.Text[i].ToString()) && textBox4.Text[i] != '¬')
                        count++;
                }
                label4.Text ="кол-во операций "  + count;
                spnfCount = count;
            }
            else
            {
                spnfCount = 9999;
                textBox4.Text = "Не существует";
            }
            MinOperations();
        }
        private void WriteButtons_Click(object sender, EventArgs e)
        {
            string s = ((Button)sender).Text;
            int length = ExpressionTextBox.Text.Length;
            if ((ExpressionTextBox.Text.Length > 0 && !operands.Contains(ExpressionTextBox.Text[length - 1].ToString()) && s != "¬" && ExpressionTextBox.Text[length - 1] != '(') || (s == "¬" && (length == 0 || (ExpressionTextBox.Text[length - 1] != ')' && !char.IsLetter(ExpressionTextBox.Text[length - 1])))))
            {
                ExpressionTextBox.Text += ((Button)sender).Text;
            }
            

        }

        private void button13_Click(object sender, EventArgs e)
        {
            string s = ExpressionTextBox.Text;
            if (operands.Contains(s[s.Length - 1].ToString()))
                MessageBox.Show("Некорректная запись выражения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                if (expression != null)
                {
                    for (int i = 0; i < expression.trustTable.varCount + 2; i++)
                    {
                        panel1.Controls.Remove(vars[i]);
                        panel1.Controls.Remove(vars[i]);
                        for (int j = 0; j < expression.trustTable.trustTableRowCount; j++)
                        {
                            panel1.Controls.Remove(tex[j, i]);

                        }
                    }
                }
                try
                {
                    expression = new NormalFormConvertor(s);
                    CreateTrustTable();
                    Fill();
                    Solve();
                }
                catch
                {
                    MessageBox.Show("Некорректная запись выражения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
                

            
        }

        private void ExpressionTextBox_TextChanged(object sender, EventArgs e)
        {
            int length = ExpressionTextBox.Text.Length;
            if(length > 0)
            {
                char l = ExpressionTextBox.Text[length - 1];
                if ((!operands.Contains(l.ToString()) && !char.IsLetter(l) && l != '(' && l != ')') || (length > 1 && char.IsLetter(l) && char.IsLetter(ExpressionTextBox.Text[length - 2])))
                {
                    ExpressionTextBox.Text = ExpressionTextBox.Text.Remove(length - 1);
                }
            }
            
        }
    }
}
