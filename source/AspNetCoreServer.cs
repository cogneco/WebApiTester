using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Cogneco.WebApiTester
{
	public class AspNetCoreServer : Server
	{
		IWebHost host;
		AspNetCoreServer(IWebHost host, string listen) : base(listen)
		{
			this.host = host;
		}
		public override async Task Stop()
		{
			if (this.host != null)
				await this.host.StopAsync();
			this.host = null;
		}
			public static async Task<Fixture> Start<T>() where T: class
			{
				string listen = "http://localhost:" + AspNetCoreServer.NextPort();
				var host = WebHost.CreateDefaultBuilder(new string[0])
					.UseStartup<T>()
					.UseUrls(listen)
					.Build();
				await host.StartAsync();
				return host != null ? new Fixture(new AspNetCoreServer(host, listen)) : null;
			}

	}
}