using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace HTTPServerConsole
{
	public class Server : IDisposable
	{

		TcpListener Listener;

		public Server(int Port, NLog.Logger logger)
		{

			Listener = new TcpListener(IPAddress.Any, Port);
			{
				Listener.Start();
				hsLogger.Start();
			}

			while (true)
			{
				TcpClient Client = Listener.AcceptTcpClient();
				Thread Thread = new Thread(new ParameterizedThreadStart(ClientThread));
				Thread.Start(Client);

				hsLogger.Connected(Client.Client.RemoteEndPoint.ToString()); //лог
			}
		}

		public void Dispose()
		{
			hsLogger.Close();
		}



		static void ClientThread(Object StateInfo)
		{
			new Client((TcpClient)StateInfo);
		}
	}
}