using System;
using UnityEngine;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Logger
{
    internal static class ConsoleLogger
    {
        internal static void LogError(string message, string htmlColor)
        {
            Debug.LogError($"<color={htmlColor}>{message}</color>");
        }
        internal static void Log(string message, string htmlColor)
        {
            Debug.Log($"<color={htmlColor}>{message}</color>");
        }
        internal static void LogException(Exception ex, string htmlColor)
        {
            Debug.LogError($"<color={htmlColor}>{ex.Message}</color>");
        }

        internal static void writeLine(string v)
        {
            throw new NotImplementedException();
        }
    }
}
