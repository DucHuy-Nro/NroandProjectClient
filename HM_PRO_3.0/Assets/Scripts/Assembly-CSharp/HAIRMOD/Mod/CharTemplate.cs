using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Menu;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.ModHelper.CommandMod.Chat;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.ModHelper.CommandMod.Hotkey;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Object;
using Assets.src.e;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod
{
    internal class CharTemplate
    {
        internal static bool _show_char_info { get; set; }
        internal static bool _show_pet_info { get; set; }
        internal static bool _show_list_char { get; set; }
        internal static bool _auto_revive { get; set; }
        internal static bool _auto_login { get; set; }
        internal static bool _Caro_Inventory { get; set; }

        internal static bool _show_buttons { get; set; } = true;

        internal static string _Acc;

        internal static string _Pass;

        internal static int _Server = -1;

        internal static List<Char> _List_Chars = new List<Char>();

        internal static int _X, _Y;

        internal static SButton capsule = new SButton("", 1088, () => useCapsule()); 
        internal static SButton porata = new SButton("", 7993, () => usePorata()); 
        internal static SButton khu = new SButton("Khu", -1,  () => menuZone()); 


        internal class CharTemplateChatable : IActionListener, IChatable
        {
            public void onCancelChat() => ChatTextField.gI().ResetTF();

            public void onChatFromMe(string text, string to)
            {
                if (ChatTextField.gI().tfChat.name == "Tốc độ game")
                {
                    if (float.TryParse(text, out float timeout))
                    {
                        Time.timeScale = timeout;
                        GameScr.info1.addInfo("Tốc độ game" + ": " + Time.timeScale.ToString().Replace(",", "."), 0);

                    }
                    else
                    {
                        GameCanvas.startOKDlg(Strings.invalidValue + '!');
                        return;
                    }
                    onCancelChat();
                }
                else if (ChatTextField.gI().tfChat.name == "Tốc độ chạy")
                {
                    if (int.TryParse(text, out int timeout))
                    {
                        Char.myCharz().cspeed = timeout;
                        GameScr.info1.addInfo("Tốc độ chạy" + ": " + Char.myCharz().cspeed, 0);

                    }
                    else
                    {
                        GameCanvas.startOKDlg(Strings.invalidValue + '!');
                        return;
                    }
                    onCancelChat();
                }
            }

            public void perform(int idAction, object p)
            {
                switch (idAction)
                {
                    case 1:
                        ChatTextField.gI().strChat = "Tốc độ game";
                        ChatTextField.gI().tfChat.name = "Tốc độ game";
                        ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                        ChatTextField.gI().startChat2(this, string.Empty);
                        break;
                    case 2:
                        ChatTextField.gI().strChat = "Tốc độ chạy";
                        ChatTextField.gI().tfChat.name = "Tốc độ chạy";
                        ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                        ChatTextField.gI().startChat2(this, string.Empty);
                        break;
                }
            }
        }
        internal static void Paint(mGraphics g)
        {
            mFont.tahoma_7b_white.drawStringBd(g, NinjaUtil.getMoneys(Char.myCharz().cHP), 85, 4, 0, mFont.tahoma_7_grey);
            mFont.tahoma_7b_white.drawStringBd(g, NinjaUtil.getMoneys(Char.myCharz().cMP), 85, 17, 0, mFont.tahoma_7_grey);
            mFont.tahoma_7_white.drawStringBd(g, $"{TileMap.mapName}[{TileMap.zoneID}]", 85, 30, mFont.LEFT, mFont.tahoma_7_grey);

        if (LoginScr.imgTitle != null)
                g.drawImage(LoginScr.imgTitle, GameCanvas.hw, 20, 3);
                
            Paint_Char_Info(g);
            Paint_Pet_Info(g);
            Paint_List_Chars(g);
            PaintButton(g);
        }
        static void Paint_Char_Info(mGraphics g)
        {
            if (!_show_char_info) return;
            List<string> info = new List<string>();
            string[] name = new string[2] { "Sư Phụ: ", Char.myCharz().cName };
            string[] colors = new string[2] { "cyan", "red" };
            info.Add(Utils.RichText(name.ToList(), colors.ToList()));
            info.Add($"SM: {NinjaUtil.getMoneys(Char.myCharz().cPower)}");
            info.Add($"TN: {NinjaUtil.getMoneys(Char.myCharz().cTiemNang)}");
            info.Add($"HP: {NinjaUtil.getMoneys(Char.myCharz().cHP)} / {NinjaUtil.getMoneys(Char.myCharz().cHPFull)}");
            info.Add($"KI: {NinjaUtil.getMoneys(Char.myCharz().cMP)} / {NinjaUtil.getMoneys(Char.myCharz().cMPFull)}");
            info.Add($"SD: {NinjaUtil.getMoneys(Char.myCharz().cDamFull)}");
            info.Add($"Giáp: {NinjaUtil.getMoneys(Char.myCharz().cDefull)} - CM: {Char.myCharz().cCriticalFull}");
            mFont mFont = mFont.tahoma_7;
            int totalHeight = info.Count * (mFont.getHeight() + 10);
            int startY = (GameCanvas.hh - totalHeight) / 2;

            for (int i = 0; i < info.Count; i++)
            {
                mFont.drawString(g, info[i], 50, startY + 10 + GameCanvas.hh / 2 + i * (mFont.getHeight()), mFont.LEFT);
            }
        }
        static void Paint_Pet_Info(mGraphics g)
        {
            if (!_show_pet_info) return;
            if (GameCanvas.gameTick % (int)(10 * Time.timeScale) == 0)
            {
                Service.gI().petInfo();
            }
            List<string> info = new List<string>();
            string[] name = new string[2] { "Đệ Tử: ", Char.myPetz().cName.Replace("$", "") };
            string[] colors = new string[2] { "cyan", "red" };
            info.Add(Utils.RichText(name.ToList(), colors.ToList()));
            info.Add($"SM: {NinjaUtil.getMoneys(Char.myPetz().cPower)}");
            info.Add($"TN: {NinjaUtil.getMoneys(Char.myPetz().cTiemNang)}");
            info.Add($"HP: {NinjaUtil.getMoneys(Char.myPetz().cHP)} / {NinjaUtil.getMoneys(Char.myPetz().cHPFull)}");
            info.Add($"KI: {NinjaUtil.getMoneys(Char.myPetz().cMP)} / {NinjaUtil.getMoneys(Char.myPetz().cMPFull)}");
            info.Add($"SD: {NinjaUtil.getMoneys(Char.myPetz().cDamFull)}");
            info.Add($"Giáp: {NinjaUtil.getMoneys(Char.myPetz().cDefull)} - CM: {Char.myPetz().cCriticalFull}");
            mFont mFont = mFont.tahoma_7;
            int totalHeight = info.Count * (mFont.getHeight() + 10);
            int startY = (GameCanvas.hh - totalHeight) / 2;

            for (int i = 0; i < info.Count; i++)
            {
                mFont.drawString(g, info[i], 50 + GameCanvas.hw / 2, startY + 10 + GameCanvas.hh / 2 + i * (mFont.getHeight()), mFont.LEFT);
            }
        }
        static void Paint_List_Chars(mGraphics g)
        {
            if (!_show_list_char || _List_Chars.Count == 0) return;
            _Y = HM9r329.rectBossChar ? 130 :  95;
            int maxWidth = 0;
            int totalHeight = _List_Chars.Count * (mFont.tahoma_7_white.getHeight() + 2);

            for (int i = 0; i < _List_Chars.Count; i++)
            {
                var ch = _List_Chars[i];
                Func<string> charColor = () =>
                {
                    if (ch.isBOSS()) return "yellow";
                    else return "white";
                };
                Func<string> genderName = () =>
                {
                    if (ch.isBOSS()) return "";
                    else if (ch.cgender == 0) return "TĐ";
                    else if (ch.cgender == 1) return "NM";
                    else return "XD";
                };
                string[] str = new string[]
                {
                    (i + 1).ToString() +
                    ". " +
                    genderName() +
                    (ch.isBOSS() ?  "" : ": "),
                    ch.cName,
                    " [",
                    NinjaUtil.getMoneys(ch.cHP),
                    $"/{mSystem.numberTostring(ch.cHPFull)} ]"

                };
                string[] color = new string[]
                {
                    "black",
                   (Char.myCharz().charFocus == ch)  ? "cyan" : charColor() ,
                    "black",
                    "red",
                    "black"
                };
                int textWidth = mFont.tahoma_7_white.getWidth(Utils.RichText(str.ToList(), color.ToList()));
                if (textWidth > maxWidth)
                {
                    maxWidth = textWidth;
                }
            }

            _X = maxWidth + 16;
            int rectX = GameCanvas.w - _X - 12;
            int rectY = _Y - 10;
            int rectHeight = totalHeight + 8;
            if (HM9r329.rectBossChar)
            {
                g.setColor(16713728);
                g.drawRect(rectX, rectY, GameCanvas.w - rectX, rectHeight);
                string title = $"{TileMap.mapName} - K{TileMap.zoneID}";
                int bW = mFont.tahoma_7.getWidth(title + 10);
                int bH = 10;
                int bX = GameCanvas.w - 5 - bW;
                int bY = rectY - bH / 2 + 2;
                g.setColor(0);
                g.fillRect(bX, bY, bW, bH);
                mFont.tahoma_7_white.drawStringBd(g, title, bX + bW / 2, bY + bH / 2 - mFont.tahoma_7_white.getHeight() / 2 - 1, 3, mFont.tahoma_7);
            }
            for (int i = 0; i < _List_Chars.Count; i++)
            {
                var ch = _List_Chars[i];
                PaintFlag(ch, g, GameCanvas.w - _X - 9, _Y + 2);
                Func<string> charColor = () =>
                {
                    if (ch.isBOSS()) return "yellow";
                    else return "white";
                };
                Func<string> genderName = () =>
                {
                    if (ch.isBOSS()) return "";
                    else if (ch.cgender == 0) return "TĐ";
                    else if (ch.cgender == 1) return "NM";
                    else return "XD";
                };
                string[] str = new string[]
                {
                    (i + 1).ToString() +
                    ". " +
                    genderName() +
                     (ch.isBOSS() ?  "" : ": "),
                    ch.cName,
                    " [",
                    NinjaUtil.getMoneys(ch.cHP),
                    $"/{mSystem.numberTostring(ch.cHPFull)} ]"

                };
                string[] color = new string[]
                {
                    "black",
                      (Char.myCharz().charFocus == ch)  ? "cyan" : charColor() ,
                    "black",
                    "red",
                    "black"
                };
                var genderFont = mFont.tahoma_7;
                g.setColor(0, 0.15f);
                g.fillRect(GameCanvas.w - _X, _Y + 12 * i, GameCanvas.w, genderFont.getHeight() - 1);
                genderFont.drawString(g, Utils.RichText(str.ToList(), color.ToList()), GameCanvas.w - _X + 5, _Y - 2 + 12 * i, mFont.LEFT);
            }
        }
        static void PaintFlag(Char ch, mGraphics g, int x, int y)
        {

            int[] colors = { 7468284, 16711680, 10895840, 14925336, 6406954, 15163872, 15362839, 0 };
            if (ch.cFlag >= 1 && ch.cFlag <= 8)
            {
                g.setColor(colors[ch.cFlag - 1]);
                g.fillRect(x, y, 7, 7);
            }
        }

        static void PaintButton(mGraphics g)
        {
            if (!_show_buttons || GameScr.isAnalog != 1) return;
            capsule.x = GameScr.xF - 5 - GameScr.imgNut.getWidth();
            capsule.y = GameScr.yF - GameScr.imgNut.getHeight() - 5;
            capsule.Paint(g);
            porata.x = GameScr.xF - 15 - GameScr.imgNut.getWidth() * 2;
            porata.y = GameScr.yF - 5;
            porata.Paint(g);
            khu.x = GameScr.xF - 10 - GameScr.imgNut.getWidth() * 2;
            khu.y = GameScr.yF - GameScr.imgNut.getHeight() - 7;
            khu.Paint(g);
        }
        static void updateTouchButton()
        {
            if (!_show_buttons || GameScr.isAnalog != 1) return;
            if (capsule != null && capsule.Pressed()) capsule.actionPerform();
            if (porata != null && porata.Pressed()) porata.actionPerform();
            if (khu != null && khu.Pressed()) khu.actionPerform();
        }
        internal static void _Auto_Revive()
        {
            if (!_auto_revive) return;
            if (Char.myCharz().meDead && Char.myCharz().cHP == 0)
            {
                if ((Char.myCharz().luong + Char.myCharz().luongKhoa) > 0)
                {
                    if ((GameCanvas.gameTick % (int)(15 * Time.timeScale)) == 0)
                    {
                        Service.gI().wakeUpFromDead();

                    }
                }
                else
                {
                    GameScr.info1.addInfo("Hết ngọc!", 0);
                    _auto_revive = false;

                }
            }
        }
        internal static System.Collections.IEnumerator _Auto_Login()
        {
            if (!_auto_login) yield break;
            while (GameCanvas.currentScreen is ServerListScreen || GameCanvas.currentScreen is LoginScr)
            {
                if (GameCanvas.loginScr == null)
                {
                    GameCanvas.loginScr = new LoginScr();
                }

                _Acc = _Acc != null && _Acc != string.Empty ? _Acc : GameCanvas.loginScr.tfUser.getText();
                _Pass = _Pass != null && _Pass != string.Empty ? _Pass : GameCanvas.loginScr.tfPass.getText();
                _Server = _Server != -1 ? _Server : ServerListScreen.ipSelect;

                if (_Acc != string.Empty && _Pass != string.Empty && _Server != -1)
                {
                    yield return new WaitForSecondsRealtime(20f);
                    if (GameCanvas.currentScreen == GameCanvas.loginScr || GameCanvas.currentScreen == GameCanvas.serverScreen)
                    {

                        if (_Server != ServerListScreen.ipSelect)
                        {
                            GameCanvas.serverScr.perform(100 + _Server, null);
                            yield return new WaitForSecondsRealtime(0.5f);
                        }

                        if (Session_ME.gI().isConnected() == false)
                            GameCanvas.connect();

                        if (GameCanvas.loginScr == null)
                        {
                            GameCanvas.loginScr = new LoginScr();
                        }
                        GameCanvas.loginScr.switchToMe();
                        Service.gI().login(_Acc, _Pass, GameMidlet.VERSION, (sbyte)0);
                    }
                }
                else
                {
                    if ((GameCanvas.currentScreen == GameCanvas.loginScr || GameCanvas.currentScreen == GameCanvas.serverScreen) &&
                        Rms.loadRMSString("acc") != string.Empty)
                    {
                        GameCanvas.serverScreen.perform(3, null);
                    }
                }
            }
        }
        internal static void Update()
        {
            _Auto_Revive();
            if (!_show_list_char) return;
            _List_Chars.Clear();
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char ch = (Char)(GameScr.vCharInMap.elementAt(i));
                if (ch != null && !ch.isPet && !ch.isMiniPet && !string.IsNullOrEmpty(ch.cName) && !ch.cName.StartsWith("#") && !ch.cName.StartsWith("$") && ch.cName != "Trọng tài")
                    _List_Chars.Add(ch);
            }
            //Check();
        }
        static MenuBoolean[] getMenuBooleans()
        {
            return new MenuBoolean[]
            {
                new MenuBoolean(new MenuBooleanConfig()
                {
                    Title = "T.Tin S.Phụ",
                    GetValueFunc = () => _show_char_info,
                    SetValueAction = value => _show_char_info = value,
                    RMSName = nameof(_show_char_info)
                }),
                new MenuBoolean(new MenuBooleanConfig()
                {
                    Title = "T.Tin Đệ Tử",
                    GetValueFunc = () => _show_pet_info,
                    SetValueAction = value => _show_pet_info = value,
                    GetIsDisabled = () => !Char.myCharz().havePet,
                    GetDisabledReason = () => "Đéo có đệ tử",
                    RMSName = nameof(_show_pet_info)
                }),
                new MenuBoolean(new MenuBooleanConfig()
                {
                    Title = "Danh Sách N.Vật",
                    GetValueFunc = () => _show_list_char,
                    SetValueAction = value => _show_list_char = value,
                    RMSName = nameof(_show_list_char)
                })
            };
        }
        internal static MenuBoolean[] getTemplateMenu()
        {
            return new MenuBoolean[]
            {
                new MenuBoolean(new MenuBooleanConfig()
                {
                      Title = "Auto H.Sinh",
                    GetValueFunc = () => _auto_revive,
                    SetValueAction = value => _auto_revive = value,
                    RMSName = nameof(_auto_revive)
                }),
                  new MenuBoolean(new MenuBooleanConfig()
                {
                      Title = "Auto Login",
                    GetValueFunc = () => _auto_login,
                    SetValueAction = value => _auto_login = value
                }),
                  new MenuBoolean(new MenuBooleanConfig()
                {
                      Title = "Hành Trang Lưới",
                    GetValueFunc = () => _Caro_Inventory,
                    SetValueAction = value => _Caro_Inventory = value,
                    RMSName = nameof(_Caro_Inventory)
                })
            };
        }
        internal static void loadData()
        {
            foreach (var menu in getMenuBooleans())
            {
                if (menu is not null)
                {
                    menu.loadData();
                }
            }
            foreach (var menu in getTemplateMenu())
            {
                if (menu is not null) menu.loadData();
            }
        }
        internal static void showMenu()
        {
            MyVector myVector = new MyVector();
            foreach (var menu in getMenuBooleans())
            {
                if (menu is not null)
                {
                    myVector.addElement(new Command(Utils.GetToggleStringMenuOutRef(menu.Title, menu.GetValueFunc()), () => menu.action()));
                }
            }
            GameCanvas.menu.startAt(myVector, 0);
        }
        internal static void updateTouch()
        {
            updateTouchButton();
            if (!_show_list_char || _List_Chars.Count == 0) return;
            for (int i = 0; i < _List_Chars.Count; i++)
            {
                if (GameCanvas.isPointerHoldIn(GameCanvas.w - _X, _Y + 12 * i, GameCanvas.w, mFont.tahoma_7.getHeight() - 1))
                {
                    GameCanvas.isPointerJustDown = false;
                    GameScr.gI().isPointerDowning = false;
                    if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
                    {
                        Char @char = _List_Chars[i];
                        if (Char.myCharz().charFocus != null && Char.myCharz().charFocus.cName == @char.cName)
                        {
                            Utils.TeleportMyChar(Char.myCharz().charFocus.cx, Char.myCharz().charFocus.cy);
                        }
                        else
                        {
                            Char.myCharz().focusManualTo(@char);
                        }
                        Char.myCharz().currentMovePoint = null;
                        GameCanvas.clearAllPointerEvent();
                        return;
                    }
                }
            }
        }
        internal static sbyte getIndexItemBag(params short[] templatesId)
        {
            var myChar = Char.myCharz();
            int length = myChar.arrItemBag.Length;
            for (sbyte i = 0; i < length; i++)
            {
                var item = myChar.arrItemBag[i];
                if (item != null && templatesId.Contains(item.template.id))
                {
                    return i;
                }
            }

            return -1;
        }


        [HotkeyCommand('f'), ChatCommand("b")]
        internal static void usePorata()
        {
            var index = getIndexItemBag(1411, 921, 454);
            if (index == -1)
            {
                GameScr.info1.addInfo("Không tìm thấy bông tai", 0);
                return;
            }

            Service.gI().useItem(0, 1, index, -1);
        }
        [HotkeyCommand('c'), ChatCommand("c")]
        internal static void useCapsule()
        {
            var index = getIndexItemBag(194, 193);
            if (index == -1)
            {
                GameScr.info1.addInfo("Không tìm thấy capsule", 0);
                return;
            }

            Service.gI().useItem(0, 1, index, -1);
        }
        [HotkeyCommand('w')]
        internal static void KhinhCong()
        {
            Char.myCharz().cy -= 50;
            Service.gI().charMove();
        }

        [HotkeyCommand('s')]
        internal static void DonTho()
        {
            Char.myCharz().cy += 50;
            Service.gI().charMove();
        }

        [HotkeyCommand('a')]
        internal static void DichTrai()
        {
            Char.myCharz().cx -= 50;
            Service.gI().charMove();
        }

        [HotkeyCommand('d')]
        internal static void DichPhai()
        {
            Char.myCharz().cx += 50;
            Service.gI().charMove();
        }


        [HotkeyCommand('m')]
        internal static void menuZone()
        {
            Service.gI().openUIZone();
            GameCanvas.panel.setTypeZone();
            GameCanvas.panel.show();
        }


    }
}
