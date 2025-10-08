using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Menu;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.ModHelper.CommandMod.Chat;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod
{
    internal class Boss
    {
        internal static bool _isShow { get; set; }
        internal static bool _LineToBoss { get; set; }
        internal static bool _FocusToBoss { get; set; }
        internal static List<Boss> _ListBosses { get; set; } = new List<Boss>();
        internal List<string> _ListName { get; set; } = new List<string>();
        internal string _Name { get; set; }
        internal string _MapName { get; set; }
        internal int _MapID { get; set; }
        internal DateTime _AppearTime { get; set; }
        internal Boss(string chatVip)
        {

            chatVip = chatVip.Replace("BOSS ", "").Replace(" vừa xuất hiện tại ", "|");
            string[] array = chatVip.Split('|');

            if (array.Length < 2)
            {
                Debug.LogError($"[Boss Constructor] Invalid chatVip format after replace: {chatVip}");
                return;
            }

            _Name = array[0].Trim();
            _MapName = array[1].Trim();
            _MapID = getMapID(_MapName);
            _AppearTime = DateTime.Now;
            Debug.Log($"[Boss Constructor] Created Boss: {_Name} at {_MapName}, Map ID: {_MapID}");
        }

        internal int getMapID(string mapName)
        {
            for (int i = 0; i < TileMap.mapNames.Length; i++)
            {
                if (TileMap.mapNames[i] == mapName)
                {
                    return i;
                }
            }
            return -1;
        }
        //[ChatCommand("test")]
        internal static void calledBoss()
        {
            for (int i = 0; i < 5; i++)
            {
                string notif = i <= 3 ? "BOSS HairMod vừa xuất hiện tại Làng Aru" : "Boss A vừa xuất hiện tại ABC";
                GameScr.gI().chatVip(notif);
            }
        }

        internal void paintBoss(mGraphics g, int x, int y, int anchor)
        {
            var subtractTime = DateTime.Now.Subtract(_AppearTime);
            int totalSeconds = (int)subtractTime.TotalSeconds;
            Func<bool> isBoss = () =>
            {
                for (int i = 0; i < GameScr.vCharInMap.size(); i++)
                {
                    if (((Char)GameScr.vCharInMap.elementAt(i)).cName.Equals(_Name))
                    {
                        return true;
                    }
                }
                return false;
            };
            mFont mFont = TileMap.mapID == _MapID ? mFont.tahoma_7_red : isBoss() ? mFont.tahoma_7_green : mFont.tahoma_7_yellow;
            string info = string.Concat(new string[]
            {
                _Name, " - ",
                _MapName, " - ",
                totalSeconds < 60 ? totalSeconds + "s" : subtractTime.Minutes + "p"
            });
            g.setColor(0, 0.15f);
            g.fillRect(x - mFont.getWidth(info) - 3, y, mFont.getWidth(info) + 6, mFont.getHeight() - 1);
            mFont.drawStringBd(g, info, x, y, anchor, mFont.tahoma_7_grey);
        }
        internal static void Paint(mGraphics g)
        {
            if (_LineToBoss)
            {
                for (int i = 0; i < GameScr.vCharInMap.size(); i++)
                {
                    Char @char = (Char)GameScr.vCharInMap.elementAt(i);
                    if (@char != null && !@char.isPet && !@char.isMiniPet && @char.charID < 0 && @char.cTypePk == 5 && !@char.cName.ToLower().Contains("trọng tài"))
                    {
                        g.setColor(16776960);
                        g.drawLine(Char.myCharz().cx - GameScr.cmx, Char.myCharz().cy - GameScr.cmy, @char.cx - GameScr.cmx, @char.cy - GameScr.cmy);
                    }
                }
            }
            if (!_isShow) return;
            try
            {
                if (_ListBosses.Count == 0) return;
                int x = GameCanvas.w - 7;
                int y = HM9r329.rectBossChar ? 52 : 42;
                if (HM9r329.rectBossChar)
                {
                    int maxWidth = 0;
                    int totalHeight = 0;
                    foreach (var boss in _ListBosses)
                    {
                        if (boss != null)
                        {
                            var subtractTime = DateTime.Now.Subtract(boss._AppearTime);
                            int totalSeconds = (int)subtractTime.TotalSeconds;
                            string info = string.Concat(new string[]
                            {
                        boss._Name, " - ",
                        boss._MapName, " - ",
                        totalSeconds < 60 ? totalSeconds + "s" : subtractTime.Minutes + "p"
                            });

                            int textWidth = mFont.tahoma_7_yellow.getWidth(info);
                            if (textWidth > maxWidth)
                            {
                                maxWidth = textWidth;
                            }
                        }
                    }
                    totalHeight = _ListBosses.Count * (mFont.tahoma_7_white.getHeight());
                    int rectX = x - maxWidth - 8;
                    int rectY = y - 10;
                    int rectWidth = GameCanvas.w - rectX;
                    int rectHeight = totalHeight + 10;
                    g.setColor(16713728);
                    g.drawRect(rectX, rectY, rectWidth, rectHeight);

                    int bW = 70;
                    int bH = 10;
                    int bX = GameCanvas.w - 5 - bW;
                    int bY = 42 - bH / 2 + 2;
                    g.setColor(0);
                    g.fillRect(bX, bY, bW, bH);
                    mFont.tahoma_7_white.drawStringBd(g, "Thông báo Boss", bX + bW / 2, bY + bH / 2 - mFont.tahoma_7_white.getHeight() / 2 - 1, 3, mFont.tahoma_7);
                    g.reset();
                }
                for (int i = 0; i < _ListBosses.Count; i++)
                {
                    Boss boss = _ListBosses[i];
                    if (boss != null)
                    {
                        boss.paintBoss(g, x, y + (i * 10), mFont.RIGHT);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        internal static void Update()
        {
            if (!_FocusToBoss) return;
            for (int i = 0; i < GameScr.vCharInMap.size(); i++)
            {
                Char @char = (Char)GameScr.vCharInMap.elementAt(i);
                if (@char != null && !@char.isPet && !@char.isMiniPet && @char.charID < 0 && @char.cTypePk == 5 && !@char.cName.ToLower().Contains("trọng tài"))
                {
                    Char.myCharz().focusManualTo(@char);
                    break;
                }
            }

        }
        static MenuBoolean[] getMenuBooleans()
        {
            return new MenuBoolean[]
            {
                new MenuBoolean(new MenuBooleanConfig()
                {
                    Title = "Thông báo BOSS",
                    GetValueFunc = () => _isShow,
                    SetValueAction = value => _isShow = value,
                    RMSName = "boss_notif",
                }),
                new MenuBoolean(new MenuBooleanConfig()
                {
                    Title = "Kẻ Tới Boss",
                    GetValueFunc = () => _LineToBoss,
                    SetValueAction = value => _LineToBoss = value,
                    RMSName = "line_to_boss"
                }),
                new MenuBoolean(new MenuBooleanConfig()
                {
                    Title = "Focus Tới Boss",
                    GetValueFunc = () => _FocusToBoss,
                    SetValueAction = value => _FocusToBoss = value,
                    RMSName = "focus_to_boss"
                })
            };
        }
        internal static void loadData()
        {
            foreach (var menu in getMenuBooleans())
            {
                if (menu != null)
                {
                    menu.loadData();
                }
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
    }
}
