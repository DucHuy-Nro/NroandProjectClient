using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.AStar
{
    public class DiagonalShortcut : ICalculateHeuristic
    {
        public int Calculate(Position source, Position destination)
        {
            var hDiagonal = Math.Min(Math.Abs(source.Row - destination.Row),
                Math.Abs(source.Column - destination.Column));
            var hStraight = (Math.Abs(source.Row - destination.Row) + Math.Abs(source.Column - destination.Column));
            var heuristicEstimate = 2;
            var h = (heuristicEstimate * 2) * hDiagonal + heuristicEstimate * (hStraight - 2 * hDiagonal);
            return h;
        }
    }
}
