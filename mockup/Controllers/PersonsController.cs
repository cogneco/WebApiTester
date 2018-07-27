using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Cogneco.WebApiTester.Mockup.Controllers
{
	[Route("api/[controller]")]
	[ApiController()]
	public class PersonsController : ControllerBase
	{
		Services.PersonsService service;
		public PersonsController(Services.PersonsService service)
		{
			this.service = service;
		}
		// GET api/values
		[HttpGet]
		public ActionResult<IEnumerable<Models.Person>> Get() => this.service;
		// GET api/values/5
		[HttpGet("{index}")]
		public ActionResult<Models.Person> Get(int index) => this.service[index];
		// POST api/values
		[HttpPost]
		public ActionResult<Models.Person> Post([FromBody] Models.Person value) => this.Created("/api/persons/" + (this.service.Count - 1), this.service.Add(value));
		// PUT api/values/5
		[HttpPut("{index}")]
		public ActionResult<Models.Person> Put(int index, [FromBody] Models.Person value) => this.service[index] = value;
		// PATCH api/values/5
		[HttpPatch("{index}")]
		public ActionResult<Models.Person> Patch(int index, [FromBody] Models.Person value) => this.service.Merge(index, value);
		// DELETE api/values/5
		[HttpDelete("{index}")]
		public ActionResult<Models.Person> Delete(int index) => this.service.Remove(index);
	}
}
