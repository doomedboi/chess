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

        public int GetDx()
        {
            return (int)(To.x - From.x);
        }

        public int GetDy()
        {
            return (int)(To.y - From.y);
        }
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


            LocateStartChess();
            InitDict();
        }
        void LocateStartChess()
        {
            //pawns
            for (int x = 0; x < 8; x++)
            {
                Desktop[x, 1] = new Pawn(1);
                Desktop[x, 6] = new Pawn(0);
            }
            Desktop[0, 0] = new Rook(1);
            Desktop[1, 0] = new Knight(1);
            Desktop[2, 0] = new Bishop(1);
            Desktop[3, 0] = new Queen(1);
            Desktop[4, 0] = new King(1);
            Desktop[5, 0] = new Bishop(1);
            Desktop[6, 0] = new Knight(1);
            Desktop[7, 0] = new Rook(1);
            Desktop[0, 7] = new Rook(0);
            Desktop[1, 7] = new Knight(0);
            Desktop[2, 7] = new Bishop(0);
            Desktop[3, 7] = new Queen(0);
            Desktop[4, 7] = new King(0);
            Desktop[5, 7] = new Bishop(0);
            Desktop[6, 7] = new Knight(0);
            Desktop[7, 7] = new Rook(0);
            
        }
        void InitDict()
        {
            Icons.Add("empty", "[  ]");
            Icons.Add("pawn0", "[♟]");
            Icons.Add("pawn1", "[♙ ]"); // бледные - белые
            Icons.Add("queen0", "[♛ ]");
            Icons.Add("queen1", "[♕ ]");
            Icons.Add("king0", "[♚ ]");
            Icons.Add("king1", "[♔ ]"); //white king
            Icons.Add("bishop0", "[♝ ]");
            Icons.Add("bishop1", "[♗ ]");
            Icons.Add("knight0", "[♞ ]");
            Icons.Add("knight1", "[♘ ]");
            Icons.Add("rook0", "[♜ ]");
            Icons.Add("rook1", "[♖ ]");
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

        bool CanRook(Step step)
        {
            var name = Desktop[step.From.x, step.From.y].GetName();
            if (name != "rook0" && name != "rook1")
                return true;
            int sy = 0;
            var dy = step.GetDy();
            if (dy != 0) 
                sy = dy / Math.Abs(dy);
            int sx = 0;
            var dx = step.GetDx();
            if (dx != 0) 
                sx = dx / Math.Abs(dx);
            if (step.GetDx() == 0)
            {
                for (int i = (int)step.From.y + sy; i != step.To.y; i += sy)
                    if (Desktop[step.From.x, i].Empty())
                        continue;
                    else
                        return false;
            }
            else if (dy == 0)
            {
                for (int z = (int)step.From.x + sx; z != step.To.x; z += sx)
                    if (Desktop[z, step.From.y].Empty())
                        continue;
                    else
                        return false;
            }
            
            return true;
        }
        bool CanQueen(Step move)
        {
            var name = Desktop[move.From.x, move.From.y].GetName();
            if (name != "queen0" && name != "queen1")
                return true;
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

        bool CanBishop(Step move)
        {
            var name = Desktop[move.From.x, move.From.y].GetName();
            if (name != "bishop0" && name != "bishop1")
                return true;
            var dx = move.GetDx();
            var dy = move.GetDy();
            int sx = 0;
            if (dx != 0) sx = dx / Math.Abs(dx);
            int sy = 0;
            if (dy != 0) sy = dy / Math.Abs(dy);
            for (int x = (int)move.From.x + sx, y = (int)move.From.y + sy;
                x != move.To.x; x += sx, y += sy)
            {
                if (Desktop[x, y].Empty())
                    continue;
                else
                    return false;
            }
            return true;
        }

        bool SpecOnesMoves(Step move)
        {
            return
                CanPawn(move) && CanQueen(move) &&
                CanRook(move) && CanBishop(move);
        }
        public bool Move(Step move)
        {
            if (Desktop[move.From.x, move.From.y].CanInternalMove(move))
            {
                if (bDifferentSides(move) != true ||
                    SpecOnesMoves(move) != true) // жесткий костыль
                {
                    Console.Clear(); //rewrite this
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
