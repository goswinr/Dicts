namespace Dic

open System
open System.Collections.Generic


/// This module is set to auto open when opening FsEx namespace.
/// Static Extension methods on Exceptions to cal Exception.Raise "%A" x with F# printf string formatting
[<AutoOpen>]
[<Obsolete("It is not actually obsolete, but hidden from editor tools.")>]
module  AutoOpenExtensionsExceptions =

    type ArgumentNullException with
        /// Raise ArgumentNullException with F# printf string formatting
        static member Raise msg =  Printf.kprintf (fun s -> raise (ArgumentNullException(s))) msg

    type KeyNotFoundException with
        /// Raise KeyNotFoundException with F# printf string formatting
        static member Raise msg =  Printf.kprintf (fun s -> raise (KeyNotFoundException(s))) msg


/// A System.Collections.Generic.Dictionary with default Values that get created upon accessing a missing key.
/// If accessing a non exiting key , the default function is called to create and set it.
/// Like defaultDic in Python
/// If you need to provide a custom implementation of the default function depending on the key
/// then use the Dic type and its method <c>Dic.getOrSetDefault func key</c>.
[<NoComparison>]
[<NoEquality>] // TODO add structural equality
[<Sealed>]
type DefaultDic<'K,'V when 'K:equality > private (defaultOfKeyFun: 'K -> 'V, baseDic : Dictionary<'K,'V>) =

    //using inheritance from Dictionary would not work because .Item method is sealed and cant have an override

    let dGet key  =
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "DefaultDic.get key is null "
        | _ ->
            match baseDic.TryGetValue(key) with
            |true , v -> v
            |false, _ ->
                let v = defaultOfKeyFun(key)
                baseDic.[key] <- v
                v

    let set' key value =
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise  "DefaultDic.set key is null for value %A" value
        | _ -> baseDic.[key] <- value


    /// <summary>A System.Collections.Generic.Dictionary with default Values that get created upon accessing a key.
    /// If accessing a non exiting key , the default function is called on the key to create the value and set it.
    /// Similar to  defaultDic in Python</summary>
    /// <param name="defaultOfKeyFun">(&apos;K-&gt;&apos;V): The function to create a default value from the key</param>
    new (defaultOfKeyFun: 'K -> 'V) =
        let d = new  Dictionary<'K,'V>()
        DefaultDic( defaultOfKeyFun, d )


    /// Constructs a new DefaultDic by using the supplied Dictionary<'K,'V> directly, without any copying of items
    static member createDirectly (defaultOfKeyFun: 'K->'V) (di:Dictionary<'K,'V> ) =
        if isNull di then ArgumentNullException.Raise "Dictionary in DefaultDic.CreateDirectly is null"
        let d = new  Dictionary<'K,'V>()
        DefaultDic( defaultOfKeyFun, d)

    /// Constructs a new DefaultDic from seq of key and value pairs
    static member create (defaultOfKeyFun: 'K->'V) (keysValues: seq<'K * 'V>) =
        if isNull keysValues then ArgumentNullException.Raise "seq in DefaultDic.Create is null"
        let d = new  Dictionary<'K,'V>()
        for k,v in keysValues do
            d.[k] <- v
        DefaultDic( defaultOfKeyFun, d)

    /// Access the underlying Collections.Generic.Dictionary<'K,'V>
    /// ATTENTION! This is not even a shallow copy, mutating it will also change this Instance of DefaultDic!
    /// Use #nowarn "44" to disable the obsolete warning
    [<Obsolete("It is not actually obsolete but unsafe to use, so hidden from editor tools. In F# use #nowarn \"44\" to disable the obsolete warning")>]
    member _.Dictionary = baseDic

    /// For Index operator: get or set the value for given key
    member _.Item
        with get k   = dGet k
        and  set k v = set' k v

    /// Get value for given key.
    /// Calls defaultFun to get value if key not found.
    /// Also sets key to returned value.
    /// Use .TryGetValue(k) if you don't want a missing key to be created
    member _.Get k = dGet k

    /// Set value for given key, same as <c>Dic.add key value</c>
    member _.set key value = set' key value // dic.[key] <- value

    /// Set value for given key, same as <c>Dic.set key value</c>
    member _.add key value = set' key value // dic.[key] <- value

    /// Get a value and remove key and value it from Dictionary, like *.pop() in Python
    /// Will fail if key does not exist
    /// Does not set any new key if key is missing
    member _.Pop(k:'K) =
        match box k with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "DefaultDic.Pop(key) key is null"
        | _ ->
            let ok, v = baseDic.TryGetValue(k)
            if ok then
                baseDic.Remove k |>ignore
                v
            else
                raise <|  KeyNotFoundException( sprintf "DefaultDic.Pop(key): Failed to pop key %A in %A of %d items" k baseDic baseDic.Count)

    /// Returns a (lazy) sequence of key and value tuples
    member _.Items =
        seq { for KeyValue(k, v) in baseDic -> k, v}

    // override baseDic.ToString() = // covered by NiceString Pretty printer ?
        //stringBuffer {
        //    yield "DefaultDic with "
        //    yield baseDic.Count.ToString()
        //    yield! "entries"
        //    for k, v in baseDic.Items  |> Seq.truncate 3 do // Add sorting ? print 3 lines??
        //        yield  k.ToString()
        //        yield " : "
        //        yield! v.ToString()
        //    yield "..."
        //    }


    // -------------------- properties: --------------------------------------

    #if FABLE_COMPILER
    #else
    /// Gets the IEqualityComparer<T> that is used to determine equality of keys for the Dictionary.
    member _.Comparer with get() = baseDic.Comparer
    #endif

    /// Gets the number of key/value pairs contained in the Dictionary
    member _.Count with get() = baseDic.Count

    /// Gets a collection containing the keys in the Dictionary
    member _.Keys with get() = baseDic.Keys

    /// Gets a collection containing the values in the Dictionary
    member _.Values with get() = baseDic.Values

    // -------------------------------------methods:-------------------------------

    /// Add the specified key and value to the Dictionary.
    member _.Add(k, v) = baseDic.Add(k, v)

    /// Removes all keys and values from the Dictionary
    member _.Clear() = baseDic.Clear()

    /// Determines whether the Dictionary contains the specified key.
    member _.ContainsKey(k) = baseDic.ContainsKey(k)

    /// Determines whether the Dictionary contains a specific value.
    member _.ContainsValue(v) = baseDic.ContainsValue(v)

    /// Removes the value with the specified key from the Dictionary.
    /// See also .Pop(key) method to get the contained value too.
    member _.Remove(k) = baseDic.Remove(k)

    /// Gets the value associated with the specified key.
    /// As opposed to Get(key) this does not create a key if it is missing.
    member _.TryGetValue(k) = baseDic.TryGetValue(k)

    /// Returns an enumerator that iterates through the Dictionary.
    member _.GetEnumerator() = baseDic.GetEnumerator()

    //---------------------------------------interfaces:-------------------------------------
    // TODO Add XML doc str

    interface IEnumerable<KeyValuePair<'K ,'V>> with
        member _.GetEnumerator() = (baseDic:>IDictionary<'K,'V>).GetEnumerator()

    interface Collections.IEnumerable with // Non generic needed too ?
        member __.GetEnumerator() = baseDic.GetEnumerator():> System.Collections.IEnumerator

    interface Collections.ICollection with // Non generic needed too ?
        member _.Count = baseDic.Count

        member _.CopyTo(arr, i) = (baseDic:>Collections.ICollection).CopyTo(arr, i)

        member _.IsSynchronized= (baseDic:>Collections.ICollection).IsSynchronized

        member _.SyncRoot= (baseDic:>Collections.ICollection).SyncRoot

    interface ICollection<KeyValuePair<'K,'V>> with
        member _.Add(x) = (baseDic:>ICollection<KeyValuePair<'K,'V>>).Add(x)

        member _.Clear() = baseDic.Clear()

        member _.Remove x = (baseDic:>ICollection<KeyValuePair<'K,'V>>).Remove x

        member _.Contains x = (baseDic:>ICollection<KeyValuePair<'K,'V>>).Contains x

        member _.CopyTo(arr, i) = (baseDic:>ICollection<KeyValuePair<'K,'V>>).CopyTo(arr, i)

        member _.IsReadOnly = false

        member _.Count = baseDic.Count

    interface IDictionary<'K,'V> with
        member _.Item
            with get k   = dGet k
            and  set k v = set' k v

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
            with get k = dGet k

        member _.Keys = (baseDic:>IReadOnlyDictionary<'K,'V>).Keys

        member _.Values = (baseDic:>IReadOnlyDictionary<'K,'V>).Values

        member _.ContainsKey k = baseDic.ContainsKey k

        member _.TryGetValue(k, r ) = baseDic.TryGetValue(k, ref r)

    // TODO add these too?

    //member _.GetObjectData() = baseDic.GetObjectData()

    //member _.OnDeserialization() = baseDic.OnDeserialization()

    //member _.Equals() = baseDic.Equals()

    //member _.GetHashCode() = baseDic.GetHashCode()

    //member _.GetType() = baseDic.GetType()


    //interface _.ISerializable() = baseDic.ISerializable()

    //interface _.IDeserializationCallback() = baseDic.IDeserializationCallback()

