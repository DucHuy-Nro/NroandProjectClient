using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Logger;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.ModHelper.CommandMod.Chat;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.ModHelper.CommandMod.Hotkey
{
    public class HotkeyCommandHandler
    {
        public static List<HotkeyCommand> hotkeyCommands = new List<HotkeyCommand>();

        /// <summary>
        /// Tải phím tắt mặc định.
        /// </summary>
        public static void loadDefault()
        {
            var methods = CommandUtils.GetMethods();

            for (int i = 0; i < methods.Length; i++)
            {
                var method = methods[i];
                var attributes = Attribute.GetCustomAttributes(method, typeof(HotkeyCommandAttribute));
                foreach (var attribute in attributes)
                {
                    if (attribute is HotkeyCommandAttribute kca)
                    {
                        var hotkeyCommand = new HotkeyCommand()
                        {
                            key = kca.key,
                            delimiter = kca.delimiter,
                            fullCommand = method.DeclaringType.FullName + "." + method.Name,
                            method = method,
                            parameterInfos = method.GetParameters(),
                        };
                        if (hotkeyCommand.canExecute(kca.agrs, out hotkeyCommand.parameters))
                        {
                            hotkeyCommands.Add(hotkeyCommand);
                        }
                    }
                }
            }

            save();
        }

        /// <summary>
        /// Lưu phím tắt.
        /// </summary>
        public static void save()
        {
            try
            {
                string directory = Path.GetDirectoryName(Utils.PathHotkeyCommand);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                if (!File.Exists(Utils.PathHotkeyCommand))
                {
                    using (File.Create(Utils.PathHotkeyCommand)) { }
                }
                File.WriteAllText(Utils.PathHotkeyCommand, JsonConvert.SerializeObject(hotkeyCommands));
            }
            catch (Exception ex)
            {
                ConsoleLogger.LogError($"Error saving file: {ex.Message}", "magenta");
            }
        }
      
        /// <summary>
        /// Xử lý phím nhấn.
        /// </summary>
        /// <param name="key">Mã ASCII phím được nhấn.</param>
        /// <returns>true nếu có lệnh được thực hiện thành công.</returns>
        public static bool handleHotkey(int key)
        {
            foreach (var h in hotkeyCommands)
            {
                if (h.key == key)
                {
                    h.execute();
                    return true;
                }
            }

            return false;
        }
    }
}
