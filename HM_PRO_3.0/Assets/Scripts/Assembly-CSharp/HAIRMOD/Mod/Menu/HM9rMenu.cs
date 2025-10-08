
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Auto;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Graphics;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.ModHelper.CommandMod.Hotkey;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.PickMob;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Xmap;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Native;
using UnityEngine;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Menu
{
    internal class HM9rMenu
    {
        internal static bool showPopup = false;
        static int _X = 20, _Y = GameCanvas.h - 60 - (GameCanvas.hh / 2 + 30), _W = GameCanvas.w - 40, _H = GameCanvas.hh / 2 + 30;
        static Image imgSettings;
        static HM9rMenu()
        {
            imgSettings ??= Image.createImage("res/x4/mainImage/myTexture2dSettings");
            if (mGraphics.zoomLevel != 4) Utils.resizeImage(imgSettings, 4);
        }
        [HotkeyCommand('x')]
        internal static void ToggleMainMenu()
        {
            MyVector myVector = new MyVector();
            myVector.addElement(new Command("Hành Trang\nNhanh", () => CustomPanel.Show()));
            myVector.addElement(new Command("Xmap", () => HM9rX.ShowXmapMenu()));
            myVector.addElement(new Command("Tàn Sát", () => Hm9rPickMob.showBaseMenu()));
            myVector.addElement(new Command(Utils.GetToggleStringMenuOutRef("Tự Nhặt", Hm9rPickMob.IsAutoPickItems), () =>
            {
                Hm9rPickMob.IsAutoPickItems = !Hm9rPickMob.IsAutoPickItems;
                GameScr.info1.addInfo("Tự Nhặt: " + (Hm9rPickMob.IsAutoPickItems ? "ON" : "OFF"), 0);
            }));
            myVector.addElement(new Command(Utils.GetToggleStringMenuOutRef("Tự Đánh", AutoSendAttack.gI.IsActing), () => AutoSendAttack.toggleAutoAttack()));
            myVector.addElement(new Command("Đậu Thần", () => AutoPean.showMenu()));
            myVector.addElement(new Command("Đệ Tử", () =>
            {
                AutoTrainPet.showMenu();
                showPopup = true;
            }));
            myVector.addElement(new Command("Boss", () => Boss.showMenu()));
            myVector.addElement(new Command("Khác", () => ToggleMoreMenu()));
            GameCanvas.menu.startAt(myVector, 0);
        }
        internal static void ToggleMoreMenu()
        {
            MyVector myVector = new MyVector();
            myVector.addElement(new Command("Thông tin", () => CharTemplate.showMenu()));
            foreach(var menu in CharTemplate.getTemplateMenu())
            {
                if(menu is not null)
                {
                    myVector.addElement(new Command(Utils.GetToggleStringMenuOutRef(menu.Title, menu.GetValueFunc()), () => menu.action()));
                }
            }
            myVector.addElement(new Command($"Cheat\n[{Time.timeScale.ToString().Replace(",", ".")}]", () => new CharTemplate.CharTemplateChatable().perform(1, null)));
            myVector.addElement(new Command(Utils.GetToggleStringMenuOutRef("Phím Tắt",CharTemplate._show_buttons), () =>
            {
                CharTemplate._show_buttons = !CharTemplate._show_buttons;
                GameScr.info1.addInfo("Phím Tắt: " + (CharTemplate._show_buttons ? "ON" : "OFF"), 0);
            }));
            myVector.addElement(new Command($"Tốc chạy\n[{Char.myCharz().cspeed}]", () => new CharTemplate.CharTemplateChatable().perform(2, null)));
            // myVector.addElement(new Command($"Liên Hệ", () =>
            // {
            //     LuaNativeMenuBuilder.getFunc();
            // }));
            GameCanvas.menu.startAt(myVector, 0);
        }
        internal static void paintPopup(mGraphics g)
        {
            if (!showPopup) return;
            PopUp.paintPopUp(g, _X, _Y, _W, _H, 16777215, false);
           
            mFont.tahoma_7b_red.drawStringBd(g, "Trạng Thái Auto Đệ Tử", _X + 60, _Y + 5, 3, mFont.tahoma_7);
            mFont.tahoma_7b_blue.drawStringBd(g, "Kiểu Đánh Đệ Khi Đệ Kêu (Cần Bật Auto Đệ Tử)", _X + _W - 100, _Y + 5, 3, mFont.tahoma_7);
            string[] states = new string[] { "Tắt Auto Đệ", "Auto Up Đệ Thường", "Auto Up Đệ Né Siêu Quái", "Auto Up Đệ Kaioken"}; 
            string[] states2 = new string[] { "Phang quái xa nhất", "Phang đệ", "Phang bản thân"}; 
            for(int i = 0; i < states.Length; i++)
            mFont.tahoma_7b_green.drawStringBd(g, i.ToString() + "." + states[i], _X + 60, _Y + 15 + 10 * i, 3, mFont.tahoma_7);
            for (int j = 0; j < states2.Length; j++)
                mFont.tahoma_7b_yellow.drawStringBd(g, j.ToString() + "." + states2[j], _X + _W - 100, _Y + 15 + j * 10, 3, mFont.tahoma_7);
        }
        internal static void Paint(mGraphics g)
        {
            if (imgSettings == null) return;
            int x = 160, y = 5, w = imgSettings.getWidth(), h = imgSettings.getHeight();
            g.drawImage(imgSettings, x, y);
            if (GameCanvas.isMouseFocus(x, y, w, h))
            {
                g.drawImage(ItemMap.imageFlare, x + w / 2, y + h / 2, 3);
            }
        }
        internal static void updateTouch()
        {
            if (imgSettings == null) return;
            int x = 160, y = 5, w =imgSettings.getWidth(), h = imgSettings.getHeight();
            if (GameCanvas.isPointerHoldIn(x, y, w, h))
            {
                if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
                {
                    ToggleMainMenu();
                    GameCanvas.clearAllPointerEvent();
                    return;
                }
            }
        }
    }
}
