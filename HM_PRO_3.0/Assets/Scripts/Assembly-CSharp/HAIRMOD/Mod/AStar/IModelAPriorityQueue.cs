using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.AStar
{
    internal interface IModelAPriorityQueue<T>
    {
        int Push(T item);
        T Pop();
        T Peek();

        void Clear();
        int Count { get; }
    }
}
