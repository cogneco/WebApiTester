using System;
using Xunit;
using Http = System.Net.Http;
using Net = System.Net;
using System.Threading.Tasks;
using Kean.Extension;
using Json = Newtonsoft.Json;
using Reflection = System.Reflection;

namespace Cogneco.WebApiTester
{
	public class Response
	{
		public int? DefaultStatus { get; set; }
		Task<Http.HttpResponseMessage> response;
		internal Response(Task<Http.HttpResponseMessage> response) {
			this.response = response;
		}
		public Response HasStatus(int status) {
			return new Response(this.response.Then(response => {
				Assert.Equal(status, (int)response.StatusCode);
				return response;
			})) { DefaultStatus = null };
		}
		public Response HasStatus(int? status) {
			return status.HasValue ? this.HasStatus(status.Value) : this;
		}
		public Response HasStatus(Net.HttpStatusCode status) {
			return new Response(this.response.Then(response => {
				Assert.Equal(status, response.StatusCode);
				return response;
			})) { DefaultStatus = null };
		}
		public Response HasContentType(string contentType, string charSet = null) {
			return this.HasContentType(new Http.Headers.MediaTypeHeaderValue(contentType) { CharSet = charSet });
		}
		public Response HasContentType(Http.Headers.MediaTypeHeaderValue contentType) {
			return new Response(this.response.Then(response => {
				Assert.Equal(contentType, response.Content.Headers.ContentType);
				return response;
			})) { DefaultStatus = this.DefaultStatus };
		}
		public Response ContentEquals(string expected) {
			return this.ContentEquals(Task.FromResult(expected));
		}
		public Response ContentEquals(Task<string> expected) {
			return new Response(this.HasStatus(this.DefaultStatus).response.Then(async response => {
				var actual = await response.Content.ReadAsStringAsync();
				var expectedValue = await expected;
				if (expectedValue != actual)
					System.IO.File.WriteAllText("response.json", actual);
				Assert.Equal(expectedValue, actual);
				return response;
			}).Unwrap());
		}
		public Response ContentEquals<T>(T expected) {
		return new Response(this.HasStatus(this.DefaultStatus).HasContentType("application/json", "utf-8").response.Then(async response => {
				var actual = Json.JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
				Assert.Equal(expected, actual);
				return response;
			}).Unwrap());
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
		public Response ContentEqualsResource(string path) {
			var assembly = Reflection.Assembly.GetCallingAssembly();
			string prefix = assembly.GetName().Name;
			async Task<string> loadResource() {
				string result;
				try
				{
					using (var stream = assembly.GetManifestResourceStream(prefix + path.Replace('/', '.')))
						using (var reader = new System.IO.StreamReader(stream))
							result = await reader.ReadToEndAsync();
				}
				catch (System.Exception e)
				{
					throw new System.IO.FileNotFoundException($"Could not locate embedded resource: \"{path}\" in assembly \"{prefix}\".", path, e);
				}
				return result;
			}
			return this.HasContentType(Response.GetContentType(path)).ContentEquals(loadResource());
		}
		public void Wait() => this.response.Wait();
		public async Task AsTask() => await this.response;
		static Http.Headers.MediaTypeHeaderValue GetContentType(string path)
		{
			Http.Headers.MediaTypeHeaderValue result;
			switch (path.Split(new [] { "." }, StringSplitOptions.RemoveEmptyEntries).Last())
			{
				default: result = null; break;
				case "json": result = new Http.Headers.MediaTypeHeaderValue("application/json") { CharSet = "utf-8" }; break;
				case "txt": result = new Http.Headers.MediaTypeHeaderValue("text/plain") { CharSet = "utf-8" }; break;
			}
			return result;
		}
	}
}
