
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

- A `DefaultDict<'T>` type. It works like [Python's' defaultdict](https://docs.python.org/3/library/collections.html#collections.defaultdict).<br>
By providing a default function in the constructor it will always return a value for any key.

- Extension methods for working with the `IDictionary<'T>` interface.

It also works in JS and TS with [Fable](https://fable.io/).

This library was designed for use with F# scripting.
Functions and methods never return null.
Only functions starting with `try...` will return an F# Option.
Otherwise when a function fails on invalid input it will throw a descriptive exception.

I was always annoyed that a KeyNotFoundExceptions does not include the actual bad key nor a pretty printed dictionary.
This library fixes that in `iDictionary.Get`, `iDictionary.Set` and other item access functions.

## Examples

All examples assume:
```fsharp
#r "nuget: Dicts"
open Dicts
```

### Dict — Better error messages

`Dict<'K,'V>` is a thin wrapper around `Dictionary<'K,'V>` that gives descriptive exceptions when a key is missing, including the key, item count, and a pretty-printed dictionary.

```fsharp
// Create from key-value pairs
let d = Dict.create [ "a", 1; "b", 2; "c", 3 ]

d.["a"]          // 1
d.Get "b"        // 2
d.Set "d" 4      // adds or updates key "d"
d.Count          // 4

// Nicer error messages than System.Collections.Generic.Dictionary:
d.["z"]          // throws KeyNotFoundException:
                 // "Dict.get failed to find key "z" in Dict<...> of 4 items"

// Check for keys
d.ContainsKey "a"       // true
d.DoesNotContainKey "z" // true

// Use IsEmpty / IsNotEmpty
d.IsEmpty       // false
d.IsNotEmpty    // true
```

### Dict — Pop and TryPop (Python-like)

```fsharp
let d = Dict.create [ "x", 10; "y", 20 ]

d.Pop "x"       // 10  (key "x" is removed from d)
d.Count          // 1

d.TryPop "y"     // Some 20  (key "y" is removed)
d.TryPop "y"     // None     (already removed, no exception)
```

### Dict — Conditional set and defaults

```fsharp
let d = Dict.create [ "a", 1 ]

// Only sets if key is absent, returns true if it was set
d.SetIfKeyAbsent "a" 99  // false  (key "a" exists, value stays 1)
d.SetIfKeyAbsent "b" 42  // true   (key "b" is now 42)

// Get existing value, or create and store a default
d.GetOrSetDefaultValue 0 "c"   // 0  (key "c" is now set to 0)
d.GetOrSetDefaultValue 0 "a"   // 1  (key "a" already exists)

// Default from a function that receives the key
d.GetOrSetDefault (fun k -> k.Length) "hello"  // 5
```

### DefaultDict — Auto-creating missing keys

`DefaultDict<'K,'V>` calls a default function whenever a missing key is accessed.
This is inspired by Python's `defaultdict`.

```fsharp
// Count word occurrences using a mutable ref cell
let counter = DefaultDict<string, int ref>(fun _ -> ref 0)
for word in ["hi"; "world"; "hi"; "hi"] do
    incr counter.[word]

counter.["hi"].Value     // 3
counter.["world"].Value  // 1

// Group items by key
let groups = DefaultDict<string, ResizeArray<int>>(fun _ -> ResizeArray())
groups.["evens"].Add 2
groups.["odds"].Add  1
groups.["evens"].Add 4

groups.["evens"] |> Seq.toList  // [2; 4]
groups.["odds"]  |> Seq.toList  // [1]
```

**Important:** Accessing a missing key with `Get` or the indexer `.[key]` **creates** it.
Use `TryGetValue` or `ContainsKey` to check without creating:
```fsharp
let dd = DefaultDict<string, int>(fun _ -> 0)
dd.ContainsKey "x"         // false  (does not create "x")
let ok, _ = dd.TryGetValue "x"  // ok = false  (does not create "x")
dd.["x"]                   // 0  (now "x" IS created with the default)
dd.ContainsKey "x"         // true
```

### Dict module — Functional-style operations

The `Dict` module provides functions that work on any `IDictionary<'K,'V>`, including plain `Dictionary` and `Dict`.

```fsharp
let d = Dict.create [ "a", 1; "b", 2; "c", 3 ]

// Functional get / set / tryGet
Dict.get "a" d            // 1
Dict.set "d" 4 d          // sets key "d" to 4
Dict.tryGet "z" d         // None
Dict.tryGet "a" d         // Some 1

// Pop and tryPop
Dict.pop "d" d             // 4  (removes key "d")
Dict.tryPop "d" d          // None  (already removed)

// Conditional set
Dict.setIfKeyAbsent "a" 99 d  // false  (key "a" exists)
Dict.setIfKeyAbsent "e" 5 d   // true   (key "e" is now 5)

// Get or create a default
Dict.getOrSetDefaultValue 0 "f" d   // 0  (key "f" is now 0)

// Iteration
Dict.keys   d |> Seq.toList   // ["a"; "b"; "c"; "e"; "f"]
Dict.values d |> Seq.toList   // [1; 2; 3; 5; 0]
Dict.items  d |> Seq.toList   // [("a",1); ("b",2); ("c",3); ("e",5); ("f",0)]

Dict.iter (fun k v -> printfn "%s = %d" k v) d
Dict.map  (fun k v -> $"{k}:{v}") d |> Seq.toList
```

### Dict.memoize — Cache function results

```fsharp
let expensiveComputation = Dict.memoize (fun n ->
    printfn "computing %d..." n
    n * n
)

expensiveComputation 5   // prints "computing 5...", returns 25
expensiveComputation 5   // returns 25 immediately, no print
```

### IDictionary extensions

Extension methods available on any `IDictionary<'K,'V>` (including `Dictionary<'K,'V>`):
```fsharp
open Dicts.ExtensionsIDictionary

let d = System.Collections.Generic.Dictionary<string,int>()
d.["x"] <- 10; d.["y"] <- 20; d.["z"] <- 30

d.GetValue "x"           // 10  (with descriptive error on missing key)
d.SetValue "w" 40        // adds key "w"
d.Pop "w"                // 40  (removes key "w")
d.TryPop "w"             // None
d.Items      |> Seq.toList  // seq of (key, value) tuples
d.KeysSeq    |> Seq.toList  // seq of keys
d.ValuesSeq  |> Seq.toList  // seq of values
d.DoesNotContainKey "w"  // true
```

### Pretty printing

All types provide readable string representations:
```fsharp
let d = Dict.create [ "name", "Alice"; "city", "Zurich" ]

d.ToString()    // "Dict<String,String> with 2 items"
d.AsString      // "Dict<String,String> with 2 items:\n  name : Alice\n  city : Zurich\n"
d.ToString(1)   // prints only the first entry
```

## Full API Documentation

[goswinr.github.io/Dicts](https://goswinr.github.io/Dicts/reference/dicts.html)


## Tests
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

## License
[MIT](https://raw.githubusercontent.com/goswinr/Dicts/main/LICENSE.txt)

## Changelog
see [CHANGELOG.md](https://github.com/goswinr/Dicts/blob/main/CHANGELOG.md)


