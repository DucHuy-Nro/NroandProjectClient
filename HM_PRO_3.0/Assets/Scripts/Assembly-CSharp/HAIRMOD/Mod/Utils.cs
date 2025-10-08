using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.ModHelper.CommandMod.Chat;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.ModHelper.CommandMod.Hotkey;
using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Object;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod
{
    internal class Utils
    {
        internal static readonly short ID_ICON_ITEM_TDLT = 4387;

        static string persistentDataPath = Application.persistentDataPath;
        internal static readonly string dataPath = Path.Combine(GetRootDataPath(), "CommonModData");
        internal static string PersistentDataPath => persistentDataPath;
        internal static readonly string PathHotkeyCommand = Path.Combine(dataPath, "hotkeyCommands.json");
        internal static readonly string PathChatCommand = Path.Combine(dataPath, "chatCommands.json");
        internal static readonly sbyte ID_SKILL_BUFF = 7;

        internal static Waypoint waypointLeft;
        internal static Waypoint waypointMiddle;
        internal static Waypoint waypointRight;
        internal static string GetRootDataPath()
        {
            string result = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Data");
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                result = PersistentDataPath;
            return result;
        }
        internal static bool CanNextMap() => !Char.isLoadingMap && !Char.ischangingMap && !Controller.isStopReadMessage;
        internal static void TeleportMyChar(int x, int y)
        {
            Char.myCharz().currentMovePoint = null;
            Char.myCharz().cx = x;
            Char.myCharz().cy = y;
            Service.gI().charMove();

            if (isUsingTDLT())
                return;

            Char.myCharz().cx = x;
            Char.myCharz().cy = y + 1;
            Service.gI().charMove();
            Char.myCharz().cx = x;
            Char.myCharz().cy = y;
            Service.gI().charMove();
        }
         static Texture2D Resize(Texture2D texture2D, int targetX, int targetY)
        {
            RenderTexture rt = new RenderTexture(targetX, targetY, 24);
            RenderTexture.active = rt;
            UnityEngine.Graphics.Blit(texture2D, rt);
            Texture2D result = new Texture2D(targetX, targetY);
            result.ReadPixels(new Rect(0, 0, targetX, targetY), 0, 0);
            result.Apply();
            return result;
        }
        internal static void resizeImage(Image img, byte zoomLevel)
        {
            int wx1 = img.w / zoomLevel;
            int hx1 = img.h / zoomLevel;
            int w = wx1 * mGraphics.zoomLevel;
            int h = hx1 * mGraphics.zoomLevel;
            img.texture = Resize(img.texture, w, h);
            img.w = img.texture.width;
            img.h = img.texture.height;
            Image.setTextureQuality(img.texture);
        }
        internal static void CheckBackButtonPress()
        {
            if (GameCanvas.panel != null || GameCanvas.panel2 != null)
            {
                if (GameCanvas.panel != null && GameCanvas.panel.isShow)
                {
                    GameCanvas.panel.hide();
                    return;
                }
                if (GameCanvas.panel2 != null && GameCanvas.panel2.isShow)
                {
                    GameCanvas.panel2.hide();
                    return;
                }
            }
            if (InfoDlg.isShow)
                return;
            if (GameCanvas.currentDialog != null && GameCanvas.currentDialog is MsgDlg)
            {
                GameCanvas.endDlg();
                return;
            }
            if (ChatTextField.gI().isShow)
            {
                ChatTextField.gI().close();
                return;
            }
            if (GameCanvas.menu.showMenu)
            {
                GameCanvas.menu.closeMenu();
                return;
            }
            GameCanvas.checkBackButton();
        }
        internal static void TeleportMyChar(int x)
        {
            TeleportMyChar(x, GetYGround(x));
        }

        internal static bool isUsingTDLT() =>
                  ItemTime.isExistItem(ID_ICON_ITEM_TDLT);
        internal static string getTextPopup(PopUp popUp)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < popUp.says.Length; i++)
            {
                stringBuilder.Append(popUp.says[i]);
                stringBuilder.Append(" ");
            }
            return stringBuilder.ToString().Trim();
        }
        internal static void ChangeMap(Waypoint waypoint)
        {
            if (waypoint != null)
            {
                TeleportMyChar(waypoint.GetX(), waypoint.GetY());
                requestChangeMap(waypoint);
            }
        }
        internal static void requestChangeMap(Waypoint waypoint)
        {
            if (waypoint.isOffline)
            {
                Service.gI().getMapOffline();
                return;
            }

            Service.gI().requestChangeMap();
        }
        internal static int Distance(IMapObject mapObject1, IMapObject mapObject2)
        {
            return Res.distance(mapObject1.getX(), mapObject1.getY(), mapObject2.getX(), mapObject2.getY());
        }
        internal static double Distance(double x1, double y1, double x2, double y2) => Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        internal static string ToggleStringRefByBoolean(string caption, bool a)
        {
            return caption + ": " + (a ? "[ON]" : "[OFF]");
        }
        internal static void SetToggleRefByBoolean(Action acton, ref bool a)
        {
            a = !a;
            acton();
        }
        internal static string GetToggleStringMenu(string caption, ref bool a)
        {
            return caption + "\n" + (a ? "[ON]" : "[OFF]");
        }
        internal static string GetToggleStringMenuOutRef(string caption, bool a)
        {
            return caption + "\n" + (a ? "[ON]" : "[OFF]");
        }
        internal static int GetYGround(int x)
        {
            int y = 50;
            for (int i = 0; i < 30; i++)
            {
                y += 24;
                if (TileMap.tileTypeAt(x, y, 2))
                {
                    if (y % 24 != 0)
                        y -= y % 24;
                    break;
                }
            }
            return y;
        }
        internal static MyVector getMyVectorMe()
        {
            var vMe = new MyVector();
            vMe.addElement(Char.myCharz());
            return vMe;
        }
        internal static void DoDoubleClickToObj(IMapObject mapObject) => GameScr.gI().doDoubleClickToObj(mapObject);
        internal static bool IsMyCharHome() => TileMap.mapID == Char.myCharz().cgender + 21;
        internal static bool TryLoadDataBool(string name, out bool value, bool isCommon = true)
        {
            value = false; // Giá trị mặc định nếu file không tồn tại

            try
            {
                if (!File.Exists(Path.Combine(isCommon ? dataPath : Path.Combine(Rms.GetiPhoneDocumentsPath(), "ModData"), name)))
                    return false; // Không có file -> không kiểm tra

                value = LoadDataBool(name, isCommon);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            return false;
        }

        internal static bool LoadDataBool(string name, bool isCommon = true)
        {
            string path = dataPath;
            if (!isCommon)
                path = Path.Combine(Rms.GetiPhoneDocumentsPath(), "ModData");

            string filePath = Path.Combine(path, name);

            // Kiểm tra nếu file không tồn tại, trả về false
            if (!File.Exists(filePath))
                return false;

            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            byte[] array = new byte[1];
            fileStream.Read(array, 0, 1);
            fileStream.Close();

            return array[0] == 1;
        }

        internal static void SaveData(string name, bool status, bool isCommon = true)
        {
            string path = dataPath;
            if (!isCommon)
                path = Path.Combine(Rms.GetiPhoneDocumentsPath(), "ModData");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            FileStream fileStream = new FileStream(Path.Combine(path, name), FileMode.Create);
            fileStream.Write(new byte[] { (byte)(status ? 1 : 0) }, 0, 1);
            fileStream.Flush();
            fileStream.Close();
        }
        internal static void SaveData(string name, string data, bool isCommon = true)
        {
            try
            {
                string path = GetPath(isCommon);
                EnsureDirectoryExists(path);
                File.WriteAllText(Path.Combine(path, name), data, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveData] Error saving string: {ex}");
            }
        }

        internal static void SaveData(string name, double value, bool isCommon = true)
        {
            try
            {
                string path = GetPath(isCommon);
                EnsureDirectoryExists(path);
                File.WriteAllBytes(Path.Combine(path, name), BitConverter.GetBytes(value));
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveData] Error saving double: {ex}");
            }
        }

        internal static void SaveData(string name, long value, bool isCommon = true)
        {
            try
            {
                string path = GetPath(isCommon);
                EnsureDirectoryExists(path);
                File.WriteAllBytes(Path.Combine(path, name), BitConverter.GetBytes(value));
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveData] Error saving long: {ex}");
            }
        }

        internal static long LoadDataLong(string name, bool isCommon = true)
        {
            try
            {
                string path = GetPath(isCommon);
                string filePath = Path.Combine(path, name);

                if (!File.Exists(filePath))
                    return default;

                byte[] data = File.ReadAllBytes(filePath);
                return BitConverter.ToInt64(data, 0);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[LoadDataLong] Error: {ex}");
                return default;
            }
        }
        internal static int LoadDataInt(string name, bool isCommon = true)
        {
            try
            {
                string path = GetPath(isCommon);
                string filePath = Path.Combine(path, name);

                if (!File.Exists(filePath))
                    return default;

                byte[] data = File.ReadAllBytes(filePath);
                return BitConverter.ToInt32(data, 0);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[LoadDataLong] Error: {ex}");
                return default;
            }
        }
        internal static string LoadDataString(string name, bool isCommon = true)
        {
            try
            {
                string path = GetPath(isCommon);
                string filePath = Path.Combine(path, name);

                if (!File.Exists(filePath))
                    return default;

                return File.ReadAllText(filePath, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[LoadDataString] Error: {ex}");
                return default;
            }
        }

        internal static double LoadDataDouble(string name, bool isCommon = true)
        {
            try
            {
                string path = GetPath(isCommon);
                string filePath = Path.Combine(path, name);

                if (!File.Exists(filePath))
                    return default;

                byte[] data = File.ReadAllBytes(filePath);
                return BitConverter.ToDouble(data, 0);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[LoadDataDouble] Error: {ex}");
                return default;
            }
        }

        internal static bool TryLoadDataLong(string name, out long value, bool isCommon = true)
        {
            value = default;
            try
            {
                value = LoadDataLong(name, isCommon);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[TryLoadDataLong] Error: {ex}");
                return false;
            }
        }
        internal static bool TryLoadDataInt(string name, out int value, bool isCommon = true)
        {
            value = default;
            try
            {
                value = LoadDataInt(name, isCommon);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[TryLoadDataLong] Error: {ex}");
                return false;
            }
        }
        internal static bool TryLoadDataString(string name, out string value, bool isCommon = true)
        {
            value = default;
            try
            {
                value = LoadDataString(name, isCommon);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[TryLoadDataString] Error: {ex}");
                return false;
            }
        }

        internal static bool TryLoadDataDouble(string name, out double value, bool isCommon = true)
        {
            value = default;
            try
            {
                value = LoadDataDouble(name, isCommon);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[TryLoadDataDouble] Error: {ex}");
                return false;
            }
        }

        internal static string GetPath(bool isCommon)
        {
            return isCommon ? dataPath : Path.Combine(Rms.GetiPhoneDocumentsPath(), "ModData");
        }

        internal static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        [ChatCommand("hsme"), ChatCommand("buffme"), HotkeyCommand('b')]
        internal static void buffMe()
        {
            if (!canBuffMe(out Skill skillBuff))
            {
                GameScr.info1.addInfo("Không tìm thấy kỹ năng Trị thương", 0);
                return;
            }

            // Đổi sang skill hồi sinh
            Service.gI().selectSkill(ID_SKILL_BUFF);

            // Tự tấn công vào bản thân
            Service.gI().sendPlayerAttack(new MyVector(), getMyVectorMe(), -1);

            // Trả về skill cũ
            Service.gI().selectSkill(Char.myCharz().myskill.template.id);

            // Đặt thời gian hồi cho skill
            skillBuff.lastTimeUseThisSkill = mSystem.currentTimeMillis();
        }
        internal static bool canBuffMe(out Skill skillBuff)
        {
            skillBuff = Char.myCharz().
                getSkill(new SkillTemplate { id = ID_SKILL_BUFF });

            if (skillBuff == null)
            {
                return false;
            }

            return true;
        }
        public static string RichText(List<string> s, List<string> color)
        {
            string result = "";
            for (int i = 0; i < s.Count; i++)
            {
                result += $"<color={color[i]}>{s[i]}</color> ";
            }
            return result;
        }

        internal static bool IsMeInNRDMap() => TileMap.mapID >= 85 && TileMap.mapID <= 91;

        public static void setWaypointChangeMap(Waypoint waypoint)
        {
            int cMapID = TileMap.mapID;
            var textPopup = getTextPopup(waypoint.popup);

            if (cMapID == 27 && textPopup == "Tường thành 1")
                return;

            if (cMapID == 70 && textPopup == "Vực cấm" ||
                cMapID == 73 && textPopup == "Vực chết" ||
                cMapID == 110 && textPopup == "Rừng tuyết")
            {
                waypointLeft = waypoint;
                return;
            }

            if (((cMapID == 106 || cMapID == 107) && textPopup == "Hang băng") ||
                ((cMapID == 105 || cMapID == 108) && textPopup == "Rừng băng") ||
                (cMapID == 109 && textPopup == "Cánh đồng tuyết"))
            {
                waypointMiddle = waypoint;
                return;
            }

            if (cMapID == 70 && textPopup == "Căn cứ Raspberry")
            {
                waypointRight = waypoint;
                return;
            }

            if (waypoint.maxX < 60)
            {
                waypointLeft = waypoint;
                return;
            }

            if (waypoint.minX > TileMap.pxw - 60)
            {
                waypointRight = waypoint;
                return;
            }

            waypointMiddle = waypoint;
        }

        public static void updateWaypointChangeMap()
        {
            waypointLeft = waypointMiddle = waypointRight = null;

            var vGoSize = TileMap.vGo.size();
            for (int i = 0; i < vGoSize; i++)
            {
                Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(i);
                setWaypointChangeMap(waypoint);
            }
        }
        [HotkeyCommand('j')]
        internal static void ChangeMapLeft()
        {
            if (IsMeInNRDMap() || waypointLeft == null)
                TeleportMyChar(60);
            else
                ChangeMap(waypointLeft);
        }

        [HotkeyCommand('k')]
        internal static void ChangeMapMiddle()
        {
            if (IsMeInNRDMap())
            {
                if (Char.myCharz().bag >= 0 && ClanImage.idImages.containsKey(Char.myCharz().bag.ToString()))
                {
                    ClanImage clanImage = (ClanImage)ClanImage.idImages.get(Char.myCharz().bag.ToString());
                    if (clanImage.idImage != null)
                    {
                        for (int i = 0; i < clanImage.idImage.Length; i++)
                        {
                            if (clanImage.idImage[i] == 2322)
                            {
                                for (int j = 0; j < GameScr.vNpc.size(); j++)
                                {
                                    Npc npc = (Npc)GameScr.vNpc.elementAt(j);
                                    if (npc.template.npcTemplateId >= 30 && npc.template.npcTemplateId <= 36)
                                    {
                                        Char.myCharz().npcFocus = npc;
                                        TeleportMyChar(npc.cx, npc.cy - 3);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                for (int k = 0; k < GameScr.vItemMap.size(); k++)
                {
                    ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(k);
                    if (itemMap != null && itemMap.IsNRD())
                    {
                        Char.myCharz().itemFocus = itemMap;
                        TeleportMyChar(itemMap.x, itemMap.y);
                        return;
                    }
                }
            }
            else if (waypointMiddle == null)
                TeleportMyChar(TileMap.pxw / 2);
            else
                ChangeMap(waypointMiddle);
        }

        [HotkeyCommand('l')]
        internal static void ChangeMapRight()
        {
            if (IsMeInNRDMap() || waypointRight == null)
                TeleportMyChar(TileMap.pxw - 60);
            else
                ChangeMap(waypointRight);
        }
        [HotkeyCommand('g')]
        internal static void sendGiaoDichToCharFocusing()
        {
            var charFocus = Char.myCharz().charFocus;
            if (charFocus == null)
            {
                GameScr.info1.addInfo("Trỏ vào nhân vật để giao dịch", 0);
                return;
            }

            Service.gI().giaodich(0, charFocus.charID, -1, -1);
            GameScr.info1.addInfo("Đã gửi lời mời giao dịch đến " + charFocus.cName, 0);
        }

        [ChatCommand("k")]
        internal static void changeZone(int zone)
        {
            Service.gI().requestChangeZone(zone, -1);
        }
        [ChatCommand("cheat")]
        internal static void changeGameSpeed(int speed)
        {
            Time.timeScale = speed;
            GameScr.info1.addInfo("Tốc Độ Game: " + speed, 0);
        }
    }
}
