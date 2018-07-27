# WebApiTester
A library for testing web API:s created in ASP.NET Core MVC using XUnit.

[![Build Status](https://travis-ci.org/cogneco/WebApiTester.svg?branch=master)](https://travis-ci.org/cogneco/WebApiTester)
![NuGet](https://img.shields.io/nuget/v/Cogneco.WebApiTester.svg)
![NuGet](https://img.shields.io/nuget/dt/Cogneco.WebApiTester.svg)

## Usage
### Example
``` c#
using System;
using Xunit;
using Tasks = System.Threading.Tasks;

namespace Cogneco.WebApiTester.Test.Values
{
		[Collection("WebApiTests")]
		public class CompareToObject
		{
				[Fact]
				public async Tasks.Task GetCollection()
				{
					using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>())
						await fixture.Get("/api/values").ContentContains(new [] { "value0", "value1", "value2", "value3", "value4", "value5" });
				}
		}
}
```

### Install
Add dependency to your test project to the latest `Cogneco.WebApi.Tester` package using NuGet:
``` shell
dotnet add test/ package Cogneco.WebApi.Tester
```

### Configuration

#### `Startup.cs`
If necessary, create a new class that inherits your `Startup.cs` and sets up the server for testing. This way you can ensure that you mock up external services and use a in memory database replacement.

#### Don't Run in Parallel
Likely due to issues with running multiple web servers in parallel you need to ensure that your web API tests need to run single threaded. This is the easiest achieved by placing the `Collection` attribute with the same argument on all your test classes containing web API tests like this:
``` c#
[Collection("WebApiTests")]
public class CompareToObject
```

### Write Tests
#### Create a Fixture
To run a test you need a `Fixture`. When created the fixture will start a new web server instance and against which a test is run. After the test is done running the fixture needs to be stoped so that web server and other resources can be freed. The way to do this is to put the creation of the fixture inside a using statement.
``` c#
using (var fixture = await WebApiTester.AspNetCoreServer.Start<Mockup.Startup>()) {

}
```
Observe that when creating a fixture you give the type of your custom test startup class as an argument.

#### HTTP Methods
The web API tester currently supports the following HTTP methods:
* `GET`
* `PUT`
* `POST`
* `PATCH`
* `DELETE`

##### `GET` test
``` c#
await fixture.Get("/api/persons/1");
```
##### `PUT` test
``` c#
await fixture.Put("/api/persons/1", new { name = "Joe Smith", age = 45 });
```
##### `POST` test
``` c#
await fixture.Post("/api/persons", new { name = "Joe Smith", age = 45 });
```
##### `PATCH` test
``` c#
await fixture.Put("/api/persons/1", new { age = 48 });
```
##### `DELETE` test
``` c#
await fixture.Delete("/api/persons/1");
```

#### Assert Content
There are three ways to assert the returned content.

##### Assert Content Using Strings
``` c#
await fixture.Get("/api/values").HasContentType("application/json", "utf-8").ContentEquals("[\"value0\",\"value1\",\"value2\",\"value3\",\"value4\",\"value5\"]");
```
##### Assert Content Using Embedded Resources
``` c#
await fixture.Get("/api/values").ContentEqualsResource("/Values/GetCollection.json");
```
With the following configuration in the `csproj` file.
``` xml
<ItemGroup>
	<EmbeddedResource Include="Values\GetCollection.json" />
</ItemGroup>
```
##### Assert Content Using JSON Deserialization
It is also possible to compare the content of a response by deserializing it from JSON into an object to compare to. Deserialization is done using the `NewtonSoft.Json` library. It is possible to use the object literal notion for this.
``` c#
await fixture.Get("/api/persons/1").ContentContains( new { name = "Ann Bok", age = 22 });
```
Please observe that properties of objects not available in the type you deserialize to will be ignored.

#### Assert Status Code
There are three ways to assert the status code of the repsonse:
1. Explicitly using a number
1. Explicitly using the `System.Net.HttpStatusCode` enum
1. Implicitly when asserting the content

##### Explicitly using a number
``` c#
await fixture.Get("/api/persons/1").HasStatus(200);
```
##### Explicitly using the `System.Net.HttpStatusCode` enum
``` c#
await fixture.Get("/api/persons/1").HasStatus(Net.HttpStatusCode.OK);
```
##### Implicitly when asserting the content
When asserting the content the framework will, if the status has not been asserted previously in the chain, automatically insert an assert for the status dependent on the method used to create the response according to the table below.

| Method | Default Status Code |
|:-------|:--------------------|
| GET    | 200 OK              |
| PUT    | 200 OK              |
| POST   | 201 Created         |
| PATCH  | 200 OK              |
| DELETE | 200 OK              |

#### Assert Content Type
There are four ways to assert the content type of the repsonse:
1. Explicitly using a `string`
1. Explicitly using the `System.Net.Http.Headers.MediaTypeHeaderValue` class
1. Implicitly when asserting the content using an embedded resource
1. Implicitly when asserting the content using JSON deserialization

##### Explicitly using a `string`
``` c#
await fixture.Get("/api/persons/1").HasContentType("application/json", "utf-8");
```
``` c#
await fixture.Get("/api/persons/1").HasContentType("application/json");
```
##### Explicitly using the `System.Net.Http.Headers.MediaTypeHeaderValue` class
``` c#
var contentType = new Http.Headers.MediaTypeHeaderValue("application/json") { CharSet = "utf-8" };
await fixture.Get("/api/persons/1").HasContentType(contentType);
```
##### Implicitly when asserting the content using an embedded resource
When asserting the content by comparing against an embedded resource the framework will, if no assert of the content type was performed previously in the chain automatically insert an assert of the content type before asserting the content based on the embedded resources file extension according to the table below. No assertion will be inserted if the framework does not know any content type for the extension.

| Extension | `content-type`            |
|-----------|---------------------------|
| json      | `application/json, utf-8` |
| txt       | `text/plain, utf-8`       |

##### Implicitly when asserting the content using JSON deserialization
When asserting the content type using JSON deserialization and the content type has not been previously asserted in the chain an assert first be inserted for the content type `application/json, utf-8`.

### Run Tests
Tests are runned using the XUnit test runner. This can be done using the `dotnet` command but also the `ms-vscode.csharp` extension for Visual Studio Code.
``` shell
dotnet test test/
```
### Debug Tests
Tests can be debugged using the `ms-vscode.csharp` extension for Visual Studio Code. Note that it is possible to debug the server side as both sides recide in the same process by placing breakpoints in for example the controller.

## Contribute
If you find something you are missing or wrong don't hesitate to contact the maintainer of this library by [creating an issue](https://github.com/cogneco/WebApiTester/issues/new). Contributions thru pull requests are welcomed.
