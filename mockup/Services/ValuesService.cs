using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Cogneco.WebApiTester.Mockup.Services
{
	public class ValuesService : IEnumerable<string>
	{
		List<string> backend = new List<string> { "value0", "value1", "value2", "value3", "value4", "value5" };
		public string this[int index] {
			get => this.backend[index];
			set => this.backend[index] = value;
		}
		public ValuesService()
		{ }
		public string Remove(int index) {
			var result = this.backend[index];
			this.backend.RemoveAt(index);
			return result;
		}
		public string Add(string value) {
			this.backend.Add(value);
			return value;
		}
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			return this.backend.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return (this as IEnumerable<string>).GetEnumerator();
		}
	}
}
