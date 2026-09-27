namespace Dicts

open System
open System.Collections.Generic


/// Static Extension methods on Exceptions to cal Exception.Raise "%A" x with F# printf string formatting
module internal ExtensionsExceptions =
    type ArgumentNullException with
        /// Raise ArgumentNullException with F# printf string formatting
        static member Raise msg : 'T = Printf.kprintf (fun s -> raise (ArgumentNullException(s))) msg

    type KeyNotFoundException with
        /// Raise KeyNotFoundException with F# printf string formatting
        static member Raise msg : 'T = Printf.kprintf (fun s -> raise (KeyNotFoundException(s))) msg

    /// Raise ArgumentException with F# printf string formatting
    /// (a function, not a static extension, so it can't clash with ArgumentNullException.Raise)
    let argumentFail msg : 'T = Printf.kprintf (fun s -> raise (ArgumentException(s))) msg


/// Shared implementation of AsString and ToString(entriesToPrint)
module internal PrettyPrint =

    /// The header, followed by up to entriesToPrint entries, one per line.
    /// Ends with "  ..." if not all entries are shown.
    let withEntries (header:string) (count:int) (entries:seq<KeyValuePair<'K,'V>>) (entriesToPrint:int) : string =
        let b = Text.StringBuilder()
        b.Append header |> ignore
        if count > 0 && entriesToPrint > 0 then
            b.AppendLine ":" |> ignore
            for KeyValue(k, v) in entries |> Seq.truncate entriesToPrint do // Add sorting ? print 3 lines??
                b.AppendLine $"  {k} : {v}" |> ignore
            if count > entriesToPrint then
                b.AppendLine "  ..." |> ignore
        b.ToString()


open ExtensionsExceptions

/// Provides Extensions for IDictionary<'K,'V> interface.
/// Such as Items as key value tuples , Pop(key)  or GetValue with nicer error message)
module ExtensionsIDictionary =


    /// The string representation of the Dict including the count of entries.
    let inline internal toString(dic: IDictionary<'K,'V>) : string =
        let d =
            let fn = dic.GetType().Name
            let start = fn.IndexOf '`'
            if start = -1 then fn
            else fn.Substring(0, start) // trim off the `2 suffix on the type
        let k = typeof<'K>.Name
        let v = typeof<'V>.Name
        if dic.Count = 0 then
            $"empty {d}<{k},{v}>"
        elif dic.Count = 1 then
            $"{d}<{k},{v}> with 1 item"
        else
            $"{d}<{k},{v}> with {dic.Count} items"


    type IDictionary<'K,'V> with
        // overrides of existing methods are unfortunately silently ignored and not possible.
        // see https://github.com/dotnet/fsharp/issues/3692#issuecomment-334297164

        /// <summary>Set value at key, adds the key if it is missing. With a nicer error message for null keys.
        /// Same as <c>Dict.set key value dic</c></summary>
        /// <param name="k">The key to set.</param>
        /// <param name="v">The value to set.</param>
        member d.SetValue k v : unit =
            // this cant be called just .Set because
            // there would be a clash in member overloading a curried function with Dicts type that is also a IDictionary ??
            match box k with // or https://stackoverflow.com/a/864860/969070
            | null -> ArgumentNullException.Raise "Dicts: IDictionary.SetValue: key is null for value %A" v
            | _ -> d.[k] <- v

        /// <summary>Get value at key, with nicer error messages.
        /// Throws a KeyNotFoundException if the key is not found.</summary>
        /// <param name="k">The key to look up.</param>
        member d.GetValue k : 'V =
            let ok, v = d.TryGetValue(k)
            if ok then  v
            else KeyNotFoundException.Raise "Dicts: IDictionary.GetValue(key) failed to find key %A in %A of %d items" k d d.Count


        /// <summary>Get a value and remove it from Dictionary, like *.pop() in Python.
        /// Throws a KeyNotFoundException if the key is not found.</summary>
        /// <param name="k">The key to remove.</param>
        /// <returns>The value that was stored at the key.</returns>
        member d.Pop k : 'V =
            let ok, v = d.TryGetValue(k)
            if ok then
                d.Remove k |>ignore
                v
            else
                KeyNotFoundException.Raise "Dicts: IDictionary.Pop(key): Failed to pop key %A in %A of %d items" k d d.Count

        /// <summary>Try to get a value and remove it from Dictionary, like *.pop() in Python.</summary>
        /// <param name="k">The key to remove.</param>
        /// <returns><c>Some value</c> if the key was found, <c>None</c> if not.</returns>
        member d.TryPop k : 'V option =
            let ok, v = d.TryGetValue(k)
            if ok then
                d.Remove k |>ignore
                Some v
            else
                None

        /// Returns a lazy seq of key and value tuples
        member d.Items : seq<'K*'V> =
            seq { for KeyValue(k, v) in d -> k, v}

        /// Returns a (lazy) sequence of values
        member d.ValuesSeq with get() =
            seq { for kvp in d -> kvp.Value}

        /// Returns a (lazy) sequence of Keys
        member d.KeysSeq with get() =
            seq { for kvp in d -> kvp.Key}

        /// <summary>Determines whether the Dictionary does not contain the specified key.
        /// Same as <c>not(dic.ContainsKey(key))</c></summary>
        /// <param name="key">The key to look for.</param>
        member d.DoesNotContainKey(key) = not(d.ContainsKey(key))


        /// A string representation of the IDictionary including the count of entries and the first 5 entries.
        #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
        member inline this.AsString : string =  // inline needed for Fable reflection
        #else
        member this.AsString : string =  // on .NET inline fails because it's using internal DefaultDictUtil
        #endif
            PrettyPrint.withEntries (toString this) this.Count this 5


        /// <summary>A string representation of the IDictionary including the count of entries
        /// and the specified amount of entries.</summary>
        /// <param name="entriesToPrint">The maximum number of entries to show. Zero or less shows only the header.</param>
        #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
        member inline this.ToString(entriesToPrint) : string =  // inline needed for Fable reflection
        #else
        member this.ToString(entriesToPrint) : string = // on .NET inline fails because it's using internal DefaultDictUtil
        #endif
            PrettyPrint.withEntries (toString this) this.Count this entriesToPrint
