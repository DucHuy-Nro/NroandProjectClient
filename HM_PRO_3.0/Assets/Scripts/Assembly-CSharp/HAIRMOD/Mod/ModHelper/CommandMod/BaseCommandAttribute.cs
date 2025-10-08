using System;
namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.ModHelper.CommandMod
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class BaseCommandAttribute : Attribute
    {
        public char delimiter = ' ';
    }
}
