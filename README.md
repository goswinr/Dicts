
![Logo](https://raw.githubusercontent.com/goswinr/Dicts/main/Docs/img/logo128.png)
# Dicts

[![Dicts on nuget.org](https://img.shields.io/nuget/v/Dicts)](https://www.nuget.org/packages/Dicts/)
[![Build Status](https://github.com/goswinr/Dicts/actions/workflows/build.yml/badge.svg)](https://github.com/goswinr/Dicts/actions/workflows/build.yml)
[![Docs Build Status](https://github.com/goswinr/Dicts/actions/workflows/docs.yml/badge.svg)](https://github.com/goswinr/Dicts/actions/workflows/docs.yml)
[![Test Status](https://github.com/goswinr/Dicts/actions/workflows/test.yml/badge.svg)](https://github.com/goswinr/Dicts/actions/workflows/test.yml)
[![license](https://img.shields.io/github/license/goswinr/Dicts)](LICENSE.md)
![code size](https://img.shields.io/github/languages/code-size/goswinr/Dicts.svg)

This F# library provides:
- A dedicated `Dict<'T>` type. It is a thin wrapper around `Dictionary<'T>` with more functionality and nicer Error messages.
- A `DefaultDict<'T>` type. It works like [Python's' defaultdict](https://docs.python.org/3/library/collections.html#collections.defaultdict).\
By providing a default function in the constructor it will always return a value for any key.
- Extension methods for working with the `IDictionary<'T>` interface.


### Example

```fsharp
#r "nuget: Dicts"
open Dicts

let dd = DefaultDict<string,int>(fun _ -> ref 99)
incr dd.["A"]   // since dd.["A"] does not exist it will be created with the default value 99, and then incremented  to 100
incr dd.["A"]   // now it exists and will be incremented to 101
dd.["A"].Value  = 101 // true
```

### Full API Documentation

[goswinr.github.io/Dicts](https://goswinr.github.io/Dicts/reference/dicts.html)


### Tests
All Tests run in both javascript and dotnet.
Successful Fable compilation to typescript is verified too.
Go to the tests folder:

```bash
cd Tests
```

For testing with .NET using Expecto:

```bash
dotnet run
```

for JS testing with Fable.Mocha and TS verification:

```bash
npm test
```

### License
[MIT](https://raw.githubusercontent.com/goswinr/Dicts/main/LICENSE.txt)

### Changelog
see [CHANGELOG.md](https://github.com/goswinr/Dicts/blob/main/CHANGELOG.md)


