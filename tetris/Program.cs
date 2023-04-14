using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace tetris
{
    public class Program
    {
        Form1 form;
        public static int[,] board = new int[10, 20];
        Tetris t;
        public static Program instance;
        Random r = new Random();
        public List<int[]> back = new List<int[]>();
        bool toRight = false;
        bool toLeft = false;
        bool rotate = false;
        public bool enable = false;
        static Thread move;
        public int diff = 1000;
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Thread tr = new Thread(() =>
            {
                Thread.Sleep(100);
                new Program();
            });
            tr.Start();
            Application.Run(new Form1());
            tr.Abort();
            move.Abort();
        }
        Program()
        {
            instance = this;
            form = Form1.instance;
            //Start();
            move = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(100);
                    if (enable)
                    {
                        Move();
                    }
                }
            });
            move.Start();
            while (true)
            {
                Thread.Sleep(diff);
                if (enable)
                {
                    Update();
                }
            }
        }


        public void Next()
        {
            int a = r.Next(0, 7);

            t = new Tetris(a);
        }


        public void Start(int d)
        {
            diff = d == 0 ? 1000 :
                   d == 1 ? 500 : 300;
            Clear();
            Next();
            t.Set();
            Print();
            enable = true;
        }

        public void Restart()
        {
            board = new int[10, 20];
            back = new List<int[]>();
        }

        void Move()
        {
            Clear();
            if (toRight)
            {
                t.Right();
                toRight = false;
            }
            if (toLeft)
            {
                t.Left();
                toLeft = false;
            }
            if (rotate)
            {
                t.Rotate();
                rotate = false;
            }
            t.Set();
            Check();
            Print();
        }
        public void Down()
        {
            t.Down();
        }

        void Update()
        {

            Clear();
            t.Down();
            t.Set();
            Check();
            Print();
        }

        public void PastDown()
        {
            int a = 1;
            while(a == 1)
            {
                Thread.Sleep(1);
                a = t.Down();
            }
        }

        public void Rotate()
        {
            rotate = true;
        }
        public void Left()
        {
            toLeft = true;
        }
        public void Right()
        {
            toRight = true;
        }
        void Clear()
        {
            for (int i = 0; i < board.GetLength(1); i++)
            {
                for (int j = 0; j < board.GetLength(0); j++)
                {
                    board[j, i] = 0;
                }
            }
            foreach (var item in back)
            {
                board[item[0], item[1]] = 1;
            }
        }
        
        public void Stop()
        {
            enable = false;
            Restart();
            Print();
        }
        void Check()
        {
            for (int i = 0; i < board.GetLength(1); i++) 
            {
                bool canRem = true;
                for (int j = 0; j < board.GetLength(0); j++) 
                {
                    if (board[j, i] == 0)
                    {
                        canRem = false;
                        break;
                    }
                }
                if (canRem)
                {
                    for (int j = i - 1; j >= 0; j--)
                    {
                        for (int k = 0; k < board.GetLength(0); k++)
                        {
                            board[k, j + 1] = board[k, j];
                        }
                    }
                    for (int k = 0; k < board.GetLength(0); k++)
                    {
                        board[k, 0] = 0;
                    }
                }
            }
            for(int i = 0; i < 10; i++)
            {
                if (board[i, 0] == 1)
                {
                    enable = false;
                    MessageBox.Show("Game over");
                    return;
                }
            }
        }
        void Print()
        {
            //Console.Clear();
            //for (int i = 0; i < board.GetLength(1); i++)
            //{
            //    for (int j = 0; j < board.GetLength(0); j++)
            //    {
            //        if (board[j, i] == 0)
            //        {
            //            Console.Write(" ");
            //        }
            //        else
            //        {
            //            Console.Write("*");
            //        }
            //    }
            //    Console.WriteLine();
            //}

            form.Display(board);

        }
    }

    public class Tetris
    {
        List<int[]> board = new List<int[]>();
        public bool stop = false;
        int tetrisCode;
        int rotateState;
        public Tetris(int code)
        {
            tetrisCode = code;
            switch (code)
            {
                case 0:
                    board.Add(new int[2] { 5, 0 });
                    board.Add(new int[2] { 6, 0 });
                    board.Add(new int[2] { 5, 1 });
                    board.Add(new int[2] { 6, 1 });
                    break;
                case 1:
                    board.Add(new int[2] { 5, 0 });
                    board.Add(new int[2] { 6, 0 });
                    board.Add(new int[2] { 7, 0 });
                    board.Add(new int[2] { 6, 1 });
                    break;
                case 2:
                    board.Add(new int[2] { 5, 0 });
                    board.Add(new int[2] { 6, 0 });
                    board.Add(new int[2] { 7, 0 });
                    board.Add(new int[2] { 5, 1 });
                    break;
                case 3:
                    board.Add(new int[2] { 5, 0 });
                    board.Add(new int[2] { 6, 0 });
                    board.Add(new int[2] { 7, 0 });
                    board.Add(new int[2] { 7, 1 });
                    break;
                case 4:
                    board.Add(new int[2] { 5, 0 });
                    board.Add(new int[2] { 6, 0 });
                    board.Add(new int[2] { 7, 1 });
                    board.Add(new int[2] { 6, 1 });
                    break;
                case 5:
                    board.Add(new int[2] { 5, 0 });
                    board.Add(new int[2] { 6, 0 });
                    board.Add(new int[2] { 5, 1 });
                    board.Add(new int[2] { 4, 1 });
                    break;
                case 6:
                    board.Add(new int[2] { 5, 0 });
                    board.Add(new int[2] { 6, 0 });
                    board.Add(new int[2] { 7, 0 });
                    board.Add(new int[2] { 4, 0 });
                    break;
                default:
                    break;
            }
        }
        public void Right()
        {
            foreach (var item in board)
            {
                if (item[0] + 1 >= 10 || Program.board[item[0] + 1, item[1]] == 1)
                {
                    return;
                }
            }
            foreach (var item in board)
            {
                item[0]++;
            }
        }
        public void Left()
        {
            foreach (var item in board)
            {
                if (item[0] - 1 < 0 || Program.board[item[0] - 1, item[1]] == 1)
                {
                    return;
                }
            }
            foreach (var item in board)
            {
                item[0]--;
            }
        }
        public int Down()
        {
            foreach (var item in board)
            {
                try
                {
                    if (item[1] + 1 >= 20 || Program.board[item[0], item[1] + 1] == 1)
                {
                    foreach (var item3 in board)
                    {
                        Program.board[item3[0], item3[1]] = 1;
                    }
                    foreach (var item2 in board)
                    {
                        Program.instance.back.Add(item2);
                    }
                    Program.instance.Next();
                    return 0;
                }
                }
                catch { }
            }
            foreach (var item in board)
            {
                item[1]++;
            }
            return 1;

        }

        //public int Down()
        //{
        //    bool canDown = true;
        //    foreach (var item in board)
        //    {
        //        if (item[1] + 1 >= 20 || Program.board[item[0], item[1] + 1] == 1)
        //        {
        //            canDown = false;
        //            break;
        //        }
        //    }
        //    if (canDown)
        //    {
        //        foreach (var item in board)
        //        {
        //            item[1]++;
        //        }
        //        return 1;
        //    }
        //    else
        //    {
        //        foreach (var item3 in board)
        //        {
        //            Program.board[item3[0], item3[1]] = 1;
        //        }
        //        foreach (var item2 in board)
        //        {
        //            Program.instance.back.Add(item2);
        //        }
        //        Program.instance.Next();
        //        return -1;
        //    }
        //}

        bool Check(int y, int num)
        {
            if (num < 0 || num >= 10 || Program.board[num, y] == 1 || y < 0 || y >= 20)
            {
                return false;
            }
            return true;
        }
        public void Rotate()
        {

            switch (tetrisCode)
            {
                case 0:
                    break;
                case 1:
                    switch (rotateState % 4)
                    {
                        case 0:
                            //if (Check(board[1][0], board[1][1] - 1) && Check(board[1][0], board[1][1] + 1) && Check(board[1][0] - 1, board[1][1]))
                            //{
                            board[0] = new int[2] { board[1][0], board[1][1] - 1 };
                            board[2] = new int[2] { board[1][0], board[1][1] + 1 };
                            board[3] = new int[2] { board[1][0] - 1, board[1][1] };
                            //}
                            break;
                        case 1:
                            //if (Check(board[1][0], board[1][1] - 1) && Check(board[1][0]+1, board[1][1]) && Check(board[1][0] - 1, board[1][1]))
                            //{
                            board[0] = new int[2] { board[1][0], board[1][1] - 1 };
                            board[2] = new int[2] { board[1][0] + 1, board[1][1] };
                            board[3] = new int[2] { board[1][0] - 1, board[1][1] };
                            //}
                            break;
                        case 2:
                            //if (Check(board[1][0], board[1][1] - 1) && Check(board[1][0], board[1][1] + 1) && Check(board[1][0] + 1, board[1][1]))
                            //{
                            board[0] = new int[2] { board[1][0], board[1][1] - 1 };
                            board[2] = new int[2] { board[1][0], board[1][1] + 1 };
                            board[3] = new int[2] { board[1][0] + 1, board[1][1] };
                            //}
                            break;
                        case 3:
                            //if (Check(board[1][0] + 1, board[1][1]) && Check(board[1][0], board[1][1] + 1) && Check(board[1][0] - 1, board[1][1]))
                            //{
                            board[0] = new int[2] { board[1][0] + 1, board[1][1] };
                            board[2] = new int[2] { board[1][0], board[1][1] + 1 };
                            board[3] = new int[2] { board[1][0] - 1, board[1][1] };
                            //}
                            break;
                    }

                    break;
                case 2:
                    switch (rotateState % 4)
                    {
                        case 0:
                            //if (Check(board[1][0], board[1][1] - 1) && Check(board[1][0], board[1][1] + 1) && Check(board[1][0] - 1, board[1][1] - 1))
                            //{
                            board[0] = new int[2] { board[1][0], board[1][1] - 1 };
                            board[2] = new int[2] { board[1][0], board[1][1] + 1 };
                            board[3] = new int[2] { board[1][0] - 1, board[1][1] - 1 };
                            //}
                            break;
                        case 1:
                            //if (Check(board[1][0] + 1, board[1][1]) && Check(board[1][0] - 1, board[1][1]) && Check(board[1][0] + 1, board[1][1] - 1))
                            //{
                            board[0] = new int[2] { board[1][0] + 1, board[1][1] };
                            board[2] = new int[2] { board[1][0] - 1, board[1][1] };
                            board[3] = new int[2] { board[1][0] + 1, board[1][1] - 1 };
                            //}
                            break;
                        case 2:
                            //if (Check(board[1][0], board[1][1] - 1) && Check(board[1][0], board[1][1] + 1) && Check(board[1][0] + 1, board[1][1] + 1))
                            //{
                            board[0] = new int[2] { board[1][0], board[1][1] - 1 };
                            board[2] = new int[2] { board[1][0], board[1][1] + 1 };
                            board[3] = new int[2] { board[1][0] + 1, board[1][1] + 1 };
                            //}
                            break;
                        case 3:
                            //if (Check(board[1][0] + 1, board[1][1]) && Check(board[1][0] - 1, board[1][1]) && Check(board[1][0] - 1, board[1][1] + 1))
                            //{
                            board[0] = new int[2] { board[1][0] + 1, board[1][1] };
                            board[2] = new int[2] { board[1][0] - 1, board[1][1] };
                            board[3] = new int[2] { board[1][0] - 1, board[1][1] + 1 };
                            //}
                            break;
                    }
                    break;
                case 3:
                    switch (rotateState % 4)
                    {
                        case 0:
                            //if (Check(board[1][0], board[1][1] - 1) && Check(board[1][0], board[1][1] + 1) && Check(board[1][0] - 1, board[1][1] + 1))
                            //{
                            board[0] = new int[2] { board[1][0], board[1][1] - 1 };
                            board[2] = new int[2] { board[1][0], board[1][1] + 1 };
                            board[3] = new int[2] { board[1][0] - 1, board[1][1] + 1 };
                            //}
                            break;
                        case 1:
                            //if (Check(board[1][0] + 1, board[1][1]) && Check(board[1][0] - 1, board[1][1]) && Check(board[1][0] - 1, board[1][1] - 1))
                            //{
                            board[0] = new int[2] { board[1][0] + 1, board[1][1] };
                            board[2] = new int[2] { board[1][0] - 1, board[1][1] };
                            board[3] = new int[2] { board[1][0] - 1, board[1][1] - 1 };
                            //}
                            break;
                        case 2:
                            //if (Check(board[1][0], board[1][1] - 1) && Check(board[1][0], board[1][1] + 1) && Check(board[1][0] + 1, board[1][1] - 1))
                            //{
                            board[0] = new int[2] { board[1][0], board[1][1] - 1 };
                            board[2] = new int[2] { board[1][0], board[1][1] + 1 };
                            board[3] = new int[2] { board[1][0] + 1, board[1][1] - 1 };
                            //}
                            break;
                        case 3:
                            //if (Check(board[1][0] + 1, board[1][1]) && Check(board[1][0] - 1, board[1][1]) && Check(board[1][0] + 1, board[1][1] + 1))
                            //{
                            board[0] = new int[2] { board[1][0] + 1, board[1][1] };
                            board[2] = new int[2] { board[1][0] - 1, board[1][1] };
                            board[3] = new int[2] { board[1][0] + 1, board[1][1] + 1 };
                            //}
                            break;
                    }
                    break;
                case 4:
                    switch (rotateState % 2)
                    {
                        case 0:
                            board[0] = new int[2] { board[1][0], board[1][1] - 1 };
                            board[2] = new int[2] { board[1][0] - 1, board[1][1] };
                            board[3] = new int[2] { board[1][0] - 1, board[1][1] + 1 };
                            break;
                        case 1:
                            board[0] = new int[2] { board[1][0] - 1, board[1][1] };
                            board[2] = new int[2] { board[1][0], board[1][1] + 1 };
                            board[3] = new int[2] { board[1][0] + 1, board[1][1] + 1 };
                            break;
                    }
                    break;
                case 5:
                    switch (rotateState % 2)
                    {
                        case 0:
                            board[0] = new int[2] { board[1][0], board[1][1] - 1 };
                            board[2] = new int[2] { board[1][0] + 1, board[1][1] };
                            board[3] = new int[2] { board[1][0] + 1, board[1][1] + 1 };
                            break;
                        case 1:
                            board[0] = new int[2] { board[1][0] + 1, board[1][1] };
                            board[2] = new int[2] { board[1][0], board[1][1] + 1 };
                            board[3] = new int[2] { board[1][0] - 1, board[1][1] + 1 };
                            break;
                    }
                    break;
                case 6:
                    switch (rotateState % 2)
                    {
                        case 0:
                            board[0] = new int[2] { board[1][0], board[1][1] - 1 };
                            board[2] = new int[2] { board[1][0], board[1][1] + 1 };
                            board[3] = new int[2] { board[1][0], board[1][1] + 2 };
                            break;
                        case 1:
                            board[0] = new int[2] { board[1][0] - 1, board[1][1] };
                            board[2] = new int[2] { board[1][0] + 1, board[1][1] };
                            board[3] = new int[2] { board[1][0] + 2, board[1][1] };
                            break;
                    }
                    break;
            }
            rotateState++;
        }
        public void Set()
        {
            foreach (var item in board)
            {
                try
                {
                    Program.board[item[0], item[1]] = 2;
                }
                catch { }
            }
        }
    }
}
