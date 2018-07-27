using System;
using Xunit;
using Tasks = System.Threading.Tasks;

namespace Cogneco.WebApiTester.Test.Values
{
		[Collection("WebApiTests")]
		public class CompareToString
		{
				[Fact]
				public async Tasks.Task GetCollection()
				{
					using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>())
						await fixture.Get("/api/values").HasContentType("application/json", "utf-8").ContentEquals("[\"value0\",\"value1\",\"value2\",\"value3\",\"value4\",\"value5\"]");
				}
				[Fact]
				public async Tasks.Task GetResource()
				{
					using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>())
						await fixture.Get("/api/values/5").HasContentType("text/plain", "utf-8").ContentEquals("value5");
				}
				[Fact]
				public async Tasks.Task PostCollection()
				{
					using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>()) {
						await fixture.Post("/api/values", "value6").HasContentType("text/plain", "utf-8").ContentEquals("value6");
						await fixture.Get("/api/values").HasContentType("application/json", "utf-8").ContentEquals("[\"value0\",\"value1\",\"value2\",\"value3\",\"value4\",\"value5\",\"value6\"]");
					}
				}
				[Fact]
				public async Tasks.Task PutResource()
				{
					using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>()) {
						await fixture.Put("/api/values/3", "item3").HasContentType("text/plain", "utf-8").ContentEquals("item3");
						await fixture.Get("/api/values").HasContentType("application/json", "utf-8").ContentEquals("[\"value0\",\"value1\",\"value2\",\"item3\",\"value4\",\"value5\"]");
					}
				}
				[Fact]
				public async Tasks.Task DeleteResource()
				{
					using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>()) {
						await fixture.Delete("/api/values/3").HasContentType("text/plain", "utf-8").ContentEquals("value3");
						await fixture.Get("/api/values").HasContentType("application/json", "utf-8").ContentEquals("[\"value0\",\"value1\",\"value2\",\"value4\",\"value5\"]");
					}
				}
		}
}
