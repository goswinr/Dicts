# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.5.0] - 2025-10-11
### Changed
- BREAKING CHANGE: change order of arguments for Dict.set and Dict.get for better function composition.


## [0.4.0] - 2025-03-09
### Added
Several new function in a `Dict` module:
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
- in IDictionary rename Get ans Set to GetValue and SetValue

## [0.2.1] - 2024-10-30
### Changed
- Removed IDictionary and IEnumerable interface becaus not compatible with Fable JS & TS yet
- Unified API
### Fixed
- fixed ToString() members
### Added
- docs

## [0.1.0] - 2024-09-29
### Added
- Implementation ported from [FsEx](https://github.com/goswinr/FsEx)
- Added more tests


[Unreleased]: https://github.com/goswinr/Dicts/compare/0.5.0...HEAD
[0.5.0]: https://github.com/goswinr/Dicts/compare/0.4.0...0.5.0
[0.4.0]: https://github.com/goswinr/Dicts/compare/0.3.0...0.4.0
[0.3.0]: https://github.com/goswinr/Dicts/compare/0.2.1...0.3.0
[0.2.1]: https://github.com/goswinr/Dicts/compare/0.1.0...0.2.1
[0.1.0]: https://github.com/goswinr/Dicts/releases/tag/0.1.0


