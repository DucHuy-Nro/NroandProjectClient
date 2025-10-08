namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.AStar
{
    public interface ICalculateHeuristic
    {
        int Calculate(Position source, Position destination);
    }
}
