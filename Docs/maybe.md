# Maybe monad

This document describes a goal and common usage scenarios of the Maybe monad

## Overview
The purpose of Maybe effect is to describe a possibility of absence of a value inside it. It is modeled by introducing two projections
- Some
- None

[Some] projection has a value, whilst [None] does not.

## Usage
1. One and very popular scenario is dealing with a data

```csharp
interface IRepository<T>
{
  Maybe<T> Find(string name)
}

var employee = employeeRepository.Find("john")
var salary = employee.Map(e => e.Salary);

if (salary.HasValue())
    Console.WriteLine("John's salary = " + salary.GetValue())
else
    Console.WriteLine("There's no John in the database")

```

2. Querying multiple sources

```csharp

interface IRepository<T>
{
  Maybe<T> Find(string name)
}

var s = from john in repository.Find("John")
        from mary in repository.Find("Mary")
        select new
        {
           John = john.Salary,
           Mary = mary.Salary
        };

if (s.HasValue())
  Console.WriteLine("John's salary = " + s.John + ", whereas Marie's salary = " + s.Mary)
else
  Console.WriteLine("There are no John and Mary in the database")

```
