using System;
using Xunit;
using Tasks = System.Threading.Tasks;

namespace Cogneco.WebApiTester.Test.Persons
{
		[Collection("WebApiTests")]
		public class CompareToObject
		{
				[Fact]
				public async Tasks.Task GetCollection()
				{
					using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>())
						await fixture.Get("/api/persons").ContentContains(new [] { new { name = "Kurt Strid", age = 25 }, new { name = "Ann Bok", age = 22 } });
				}
				[Fact]
				public async Tasks.Task GetResource()
				{
					using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>())
						await fixture.Get("/api/persons/1").ContentContains( new { name = "Ann Bok", age = 22 });
				}
				[Fact]
				public async Tasks.Task PostCollection()
				{
					using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>()) {
						await fixture.Post("/api/persons", new { name = "Elin Blom", age = 21 }).ContentContains(new { name = "Elin Blom", age = 21 });
						await fixture.Get("/api/persons").ContentContains(new [] { new { name = "Kurt Strid", age = 25 }, new { name = "Ann Bok", age = 22 }, new { name = "Elin Blom", age = 21 } });
					}
				}
				[Fact]
				public async Tasks.Task PutResource()
				{
					using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>()) {
						await fixture.Put("/api/persons/1", new { name = "Ann Strid", age = 23 }).ContentContains(new { name = "Ann Strid", age = 23 });
						await fixture.Get("/api/persons").ContentContains(new [] { new { name = "Kurt Strid", age = 25 }, new { name = "Ann Strid", age = 23 } });
					}
				}
				[Fact]
				public async Tasks.Task PatchResource()
				{
					using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>()) {
						await fixture.Patch("/api/persons/1", new { name = "Ann Strid" }).ContentContains(new { name = "Ann Strid", age = 22 });
						await fixture.Get("/api/persons").ContentContains(new [] { new { name = "Kurt Strid", age = 25 }, new { name = "Ann Strid", age = 22 } });
					}
				}
				[Fact]
				public async Tasks.Task DeleteResource()
				{
					using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>()) {
						await fixture.Delete("/api/persons/0").ContentContains(new { name = "Kurt Strid", age = 25 });
						await fixture.Get("/api/persons").ContentContains(new [] { new { name = "Ann Bok", age = 22 } });
					}
				}
		}
}
