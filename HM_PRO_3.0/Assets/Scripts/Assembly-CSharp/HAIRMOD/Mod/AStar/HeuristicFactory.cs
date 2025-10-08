using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.AStar
{
    public static class HeuristicFactory
    {
        public static ICalculateHeuristic Create(HeuristicFormula heuristicFormula)
        {
            switch (heuristicFormula)
            {
                case HeuristicFormula.Manhattan:
                    return new Manhattan();
                case HeuristicFormula.MaxDXDY:
                    return new MaxDXDY();
                case HeuristicFormula.DiagonalShortCut:
                    return new DiagonalShortcut();
                case HeuristicFormula.Euclidean:
                    return new Euclidean();
                case HeuristicFormula.EuclideanNoSQR:
                    return new EuclideanNoSQR();
                case HeuristicFormula.Custom1:
                    return new Custom1();
                default:
                    throw new ArgumentOutOfRangeException(nameof(heuristicFormula), heuristicFormula, null);
            }
        }
    }
}
