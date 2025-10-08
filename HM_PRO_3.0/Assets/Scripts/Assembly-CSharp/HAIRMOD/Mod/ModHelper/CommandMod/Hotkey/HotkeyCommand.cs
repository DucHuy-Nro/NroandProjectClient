using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.ModHelper.CommandMod.Hotkey
{
    public class HotkeyCommand : BaseCommand
    {
        public char key;
        public string fullCommand;

        [JsonIgnore]
        public object[] parameters;

        public void execute()
        {
            method.Invoke(null, parameters);
        }
    }
}
