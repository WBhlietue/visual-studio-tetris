using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tetris
{
    public partial class Form1 : Form
    {
        bool start = false;
        public static Form1 instance;
        int[,] board = new int[10, 20];
        public int selectedDiff = 0;
        public Form1()
        {
            instance = this;
            InitializeComponent();
        }
        public void Display(int[,] board)
        {
            this.board= board;
            pictureBox1.Invalidate();
        }

        public void OnKeyDown(object sender, KeyEventArgs key)
        {
            switch (key.KeyCode)
            {
                case Keys.A:
                    Program.instance.Left();
                    break;
                case Keys.D:
                    Program.instance.Right();
                    break;
                case Keys.W:
                    Program.instance.Rotate();
                    break;
                case Keys.Space:
                    Program.instance.PastDown();
                    break;
                case Keys.S:
                    Program.instance.Down();
                    break;
            }
        }

        private void pictureBox1_Click(object sender, PaintEventArgs e)
        {
            Brush brush = new SolidBrush(Color.Red);
            Rectangle rect = new Rectangle(1, 1, 170, 340);

            e.Graphics.FillRectangle(brush, rect);

            Brush br = new SolidBrush(Color.Black);
            Brush en = new SolidBrush(Color.Blue);
            Brush mv = new SolidBrush(Color.Green);
            for(int i = 0; i < board.GetLength(1); i++)
            {
                for (int j = 0; j < board.GetLength(0); j++)
                {
                    if (i == 0)
                    {
                        e.Graphics.FillRectangle(brush, new Rectangle(2 + 17 * j, 2 + 17 * i, 15, 15));
                    }
                    else
                    {
                        e.Graphics.FillRectangle(board[j, i] == 0 ? br :
                            board[j, i] == 1 ? en : mv, new Rectangle(2 + 17 * j, 2 + 17 * i, 15, 15));
                    }
                }
            }
            br.Dispose();
            brush.Dispose();
        }

        

        private void label2_Click(object sender, EventArgs e)
        {
            if (!start)
            {
                int a = radioButton1.Checked ? 0 :
                        radioButton2.Checked ? 1 : 2;
                Program.instance.Start(a);
                label2.Text = "End";
                groupBox1.Enabled = false ;
                start= true;
            }
            else
            {
                Program.instance.Stop();
                groupBox1.Enabled = true;
                start = false;
                label2.Text = "Start";
            }
        }
    }
}
