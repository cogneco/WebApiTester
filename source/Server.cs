using System;
using Tasks = System.Threading.Tasks;

namespace Cogneco.WebApiTester
{
		public abstract class Server : IDisposable
		{
			public string Listen { get; }
			protected Server(string listen)
			{
				this.Listen = listen;
			}
			public abstract Tasks.Task Stop();
			void IDisposable.Dispose() => this.Stop().Wait();
			static int port = 5100;
			protected static int NextPort() => Server.port++;
		}
}