## Stringification&nbsp;&nbsp; [![License](https://img.shields.io/badge/license-Apache%202.0-7755BB.svg)](https://opensource.org/licenses/Apache-2.0)
***A simple utility which creates a json-like string representation of an object***
- contained in a **single** C# 7 source file supporting **.NET Standard 2.0**
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
}
    
Write(company.Stringify());
```
```csharp
Company: {Name:"Gazprom", Id:9, Active:True, Location:{Address:"3 Vuy", Country:Macedonia, LastUpdate:8/13/2018 2:30:10}, People:[{Name:"Natalia", Age:18}, {Name:"Natasha", Age:81}]}
```
