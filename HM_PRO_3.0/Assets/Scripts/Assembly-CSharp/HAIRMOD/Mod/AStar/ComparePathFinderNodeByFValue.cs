using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.AStar
{
    internal class ComparePathFinderNodeByFValue : IComparer<PathFinderNode>
    {
        public int Compare(PathFinderNode a, PathFinderNode b)
        {
            if (a.F > b.F)
            {
                return 1;
            }

            if (a.F < b.F)
            {
                return -1;
            }

            return 0;
        }
    }
}
