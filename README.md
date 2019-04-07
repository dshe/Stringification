## Stringification&nbsp;&nbsp; [![Build status](https://ci.appveyor.com/api/projects/status/45p92vlwqqgm9vb1?svg=true)](https://ci.appveyor.com/project/dshe/Stringification) [![NuGet](https://img.shields.io/nuget/vpre/Stringification.svg)](https://www.nuget.org/packages/Stringification/) [![License](https://img.shields.io/badge/license-Apache%202.0-7755BB.svg)](https://opensource.org/licenses/Apache-2.0)

***A simple utility which creates a json-like string representation of an object***
- supports **.NET Standard 2.0**
- simple and intuitive API
- no dependencies
- tested

```csharp
var company = new Company
{
    Name = "Gazaprompter",
    Id = 999,
    Active = true,
    Location = new Location("31 Vuetra", Country.Macedonia, DateTime.Now),
    People = new List<Person>() { new Person("Natalia", 18), new Person("Natasha", 42) }
};

Write(company.Stringify());
```
```csharp
Company: {Name:"Gazaprompter", Id:999, Active:True, Location:{Address:"31 Vuetra", Country:Macedonia, Updated:4/7/2019 10:03:54 PM}, People:[{Name:"Natalia", Age:18}, {Name:"Natasha", Age:42}]}
```
