# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.3.0] - 2025-02-22
### Fixed
- fixed interfaces to be compatible with Fable JS & TS
### Added
- TryPop function
## Changed
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


[Unreleased]: https://github.com/goswinr/Dicts/compare/0.3.0...HEAD
[0.3.0]: https://github.com/goswinr/Dicts/compare/0.2.1...0.3.0
[0.2.1]: https://github.com/goswinr/Dicts/compare/0.1.0...0.2.1
[0.1.0]: https://github.com/goswinr/Dicts/releases/tag/0.1.0

<!--
use to get tag dates:
git log --tags --simplify-by-decoration --pretty="format:%ci %d"
-->

