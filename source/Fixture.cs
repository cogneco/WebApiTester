using System;
using Http = System.Net.Http;
using Tasks = System.Threading.Tasks;
using Json = Newtonsoft.Json;

namespace Cogneco.WebApiTester
{
	public class Fixture : IDisposable
	{
		Server server;
		Http.HttpClient client;
		public Fixture(Server server)
		{
			this.server = server;
			this.client = new Http.HttpClient();
		}
		public async Tasks.Task Stop()
		{
			if (this.server != null)
				await this.server.Stop();
			this.server = null;
			if (this.client != null)
				this.client.Dispose();
			this.client = null;
		}
		public Response Get(string path) {
			return new Response(this.client.GetAsync(this.server.Listen + path)) { DefaultStatus = 200 };
		}
		Http.HttpContent SerializeContent(object content) => new Http.StringContent(Json.JsonConvert.SerializeObject(content), new System.Text.UTF8Encoding(), "application/json");
		public Response Put(string path, object content) {
			return new Response(this.client.PutAsync(this.server.Listen + path, this.SerializeContent(content))) { DefaultStatus = 200 };
		}
		public Response Post(string path, object content) {
			return new Response(this.client.PostAsync(this.server.Listen + path, this.SerializeContent(content))) { DefaultStatus = 201 };
		}
		public Response Patch(string path, object content) {
			return new Response(this.client.SendAsync(new Http.HttpRequestMessage(new Http.HttpMethod("PATCH"), this.server.Listen + path) { Content = this.SerializeContent(content) })) { DefaultStatus = 200 };
		}
		public Response Delete(string path) {
			return new Response(this.client.DeleteAsync(this.server.Listen + path)) { DefaultStatus = 200 };
		}
		void IDisposable.Dispose() => this.Stop().Wait();
	}
}
