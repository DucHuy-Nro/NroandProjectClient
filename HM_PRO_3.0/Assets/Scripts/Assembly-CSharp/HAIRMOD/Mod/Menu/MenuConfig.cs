using System;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Menu
{
    internal class MenuConfig
    {
        internal string ID { get; set; } = "";
        internal string Title { get; set; } = "";
        internal string Description { get; set; } = "";
        internal Func<bool> GetIsDisabled { get; set; }
        internal Func<string> GetDisabledReason { get; set; }
    }
}
