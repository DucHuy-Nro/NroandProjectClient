using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Auto;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Graphics;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Menu;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.ModHelper.CommandMod.Chat;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.ModHelper.CommandMod.Hotkey;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Object;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.PickMob;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.ThreadAction;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Xmap;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Native;
using System;
using UglyBoy;
using UnityEngine;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod
{
    internal class HM9r329
    {

        internal static bool checkTypeData = true; //true là int, false là long
        internal static bool rectBossChar = true; //Viền đỏ
        internal static bool shopKiGui = false; //false is statement for v231
        internal static bool achievement = false; //false is statement for goldbar


        internal static void onUpdateMain()
        {
            try
            {
                MainThreadDispatcher.update();
                onInitCanvas();
                Main.main.StartCoroutine(CharTemplate._Auto_Login());
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
        }
        internal static void onGameStarted()
        {
            try
            {
                ChatCommandHandler.loadDefault();
                HotkeyCommandHandler.loadDefault();
                VietnameseInput.LoadData();
                if (!PlayerPrefs.HasKey("IsNewPlayer"))
                {
                    Utils.SaveData(nameof(CharTemplate._Caro_Inventory), true);
                    PlayerPrefs.SetInt("IsNewPlayer", 1); 
                    PlayerPrefs.Save();
                }
                else
                {
                    Debug.Log("Người chơi cũ. Không cần lưu RMS.");
                }
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
        }
        internal static void onGameClosing()
        {
            try
            {
                VietnameseInput.SaveData();
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
        }
        internal static void onUpdateGameScr()
        {
            try
            {
                if (Hm9rPickMob.IsTanSat)
                    GameScr.isAutoPlay = GameScr.canAutoPlay = false;
                Hm9rPickMob.update();
                AutoPean.Update();
                AutoTrainPet.Update();
                Boss.Update();
                CharTemplate.Update();
                AutoItem.Update();
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
        }
        internal static void OnAddInfoChar(Char c, string info)
        {
            try
            {
                if (LocalizedString.saoMayLuoiThe.ContainsReversed(info.ToLower()) && AutoTrainPet.Mode > AutoTrainPetMode.Disabled && c.charID == -Char.myCharz().charID)
                    AutoTrainPet.saoMayLuoiThe = true;
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
        }
        internal static bool onSendChat(string text)
        {
            bool result = ChatCommandHandler.handleChatText(text);
            return result;
        }
        internal static void onUpdateTouchGameScr()
        {
            try
            {
                AutoNoiTai.updateTouch();
                if (!GameCanvas.isTouch && (GameCanvas.panel.isShow || (GameCanvas.panel2 != null && GameCanvas.panel2.isShow) || GameCanvas.menu.showMenu || ChatTextField.gI().isShow)) return;
                HM9rMenu.updateTouch();
                CharTemplate.updateTouch();
                HM9rX.updateTouch();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        internal static void onInitCanvas()
        {
            try
            {
                Button.title ??= GameCanvas.loadImage("/board/title");
                Button.title_select ??= GameCanvas.loadImage("/board/title_select");
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
        }
        internal static void onPaintGameScr(mGraphics g)
        {
            try
            {
                HM9rMenu.paintPopup(g);

                AutoNoiTai.Paint(g);
                if (GameCanvas.panel.isShow || (GameCanvas.panel2 != null && GameCanvas.panel2.isShow) || GameCanvas.menu.showMenu || ChatTextField.gI().isShow) return;
                HM9rMenu.Paint(g);
                Boss.Paint(g);
                CharTemplate.Paint(g);
                HM9rX.Paint(g);
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
        }
        internal static void onLoadedData()
        {
            try
            {
                AutoPean.loadData();
                AutoTrainPet.loadData();
                Boss.loadData();
                CharTemplate.loadData();
                //HM9rMenu.loadData();
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
