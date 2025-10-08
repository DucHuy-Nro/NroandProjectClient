using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.AStar
{
    public class Euclidean : ICalculateHeuristic
    {
        public int Calculate(Position source, Position destination)
        {
            var heuristicEstimate = 2;
            var h = (int)(heuristicEstimate * Math.Sqrt(Math.Pow((source.Row - destination.Row), 2) + Math.Pow((source.Column - destination.Column), 2)));
            return h;
        }
    }
}
