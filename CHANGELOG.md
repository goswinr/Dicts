# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Changed
- BREAKING CHANGE: `Dict.Add`, `DefaultDict.Add`, their `IDictionary` and `ICollection` implementations and the module function `Dict.add` now work like `Dictionary.Add` and throw an ArgumentException if the key already exists. Use `Set`, the indexer or `Dict.set` to add or replace a value.
- BREAKING CHANGE: `Dict.create` and `DefaultDict.create` throw an ArgumentException on duplicate keys, like the `Dictionary` constructor.
- BREAKING CHANGE: `DefaultDict.get` and `DefaultDict.set` take the key first, like `Dict.get` and `Dict.set`.
- `IDictionary.SetValue` no longer turns every exception into a KeyNotFoundException, it only adds a nicer message for null keys.
- Tests now run on [Scriptorium](https://fable-hub.github.io/Scriptorium/guides/getting-started/) (`Scriptorium.Quill` and `Scriptorium.Nib`) on .NET and JavaScript, replacing Expecto on .NET and Fable.Mocha plus the `mocha` npm package on JavaScript.
- The JavaScript tests are run by `dotnet fable --runScript` instead of `mocha`, so `Tests/package.json` has no runtime test dependency left.
### Removed
- BREAKING CHANGE: the static members `create`, `get` and `set` on the `Dict<'K,'V>` type. They were hidden by the `Dict` module functions of the same name.
### Fixed
- `DefaultDict.createDirectly` ignored the given Dictionary and returned an empty DefaultDict.
- `ICollection<KeyValuePair>.Contains` and `.Remove` on `Dict` and `DefaultDict` now compare the value too, like `Dictionary`.
- `ToString(n)` with n <= 0 no longer appends "  ..." to the header line.
- Wrong function names in some error messages.

## [0.5.1] - 2026-09-07
### Fixed
- Packaging: the Fable content glob is no longer recursive, so the package no longer ships generated obj AssemblyInfo files, only the real source files.

## [0.5.0] - 2025-10-11
### Changed
- BREAKING CHANGE: change order of arguments for Dict.set and Dict.get for better function composition.


## [0.4.0] - 2025-03-09
### Added
- memoize - Caches results of a function in a Dictionary
- get - Gets value at key from IDictionary with better error messages
- set - Sets value at key in an IDictionary
- add - Sets value at key in an IDictionary (alias for set)
- tryGet - Tries to get a value from an IDictionary
- create - Creates a Dict from sequence of key-value pairs
- setIfKeyAbsent - Sets value only if key doesn't exist yet
- addIfKeyAbsent - Sets value only if key doesn't exist yet (alias for setIfKeyAbsent)
- getOrSetDefault - Gets value or sets default using function if key doesn't exist
- getOrSetDefaultValue - Gets value or sets provided default value if key doesn't exist
- tryPop - Tries to get value and remove key-value pair from dictionary
- pop - Gets value and removes key-value pair from dictionary
- items - Returns sequence of key-value tuples
- values - Returns sequence of values
- keys - Returns sequence of keys
- iter - Iterates over keys and values of a Dict
- map - Maps over keys and values of a Dict


## [0.3.0] - 2025-02-22
### Fixed
- fixed interfaces to be compatible with Fable JS & TS
### Added
- TryPop function
### Changed
- in IDictionary rename Get and Set to GetValue and SetValue

## [0.2.1] - 2024-10-30
### Changed
- Removed IDictionary and IEnumerable interface because not compatible with Fable JS & TS yet
- Unified API
### Fixed
- fixed ToString() members
### Added
- docs

## [0.1.0] - 2024-09-29
### Added
- Implementation ported from [FsEx](https://github.com/goswinr/FsEx)
- Added more tests


[Unreleased]: https://github.com/goswinr/Dicts/compare/0.5.1...HEAD
[0.5.1]: https://github.com/goswinr/Dicts/compare/0.5.0...0.5.1
[0.5.0]: https://github.com/goswinr/Dicts/compare/0.4.0...0.5.0
[0.4.0]: https://github.com/goswinr/Dicts/compare/0.3.0...0.4.0
[0.3.0]: https://github.com/goswinr/Dicts/compare/0.2.1...0.3.0
[0.2.1]: https://github.com/goswinr/Dicts/compare/0.1.0...0.2.1
[0.1.0]: https://github.com/goswinr/Dicts/releases/tag/0.1.0


