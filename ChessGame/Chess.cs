using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{

    abstract class ChessUnit
    {
        public enum COLOR_OF_CHESS
        {
            WHITE,
            BLACK,
            NONE
        }
        public class Side
        {
            public Side(int side)
            {
                if (side == 0)
                    color = COLOR_OF_CHESS.BLACK;
                else if (side == 1)
                    color = COLOR_OF_CHESS.WHITE;
                else
                    color = COLOR_OF_CHESS.NONE;
            }
            public COLOR_OF_CHESS color;
        }

        public bool Empty() { return _side.color == COLOR_OF_CHESS.NONE; }
        //TODO: wrapper this
        abstract public bool CanInternalMove(Step step);
        public virtual string GetName() { return name; }

        public Side _side; 
        protected string name;
    }

    class EmptyUnit : ChessUnit
    {
        public override bool CanInternalMove(Step step) { return false; }
        public EmptyUnit(int side)
        {
            _side = new Side(side);
            name = "empty";
        }
    }

    class Pawn : ChessUnit
    {
        //TODO: check color of units or it musts do board?? I think 2 way
        public override bool CanInternalMove(Step step)
        {
            var from = step.From;
            var to = step.To;
            var dy = (int)(to.y - from.y);
            int comp = 1;
            if (from.x == to.x) //dy = to - from
            {
                //classic move to one
                if (_side.color != COLOR_OF_CHESS.WHITE)
                    comp = -1;
                if (dy == comp)
                    return true;
                //else if.... fisrt step to 2 or vzyatie na prohode
            }
            // + if to coor is not empty - rubim
            else if (Math.Abs((int)(to.x - from.x)) == Math.Abs( 1 )
                && Math.Abs( dy ) == Math.Abs(1))
                return true;
            return false;
        }

        public Pawn(int side) // 0 = black
        {
            _side = new Side(side);
            name = "pawn" + side;
        }
    }
    
    class Queen : ChessUnit 
    {
        public override bool CanInternalMove(Step step)
        {
            int dx = (int)(step.To.x - step.From.x);
            int dy = (int)(step.To.y - step.From.y);
            return true;
        }

        public Queen(int side)
        {
            _side = new Side(side);
            name = "queen" + side;
        }
    }

    class King : ChessUnit
    {
        public override bool CanInternalMove(Step step)
        {
            if (Math.Abs(step.To.x - step.From.x) < 2 &&
                Math.Abs(step.To.y - step.From.y) < 2)
                return true;
            else
                return false;
        }

        public King(int side)
        {
            _side = new Side(side);
            name = "king" + side;
        }
    }

    class Bishop : ChessUnit
    {
        public override bool CanInternalMove(Step step)
        {
            var dx = step.GetDx();
            var dy = step.GetDy();
            return Math.Abs(dx) == Math.Abs(dy);   
        }

        public Bishop(int side)
        {
            _side = new Side(side);
            name = "bishop" + side;
        }
    }

    class Knight : ChessUnit
    {
        public override bool CanInternalMove(Step step)
        {
            var dx = step.GetDx();
            var dy = step.GetDy();
            return Math.Pow((int)(step.From.x) - step.To.x, 2)
                + Math.Pow((int)(step.From.y) - step.To.y, 2) == 5;
        }

        public Knight(int side)
        {
            _side = new Side(side);
            name = "knight" + side;
        }
    }

    class Rook : ChessUnit
    {
        public override bool CanInternalMove(Step step)
        {
            return step.GetDx() * step.GetDy() == 0;
        }
        public Rook(int side)
        {
            _side = new Side(side);
            name = "rook" + side;
        }
    }

}
