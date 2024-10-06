namespace Dic

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

/// Provides Extensions for IDictionary
/// Such as Items as key value tuples , Pop(key)  or GetValue with nicer error message)
module ExtensionsIDictionary =

    type IDictionary<'K,'V> with
        // overrides of existing methods are unfortunately silently ignored and not possible.
        // see https://github.com/dotnet/fsharp/issues/3692#issuecomment-334297164

        /// Set/add value at key, with nicer error messages.
        /// Same as <c>Dic.addValue key value</c>
        member d.setValue k v = // this cant be called just 'set' because there would be a clash in member overloading with Dic type that is also a IDictionary
            try  d.[k] <- v
            with
                | :? KeyNotFoundException  -> KeyNotFoundException.Raise "Dic: IDictionary.SetValue failed to find key '%A' in %A of %d items (for value: '%A')" k d d.Count v
                | e                        -> raise e

        /// Set/add value at key, with nicer error messages.
        /// Same as <c>Dic.setValue key value</c>
        member d.addValue k v = // this cant be called just 'add' because there would be a clash in member overloading with Dic type that is also a IDictionary
            try  d.[k] <-v
            with
                | :? KeyNotFoundException  -> KeyNotFoundException.Raise "Dic: IDictionary.SetValue failed to find key '%A' in %A of %d items (for value: '%A')" k d d.Count v
                | e                        -> raise e

        /// Get value at key, with nicer error messages.
        member d.Get k  =
             let ok, v = d.TryGetValue(k)
             if ok then  v
             else KeyNotFoundException.Raise "Dic: IDictionary.GetValue failed to find key %A in %A of %d items" k d d.Count


        /// Get a value and remove it from Dictionary, like *.pop() in Python.
        member d.Pop k  =
            let ok, v = d.TryGetValue(k)
            if ok then
                d.Remove k |>ignore
                v
            else
                KeyNotFoundException.Raise "Dic: IDictionary.Pop(key): Failed to pop key %A in %A of %d items" k d d.Count

        /// Returns a lazy seq of key and value tuples
        member d.Items : seq<'K*'V> =
            seq { for KeyValue(k, v) in d -> k, v}

        // /// A property like the ToString() method,
        // /// But with richer formatting for collections
        // member obj.ToNiceString =
        //     NiceString.toNiceString obj


