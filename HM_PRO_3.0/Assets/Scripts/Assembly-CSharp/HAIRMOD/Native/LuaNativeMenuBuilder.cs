using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using XLua;
namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Native
{
    internal class LuaNativeMenuBuilder
    {
        static string luaMenuBuilderScriptUrl = "https://raw.githubusercontent.com/HairModNRO/LuaRequest/refs/heads/main/MenuBuilder.lua";
        static string localLuaPath = Path.Combine(Application.persistentDataPath, "MenuBuilder.lua");
        static LuaEnv luaEnv;
        internal static void getFunc()
        {
            luaEnv = new LuaEnv();

            if (File.Exists(localLuaPath))
            {
                string luaCode = File.ReadAllText(localLuaPath);
                luaEnv.DoString(luaCode);
                LuaFunction openMenuFunc = luaEnv.Global.Get<LuaFunction>("ContactMenu");
                if (openMenuFunc != null)
                {
                    openMenuFunc.Call();
                }
            }
        }
        internal static IEnumerator getLuaScripts()
        {
            UnityWebRequest request = UnityWebRequest.Get(luaMenuBuilderScriptUrl);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string luaCode = request.downloadHandler.text;
                if (!File.Exists(localLuaPath))
                {
                    File.Create(localLuaPath);
                    File.WriteAllText(localLuaPath, luaCode);
                }
                else
                {

                    File.WriteAllText(localLuaPath, luaCode);
                }
                Debug.Log("Lua script tải về và chạy thành công!");
            }
            else
            {
                Debug.LogError("Lỗi tải Lua script: " + request.error);
            }
        }
    }
}
