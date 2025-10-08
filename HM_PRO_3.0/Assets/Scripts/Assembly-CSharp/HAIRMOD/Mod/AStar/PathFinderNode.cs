using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.AStar
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal readonly struct PathFinderNode
    {
        /// <summary>
        /// The position of the node
        /// </summary>
        public Position Position { get; }

        /// <summary>
        /// Distance from home
        /// </summary>
        public int G { get; }

        /// <summary>
        /// Heuristic
        /// </summary>
        public int H { get; }

        /// <summary>
        /// This nodes parent
        /// </summary>
        public Position ParentNodePosition { get; }

        /// <summary>
        /// Gone + Heuristic (H)
        /// </summary>
        public int F { get; }

        /// <summary>
        /// If the node has been considered yet
        /// </summary>
        public bool HasBeenVisited => F > 0;

        public PathFinderNode(Position position, int g, int h, Position parentNodePosition)
        {
            Position = position;
            G = g;
            H = h;
            ParentNodePosition = parentNodePosition;

            F = g + h;
        }
    }
}
