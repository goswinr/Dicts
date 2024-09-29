namespace FsEx

open System
open System.Collections.Generic


/// A System.Collections.Generic.Dictionary with default Values that get created upon accessing a missing key.
/// If accessing a non exiting key , the default function is called to create and set it.
/// Like defaultdict in Python
/// If you need to provide a custom implementation of the default function depending on the key
/// then use the Dict type and its method <c>dict.getOrSetDefault func key</c>.
[<NoComparison>]
[<NoEquality>] // TODO add structural equality
[<Sealed>]
type DefaultDict<'K,'V when 'K:equality > private (defaultOfKeyFun: 'K -> 'V, baseDict : Dictionary<'K,'V>) = 

    //using inheritance from Dictionary would not work because .Item method is sealed and cant have an override

    let dGet key  = 
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "DefaultDict.get key is null "
        | _ ->
            match baseDict.TryGetValue(key) with
            |true, v-> v
            |false, _ ->
                let v = defaultOfKeyFun(key)
                baseDict.[key] <- v
                v

    let set' key value = 
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise  "DefaultDict.set key is null for value %A" value
        | _ -> baseDict.[key] <- value


    /// <summary>A System.Collections.Generic.Dictionary with default Values that get created upon accessing a key.
    /// If accessing a non exiting key , the default function is called on the key to create the value and set it.
    /// Similar to  defaultdict in Python</summary>
    /// <param name="defaultOfKeyFun">(&apos;K-&gt;&apos;V): The function to create a default value from the key</param>
    new (defaultOfKeyFun: 'K -> 'V) = 
        let d = new  Dictionary<'K,'V>()
        DefaultDict( defaultOfKeyFun, d )


    /// Constructs a new DefaultDict by using the supplied Dictionary<'K,'V> directly, without any copying of items
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

    /// Access the underlying Collections.Generic.Dictionary<'K,'V>
    /// ATTENTION! This is not even a shallow copy, mutating it will also change this Instance of DefaultDict!
    /// Use #nowarn "44" to disable the obsolete warning
    [<Obsolete("It is not actually obsolete but unsafe to use, so hidden from editor tools. In F# use #nowarn \"44\" to disable the obsolete warning")>]
    member _.Dictionary = baseDict

    /// For Index operator: get or set the value for given key
    member _.Item
        with get k   = dGet k
        and  set k v = set' k v

    /// Get value for given key.
    /// Calls defaultFun to get value if key not found.
    /// Also sets key to returned value.
    /// Use .TryGetValue(k) if you don't want a missing key to be created
    member _.Get k = dGet k

    /// Set value for given key, same as <c>dict.add key value</c>
    member _.set key value = set' key value // dic.[key] <- value

    /// Set value for given key, same as <c>dict.set key value</c>
    member _.add key value = set' key value // dic.[key] <- value

    /// Get a value and remove key and value it from dictionary, like *.pop() in Python
    /// Will fail if key does not exist
    /// Does not set any new key if key is missing
    member _.Pop(k:'K) = 
        match box k with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "DefaultDict.Pop(key) key is null"
        | _ ->
            let ok, v = baseDict.TryGetValue(k)
            if ok then
                baseDict.Remove k |>ignore
                v
            else
                raise <|  KeyNotFoundException( sprintf "DefaultDict.Pop(key): Failed to pop key %A in %A of %d items" k baseDict baseDict.Count)

    /// Returns a (lazy) sequence of key and value tuples
    member _.Items = 
        seq { for KeyValue(k, v) in baseDict -> k, v}

    // override baseDict.ToString() = // covered by NiceString Pretty printer ?
        //stringBuffer {
        //    yield "DefaultDict with "
        //    yield baseDict.Count.ToString()
        //    yield! "entries"
        //    for k, v in baseDict.Items  |> Seq.truncate 3 do // Add sorting ? print 3 lines??
        //        yield  k.ToString()
        //        yield " : "
        //        yield! v.ToString()
        //    yield "..."
        //    }


    // -------------------- properties: --------------------------------------

    /// Gets the IEqualityComparer<T> that is used to determine equality of keys for the dictionary.
    member _.Comparer with get() = baseDict.Comparer

    /// Gets the number of key/value pairs contained in the dictionary
    member _.Count with get() = baseDict.Count

    /// Gets a collection containing the keys in the dictionary
    member _.Keys with get() = baseDict.Keys

    /// Gets a collection containing the values in the dictionary
    member _.Values with get() = baseDict.Values

    // -------------------------------------methods:-------------------------------

    /// Add the specified key and value to the dictionary.
    member _.Add(k, v) = baseDict.Add(k, v)

    /// Removes all keys and values from the dictionary
    member _.Clear() = baseDict.Clear()

    /// Determines whether the dictionary contains the specified key.
    member _.ContainsKey(k) = baseDict.ContainsKey(k)

    /// Determines whether the dictionary contains a specific value.
    member _.ContainsValue(v) = baseDict.ContainsValue(v)

    /// Removes the value with the specified key from the dictionary.
    /// See also .Pop(key) method to get the contained value too.
    member _.Remove(k) = baseDict.Remove(k)

    /// Gets the value associated with the specified key.
    /// As opposed to Get(key) this does not create a key if it is missing.
    member _.TryGetValue(k) = baseDict.TryGetValue(k)

    /// Returns an enumerator that iterates through the dictionary.
    member _.GetEnumerator() = baseDict.GetEnumerator()

    //---------------------------------------interfaces:-------------------------------------
    // TODO Add XML doc str

    interface IEnumerable<KeyValuePair<'K ,'V>> with
        member _.GetEnumerator() = (baseDict:>IDictionary<'K,'V>).GetEnumerator()

    interface Collections.IEnumerable with // Non generic needed too ?
        member __.GetEnumerator() = baseDict.GetEnumerator():> System.Collections.IEnumerator

    interface Collections.ICollection with // Non generic needed too ?
        member _.Count = baseDict.Count

        member _.CopyTo(arr, i) = (baseDict:>Collections.ICollection).CopyTo(arr, i)

        member _.IsSynchronized= (baseDict:>Collections.ICollection).IsSynchronized

        member _.SyncRoot= (baseDict:>Collections.ICollection).SyncRoot

    interface ICollection<KeyValuePair<'K,'V>> with
        member _.Add(x) = (baseDict:>ICollection<KeyValuePair<'K,'V>>).Add(x)

        member _.Clear() = baseDict.Clear()

        member _.Remove x = (baseDict:>ICollection<KeyValuePair<'K,'V>>).Remove x

        member _.Contains x = (baseDict:>ICollection<KeyValuePair<'K,'V>>).Contains x

        member _.CopyTo(arr, i) = (baseDict:>ICollection<KeyValuePair<'K,'V>>).CopyTo(arr, i)

        member _.IsReadOnly = false

        member _.Count = baseDict.Count

    interface IDictionary<'K,'V> with
        member _.Item
            with get k   = dGet k
            and  set k v = set' k v

        member _.Keys = (baseDict:>IDictionary<'K,'V>).Keys

        member _.Values = (baseDict:>IDictionary<'K,'V>).Values

        member _.Add(k, v) = baseDict.Add(k, v)

        member _.ContainsKey k = baseDict.ContainsKey k

        member _.TryGetValue(k, r ) = baseDict.TryGetValue(k, ref r)

        member _.Remove(k) = baseDict.Remove(k)

    interface IReadOnlyCollection<KeyValuePair<'K,'V>> with
        member _.Count = baseDict.Count

    interface IReadOnlyDictionary<'K,'V> with
        member _.Item
            with get k = dGet k

        member _.Keys = (baseDict:>IReadOnlyDictionary<'K,'V>).Keys

        member _.Values = (baseDict:>IReadOnlyDictionary<'K,'V>).Values

        member _.ContainsKey k = baseDict.ContainsKey k

        member _.TryGetValue(k, r ) = baseDict.TryGetValue(k, ref r)

    // TODO add these too?

    //member _.GetObjectData() = baseDict.GetObjectData()

    //member _.OnDeserialization() = baseDict.OnDeserialization()

    //member _.Equals() = baseDict.Equals()

    //member _.GetHashCode() = baseDict.GetHashCode()

    //member _.GetType() = baseDict.GetType()


    //interface _.ISerializable() = baseDict.ISerializable()

    //interface _.IDeserializationCallback() = baseDict.IDeserializationCallback()

