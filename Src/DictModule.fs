namespace Dicts

open System
open System.Collections.Generic
open ExtensionsExceptions


/// Static Functions on IDictionary Interface
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>] //need this so doesn't hide Dict alias class in C# assemblies
[<RequireQualifiedAccess>]
module Dict =

    /// Wraps the argument of a memoized function, so that null, unit or None can be a key in the cache Dictionary.
    type internal Wrapper<'T> = Wrap of 'T

    /// <summary>Caches the results of a function in a Dictionary.
    /// The argument 'T is packed in a wrapper so it can be unit or null(=None) too.
    /// (A Dictionary would fail on a null as key )
    /// The cache is not thread-safe, don't call the returned function from several threads at the same time.</summary>
    /// <param name="f">The function to cache the results of. It is called once per distinct argument.</param>
    /// <returns>A function that returns the cached result if it was called with the same argument before.</returns>
    let memoize (f: 'T -> 'U)  : 'T -> 'U =
        // https://stackoverflow.com/questions/20548864/memoize-a-function-of-type-a
        let cache = Dictionary<Wrapper<'T>,'U>() // using a Dictionary  fails on a null or unit key
        fun x ->
            let w = Wrap x
            match cache.TryGetValue(w) with
            | true, res -> res
            | false, _ ->
                let res = f x
                cache.[w] <- res
                res


    /// <summary>Get value at key from IDictionary, with nicer Error messages.
    /// Throws a KeyNotFoundException if the key is not found.</summary>
    /// <param name="key">The key to look up.</param>
    /// <param name="dic">The dictionary to read from.</param>
    let get (key:'Key) (dic:IDictionary<'Key,'Value>) : 'Value =
        let ok, v = dic.TryGetValue(key)
        if ok then  v
        else KeyNotFoundException.Raise "Dict.get failed to find key %A in %A of %d items" key dic dic.Count

    /// <summary>Set value at key in a IDictionary, adds the key if it is missing.
    /// Same as <c>dic.[key] &lt;- value</c></summary>
    /// <param name="key">The key to set.</param>
    /// <param name="value">The value to set.</param>
    /// <param name="dic">The dictionary to change.</param>
    let set (key:'Key) (value:'Value) (dic:IDictionary<'Key,'Value>) : unit =
        dic.[key] <- value


    /// <summary>Add a key and value to a IDictionary.
    /// Like Dictionary.Add, it throws an ArgumentException if the key already exists.
    /// Use Dict.set to add or replace a value.</summary>
    /// <param name="key">The key to add.</param>
    /// <param name="value">The value to add.</param>
    /// <param name="dic">The dictionary to change.</param>
    let add (key:'Key) (value:'Value) (dic:IDictionary<'Key,'Value>) : unit =
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "Dict.add: key is null for value %A" value
        | _ ->
            if dic.ContainsKey key then
                argumentFail "Dict.add: an item with the same key %A has already been added to %A of %d items" key dic dic.Count
            else
                dic.Add(key, value)

    /// <summary>Tries to get a value from a IDictionary.</summary>
    /// <param name="k">The key to look up.</param>
    /// <param name="dic">The dictionary to read from.</param>
    /// <returns><c>Some value</c> if the key was found, <c>None</c> if not.</returns>
    let tryGet (k:'Key) (dic:IDictionary<'Key,'Value>) : 'Value option=
        let ok, v = dic.TryGetValue(k)
        if ok then Some v
        else None

    /// <summary>Create a Dict from seq of key and value pairs.
    /// Like the Dictionary constructor, it throws an ArgumentException on duplicate keys.</summary>
    /// <param name="xs">The key and value pairs to fill the new Dict with.</param>
    let create (xs:seq<'Key * 'Value>) : Dict<'Key,'Value>=
        if isNull xs then ArgumentNullException.Raise "seq in Dict.create is null"
        let dic = Dictionary()
        for k,v in xs do
            DictUtil.add' "Dict.create" k v dic
        Dict.createDirectly dic

    /// The shared implementation of setIfKeyAbsent and addIfKeyAbsent.
    /// The name is the calling function, used in the error message.
    let private setIfAbsent (name:string) (key:'Key) (value:'Value) (dic:IDictionary<'Key,'Value>) : bool =
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "%s: key is null for value %A" name value
        | _ ->
            if dic.ContainsKey key then
                false
            else
                dic.[key] <- value
                true

    /// <summary>Set value only if key does not exist yet.
    /// Same as <c>Dict.addIfKeyAbsent key value dic</c></summary>
    /// <param name="key">The key to set.</param>
    /// <param name="value">The value to set if the key is missing.</param>
    /// <param name="dic">The dictionary to change.</param>
    /// <returns><c>true</c> if the value was set, <c>false</c> if the key already existed and nothing was changed.</returns>
    let setIfKeyAbsent  (key:'Key) (value:'Value)  (dic:IDictionary<'Key,'Value>) : bool =
        setIfAbsent "Dict.setIfKeyAbsent" key value dic

    /// <summary>Set value only if key does not exist yet.
    /// Same as <c>Dict.setIfKeyAbsent key value dic</c></summary>
    /// <param name="key">The key to set.</param>
    /// <param name="value">The value to set if the key is missing.</param>
    /// <param name="dic">The dictionary to change.</param>
    /// <returns><c>true</c> if the value was set, <c>false</c> if the key already existed and nothing was changed.</returns>
    let addIfKeyAbsent  (key:'Key) (value:'Value)  (dic:IDictionary<'Key,'Value>) : bool =
        setIfAbsent "Dict.addIfKeyAbsent" key value dic

    /// <summary>If the key is not present calls the default function, sets its result as value at the key and returns it.
    /// This function is an alternative to the DefaultDict type. Use it if you need to provide a custom implementation of the default function depending on the key.</summary>
    /// <param name="getDefault">The function to create the value from the key, only called if the key is missing.</param>
    /// <param name="key">The key to look up.</param>
    /// <param name="dic">The dictionary to read from and change.</param>
    /// <returns>The existing or the newly created value.</returns>
    let getOrSetDefault (getDefault:'Key -> 'Value) (key:'Key)  (dic:IDictionary<'Key,'Value>) : 'Value =
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "Dict.getOrSetDefault: key is null"
        | _ ->
            match dic.TryGetValue(key) with
            |true, v-> v
            |false, _ ->
                let v = getDefault(key)
                dic.[key] <- v
                v

    /// <summary>If the key is not present sets the default value at the key and returns it.</summary>
    /// <param name="defaultValue">The value to set if the key is missing.</param>
    /// <param name="key">The key to look up.</param>
    /// <param name="dic">The dictionary to read from and change.</param>
    /// <returns>The existing value or the default value.</returns>
    let getOrSetDefaultValue (defaultValue: 'Value) (key:'Key)  (dic:IDictionary<'Key,'Value>) : 'Value =
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "Dict.getOrSetDefaultValue: key is null for default value %A" defaultValue
        | _ ->
            match dic.TryGetValue(key) with
            |true, v-> v
            |false, _ ->
                dic.[key] <- defaultValue
                defaultValue

    /// <summary>Tries to get a value and remove the key and value from the dictionary, like *.pop() in Python.</summary>
    /// <param name="key">The key to remove.</param>
    /// <param name="dic">The dictionary to change.</param>
    /// <returns><c>Some value</c> if the key was found, <c>None</c> if not.</returns>
    let tryPop(key:'Key)  (dic:IDictionary<'Key,'Value>) : 'Value option =
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "Dict.tryPop(key) key is null"
        | _ ->
            let ok, v = dic.TryGetValue(key)
            if ok then
                dic.Remove key |>ignore
                Some v
            else
                None


    /// <summary>Get a value and remove the key and value from the dictionary, like *.pop() in Python.
    /// Throws a KeyNotFoundException if the key does not exist.</summary>
    /// <param name="key">The key to remove.</param>
    /// <param name="dic">The dictionary to change.</param>
    /// <returns>The value that was stored at the key.</returns>
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


    /// <summary>Returns a (lazy) sequence of key and value tuples.</summary>
    /// <param name="dic">The dictionary to read from.</param>
    let items(dic:IDictionary<'Key,'Value>) : seq<'Key * 'Value> =
        seq { for kvp in dic -> kvp.Key, kvp.Value}

    /// <summary>Returns a (lazy) sequence of values.</summary>
    /// <param name="dic">The dictionary to read from.</param>
    let values (dic:IDictionary<'Key,'Value>) : seq<'Value>=
        seq { for kvp in dic -> kvp.Value}

    /// <summary>Returns a (lazy) sequence of keys.</summary>
    /// <param name="dic">The dictionary to read from.</param>
    let keys (dic:IDictionary<'Key,'Value>) : seq<'Key> =
        seq { for kvp in dic -> kvp.Key}

    /// <summary>Iterate over keys and values of a dictionary.</summary>
    /// <param name="f">The function to call with each key and value.</param>
    /// <param name="dic">The dictionary to iterate over.</param>
    let iter (f: 'Key -> 'Value -> unit) (dic:IDictionary<'Key,'Value>) : unit =
        for kvp in dic do
            f kvp.Key kvp.Value

    /// <summary>Map over keys and values of a dictionary.</summary>
    /// <param name="f">The function to call with each key and value.</param>
    /// <param name="dic">The dictionary to map over.</param>
    /// <returns>A (lazy) sequence of the results of f.</returns>
    let map(f: 'Key -> 'Value -> 'T) (dic:IDictionary<'Key,'Value>) : seq<'T> =
        seq { for kvp in dic do
                f kvp.Key kvp.Value}
