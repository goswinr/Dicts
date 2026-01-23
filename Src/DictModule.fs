namespace Dicts

open System
open System.Collections.Generic
open ExtensionsExceptions


/// Static Functions on IDictionary Interface
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>] //need this so doesn't hide Dict alias class in C# assemblies
[<RequireQualifiedAccess>]
module Dict =

    type internal Wrapper<'T> = Wrap of 'T

    /// Caches the results of a function in a Dictionary.
    /// The argument 'T is packed in a wrapper so it can be unit or null(=None) too.
    /// (A Dictionary would fail on a null as key )
    let memoize (f: 'T -> 'U)  : 'T -> 'U =
        // https://stackoverflow.com/questions/20548864/memoize-a-function-of-type-a
        let cache = Dictionary<Wrapper<'T>,'U>() // using a Dictionary  fails on a null or unit key
        fun x ->
            let w = Wrap x
            match cache.TryGetValue(w) with
            | true, res -> res
            | false, _ ->
                let res = f x
                cache.Add(w, res)
                res


    /// Get value at key from IDictionary, with nicer Error messages
    let get (key:'Key) (dic:IDictionary<'Key,'Value>) : 'Value =
        let ok, v = dic.TryGetValue(key)
        if ok then  v
        else KeyNotFoundException.Raise "Dict.get failed to find key %A in %A of %d items" key dic dic.Count

    /// Set value at key in a IDictionary
    /// just d.[k]<-v
    let set (key:'Key) (value:'Value) (dic:IDictionary<'Key,'Value>) : unit =
        dic.[key] <- value


    /// Set value at key in a IDictionary
    /// just d.[k]<-v
    let add (key:'Key) (value:'Value) (dic:IDictionary<'Key,'Value>) : unit =
        dic.[key] <- value

    /// Tries to get a value from a IDictionary
    let tryGet (k:'Key) (dic:IDictionary<'Key,'Value>) : 'Value option=
        let ok, v = dic.TryGetValue(k)
        if ok then Some v
        else None

    /// Create a Dict from seq of key and value pairs
    let create (xs:seq<'Key * 'Value>) : Dict<'Key,'Value>=
        let dic = Dict()
        for k,v in xs do
            dic.[k] <- v
        dic

    /// Set value only if key does not exist yet.
    /// Returns false if key already exist, does not set value in this case
    /// Same as <c>Dict.addIfKeyAbsent key value dic</c>
    let setIfKeyAbsent  (key:'Key) (value:'Value)  (dic:IDictionary<'Key,'Value>) : bool =
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise $"Dict.setIfKeyAbsent key is null, value: {value}"
        | _ ->
            if dic.ContainsKey key then
                false
            else
                dic.[key] <- value
                true

    /// Set value only if key does not exist yet.
    /// Returns false if key already exist, does not set value in this case
    /// Same as <c>Dict.setIfKeyAbsent key value dic</c>
    let addIfKeyAbsent  (key:'Key) (value:'Value)  (dic:IDictionary<'Key,'Value>) : bool =
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise $"Dict.addIfKeyAbsent key is null, value: {value}"
        | _ ->
            if dic.ContainsKey key then
                false
            else
                dic.[key] <- value
                true

    /// If the key ist not present calls the default function, set it as value at the key and return the value.
    /// This function is an alternative to the DefaultDict type. Use it if you need to provide a custom implementation of the default function depending on the key.
    let getOrSetDefault (getDefault:'Key -> 'Value) (key:'Key)  (dic:IDictionary<'Key,'Value>) : 'Value =
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "Dict.getOrSetDefault key is null, value"
        | _ ->
            match dic.TryGetValue(key) with
            |true, v-> v
            |false, _ ->
                let v = getDefault(key)
                dic.[key] <- v
                v

    /// If the key ist not present set it as value at the key and return the value.
    let getOrSetDefaultValue (defaultValue: 'Value) (key:'Key)  (dic:IDictionary<'Key,'Value>) : 'Value =
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise $"Dict.getOrSetDefaultValue key is null, value"
        | _ ->
            match dic.TryGetValue(key) with
            |true, v-> v
            |false, _ ->
                let v = defaultValue
                dic.[key] <- v
                v

    /// Tries to  get a value and remove key and value it from dictionary, like *.pop() in Python.
    /// Will return None if key does not exist
    let tryPop(key:'Key)  (dic:IDictionary<'Key,'Value>) : 'Value option =
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "Dict.pop(key) key is null"
        | _ ->
            let ok, v = dic.TryGetValue(key)
            if ok then
                dic.Remove key |>ignore
                Some v
            else
                None


    /// Get a value and remove key and value it from dictionary, like *.pop() in Python.
    /// Will fail if key does not exist
    let pop(key:'Key)  (dic:IDictionary<'Key,'Value>) : 'Value =
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "Dict.pop(key) key is null"
        | _ ->
            let ok, v = dic.TryGetValue(key)
            if ok then
                dic.Remove key |>ignore
                v
            else
                KeyNotFoundException.Raise "Dict.pop(key): Failed to pop key %A in %A of %d items" key dic dic.Count


    /// Returns a (lazy) sequence of key and value tuples
    let items(dic:IDictionary<'Key,'Value>) : seq<'Key * 'Value> =
        seq { for kvp in dic -> kvp.Key, kvp.Value}

    /// Returns a (lazy) sequence of values
    let values (dic:IDictionary<'Key,'Value>) : seq<'Value>=
        seq { for kvp in dic -> kvp.Value}

    /// Returns a (lazy) sequence of Keys
    let keys (dic:IDictionary<'Key,'Value>) : seq<'Key> =
        seq { for kvp in dic -> kvp.Key}

    /// Iterate over keys and values of a Dict
    let iter (f: 'Key -> 'Value -> unit) (dic:IDictionary<'Key,'Value>) : unit =
        for kvp in dic do
            f kvp.Key kvp.Value

    /// Map over keys and values of a Dict
    let map(f: 'Key -> 'Value -> 'T) (dic:IDictionary<'Key,'Value>) : seq<'T> =
        seq { for kvp in dic do
                f kvp.Key kvp.Value}