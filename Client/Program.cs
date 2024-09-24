using ChatStream;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System;

namespace Client
{
	public class Program
	{
		static void Main(string[] args)
		{
			TcpClient client = new TcpClient();

			client.Connect(Constants.Address, Constants.PORT);

			Console.WriteLine("Connected to the server!");

			
			Messenger inStream = new Messenger();
			Messenger outStream = new Messenger();

			Console.Write("Enter your nickname: ");
			string nickname = Console.ReadLine();


			var nicknamePacket = outStream.CreateMessagePacket(0, nickname);
			client.GetStream().Write(nicknamePacket, 0, nicknamePacket.Length);


			List<string> outgoingMessages = new List<string>();

			new TaskFactory().StartNew(() =>
			{
				while (true)
				{
					var msg = Console.ReadLine();
					outgoingMessages.Add(msg);
				}
			});

			while (true)
			{
				ReadPackets();
				SendPackets();
			}

			void ReadPackets()
			{
				var stream = client.GetStream();
				for (int i = 0; i < 10; i++)
				{
					if (stream.DataAvailable)
					{
						byte[] buffer = new byte[client.ReceiveBufferSize];

						int bytesRead = stream.Read(buffer, 0, buffer.Length);

						(int opcode, string message) = inStream.ParseMessagePacket(buffer.Take(bytesRead).ToArray());

						Console.WriteLine($"Received: [{opcode}] - {message}");
					}
				}
			}

			void SendPackets()
			{
				if (outgoingMessages.Count > 0)
				{
					var msg = outgoingMessages[0];

					var packet = outStream.CreateMessagePacket(10, msg);

					client.GetStream().Write(packet, 0, packet.Length);

					outgoingMessages.RemoveAt(0);
				}
			}

		}
	}
}
