using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.AStar
{
    public static class PositionExtensions
    {
        public static Point ToPoint(this Position position)
        {
            return new Point(position.Column, position.Row);
        }

        public static Position ToPosition(this Point point)
        {
            return new Position(point.Y, point.X);
        }
    }
}
