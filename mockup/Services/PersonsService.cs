using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Cogneco.WebApiTester.Mockup.Services
{
	public class PersonsService : IEnumerable<Models.Person>
	{
		List<Models.Person> backend = new List<Models.Person> { new Models.Person("Kurt Strid", 25), new Models.Person("Ann Bok", 22) };
		public int Count => this.backend.Count;
		public Models.Person this[int index] {
			get => this.backend[index];
			set => this.backend[index] = value;
		}
		public PersonsService()
		{ }
		public Models.Person Remove(int index) {
			var result = this.backend[index];
			this.backend.RemoveAt(index);
			return result;
		}
		public Models.Person Add(Models.Person value) {
			this.backend.Add(value);
			return value;
		}
		public Models.Person Merge(int index, Models.Person person) {
			var result = this[index];
			if (person.Name != null)
				result.Name = person.Name;
			if (person.Age != 0)
				result.Age = person.Age;
			return result;
		}
		IEnumerator<Models.Person> IEnumerable<Models.Person>.GetEnumerator()
		{
			return this.backend.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return (this as IEnumerable<Models.Person>).GetEnumerator();
		}
	}
}
