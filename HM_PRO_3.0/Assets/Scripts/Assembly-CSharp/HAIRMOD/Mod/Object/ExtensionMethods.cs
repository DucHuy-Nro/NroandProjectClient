namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Object
{
    internal static class ExtensionMethods
    {
        internal static bool IsCharDead(this Char ch) => ch.isDie || ch.cHP <= 0 || ch.statusMe == 14;
        internal static void ResetTF(this ChatTextField tf)
        {
            tf.strChat = "Chat";
            tf.tfChat.name = "chat";
            tf.to = "";
            tf.tfChat.setIputType(TField.INPUT_TYPE_ANY);
            tf.isShow = false;
            tf.parentScreen = GameScr.gI();
        }
        internal static int GetX(this Waypoint waypoint)
        {
            return waypoint.maxX < 60 ? 15 :
                waypoint.minX > TileMap.pxw - 60 ? TileMap.pxw - 15 :
                waypoint.minX + ((waypoint.maxX - waypoint.minX) / 2);
        }

        internal static int GetXInsideMap(this Waypoint waypoint)
        {
            return waypoint.maxX < TileMap.size ? TileMap.size :
                waypoint.minX > TileMap.pxw - TileMap.size ? TileMap.pxw - TileMap.size :
                waypoint.minX + ((waypoint.maxX - waypoint.minX) / 2);
        }

        internal static int GetY(this Waypoint waypoint)
        {
            return waypoint.maxY;
        }
        internal static bool isBOSS(this Char @char)
        {
            return (@char != null && !@char.isPet && !@char.isMiniPet && @char.charID < 0 && @char.cTypePk == 5 && !@char.cName.ToLower().Contains("trọng tài"));
        }
        internal static bool IsNRD(this ItemMap item) => item.template.id >= 372 && item.template.id <= 378;
        internal static string CharCheck(this Char ch)
        {
            bool flag = !ch.cName.StartsWith("[");
            string result;
            if (flag)
            {
                result = "";
            }
            else
            {
                result = ch.cName.Substring(0, ch.cName.IndexOf("]") + 1);
            }
            return result;
        }
    }
}
