namespace Dicts

open System
open System.Collections.Generic
open ExtensionsExceptions

module internal DefaultDictUtil =
    // these functions can't be inside the class because of https://github.com/fable-compiler/Fable/issues/3911

    /// The internal get function, calls defaultOfKeyFun and sets the result if the key is missing.
    let inline dGet (baseDic:Dictionary<'K,'V>) defaultOfKeyFun key : 'V =
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "DefaultDict.get key is null "
        | _ ->
            match baseDic.TryGetValue(key) with
            |true , v -> v
            |false, _ ->
                let v = defaultOfKeyFun(key)
                baseDic.[key] <- v
                v

    /// The internal set function, that throws an exception if the key is null.
    let inline set' (baseDic:Dictionary<'K,'V>) key value : unit =
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise  "DefaultDict.set key is null for value %A" value
        | _ -> baseDic.[key] <- value


    /// The header of the string representation, k and v are the names of the key and value types.
    let inline toString (baseDic:Dictionary<'K,'V>) (k:string)  (v:string) : string =
        if baseDic.Count = 0 then
            $"empty DefaultDict<{k},{v}>"
        elif baseDic.Count = 1 then
            $"DefaultDict<{k},{v}> with 1 item"
        else
            $"DefaultDict<{k},{v}> with {baseDic.Count} items"


open DefaultDictUtil

