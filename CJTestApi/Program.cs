using System;
using Microsoft.Owin.Hosting;

namespace CJTestApi
{
	internal class Program
	{
		static IDisposable _server;

		static void Main(string[] args)
		{
			_server = WebApp.Start<Startup>("http://localhost:9001");
			Console.WriteLine("Server läuft");
			Console.ReadLine();
			_server.Dispose();
		}
	}
}