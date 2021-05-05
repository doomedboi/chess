using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{

    abstract class ChessUnit
    {
        //TODO: wrapper this
        abstract public bool CanInternalMove(Step step);
        public virtual string GetName() { return name; }

        // 0 = black
        public int side = 0; // is it useless?
        protected string name;

    }

    class EmptyUnit : ChessUnit
    {
        public override bool CanInternalMove(Step step) { return false; }
        public EmptyUnit()
        {
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
            if (from.x == to.x) //dy = to - from
            {
                //classic move to one 
                if (Math.Abs(dy) == 1)
                    return true;
                //else if.... fisrt step to 2
            }
            // + if to coor is not empty - rubim
            else if (Math.Abs((int)(to.x - from.x)) == Math.Abs( 1 )
                && dy == Math.Abs(1))
                return true;
            return false;
        }

        public Pawn(int side) // 0 = black
        {
            name = "pawn" + side;
        }
    }

}
