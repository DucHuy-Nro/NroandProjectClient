using System.Collections;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Native
{
    internal class MainLua
    {
        internal static IEnumerator LuaDownloadHandler()
        {
            return LuaNativeMenuBuilder.getLuaScripts();
        }
    }
}
