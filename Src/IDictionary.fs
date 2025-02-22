namespace Dicts

open System
open System.Collections.Generic


/// Static Extension methods on Exceptions to cal Exception.Raise "%A" x with F# printf string formatting
module internal ExtensionsExceptions =
    type ArgumentNullException with
        /// Raise ArgumentNullException with F# printf string formatting
        static member Raise msg = Printf.kprintf (fun s -> raise (ArgumentNullException(s))) msg

    type KeyNotFoundException with
        /// Raise KeyNotFoundException with F# printf string formatting
        static member Raise msg = Printf.kprintf (fun s -> raise (KeyNotFoundException(s))) msg

open ExtensionsExceptions

/// Provides Extensions for IDictionary<'K,'V> interface.
/// Such as Items as key value tuples , Pop(key)  or GetValue with nicer error message)
module ExtensionsIDictionary =


    /// The string representation of the Dict including the count of entries.
    let inline internal toString(dic: IDictionary<'K,'V>) =
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

        /// Set/add value at key, with nicer error messages.
        /// Same as <c>Dicts.addValue key value</c>
        member d.SetValue k v =
            // this cant be called just .Set because
            // there would be a clash in member overloading a curried function with Dicts type that is also a IDictionary ??
            try d.[k] <- v
            with _  -> KeyNotFoundException.Raise "Dicts: IDictionary.SetValue key value ;failed for key '%A' in %A of %d items (for value: '%A')" k d d.Count v

        /// Get value at key, with nicer error messages.
        member d.GetValue k  =
            // try d.[k]
            // with _ -> KeyNotFoundException.Raise "Dicts: IDictionary.GetValue(key) failed to find key %A in %A of %d items" k d d.Count
            // don't do it like this, because get on a defaultDict should set a missing value, TryGetValue does not for defaultDict
            let ok, v = d.TryGetValue(k)
            if ok then  v
            else KeyNotFoundException.Raise "Dicts: IDictionary.Get(key) failed to find key %A in %A of %d items" k d d.Count


        /// Get a value and remove it from Dictionary, like *.pop() in Python.
        member d.Pop k  =
            let ok, v = d.TryGetValue(k)
            if ok then
                d.Remove k |>ignore
                v
            else
                KeyNotFoundException.Raise "Dicts: IDictionary.Pop(key): Failed to pop key %A in %A of %d items" k d d.Count

        /// Try to get a value and remove it from Dictionary, like *.pop() in Python.
        /// Returns None if key is not found.
        member d.TryPop k  =
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

        /// Determines whether the Dictionary does not contains the specified key.
        /// not(dic.ContainsKey(key))
        member d.DoesNotContainKey(key) = not(d.ContainsKey(key))


        /// A string representation of the IDictionary including the count of entries and the first 5 entries.
        #if FABLE_COMPILER
        member inline this.AsString =  // inline needed for Fable reflection
        #else
        member this.AsString =  // on .NET inline fails because it's using internal DefaultDictUtil
        #endif
            let b = Text.StringBuilder()
            let c = this.Count
            b.Append(toString this) |> ignore
            if c > 0  then b.AppendLine ":"  |> ignore
            for KeyValue(k, v) in this |> Seq.truncate 5 do // Add sorting ? print 3 lines??
                b.AppendLine $"  {k} : {v}" |> ignore
            if c > 5 then b.AppendLine "  ..." |> ignore
            b.ToString()


        /// A string representation of the IDictionary including the count of entries
        /// and the specified amount of entries.
        #if FABLE_COMPILER
        member inline this.ToString(entriesToPrint) =  // inline needed for Fable reflection
        #else
        member this.ToString(entriesToPrint) = // on .NET inline fails because it's using internal DefaultDictUtil
        #endif
            let b = Text.StringBuilder()
            let c = this.Count
            b.Append(toString this) |> ignore
            if c > 0  && entriesToPrint > 0 then b.AppendLine ":"  |> ignore
            for KeyValue(k, v) in this |> Seq.truncate (max 0 entriesToPrint) do // Add sorting ? print 3 lines??
                b.AppendLine $"  {k} : {v}" |> ignore
            if c > entriesToPrint then b.AppendLine "  ..." |> ignore
            b.ToString()
