using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.AStar
{
    public class MaxDXDY : ICalculateHeuristic
    {
        public int Calculate(Position source, Position destination)
        {
            var heuristicEstimate = 2;
            var h = heuristicEstimate * (System.Math.Max(System.Math.Abs(source.Row - destination.Row), System.Math.Abs(source.Column - destination.Column)));
            return h;
        }
    }
}
