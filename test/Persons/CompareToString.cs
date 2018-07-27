using System;
using Xunit;
using Tasks = System.Threading.Tasks;

namespace Cogneco.WebApiTester.Test.Persons
{
		[Collection("WebApiTests")]
		public class CompareToString
		{
				[Fact]
				public async Tasks.Task GetCollection()
				{
					using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>())
						await fixture.Get("/api/persons").HasContentType("application/json", "utf-8").ContentEquals("[{\"name\":\"Kurt Strid\",\"age\":25},{\"name\":\"Ann Bok\",\"age\":22}]");
				}
				[Fact]
				public async Tasks.Task GetResource()
				{
					using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>())
						await fixture.Get("/api/persons/1").HasContentType("application/json", "utf-8").ContentEquals("{\"name\":\"Ann Bok\",\"age\":22}");
				}
				[Fact]
				public async Tasks.Task PostCollection()
				{
					using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>()) {
						await fixture.Post("/api/persons", new { name = "Elin Blom", age = 21 }).HasContentType("application/json", "utf-8").ContentEquals("{\"name\":\"Elin Blom\",\"age\":21}");
						await fixture.Get("/api/persons").HasContentType("application/json", "utf-8").ContentEquals("[{\"name\":\"Kurt Strid\",\"age\":25},{\"name\":\"Ann Bok\",\"age\":22}]");
					}
				}
				[Fact]
				public async Tasks.Task PutResource()
				{
					using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>()) {
						await fixture.Put("/api/persons/1", new { name = "Ann Strid", age = "23" }).HasContentType("application/json", "utf-8").ContentEquals("{\"name\":\"Ann Strid\",\"age\":23}");
						await fixture.Get("/api/persons").HasContentType("application/json", "utf-8").ContentEquals("[{\"name\":\"Kurt Strid\",\"age\":25},{\"name\":\"Ann Strid\",\"age\":23}]");
					}
				}
				[Fact]
				public async Tasks.Task DeleteResource()
				{
					using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>()) {
						await fixture.Delete("/api/persons/0").HasContentType("application/json", "utf-8").ContentEquals("{\"name\":\"Kurt Strid\",\"age\":25}");
						await fixture.Get("/api/persons").HasContentType("application/json", "utf-8").ContentEquals("[{\"name\":\"Ann Bok\",\"age\":22}]");
					}
				}
		}
}
