using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace UglyBoy
{
    class AutoNoiTai : IActionListener, IChatable
    {
        public static string tennoitaicanmo;
        public static sbyte type = 1; // 1 la mo thuong, 2 la mo vip
        public static bool openMax = false;
        public static int max = -1;
        public static int[] chiso = new int[2];
        private static AutoNoiTai instance;
        public bool isnoitai = false;
        public static string noitai = "Chọn chỉ số";
        static Command cmdClose = new Command("Dừng", () =>
        {
            gI().isnoitai = false;
            openMax = false;
        });
        public static AutoNoiTai gI()
        {
            return instance == null ? instance = new AutoNoiTai() : instance;
        }
        public void StartOpenProcess()
        {
            Main.main.StopAllCoroutines();
            Main.main.StartCoroutine(OpenCoroutine());
        }
        private IEnumerator OpenCoroutine()
        {
            if (Char.myCharz().cPower < 10000000000)
            {
                GameScr.info1.addInfo("Cần 10 tỷ sức mạnh để mở.", 0);
                yield break;
            }

            if (openMax && max == -1)
            {
                isnoitai = false;
                openMax = false;
                yield break;
            }

            while (isnoitai)
            {
                Service.gI().speacialSkill(0);
                yield return new WaitForSeconds(0.3f);

                if (Panel.specialInfo.Contains(tennoitaicanmo))
                {
                    if (openMax)
                    {
                        var @char = Char.myCharz();
                        if((@char.luongKhoa) < 1000L)
                        {
                            GameScr.info1.addInfo("Hết ngọc", 0);
                            isnoitai = false;
                            openMax = false;
                            yield break;
                        }
                        int indexmod = Panel.specialInfo.IndexOf("%");
                        if (indexmod == -1) yield break;

                        string toSpace = Panel.specialInfo.Substring(0, indexmod);
                        int indexSpace = toSpace.LastIndexOf(' ');
                        if (indexSpace == -1) yield break;

                        string s = CutString(indexSpace + 1, indexmod - 1, Panel.specialInfo);
                        if (!int.TryParse(s, out int val)) yield break;

                        if (val == max)
                        {
                            isnoitai = false;
                            openMax = false;
                            GameScr.info1.addInfo("Xong", 0);
                            yield break;
                        }
                    }
                    else
                    {
                        isnoitai = false;
                        GameScr.info1.addInfo("Xong", 0);
                        yield break;
                    }
                }

                Service.gI().confirmMenu(5, type);
                yield return new WaitForSecondsRealtime(0.3f);

                Service.gI().confirmMenu(5, 0);
                yield return new WaitForSecondsRealtime(0.3f);
            }
        }

        
        internal static void Paint(mGraphics g)
        {
            if (gI().isnoitai)
            {
                for(int i = 0; i < Panel.listPassiveSkillInfo.Count; i++)
                {
                    var p = Panel.listPassiveSkillInfo[i];
                    mFont mFont = p.Contains(tennoitaicanmo) ? mFont.tahoma_7_red : mFont.tahoma_7_green2;
                    mFont.drawString(g, p, GameCanvas.hw, 30 + (i * 10), 3);
                }
                cmdClose.x = 50;
                cmdClose.y = 100;
                cmdClose.paint(g);
            }
        }
        internal static void updateTouch()
        {
            if (cmdClose.isPointerPressInside()) cmdClose.performAction();
        }
        public void perform(int idAction, object p)
        {
            if (idAction == 1)
            {
                string name = (string)p;
                int index = name.Substring(0, name.IndexOf('%')).LastIndexOf(' ');
                tennoitaicanmo = name.Substring(0, index);
                isnoitai = true;
                type = (sbyte)idAction;
                GameCanvas.panel.hide();
                StartOpenProcess();
            }
            else if (idAction == 2)
            {
                string name = (string)p;
                int index = name.Substring(0, name.IndexOf('%')).LastIndexOf(' ');
                tennoitaicanmo = name.Substring(0, index);
                isnoitai = true;
                type = (sbyte)idAction;
                GameCanvas.panel.hide();
                StartOpenProcess();
            }
            if (idAction == 3)
            {
                openMax = false;
                MyVector vector = new MyVector();
                vector.addElement(new Command("Mở Vip", AutoNoiTai.gI(), 2, p));
                vector.addElement(new Command("Mở Thường", AutoNoiTai.gI(), 1, p));
                GameCanvas.menu.startAt(vector, 3);

            }
            if (idAction == 4)
            {
                string name = (string)p;
                openMax = true;
                int index1 = name.IndexOf("đến ");
                int index2 = name.Substring(index1 + 4).IndexOf("%");
                max = int.Parse(name.Substring(index1 + 4, index2));
                MyVector vector = new MyVector();
                vector.addElement(new Command("Mở Vip", AutoNoiTai.gI(), 2, p));
                vector.addElement(new Command("Mở Thường", AutoNoiTai.gI(), 1, p));
                GameCanvas.menu.startAt(vector, 3);
            }
            if (idAction == 8)
            {
                string name = (string)p;
                int index = name.Substring(0, name.IndexOf('%')).LastIndexOf(' ');
                tennoitaicanmo = name.Substring(0, index);

                int index1 = name.IndexOf("%");
                int index2 = name.IndexOf("đến ");
                int space1 = name.Substring(0, index1).LastIndexOf(' ');
                int space2 = name.LastIndexOf('%');

                chiso[0] = int.Parse(CutString(space1, index1 - 1, name));
                chiso[1] = int.Parse(CutString(index2 + 4, space2 - 1, name));


                string khoang = CutString(space1, space2, name);
                noitai = "Nhập chỉ số bạn muốn chọn trong khoảng " + khoang;
                MyVector vector = new MyVector();
                vector.addElement(new Command("Mở Vip", AutoNoiTai.gI(), 9, 2));
                vector.addElement(new Command("Mở Thường", AutoNoiTai.gI(), 9, 1));
                GameCanvas.menu.startAt(vector, 3);

            }

            if (idAction == 9)
            {
                int id = (int)p;
                type = (sbyte)id;
                GameCanvas.panel.hideNow();
                ChatTextField.gI().strChat = noitai;
                ChatTextField.gI().tfChat.name = noitai;
                ChatTextField.gI().tfChat.setIputType(1);
                ChatTextField.gI().startChat2(this, string.Empty);

            }
        }

        public string CutString(int start, int end, string s)
        {
            string str = "";
            for (int i = start; i <= end; i++)
            {
                str += s[i];
            }
            return str;
        }

        public void onChatFromMe(string text, string to)
        {
            if(ChatTextField.gI().strChat == noitai)
            {
                int num = -1;
                try
                {
                    num = int.Parse(ChatTextField.gI().tfChat.getText());
                }
                catch
                {
                    GameScr.info1.addInfo("Invaild Value", 0);
                    return;
                }
                max = num;
                openMax = true;
                isnoitai = true;
                StartOpenProcess();
                onCancelChat();
            }
        }

        public void onCancelChat() => ChatTextField.gI().ResetTF();
    }
}

