# Stringification&nbsp;&nbsp;
[![Build, Test, Publish](https://github.com/dshe/Stringification/actions/workflows/ci.yml/badge.svg)](https://github.com/dshe/Stringification/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/vpre/Stringification.svg)](https://www.nuget.org/packages/Stringification/)
[![NuGet](https://img.shields.io/nuget/dt/Stringification?color=orange)](https://www.nuget.org/packages/Stringification/)
[![License](https://img.shields.io/badge/license-Apache%202.0-7755BB.svg)](https://opensource.org/licenses/Apache-2.0)

***A simple utility which creates a json-like string representation of the non-default public properties of an object***
- **.NET 10.0** library
- simple and intuitive API
- tested
- dependencies: none

```csharp
Company company = new()
{
    Name = "Aco",
    Id = 9,
    Active = true,
    Location = new Location("3 Ruey", Country.Macedonia, DateTime.Now),
    People = new List<Person>() 
            { new Person("Natalia", 18), new Person("Natasha", 42) }
};
```
```csharp
company.Stringify() =>
    "Company: {Name:'Aco', Id:9, Active:True, Location:{Address:'3 Ruey', Country:Macedonia, Updated:4/7/2019 10:10:20 PM}, People:[{Name:'Natalia', Age:18}, {Name:'Natasha', Age:42}]}"
```
