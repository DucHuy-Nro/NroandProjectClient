using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Object;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Auto
{
    internal class AutoPoint : IActionListener, IChatable
    {
        internal int inputPoint;
        internal int typePotential;

        public void onCancelChat() => ChatTextField.gI().ResetTF();

        public void onChatFromMe(string text, string to)
        {
            if (ChatTextField.gI().tfChat.getText() != null && !ChatTextField.gI().tfChat.getText().Equals(string.Empty))
            {
                try
                {
                    if (int.TryParse(text, out int num2))
                    {
                        if ((typePotential == 0 || typePotential == 1) && num2 % 20 != 0)
                        {
                            GameScr.info1.addInfo("Chỉ Số HP, MP Phải chia hết cho 20. Vui Lòng Nhập Lại!", 0);
                            return;
                        }
                        if (typePotential == 0 || typePotential == 1)
                            num2 /= 20;
                        long num3 = Char.myCharz().cHPGoc / 20;
                        if (typePotential == 1)
                            num3 = Char.myCharz().cMPGoc / 20;
                        if (typePotential == 2)
                            num3 = Char.myCharz().cDamGoc;
                        if (typePotential == 3)
                            num3 = Char.myCharz().cDefGoc;
                        if (typePotential == 4)
                            num3 = Char.myCharz().cCriticalGoc;
                        if (num2 <= num3)
                        {
                            GameScr.info1.addInfo("Chỉ Số Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
                            return;
                        }
                        Service.gI().upPotential(typePotential, (int)(num2 - num3));
                        GameScr.info1.addInfo("Đã Cộng Xong!", 0);
                    }
                }
                catch
                {
                    GameScr.info1.addInfo("Chỉ Số Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
                }
            }
            ChatTextField.gI().isShow = false;
        }

        public void perform(int idAction, object p)
        {
            GameCanvas.panel.hideNow();
            switch (idAction)
            {
                case 1:
                    typePotential = 0;
                    StartInput("Nhập HP Mà Bạn Muốn Auto", "HP");
                    break;
                case 2:
                    typePotential = 1;
                    StartInput("Nhập MP Mà Bạn Muốn Auto", "MP");
                    break;
                case 3:
                    typePotential = 2;
                    StartInput("Nhập Sức Đánh Mà Bạn Muốn Auto", "Sức Đánh");
                    break;
                case 4:
                    typePotential = 3;
                    StartInput("Nhập Giáp Mà Bạn Muốn Auto", "Giáp");
                    break;
                case 5:
                    typePotential = 4;
                    StartInput("Nhập Chí Mạng Mà Bạn Muốn Auto", "Chí Mạng");
                    break;
                default:
                    break;
            }
        }

        private void StartInput(string prompt, string fieldName)
        {
            ChatTextField.gI().strChat = prompt;
            ChatTextField.gI().tfChat.name = fieldName;
            ChatTextField.gI().tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
            ChatTextField.gI().startChat2(this, string.Empty);
        }
    }
}
