
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Menu;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Object;
using System;
using UnityEngine;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Auto
{
    internal class AutoPean
    {
        internal protected class AutoPeanChatable : IChatable, IActionListener
        {
            public void onCancelChat() { ChatTextField.gI().ResetTF();}

            public void onChatFromMe(string text, string to)
            {
                if (ChatTextField.gI().strChat == "HP")
                {
                    if (long.TryParse(text, out long timeout))
                    {
                        if (timeout < 0)
                        {
                            GameCanvas.startOKDlg(string.Format(Strings.inputNumberOutOfRange, 0, Char.myCharz().cHPFull) + '!');
                            return;
                        }
                        else
                        {
                            HP = timeout;
                            Debug.Log($"HP Set To: {HP}"); // Kiểm tra xem HP có được cập nhật hay không
                            Utils.SaveData("hpPean", HP);
                            ShowHPInfo();
                        }
                    }
                    else
                    {
                        GameCanvas.startOKDlg(Strings.invalidValue + '!');
                        return;
                    }
                    onCancelChat();
                }
                else if (ChatTextField.gI().strChat == "KI")
                {
                    if (long.TryParse(text, out long timeout))
                    {
                        if (timeout < 0)
                        {
                            GameCanvas.startOKDlg(string.Format(Strings.inputNumberOutOfRange, 0, Res.formatNumber(long.MaxValue)) + '!');
                            return;
                        }
                        else
                        {
                            MP = timeout;
                            Debug.Log($"MP Set To: {MP}");
                            Utils.SaveData("mpPean", MP);
                            ShowMPInfo();
                        }
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
                Action action = idAction switch
                {
                    1 => () =>
                    {
                        ChatTextField.gI().strChat = "HP";
                        ChatTextField.gI().tfChat.name = "HP" + $" (0-{Res.formatNumber2(long.MaxValue)})";
                        ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                        ChatTextField.gI().startChat2(this, string.Empty);
                        //ChatTextField.gI().tfChat.setText("-1");
                    }
                    ,
                    2 => () =>
                    {
                        ChatTextField.gI().strChat = "KI";
                        ChatTextField.gI().tfChat.name = "KI" + $" (0-{Res.formatNumber2(long.MaxValue)})";
                        ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                        ChatTextField.gI().startChat2(this, string.Empty);
                    }
                    ,
                    _ => () => { }
                };
                action();
            }
        }
        static MenuBoolean[] menuBooleans = new MenuBoolean[]
          {
                new MenuBoolean(new MenuBooleanConfig()
                {
                    Title = "Xin Đậu",
                    GetValueFunc = () => isAutoRequest,
                    SetValueAction = value => isAutoRequest = value,
                    RMSName = "auto_ask_for_peans",
                    GetIsDisabled = () => Char.myCharz().clan == null,
                    GetDisabledReason = () => "Đéo có Bang"
                }),
                new MenuBoolean(new MenuBooleanConfig()
                {
                    Title = "Cho Đậu",
                    GetValueFunc = () => isAutoDonate,
                    SetValueAction = value => isAutoDonate = value,
                     RMSName = "auto_donate_peans",
                    GetIsDisabled = () => Char.myCharz().clan == null,
                     GetDisabledReason = () => "Đéo có Bang"
                }),
                new MenuBoolean(new MenuBooleanConfig()
                {
                    Title = Strings.autoHarvestPeansTitle,
                    GetValueFunc = () => AutoPean.isAutoHarvest,
                    SetValueAction = value => AutoPean.isAutoHarvest = value,
                    RMSName = "auto_harvest_peans"
                })
          };
        internal static bool _isAutoRequest;
        internal static bool isAutoRequest
        {
            get => _isAutoRequest;
            set
            {
                _isAutoRequest = value;
                if (value)
                    HandleSenzuBeans();
            }
        }

        internal static bool _isAutoDonate;
        internal static bool isAutoDonate
        {
            get => _isAutoDonate;
            set
            {
                _isAutoDonate = value;
                if (value)
                    HandleSenzuBeans();
            }
        }

        internal static bool _isAutoHarvest;
        internal static bool isAutoHarvest
        {
            get => _isAutoHarvest;
            set
            {
                _isAutoHarvest = value;
                if (value)
                    HandleSenzuBeans();
            }
        }

        internal static bool _isAutoHPPean;

        internal static long HP = -1;

        internal static long MP = -1;


        internal static void Update()
        {
            HandleHPPeans();
            HandleMPPeans();
            if (GameCanvas.gameTick % (60 * Time.timeScale) != 0)
                return;
            HandleSenzuBeans();
        }

        static void HandleSenzuBeans()
        {
            if (isAutoRequest && ClanUtils.CanAskForPeans())
                ClanUtils.RequestPeans();
            if (isAutoDonate && ClanUtils.CanDonatePeans())
                ClanUtils.DonatePeans();
            if (isAutoHarvest)
                HarvestMagicTree();
        }
        static void HandleHPPeans()
        {
            if (HP != -1)
            {
                if (Char.myCharz().cHP <= HP && GameCanvas.gameTick % 15 == 0)
                {
                    if (GameScr.hpPotion > 0)
                        GameScr.gI().doUseHP();
                    else HP = -1;
                }

            }
        }
        static void HandleMPPeans()
        {
            if (MP != -1)
            {
                if (Char.myCharz().cMP <= MP && GameCanvas.gameTick % 15 == 0)
                {
                    if (GameScr.hpPotion > 0)
                        GameScr.gI().doUseHP();
                    else MP = -1;
                }

            }
        }
        internal static void ShowHPInfo()
        {
            if (HP == -1)
            {
                GameScr.info1.addInfo("Chưa Setup HP", 0);
                if (GameScr.hpPotion <= 0)
                {
                    GameScr.info1.addInfo("Hết Mẹ Đậu Rồi", 0);
                }
            }
            else
            {
                GameScr.info1.addInfo($"Auto Đậu Khi HP Dưới {NinjaUtil.getMoneys(HP)}", 0);
            }
        }
        internal static void ShowMPInfo()
        {
            if (MP == -1)
            {
                GameScr.info1.addInfo("Chưa Setup MP", 0);
                if (GameScr.hpPotion <= 0)
                {
                    GameScr.info1.addInfo("Hết Mẹ Đậu Rồi", 0);
                }
            }
            else
            {
                GameScr.info1.addInfo($"Auto Đậu Khi MP Dưới {NinjaUtil.getMoneys(MP)}", 0);
            }
        }

        internal static void HarvestMagicTree()
        {
            var magicTree = GameScr.gI().magicTree;
            if (!Utils.IsMyCharHome() || magicTree.isUpdate || magicTree.isPeasEffect || magicTree.currPeas == 0)
                return;
            Service.gI().openMenu(4);
            Service.gI().confirmMenu(4, 0);
        }
        internal static void loadData()
        {
            foreach (var menu in menuBooleans)
            {
                if (menu != null && !string.IsNullOrEmpty(menu.RMSName))
                {
                    menu.loadData();
                }
            }
            if (Utils.TryLoadDataInt("hpPean", out int value))
            {
                HP = value;
            }
            if (Utils.TryLoadDataInt("mpPean", out int value2))
            {
                MP = value2;
            }
        }
        internal static void showMenu()
        {
            MyVector myVector = new MyVector();
            foreach (var menu in menuBooleans)
            {
                if (menu != null)
                {
                    myVector.addElement(new Command(Utils.GetToggleStringMenuOutRef(menu.Title, menu.GetValueFunc()), () => menu.action()));
                }
            }
            myVector.addElement(new Command($"Auto Khi HP Duới\n[{NinjaUtil.getMoneys(HP)}]", () => new AutoPeanChatable().perform(1, null)));
            myVector.addElement(new Command($"Auto Khi KI Duới\n[{NinjaUtil.getMoneys(MP)}]", () => new AutoPeanChatable().perform(2, null)));
            GameCanvas.menu.startAt(myVector, 0);
        }
    }
}
