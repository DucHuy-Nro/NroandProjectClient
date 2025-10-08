using System;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Menu
{
    internal class MenuItem
    {
        /// <summary>ID</summary>
        internal string ID => _config.ID;
        /// <summary>Tiêu đề</summary>
        internal string Title => _config.Title;

        /// <summary>Hàm được gọi để lấy giá trị của trạng thái vô hiệu hóa, không có đối số và trả về giá trị kiểu <see cref="bool"/>.</summary>
        internal Func<bool> GetIsDisabled => _config.GetIsDisabled;
        /// <summary>Hàm trả về lý do bị vô hiệu hóa, được gọi khi ModMenuItem được chọn bị vô hiệu hóa.</summary>
        internal Func<string> GetDisabledReason => _config.GetDisabledReason;

        /// <summary>Trạng thái vô hiệu hóa</summary>
        internal bool IsDisabled => GetIsDisabled != null && GetIsDisabled();
        /// <summary>Lý do bị vô hiệu hóa</summary>
        internal string DisabledReason => GetDisabledReason == null ? "" : GetDisabledReason();

        MenuConfig _config;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">Cấu hình <see cref="ModMenuItem"/></param>
        internal MenuItem(MenuConfig config)
        {
            _config = config;
        }
    }
}
