using UnityEngine.Networking;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Text;
using System;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Native;

public class WebIP : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(MainLua.LuaDownloadHandler());
        StartCoroutine(GetTextFromURL());

    }
    public static string Decrypt(string encryptedText, int key)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (char c in encryptedText)
        {
            stringBuilder.Append((char)(c - key));
        }

        byte[] bytes = Convert.FromBase64String(stringBuilder.ToString());
        return Encoding.UTF8.GetString(bytes);
    }

    IEnumerator GetTextFromURL()
    {
        UnityWebRequest request = UnityWebRequest.Get(Decrypt("씊쓱쓻쓙씌쓱쓶쓟쓵씢쓢씢씂씁씌씞씃쓛씕쓙씊쓱쓿씒씍씁쓷씕씌씖쓷씟씋씗쓻씕씋씗쓺씞씂쓛쓢씝쓵쓙씑씟씂씀쓞쓮쓽씒쓢쓬쓼쓮씐씟씌씖쓿씖씌씢쓢씘씃씀쓯씔씌씢쓢씝씂씀씕씞쓵쓙쓞씕씍씢쓾씢쓶쓯쓻씕씎쓱쓺씕쓶씓쓫쓮씋쓛쓷쓚씋씀쓿씞씍쓬쓞쓙씎쓱쓺쓦", -998231));
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            StartCoroutine(GetTextFromURL());
        }
        else
        {
            string responseText = request.downloadHandler.text;
            ServerListScreen.smartPhoneVN = responseText;
            if (ServerListScreen.smartPhoneVN != string.Empty)
            {
                SceneManager.LoadScene("NROL");
            }
            //Debug.LogError(responseText);
        }
    }
}