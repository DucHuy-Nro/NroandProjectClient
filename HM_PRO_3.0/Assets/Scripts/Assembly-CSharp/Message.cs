using Assets.Scripts.Assembly_CSharp.HAIRMOD.Mod;

public class Message
{
	public sbyte command;

	private myReader dis;

	private myWriter dos;

	public Message(int command)
	{
		this.command = (sbyte)command;
		dos = new myWriter();
	}

	public Message()
	{
		dos = new myWriter();
	}

	public Message(sbyte command)
	{
		this.command = command;
		dos = new myWriter();
	}

	public Message(sbyte command, sbyte[] data)
	{
		this.command = command;
		dis = new myReader(data);
	}

	public sbyte[] getData()
	{
		return dos.getData();
	}

	public myReader reader()
	{
		return dis;
	}

	public myWriter writer()
	{
		return dos;
	}

	public int readInt3Byte()
	{
		return dis.readInt();
	}
	public long readLong3Byte()
	{
		if (HM9r329.checkTypeData) return dis.readInt();
		return dis.readLong();
	}
	public void cleanup()
	{
	}
}
