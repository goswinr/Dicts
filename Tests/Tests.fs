module TestList

open Dicts
open System.Collections.Generic
open Scriptorium.Nib.Assertion
open type Scriptorium.Quill.Test

open System
open ExtensionsIDictionary

#nowarn "3370" // incr ref

/// Checks that f throws and that the exception message contains the given text.
/// On .NET it also checks the exact exception type.
let throwsWith<'E when 'E :> exn> (text:string) (f: unit -> unit) (msg:string) =
    assertThat f (
        tag msg
        >> throws
        >> assertion (fun e -> e.Message.Contains text) (fun e -> $"message '{e.Message}' should contain '{text}'")
        #if !FABLE_COMPILER
        >> assertion (fun e -> e.GetType() = typeof<'E>) (fun e -> $"exception type {e.GetType().Name} should be {typeof<'E>.Name}")
        #endif
    )


let tests  =
  testList ("Module.fs Tests", [

    // ---------------------------------------------------------
    // IDictionary:
    // ---------------------------------------------------------


    test ("iDic-Items", fun _ ->
        let b = Dictionary()  :> IDictionary<string, int>
        b.["A"] <- 1
        b.["B"] <- 2
        let items = b.Items |> Seq.toList
        assertThat items (tag "Items returns the key-value pairs" >> isEqualTo [("A", 1); ("B", 2)])
    )


    test ("iDic-Get", fun _ ->
        let b = Dictionary() :> IDictionary<string, int>
        b.["A"] <- 1
        let result = b.GetValue "A"
        assertThat result (tag "Get returns the value of key A" >> isEqualTo 1)
    )


    test ("iDic-Pop", fun _ ->
        let b = Dictionary() :> IDictionary<string, int>
        b.["A"] <- 1
        let popped  = b.Pop "A"
        let result = b.DoesNotContainKey "A" && popped = 1
        assertThat result (tag "Pop removed key A" >> isTrue)
    )


    // ---------------------------------------------------------
    // Dict:
    // ---------------------------------------------------------

    test ("Empty", fun _ ->
        let b = Dict()
        let result = b.IsEmpty
        assertThat result (tag "Empty returns true for empty dictionary" >> isTrue)
    )


    test ("Pop", fun _ ->
        let b = Dict()
        b.["A"] <- 1
        let popped  = b.Pop "A"
        let result = b.DoesNotContainKey "A" && popped = 1
        assertThat result (tag "Pop removed key A" >> isTrue)
    )


    test ("Item", fun _ ->
        let b = Dict()
        b.["A"] <- 1
        let result = b.Item "A"
        assertThat result (tag "Item returns value of key A" >> isEqualTo 1)
    )


    test ("Item fails", fun _ ->
        let b = Dict()
        b.["A"] <- 1
        assertThat (fun () -> b.Get "B" |> ignore ) (tag "Item throws exception when key does not exist" >> throws)
    )

    test ("Add fails", fun _ ->
        let b = Dict<string,int>()
        assertThat (fun () ->b.Set null 1 ) (tag "Add throws exception when key does not exist" >> throws)
    )


    test ("Remove", fun _ ->
        let b = Dict()
        b.["A"] <- 1
        let removed  = b.Remove "A"
        let result = b.DoesNotContainKey "A"
        assertThat (result&&removed) (tag "Remove deletes key A" >> isTrue)
    )

    test ("Remove2", fun _ ->
        let b = Dict()
        // b.["A"] <- 1
        let removed  = b.Remove "A"
        let result = b.DoesNotContainKey "A"
        assertThat (result&& not removed) (tag "Remove missing key A" >> isTrue)
    )


    test ("Clear", fun _ ->
        let b = Dict()
        b.["A"] <- 1
        b.["B"] <- 2
        b.Clear()
        let resultA = b.DoesNotContainKey "A"
        let resultB = b.DoesNotContainKey "B"
        assertThat resultA (tag "Clear removes key A" >> isTrue)
        assertThat resultB (tag "Clear removes key B" >> isTrue)
    )

    test ("ContainsKey", fun _ ->
        let b = Dict()
        b.["A"] <- 1
        let result = b.ContainsKey "A"
        assertThat result (tag "ContainsKey returns true for existing key" >> isTrue)
    )

    test ("Count", fun _ ->
        let b = Dict()
        b.["A"] <- 1
        b.["B"] <- 2
        let result = b.Count
        assertThat result (tag "Count returns the number of key-value pairs" >> isEqualTo 2)
    )

    test ("Keys", fun _ ->
        let b = Dict()
        b.["A"] <- 1
        b.["B"] <- 2
        let keys = b.Keys |> Seq.toList
        assertThat keys (tag "Keys returns the keys" >> isEqualTo ["A"; "B"])
    )


    test ("Values", fun _ ->
        let b = Dict()
        b.["A"] <- 1
        b.["B"] <- 2
        let values = b.Values |> Seq.toList
        assertThat values (tag "Values returns the values" >> isEqualTo [1; 2])
    )


    test ("SetIfKeyAbsent - key does not exist", fun _ ->
        let b = Dict()
        let result = b.SetIfKeyAbsent "A" 1
        assertThat result (tag "SetIfKeyAbsent should return true when key does not exist" >> isTrue)
        assertThat (b.Get "A") (tag "SetIfKeyAbsent should set the value when key does not exist" >> isEqualTo 1)
    )

    test ("SetIfKeyAbsent - key exists", fun _ ->
        let b = Dict()
        b.["A"] <- 1
        let result = b.SetIfKeyAbsent "A" 2
        assertThat result (tag "SetIfKeyAbsent should return false when key exists" >> isFalse)
        assertThat (b.Get "A") (tag "SetIfKeyAbsent should not change the value when key exists" >> isEqualTo 1)
    )

    test ("AddIfKeyAbsent - key does not exist", fun _ ->
        let b = Dict()
        let result = b.AddIfKeyAbsent "A" 1
        assertThat result (tag "AddIfKeyAbsent should return true when key does not exist" >> isTrue)
        assertThat (b.Get "A") (tag "AddIfKeyAbsent should set the value when key does not exist" >> isEqualTo 1)
    )

    test ("AddIfKeyAbsent - key exists", fun _ ->
        let b = Dict()
        b.["A"] <- 1
        let result = b.AddIfKeyAbsent "A" 2
        assertThat result (tag "AddIfKeyAbsent should return false when key exists" >> isFalse)
        assertThat (b.Get "A") (tag "AddIfKeyAbsent should not change the value when key exists" >> isEqualTo 1)
    )

    test ("GetOrSetDefault - key does not exist", fun _ ->
        let b = Dict()
        let result = b.GetOrSetDefault (fun _ -> 1) "A"
        assertThat result (tag "GetOrSetDefault should return the default value when key does not exist" >> isEqualTo 1)
        assertThat (b.Get "A") (tag "GetOrSetDefault should set the default value when key does not exist" >> isEqualTo 1)
    )

    test ("GetOrSetDefault - key exists", fun _ ->
        let b = Dict()
        b.["A"] <- 1
        let result = b.GetOrSetDefault (fun _ -> 2) "A"
        assertThat result (tag "GetOrSetDefault should return the existing value when key exists" >> isEqualTo 1)
        assertThat (b.Get "A") (tag "GetOrSetDefault should not change the value when key exists" >> isEqualTo 1)
    )

    test ("GetOrSetDefaultValue - key does not exist", fun _ ->
        let b = Dict()
        let result = b.GetOrSetDefaultValue 1 "A"
        assertThat result (tag "GetOrSetDefaultValue should return the default value when key does not exist" >> isEqualTo 1)
        assertThat (b.Get "A") (tag "GetOrSetDefaultValue should set the default value when key does not exist" >> isEqualTo 1)
    )

    test ("GetOrSetDefaultValue - key exists", fun _ ->
        let b = Dict()
        b.["A"] <- 1
        let result = b.GetOrSetDefaultValue 2 "A"
        assertThat result (tag "GetOrSetDefaultValue should return the existing value when key exists" >> isEqualTo 1)
        assertThat (b.Get "A") (tag "GetOrSetDefaultValue should not change the value when key exists" >> isEqualTo 1)
    )

    test ("TryGetValue", fun _ ->
        let b = Dict()
        b.["A"] <- 1
        let ok, value = b.TryGetValue "A"
        assertThat ok (tag "TryGetValue should return true when the key exists" >> isTrue)
        assertThat value (tag "TryGetValue should return the value when the key exists" >> isEqualTo 1)
    )


    test ("TryGetValue - key does not exist", fun _ ->
        let b = Dict()
        let ok, value = b.TryGetValue "A"
        assertThat ok (tag "TryGetValue should return false when the key does not exist" >> isFalse)
        assertThat value (tag "TryGetValue should return the default value when the key does not exist" >> isEqualTo 0)
    )

    test ("AsString - empty dictionary", fun _ ->
        let b = Dict<string, int>()
        let result = b.AsString
        assertThat result (tag "AsString should return the correct string for an empty dictionary" >> isEqualTo "empty Dict<String,Int32>")
    )

    test ("AsString - single item", fun _ ->
        let b = Dict<string, int>()
        b.["A"] <- 1
        let result = b.AsString.Replace("\r\n", "\n").Replace("\n", "$")
        assertThat result (tag "AsString should return the correct string for a dictionary with one item" >> isEqualTo "Dict<String,Int32> with 1 item:$  A : 1$")
    )

    test ("AsString - multiple items", fun _ ->
        let b = Dict<string, int>()
        b.["A"] <- 1
        b.["B"] <- 2
        b.["C"] <- 3
        b.["D"] <- 4
        b.["E"] <- 5
        b.["F"] <- 6
        let result = b.AsString.Replace("\r\n", "\n").Replace("\n", "$")
        assertThat result (tag "AsString should return the correct string for a dictionary with multiple items" >> isEqualTo "Dict<String,Int32> with 6 items:$  A : 1$  B : 2$  C : 3$  D : 4$  E : 5$  ...$")
    )


    // test ("iEqualityComparer", fun _ ->
    //     let comparer = StringComparer.OrdinalIgnoreCase // not supported in Fable
    //     let b = Dict<string, int>(comparer)
    //     b.["A"] <- 1
    //     let result = b.ContainsKey "a"
    //     assertThat result (tag "iEqualityComparer should allow case-insensitive key lookup" >> isTrue)
    // )

    test ("iEqualityComparer with object expression", fun _ ->
        let comparer =
            { new IEqualityComparer<string> with
                member _.Equals(x, y) = x.ToLowerInvariant() = y.ToLowerInvariant()
                member _.GetHashCode(obj) = obj.ToLowerInvariant().GetHashCode() }
        let b = Dict<string, int>(comparer)
        b.["A"] <- 1
        let result = b.ContainsKey "a"
        assertThat result (tag "iEqualityComparer should allow case-insensitive key lookup with object expression" >> isTrue)
    )



    test ("KV-Add", fun _ ->
        let b = Dict<string, int>()
        let kvp = KeyValuePair("A", 1)
        let iColl = (b :> ICollection<KeyValuePair<string, int>>)
        iColl.Add(kvp)
        assertThat (b.Get "A") (tag "Add should add the key-value pair to the dictionary" >> isEqualTo 1)
    )

    test ("KV-Clear", fun _ ->
        let b = Dict<string, int>()
        b.Add("A", 1)
        (b :> ICollection<KeyValuePair<string, int>>).Clear()
        assertThat (b.ContainsKey "A") (tag "Clear should remove all key-value pairs from the dictionary" >> isFalse)
    )


    // TODO still fails: https://fable.io/repl/#?code=PYBwpgdgBAygngZwC5gLYDoDCwA2OwDGSAlsBAugOKRgBOxBAUKJFAGICGARvlsLWHQApBAEkIKWqEaN8SKABMoAXigARBiTIdacADzEJAGkNIAfAAoAlLLDyCKxVABcZqKOx5CWiHoDSYHAAahw4AK5gAAocxLQGxlCmZmaMCugA2gCMALpQegC0UAAMMrbyAqjAAG5gnjgWANZVzgHBoRHRsfFIJhJmRooKzhpEpBA6+qa95laqjFALiQBmg3wSMeStUE3oWwBke6vpO625qjsh4WBQSAAWrPOLT08KaQBKaNXXJ4GPz1BgHAIMB-f6LJahYGgxhoYhIEQwJAcFCoSDyaxQABEFS+4kiOA4BGuqiWYQgozIjSqRgIRjAAEcrABvARIMK0aA4mp1Kk0qwAXwA3JjQWCoKU5Nsqo5WpcOjFaBZMgMijZJVywAo-EFHAR0B9KjUqTYQPQJEtoAASTGy9pRBVQADuHAQUA1QygLM+NS1QX5IvV3s1akc7yDSpNZqQFqg1q2CFuwDCOCUXGuAHMyGABlwwvJiK6CBxoGm3UGlBBgI7nJ73Wp-UA&html=DwCwLgtgNgfAsAKAAQqaApgQwCb2ag4CdMTJcMABwFp0BHAVwEsA3AXgCIBhAewDsw6AdQAqAT0roOSAMb9BAzoIAeYAPThoAbhkhMAJwDOJNgzAAzagA4OeQhqy5EhAEY9sYu6mBq3HvD6asEA&css=Q

    test ("KV-Remove", fun _ ->
        let b = Dict<string, int>()
        b.Add("A", 1)
        let result = (b :> ICollection<KeyValuePair<string, int>>).Remove(KeyValuePair("A", 1))
        assertThat result (tag "Remove should return true when the key-value pair is removed" >> isTrue)
        assertThat (b.ContainsKey "A") (tag "Remove should remove the key-value pair from the dictionary" >> isFalse)
    )

    test ("KV-Contains", fun _ ->
        let b = Dict<string, int>()
        b.Add("A", 1)
        let result = (b :> ICollection<KeyValuePair<string, int>>).Contains(KeyValuePair("A", 1))
        assertThat result (tag "Contains should return true when the key-value pair is in the dictionary" >> isTrue)
    )

    test ("KV-CopyTo", fun _ ->
        let b = Dict<string, int>()
        b.Add("A", 1)
        let arr = Array.zeroCreate<KeyValuePair<string, int>> 1
        (b :> ICollection<KeyValuePair<string, int>>).CopyTo(arr, 0)
        assertThat arr.[0] (tag "CopyTo should copy the key-value pairs to the array" >> isEqualTo (KeyValuePair("A", 1)))
    )

    test ("KV-IsReadOnly", fun _ ->
        let b = Dict<string, int>()
        let result = (b :> ICollection<KeyValuePair<string, int>>).IsReadOnly
        assertThat result (tag "IsReadOnly should return false" >> isFalse)
    )

    test ("KV-Count", fun _ ->
        let b = Dict<string, int>()
        b.Add("A", 1)
        let result = (b :> ICollection<KeyValuePair<string, int>>).Count
        assertThat result (tag "Count should return the number of key-value pairs in the dictionary" >> isEqualTo 1)
    )


    test ("iDictionary - Add", fun _ ->
        let b = Dict<string,int>() :> IDictionary<string,int>
        b.Add("A", 1)
        let hasKey = b.ContainsKey("A")
        assertThat hasKey (tag "IDictionary Add should add key-value pair" >> isTrue)
    )

    test ("iDictionary - Remove", fun _ ->
        let b = Dict<string,int>() :> IDictionary<string,int>
        b.Add("A", 1)
        b.Remove("A") |> ignore
        let removed = not (b.ContainsKey("A"))
        assertThat removed (tag "IDictionary Remove should remove key" >> isTrue)
    )

    test ("iDictionary - Clear", fun _ ->
        let b = Dict<string,int>() :> IDictionary<string,int>
        b.Add("A", 1)
        b.Add("B", 2)
        b.Clear()
        let cleared = b.Count = 0
        assertThat cleared (tag "IDictionary Clear should remove all items" >> isTrue)
    )

    test ("iDictionary - Keys", fun _ ->
        let b = Dict<string,int>() :> IDictionary<string,int>
        b.Add("A", 1)
        b.Add("B", 2)
        let keys = b.Keys |> Seq.toList
        assertThat keys (tag "IDictionary Keys should return all keys" >> isEqualTo ["A"; "B"])
    )

    test ("iDictionary - Values", fun _ ->
        let b = Dict<string,int>() :> IDictionary<string,int>
        b.Add("A", 1)
        b.Add("B", 2)
        let values = b.Values |> Seq.toList
        assertThat values (tag "IDictionary Values should return all values" >> isEqualTo [1; 2])
    )

    test ("iDictionary - TryGetValue exists", fun _ ->
        let b = Dict<string,int>() :> IDictionary<string,int>
        b.Add("A", 1)
        let success, value = b.TryGetValue("A")
        assertThat success (tag "TryGetValue should return true for existing key" >> isTrue)
        assertThat value (tag "TryGetValue should return correct value" >> isEqualTo 1)
    )

    test ("iDictionary - TryGetValue missing", fun _ ->
        let b = Dict<string,int>() :> IDictionary<string,int>
        let success, value = b.TryGetValue("missing")
        assertThat success (tag "TryGetValue should return false for missing key" >> isFalse)
        assertThat value (tag "TryGetValue should return default value" >> isEqualTo 0)
    )

    test ("Remove - key does not exist", fun _ ->
        let b = Dict<string,int>()
        let result = b.Remove "NonExistentKey"
        assertThat result (tag "Remove should return false when key does not exist" >> isFalse)
    )

    test ("Remove - key exists", fun _ ->
        let b = Dict<string,int>()
        b.["A"] <- 1
        let result = b.Remove "A"
        assertThat result (tag "Remove should return true when key exists" >> isTrue)
        assertThat (b.ContainsKey "A") (tag "Remove should remove the key" >> isFalse)
    )

    test ("Remove - multiple keys", fun _ ->
        let b = Dict<string,int>()
        b.["A"] <- 1
        b.["B"] <- 2
        b.["C"] <- 3
        let result1 = b.Remove "A"
        let result2 = b.Remove "B"
        assertThat (result1 && result2) (tag "Remove should return true for existing keys" >> isTrue)
        assertThat b.Count (tag "Remove should decrease count accordingly" >> isEqualTo 1)
    )

    test ("Get - key exists", fun _ ->
        let b = Dict<string,int>()
        b.["A"] <- 1
        let result = b["A" ]
        assertThat result (tag "Get should return value for existing key" >> isEqualTo 1)
    )

    test ("Get - key does not exist", fun _ ->
        let b = Dict<string,int>()
        assertThat (fun () -> b.Get "A" |> ignore) (tag "Get should throw for missing key" >> throws)
    )

    test ("Set - add new key", fun _ ->
        let b = Dict<string,int>()
        b.Set "A" 1
        let result = b["A" ]
        assertThat result (tag "Set should add new key-value pair" >> isEqualTo 1)
    )

    test ("Set - update existing key", fun _ ->
        let b = Dict<string,int>()
        b.["A"] <- 1
        b.Set "A" 2
        let result = b.Get "A"
        assertThat result (tag "Set should update value for existing key" >> isEqualTo 2)
    )

    test ("Set - null key", fun _ ->
        let b = Dict<string,int>()
        assertThat (fun () -> b[null ] <- 1) (tag "Set should throw for null key" >> throws)
    )



    // ---------------------------------------------------------
    // ---------------------------------------------------------
    // ---------------------------------------------------------
    // ---------------------------------------------------------
    // ---------------------------------------------------------
    // DefaultDict:
    // DefaultDict:
    // ---------------------------------------------------------
    // ---------------------------------------------------------
    // ---------------------------------------------------------
    // ---------------------------------------------------------
    // ---------------------------------------------------------
    // ---------------------------------------------------------

    test ("DefaultDict", fun _ ->
        let b = DefaultDict(fun _ -> 0)
        let result = b.Get "A"
        assertThat result (tag "DefaultDict should return the default value when the key does not exist" >> isEqualTo 0)
    )

    test ("DefaultDict - key exists", fun _ ->
        let b = DefaultDict(fun _ -> 0)
        b.["A"] <- 1
        let result = b.Get "A"
        assertThat result (tag "DefaultDict should return the existing value when the key exists" >> isEqualTo 1)
    )

    test ("DefaultDict - default value incr", fun _ ->
        let b = DefaultDict(fun _ -> ref 0)
        incr (b.["A"])
        assertThat b.["A"].Value (tag "DefaultDict should return the default value when the key does not exist" >> isEqualTo 1)
    )

    test ("DefaultDict - key exists - default value", fun _ ->
        let b = DefaultDict(fun _ -> ref 0)
        b.["A"] <- ref 1
        incr (b.Get "A")
        assertThat b.["A"].Value (tag "DefaultDict should return the existing value when the key exists" >> isEqualTo 2)
    )

    test ("DefaultDict - default value - ref", fun _ ->
        let b = DefaultDict(fun _ -> ref 0)
        incr (b.Get "A")
        assertThat b.["A"].Value (tag "DefaultDict should return the default value when the key does not exist" >> isEqualTo 1)
    )

    test ("DefaultDict - Pop", fun _ ->
        let b = DefaultDict(fun _ -> 0)
        b.["A"] <- 1
        let popped  = b.Pop "A"
        let result = b.DoesNotContainKey "A" && popped = 1
        assertThat result (tag "Pop removed key A" >> isTrue)
    )


    test ("DefaultDict - default value", fun _ ->
            let b = DefaultDict(fun _ -> 0)
            let result = b.Get "A"
            assertThat result (tag "DefaultDict should return the default value when the key does not exist" >> isEqualTo 0)
    )

    test ("DefaultDict - set value", fun _ ->
            let b = DefaultDict(fun _ -> 0)
            b.["A"] <- 1
            let result = b.Get "A"
            assertThat result (tag "DefaultDict should allow setting a value for a key" >> isEqualTo 1)
    )

    test ("DefaultDict - remove key", fun _ ->
            let b = DefaultDict(fun _ -> 0)
            b.["A"] <- 1
            b.Remove "A" |> ignore
            let result = b.ContainsKey "A"
            assertThat result (tag "DefaultDict should remove the key" >> isFalse)
    )

    test ("DefaultDict - clear", fun _ ->
            let b = DefaultDict(fun _ -> 0)
            b.["A"] <- 1
            b.["B"] <- 2
            b.Clear()
            let resultA = b.ContainsKey "A"
            let resultB = b.ContainsKey "B"
            assertThat resultA (tag "DefaultDict should clear all keys" >> isFalse)
            assertThat resultB (tag "DefaultDict should clear all keys" >> isFalse)
    )

    test ("DefaultDict - contains key", fun _ ->
            let b = DefaultDict(fun _ -> 0)
            b.["A"] <- 1
            let result = b.ContainsKey "A"
            assertThat result (tag "DefaultDict should return true for existing key" >> isTrue)
    )

    test ("DefaultDict - count", fun _ ->
            let b = DefaultDict(fun _ -> 0)
            b.["A"] <- 1
            b.["B"] <- 2
            let result = b.Count
            assertThat result (tag "DefaultDict should return the number of key-value pairs" >> isEqualTo 2)
    )

    test ("DefaultDict - keys", fun _ ->
            let b = DefaultDict(fun _ -> 0)
            b.["A"] <- 1
            b.["B"] <- 2
            let keys = b.Keys |> Seq.toList
            assertThat keys (tag "DefaultDict should return the keys" >> isEqualTo ["A"; "B"])
    )

    test ("DefaultDict - values", fun _ ->
            let b = DefaultDict(fun _ -> 0)
            b.["A"] <- 1
            b.["B"] <- 2
            let values = b.Values |> Seq.toList
            assertThat values (tag "DefaultDict should return the values" >> isEqualTo [1; 2])
    )

    test ("DefaultDict - pop key", fun _ ->
            let b = DefaultDict(fun _ -> 0)
            b.["A"] <- 1
            let value = b.Pop "A"
            assertThat value (tag "DefaultDict should return the value when popping a key" >> isEqualTo 1)
            let result = b.ContainsKey "A"
            assertThat result (tag "DefaultDict should remove the key when popping" >> isFalse)
    )

    test ("DefaultDict - AsString", fun _ ->
            let b = DefaultDict(fun _ -> 0)
            b.["A"] <- 1
            b.["B"] <- 2
            let result = b.AsString.Replace("\r\n", "\n").Replace("\n", "$")
            assertThat result (tag "DefaultDict should return the correct string representation with items" >> isEqualTo "DefaultDict<String,Int32> with 2 items:$  A : 1$  B : 2$")
    )

    test ("DefaultDict - KV-Add", fun _ ->
        let b = DefaultDict(fun _ -> 0)
        let kvp = KeyValuePair("A", 1)
        let iColl = (b :> ICollection<KeyValuePair<string, int>>)
        iColl.Add(kvp)
        assertThat (b.Get "A") (tag "Add should add the key-value pair to the dictionary" >> isEqualTo 1)
    )

    test ("DefaultDict - KV-Clear", fun _ ->
        let b = DefaultDict(fun _ -> 0)
        b.Add("A", 1)
        (b :> ICollection<KeyValuePair<string, int>>).Clear()
        assertThat (b.ContainsKey "A") (tag "Clear should remove all key-value pairs" >> isFalse)
    )

    test ("DefaultDict - KV-Remove", fun _ ->
        let b = DefaultDict(fun _ -> 0)
        b.Add("A", 1)
        let result = (b :> ICollection<KeyValuePair<string, int>>).Remove(KeyValuePair("A", 1))
        assertThat result (tag "Remove should return true when pair removed" >> isTrue)
        assertThat (b.ContainsKey "A") (tag "Remove should remove the key-value pair" >> isFalse)
    )

    test ("DefaultDict - KV-Contains", fun _ ->
        let b = DefaultDict(fun _ -> 0)
        b.Add("A", 1)
        let result = (b :> ICollection<KeyValuePair<string, int>>).Contains(KeyValuePair("A", 1))
        assertThat result (tag "Contains should return true for existing pair" >> isTrue)
    )

    test ("DefaultDict - KV-CopyTo", fun _ ->
        let b = DefaultDict(fun _ -> 0)
        b.Add("A", 1)
        let arr = Array.zeroCreate<KeyValuePair<string, int>> 1
        (b :> ICollection<KeyValuePair<string, int>>).CopyTo(arr, 0)
        assertThat arr.[0] (tag "CopyTo should copy pairs to array" >> isEqualTo (KeyValuePair("A", 1)))
    )

    test ("DefaultDict - KV-IsReadOnly", fun _ ->
        let b = DefaultDict(fun _ -> 0)
        let result = (b :> ICollection<KeyValuePair<string, int>>).IsReadOnly
        assertThat result (tag "IsReadOnly should return false" >> isFalse)
    )

    test ("DefaultDict - KV-Count", fun _ ->
        let b = DefaultDict(fun _ -> 0)
        b.Add("A", 1)
        let result = (b :> ICollection<KeyValuePair<string, int>>).Count
        assertThat result (tag "Count should return number of pairs" >> isEqualTo 1)
    )

    test ("Dict ReadOnly collection", fun _ ->
        let b = Dict<string,int>()
        b.["A"] <- 1
        b.["B"] <- 2
        let ro = b//.AsReadOnly() is not supported by Fable
        let result = ro :> IReadOnlyCollection<KeyValuePair<string,int>>
        assertThat result.Count (tag "ReadOnly collection count should match original" >> isEqualTo 2)
    )

    test ("Dict ReadOnly collection content", fun _ ->
        let b = Dict<string,int>()
        b.["A"] <- 1
        b.["B"] <- 2
        let ro = b//.AsReadOnly() is not supported by Fable
        let pairs = ro |> Seq.map (fun kvp -> kvp.Key, kvp.Value) |> Set.ofSeq
        let expected = set [("A",1); ("B",2)]
        assertThat pairs (tag "ReadOnly collection should have same content" >> isEqualTo expected)
    )

    test ("DefaultDict ReadOnly collection", fun _ ->
        let b = DefaultDict(fun _ -> 8)
        b.["A"] <- 1
        b.["B"] <- 2
        let result = b :> IReadOnlyCollection<KeyValuePair<string,int>>
        assertThat result.Count (tag "ReadOnly collection count should match original" >> isEqualTo 2)
    )

    test ("DefaultDict ReadOnly collection content", fun _ ->
        let b = DefaultDict(fun _ -> 8)
        b.["A"] <- 1
        b.["B"] <- 2
        let ro = b
        let pairs = ro |> Seq.map (fun kvp -> kvp.Key, kvp.Value) |> Set.ofSeq
        let expected = set [("A",1); ("B",2)]
        assertThat pairs (tag "ReadOnly collection should have same content" >> isEqualTo expected)
    )

    // IDictionary interface is removed from DefaultDict becaus the semantics don't fit
    // don't add IDictionary because of TryGetValue might return might no Value while get would.
    // this is not consistent with the IDictionary interface

    // test ("iDictionary DefDict - Get item", fun _ ->
    //     let b = DefaultDict(fun _ -> 0) :> IDictionary<string,int>
    //     let d = b.["A"]
    //     let hasKey = b.ContainsKey("A")
    //     assertThat (hasKey && d=0) (tag "IDictionary Get should add key-value pair" >> isTrue)
    // )

    // test ("iDictionary DefDict - Get method", fun _ ->
    //     let b = DefaultDict(fun _ -> 0) :> IDictionary<string,int>
    //     let d = b.Get "A"
    //     let hasKey = b.ContainsKey("A")
    //     assertThat (hasKey && d=0) (tag "IDictionary Get should add key-value pair" >> isTrue)
    // )


    // test ("iDictionary DefDict - Add", fun _ ->
    //     let b = DefaultDict(fun _ -> 0) :> IDictionary<string,int>
    //     b.Add("A", 1)
    //     let hasKey = b.ContainsKey("A")
    //     assertThat hasKey (tag "IDictionary Add should add key-value pair" >> isTrue)
    // )

    // test ("iDictionary DefDict - Remove", fun _ ->
    //     let b = DefaultDict(fun _ -> 0) :> IDictionary<string,int>
    //     b.Add("A", 1)
    //     b.Remove("A") |> ignore
    //     let removed = not (b.ContainsKey("A"))
    //     assertThat removed (tag "IDictionary Remove should remove key" >> isTrue)
    // )

    // test ("iDictionary DefDict - Clear", fun _ ->
    //     let b = DefaultDict(fun _ -> 0) :> IDictionary<string,int>
    //     b.Add("A", 1)
    //     b.Add("B", 2)
    //     b.Clear()
    //     let cleared = b.Count = 0
    //     assertThat cleared (tag "IDictionary Clear should remove all items" >> isTrue)
    // )

    // test ("iDictionary DefDict - TryGetValue exists for add", fun _ ->
    //     let b = DefaultDict(fun _ -> 3) :> IDictionary<string,int>
    //     b.Add("A", 8)
    //     let success, value = b.TryGetValue("A")
    //     assertThat success (tag "TryGetValue should return true for existing key" >> isTrue)
    //     assertThat value (tag "TryGetValue should return correct value" >> isEqualTo 8)
    // )

    // test ("iDictionary DefDict - TryGetValue exists for <- ", fun _ ->
    //     let dd = DefaultDict(fun _ -> 3)
    //     dd["A"] <- 8
    //     let b = dd :> IDictionary<string,int>
    //     let success, value = b.TryGetValue("A")
    //     assertThat success (tag "TryGetValue should return true for existing key" >> isTrue)
    //     assertThat value (tag "TryGetValue should return correct value" >> isEqualTo 8)
    // )


    // test ("iDictionary DefDict - TryGetValue missing", fun _ ->
    //     let b = DefaultDict(fun _ -> 0) :> IDictionary<string,int>
    //     let success, _ = b.TryGetValue("missing")
    //     assertThat success (tag "TryGetValue should return true even for missing key" >> isFalse)
    // )





    // ---------------------------------------------------------
    // Dict Module Tests:
    // ---------------------------------------------------------

    test ("Dict.memoize - caches function results", fun _ ->
        let callCount = ref 0
        let expensiveFunc x =
            incr callCount
            x * 2

        let memoizedFunc = Dict.memoize expensiveFunc

        // First call should execute the function
        let result1 = memoizedFunc 5
        assertThat result1 (tag "Function should return correct result" >> isEqualTo 10)
        assertThat !callCount (tag "Function should be called once" >> isEqualTo 1)

        // Second call with same input should use cached result
        let result2 = memoizedFunc 5
        assertThat result2 (tag "Function should return correct result" >> isEqualTo 10)
        assertThat !callCount (tag "Function should not be called again" >> isEqualTo 1)

        // Call with different input should execute function again
        let result3 = memoizedFunc 7
        assertThat result3 (tag "Function should return correct result" >> isEqualTo 14)
        assertThat !callCount (tag "Function should be called for new input" >> isEqualTo 2)
    )


    test ("Dict.memoize - null", fun _ ->
        let unitf () = 5
        let memoizedFunc = Dict.memoize unitf
        let result = memoizedFunc ()
        assertThat result (tag "Function should return correct result" >> isEqualTo 5)

        let nullF (_x:obj) = 6
        let memoizedFunc = Dict.memoize nullF
        let result = memoizedFunc null
        assertThat result (tag "Function should return correct result" >> isEqualTo 6)

        let noneF (_x:Option<int>) = 7
        let memoizedFunc = Dict.memoize noneF
        let result = memoizedFunc None
        assertThat result (tag "Function should return correct result" >> isEqualTo 7)
    )


    test ("Dict.get - retrieves value for existing key", fun _ ->
        let dict = Dict<string, int>()
        dict.["test"] <- 42

        let result = Dict.get "test" dict
        assertThat result (tag "Should retrieve correct value" >> isEqualTo 42)
    )

    test ("Dict.get - throws for non-existent key", fun _ ->
        let dict = Dict<string, int>()
        assertThat (fun () -> Dict.get "missing" dict |> ignore) (tag "Should throw KeyNotFoundException" >> throws)
    )

    test ("Dict.set - sets value for key", fun _ ->
        let dict = Dict<string, int>()
        Dict.set "test" 42 dict

        assertThat dict.["test"] (tag "Should set correct value" >> isEqualTo 42)
    )

    test ("Dict.tryGet - returns Some for existing key", fun _ ->
        let dict = Dict<string, int>()
        dict.["test"] <- 42

        let result = Dict.tryGet "test" dict
        assertThat result (tag "Should return Some with correct value" >> isEqualTo (Some 42))
    )

    test ("Dict.tryGet - returns None for non-existent key", fun _ ->
        let dict = Dict<string, int>()

        let result = Dict.tryGet "missing" dict
        assertThat result (tag "Should return None" >> isEqualTo None)
    )

    test ("Dict.create - creates dictionary from key-value pairs", fun _ ->
        let pairs = [("a", 1); ("b", 2); ("c", 3)]
        let dict = Dict.create pairs

        assertThat dict.Count (tag "Dictionary should have correct count" >> isEqualTo 3)
        assertThat dict.["a"] (tag "Dictionary should contain correct values" >> isEqualTo 1)
        assertThat dict.["b"] (tag "Dictionary should contain correct values" >> isEqualTo 2)
        assertThat dict.["c"] (tag "Dictionary should contain correct values" >> isEqualTo 3)
    )

    test ("Dict.setIfKeyAbsent - adds when key doesn't exist", fun _ ->
        let dict = Dict<string, int>()

        let result = Dict.setIfKeyAbsent "test" 42 dict
        assertThat result (tag "Should return true when key doesn't exist" >> isTrue)
        assertThat dict.["test"] (tag "Should set the value" >> isEqualTo 42)
    )

    test ("Dict.setIfKeyAbsent - doesn't add when key exists", fun _ ->
        let dict = Dict<string, int>()
        dict.["test"] <- 42

        let result = Dict.setIfKeyAbsent "test" 84 dict
        assertThat result (tag "Should return false when key exists" >> isFalse)
        assertThat dict.["test"] (tag "Should not change the value" >> isEqualTo 42)
    )

    test ("Dict.addIfKeyAbsent - adds when key doesn't exist", fun _ ->
        let dict = Dict<string, int>()

        let result = Dict.addIfKeyAbsent "test" 42 dict
        assertThat result (tag "Should return true when key doesn't exist" >> isTrue)
        assertThat dict.["test"] (tag "Should add the value" >> isEqualTo 42)
    )

    test ("Dict.addIfKeyAbsent - doesn't add when key exists", fun _ ->
        let dict = Dict<string, int>()
        dict.["test"] <- 42

        let result = Dict.addIfKeyAbsent "test" 84 dict
        assertThat result (tag "Should return false when key exists" >> isFalse)
        assertThat dict.["test"] (tag "Should not change the value" >> isEqualTo 42)
    )

    test ("Dict.getOrSetDefault - returns existing value for existing key", fun _ ->
        let dict = Dict<string, int>()
        dict.["test"] <- 42

        let result = Dict.getOrSetDefault (fun _ -> 99) "test" dict
        assertThat result (tag "Should return existing value" >> isEqualTo 42)
        assertThat dict.["test"] (tag "Should not change existing value" >> isEqualTo 42)
    )

    test ("Dict.getOrSetDefault - sets default for missing key", fun _ ->
        let dict = Dict<string, int>()

        let result = Dict.getOrSetDefault (fun _k -> 4) "test" dict
        assertThat result (tag "Should return default value based on key" >> isEqualTo 4)
        assertThat dict.["test"] (tag "Should set default value in dictionary" >> isEqualTo 4)
    )

    test ("Dict.getOrSetDefaultValue - returns existing value for existing key", fun _ ->
        let dict = Dict<string, int>()
        dict.["test"] <- 42

        let result = Dict.getOrSetDefaultValue 99 "test" dict
        assertThat result (tag "Should return existing value" >> isEqualTo 42)
        assertThat dict.["test"] (tag "Should not change existing value" >> isEqualTo 42)
    )

    test ("Dict.getOrSetDefaultValue - sets default for missing key", fun _ ->
        let dict = Dict<string, int>()

        let result = Dict.getOrSetDefaultValue 99 "test" dict
        assertThat result (tag "Should return default value" >> isEqualTo 99)
        assertThat dict.["test"] (tag "Should set default value in dictionary" >> isEqualTo 99)
    )

    test ("Dict.tryPop - returns Some and removes for existing key", fun _ ->
        let dict = Dict<string, int>()
        dict.["test"] <- 42

        let result = Dict.tryPop "test" dict
        assertThat result (tag "Should return Some with value" >> isEqualTo (Some 42))
        assertThat (dict.ContainsKey "test") (tag "Should remove key from dictionary" >> isFalse)
    )

    test ("Dict.tryPop - returns None for non-existent key", fun _ ->
        let dict = Dict<string, int>()

        let result = Dict.tryPop "missing" dict
        assertThat result (tag "Should return None for missing key" >> isEqualTo None)
    )

    test ("Dict.pop - returns value and removes for existing key", fun _ ->
        let dict = Dict<string, int>()
        dict.["test"] <- 42

        let result = Dict.pop "test" dict
        assertThat result (tag "Should return value" >> isEqualTo 42)
        assertThat (dict.ContainsKey "test") (tag "Should remove key from dictionary" >> isFalse)
    )

    test ("Dict.pop - throws for non-existent key", fun _ ->
        let dict = Dict<string, int>()

        assertThat (fun () -> Dict.pop "missing" dict |> ignore) (tag "Should throw KeyNotFoundException" >> throws)
    )

    test ("Dict.items - returns sequence of key-value pairs", fun _ ->
        let dict = Dict<string, int>()
        dict.["a"] <- 1
        dict.["b"] <- 2

        let items = Dict.items dict |> Seq.toList |> List.sortBy fst
        assertThat items (tag "Should return correct key-value pairs" >> isEqualTo [("a", 1); ("b", 2)])
    )

    test ("Dict.values - returns sequence of values", fun _ ->
        let dict = Dict<string, int>()
        dict.["a"] <- 1
        dict.["b"] <- 2

        let values = Dict.values dict |> Seq.toList |> List.sort
        assertThat values (tag "Should return correct values" >> isEqualTo [1; 2])
    )

    test ("Dict.keys - returns sequence of keys", fun _ ->
        let dict = Dict<string, int>()
        dict.["a"] <- 1
        dict.["b"] <- 2

        let keys = Dict.keys dict |> Seq.toList |> List.sort
        assertThat keys (tag "Should return correct keys" >> isEqualTo ["a"; "b"])
    )

    test ("Dict.iter - iterates over dictionary", fun _ ->
        let dict = Dict<string, int>()
        dict.["a"] <- 1
        dict.["b"] <- 2

        let result = ref []
        Dict.iter (fun k v -> result := (k, v) :: !result) dict
        let sorted = !result |> List.sortBy fst

        assertThat sorted (tag "Should iterate over all key-value pairs" >> isEqualTo [("a", 1); ("b", 2)])
    )

    test ("Dict.map - maps dictionary to sequence", fun _ ->
        let dict = Dict<string, int>()
        dict.["a"] <- 1
        dict.["b"] <- 2

        let result = Dict.map (fun k v -> k + string v) dict |> Seq.toList |> List.sort
        assertThat result (tag "Should map key-value pairs correctly" >> isEqualTo ["a1"; "b2"])
    )


    // =============================================================
    // Fable/JS parity tests:
    // Tests for functions with FABLE_COMPILER_JAVASCRIPT directives
    // to ensure .NET and JS runtimes behave the same way.
    // =============================================================

    // ---------------------------------------------------------
    // Dict - Core operations (routed through IJSMap in Fable)
    // ---------------------------------------------------------

    test ("Dict-Fable - Get on empty dict throws", fun _ ->
        let d = Dict<string, int>()
        assertThat (fun () -> d.Get "anything" |> ignore) (tag "Get on empty dict should throw" >> throws)
    )

    test ("Dict-Fable - Set and Get with int keys (value type)", fun _ ->
        let d = Dict<int, string>()
        d.Set 1 "one"
        d.Set 2 "two"
        d.Set 3 "three"
        assertThat (d.Get 1) (tag "Int key 1" >> isEqualTo "one")
        assertThat (d.Get 2) (tag "Int key 2" >> isEqualTo "two")
        assertThat (d.Get 3) (tag "Int key 3" >> isEqualTo "three")
    )

    test ("Dict-Fable - Set and Get with float keys (value type)", fun _ ->
        let d = Dict<float, string>()
        d.Set 1.5 "a"
        d.Set 2.5 "b"
        assertThat (d.Get 1.5) (tag "Float key 1.5" >> isEqualTo "a")
        assertThat (d.Get 2.5) (tag "Float key 2.5" >> isEqualTo "b")
    )

    test ("Dict-Fable - Set and Get with bool keys (value type)", fun _ ->
        let d = Dict<bool, string>()
        d.Set true "yes"
        d.Set false "no"
        assertThat (d.Get true) (tag "Bool key true" >> isEqualTo "yes")
        assertThat (d.Get false) (tag "Bool key false" >> isEqualTo "no")
        assertThat d.Count (tag "Should have 2 entries" >> isEqualTo 2)
    )

    test ("Dict-Fable - Set and Get with tuple keys", fun _ ->
        let d = Dict<(int * string), float>()
        d.Set (1, "a") 10.0
        d.Set (2, "b") 20.0
        assertThat (d.Get (1, "a")) (tag "Tuple key (1,a)" >> isEqualTo 10.0)
        assertThat (d.Get (2, "b")) (tag "Tuple key (2,b)" >> isEqualTo 20.0)
    )

    test ("Dict-Fable - Set overwrites existing value", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        d.Set "A" 2
        assertThat (d.Get "A") (tag "Set should overwrite" >> isEqualTo 2)
        assertThat d.Count (tag "Count should still be 1" >> isEqualTo 1)
    )

    test ("Dict-Fable - Item indexer set overwrites", fun _ ->
        let d = Dict<string, int>()
        d.["X"] <- 10
        d.["X"] <- 20
        assertThat d.["X"] (tag "Item indexer should overwrite" >> isEqualTo 20)
        assertThat d.Count (tag "Count should still be 1" >> isEqualTo 1)
    )

    test ("Dict-Fable - null key throws on Set", fun _ ->
        let d = Dict<string, int>()
        assertThat (fun () -> d.Set null 1) (tag "Set null key should throw" >> throws)
    )

    test ("Dict-Fable - null key throws on Get", fun _ ->
        let d = Dict<string, int>()
        assertThat (fun () -> d.Get null |> ignore) (tag "Get null key should throw" >> throws)
    )

    test ("Dict-Fable - null key throws on Item get", fun _ ->
        let d = Dict<string, int>()
        assertThat (fun () -> d.[null] |> ignore) (tag "Item get null key should throw" >> throws)
    )

    test ("Dict-Fable - null key throws on Item set", fun _ ->
        let d = Dict<string, int>()
        assertThat (fun () -> d.[null] <- 1) (tag "Item set null key should throw" >> throws)
    )

    test ("Dict-Fable - null key throws on Pop", fun _ ->
        let d = Dict<string, int>()
        assertThat (fun () -> d.Pop null |> ignore) (tag "Pop null key should throw" >> throws)
    )

    test ("Dict-Fable - null key throws on TryPop", fun _ ->
        let d = Dict<string, int>()
        assertThat (fun () -> d.TryPop null |> ignore) (tag "TryPop null key should throw" >> throws)
    )

    test ("Dict-Fable - null key throws on SetIfKeyAbsent", fun _ ->
        let d = Dict<string, int>()
        assertThat (fun () -> d.SetIfKeyAbsent null 1 |> ignore) (tag "SetIfKeyAbsent null key should throw" >> throws)
    )

    test ("Dict-Fable - null key throws on AddIfKeyAbsent", fun _ ->
        let d = Dict<string, int>()
        assertThat (fun () -> d.AddIfKeyAbsent null 1 |> ignore) (tag "AddIfKeyAbsent null key should throw" >> throws)
    )

    test ("Dict-Fable - null key throws on GetOrSetDefault", fun _ ->
        let d = Dict<string, int>()
        assertThat (fun () -> d.GetOrSetDefault (fun _ -> 0) null |> ignore) (tag "GetOrSetDefault null key should throw" >> throws)
    )

    test ("Dict-Fable - null key throws on GetOrSetDefaultValue", fun _ ->
        let d = Dict<string, int>()
        assertThat (fun () -> d.GetOrSetDefaultValue 0 null |> ignore) (tag "GetOrSetDefaultValue null key should throw" >> throws)
    )

    test ("Dict-Fable - ContainsKey on empty", fun _ ->
        let d = Dict<string, int>()
        assertThat (d.ContainsKey "A") (tag "Empty dict should not contain any key" >> isFalse)
    )

    test ("Dict-Fable - ContainsKey after Set", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        assertThat (d.ContainsKey "A") (tag "Should contain key after Set" >> isTrue)
        assertThat (d.ContainsKey "B") (tag "Should not contain absent key" >> isFalse)
    )

    test ("Dict-Fable - DoesNotContainKey", fun _ ->
        let d = Dict<string, int>()
        assertThat (d.DoesNotContainKey "A") (tag "Empty dict DoesNotContainKey" >> isTrue)
        d.Set "A" 1
        assertThat (d.DoesNotContainKey "A") (tag "Should not say DoesNotContainKey for present key" >> isFalse)
    )

    test ("Dict-Fable - Remove returns false for missing key", fun _ ->
        let d = Dict<string, int>()
        assertThat (d.Remove "missing") (tag "Remove missing key returns false" >> isFalse)
    )

    test ("Dict-Fable - Remove returns true and removes key", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        let removed = d.Remove "A"
        assertThat removed (tag "Remove existing key returns true" >> isTrue)
        assertThat (d.ContainsKey "A") (tag "Key should be gone after Remove" >> isFalse)
        assertThat d.Count (tag "Count should be 0 after removing only key" >> isEqualTo 0)
    )

    test ("Dict-Fable - Pop existing key", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 42
        let v = d.Pop "A"
        assertThat v (tag "Pop should return value" >> isEqualTo 42)
        assertThat (d.ContainsKey "A") (tag "Pop should remove key" >> isFalse)
    )

    test ("Dict-Fable - Pop missing key throws", fun _ ->
        let d = Dict<string, int>()
        assertThat (fun () -> d.Pop "missing" |> ignore) (tag "Pop missing key should throw" >> throws)
    )

    test ("Dict-Fable - TryPop existing key", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 42
        let v = d.TryPop "A"
        assertThat v (tag "TryPop existing key returns Some" >> isEqualTo (Some 42))
        assertThat (d.ContainsKey "A") (tag "TryPop should remove key" >> isFalse)
    )

    test ("Dict-Fable - TryPop missing key returns None", fun _ ->
        let d = Dict<string, int>()
        let v = d.TryPop "missing"
        assertThat v (tag "TryPop missing key returns None" >> isEqualTo None)
    )

    test ("Dict-Fable - Clear on empty dict", fun _ ->
        let d = Dict<string, int>()
        d.Clear()
        assertThat d.Count (tag "Clear on empty dict should work" >> isEqualTo 0)
    )

    test ("Dict-Fable - Clear removes all entries", fun _ ->
        let d = Dict<string, int>()
        for i in 1..100 do
            d.Set (string i) i
        assertThat d.Count (tag "Should have 100 entries" >> isEqualTo 100)
        d.Clear()
        assertThat d.Count (tag "Clear should remove all entries" >> isEqualTo 0)
        assertThat d.IsEmpty (tag "Should be empty after Clear" >> isTrue)
    )

    test ("Dict-Fable - IsEmpty and IsNotEmpty", fun _ ->
        let d = Dict<string, int>()
        assertThat d.IsEmpty (tag "New dict is empty" >> isTrue)
        assertThat d.IsNotEmpty (tag "New dict is not not-empty" >> isFalse)
        d.Set "A" 1
        assertThat d.IsEmpty (tag "Dict with entry is not empty" >> isFalse)
        assertThat d.IsNotEmpty (tag "Dict with entry is not-empty" >> isTrue)
    )

    test ("Dict-Fable - Count tracks additions and removals", fun _ ->
        let d = Dict<string, int>()
        assertThat d.Count (tag "Initial count is 0" >> isEqualTo 0)
        d.Set "A" 1
        assertThat d.Count (tag "Count after 1 add" >> isEqualTo 1)
        d.Set "B" 2
        assertThat d.Count (tag "Count after 2 adds" >> isEqualTo 2)
        d.Set "A" 99  // overwrite, no new key
        assertThat d.Count (tag "Count after overwrite stays same" >> isEqualTo 2)
        d.Remove "A" |> ignore
        assertThat d.Count (tag "Count after remove" >> isEqualTo 1)
    )

    test ("Dict-Fable - Keys returns all keys", fun _ ->
        let d = Dict<string, int>()
        d.Set "X" 1
        d.Set "Y" 2
        d.Set "Z" 3
        let keys = d.Keys |> Seq.toList |> List.sort
        assertThat keys (tag "Keys should return all keys" >> isEqualTo ["X"; "Y"; "Z"])
    )

    test ("Dict-Fable - Values returns all values", fun _ ->
        let d = Dict<string, int>()
        d.Set "X" 10
        d.Set "Y" 20
        let values = d.Values |> Seq.toList |> List.sort
        assertThat values (tag "Values should return all values" >> isEqualTo [10; 20])
    )

    test ("Dict-Fable - Items returns tuples", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        d.Set "B" 2
        let items = d.Items |> Seq.toList |> List.sortBy fst
        assertThat items (tag "Items should return key-value tuples" >> isEqualTo [("A", 1); ("B", 2)])
    )

    test ("Dict-Fable - TryGetValue existing", fun _ ->
        let d = Dict<string, int>()
        d.Set "K" 99
        let ok, v = d.TryGetValue "K"
        assertThat ok (tag "TryGetValue should return true for existing key" >> isTrue)
        assertThat v (tag "TryGetValue should return correct value" >> isEqualTo 99)
    )

    test ("Dict-Fable - TryGetValue missing", fun _ ->
        let d = Dict<string, int>()
        let ok, v = d.TryGetValue "missing"
        assertThat ok (tag "TryGetValue should return false for missing key" >> isFalse)
        assertThat v (tag "TryGetValue should return default int value" >> isEqualTo 0)
    )

    test ("Dict-Fable - TryGetValue missing string value", fun _ ->
        let d = Dict<int, string>()
        let ok, v = d.TryGetValue 42
        assertThat ok (tag "TryGetValue should return false for missing key" >> isFalse)
        assertThat v (tag "TryGetValue should return null for missing string value" >> isEqualTo null)
    )

    test ("Dict-Fable - null string value is allowed", fun _ ->
        let d = Dict<string, string>()
        d.Set "key" null
        assertThat (d.Get "key") (tag "Null string value should be stored and retrieved" >> isEqualTo null)
        assertThat (d.ContainsKey "key") (tag "Key with null value should exist" >> isTrue)
    )

    test ("Dict-Fable - empty string key", fun _ ->
        let d = Dict<string, int>()
        d.Set "" 42
        assertThat (d.Get "") (tag "Empty string key should work" >> isEqualTo 42)
        assertThat (d.ContainsKey "") (tag "Empty string key should be found" >> isTrue)
    )

    test ("Dict-Fable - many entries", fun _ ->
        let d = Dict<int, int>()
        for i in 0..999 do
            d.Set i (i * i)
        assertThat d.Count (tag "Should have 1000 entries" >> isEqualTo 1000)
        assertThat (d.Get 0) (tag "First entry" >> isEqualTo 0)
        assertThat (d.Get 999) (tag "Last entry" >> isEqualTo (999 * 999))
        assertThat (d.Get 500) (tag "Middle entry" >> isEqualTo (500 * 500))
    )

    test ("Dict-Fable - duplicate Add via indexer", fun _ ->
        let d = Dict<string, int>()
        d.["A"] <- 1
        d.["A"] <- 2
        assertThat d.["A"] (tag "Second set should win" >> isEqualTo 2)
        assertThat d.Count (tag "No duplicate keys" >> isEqualTo 1)
    )

    test ("Dict-Fable - Add method on duplicate key throws", fun _ ->
        let d = Dict<string, int>()
        d.Add("A", 1)
        // Add via IDictionary should throw on duplicate
        let iDict = d :> IDictionary<string, int>
        assertThat (fun () -> iDict.Add("A", 2)) (tag "IDictionary.Add should throw on duplicate key" >> throws)
    )

    test ("Dict-Fable - enumerate empty dict", fun _ ->
        let d = Dict<string, int>()
        let items = d |> Seq.toList
        assertThat items (tag "Enumerating empty dict should yield empty list" >> isEqualTo [])
    )

    test ("Dict-Fable - enumerate dict", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        d.Set "B" 2
        let items = d |> Seq.map (fun kvp -> kvp.Key, kvp.Value) |> Seq.toList |> List.sortBy fst
        assertThat items (tag "Enumerating dict should yield all entries" >> isEqualTo [("A", 1); ("B", 2)])
    )

    test ("Dict-Fable - ContainsValue", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 42
        assertThat (d.ContainsValue 42) (tag "ContainsValue should find existing value" >> isTrue)
        assertThat (d.ContainsValue 99) (tag "ContainsValue should not find missing value" >> isFalse)
    )

    test ("Dict-Fable - KeysSeq and ValuesSeq", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        d.Set "B" 2
        let ks = d.KeysSeq |> Seq.toList |> List.sort
        let vs = d.ValuesSeq |> Seq.toList |> List.sort
        assertThat ks (tag "KeysSeq should return all keys" >> isEqualTo ["A"; "B"])
        assertThat vs (tag "ValuesSeq should return all values" >> isEqualTo [1; 2])
    )

    test ("Dict-Fable - SetIfKeyAbsent on empty", fun _ ->
        let d = Dict<string, int>()
        let r = d.SetIfKeyAbsent "A" 1
        assertThat r (tag "Should return true on empty dict" >> isTrue)
        assertThat (d.Get "A") (tag "Value should be set" >> isEqualTo 1)
    )

    test ("Dict-Fable - SetIfKeyAbsent does not overwrite", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        let r = d.SetIfKeyAbsent "A" 2
        assertThat r (tag "Should return false when key exists" >> isFalse)
        assertThat (d.Get "A") (tag "Original value should be preserved" >> isEqualTo 1)
    )

    test ("Dict-Fable - GetOrSetDefault creates and returns default", fun _ ->
        let d = Dict<string, int>()
        let v = d.GetOrSetDefault (fun k -> k.Length) "hello"
        assertThat v (tag "Default function should use the key" >> isEqualTo 5)
        assertThat (d.Get "hello") (tag "Value should be stored" >> isEqualTo 5)
    )

    test ("Dict-Fable - GetOrSetDefault does not overwrite existing", fun _ ->
        let d = Dict<string, int>()
        d.Set "hello" 42
        let v = d.GetOrSetDefault (fun k -> k.Length) "hello"
        assertThat v (tag "Should return existing value" >> isEqualTo 42)
    )

    test ("Dict-Fable - GetOrSetDefaultValue creates value", fun _ ->
        let d = Dict<string, int>()
        let v = d.GetOrSetDefaultValue 99 "key"
        assertThat v (tag "Should return default value" >> isEqualTo 99)
        assertThat (d.Get "key") (tag "Should be stored" >> isEqualTo 99)
    )

    test ("Dict-Fable - reference type values", fun _ ->
        let d = Dict<string, int list>()
        d.Set "A" [1; 2; 3]
        d.Set "B" []
        assertThat (d.Get "A") (tag "List value should be stored" >> isEqualTo [1; 2; 3])
        assertThat (d.Get "B") (tag "Empty list value should be stored" >> isEqualTo [])
    )

    test ("Dict-Fable - create from pairs", fun _ ->
        let d = Dict.create [("a", 1); ("b", 2); ("c", 3)]
        assertThat d.Count (tag "Should have 3 items" >> isEqualTo 3)
        assertThat (d.Get "a") (tag "Value a" >> isEqualTo 1)
        assertThat (d.Get "b") (tag "Value b" >> isEqualTo 2)
        assertThat (d.Get "c") (tag "Value c" >> isEqualTo 3)
    )

    test ("Dict-Fable - create from empty list", fun _ ->
        let d = Dict.create []
        assertThat d.Count (tag "Should be empty" >> isEqualTo 0)
        assertThat d.IsEmpty (tag "Should be empty" >> isTrue)
    )

    // ---------------------------------------------------------
    // Dict - AsString / ToString (FABLE_COMPILER branching)
    // ---------------------------------------------------------

    test ("Dict-Fable - AsString empty", fun _ ->
        let d = Dict<string, int>()
        let s = d.AsString
        assertThat (s.Contains "Dict") (tag "AsString should contain Dict" >> isTrue)
        assertThat (s.Contains "0" || s.Contains "empty") (tag "AsString on empty should indicate empty" >> isTrue)
    )

    test ("Dict-Fable - AsString with items", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        let s = d.AsString
        assertThat (s.Contains "A") (tag "AsString should contain key" >> isTrue)
        assertThat (s.Contains "1") (tag "AsString should contain value" >> isTrue)
    )

    test ("Dict-Fable - AsString with more than 5 items shows ellipsis", fun _ ->
        let d = Dict<string, int>()
        for i in 1..7 do
            d.Set (string (char (64 + i))) i
        let s = d.AsString
        assertThat (s.Contains "...") (tag "AsString with >5 items should show ellipsis" >> isTrue)
    )

    test ("Dict-Fable - AsString with exactly 5 items no ellipsis", fun _ ->
        let d = Dict<string, int>()
        for i in 1..5 do
            d.Set (string (char (64 + i))) i
        let s = d.AsString
        assertThat (s.Contains "...") (tag "AsString with exactly 5 items should not show ellipsis" >> isFalse)
    )

    test ("Dict-Fable - ToString() empty", fun _ ->
        let d = Dict<string, int>()
        let s = d.ToString()
        assertThat (s.Contains "Dict") (tag "ToString should contain Dict" >> isTrue)
        assertThat (s.Contains "empty") (tag "ToString on empty should say empty" >> isTrue)
    )

    test ("Dict-Fable - ToString() with items", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        d.Set "B" 2
        let s = d.ToString()
        assertThat (s.Contains "2") (tag "ToString should show count" >> isTrue)
    )

    test ("Dict-Fable - ToString(n) with 0 entries to print", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        let s = d.ToString(0)
        assertThat (s.Contains "Dict") (tag "ToString(0) should contain Dict" >> isTrue)
        assertThat (s.Contains "A : 1") (tag "ToString(0) should not show entries" >> isFalse)
    )

    test ("Dict-Fable - ToString(n) with limited entries", fun _ ->
        let d = Dict<string, int>()
        for i in 1..10 do
            d.Set (string (char (64 + i))) i
        let s = d.ToString(3)
        assertThat (s.Contains "...") (tag "ToString(3) with 10 items should show ellipsis" >> isTrue)
    )

    // ---------------------------------------------------------
    // Dict - ICollection interface (routed differently in Fable)
    // ---------------------------------------------------------

    test ("Dict-Fable - ICollection Add and Count", fun _ ->
        let d = Dict<string, int>()
        let coll = d :> ICollection<KeyValuePair<string, int>>
        coll.Add(KeyValuePair("A", 1))
        coll.Add(KeyValuePair("B", 2))
        assertThat coll.Count (tag "ICollection count should match" >> isEqualTo 2)
    )

    test ("Dict-Fable - ICollection Contains", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        let coll = d :> ICollection<KeyValuePair<string, int>>
        assertThat (coll.Contains(KeyValuePair("A", 1))) (tag "ICollection should contain existing pair" >> isTrue)
        assertThat (coll.Contains(KeyValuePair("A", 999))) (tag "ICollection Contains checks the value too, like Dictionary" >> isFalse)
    )

    test ("Dict-Fable - ICollection Remove", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        let coll = d :> ICollection<KeyValuePair<string, int>>
        let removed = coll.Remove(KeyValuePair("A", 1))
        assertThat removed (tag "ICollection Remove should return true" >> isTrue)
        assertThat d.Count (tag "Count should be 0 after remove" >> isEqualTo 0)
    )

    test ("Dict-Fable - ICollection Clear", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        d.Set "B" 2
        let coll = d :> ICollection<KeyValuePair<string, int>>
        coll.Clear()
        assertThat d.Count (tag "Clear via ICollection should remove all" >> isEqualTo 0)
    )

    test ("Dict-Fable - ICollection IsReadOnly", fun _ ->
        let d = Dict<string, int>()
        let coll = d :> ICollection<KeyValuePair<string, int>>
        assertThat coll.IsReadOnly (tag "IsReadOnly should be false" >> isFalse)
    )

    // ---------------------------------------------------------
    // Dict - IDictionary interface
    // ---------------------------------------------------------

    test ("Dict-Fable - IDictionary Item get and set", fun _ ->
        let d = Dict<string, int>() :> IDictionary<string, int>
        d.["A"] <- 1
        assertThat d.["A"] (tag "IDictionary indexer should work" >> isEqualTo 1)
    )

    test ("Dict-Fable - IDictionary TryGetValue existing", fun _ ->
        let d = Dict<string, int>() :> IDictionary<string, int>
        d.Add("A", 1)
        let ok, v = d.TryGetValue "A"
        assertThat ok (tag "TryGetValue should succeed" >> isTrue)
        assertThat v (tag "TryGetValue should return value" >> isEqualTo 1)
    )

    test ("Dict-Fable - IDictionary TryGetValue missing", fun _ ->
        let d = Dict<string, int>() :> IDictionary<string, int>
        let ok, _ = d.TryGetValue "missing"
        assertThat ok (tag "TryGetValue should fail for missing key" >> isFalse)
    )

    // ---------------------------------------------------------
    // Dict - IReadOnlyDictionary interface
    // ---------------------------------------------------------

    test ("Dict-Fable - IReadOnlyDictionary Item", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        let rod = d :> IReadOnlyDictionary<string, int>
        assertThat rod.["A"] (tag "IReadOnlyDictionary indexer should work" >> isEqualTo 1)
    )

    test ("Dict-Fable - IReadOnlyDictionary ContainsKey", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        let rod = d :> IReadOnlyDictionary<string, int>
        assertThat (rod.ContainsKey "A") (tag "Should contain key" >> isTrue)
        assertThat (rod.ContainsKey "B") (tag "Should not contain missing key" >> isFalse)
    )

    test ("Dict-Fable - IReadOnlyDictionary Count", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        d.Set "B" 2
        let rod = d :> IReadOnlyDictionary<string, int>
        assertThat rod.Count (tag "IReadOnlyDictionary count should match" >> isEqualTo 2)
    )

    test ("Dict-Fable - IReadOnlyDictionary Keys and Values", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        d.Set "B" 2
        let rod = d :> IReadOnlyDictionary<string, int>
        let keys = rod.Keys |> Seq.toList |> List.sort
        let values = rod.Values |> Seq.toList |> List.sort
        assertThat keys (tag "Keys via IReadOnlyDictionary" >> isEqualTo ["A"; "B"])
        assertThat values (tag "Values via IReadOnlyDictionary" >> isEqualTo [1; 2])
    )

    // =============================================================
    // DefaultDict - Fable/JS parity tests
    // =============================================================

    test ("DefaultDict-Fable - Get on missing key creates default", fun _ ->
        let d = DefaultDict(fun _ -> 42)
        let v = d.Get "missing"
        assertThat v (tag "Should return default value" >> isEqualTo 42)
        assertThat (d.ContainsKey "missing") (tag "Key should be created" >> isTrue)
        assertThat d.Count (tag "Count should be 1" >> isEqualTo 1)
    )

    test ("DefaultDict-Fable - Get with key-dependent default", fun _ ->
        let d = DefaultDict(fun k -> String.length k)
        let v1 = d.Get "hi"
        let v2 = d.Get "hello"
        assertThat v1 (tag "Default for 'hi'" >> isEqualTo 2)
        assertThat v2 (tag "Default for 'hello'" >> isEqualTo 5)
    )

    test ("DefaultDict-Fable - Item indexer creates default", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        let v = d.["new_key"]
        assertThat v (tag "Indexer should return default" >> isEqualTo 0)
        assertThat (d.ContainsKey "new_key") (tag "Key should exist after indexer access" >> isTrue)
    )

    test ("DefaultDict-Fable - Item indexer set", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.["A"] <- 99
        assertThat d.["A"] (tag "Indexer set should store value" >> isEqualTo 99)
    )

    test ("DefaultDict-Fable - Item indexer set overwrites", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.["A"] <- 1
        d.["A"] <- 2
        assertThat d.["A"] (tag "Indexer set should overwrite" >> isEqualTo 2)
        assertThat d.Count (tag "No duplicate keys" >> isEqualTo 1)
    )

    test ("DefaultDict-Fable - Set and Get with int keys", fun _ ->
        let d = DefaultDict(fun _ -> "default")
        d.Set 1 "one"
        d.Set 2 "two"
        assertThat (d.Get 1) (tag "Int key 1" >> isEqualTo "one")
        assertThat (d.Get 2) (tag "Int key 2" >> isEqualTo "two")
        assertThat (d.Get 3) (tag "Missing int key gets default" >> isEqualTo "default")
    )

    test ("DefaultDict-Fable - null key throws on Get", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        assertThat (fun () -> d.Get null |> ignore) (tag "Get null key should throw" >> throws)
    )

    test ("DefaultDict-Fable - null key throws on Set", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        assertThat (fun () -> d.Set null 1) (tag "Set null key should throw" >> throws)
    )

    test ("DefaultDict-Fable - null key throws on Item get", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        assertThat (fun () -> d.[null] |> ignore) (tag "Item get null key should throw" >> throws)
    )

    test ("DefaultDict-Fable - null key throws on Item set", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        assertThat (fun () -> d.[null] <- 1) (tag "Item set null key should throw" >> throws)
    )

    test ("DefaultDict-Fable - null key throws on Pop", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        assertThat (fun () -> d.Pop null |> ignore) (tag "Pop null key should throw" >> throws)
    )

    test ("DefaultDict-Fable - null key throws on TryPop", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        assertThat (fun () -> d.TryPop null |> ignore) (tag "TryPop null key should throw" >> throws)
    )

    test ("DefaultDict-Fable - Pop existing key", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 42
        let v = d.Pop "A"
        assertThat v (tag "Pop should return stored value" >> isEqualTo 42)
        assertThat (d.ContainsKey "A") (tag "Pop should remove key" >> isFalse)
    )

    test ("DefaultDict-Fable - Pop missing key throws (does not create default)", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        assertThat (fun () -> d.Pop "missing" |> ignore) (tag "Pop missing key should throw, not create default" >> throws)
    )

    test ("DefaultDict-Fable - TryPop existing key", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 42
        let v = d.TryPop "A"
        assertThat v (tag "TryPop should return Some" >> isEqualTo (Some 42))
        assertThat (d.ContainsKey "A") (tag "TryPop should remove key" >> isFalse)
    )

    test ("DefaultDict-Fable - TryPop missing key returns None (no default creation)", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        let v = d.TryPop "missing"
        assertThat v (tag "TryPop missing should return None" >> isEqualTo None)
        assertThat (d.ContainsKey "missing") (tag "TryPop should not create key" >> isFalse)
    )

    test ("DefaultDict-Fable - TryGetValue does not create default", fun _ ->
        let d = DefaultDict(fun _ -> 42)
        let ok, _ = d.TryGetValue "missing"
        assertThat ok (tag "TryGetValue should return false for missing key" >> isFalse)
        assertThat (d.ContainsKey "missing") (tag "TryGetValue should NOT create key" >> isFalse)
        assertThat d.Count (tag "Count should still be 0" >> isEqualTo 0)
    )

    test ("DefaultDict-Fable - TryGetValue existing key", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 99
        let ok, v = d.TryGetValue "A"
        assertThat ok (tag "TryGetValue should return true" >> isTrue)
        assertThat v (tag "TryGetValue should return value" >> isEqualTo 99)
    )

    test ("DefaultDict-Fable - ContainsKey on empty", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        assertThat (d.ContainsKey "A") (tag "Empty DefaultDict should not contain any key" >> isFalse)
    )

    test ("DefaultDict-Fable - DoesNotContainKey", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        assertThat (d.DoesNotContainKey "A") (tag "Empty DefaultDict DoesNotContainKey" >> isTrue)
        d.Set "A" 1
        assertThat (d.DoesNotContainKey "A") (tag "Should not say DoesNotContainKey for present key" >> isFalse)
    )

    test ("DefaultDict-Fable - Remove existing key", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 1
        let removed = d.Remove "A"
        assertThat removed (tag "Remove existing key should return true" >> isTrue)
        assertThat (d.ContainsKey "A") (tag "Key should be gone" >> isFalse)
    )

    test ("DefaultDict-Fable - Remove missing key", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        let removed = d.Remove "missing"
        assertThat removed (tag "Remove missing key should return false" >> isFalse)
    )

    test ("DefaultDict-Fable - Clear", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        for i in 1..50 do d.Set (string i) i
        assertThat d.Count (tag "Should have 50 entries" >> isEqualTo 50)
        d.Clear()
        assertThat d.Count (tag "Clear should remove all" >> isEqualTo 0)
    )

    test ("DefaultDict-Fable - Clear preserves default function", fun _ ->
        let d = DefaultDict(fun _ -> 42)
        d.Set "A" 1
        d.Clear()
        let v = d.Get "B"
        assertThat v (tag "Default function should still work after Clear" >> isEqualTo 42)
    )

    test ("DefaultDict-Fable - Count", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        assertThat d.Count (tag "Initial count 0" >> isEqualTo 0)
        d.Set "A" 1
        assertThat d.Count (tag "Count after set" >> isEqualTo 1)
        let _ = d.Get "B" // creates default
        assertThat d.Count (tag "Count after Get on missing key" >> isEqualTo 2)
        d.Remove "A" |> ignore
        assertThat d.Count (tag "Count after remove" >> isEqualTo 1)
    )

    test ("DefaultDict-Fable - Keys and Values", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "X" 10
        d.Set "Y" 20
        let keys = d.Keys |> Seq.toList |> List.sort
        let values = d.Values |> Seq.toList |> List.sort
        assertThat keys (tag "Keys" >> isEqualTo ["X"; "Y"])
        assertThat values (tag "Values" >> isEqualTo [10; 20])
    )

    test ("DefaultDict-Fable - Items", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 1
        d.Set "B" 2
        let items = d.Items |> Seq.toList |> List.sortBy fst
        assertThat items (tag "Items should return tuples" >> isEqualTo [("A", 1); ("B", 2)])
    )

    test ("DefaultDict-Fable - empty string key", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "" 42
        assertThat (d.Get "") (tag "Empty string key should work" >> isEqualTo 42)
    )

    test ("DefaultDict-Fable - null value allowed", fun _ ->
        let d = DefaultDict(fun _ -> "default")
        d.Set "key" null
        assertThat (d.Get "key") (tag "Null value should be stored and retrieved" >> isEqualTo null)
    )

    test ("DefaultDict-Fable - reference type default (list)", fun _ ->
        let d = DefaultDict(fun _ -> ResizeArray<int>())
        d.Get("A").Add(1)
        d.Get("A").Add(2)
        d.Get("B").Add(10)
        assertThat (d.Get("A") |> Seq.toList) (tag "List for A" >> isEqualTo [1; 2])
        assertThat (d.Get("B") |> Seq.toList) (tag "List for B" >> isEqualTo [10])
    )

    test ("DefaultDict-Fable - ref cell default (value type wrapper)", fun _ ->
        let d = DefaultDict(fun _ -> ref 0)
        incr d.["counter1"]
        incr d.["counter1"]
        incr d.["counter2"]
        assertThat d.["counter1"].Value (tag "Counter1 should be 2" >> isEqualTo 2)
        assertThat d.["counter2"].Value (tag "Counter2 should be 1" >> isEqualTo 1)
    )

    test ("DefaultDict-Fable - int key with default", fun _ ->
        let d = DefaultDict(fun k -> k * 10)
        assertThat (d.Get 5) (tag "Default for key 5" >> isEqualTo 50)
        assertThat (d.Get 0) (tag "Default for key 0" >> isEqualTo 0)
        assertThat (d.Get -3) (tag "Default for key -3" >> isEqualTo (-30))
    )

    test ("DefaultDict-Fable - many entries", fun _ ->
        let d = DefaultDict(fun k -> k * k)
        for i in 0..999 do
            let _ = d.Get i
            ()
        assertThat d.Count (tag "Should have 1000 entries" >> isEqualTo 1000)
        assertThat (d.Get 0) (tag "Value at 0" >> isEqualTo 0)
        assertThat (d.Get 999) (tag "Value at 999" >> isEqualTo (999 * 999))
    )

    test ("DefaultDict-Fable - create from pairs", fun _ ->
        let d = DefaultDict.create (fun _ -> 0) [("a", 1); ("b", 2)]
        assertThat d.Count (tag "Should have 2 items" >> isEqualTo 2)
        assertThat (d.Get "a") (tag "Existing value a" >> isEqualTo 1)
        assertThat (d.Get "b") (tag "Existing value b" >> isEqualTo 2)
        assertThat (d.Get "c") (tag "Missing key gets default" >> isEqualTo 0)
    )

    test ("DefaultDict-Fable - create from empty", fun _ ->
        let d = DefaultDict.create (fun _ -> 99) []
        assertThat d.Count (tag "Should be empty" >> isEqualTo 0)
        assertThat (d.Get "any") (tag "Missing key gets default" >> isEqualTo 99)
    )

    test ("DefaultDict-Fable - Add method", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Add("A", 1)
        assertThat (d.Get "A") (tag "Add should store value" >> isEqualTo 1)
    )

    test ("DefaultDict-Fable - ContainsValue", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 42
        assertThat (d.ContainsValue 42) (tag "Should find existing value" >> isTrue)
        assertThat (d.ContainsValue 99) (tag "Should not find missing value" >> isFalse)
    )

    test ("DefaultDict-Fable - enumerate empty", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        let items = d |> Seq.toList
        assertThat items (tag "Enumerating empty DefaultDict should yield empty list" >> isEqualTo [])
    )

    test ("DefaultDict-Fable - enumerate with items", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 1
        d.Set "B" 2
        let items = d |> Seq.map (fun kvp -> kvp.Key, kvp.Value) |> Seq.toList |> List.sortBy fst
        assertThat items (tag "Should enumerate all entries" >> isEqualTo [("A", 1); ("B", 2)])
    )

    // DefaultDict - AsString / ToString (FABLE_COMPILER branching)

    test ("DefaultDict-Fable - AsString empty", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        let s = d.AsString
        assertThat (s.Contains "DefaultDict") (tag "AsString should contain DefaultDict" >> isTrue)
        assertThat (s.Contains "empty") (tag "AsString on empty should say empty" >> isTrue)
    )

    test ("DefaultDict-Fable - AsString with items", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 1
        let s = d.AsString
        assertThat (s.Contains "A") (tag "AsString should contain key" >> isTrue)
        assertThat (s.Contains "1") (tag "AsString should contain value" >> isTrue)
    )

    test ("DefaultDict-Fable - AsString with more than 5 items shows ellipsis", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        for i in 1..7 do
            d.Set (string (char (64 + i))) i
        let s = d.AsString
        assertThat (s.Contains "...") (tag "AsString with >5 items should show ellipsis" >> isTrue)
    )

    test ("DefaultDict-Fable - AsString with exactly 5 items no ellipsis", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        for i in 1..5 do
            d.Set (string (char (64 + i))) i
        let s = d.AsString
        assertThat (s.Contains "...") (tag "AsString with exactly 5 items should not show ellipsis" >> isFalse)
    )

    test ("DefaultDict-Fable - ToString() empty", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        let s = d.ToString()
        assertThat (s.Contains "DefaultDict") (tag "ToString should contain DefaultDict" >> isTrue)
        assertThat (s.Contains "empty") (tag "ToString on empty should say empty" >> isTrue)
    )

    test ("DefaultDict-Fable - ToString() with items", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 1
        d.Set "B" 2
        let s = d.ToString()
        assertThat (s.Contains "2") (tag "ToString should show count" >> isTrue)
    )

    test ("DefaultDict-Fable - ToString(0) hides entries", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 1
        let s = d.ToString(0)
        assertThat (s.Contains "DefaultDict") (tag "Should contain DefaultDict" >> isTrue)
        assertThat (s.Contains "A : 1") (tag "Should not show entries" >> isFalse)
    )

    test ("DefaultDict-Fable - ToString(n) with limited entries", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        for i in 1..10 do
            d.Set (string (char (64 + i))) i
        let s = d.ToString(3)
        assertThat (s.Contains "...") (tag "ToString(3) with 10 items should show ellipsis" >> isTrue)
    )

    // DefaultDict - ICollection interface

    test ("DefaultDict-Fable - ICollection Add", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        let coll = d :> ICollection<KeyValuePair<string, int>>
        coll.Add(KeyValuePair("A", 1))
        assertThat d.Count (tag "ICollection Add should work" >> isEqualTo 1)
        assertThat (d.Get "A") (tag "Value should be accessible" >> isEqualTo 1)
    )

    test ("DefaultDict-Fable - ICollection Contains", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 1
        let coll = d :> ICollection<KeyValuePair<string, int>>
        assertThat (coll.Contains(KeyValuePair("A", 1))) (tag "Should contain existing pair" >> isTrue)
    )

    test ("DefaultDict-Fable - ICollection Remove", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 1
        let coll = d :> ICollection<KeyValuePair<string, int>>
        let removed = coll.Remove(KeyValuePair("A", 1))
        assertThat removed (tag "Should remove" >> isTrue)
        assertThat d.Count (tag "Count should be 0" >> isEqualTo 0)
    )

    test ("DefaultDict-Fable - ICollection Clear", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 1
        d.Set "B" 2
        let coll = d :> ICollection<KeyValuePair<string, int>>
        coll.Clear()
        assertThat d.Count (tag "Should be empty after clear" >> isEqualTo 0)
    )

    test ("DefaultDict-Fable - ICollection IsReadOnly", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        let coll = d :> ICollection<KeyValuePair<string, int>>
        assertThat coll.IsReadOnly (tag "Should not be read-only" >> isFalse)
    )

    test ("DefaultDict-Fable - IReadOnlyCollection Count", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 1
        d.Set "B" 2
        let roc = d :> IReadOnlyCollection<KeyValuePair<string, int>>
        assertThat roc.Count (tag "IReadOnlyCollection count should match" >> isEqualTo 2)
    )

    // =============================================================
    // IDictionary Extension Methods - Fable/JS parity tests
    // =============================================================

    test ("IDic-Fable - SetValue and GetValue", fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        d.SetValue "K" 42
        assertThat (d.GetValue "K") (tag "SetValue/GetValue round-trip" >> isEqualTo 42)
    )

    test ("IDic-Fable - GetValue missing key throws", fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        assertThat (fun () -> d.GetValue "missing" |> ignore) (tag "GetValue missing key should throw" >> throws)
    )

    test ("IDic-Fable - Pop existing", fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        d.["A"] <- 1
        let v = d.Pop "A"
        assertThat v (tag "Pop should return value" >> isEqualTo 1)
        assertThat (d.ContainsKey "A") (tag "Pop should remove key" >> isFalse)
    )

    test ("IDic-Fable - Pop missing throws", fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        assertThat (fun () -> d.Pop "missing" |> ignore) (tag "Pop missing should throw" >> throws)
    )

    test ("IDic-Fable - TryPop existing", fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        d.["A"] <- 1
        let v = d.TryPop "A"
        assertThat v (tag "TryPop should return Some" >> isEqualTo (Some 1))
        assertThat (d.ContainsKey "A") (tag "TryPop should remove key" >> isFalse)
    )

    test ("IDic-Fable - TryPop missing returns None", fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        let v = d.TryPop "missing"
        assertThat v (tag "TryPop missing should return None" >> isEqualTo None)
    )

    test ("IDic-Fable - Items empty", fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        let items = d.Items |> Seq.toList
        assertThat items (tag "Items on empty should be empty" >> isEqualTo [])
    )

    test ("IDic-Fable - Items with entries", fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        d.["A"] <- 1
        d.["B"] <- 2
        let items = d.Items |> Seq.toList |> List.sortBy fst
        assertThat items (tag "Items should return tuples" >> isEqualTo [("A", 1); ("B", 2)])
    )

    test ("IDic-Fable - KeysSeq and ValuesSeq", fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        d.["A"] <- 1
        d.["B"] <- 2
        let ks = d.KeysSeq |> Seq.toList |> List.sort
        let vs = d.ValuesSeq |> Seq.toList |> List.sort
        assertThat ks (tag "KeysSeq" >> isEqualTo ["A"; "B"])
        assertThat vs (tag "ValuesSeq" >> isEqualTo [1; 2])
    )

    test ("IDic-Fable - DoesNotContainKey", fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        assertThat (d.DoesNotContainKey "A") (tag "Empty dict DoesNotContainKey" >> isTrue)
        d.["A"] <- 1
        assertThat (d.DoesNotContainKey "A") (tag "Present key" >> isFalse)
    )

    test ("IDic-Fable - AsString empty", fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        let s = d.AsString
        assertThat (s.Contains "empty") (tag "AsString on empty should say empty" >> isTrue)
    )

    test ("IDic-Fable - AsString with items", fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        d.["A"] <- 1
        let s = d.AsString
        assertThat (s.Contains "A") (tag "AsString should contain key" >> isTrue)
        assertThat (s.Contains "1") (tag "AsString should contain value" >> isTrue)
    )

    test ("IDic-Fable - AsString with more than 5 items shows ellipsis", fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        for i in 1..7 do
            d.[string (char (64 + i))] <- i
        let s = d.AsString
        assertThat (s.Contains "...") (tag "Should show ellipsis for >5 items" >> isTrue)
    )

    test ("IDic-Fable - ToString(n) with 0 entries", fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        d.["A"] <- 1
        let s = d.ToString(0)
        assertThat (s.Contains "A : 1") (tag "Should not show entries with 0 entriesToPrint" >> isFalse)
    )

    test ("IDic-Fable - ToString(n) with limited entries shows ellipsis", fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        for i in 1..10 do
            d.[string (char (64 + i))] <- i
        let s = d.ToString(3)
        assertThat (s.Contains "...") (tag "Should show ellipsis" >> isTrue)
    )

    // IDictionary extensions used via Dict (cast to IDictionary)

    test ("IDic-Fable - Dict cast to IDictionary Pop", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 42
        let iDict = d :> IDictionary<string, int>
        let v = iDict.Pop "A"
        assertThat v (tag "Pop via IDictionary on Dict" >> isEqualTo 42)
        assertThat (d.ContainsKey "A") (tag "Should be removed from Dict too" >> isFalse)
    )

    test ("IDic-Fable - Dict cast to IDictionary TryPop", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 42
        let iDict = d :> IDictionary<string, int>
        let v = iDict.TryPop "A"
        assertThat v (tag "TryPop via IDictionary on Dict" >> isEqualTo (Some 42))
        assertThat d.Count (tag "Should be removed" >> isEqualTo 0)
    )

    test ("IDic-Fable - Dict cast to IDictionary Items", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        d.Set "B" 2
        let iDict = d :> IDictionary<string, int>
        let items = iDict.Items |> Seq.toList |> List.sortBy fst
        assertThat items (tag "Items via IDictionary on Dict" >> isEqualTo [("A", 1); ("B", 2)])
    )

    // Edge cases: value types as values

    test ("Dict-Fable - struct tuple values", fun _ ->
        let d = Dict<string, struct(int * int)>()
        d.Set "point" (struct(1, 2))
        let struct(x, y) = d.Get "point"
        assertThat x (tag "Struct tuple x" >> isEqualTo 1)
        assertThat y (tag "Struct tuple y" >> isEqualTo 2)
    )

    test ("DefaultDict-Fable - zero default for int", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        assertThat (d.Get "any") (tag "Default int should be 0" >> isEqualTo 0)
    )

    test ("DefaultDict-Fable - empty string default", fun _ ->
        let d = DefaultDict(fun _ -> "")
        assertThat (d.Get "any") (tag "Default string should be empty" >> isEqualTo "")
    )

    test ("DefaultDict-Fable - false default for bool", fun _ ->
        let d = DefaultDict(fun _ -> false)
        assertThat (d.Get "any") (tag "Default bool should be false" >> isEqualTo false)
    )


    // =============================================================
    // Same semantics as System.Collections.Generic.Dictionary:
    // Add and create throw on duplicate keys, ICollection compares values
    // =============================================================

    test ("Dict.Add throws on duplicate key", fun _ ->
        let d = Dict<string, int>()
        d.Add("dupKey", 1)
        throwsWith<ArgumentException> "dupKey" (fun () -> d.Add("dupKey", 2)) "Dict.Add duplicate"
        assertThat d.["dupKey"] (tag "Value should be unchanged" >> isEqualTo 1)
    )

    test ("Dict.Add throws on null key", fun _ ->
        let d = Dict<string, int>()
        throwsWith<ArgumentNullException> "Dict.Add" (fun () -> d.Add(null, 1)) "Dict.Add null key"
    )

    test ("Dict IDictionary.Add throws on duplicate key", fun _ ->
        let d = Dict<string, int>()
        d.Set "dupKey" 1
        let iDict = d :> IDictionary<string, int>
        throwsWith<ArgumentException> "dupKey" (fun () -> iDict.Add("dupKey", 2)) "IDictionary.Add duplicate"
        assertThat d.["dupKey"] (tag "Value should be unchanged" >> isEqualTo 1)
    )

    test ("Dict IDictionary indexer set null key throws", fun _ ->
        let d = Dict<string, int>() :> IDictionary<string, int>
        assertThat (fun () -> d.[null] <- 1) (tag "IDictionary indexer set with null key should throw" >> throws)
    )

    test ("Dict ICollection.Add throws on duplicate key", fun _ ->
        let d = Dict<string, int>()
        d.Set "dupKey" 1
        let coll = d :> ICollection<KeyValuePair<string, int>>
        throwsWith<ArgumentException> "dupKey" (fun () -> coll.Add(KeyValuePair("dupKey", 2))) "ICollection.Add duplicate"
    )

    test ("Dict ICollection Contains and Remove compare the value", fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        let coll = d :> ICollection<KeyValuePair<string, int>>
        assertThat (coll.Contains(KeyValuePair("A", 2))) (tag "Contains with a different value" >> isFalse)
        assertThat (coll.Contains(KeyValuePair("B", 1))) (tag "Contains with a missing key" >> isFalse)
        assertThat (coll.Remove(KeyValuePair("A", 2))) (tag "Remove with a different value" >> isFalse)
        assertThat d.Count (tag "Remove with a different value should not remove" >> isEqualTo 1)
        assertThat (coll.Remove(KeyValuePair("A", 1))) (tag "Remove with the same value" >> isTrue)
        assertThat d.Count (tag "Remove with the same value should remove" >> isEqualTo 0)
    )

    test ("Dict ICollection Contains uses value equality", fun _ ->
        let d = Dict<string, int list>()
        d.Set "A" [1; 2]
        let coll = d :> ICollection<KeyValuePair<string, int list>>
        assertThat (coll.Contains(KeyValuePair("A", [1; 2]))) (tag "An equal list should be found" >> isTrue)
        assertThat (coll.Contains(KeyValuePair("A", [1]))) (tag "A different list should not be found" >> isFalse)
    )

    test ("DefaultDict.Add throws on duplicate key", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Add("dupKey", 1)
        throwsWith<ArgumentException> "dupKey" (fun () -> d.Add("dupKey", 2)) "DefaultDict.Add duplicate"
        assertThat d.["dupKey"] (tag "Value should be unchanged" >> isEqualTo 1)
    )

    test ("DefaultDict.Add throws on null key", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        throwsWith<ArgumentNullException> "DefaultDict.Add" (fun () -> d.Add(null, 1)) "DefaultDict.Add null key"
    )

    test ("DefaultDict ICollection.Add throws on duplicate key", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "dupKey" 1
        let coll = d :> ICollection<KeyValuePair<string, int>>
        throwsWith<ArgumentException> "dupKey" (fun () -> coll.Add(KeyValuePair("dupKey", 2))) "DefaultDict ICollection.Add duplicate"
    )

    test ("DefaultDict ICollection Contains and Remove compare the value", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 1
        let coll = d :> ICollection<KeyValuePair<string, int>>
        assertThat (coll.Contains(KeyValuePair("A", 2))) (tag "Contains with a different value" >> isFalse)
        assertThat (coll.Remove(KeyValuePair("A", 2))) (tag "Remove with a different value" >> isFalse)
        assertThat d.Count (tag "Remove with a different value should not remove" >> isEqualTo 1)
        assertThat (coll.Remove(KeyValuePair("A", 1))) (tag "Remove with the same value" >> isTrue)
        assertThat d.Count (tag "Remove with the same value should remove" >> isEqualTo 0)
    )

    test ("Dict.add throws on duplicate key", fun _ ->
        let d = Dict<string, int>()
        Dict.add "dupKey" 1 d
        throwsWith<ArgumentException> "dupKey" (fun () -> Dict.add "dupKey" 2 d) "Dict.add duplicate"
        assertThat d.["dupKey"] (tag "Value should be unchanged" >> isEqualTo 1)
    )

    test ("Dict.add on a Dictionary throws on duplicate key", fun _ ->
        let d = Dictionary<string, int>()
        Dict.add "dupKey" 1 d
        throwsWith<ArgumentException> "dupKey" (fun () -> Dict.add "dupKey" 2 d) "Dict.add duplicate on Dictionary"
    )

    test ("Dict.add throws on null key", fun _ ->
        let d = Dict<string, int>()
        throwsWith<ArgumentNullException> "Dict.add" (fun () -> Dict.add null 1 d) "Dict.add null key"
    )

    test ("Dict.create throws on duplicate keys", fun _ ->
        throwsWith<ArgumentException> "dupKey" (fun () -> Dict.create ["dupKey", 1; "dupKey", 2] |> ignore) "Dict.create duplicate"
    )

    test ("Dict.create throws on null key", fun _ ->
        throwsWith<ArgumentNullException> "Dict.create" (fun () -> Dict.create [(null:string), 1] |> ignore) "Dict.create null key"
    )

    test ("DefaultDict.create throws on duplicate keys", fun _ ->
        throwsWith<ArgumentException> "dupKey" (fun () -> DefaultDict.create (fun _ -> 0) ["dupKey", 1; "dupKey", 2] |> ignore) "DefaultDict.create duplicate"
    )

    test ("Dict.createDirectly shares the Dictionary", fun _ ->
        let src = Dictionary<string, int>()
        src.["A"] <- 1
        let d = Dict.createDirectly src
        assertThat (d.Get "A") (tag "Existing item should be there" >> isEqualTo 1)
        src.["B"] <- 2
        assertThat d.Count (tag "Items added to the source should show in the Dict" >> isEqualTo 2)
        d.Set "C" 3
        assertThat src.["C"] (tag "Items added to the Dict should show in the source" >> isEqualTo 3)
        assertThat d.InternalDictionary.Count (tag "InternalDictionary should be the source" >> isEqualTo 3)
    )

    test ("DefaultDict.createDirectly shares the Dictionary", fun _ ->
        let src = Dictionary<string, int>()
        src.["A"] <- 1
        let d = DefaultDict.createDirectly (fun _ -> 0) src
        assertThat d.Count (tag "Existing item should be there" >> isEqualTo 1)
        assertThat (d.Get "A") (tag "Existing value" >> isEqualTo 1)
        assertThat (d.Get "B") (tag "Default value" >> isEqualTo 0)
        assertThat (src.ContainsKey "B") (tag "Default value should be added to the source" >> isTrue)
        assertThat d.InternalDictionary.Count (tag "InternalDictionary should be the source" >> isEqualTo 2)
    )

    test ("DefaultDict.get and DefaultDict.set take the key first", fun _ ->
        let d = DefaultDict(fun _ -> 0)
        DefaultDict.set "A" 5 d
        assertThat (DefaultDict.get "A" d) (tag "get after set" >> isEqualTo 5)
        assertThat (d |> DefaultDict.get "B") (tag "get of missing key returns default" >> isEqualTo 0)
    )

    test ("IDictionary.SetValue null key throws", fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        throwsWith<ArgumentNullException> "SetValue" (fun () -> d.SetValue null 1) "SetValue null key"
    )

    #if !FABLE_COMPILER
    test ("IDictionary.SetValue keeps the original exception", fun _ ->
        let ro = Collections.ObjectModel.ReadOnlyDictionary(Dictionary<string, int>()) :> IDictionary<string, int>
        assertThat (fun () -> ro.SetValue "A" 1) (tag "Read-only dictionary should throw NotSupportedException" >> throws >> satisfy (fun e -> e :? NotSupportedException))
    )
    #endif

    test ("Error messages contain the missing key", fun _ ->
        let d = Dict.create ["A", 1]
        throwsWith<KeyNotFoundException> "missingKey" (fun () -> d.Get "missingKey" |> ignore) "Dict.Get"
        throwsWith<KeyNotFoundException> "missingKey" (fun () -> d.["missingKey"] |> ignore) "Dict.Item"
        throwsWith<KeyNotFoundException> "missingKey" (fun () -> d.Pop "missingKey" |> ignore) "Dict.Pop"
        throwsWith<KeyNotFoundException> "missingKey" (fun () -> Dict.get "missingKey" d |> ignore) "Dict.get"
        throwsWith<KeyNotFoundException> "missingKey" (fun () -> Dict.pop "missingKey" d |> ignore) "Dict.pop"
        let dd = DefaultDict(fun _ -> 0)
        throwsWith<KeyNotFoundException> "missingKey" (fun () -> dd.Pop "missingKey" |> ignore) "DefaultDict.Pop"
        let iDic = Dictionary<string, int>() :> IDictionary<string, int>
        throwsWith<KeyNotFoundException> "missingKey" (fun () -> iDic.GetValue "missingKey" |> ignore) "IDictionary.GetValue"
        throwsWith<KeyNotFoundException> "missingKey" (fun () -> iDic.Pop "missingKey" |> ignore) "IDictionary.Pop"
    )

    test ("ToString(n) edge cases", fun _ ->
        let norm (s:string) = s.Replace("\r\n", "\n").Replace("\n", "$")
        let d = Dict<string, int>()
        assertThat (d.ToString(-1)) (tag "negative count on empty Dict" >> isEqualTo "empty Dict<String,Int32>")
        d.Set "A" 1
        d.Set "B" 2
        d.Set "C" 3
        assertThat (norm (d.ToString(0))) (tag "zero entries: header only" >> isEqualTo "Dict<String,Int32> with 3 items")
        assertThat (norm (d.ToString(-1))) (tag "negative count: header only" >> isEqualTo "Dict<String,Int32> with 3 items")
        assertThat (norm (d.ToString(2))) (tag "two of three entries" >> isEqualTo "Dict<String,Int32> with 3 items:$  A : 1$  B : 2$  ...$")
        assertThat (norm (d.ToString(3))) (tag "all entries, no ellipsis" >> isEqualTo "Dict<String,Int32> with 3 items:$  A : 1$  B : 2$  C : 3$")
    )

    test ("ToString(n) edge cases DefaultDict and IDictionary", fun _ ->
        let norm (s:string) = s.Replace("\r\n", "\n").Replace("\n", "$")
        let dd = DefaultDict(fun _ -> 0)
        dd.Set "A" 1
        dd.Set "B" 2
        assertThat (norm (dd.ToString(0))) (tag "DefaultDict zero entries: header only" >> isEqualTo "DefaultDict<String,Int32> with 2 items")
        assertThat (norm (dd.ToString(1))) (tag "DefaultDict one of two entries" >> isEqualTo "DefaultDict<String,Int32> with 2 items:$  A : 1$  ...$")
        let iDic = Dictionary<string, int>() :> IDictionary<string, int>
        iDic.["A"] <- 1
        assertThat ((iDic.ToString(0)).Contains "...") (tag "IDictionary zero entries: no ellipsis" >> isFalse)
        assertThat ((iDic.ToString(-1)).Contains "...") (tag "IDictionary negative count: no ellipsis" >> isFalse)
    )

  ])


















