using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Cogneco.WebApiTester.Mockup.Controllers
{
	[Route("api/[controller]")]
	[ApiController()]
	public class ValuesController : ControllerBase
	{
		Services.ValuesService service;
		public ValuesController(Services.ValuesService service)
		{
			this.service = service;
		}
		// GET api/values
		[HttpGet]
		public ActionResult<IEnumerable<string>> Get() => this.service;
		// GET api/values/5
		[HttpGet("{index}")]
		public ActionResult<string> Get(int index) => this.service[index];
		// POST api/values
		[HttpPost]
		public ActionResult<string> Post([FromBody] string value) => this.Created("/api/values/5", this.service.Add(value));
		// PUT api/values/5
		[HttpPut("{index}")]
		public ActionResult<string> Put(int index, [FromBody] string value) => this.service[index] = value;
		// DELETE api/values/5
		[HttpDelete("{index}")]
		public ActionResult<string> Delete(int index) => this.service.Remove(index);
	}
}
