using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Object;
using System;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Menu
{
    internal class MenuValues : MenuItem, IChatable
    {
        internal double SelectedValue
        {
            get => GetValueFunc();
            set
            {
                if (value < MinValue || value > MaxValue)
                    return;
                SetValueAction(value);
            }
        }

        /// <summary>Danh sách giá trị để lựa chọn</summary>
        internal string[] Values => _config.Values;
        /// <summary>Tên tệp lưu dữ liệu</summary>
        internal string RMSName => _config.RMSName;
        /// <summary>Hàm được gọi để lấy giá trị hiện tại, không có đối số và trả về giá trị kiểu <see cref="double"/>.</summary>
        internal Action<double> SetValueAction => _config.SetValueAction;
        /// <summary>Hàm được gọi để gán giá trị mới, có 1 đối số kiểu <see cref="double"/> và không trả về giá trị.</summary>
        internal Func<double> GetValueFunc => _config.GetValueFunc;
        /// <summary>Tiêu đề của trường nhập liệu <see cref="ChatTextField"/></summary>
        internal string TextFieldName => _config.TextFieldTitle;
        /// <summary>Gợi ý cho trường nhập liệu <see cref="ChatTextField"/></summary>
        internal string TextFieldHint => _config.TextFieldHint;
        /// <summary>Giá trị tối thiểu của <see cref="MenuValues"/>, chỉ có hiệu lực khi nhập giá trị bằng <see cref="ChatTextField"/></summary>
        internal double MinValue => _config.MinValue;
        /// <summary>Giá trị tối đa của <see cref="MenuValues"/>, chỉ có hiệu lực khi nhập giá trị bằng <see cref="ChatTextField"/></summary>
        internal double MaxValue => _config.MaxValue;
        /// <summary> Quyết định giá trị của <see cref="MenuValues"/> có phải là số thực hay không. </summary>
        internal bool IsFloatingPoint => _config.IsFloatingPoint;

        MenuValuesConfig _config;
        ChatTextField currentCTF = ChatTextField.gI();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">Cấu hình <see cref="MenuValues"/></param>
        /// <exception cref="ArgumentException">Danh sách giá trị và mô tả đều bằng <see langword="null"/> hoặc rỗng.</exception>
        internal MenuValues(MenuValuesConfig config) : base(config)
        {
            if ((config.Values == null || config.Values.Length <= 0) && string.IsNullOrEmpty(config.Description))
                throw new ArgumentException("Values and description cannot be null at the same time");
            _config = config;
        }

        internal string getSelectedValue() => Values[(int)SelectedValue];

        internal void SwitchSelection()
        {
            if (Values != null)
            {
                if (SelectedValue < Values.Length - 1)
                    SelectedValue++;
                else
                    SelectedValue = 0;
            }
        }
        
        internal void saveData()
        {
            if (string.IsNullOrEmpty(RMSName))
                return;
            if (IsFloatingPoint)
                Utils.SaveData(RMSName, SelectedValue);
            else
                Utils.SaveData(RMSName, (long)SelectedValue);
        }
        internal void loadData()
        {
            if (string.IsNullOrEmpty(RMSName)) return;
            if (IsFloatingPoint && Utils.TryLoadDataDouble(RMSName, out double data))
                SelectedValue = data;
            else if (Utils.TryLoadDataLong(RMSName, out long data2))
                SelectedValue = data2;
        }

        internal void action()
        {
            HM9rMenu.showPopup = false;
            if (IsDisabled)
            {
                GameScr.info1.addInfo(GetDisabledReason(), 0);
                return;
            }
            if (Values != null)
                SwitchSelection();
            else
                StartChat(currentCTF);
            saveData();
        }

        internal void StartChat(ChatTextField textField)
        {
            currentCTF = textField;
            textField.tfChat.y = GameCanvas.h - 35 - ChatTextField.gI().tfChat.height;
            textField.initChatTextField();
            textField.strChat = string.Empty;
            textField.tfChat.name = TextFieldHint;
            textField.tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
            textField.startChat2(this, TextFieldName);
            textField.tfChat.setText(SelectedValue.ToString());
        }

        public void onChatFromMe(string text, string to)
        {
            if (string.IsNullOrEmpty(text) || to != TextFieldName)
            {
                onCancelChat();
                return;
            }
            text = text.Replace('.', ',');
            double value = 0;
            bool isNumber = double.TryParse(text, out value);
            if (isNumber)
            {
                int value2 = 0;
                isNumber = IsFloatingPoint || int.TryParse(text, out value2);
                if (!IsFloatingPoint && isNumber)
                    value = value2;
            }
            if (isNumber)
            {
                if (MinValue != MaxValue && (value < MinValue || value > MaxValue))
                    GameCanvas.startOKDlg(string.Format(Strings.inputNumberOutOfRange, MinValue, MaxValue) + '!');
                else
                {
                    SelectedValue = value;
                    GameScr.info1.addInfo(string.Format(Strings.valueChanged, Title, SelectedValue) + '!', 0);
                }
            }
            else
                GameCanvas.startOKDlg(Strings.invalidValue + '!');
            onCancelChat();
        }

        public void onCancelChat() => currentCTF.ResetTF();
    }
}
