using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Object;
using System;
namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Menu
{
    internal class MenuBoolean : MenuItem
    {
        internal bool Value
        {
            get => GetValueFunc();
            set => SetValueAction(value);
        }

        /// <summary>Tên tệp lưu dữ liệu</summary>
        internal string RMSName => _config.RMSName;
        /// <summary>Hàm được gọi để lấy giá trị hiện tại, không có đối số và trả về giá trị kiểu <see cref="bool"/>.</summary>
        internal Func<bool> GetValueFunc => _config.GetValueFunc;
        /// <summary>Hàm được gọi để gán giá trị mới, có 1 đối số kiểu <see cref="bool"/> và không trả về giá trị.</summary>
        internal Action<bool> SetValueAction => _config.SetValueAction;

        MenuBooleanConfig _config;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">Cấu hình <see cref="ModMenuItemBoolean"/></param>
        internal MenuBoolean(MenuBooleanConfig config) : base(config)
        {
            _config = config;
        }
        internal void action()
        {
            if (IsDisabled)
            {
                GameScr.info1.addInfo(GetDisabledReason(), 0);
                return;
            }
            SwitchSelection();
            GameScr.info1.addInfo(Title + ": " + Strings.OnOffStatus(Value), 0);
            saveData();

        }

        internal void loadData()
        {
            if (!string.IsNullOrEmpty(RMSName) && Utils.TryLoadDataBool(RMSName, out bool value))
                Value = value;
        }
        internal void saveData()
        {
            if (!string.IsNullOrEmpty(RMSName))
                Utils.SaveData(RMSName, Value);

        }
        internal void SwitchSelection() => SetValueAction(!GetValueFunc());
    }
}
