using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Auto
{
    internal class AutoItem : IChatable
    {
        internal int ID { get; set; }
        internal int Quantity { get; set; }
        internal int timeDelay { get; set; }
        internal sbyte indexUI { get; set; }
        internal bool isGold { get; set; }
        internal bool isGem { get; set; }
        internal long lastTimeUseItem { get; set; }
        internal long lastTimeBuy { get; set; }

        internal static List<AutoItem> listItemUses = new List<AutoItem>();
        internal static AutoItem itemToAuto { get; set; }
        internal static AutoItem instance { get; set; }

        internal static AutoItem gI() => instance ??= new AutoItem();

        internal AutoItem() { }

        internal AutoItem(int ID, sbyte indexUI, int timeDelay, bool isGold = false, bool isGem = false)
        {
            this.ID = ID;
            this.indexUI = indexUI;
            this.timeDelay = timeDelay;
            this.isGold = isGold;
            this.isGem = isGem;
        }

        internal static void Update() => AutoUse();
        static void AutoUse()
        {
            if (listItemUses.Count == 0 || GameCanvas.gameTick % 20 != 0)
                return;

            var item = listItemUses.FirstOrDefault(i =>
                mSystem.currentTimeMillis() - i.lastTimeUseItem > (long)(i.timeDelay * 1000));

            if (item != null)
            {
                var currentItem = Char.myCharz().arrItemBag.FirstOrDefault(i => i != null && i.indexUI == item.indexUI);
                if (currentItem != null && currentItem.template.id == item.ID)
                {
                    item.lastTimeUseItem = mSystem.currentTimeMillis();
                    Service.gI().useItem(0, 1, item.indexUI, -1);
                }
                else
                {
                    listItemUses.Remove(item);
                    GameScr.info1.addInfo($"Đã ngưng sử dụng {ItemTemplates.get((short)item.ID).name}", 0);
                }
            }
        }

        static async void AutoBuy(AutoItem item)
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                while (item != null && item.Quantity > 0 && !GameScr.gI().isBagFull())
                {
                    if (mSystem.currentTimeMillis() - item.lastTimeBuy >= 400L)
                    {
                        Service.gI().buyItem((sbyte)(item.isGold ? 0 : 1), item.ID, 0);
                        item.Quantity--;
                        item.lastTimeBuy = mSystem.currentTimeMillis();
                    }
                }
                GameScr.info1.addInfo("Xong!", 0);
            });
        }

        internal void removeItemUses(int ID)
        {
            foreach(var item in listItemUses)
            {
                if(item != null && item.ID == ID)
                {
                    GameScr.info1.addInfo($"Đã ngưng sử dụng {ItemTemplates.get((short)ID).name}", 0);
                    listItemUses.RemoveAt(Array.IndexOf(listItemUses.ToArray(), item));
                    
                    break;
                }
            }
        }

        internal void addItemUses(int ID, sbyte indexUI)
        {
            GameCanvas.panel.hideNow();
            this.ID = ID;
            this.indexUI = indexUI;
            var itemName = ItemTemplates.get((short)ID).name;

            ChatTextField.gI().strChat = $"Auto Sử Dụng {itemName}";
            ChatTextField.gI().tfChat.name = "Nhập thời gian delay (Giây)";
            ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
            ChatTextField.gI().startChat2(this, string.Empty);
        }

        internal void addItemBuys(AutoItem item)
        {
            GameCanvas.panel.hideNow();
            itemToAuto = item;
            var itemName = ItemTemplates.get((short)item.ID).name;

            ChatTextField.gI().strChat = $"Auto Mua {itemName}";
            ChatTextField.gI().tfChat.name = "Nhập thời gian delay (Giây)";
            ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
            ChatTextField.gI().startChat2(this, string.Empty);
        }

        internal static bool ItemMustBeUse(Item item) =>
            item.template.type is 29 or 27 or 6 or 31;

        public static bool checkList(int Id) =>
            listItemUses.Any(item => item.ID == Id);

        public void onChatFromMe(string text, string to)
        {
            if (ChatTextField.gI().strChat.StartsWith("Auto Sử Dụng"))
            {
                if (int.TryParse(text, out int time))
                {
                    GameScr.info1.addInfo($"Auto: {ItemTemplates.get((short)ID).name} [{time} giây]", 0);
                    listItemUses.Add(new AutoItem(ID, indexUI, time));
                }
                else
                {
                    GameCanvas.startOKDlg(Strings.invalidValue + '!');
                }
            }
            else if (ChatTextField.gI().strChat.StartsWith("Auto Mua"))
            {
                if (int.TryParse(text, out int quantity))
                {
                    itemToAuto.Quantity = quantity;
                    AutoBuy(itemToAuto);
                }
                else
                {
                    GameCanvas.startOKDlg(Strings.invalidValue + '!');
                }
            }
            onCancelChat();
        }

        public void onCancelChat() => ChatTextField.gI().ResetTF();
    }
}
