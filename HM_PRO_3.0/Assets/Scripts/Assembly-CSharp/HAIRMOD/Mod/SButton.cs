using Assets.src.e;
using System;

namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod
{
    internal class SButton
    {
        internal string caption;
        internal int small;
        internal Action action;
        internal int x, y, w = GameScr.imgNut.getWidth(), h = GameScr.imgNut.getHeight();
        internal bool isFocus;
        internal SButton(string caption, int small, Action action)
        {
            this.caption = caption;
            this.small = small;
            this.action = action;
        } 
        internal void Paint(mGraphics g)
        {
            g.drawImage(isFocus ? GameScr.imgNutF : GameScr.imgNut, x, y);
            if(caption != "")
            {
                mFont.tahoma_7b_white.drawString(g, caption, x + w / 2, y + h / 2 - mFont.tahoma_7b_white.getHeight() / 2, 3);
            }
            if(small != -1)
            {
                Small s = SmallImage.imgNew[small];
                if(s == null)
                {
                    SmallImage.createImage(small);
                    return;
                }
                SmallImage.drawSmallImage(g, small, x + w / 2, y + h / 2, 0, 3);
            }
        }
        internal bool Pressed()
        {
            isFocus = false;
            if(GameCanvas.isPointerHoldIn(x, y, w, h))
            {
                if (GameCanvas.isPointerDown)
                {
                    isFocus = true;
                }
                if(GameCanvas.isPointerClick && GameCanvas.isPointerJustRelease)
                {
                    return true;
                }
            }
            return false;
        }
        internal void actionPerform()
        {
            GameCanvas.clearAllPointerEvent();
            if (action != null) action();
        }
    }
}
