## Stringification&nbsp;&nbsp;
[![Build status](https://ci.appveyor.com/api/projects/status/xig5mmbk9lqus99h?svg=true)](https://ci.appveyor.com/project/dshe/Stringification) 
[![release](https://img.shields.io/github/release/dshe/Stringification.svg)](https://github.com/dshe/Stringification/releases)
[![NuGet](https://img.shields.io/nuget/vpre/Stringification.svg)](https://www.nuget.org/packages/Stringification/)
[![License](https://img.shields.io/badge/license-Apache%202.0-7755BB.svg)](https://opensource.org/licenses/Apache-2.0)

***A simple utility which creates a json-like string representation of an object***
- supports **.NET Standard 2.0**
- simple and intuitive API
- no dependencies
- tested

```csharp
public void Example()
{
    var company = new Company
    {
        Name = "Gazprom",
        Id = 9,
        Active = true,

        Location = new Location
        {
            Address = "3 Vuy",
            Country = Country.Macedonia,
            LastUpdate = DateTime.Now
        },

        People = new List<Person>
        {
            new Person
            {
                Name = "Natalia",
                Age = 18
            },
            new Person
            {
                Name = "Natasha",
                Age = 81
            }
        }
    };
    
    Write(company.Stringify());
}
    
```
```csharp
Company: {Name:"Gazprom", Id:9, Active:True, Location:{Address:"3 Vuy", Country:Macedonia, LastUpdate:8/13/2018 2:30:10}, People:[{Name:"Natalia", Age:18}, {Name:"Natasha", Age:81}]}
```
