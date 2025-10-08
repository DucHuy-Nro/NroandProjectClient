using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.ModHelper.CommandMod.Hotkey
{
    public class HotkeyCommandAttribute : BaseCommandAttribute
    {
        public char key;
        public string agrs = "";

        public HotkeyCommandAttribute(char key)
        {
            this.key = key;
        }
    }
}
