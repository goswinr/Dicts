# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.


## Project Overview

Dicts is an F# library providing enhanced dictionary types (`Dict<'K,'V>` and `DefaultDict<'K,'V>`) with better error handling. It targets both .NET (netstandard2.0) and JavaScript via Fable transpilation.

## Build Commands

```bash
# Build
dotnet build -c Release

# Restore dependencies
dotnet restore

# Generate documentation
dotnet tool restore
dotnet fsdocs build --clean --strict --properties Configuration=Release --input Docs --output DocsGenerated
```

## Testing

Tests must pass on **both** .NET and JavaScript - they are not interchangeable:

```bash
# .NET tests (Expecto)
cd Tests && dotnet run

# JavaScript tests (Fable.Mocha) - requires npm install first
cd Tests && npm test
```

The JavaScript tests transpile F# to TypeScript via Fable, then run with Mocha.

## Architecture

**Core types in Src/:**
- `IDictionary.fs` - Extension methods for `IDictionary<'K,'V>` interface
- `Dict.fs` - `Dict<'K,'V>` sealed class wrapping `Dictionary<'K,'V>` with improved error messages
- `DefaultDict.fs` - `DefaultDict<'K,'V>` for automatic default value creation (like Python's defaultdict)
- `DictModule.fs` - Static utility functions (`memoize`, `get`, `set`, `tryGet`, `tryPop`, `create`, etc.)

**Design principles:**
- Thin wrapper pattern: Dict intercepts operations to provide better error messages
- Exception-based error handling with descriptive messages (not null returns)
- Fable-compatible: all types work when transpiled to JavaScript
- Designed for F# scripting scenarios

## Release Process

Releases are tag-driven with automatic NuGet publishing. Version is synchronized from CHANGELOG.md via Ionide.KeepAChangelog.Tasks.
