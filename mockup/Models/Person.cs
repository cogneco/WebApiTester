using System;
using Json = Newtonsoft.Json;

namespace Cogneco.WebApiTester.Mockup.Models
{
	[Json.JsonObject(ItemNullValueHandling = Json.NullValueHandling.Ignore)]
	public class Person
	{
		public string Name { get; set; }
		public int Age { get; set; }
		public Person(string name, int age) {
			this.Name = name;
			this.Age = age;
		}
	}
}
