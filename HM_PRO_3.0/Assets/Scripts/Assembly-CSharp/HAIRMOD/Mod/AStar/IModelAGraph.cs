using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.AStar
{
    internal interface IModelAGraph<T>
    {
        bool HasOpenNodes { get; }
        IEnumerable<T> GetSuccessors(T node);
        T GetParent(T node);
        void OpenNode(T node);
        T GetOpenNodeWithSmallestF();
    }
}
