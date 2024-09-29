namespace FsEx

open System
open System.Collections.Generic


/// A thin wrapper over System.Collections.Generic.Dictionary<'K,'V>) with nicer Error messages on accessing missing keys.
/// There is a hidden member called "Dictionary" to access the underlying Collections.Generic.Dictionary<'K,'V> directly.
/// In F# use #nowarn "44" to disable the obsolete warning for this hidden member.
[<NoComparison>]
[<NoEquality>] // TODO add structural equality
[<Sealed>]
type Dict<'K,'V when 'K:equality > private (dic : Dictionary<'K,'V>) = 

    //using inheritance from Dictionary would not work because .Item method is sealed and can't have an override

    let get' key  = 
         match box key with // or https://stackoverflow.com/a/864860/969070
         | null -> ArgumentNullException.Raise "Dict.get key is null "
         | _ ->
             match dic.TryGetValue(key) with
             |true, v-> v
             |false, _ ->
                 let keys = NiceString.toNiceString dic.Keys
                 KeyNotFoundException.Raise "Dict.get failed to find key %A in %A  of %d items. Keys: %s" key dic dic.Count keys

    let set' key value = 
         match box key with // or https://stackoverflow.com/a/864860/969070
         | null -> ArgumentNullException.Raise  "Dict.set key is null for value %A" value
         | _ -> dic.[key] <- value

    /// Create a new empty Dict<'K,'V>.
    /// A Dict is a thin wrapper over System.Collections.Generic.Dictionary<'K,'V>) with nicer Error messages on accessing missing keys.
    new () = Dict(new Dictionary<'K,'V>())

    /// Create a new empty Dict<'K,'V> with an IEqualityComparer like HashIdentity.Structural.
    /// A Dict is a thin wrapper over System.Collections.Generic.Dictionary<'K,'V>) with nicer Error messages on accessing missing keys.
    new (iEqualityComparer:IEqualityComparer<'K>) = Dict(new Dictionary<'K,'V>(iEqualityComparer))

    /// Constructs a new Dict by using the supplied Dictionary<'K,'V> directly, without any copying of items
    static member CreateDirectly (dic:Dictionary<'K,'V> ) = 
        if isNull dic then ArgumentNullException.Raise "Dictionary in Dict.CreateDirectly is null"
        Dict(dic)

    /// Access the underlying Collections.Generic.Dictionary<'K,'V>)
    /// ATTENTION! This is not even a shallow copy, mutating it will also change this instance of Dict!
    /// use #nowarn "44" to disable the obsolete warning
    [<Obsolete("It is not actually obsolete, but normally not used, so hidden from editor tools. In F# use #nowarn \"44\" to disable the obsolete warning")>]
    member _.Dictionary = dic

    /// For Index operator .[i]: get or set the value for given key
    member _.Item
        with get k   = get' k
        and  set k v = set' k v //dic.[k] <- v

    /// Get value for given key
    member _.Get key = get' key

    /// Set value for given key, same as <c>dict.add key value</c>
    member _.set key value = set' key value // dic.[key] <- value

    /// Set value for given key, same as <c>dict.set key value</c>
    member _.add key value = set' key value // dic.[key] <- value

    /// Set value only if key does not exist yet.
    /// Returns false if key already exist, does not set value in this case
    /// Same as <c>dict.addOnce key value</c>
    member _.setIfKeyAbsent (key:'K) (value:'V) = 
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "Dict.setOnce key is null "
        | _ ->
            if dic.ContainsKey key then
                false
            else
                dic.[key] <- value
                true
    /// Set value only if key does not exist yet.
    /// Returns false if key already exist, does not set value in this case
    /// Same as <c>dict.setOnce key value</c>
    member _.addIfKeyAbsent  (key:'K) (value:'V) = 
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "Dict.addOnce key is null "
        | _ ->
            if dic.ContainsKey key then
                false
            else
                dic.[key] <- value
                true
    
    /// If the key ist not present calls the default function, set it as value at the key and return the value.
    /// This function is an alternative to the DefaultDict type. Use it if you need to provide a custom implementation of the default function depending on the key.
    member _.getOrSetDefault (getDefault:'K -> 'V) (key:'K)   = 
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "Dict.getOrSetDefault key is null "
        | _ ->
            match dic.TryGetValue(key) with
            |true, v-> v
            |false, _ ->
                let v = getDefault(key)
                dic.[key] <- v
                v
    /// If the key ist not present set it as value at the key and return the value.    
    member _.getOrSetDefaultValue (defaultValue: 'V) (key:'K)   = 
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "Dict.getOrSetDefaultValue key is null "
        | _ ->
            match dic.TryGetValue(key) with
            |true, v-> v
            |false, _ ->
                let v = defaultValue
                dic.[key] <- v
                v


    /// Get a value and remove key and value it from dictionary, like *.pop() in Python.
    /// Will fail if key does not exist
    member _.Pop(key:'K) = 
        match box key with // or https://stackoverflow.com/a/864860/969070
        | null -> ArgumentNullException.Raise "Dict.Pop(key) key is null"
        | _ ->
            let ok, v = dic.TryGetValue(key)
            if ok then
                dic.Remove key |>ignore
                v
            else
                KeyNotFoundException.Raise "Dict.Pop(key): Failed to pop key %A in %A of %d items" key dic dic.Count

    /// Returns a (lazy) sequence of key and value tuples
    member _.Items = 
        seq { for kvp in dic -> kvp.Key, kvp.Value}

    /// Returns a (lazy) sequence of values
    member _.ValuesSeq with get() =  
        seq { for kvp in dic -> kvp.Value}

    /// Returns a (lazy) sequence of Keys
    member _.KeysSeq with get() =  
        seq { for kvp in dic -> kvp.Key}
    
    /// Determines whether the dictionary does not contains the specified key.
    /// not(dic.ContainsKey(key))
    member _.DoesNotContainKey(key) = not(dic.ContainsKey(key))
    
    /// Determines whether the dictionary does not contains the specified key.
    /// not(dic.ContainsKey(key))
    [<Obsolete("Use more explicit method 'DoesNotContainKey' instead")>]
    member _.DoesNotContain(key) = not(dic.ContainsKey(key))
    
    //override dic.ToString() = // covered by NiceString Pretty printer ? TODO
        //stringBuffer {
        //    yield "DefaultDict with "
        //    yield dic.Count.ToString()
        //    yield! "entries"
        //    for k, v in dic.Items  |> Seq.truncate 3 do // dic sorting ? print 3 lines??
        //        yield  k.ToString()
        //        yield " : "
        //        yield! v.ToString()
        //    yield "..."
        //    }
    // ------------------member to match ofSystem.Collections.Generic.Dictionary<'K,'V>-------------

    // -------------------- properties:  --------------------------------------

    /// Gets the IEqualityComparer<T> that is used to determine equality of keys for the dictionary.
    member _.Comparer with get() = dic.Comparer

    /// Gets the number of key/value pairs contained in the dictionary
    member _.Count with get() = dic.Count

    /// Gets a collection containing the keys in the dictionary
    /// same as on System.Collections.Generic.Dictionary<'K,'V>
    member _.Keys with get() = dic.Keys

    /// Gets a collection containing the values in the dictionary
    /// same as on System.Collections.Generic.Dictionary<'K,'V>
    member _.Values with get() = dic.Values

    // -------------------------------------methods:-------------------------------

    /// Add the specified key and value to the dictionary.
    member _.Add(key, value) = set' key value //dic.Add(key, value)

    /// Removes all keys and values from the dictionary
    member _.Clear() = dic.Clear()

    /// Determines whether the dictionary contains the specified key.
    member _.ContainsKey(key) = dic.ContainsKey(key)

    /// Determines whether the dictionary contains a specific value.
    member _.ContainsValue(value) = dic.ContainsValue(value)

    /// Removes the value with the specified key from the dictionary.
    /// See also .Pop(key) method that gets the contained value too.
    member _.Remove(key) = dic.Remove(key)

    /// <summary>Lookup an element in the Dict, assigning it to <c>refValue</c> if the element is in the Dict and return true. Otherwise returning <c>false</c> .</summary>
    /// <param name="key">The input key.</param>
    /// <param name="refValue">A reference to the output value.</param>
    /// <returns><c>true</c> if the value is present, <c>false</c> if not.</returns>
    member _.TryGetValue(key:'K , [<Runtime.InteropServices.Out>] refValue : byref<'V>) : bool = 
        let mutable out = Unchecked.defaultof<'V>
        let found = dic.TryGetValue(key, &out)
        refValue <- out
        found

    /// Returns an enumerator that iterates through the dictionary.
    member _.GetEnumerator() = dic.GetEnumerator()

    //---------------------------------------interfaces:-------------------------------------
    // TODO dic XML doc str

    interface IEnumerable<KeyValuePair<'K ,'V>> with
        member _.GetEnumerator() = (dic:>IDictionary<'K,'V>).GetEnumerator()

    interface Collections.IEnumerable with // Non generic needed too ?
        member __.GetEnumerator() = dic.GetEnumerator():> System.Collections.IEnumerator

    interface Collections.ICollection with // Non generic needed too ?
        member _.Count = dic.Count

        member _.CopyTo(arr, i) = (dic:>Collections.ICollection).CopyTo(arr, i)

        member _.IsSynchronized= (dic:>Collections.ICollection).IsSynchronized

        member _.SyncRoot= (dic:>Collections.ICollection).SyncRoot

    interface ICollection<KeyValuePair<'K,'V>> with
        member _.Add(x) = (dic:>ICollection<KeyValuePair<'K,'V>>).Add(x)

        member _.Clear() = dic.Clear()

        member _.Remove kvp = (dic:>ICollection<KeyValuePair<'K,'V>>).Remove kvp

        member _.Contains kvp = (dic:>ICollection<KeyValuePair<'K,'V>>).Contains kvp

        member _.CopyTo(arr, i) = (dic:>ICollection<KeyValuePair<'K,'V>>).CopyTo(arr, i)

        member _.IsReadOnly = false

        member _.Count = dic.Count

    interface IDictionary<'K,'V> with
        member _.Item
            with get k   = get' k
            and  set k v = set' k v // dic.[k] <- v

        member _.Keys = (dic:>IDictionary<'K,'V>).Keys

        member _.Values = (dic:>IDictionary<'K,'V>).Values

        member _.Add(k, v) = dic.Add(k, v)

        member _.ContainsKey k = dic.ContainsKey k

        member _.TryGetValue(key:'K , [<Runtime.InteropServices.Out>] refValue : byref<'V>) : bool = 
            let mutable out = Unchecked.defaultof<'V>
            let found = dic.TryGetValue(key, &out)
            refValue <- out
            found

        member _.Remove(key) = dic.Remove(key)

    interface IReadOnlyCollection<KeyValuePair<'K,'V>> with
        member _.Count = dic.Count

    interface IReadOnlyDictionary<'K,'V> with
        member _.Item
            with get k = get' k

        member _.Keys = (dic:>IReadOnlyDictionary<'K,'V>).Keys

        member _.Values = (dic:>IReadOnlyDictionary<'K,'V>).Values

        member _.ContainsKey(key) = dic.ContainsKey(key)

        member _.TryGetValue(key:'K , [<Runtime.InteropServices.Out>] refValue : byref<'V>) : bool = 
            let mutable out = Unchecked.defaultof<'V>
            let found = dic.TryGetValue(key, &out)
            refValue <- out
            found

    // TODO

    //member _.GetObjectData(info,context) = dic.GetObjectData(info,context)

    //member _.OnDeserialization() = dic.OnDeserialization()

    //member _.Equals() = dic.Equals()

    //member _.GetHashCode() = dic.GetHashCode()

    //member _.GetType() = dic.GetType()


    //interface _.ISerializable() = dic.ISerializable()

    //interface _.IDeserializationCallback() = dic.IDeserializationCallback()

