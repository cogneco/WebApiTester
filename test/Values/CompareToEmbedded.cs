using System;
using Xunit;
using Tasks = System.Threading.Tasks;

namespace Cogneco.WebApiTester.Test.Values
{
		public class CompareToEmbedded
		{
			[Fact]
			public async Tasks.Task GetCollection()
			{
				using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>())
					await fixture.Get("/api/values").ContentEqualsResource("/Values/GetCollection.json").AsTask();
			}
			[Fact]
			public async Tasks.Task GetResource()
			{
				using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>())
					await fixture.Get("/api/values/5").ContentEqualsResource("/Values/GetResource5.txt").AsTask();
			}
			[Fact]
			public async Tasks.Task PostCollection()
			{
				using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>()) {
					await fixture.Post("/api/values", "values6").HasContentType("text/plain", "utf-8").ContentEqualsResource("/Values/PostCollection6.txt").AsTask();
					await fixture.Get("/api/values").ContentEqualsResource("/Values/GetCollection6.json").AsTask();
				}
			}
		}
}
