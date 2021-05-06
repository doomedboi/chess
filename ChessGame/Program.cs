using System;
using System.Collections.Generic;

namespace ChessGame
{
    
    //invariant: we create only valid coor
    class Coordinates
    {
        public uint x { get; set; }
        public uint y { get; set; }

        public Coordinates(uint x, uint y)
        {
            this.x = x;
            this.y = y;

            System.Diagnostics.Debug.Assert(IsValidCoord());
        }

        public bool IsValidCoord()
        {
            return (
                x >= 0 && x <= 7 && y >= 0 && y <= 7
                   );
        }
    }

    class Step
    {
        public Coordinates From { get; set; }
        public Coordinates To { get; set; }
        public Step(Coordinates from, Coordinates to)
        {
            From = from;
            To = to;
        }
        //constructor of PREOBRAZOVANIYA
        //get a step by passing a string
        public Step(string move)
        {
            From = new Coordinates(Convert.ToUInt16(Convert.ToChar(move[0]) - 'a'),
                UInt16.Parse(move.Substring(1, 1)) - (uint)1);
            To = new Coordinates(Convert.ToUInt16(Convert.ToChar(move[3]) - 'a'), 
                UInt16.Parse(move.Substring(4, 1)) - (uint)1);
        }

        public string GetStep()
        {
            return new string($"{(char)(From.x + 'a')}{From.y+1}-" +
                $"{(char)(To.x +'a')}{To.y+1}");
        }
        //private constructor against creating without args
        Step() { }

    }
    class Board
    {
        bool FirstPlayerMakeMoving = true; // white = 1

        void changePlayerMoving()
        {
            FirstPlayerMakeMoving = !FirstPlayerMakeMoving;
        }
        public Board()
        {
            Desktop = new ChessUnit[8, 8];
            History = new List<Tuple<int, Step>>();
            //initialize coords via units
            //TODO
            //temp movement
            for (int i = 0; i < 8; i++)
                for(int j = 0; j < 8; j++)
                {
                    Desktop[i, j] = new EmptyUnit(2); // NONE?
                }
            

            Desktop[1, 0] = new Pawn(1);
            Desktop[0, 1] = new Pawn(1);
            Desktop[0, 3] = new Pawn(0);
            Desktop[1, 1] = new Pawn(1);
            Desktop[3, 3] = new Queen(0);
            Desktop[3, 2] = new Pawn(1);
            InitDict();
        }

        void InitDict()
        {
            Icons.Add("empty", "[ ]");
            Icons.Add("pawn0", "[♟]");
            Icons.Add("pawn1", "[♙ ]"); // бледные - белые
            Icons.Add("queen0", "[♛ ]");
            Icons.Add("queen1", "[♕ ]");

        }

        public void PrintHistory()
        {
            Console.Clear();
            DrawDesktop();
            foreach (var elem in History)
            {
                Console.WriteLine($"Player {elem.Item1} makes" +
                    $" move {elem.Item2.GetStep()}"); 
            }
        }

        List<Tuple<int, Step>> History;
        ChessUnit[,] Desktop;
        // rule: classic name postfix of what color
        //example : pawn0 - black sides chess
        private Dictionary<string, string> Icons =
            new Dictionary<string, string>();
        
        bool bDifferentSides(Step move)
        {
            return Desktop[move.From.x, move.From.y]._side.color != 
                Desktop[move.To.x, move.To.y]._side.color;
        }

        //call after Internal check
        bool CanPawn(Step move)
        {
            var name = Desktop[move.From.x, move.From.y].GetName();
            if (name != "pawn0" && name != "pawn1")
                return true;
            //if this move
            if (Math.Abs((int)(move.To.x - move.From.x)) == Math.Abs(1)
                && Math.Abs((int)(move.To.y - move.From.y)) == Math.Abs(1))
                return Desktop[move.To.x, move.To.y]._side.color != ChessUnit.COLOR_OF_CHESS.NONE;
            
            return true;
                
        }

        bool CanQueen(Step move)
        {
            int dx = (int)(move.To.x - move.From.x);
            int dy = (int)(move.To.y - move.From.y);
            var from = move.From;
            int sy = 0;
            if (dy != 0) sy = dy / Math.Abs(dy);
            int sx = 0;
            if (dx != 0) sx = dx / Math.Abs(dx);
            var to = move.To;
            if (Math.Abs(dx) == Math.Abs(dy))
            {
                for (int x = (int)from.x + sx, y = (int)from.y + sy; x != to.x; x += sx, y += sy)
                {
                    if (Desktop[x, y].Empty())
                        continue;
                    else
                        return false;
                }

            }
            else if (dy == 0)
            {
                for (int i = (int)from.x + sx; i != to.x; i += sx)
                {
                    if (Desktop[i, from.y].Empty())
                        continue;

                    else
                        return false;
                }

            }
            else if (dx == 0)
            {
                for (int i = (int)from.y + sy; i != to.y; i += sy)
                {
                    if (Desktop[from.x, i].Empty())
                        continue;
                    else
                        return false;
                }

            }
            else
                return false;
            return true; // is it right??
        }
        public bool Move(Step move)
        {
            if (Desktop[move.From.x, move.From.y].CanInternalMove(move))
            {
                if (CanPawn(move) != true)
                {
                    Console.Clear(); //rewrite this
                    return false;
                }
                if (bDifferentSides(move) != true ) // жесткий костыль
                {
                    Console.Clear(); //rewrite this
                    return false;
                } 

                if (CanQueen(move) != true)
                {
                    Console.Clear();
                    return false;
                }
                Desktop[move.To.x, move.To.y] = Desktop[move.From.x, move.From.y];
                Desktop[move.From.x, move.From.y] = new EmptyUnit(3);
                //(-2) to maintain the invariant
                History.Add(new Tuple<int, Step>(Convert.ToInt32
                    (FirstPlayerMakeMoving) + (-2), move)); 
                changePlayerMoving();
                Console.Clear();
                return true;
            }
            Console.Clear();
            return false;
        }

        public void DrawDesktop()
        {
            for (int y = 7; y > -1; y--)
            {
                Console.Write('\n');
                for (int x = 0; x < 8; x++)
                {
                    Console.Write(Icons[Desktop[x, y].GetName()]);
                }
                Console.Write(y + 1);
                Console.Write('\n');
            }
            Console.WriteLine(" a   b   c   d   e   f   g   h");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Player {2 - Convert.ToInt32(FirstPlayerMakeMoving)} must move");
            Console.ResetColor();
        }
    }

    
    class Program
    {
        static public void GameLoop()
        {
            Board tmpBrd = new Board();
            tmpBrd.DrawDesktop();
            //test
            string mv = "";
            bool WantToPlay = true;
            while (WantToPlay)
            {
                mv = Console.ReadLine();
                if (mv == "exit")
                {
                    WantToPlay = false;
                    Console.Clear();
                    //return; need it?
                }
                if (mv == "history")
                {
                    //print history
                    tmpBrd.PrintHistory();
                    
                }
                else
                {
                    tmpBrd.Move(new Step(mv));
                    tmpBrd.DrawDesktop();
                }
            } //TODO: какой игрок сейчас ходит? - вот такие шахматы и можно перемещать...

        }
        static void Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            GameLoop();
        }
    }
}
