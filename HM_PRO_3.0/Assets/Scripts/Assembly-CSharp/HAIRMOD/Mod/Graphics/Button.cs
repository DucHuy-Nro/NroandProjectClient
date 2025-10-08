using System;
namespace Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod.Graphics
{
    internal class Button
    {
        internal static Image title, title_select;
        internal string caption;
        internal Action action;
        internal bool select;
        internal int x, y;
        delegate int Position(bool select);
        internal Button(string caption, Action action, bool select)
        {
            this.caption = caption;
            this.action = action;
            this.select = select;
        }
        internal void paint(mGraphics g)
        {
            g.drawImage(select ? title_select : title, x, y + (select ? ((GameCanvas.gameTick % 10 >= 7) ? 0 : -1) : 0));
            Position positionX = new Position(myXPos);
            Position positionY = new Position(myYPos);
            mFont mFont = select ? mFont.tahoma_7b_green2 : mFont.tahoma_7b_dark;
            mFont.drawString(g, caption, positionX(select) + 1, positionY(select) - mFont.getHeight() / 2 + (select ? ((GameCanvas.gameTick % 10 >= 7) ? 0 : -1) : 0), 3);
        }
        internal void paintNor(mGraphics g)
        {
            g.drawImage(select ? title_select : title, x, y  );
            Position positionX = new Position(myXPos);
            Position positionY = new Position(myYPos);
            mFont mFont = select ? mFont.tahoma_7b_green2 : mFont.tahoma_7b_dark;
            mFont.drawString(g, caption, positionX(select) + 1, positionY(select) - mFont.getHeight() / 2 , 3);
        }
        internal bool IsPressedInSide()
        {
            if(GameCanvas.isPointerHoldIn(x, y, title.getWidth(), title.getHeight()))
            {
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
        private int myXPos(bool select)
        {
            return select ? x + title_select.getWidth() / 2 : x + title.getWidth() / 2; 
        }
        private int myYPos(bool select)
        {
            return select ? y + title_select.getHeight() / 2 : y + title.getHeight() / 2;
        }
        internal int myWPos(bool select)
        {
            return select ? title_select.getWidth() : title.getWidth();
        }
        internal int myHPos(bool select)
        {
            return select ? title_select.getHeight() : title.getHeight(); 
        }
    }
}
