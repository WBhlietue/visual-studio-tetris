using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tetris
{
    public class Program
    {

        public static int[,] board = new int[10, 20];
        Tetris t;
        public static Program instance;
        Random r = new Random();
        public List<int[]> back = new List<int[]>();
        bool toRight = false;
        bool toLeft = false;
        bool rotate = false;

        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            new Program();

        }
        Program()
        {
            instance = this;
            Start();
            while (true)
            {
                Thread.Sleep(50);
                Update();
            }
        }

        public void Next()
        {
            int a = r.Next(0, 7);

            t = new Tetris(1);
        }


        void Start()
        {
            Clear();
            Next();
            t.Set();
            Print();
            Thread tr = new Thread(() =>
            {
                while (true)
                {
                    var k = Console.ReadKey();
                    if (k.Key == ConsoleKey.RightArrow)
                    {
                        toRight = true;
                    }
                    if (k.Key == ConsoleKey.LeftArrow)
                    {
                        toLeft = true;
                    }
                    if (k.Key == ConsoleKey.UpArrow)
                    {
                        rotate = true;
                    }
                }
            });
            tr.Start();
        }
        void Update()
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
            t.Down();
            t.Set();
            Print();
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
        void Print()
        {
            Console.Clear();
            for (int i = 0; i < board.GetLength(1); i++)
            {
                for (int j = 0; j < board.GetLength(0); j++)
                {
                    if (board[j, i] == 0)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("*");
                    }
                }
                Console.WriteLine();
            }

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
        public void Down()
        {
            foreach (var item in board)
            {
                if (item[1] + 1 >= 20 || Program.board[item[0], item[1] + 1] == 1)
                {
                    foreach (var item2 in board)
                    {
                        Program.instance.back.Add(item2);
                    }
                    Program.instance.Next();
                    return;
                }
            }
            foreach (var item in board)
            {
                item[1]++;
            }

        }
        bool Check(int y, int num)
        {
            if(num < 0 || num >= 10 || Program.board[num, y] ==1 || y < 0 || y >= 20)
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
                    switch (rotateState%4)
                    {
                        case 0:
                            //if (Check(board[1][0], board[1][1] - 1) && Check(board[1][0], board[1][1] + 1) && Check(board[1][0] - 1, board[1][1]))
                            //{
                                board[0] = new int[2] { board[1][0], board[1][1] - 1 };
                                board[2] = new int[2] { board[1][0], board[1][1] + 1 };
                                board[3] = new int[2] { board[1][0]-1, board[1][1] };
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
                    switch (rotateState%4)
                    {
                        case 0:
                            if (Check(board[1][0], board[1][1] - 1) && Check(board[1][0], board[1][1] + 1) && Check(board[1][0] - 1, board[1][1] - 1))
                            {
                                board[0] = new int[2] { board[1][0], board[1][1] - 1 };
                                board[2] = new int[2] { board[1][0], board[1][1] + 1 };
                                board[3] = new int[2] { board[1][0] - 1, board[1][1] - 1 };
                            }
                            break;
                        case 1:
                            if (Check(board[1][0] + 1, board[1][1]) && Check(board[1][0] - 1, board[1][1]) && Check(board[1][0] + 1, board[1][1] - 1))
                            {
                                board[0] = new int[2] { board[1][0] + 1, board[1][1] };
                                board[2] = new int[2] { board[1][0] - 1, board[1][1] };
                                board[3] = new int[2] { board[1][0] + 1, board[1][1] - 1 };
                            }
                            break;
                        case 2:
                            if (Check(board[1][0], board[1][1] - 1) && Check(board[1][0], board[1][1] + 1) && Check(board[1][0] + 1, board[1][1] + 1))
                            {
                                board[0] = new int[2] { board[1][0], board[1][1] - 1 };
                                board[2] = new int[2] { board[1][0], board[1][1] + 1 };
                                board[3] = new int[2] { board[1][0] + 1, board[1][1] + 1 };
                            }
                            break;
                        case 3:
                            if (Check(board[1][0] + 1, board[1][1]) && Check(board[1][0] - 1, board[1][1]) && Check(board[1][0] - 1, board[1][1] + 1))
                            {
                                board[0] = new int[2] { board[1][0] + 1, board[1][1] };
                                board[2] = new int[2] { board[1][0] - 1, board[1][1] };
                                board[3] = new int[2] { board[1][0] - 1, board[1][1] + 1 };
                            }
                            break;
                    }
                    break;
                case 3:
                    switch (rotateState % 4)
                    {
                        case 0:
                            if (Check(board[1][0], board[1][1] - 1) && Check(board[1][0], board[1][1] + 1) && Check(board[1][0] - 1, board[1][1] + 1))
                            {
                                board[0] = new int[2] { board[1][0], board[1][1] - 1 };
                                board[2] = new int[2] { board[1][0], board[1][1] + 1 };
                                board[3] = new int[2] { board[1][0] - 1, board[1][1] + 1 };
                            }
                            break;
                        case 1:
                            if (Check(board[1][0] + 1, board[1][1]) && Check(board[1][0] - 1, board[1][1]) && Check(board[1][0] - 1, board[1][1] - 1))
                            {
                                board[0] = new int[2] { board[1][0] + 1, board[1][1] };
                                board[2] = new int[2] { board[1][0] - 1, board[1][1] };
                                board[3] = new int[2] { board[1][0] - 1, board[1][1] - 1 };
                            }
                            break;
                        case 2:
                            if (Check(board[1][0], board[1][1] - 1) && Check(board[1][0], board[1][1] + 1) && Check(board[1][0] + 1, board[1][1] - 1))
                            {
                                board[0] = new int[2] { board[1][0], board[1][1] - 1 };
                                board[2] = new int[2] { board[1][0], board[1][1] + 1 };
                                board[3] = new int[2] { board[1][0] + 1, board[1][1] - 1 };
                            }
                            break;
                        case 3:
                            if (Check(board[1][0] + 1, board[1][1]) && Check(board[1][0] - 1, board[1][1]) && Check(board[1][0] + 1, board[1][1] + 1))
                            {
                                board[0] = new int[2] { board[1][0] + 1, board[1][1] };
                                board[2] = new int[2] { board[1][0] - 1, board[1][1] };
                                board[3] = new int[2] { board[1][0] + 1, board[1][1] + 1 };
                            }
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
                Program.board[item[0], item[1]] = 1;
            }
        }
    }
}
