using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.ModHelper.CommandMod.Hotkey;
using Assets.src.e;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Graphics
{
    internal class CustomPanel
    {
        internal static int _X { get; set; }
        internal static int _Y { get; set; }
        internal static int _W { get; set; }
        internal static int _H { get; set; }

        internal static int ITEM_WIDTH_BODY = 24;

        internal static int ITEM_HEIGHT_BODY = 24;

        internal static int ITEM_WIDTH = 28;

        internal static int ITEM_HEIGHT = 28;

        internal static int currentTabIndex = 0;

        internal static int currentTabInven = 0;

        internal static int numCol = 6;

        internal static int numRow;

        internal static bool m_isShow;

        internal static bool isShow
        {
            get => m_isShow;
            set
            {
                m_isShow = value;

                setScroll();
            }
        }

        internal static List<Button> buttons = new List<Button>();

        internal static Button[] inventorySwitches;

        internal static Cell[] cellsBody;

        internal static Cell[] cellsInven;

        internal static Scroll scrollInven = new Scroll();

        internal static ChatPopup cp;

        internal static Item currItem;

        internal static bool isUp;

        internal static int idIcon;

        internal static int selected = -1;
        static CustomPanel()
        {
            _W = 420;
            _H = 270;
            _X = GameCanvas.hw - _W / 2;
            _Y = GameCanvas.hh - _H / 2 + 10;
            buttons.Add(new Button("Hành Trang", () => currentTabIndex = 0, currentTabIndex == 0));
            buttons.Add(new Button("Kỹ Năng", () => currentTabIndex = 1, currentTabIndex == 1));
            buttons.Add(new Button("Bang Hội", () => currentTabIndex = 2, currentTabIndex == 2));
            buttons.Add(new Button("Chức Năng", () => currentTabIndex = 3, currentTabIndex == 3));
            inventorySwitches = new Button[2]
            {
                new Button("Trang Bị", () =>
                {
                    currentTabInven = 0;
                    resetAllItems();
                    setScroll();
                }, currentTabInven == 0),
                new Button("Rương Đồ", () =>
                {
                    currentTabInven = 1;
                     resetAllItems();
                    setScroll();
                }, currentTabInven == 1)
            };
        }
        internal class Cell
        {
            internal int x, y;
            internal Cell(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            internal void paint(mGraphics g)
            {
                g.setColor(0, 0.5f);
                g.fillRect(x, y, 30, 30, 7);
            }
            internal bool isPointerHoldIn()
            {
                return GameCanvas.isPointerHoldIn(x, y, 30, 30);
            }
        }
        internal static void Show()
        {
            isShow = !isShow;
        }
        private static void setScroll()
        {
            Item[] items = currentTabInven == 0 ? Char.myCharz().arrItemBag : Char.myCharz().arrItemBox;
            numRow = Mathf.Max(6, Mathf.CeilToInt(items.Length / 6f));
            int itemSize = ITEM_WIDTH + 5;
            scrollInven.setStyle(numRow * numCol, itemSize, _X + _W / 2 - 20, _Y + 39, 6 * (itemSize + 5) + 40, _H - 39, true, 6);
        }
        private static void resetAllItems()
        {
            scrollInven.clear();
            scrollInven.cmy = 0;
            currItem = null;
            selected = -1;
            cp = null;
        }
        internal static void updateScroll(Scroll scr)
        {
            if (scr.cmyLim > 0)
            {
                ScrollResult result = scr.updateKey();

                if (scr.cmy < 0 && result.isDowning == false)
                {

                    scr.cmy -= scr.cmy / 2;
                }
                else if (scr.cmy > scr.cmyLim && result.isDowning == false)
                {
                    scr.cmy -= (scr.cmy - scr.cmyLim + 6) / 2;
                }
                if (result.isDowning)
                    GameCanvas.isPointerMove = false;
            }
            int scrollAmount = GameCanvas.pXYScrollMouse;
            if (scrollAmount != 0 && scr.cmyLim > 0)
            {
                scr.cmy -= scrollAmount * 10;
                scr.cmy = Mathf.Clamp(scr.cmy, 0, scr.cmyLim);
            }
        }
        private static Image getImgThoiVang()
        {
            Small small = SmallImage.imgNew[4028];
            if (small == null) SmallImage.createImage(4028);
            return small.img;
        }
        private static int getQuantityThoiVang()
        {
            foreach (var item in Char.myCharz().arrItemBag)
            {
                if (item != null && item.template.id == 457)
                {
                    return item.quantity;
                }
            }
            return 0;
        }
        private static string getQuantityThoiVangStr()
        {
            if (getQuantityThoiVang() > 1_000_000_000L)
            {
                return Res.formatNumber2(getQuantityThoiVang());
            }
            return NinjaUtil.getMoneys(getQuantityThoiVang());
        }
        private static int getCompare(Item item)
        {
            if (item == null)
            {
                return -1;
            }
            if (item.isTypeBody())
            {
                if (item.itemOption == null)
                {
                    return -1;
                }
                ItemOption itemOption = item.itemOption[0];
                if (itemOption.optionTemplate.id == 22)
                {
                    itemOption.optionTemplate = GameScr.gI().iOptionTemplates[6];
                    itemOption.param *= 1000;
                }
                if (itemOption.optionTemplate.id == 23)
                {
                    itemOption.optionTemplate = GameScr.gI().iOptionTemplates[7];
                    itemOption.param *= 1000;
                }
                Item item2 = null;
                for (int i = 0; i < Char.myCharz().arrItemBody.Length; i++)
                {
                    Item item3 = Char.myCharz().arrItemBody[i];
                    if (itemOption.optionTemplate.id == 22)
                    {
                        itemOption.optionTemplate = GameScr.gI().iOptionTemplates[6];
                        itemOption.param *= 1000;
                    }
                    if (itemOption.optionTemplate.id == 23)
                    {
                        itemOption.optionTemplate = GameScr.gI().iOptionTemplates[7];
                        itemOption.param *= 1000;
                    }
                    if (item3 != null && item3.itemOption != null && item3.template.type == item.template.type)
                    {
                        item2 = item3;
                        break;
                    }
                }
                if (item2 == null)
                {
                    isUp = true;
                    return itemOption.param;
                }
                int num = 0;
                num = ((item2 == null || item2.itemOption == null) ? itemOption.param : (itemOption.param - item2.itemOption[0].param));
                if (num < 0)
                {
                    isUp = false;
                }
                else
                {
                    isUp = true;
                }
                return num;
            }
            return 0;
        }
        private static void addItemDetail(Item item)
        {
            try
            {
                cp = new ChatPopup();
                string empty = string.Empty;
                string text = string.Empty;
                if (item.template.gender != Char.myCharz().cgender)
                {
                    if (item.template.gender == 0)
                    {
                        text = text + "\n|7|1|" + mResources.from_earth;
                    }
                    else if (item.template.gender == 1)
                    {
                        text = text + "\n|7|1|" + mResources.from_namec;
                    }
                    else if (item.template.gender == 2)
                    {
                        text = text + "\n|7|1|" + mResources.from_sayda;
                    }
                }
                string text2 = string.Empty;
                if (item.itemOption != null)
                {
                    for (int i = 0; i < item.itemOption.Length; i++)
                    {
                        if (item.itemOption[i].optionTemplate.id == 72)
                        {
                            text2 = " [+" + item.itemOption[i].param + "]";
                        }
                    }
                }
                bool flag = false;
                if (item.itemOption != null)
                {
                    for (int j = 0; j < item.itemOption.Length; j++)
                    {
                        if (item.itemOption[j].optionTemplate.id == 41)
                        {
                            flag = true;
                            if (item.itemOption[j].param == 1)
                            {
                                text = text + "|0|1|" + item.template.name + text2;
                            }
                            if (item.itemOption[j].param == 2)
                            {
                                text = text + "|2|1|" + item.template.name + text2;
                            }
                            if (item.itemOption[j].param == 3)
                            {
                                text = text + "|8|1|" + item.template.name + text2;
                            }
                            if (item.itemOption[j].param == 4)
                            {
                                text = text + "|7|1|" + item.template.name + text2;
                            }
                        }
                    }
                }
                if (!flag)
                {
                    text = text + "|0|1|" + item.template.name + text2;
                }
                if (item.itemOption != null)
                {
                    for (int k = 0; k < item.itemOption.Length; k++)
                    {
                        if (item.itemOption[k].optionTemplate.name.StartsWith("$") ? true : false)
                        {
                            empty = item.itemOption[k].getOptiongColor();
                            if (item.itemOption[k].param == 1)
                            {
                                text = text + "\n|1|1|" + empty;
                            }
                            if (item.itemOption[k].param == 0)
                            {
                                text = text + "\n|0|1|" + empty;
                            }
                            continue;
                        }
                        empty = item.itemOption[k].getOptionString();
                        if (!empty.Equals(string.Empty) && item.itemOption[k].optionTemplate.id != 72)
                        {
                            if (item.itemOption[k].optionTemplate.id == 102)
                            {
                                cp.starSlot = (sbyte)item.itemOption[k].param;
                                Res.outz("STAR SLOT= " + cp.starSlot);
                            }
                            else if (item.itemOption[k].optionTemplate.id == 107)
                            {
                                cp.maxStarSlot = (sbyte)item.itemOption[k].param;
                                Res.outz("STAR SLOT= " + cp.maxStarSlot);
                            }
                            else
                            {
                                text = text + "\n|1|1|" + empty;
                            }
                        }
                    }
                }
                if (currItem.template.strRequire > 1)
                {
                    string text3 = mResources.pow_request + ": " + currItem.template.strRequire;
                    if (currItem.template.strRequire > Char.myCharz().cPower)
                    {
                        text = text + "\n|3|1|" + text3;
                        string text4 = text;
                        text = text4 + "\n|3|1|" + mResources.your_pow + ": " + Char.myCharz().cPower;
                    }
                    else
                    {
                        text = text + "\n|6|1|" + text3;
                    }
                }
                else
                {
                    text += "\n|6|1|";
                }
                currItem.compare = getCompare(currItem);
                text += "\n--";
                text = text + "\n|6|" + item.template.description;
                if (!item.reason.Equals(string.Empty))
                {
                    if (!item.template.description.Equals(string.Empty))
                    {
                        text += "\n--";
                    }
                    text = text + "\n|2|" + item.reason;
                }
                if (cp.maxStarSlot > 0)
                {
                    text += "\n\n";
                }
                popUpDetailInit(cp, text);
                idIcon = item.template.iconID;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        private static void popUpDetailInit(ChatPopup cp, string chat)
        {
            try
            {
                cp.isClip = false;
                cp.sayWidth = 180;
                cp.cx = GameCanvas.menu.menuX;
                cp.says = mFont.tahoma_7_red.splitFontArray(chat, cp.sayWidth - 10);
                cp.delay = 10000000;
                cp.c = null;
                cp.sayRun = 7;
                cp.ch = 15 - cp.sayRun + cp.says.Length * 12 + 10;
                if (cp.ch > GameCanvas.h - 80)
                {
                    cp.ch = GameCanvas.h - 80;
                    cp.lim = cp.says.Length * 12 - cp.ch + 17;
                    if (cp.lim < 0)
                    {
                        cp.lim = 0;
                    }
                    ChatPopup.cmyText = 0;
                    cp.isClip = true;
                }
                cp.cy = _Y + _H / 2 - 20 - cp.ch;
                while (cp.cy < 10)
                {
                    cp.cy++;
                    GameCanvas.menu.menuY++;
                }
                cp.mH = 0;
                cp.strY = 10;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private static void paintDetail(mGraphics g)
        {
            try
            {
                if (cp == null || cp.says == null)
                {
                    return;
                }
                cp.paint(g);
                int num = cp.cx + 13;
                int num2 = cp.cy + 11;
                if (idIcon != -1)
                {
                    SmallImage.drawSmallImage(g, idIcon, num, num2, 0, 3);
                }
                if (currItem != null && currItem.template.type != 5)
                {
                    if (currItem.compare > 0)
                    {
                        g.drawImage(Panel.imgUp, num - 7, num2 + 13, 3);
                        mFont.tahoma_7b_green.drawString(g, Res.abs(currItem.compare) + string.Empty, num + 1, num2 + 8, 0);
                    }
                    else if (currItem.compare < 0 && currItem.compare != -1)
                    {
                        g.drawImage(Panel.imgDown, num - 7, num2 + 13, 3);
                        mFont.tahoma_7b_red.drawString(g, Res.abs(currItem.compare) + string.Empty, num + 1, num2 + 8, 0);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        private static void paintTop(mGraphics g)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                var b = buttons[i];
                if (b != null)
                {
                    int buttonWidth = b.myWPos(b.select);
                    int buttonSpacing = 5;
                    int totalWidth = (buttons.Count * buttonWidth) + ((buttons.Count - 1) * buttonSpacing);
                    int startX = _X + (_W - totalWidth) / 2;
                    int startY = _Y - b.myHPos(b.select) - 3;
                    b.x = startX + i * (buttonWidth + buttonSpacing);
                    b.y = startY;
                    b.paint(g);
                }
            }
            g.drawImage(Panel.imgX, _X + _W - Panel.imgX.getWidth() / 2, _Y - Panel.imgX.getHeight() / 2);
            if(GameCanvas.isMouseFocus(_X + _W - Panel.imgX.getWidth() / 2, _Y - Panel.imgX.getHeight() / 2, Panel.imgX.getWidth(), Panel.imgX.getHeight()))
            {
                int x = _X + _W - Panel.imgX.getWidth() / 2;
                int y = _Y - Panel.imgX.getHeight() / 2;
                int w = Panel.imgX.getWidth();
                int h = Panel.imgX.getHeight();
                g.drawImage(ItemMap.imageFlare, x + w / 2, y + w / 2, 3);
            }
        }
        private static void paintBottomMoneysInfo(mGraphics g)
        {
            g.drawImage(Panel.imgXu, _X + 3, _Y + 3);
            mFont.tahoma_7_white.drawStringBd(g, Char.myCharz().xuStr, _X + 20, _Y + 2, 0, mFont.tahoma_7);
            g.drawImage(Panel.imgLuong, _X + _W / 4 + 10, _Y + 3);
            mFont.tahoma_7_white.drawStringBd(g, Char.myCharz().luongStr, _X + _W / 4 + 27, _Y + 2, 0, mFont.tahoma_7);
            g.drawImage(Panel.imgLuongKhoa, _X + 3, _Y + 18, 0);
            mFont.tahoma_7_white.drawStringBd(g, Char.myCharz().luongKhoaStr, _X + 20, _Y + 16, 0, mFont.tahoma_7);
            g.drawImage(getImgThoiVang(), _X + _W / 4 + 3, _Y + 18);
            mFont.tahoma_7_white.drawStringBd(g, getQuantityThoiVangStr(), _X + _W / 4 + 27, _Y + 16, 0, mFont.tahoma_7);
        }
        private static void paintCharBody(mGraphics g)
        {
            int cx = _X + 85;
            int cy = _Y + _H / 2;
            g.drawImage(TileMap.bong, cx, cy);
            Char.myCharz().paintCharBody(g, cx + TileMap.bong.getWidth() / 2 + 1, cy + TileMap.bong.getHeight() / 2, 1, Char.myCharz().cf, true);
            
            for (int i = 0; i < Char.myCharz().vEffChar.size(); i++)
            {
                Effect effect = (Effect)Char.myCharz().vEffChar.elementAt(i);
                if (effect.layer == 1)
                {
                    bool flag = true;
                    if (effect.isStand == 0)
                    {
                        flag = ((Char.myCharz().statusMe == 1 || Char.myCharz().statusMe == 6) ? true : false);
                    }
                    if (flag)
                    {
                        effect.x = cx;
                        effect.y = cy + 20;
                        effect.paint(g);
                    }
                }
            }
        }
        private static void paintCharacterStats(mGraphics g)
        {
            int statsX = _X;
            int statsY = _Y + _H - 60;
            int statsWidth = _W / 2 - 20;
            int statsHeight = 60;

            // Vẽ nền vùng hiển thị chỉ số
            g.setColor(0, 0.8f);
            g.fillRect(statsX, statsY, statsWidth, statsHeight);

            // Lấy thông tin nhân vật
            Char character = Char.myCharz();
            string hp = $"HP: {NinjaUtil.getMoneys(character.cHP)}/{NinjaUtil.getMoneys(character.cHPFull)}";
            string ki = $"KI: {NinjaUtil.getMoneys(character.cMP)}/{NinjaUtil.getMoneys(character.cMPFull)}";
            string damage = $"Sức đánh: {NinjaUtil.getMoneys(character.cDamFull)}";
            string defense = $"Giáp: {NinjaUtil.getMoneys(character.cDefull)}, Chí mạng: {character.cCriticalFull}%";
            string power = $"SM: {NinjaUtil.getMoneys(character.cPower)}";
            string potential = $"TN: {NinjaUtil.getMoneys(character.cTiemNang)}";

            // Vẽ thông tin nhân vật
            mFont.tahoma_7b_red.drawString(g, hp, statsX + 5, statsY, 0);
            mFont.tahoma_7b_blue.drawString(g, ki, statsX + 5, statsY + 10, 0);
            mFont.tahoma_7b_yellow.drawString(g, damage, statsX + 5, statsY + 20, 0);
            mFont.tahoma_7b_green.drawString(g, defense, statsX + 5, statsY + 30, 0);
            mFont.tahoma_7b_red.drawString(g, power, statsX + 5, statsY + 40, 0);
            mFont.tahoma_7b_red.drawString(g, potential, statsX + 5, statsY + 50, 0);
        }
        private static void paintScrollBar(mGraphics g)
        {
            int scrollBarX = _X + _W - 6; // Thu nhỏ thêm và sát lề hơn
            int scrollBarY = _Y + 39;
            int scrollBarHeight = _H - 42; // Chiều cao ngang đuôi ô item cuối cùng
            int scrollBarWidth = 3; // Giảm độ rộng thanh cuộn

            // Vẽ nền thanh cuộn
            g.setColor(0, 0.3f);
            g.fillRect(scrollBarX, scrollBarY, scrollBarWidth, scrollBarHeight, 7);

            if (scrollInven.cmyLim > 0)
            {
                int scrollThumbHeight = Mathf.Max(10, scrollBarHeight * 6 / numRow); // Thu nhỏ thanh trượt hơn
                int scrollThumbY = scrollBarY + (scrollInven.cmy * (scrollBarHeight - scrollThumbHeight) / scrollInven.cmyLim);

                // Vẽ thanh trượt
                g.setColor(16777215);
                g.fillRect(scrollBarX, scrollThumbY, scrollBarWidth, scrollThumbHeight, 7);
            }
        }
        private static void paintInventory(mGraphics g)
        {
            //Char
            g.setColor(14272691);
            g.fillRect(_X, _Y, _W / 2 - 20, _H, 7);
            g.setColor(14272691);
            g.fillRect(_X + (_W / 2 - 30), _Y, 10, _H);
            g.setColor(278362965);
            g.fillRect(_X + _W - 10, _Y + _H / 2, 10, _H / 2);
            //Info Bar
            g.setColor(16777215);
            g.fillRect(_X, _Y + 30, _W / 2 - 20, _H - 30, 7);
            g.setColor(16777215);
            g.fillRect(_X, _Y + 29, _W / 2 - 20, 2);
            g.setColor(16777215);
            g.fillRect(_X, _Y + 30, 2, _H - 40);
            g.setColor(16777215);
            g.fillRect(_X + (_W / 2 - 40), _Y + 30, 20, _H - 30);
            g.setColor(16777215);
            g.fillRect(_X, _Y + 30, 5, _H - 30);
            //Bottom Moneys
            paintBottomMoneysInfo(g);

            //NV
            paintCharBody(g);


            //States
            paintCharacterStats(g);

            //Body
            if (cellsBody == null || cellsBody.Length != Char.myCharz().arrItemBody.Length)
                cellsBody = new Cell[Char.myCharz().arrItemBody.Length];
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    if (row < 4 && (col == 0 || col == numCol - 1))
                    {
                        int dx = _X + col * (ITEM_WIDTH_BODY + 5) + 10, dy = _Y + 40 + row * (ITEM_HEIGHT_BODY + 5);
                        g.setColor(0, 0.5f);
                        g.fillRect(dx, dy, ITEM_WIDTH_BODY, ITEM_HEIGHT_BODY, 7);
                        Item item = null;
                        int idex = -1;

                        if (col == 0 && row <= 4)
                        {
                            if (Char.myCharz().arrItemBody != null && row < Char.myCharz().arrItemBody.Length && Char.myCharz().arrItemBody[row] != null)
                            {
                                item = Char.myCharz().arrItemBody[row];
                            }
                            idex = row;
                        }
                        else if (col == numCol - 1 && row <= 4)
                        {
                            if (Char.myCharz().arrItemBody != null && row + 4 < Char.myCharz().arrItemBody.Length && Char.myCharz().arrItemBody[row + 4] != null)
                            {
                                item = Char.myCharz().arrItemBody[row + 4];
                            }
                            idex = row + 4;
                        }
                        if (item != null)
                            SmallImage.drawSmallImage(g, item.template.iconID, dx + ITEM_WIDTH_BODY / 2, dy + ITEM_HEIGHT_BODY / 2, 0, mGraphics.VCENTER | mGraphics.HCENTER);

                        if (idex != -1)
                        {
                            if (cellsBody[idex] == null)
                                cellsBody[idex] = new Cell(dx, dy);
                        }
                    }
                    else if (row >= 4)
                    {
                        int dx = _X + col * (ITEM_WIDTH_BODY + 5) + 10, dy = _Y + 40 + row * (ITEM_HEIGHT_BODY + 5);
                        g.setColor(0, 0.5f);
                        g.fillRect(dx, dy, ITEM_WIDTH_BODY, ITEM_HEIGHT_BODY, 7);

                        int idex = (row - 4) * numCol + (col + 8);

                        if (Char.myCharz().arrItemBody != null && idex < Char.myCharz().arrItemBody.Length && Char.myCharz().arrItemBody[idex] != null)
                        {
                            SmallImage.drawSmallImage(g, Char.myCharz().arrItemBody[idex].template.iconID, dx + ITEM_WIDTH_BODY / 2, dy + ITEM_HEIGHT_BODY / 2, 0, mGraphics.VCENTER | mGraphics.HCENTER);
                        }
                        if (idex < cellsBody.Length && cellsBody[idex] == null)
                            cellsBody[idex] = new Cell(dx, dy);
                    }

                }
            }

            g.reset();

            //Bag
            if (cellsInven == null || cellsInven.Length != numRow * numCol)
                cellsInven = new Cell[numCol * numRow];

            g.setClip(_X + _W / 2 - 20, _Y + 39, 6 * (ITEM_WIDTH + 5) + 40, _H - 39);
            paintScrollBar(g);
            g.translate(0, -scrollInven.cmy);
            for (int row = 0; row < numRow; row++)
            {
                for (int col = 0; col < numCol; col++)
                {
                    int x = _X - 10 + _W / 2 + col * (ITEM_WIDTH + 8);
                    int y = _Y + 39 + row * (ITEM_WIDTH + 5);
                    int index = row * numCol + col;

                    if (cellsInven[index] == null)
                        cellsInven[index] = new Cell(x, y);
                    cellsInven[index].paint(g);
                    var arrItem = currentTabInven == 0 ? Char.myCharz().arrItemBag : Char.myCharz().arrItemBox;
                    if (arrItem != null && index < arrItem.Length && arrItem[index] != null)
                    {
                        SmallImage.drawSmallImage(g, arrItem[index].template.iconID, x + ITEM_WIDTH / 2 + 1, y + ITEM_HEIGHT / 2 + 1, 0, mGraphics.VCENTER | mGraphics.HCENTER);

                    }
                }
            }
            g.translate(scrollInven.cmx, scrollInven.cmy);
            g.reset();

            //Buttons
            for (int i = 0; i < inventorySwitches.Length; i++)
            {
                var b = inventorySwitches[i];
                if (b != null)
                {
                    int buttonWidth = b.myWPos(b.select);
                    int buttonSpacing = 5;
                    int totalWidth = (inventorySwitches.Length * buttonWidth) + ((inventorySwitches.Length - 1) * buttonSpacing);
                    int startX = _X - 10 + _W / 2 + (_W / 2 - totalWidth) / 2;
                    int startY = _Y + 10;
                    b.x = startX + i * (buttonWidth + buttonSpacing);
                    b.y = startY;
                    b.paintNor(g);
                }
            }
            //WATERMARK
            mFont.tahoma_7_white.drawStringBd(g, "© HairMod", _X + _W - mFont.tahoma_7_white.getWidth("© HairMod") - 20, _Y - 10, 0, mFont.tahoma_7);

        }
        internal static void Paint(mGraphics g)
        {
            if (!isShow) return;
            //Board
            g.setColor(278362965);
            g.fillRect(_X, _Y, _W, _H, 7);
            switch (currentTabIndex)
            {
                case 0:
                    paintInventory(g);
                    break;
            }
            paintTop(g);
            paintDetail(g);
        }
        private static void updateKeyTop()
        {
            foreach (var b in buttons)
            {
                if (b != null && b.IsPressedInSide())
                {
                    foreach (var btn in buttons)
                    {
                        if (btn != null) btn.select = false;
                    }
                    b.select = true;
                    b.actionPerform();
                    break;
                }
            }
            if(GameCanvas.isPointerHoldIn(_X + _W - Panel.imgX.getWidth() / 2, _Y - Panel.imgX.getHeight() / 2, Panel.imgX.getWidth(), Panel.imgX.getHeight()))
            {
                if(GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
                {
                    resetAllItems();
                    isShow = false;
                    GameCanvas.clearAllPointerEvent();
                }
            }
        }
        private static void updateKeyInven()
        {
            if (!GameCanvas.isPointerClick || !GameCanvas.isPointerJustRelease)
                return;
            foreach (var b in inventorySwitches)
            {
                if (b != null && b.IsPressedInSide())
                {
                    foreach (var btn in inventorySwitches)
                    {
                        if (btn != null) btn.select = false;
                    }
                    b.select = true;
                    b.actionPerform();
                    break;
                }
            }
            if (cellsBody != null)
            {
                for (int i = 0; i < cellsBody.Length; i++)
                {
                    if (cellsBody[i] != null && cellsBody[i].isPointerHoldIn())
                    {
                        if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
                        {
                            selected = i;
                            Debug.LogError("selected: " + selected);
                            GameCanvas.clearAllPointerEvent();
                            break;
                        }
                    }
                }
            }
            if (cellsInven != null && scrollInven != null)
            {
                for (int i = 0; i < cellsInven.Length; i++)
                {
                    if (cellsInven[i] != null && GameCanvas.isPointerHoldIn(cellsInven[i].x, cellsInven[i].y - scrollInven.cmy, 30, 30))
                    {
                        if (GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
                        {
                            selected = Char.myCharz().arrItemBody.Length + i;
                            Debug.LogError("selected: " + selected);
                            GameCanvas.clearAllPointerEvent();
                            break;
                        }
                    }
                }
            }

        }
        private static void doFireInventory()
        {
            if (selected == -1) return;
            int x, y ;
            currItem = null;
            MyVector myVector = new MyVector();
            var @char = Char.myCharz();
            if (selected < @char.arrItemBody.Length)
            {
                x = _X + 20;
                Item item = @char.arrItemBody[selected];
                if (item != null)
                {
                    currItem = item;
                    myVector.addElement(new Command(mResources.GETOUT, () =>
                    {
                        Service.gI().getItem(Panel.BODY_BAG, (sbyte)selected);
                        currItem = null;
                        selected = -1;
                        cp = null;
                    }));
                }
            }
            else
            {
                x = _X + _W / 2 + 20;
                if (currentTabInven == 0)
                {
                    Item item2 = @char.arrItemBag[selected - @char.arrItemBody.Length];
                    if (item2 != null)
                    {
                        currItem = item2;
                        if (item2.isTypeBody())
                        {
                            myVector.addElement(new Command(mResources.USE, () =>
                            {
                                Service.gI().getItem(Panel.BAG_BODY, (sbyte)(selected - @char.arrItemBody.Length));
                                currItem = null;
                                selected = -1;
                                cp = null;
                            }));
                            if (Char.myCharz().havePet)
                            {
                                myVector.addElement(new Command(mResources.MOVEFORPET, () =>
                                {
                                    Service.gI().getItem(Panel.BAG_PET, (sbyte)(selected - @char.arrItemBody.Length));
                                    currItem = null;
                                    selected = -1;
                                    cp = null;
                                }));
                            }
                            myVector.addElement(new Command(mResources.MOVEOUT, () =>
                            {
                                Service.gI().useItem(1, 1, (sbyte)item2.indexUI, -1);
                                currItem = null;
                                selected = -1;
                                cp = null;
                            }));
                        }
                        else
                        {
                            myVector.addElement(new Command(mResources.USE, () =>
                            {
                                Service.gI().useItem(0, 1, (sbyte)(item2.indexUI), -1);
                                currItem = null;
                                selected = -1;
                                cp = null;
                            }));
                            myVector.addElement(new Command(mResources.MOVEOUT, () =>
                            {
                                Service.gI().useItem(1, 1, (sbyte)item2.indexUI, -1);
                                currItem = null;
                                selected = -1;
                                cp = null;
                            }));
                            myVector.addElement(new Command(mResources.move_to_chest, () =>
                            {
                                Service.gI().getItem(Panel.BAG_BOX, (sbyte)(selected - @char.arrItemBody.Length));
                                currItem = null;
                                selected = -1;
                                cp = null;
                            }));
                        }
                    }
                }
                else
                {
                    Item item3 = @char.arrItemBox[selected - @char.arrItemBody.Length];
                    if (item3 != null)
                    {
                        currItem = item3;
                        myVector.addElement(new Command(mResources.GETOUT, () =>
                        {
                            Service.gI().getItem(Panel.BOX_BAG, (sbyte)(selected - @char.arrItemBody.Length));
                            currItem = null;
                            selected = -1;
                            cp = null;
                        }));

                    }
                }
            }
            if (currItem != null)
            {
                addItemDetail(currItem);

                GameCanvas.menu.startAt(myVector, x, cp.cy + cp.ch + 5);
            }
            else
            {
                cp = null;
            }
        }
        internal static void updateTouch()
        {
            try
            {
                if (cp != null)
                {
                    bool pointerClick =
                      GameCanvas.isPointerHoldIn(0, 0, cp.cx, cp.cy + cp.ch) ||
                      GameCanvas.isPointerHoldIn(cp.cx, 0, GameCanvas.w - cp.cx, cp.cy) ||
                      GameCanvas.isPointerHoldIn(0, cp.cy + cp.ch, cp.cx, GameCanvas.h - (cp.cy + cp.ch)) ||
                      GameCanvas.isPointerHoldIn(cp.cx, cp.cy + cp.ch + GameCanvas.menu.menuH + 2, cp.sayWidth, GameCanvas.h) ||
                      GameCanvas.isPointerHoldIn(cp.cx + cp.sayWidth, cp.cy, GameCanvas.w, GameCanvas.h);
                    if (pointerClick && GameCanvas.isPointerClick)
                    {
                        cp = null;
                        GameCanvas.menu.showMenu = false;
                        currItem = null;
                        selected = -1;
                        return;
                    }
                }
                if (GameCanvas.menu.showMenu)
                {
                    GameCanvas.clearKeyHold();
                    for (int i = 0; i < GameCanvas.keyPressed.Length; i++)
                    {
                        GameCanvas.keyPressed[i] = false;
                    }
                    GameCanvas.isPointerJustRelease = false;
                    return;
                }
                switch (currentTabIndex)
                {
                    case 0:
                        if (scrollInven != null && !GameCanvas.isPointerClick)
                        {
                            updateScroll(scrollInven);
                        }
                        updateKeyInven();
                        break;
                }
                updateKeyTop();
                GameCanvas.clearKeyHold();
                for (int i = 0; i < GameCanvas.keyPressed.Length; i++)
                {
                    GameCanvas.keyPressed[i] = false;
                }
                GameCanvas.isPointerJustRelease = false;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        internal static void update()
        {
            if (!isShow) return;
            try
            {
                switch (currentTabIndex)
                {
                    case 0:
                        doFireInventory();
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

    }
}
