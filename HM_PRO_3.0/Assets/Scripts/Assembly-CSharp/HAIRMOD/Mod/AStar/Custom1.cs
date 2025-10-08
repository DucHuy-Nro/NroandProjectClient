using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.AStar
{
    public class Custom1 : ICalculateHeuristic
    {
        public int Calculate(Position source, Position destination)
        {
            var heuristicEstimate = 2;
            var dxy = new Position(Math.Abs(destination.Row - source.Row), Math.Abs(destination.Column - source.Column));
            var Orthogonal = Math.Abs(dxy.Row - dxy.Column);
            var Diagonal = Math.Abs(((dxy.Row + dxy.Column) - Orthogonal) / 2);
            var h = heuristicEstimate * (Diagonal + Orthogonal + dxy.Row + dxy.Column);
            return h;
        }
    }
}
