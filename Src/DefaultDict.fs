namespace Dicts

open System
open System.Collections.Generic
open ExtensionsExceptions

module internal DefaultDictUtil =
    // these functions can't be inside the class because of https://github.com/fable-compiler/Fable/issues/3911

    let inline dGet (baseDic:Dictionary<'K,'V>) defaultOfKeyFun key  =
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "DefaultDict.get key is null "
        | _ ->
            match baseDic.TryGetValue(key) with
            |true , v -> v
            |false, _ ->
                let v = defaultOfKeyFun(key)
                baseDic.[key] <- v
                v

    let inline set' (baseDic:Dictionary<'K,'V>) key value =
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise  "DefaultDict.set key is null for value %A" value
        | _ -> baseDic.[key] <- value


    let inline toString (baseDic:Dictionary<'K,'V>) (k:string)  (v:string) =
        if baseDic.Count = 0 then
            $"empty DefaultDict<{k},{v}>"
        elif baseDic.Count = 1 then
            $"DefaultDict<{k},{v}> with 1 item"
        else
            $"DefaultDict<{k},{v}> with {baseDic.Count} items"


open DefaultDictUtil

/// A Collections.Generic.Dictionary<'K,'V>  with default Values that get created upon accessing a missing key.
/// If accessing a non exiting key , the default function is called to create and set it.
/// Inspired by the  defaultdict in Python.
/// If you need to provide a custom implementation of the default function depending on each key,
/// then use the Dict<'K,'V>  type and it's method <c>Dicts.getOrSetDefault func key</c>.
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


    /// Constructs a new DefaultDict by using the supplied Dictionary<'K,'V>  directly, without any copying of items
    static member createDirectly (defaultOfKeyFun: 'K->'V) (di:Dictionary<'K,'V> ) =
        if isNull di then ArgumentNullException.Raise "Dictionary in DefaultDict.CreateDirectly is null"
        let d = new  Dictionary<'K,'V>()
        DefaultDict( defaultOfKeyFun, d)

    /// Constructs a new DefaultDict from seq of key and value pairs
    static member create (defaultOfKeyFun: 'K->'V) (keysValues: seq<'K * 'V>) =
        if isNull keysValues then ArgumentNullException.Raise "seq in DefaultDict.Create is null"
        let d = new  Dictionary<'K,'V>()
        for k,v in keysValues do
            d.[k] <- v
        DefaultDict( defaultOfKeyFun, d)

    /// Access the underlying Collections.Generic.Dictionary<'K,'V>.
    /// ATTENTION! This is not even a shallow copy, mutating it will also change this instance of DefaultDict!
    member _.InternalDictionary =
        baseDic

    /// For Index operator .[i]: get or set the value for a given key
    member _.Item
        with get k   = dGet baseDic defaultOfKeyFun k
        and  set k v = set' baseDic k v

    /// Get value for given key.
    /// Calls defaultFun to get value if key not found.
    /// Also sets key to returned value.
    /// Use .TryGetValue(k) if you don't want a missing key to be created
    member _.Get k =
        dGet baseDic defaultOfKeyFun k

    /// Set value for given key, same as <c>Dicts.Add(key, value)</c>
    member _.Set key value =
        set' baseDic key value


    /// Get a value and remove key and value it from Dictionary, like *.pop() in Python
    /// Will fail if key does not exist
    /// Does not set any new key if key is missing
    member _.Pop(k:'K) =
        match box k with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "DefaultDict.Pop(key) key is null"
        | _ ->
            let ok, v = baseDic.TryGetValue(k)
            if ok then
                baseDic.Remove k |>ignore
                v
            else
                raise <|  KeyNotFoundException( sprintf "DefaultDict.Pop(key): Failed to pop key %A in %A of %d items" k baseDic baseDic.Count)

    /// Returns a (lazy) sequence of key and value tuples
    member _.Items =
        seq { for KeyValue(k, v) in baseDic -> k, v}

    /// Determines whether the DefaultDict does not contains the specified key.
    /// not(dic.ContainsKey(key))
    member _.DoesNotContainKey(key) = not(baseDic.ContainsKey(key))


    /// The string representation of the DefaultDict including the count of entries.
    override _.ToString() =
        #if FABLE_COMPILER
        let k = "'K"
        let v = "'V"
        #else
        let k = typeof<'K>.Name
        let v = typeof<'V>.Name
        #endif
        toString baseDic k v

    /// A string representation of the DefaultDict including the count of entries and the first 5 entries.
    /// When used in Fable this member is inlined for reflection to work.
    #if FABLE_COMPILER
    member inline _.AsString =  // inline needed for Fable reflection
    #else
    member _.AsString =  // on .NET inline fails because it's using internal DefaultDictUtil
    #endif
        let b = Text.StringBuilder()
        let c = baseDic.Count
        let st = toString baseDic (typeof<'K>.Name) (typeof<'V>.Name)
        b.Append st |> ignore
        if c > 0  then b.AppendLine ":"  |> ignore
        for KeyValue(k, v) in baseDic  |> Seq.truncate 5 do // Add sorting ? print 3 lines??
            b.AppendLine $"  {k} : {v}" |> ignore
        if c > 5 then b.AppendLine "  ..." |> ignore
        b.ToString()


    /// A string representation of the DefaultDict including the count of entries
    /// and the specified amount of entries.
    /// When used in Fable this member is inlined for reflection to work.
    #if FABLE_COMPILER
    member inline _.ToString(entriesToPrint) =  // inline needed for Fable reflection
    #else
    member _.ToString(entriesToPrint) = // on .NET inline fails because it's using internal DefaultDictUtil
    #endif
        let b = Text.StringBuilder()
        let c = baseDic.Count
        let st = toString baseDic (typeof<'K>.Name) (typeof<'V>.Name)
        b.Append st |> ignore
        if c > 0  && entriesToPrint > 0 then b.AppendLine ":"  |> ignore
        for KeyValue(k, v) in baseDic |> Seq.truncate (max 0 entriesToPrint) do // Add sorting ? print 3 lines??
            b.AppendLine $"  {k} : {v}" |> ignore
        if c > entriesToPrint then b.AppendLine "  ..." |> ignore
        b.ToString()

    /// Set value for given key, same as <c>Dicts.Add(key, value)</c>
    static member set (dd:DefaultDict<'K,'V>) key value =
        dd.Set key value

    /// Get value for given key.
    static member get (dd:DefaultDict<'K,'V>) key =
        dd.Get key


    // -------------------------------------------------------------------
    // members to match ofSystem.Collections.Generic.Dictionary<'K,'V>:
    // -------------------------------------------------------------------

    // -------------------- properties: --------------------------------------

    // #if FABLE_COMPILER
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

    /// Add the specified key and value to the DefaultDict.
    member _.Add(k, v) = baseDic.Add(k, v)

    /// Removes all keys and values from the DefaultDict
    member _.Clear() = baseDic.Clear()

    /// Determines whether the DefaultDict contains the specified key.
    member _.ContainsKey(k) = baseDic.ContainsKey(k)

    /// Determines whether the DefaultDict contains a specific value.
    member _.ContainsValue(v) = baseDic.ContainsValue(v)

    /// Removes the value with the specified key from the DefaultDict.
    /// See also .Pop(key) method to get the contained value too.
    member _.Remove(k) = baseDic.Remove(k)

    /// Gets the value associated with the specified key.
    /// As opposed to Get(key) this does not create a key if it is missing.
    member _.TryGetValue(k) = baseDic.TryGetValue(k)

    (*
    /// Returns an enumerator that iterates through the DefaultDict.
    member _.GetEnumerator() = baseDic.GetEnumerator()

    //---------------------------------------interfaces:-------------------------------------
    // TODO Add XML doc str

    interface IEnumerable<KeyValuePair<'K ,'V>> with
        member _.GetEnumerator() = (baseDic:>IDictionary<'K,'V>).GetEnumerator()

    interface Collections.IEnumerable with // Non generic needed too ?
        member __.GetEnumerator() = baseDic.GetEnumerator():> System.Collections.IEnumerator


    //no ICollections because of https://github.com/fable-compiler/Fable/issues/3914
    interface Collections.ICollection with // Non generic needed too ?
        member _.Count = baseDic.Count

        member _.CopyTo(arr, i) = (baseDic:>Collections.ICollection).CopyTo(arr, i)

        member _.IsSynchronized= (baseDic:>Collections.ICollection).IsSynchronized

        member _.SyncRoot= (baseDic:>Collections.ICollection).SyncRoot

    interface ICollection<KeyValuePair<'K,'V>> with
        member _.Add(x) = baseDic.Add(x.Key, x.Value) //(baseDic:>ICollection<KeyValuePair<'K,'V>>).Add(x) // fails on Fable: https://github.com/fable-compiler/Fable/issues/3914

        member _.Clear() = baseDic.Clear()

        member _.Remove x =  baseDic.Remove(x.Key) // (baseDic:>ICollection<KeyValuePair<'K,'V>>).Remove x

        member _.Contains x =  baseDic.ContainsKey x.Key //(baseDic:>ICollection<KeyValuePair<'K,'V>>).Contains x

        member _.CopyTo(arr, i) = (baseDic:>ICollection<KeyValuePair<'K,'V>>).CopyTo(arr, i)

        member _.IsReadOnly = false

        member _.Count = baseDic.Count


    interface IDictionary<'K,'V> with
        member _.Item
            with get k   = dGet baseDic defaultOfKeyFun k
            and  set k v = set' baseDic k v

        member _.Keys = (baseDic:>IDictionary<'K,'V>).Keys

        member _.Values = (baseDic:>IDictionary<'K,'V>).Values

        member _.Add(k, v) = baseDic.Add(k, v)

        member _.ContainsKey k = baseDic.ContainsKey k

        member _.TryGetValue(k, r ) = baseDic.TryGetValue(k, ref r)

        member _.Remove(k) = baseDic.Remove(k)

    interface IReadOnlyCollection<KeyValuePair<'K,'V>> with
        member _.Count = baseDic.Count

    interface IReadOnlyDictionary<'K,'V> with
        member _.Item
            with get k = dGet baseDic defaultOfKeyFun k

        member _.Keys = (baseDic:>IReadOnlyDictionary<'K,'V>).Keys

        member _.Values = (baseDic:>IReadOnlyDictionary<'K,'V>).Values

        member _.ContainsKey k = baseDic.ContainsKey k

        member _.TryGetValue(k, r ) = baseDic.TryGetValue(k, ref r)
    *)

    // TODO add these too?

    //member _.GetObjectData() = baseDic.GetObjectData()

    //member _.OnDeserialization() = baseDic.OnDeserialization()

    //member _.Equals() = baseDic.Equals()

    //member _.GetHashCode() = baseDic.GetHashCode()

    //member _.GetType() = baseDic.GetType()


    //interface _.ISerializable() = baseDic.ISerializable()

    //interface _.IDeserializationCallback() = baseDic.IDeserializationCallback()

