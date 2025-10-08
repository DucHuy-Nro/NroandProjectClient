using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.ModHelper.CommandMod.Hotkey;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Object;
using System;
using Vietpad.InputMethod;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod
{
    internal class VietnameseInput
    {
        class VietnameseInputChatable : IChatable
        {
            public void onChatFromMe(string text, string to)
            {
                GameScr.info1.addInfo(text, 0);
                onCancelChat();
            }

            public void onCancelChat() => ChatTextField.gI().ResetTF();
        }

        static VietnameseInput()
        {
            VietKeyHandler.VietModeEnabled = false;
            VietKeyHandler.SmartMark = true;
        }
        [HotkeyCommand('v')]
        internal static void ShowMenu()
        {
            MyVector myVector = new MyVector();
            myVector.addElement(new Command(Strings.vnInputEnable + ": " + Strings.OnOffStatus(VietKeyHandler.VietModeEnabled), (() =>
               {
                   VietKeyHandler.VietModeEnabled = !VietKeyHandler.VietModeEnabled;
                   GameScr.info1.addInfo(Strings.vnInputEnable + ": " + Strings.OnOffStatus(VietKeyHandler.VietModeEnabled), 0);
               })));
            myVector.addElement(new Command(Strings.vnInputInputMethod + ": " + Enum.GetName(typeof(InputMethods), VietKeyHandler.InputMethod), (() =>
               {
                   VietKeyHandler.InputMethod++;
                   if (VietKeyHandler.InputMethod > InputMethods.Auto)
                       VietKeyHandler.InputMethod = InputMethods.Telex;
                   GameScr.info1.addInfo(Strings.vnInputInputMethod + ": " + Enum.GetName(typeof(InputMethods), VietKeyHandler.InputMethod), 0);
               })));
            myVector.addElement(new Command(Strings.vnInputDiacritics + ": " + (VietKeyHandler.DiacriticsPosClassic ? "òa, úy" : "oà, uý"), () =>
               {
                   VietKeyHandler.DiacriticsPosClassic = !VietKeyHandler.DiacriticsPosClassic;
                   GameScr.info1.addInfo(Strings.vnInputDiacritics + ": " + (VietKeyHandler.DiacriticsPosClassic ? "òa, úy" : "oà, uý"), 0);
               }));
            myVector.addElement(new Command(Strings.vnInputConsumeRepeatKey + ": " + Strings.OnOffStatus(VietKeyHandler.ConsumeRepeatKey), () =>
               {
                   VietKeyHandler.ConsumeRepeatKey = !VietKeyHandler.ConsumeRepeatKey;
                   GameScr.info1.addInfo(Strings.vnInputConsumeRepeatKey + ": " + Strings.OnOffStatus(VietKeyHandler.ConsumeRepeatKey), 0);
               }));
            myVector.addElement(new Command("Test", () =>
               {
                   ChatTextField.gI().strChat = "Test";
                   ChatTextField.gI().tfChat.name = "Test";
                   ChatTextField.gI().startChat2(new VietnameseInputChatable(), string.Empty);
               }));
            GameCanvas.menu.startAt(myVector, 0);
        }

        internal static void LoadData()
        {
            if (Utils.TryLoadDataBool("vn_input_enabled", out bool value))
                VietKeyHandler.VietModeEnabled = value;
            if (Utils.TryLoadDataLong("vn_input_input_method", out long value2))
                VietKeyHandler.InputMethod = (InputMethods)(int)value2;
            if (Utils.TryLoadDataBool("vn_input_diacritics", out bool value3))
                VietKeyHandler.DiacriticsPosClassic = value3;
            if (Utils.TryLoadDataBool("vn_input_consume_repeat_key", out bool value4))
                VietKeyHandler.ConsumeRepeatKey = value4;
        }

        internal static void SaveData()
        {
            Utils.SaveData("vn_input_enabled", VietKeyHandler.VietModeEnabled);
            Utils.SaveData("vn_input_input_method", (int)VietKeyHandler.InputMethod);
            Utils.SaveData("vn_input_diacritics", VietKeyHandler.DiacriticsPosClassic);
            Utils.SaveData("vn_input_consume_repeat_key", VietKeyHandler.ConsumeRepeatKey);
        }

        internal static bool ToVietnamese(string str, out string result, ref int caretPos, int inputType)
        {
            result = "";
            if (!VietKeyHandler.VietModeEnabled)
                return false;
            if (inputType != TField.INPUT_TYPE_ANY || str.StartsWith("/"))
                return false;
            result = VietKeyHandler.HandleTextInput(str, caretPos - 1);
            if (result != str)
            {
                caretPos -= str.Length - result.Length;
                return true;
            }
            return false;
        }

    }
}
