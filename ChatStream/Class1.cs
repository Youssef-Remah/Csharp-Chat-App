using System.IO;
using System.Net;
using System.Text;

namespace ChatStream
{
    public class Constants
    {
		public static int PORT = 55555;

		public static IPAddress Address = IPAddress.Loopback;
	}

	public class Messenger
	{
		public byte[] CreateMessagePacket(int opcode, string message)
		{
			using MemoryStream ms = new MemoryStream();

			using BinaryWriter writer = new BinaryWriter(ms);

			writer.Write(opcode);

			writer.Write(message);

			return ms.ToArray();
		}

		public (int opcode, string message) ParseMessagePacket(byte[] data)
		{
			using MemoryStream ms = new MemoryStream(data);

			using BinaryReader reader = new BinaryReader(ms, Encoding.ASCII);

			int opcode = reader.ReadInt32();

			string message = reader.ReadString();

			return (opcode, message);
		}
	}
}
