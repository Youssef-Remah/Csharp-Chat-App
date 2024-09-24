using ChatStream;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System;

namespace Server
{
	public class Program
	{
		static Dictionary<TcpClient, string> clientNicknames = new Dictionary<TcpClient, string>();

		static void Main(string[] args)
		{
			var inStream = new Messenger();

			var outStream = new Messenger();


			List<TcpClient> _clients = new List<TcpClient>();

			TcpListener listener = new TcpListener(Constants.Address, Constants.PORT);
			listener.Start();

			Console.WriteLine("Server started!");

			while (true)
			{			
				AcceptClients();

				ReceiveMessage();
			}

			void AcceptClients()
			{
				for (int i = 0; i < 5; i++)
				{
					if (!listener.Pending()) continue;

					var client = listener.AcceptTcpClient();

					_clients.Add(client);

					Console.WriteLine("Client accepted!");

					
					NetworkStream stream = client.GetStream();

					byte[] buffer = new byte[client.ReceiveBufferSize];

					int bytesRead = stream.Read(buffer, 0, buffer.Length);

					(int opcode, string nickname) = inStream.ParseMessagePacket(buffer.Take(bytesRead).ToArray());

					clientNicknames[client] = nickname;

					Console.WriteLine($"Client nickname set: {nickname}");
				}
			}

			void ReceiveMessage()
			{
				foreach (var client in _clients)
				{
					NetworkStream stream = client.GetStream();

					if (stream.DataAvailable)
					{
						byte[] buffer = new byte[client.ReceiveBufferSize];

						int bytesRead = stream.Read(buffer, 0, buffer.Length);

						(int opcode, string message) = inStream.ParseMessagePacket(buffer.Take(bytesRead).ToArray());

						string clientNickname = clientNicknames[client];

						Console.WriteLine($"Received from {clientNickname}: [{opcode}] - {message}");

						Broadcast(client, $"{clientNickname}: {message}");
					}
				}
			}

			void Broadcast(TcpClient sender, string message)
			{
				foreach (var client in _clients.Where(x => x != sender))
				{
					var packet = outStream.CreateMessagePacket(10, message);

					client.GetStream().Write(packet, 0, packet.Length);
				}
			}
		}
	}
}