/// A Collections.Generic.Dictionary<'K,'V>  with default Values that get created upon accessing a missing key.
/// If accessing a non exiting key , the default function is called to create and set it.
/// Inspired by the defaultdict in Python.
/// If you need to provide a custom implementation of the default function depending on each key,
/// then use the Dict<'K,'V> type and its method <c>dict.GetOrSetDefault func key</c>.
[<NoComparison>]
[<NoEquality>] // TODO add structural equality
[<Sealed>]
type DefaultDict<'K,'V when 'K:equality > private (defaultOfKeyFun: 'K -> 'V, baseDic : Dictionary<'K,'V>) =


    /// <summary>A Collections.Generic.Dictionary with default Values that get created upon accessing a key.
    /// If accessing a non exiting key , the default function is called on the key to create the value and set it.
    /// Similar to  defaultDic in Python</summary>
    /// <param name="defaultOfKeyFun">(&apos;K-&gt;&apos;V): The function to create a default value from the key</param>
    new (defaultOfKeyFun: 'K -> 'V) =
        let d = new  Dictionary<'K,'V>()
        DefaultDict( defaultOfKeyFun, d )


    /// <summary>Constructs a new DefaultDict by using the supplied Dictionary&lt;'K,'V&gt; directly, without any copying of items.</summary>
    /// <param name="defaultOfKeyFun">The function to create a default value from a missing key.</param>
    /// <param name="di">The Dictionary to wrap. Changes to it show in the new DefaultDict and the other way round.</param>
    static member createDirectly (defaultOfKeyFun: 'K->'V) (di:Dictionary<'K,'V> ) : DefaultDict<'K,'V> =
        if isNull di then ArgumentNullException.Raise "Dictionary in DefaultDict.createDirectly is null"
        DefaultDict( defaultOfKeyFun, di)

    /// <summary>Constructs a new DefaultDict from seq of key and value pairs.
    /// Like the Dictionary constructor, it throws an ArgumentException on duplicate keys.</summary>
    /// <param name="defaultOfKeyFun">The function to create a default value from a missing key.</param>
    /// <param name="keysValues">The key and value pairs to fill the new DefaultDict with.</param>
    static member create (defaultOfKeyFun: 'K->'V) (keysValues: seq<'K * 'V>) : DefaultDict<'K,'V> =
        if isNull keysValues then ArgumentNullException.Raise "seq in DefaultDict.create is null"
        let d = new  Dictionary<'K,'V>()
        for k,v in keysValues do
            DictUtil.add' "DefaultDict.create" k v d
        DefaultDict( defaultOfKeyFun, d)

    /// Access the underlying Collections.Generic.Dictionary<'K,'V>.
    /// ATTENTION! This is not even a shallow copy, mutating it will also change this instance of DefaultDict!
    member _.InternalDictionary : Dictionary<'K,'V> =
        baseDic

    /// For Index operator .[i]: get or set the value for a given key
    /// Calls defaultFun to get value if key not found.
    /// Also sets the key to returned value.
    /// Use dict.TryGetValue(k) if you don't want a missing key to be created on the DefaultDict
    member _.Item
        with get k   = dGet baseDic defaultOfKeyFun k
        and  set k v = set' baseDic k v

    /// <summary>Get value for given key.
    /// Calls defaultFun to get value if key not found.
    /// Also sets key to returned value.
    /// Use .TryGetValue(k) if you don't want a missing key to be created</summary>
    /// <param name="k">The key to look up.</param>
    member _.Get k : 'V =
        dGet baseDic defaultOfKeyFun k

    /// <summary>Set value for given key, adds the key if it is missing.
    /// Same as the indexer setter.</summary>
    /// <param name="key">The key to set.</param>
    /// <param name="value">The value to set.</param>
    member _.Set key value : unit =
        set' baseDic key value


    /// <summary>Get a value and remove the key and value from the DefaultDict, like *.pop() in Python.
    /// Throws a KeyNotFoundException if the key does not exist.
    /// Does not create a missing key.</summary>
    /// <param name="k">The key to remove.</param>
    /// <returns>The value that was stored at the key.</returns>
    member _.Pop(k:'K) : 'V =
        match box k with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "DefaultDict.Pop(key) key is null"
        | _ ->
            let ok, v = baseDic.TryGetValue(k)
            if ok then
                baseDic.Remove k |>ignore
                v
            else
                KeyNotFoundException.Raise "DefaultDict.Pop(key): Failed to pop key %A in %A of %d items" k baseDic baseDic.Count

    /// <summary>Try to get a value and remove the key and value from the DefaultDict, like *.pop() in Python.
    /// Does not create a missing key.</summary>
    /// <param name="k">The key to remove.</param>
    /// <returns><c>Some value</c> if the key was found, <c>None</c> if not.</returns>
    member _.TryPop(k:'K) : 'V option =
        match box k with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "DefaultDict.TryPop(key) key is null"
        | _ ->
            let ok, v = baseDic.TryGetValue(k)
            if ok then
                baseDic.Remove k |>ignore
                Some v
            else
                None


    /// Returns a (lazy) sequence of key and value tuples
    member _.Items : seq<'K * 'V> =
        seq { for KeyValue(k, v) in baseDic -> k, v}

    /// <summary>Determines whether the DefaultDict does not contain the specified key.
    /// Same as <c>not(dic.ContainsKey(key))</c></summary>
    /// <param name="key">The key to look for.</param>
    member _.DoesNotContainKey(key) : bool = not(baseDic.ContainsKey(key))


    /// The string representation of the DefaultDict including the count of entries.
    override _.ToString() =
        #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
        let k = "'K"
        let v = "'V"
        #else
        let k = typeof<'K>.Name
        let v = typeof<'V>.Name
        #endif
        toString baseDic k v

    /// A string representation of the DefaultDict including the count of entries, the first 5 entries and the last entry.
    /// When used in Fable this member is inlined for reflection to work.
    #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
    member inline _.AsString : string =  // inline needed for Fable reflection
    #else
    member _.AsString : string =  // on .NET inline fails because it's using internal DefaultDictUtil
    #endif
        PrettyPrint.withEntries (toString baseDic (typeof<'K>.Name) (typeof<'V>.Name)) baseDic.Count baseDic 5


    /// <summary>A string representation of the DefaultDict including the count of entries,
    /// the specified amount of entries and the last entry.
    /// When used in Fable this member is inlined for reflection to work.</summary>
    /// <param name="entriesToPrint">The maximum number of entries to show. Zero or less shows only the header.</param>
    #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
    member inline _.ToString(entriesToPrint:int) : string =  // inline needed for Fable reflection
    #else
    member _.ToString(entriesToPrint:int) : string = // on .NET inline fails because it's using internal DefaultDictUtil
    #endif
        PrettyPrint.withEntries (toString baseDic (typeof<'K>.Name) (typeof<'V>.Name)) baseDic.Count baseDic entriesToPrint

    /// <summary>Set value for given key, adds the key if it is missing.
    /// Same as <c>dd.Set key value</c></summary>
    /// <param name="key">The key to set.</param>
    /// <param name="value">The value to set.</param>
    /// <param name="dd">The DefaultDict to change.</param>
    static member set key value (dd:DefaultDict<'K,'V>) : unit =
        dd.Set key value

    /// <summary>Get value for given key.
    /// Calls defaultFun to get value if key not found, and sets it.
    /// Same as <c>dd.Get key</c></summary>
    /// <param name="key">The key to look up.</param>
    /// <param name="dd">The DefaultDict to read from.</param>
    static member get key (dd:DefaultDict<'K,'V>) : 'V =
        dd.Get key


    // -------------------------------------------------------------------
    // members to match ofSystem.Collections.Generic.Dictionary<'K,'V>:
    // -------------------------------------------------------------------

    // -------------------- properties: --------------------------------------

    // #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
    // #else
    // /// Gets the IEqualityComparer<T> that is used to determine equality of keys for the DefaultDict.
    // member _.Comparer with get() = baseDic.Comparer
    // #endif

    /// Gets the number of key/value pairs contained in the DefaultDict
    member _.Count with get() = baseDic.Count

    /// Gets a collection containing the keys in the DefaultDict
    member _.Keys with get() = baseDic.Keys

    /// Gets a collection containing the values in the DefaultDict
    member _.Values with get() = baseDic.Values

    // -------------------------------------methods:-------------------------------

    /// <summary>Add the specified key and value to the DefaultDict.
    /// Like Dictionary.Add, it throws an ArgumentException if the key already exists.
    /// Use .Set(key, value) or the indexer to add or replace a value.</summary>
    /// <param name="k">The key to add.</param>
    /// <param name="v">The value to add.</param>
    member _.Add(k:'K, v:'V) : unit = DictUtil.add' "DefaultDict.Add" k v baseDic

    /// Removes all keys and values from the DefaultDict
    member _.Clear() : unit = baseDic.Clear()

    /// <summary>Determines whether the DefaultDict contains the specified key.
    /// Does not create a missing key.</summary>
    /// <param name="k">The key to look for.</param>
    member _.ContainsKey(k) : bool = baseDic.ContainsKey(k)

    /// <summary>Determines whether the DefaultDict contains a specific value.</summary>
    /// <param name="v">The value to look for.</param>
    member _.ContainsValue(v) : bool = baseDic.ContainsValue(v)

    /// <summary>Removes the value with the specified key from the DefaultDict.
    /// See also the .Pop(key) method that returns the removed value.</summary>
    /// <param name="k">The key to remove.</param>
    /// <returns><c>true</c> if the key was found and removed, <c>false</c> if not.</returns>
    member _.Remove(k) : bool = baseDic.Remove(k)

    /// <summary>Gets the value associated with the specified key.
    /// As opposed to Get(key) this does not create a key if it is missing.</summary>
    /// <param name="k">The key to look up.</param>
    /// <returns><c>true</c> and the value if the key was found, <c>false</c> and the default of 'V if not.</returns>
    member _.TryGetValue(k) : bool * 'V = baseDic.TryGetValue(k)


    /// Returns an enumerator that iterates through the DefaultDict.
    member _.GetEnumerator() : Dictionary<'K,'V>.Enumerator = baseDic.GetEnumerator()

    //---------------------------------------interfaces:-------------------------------------
    // The interface implementations have no own docs, editors show the docs of the interface members.

    interface IEnumerable<KeyValuePair<'K ,'V>> with
        member _.GetEnumerator() : IEnumerator<KeyValuePair<'K,'V>> = (baseDic:>IDictionary<'K,'V>).GetEnumerator()

    interface Collections.IEnumerable with // Non generic needed too ?
        member __.GetEnumerator() : Collections.IEnumerator = baseDic.GetEnumerator():> System.Collections.IEnumerator

    // The non generic Collections.ICollection is not implemented because it would yield invalid signatures in Fable Typescript target.
    // See the comment and Fable REPL link in Dict.fs

    interface ICollection<KeyValuePair<'K,'V>> with
        // not delegating to (baseDic:>ICollection<KeyValuePair<'K,'V>>) because it fails on Fable: https://github.com/fable-compiler/Fable/issues/3914
        member _.Add(x) : unit = DictUtil.add' "DefaultDict.Add" x.Key x.Value baseDic

        member _.Clear() : unit = baseDic.Clear()

        member _.Remove x : bool = DictUtil.removePair x baseDic

        member _.Contains x : bool = DictUtil.containsPair x baseDic

        member _.CopyTo(arr, i) : unit = (baseDic:>ICollection<KeyValuePair<'K,'V>>).CopyTo(arr, i)

        member _.IsReadOnly : bool = false

        member _.Count : int = baseDic.Count

    interface IReadOnlyCollection<KeyValuePair<'K,'V>> with
        member _.Count : int = baseDic.Count

    // IDictionary and IReadOnlyDictionary are not implemented because the semantics don't fit:
    // TryGetValue would return no value for a missing key, while Get and the indexer would create one.
