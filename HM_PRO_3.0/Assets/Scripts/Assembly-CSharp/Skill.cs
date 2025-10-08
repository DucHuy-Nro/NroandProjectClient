public class Skill
{
    public const sbyte ATT_STAND = 0;

    public const sbyte ATT_FLY = 1;

    public const sbyte SKILL_AUTO_USE = 0;

    public const sbyte SKILL_CLICK_USE_ATTACK = 1;

    public const sbyte SKILL_CLICK_USE_BUFF = 2;

    public const sbyte SKILL_CLICK_NPC = 3;

    public const sbyte SKILL_CLICK_LIVE = 4;

    public SkillTemplate template;

    public short skillId;

    public int point;

    public long powRequire;

    public int coolDown;

    public long lastTimeUseThisSkill;

    public int dx;

    public int dy;

    public int maxFight;

    public int manaUse;

    public SkillOption[] options;

    public bool paintCanNotUseSkill;

    public short damage;

    public string moreInfo;

    public short price;

    public short curExp;

    public string strCurExp()
    {
        if (curExp / 10 >= 100)
        {
            return "MAX";
        }
        if (curExp % 10 == 0)
        {
            return curExp / 10 + "%";
        }
        int num = curExp % 10;
        return curExp / 10 + "." + num % 10 + "%";
    }

    public string strTimeReplay()
    {
        if (coolDown % 1000 == 0)
        {
            return coolDown / 1000 + string.Empty;
        }
        int num = coolDown % 1000;
        return coolDown / 1000 + "." + ((num % 100 != 0) ? (num / 10) : (num / 100));
    }

    public void paint(int x, int y, mGraphics g)
    {
        SmallImage.drawSmallImage(g, template.iconId, x, y, 0, StaticObj.VCENTER_HCENTER);
        long num = mSystem.currentTimeMillis() - this.lastTimeUseThisSkill;
        if (num >= (long)this.coolDown)
        {
            this.paintCanNotUseSkill = false;
            return;
        }
        this.paintCanNotUseSkill = true;
        try
        {
            int num2 = (int)(((long)this.coolDown - num) * 84L / (long)this.coolDown);
            long num3 = (long)this.coolDown - num;
            g.setColor(2721889, 0.7f);
            if (this.paintCanNotUseSkill && GameCanvas.gameTick % 6 > 2)
            {
                g.setColor(876862);
            }
            int num4 = (int)(num * 20L / (long)this.coolDown);
            g.fillRect(x - 10, y - 10 + num4, 20, 20 - num4);
            if (num3 > 10000L)
            {
                mFont.tahoma_7b_white.drawString(g, NinjaUtil.getMoneys(num3).Split(new char[] { '.' })[0], x + 1, y - 6, 2, mFont.tahoma_7);
            }
            else if (num3 > 1000L)
            {
                mFont.tahoma_7b_white.drawString(g, NinjaUtil.getMoneys(num3).Substring(0, 3), x + 1, y - 6, 2, mFont.tahoma_7);
            }
            else
            {
                mFont.tahoma_7b_white.drawString(g, "0." + num3.ToString().Substring(0, 2), x + 1, y - 6, 2, mFont.tahoma_7);
            }
        }
        catch
        {
        }
    }
}
