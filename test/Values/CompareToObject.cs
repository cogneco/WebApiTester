using System;
using Xunit;
using Tasks = System.Threading.Tasks;

namespace Cogneco.WebApiTester.Test.Values
{
		[Collection("WebApiTests")]
		public class CompareToObject
		{
				[Fact]
				public async Tasks.Task GetCollection()
				{
					using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>())
						await fixture.Get("/api/values").ContentContains(new [] { "value0", "value1", "value2", "value3", "value4", "value5" });
				}
		}
}
