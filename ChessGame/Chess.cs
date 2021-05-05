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
        public bool side = false;
        string name;

    }

    class EmptyUnit : ChessUnit
    {
        public override bool CanInternalMove(Step step) { return false; }
    }

}
