﻿using System;
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
        //private constructor against creating without args
        Step() { }

    }
    class Board
    {
        public Board()
        {
            Desktop = new ChessUnit[8, 8];
            History = new List<Step>();
            //initialize coords via units
            //TODO
            //temp movement
            for (int i = 0; i < 8; i++)
                for(int j = 0; j < 8; j++)
                {
                    Desktop[i, j] = new EmptyUnit();
                }
            Icons.Add("empty", "[ ]");
        }
        List<Step> History;
        ChessUnit[,] Desktop;
        private Dictionary<string, string> Icons =
            new Dictionary<string, string>();
        void SetAt(Step move)
        {
            //TODO
        }

        public bool Move(Step move)
        {
            return true;
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
        }
    }

    
    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Board tmpBrd = new Board();
            tmpBrd.DrawDesktop();
        }
    }
}