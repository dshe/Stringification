## Stringification&nbsp;&nbsp; [![Build status](https://ci.appveyor.com/api/projects/status/45p92vlwqqgm9vb1?svg=true)](https://ci.appveyor.com/project/dshe/Stringification) [![NuGet](https://img.shields.io/nuget/vpre/Stringification.svg)](https://www.nuget.org/packages/Stringification/) [![License](https://img.shields.io/badge/license-Apache%202.0-7755BB.svg)](https://opensource.org/licenses/Apache-2.0)

***A simple utility which creates a json-like string representation of an object***
- supports **.NET Standard 2.0**
- simple and intuitive API
- dependencies: StringEnums, Reactive Extensions, NodaTime
- tested

```csharp
var company = new Company
{
    Name = "Aco",
    Id = 9,
    Active = true,
    Location = new Location("3 Ruey", Country.Macedonia, DateTime.Now),
    People = new List<Person>() { new Person("Natalia", 18), new Person("Natasha", 42) }
};

company.Stringify() =>
```
```csharp
Company: {Name:"Aco", Id:9, Active:True, Location:{Address:"3 Ruey", Country:Macedonia, Updated:4/7/2019 10:10:20 PM}, People:[{Name:"Natalia", Age:18}, {Name:"Natasha", Age:42}]}
```
