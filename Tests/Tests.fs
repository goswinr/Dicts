module TestList

open Dicts
open System.Collections.Generic

#if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
open Fable.Mocha
#else
open Expecto
#endif

open System
open ExtensionsIDictionary

#nowarn "3370" // incr ref


let tests  =
  testList "Module.fs Tests" [

    // ---------------------------------------------------------
    // IDictionary:
    // ---------------------------------------------------------


    testCase "iDic-Items" <| fun _ ->
        let b = Dictionary()  :> IDictionary<string, int>
        b.["A"] <- 1
        b.["B"] <- 2
        let items = b.Items |> Seq.toList
        Expect.equal items [("A", 1); ("B", 2)] "Items returns the key-value pairs"


    testCase "iDic-Get" <| fun _ ->
        let b = Dictionary() :> IDictionary<string, int>
        b.["A"] <- 1
        let result = b.GetValue "A"
        Expect.equal result 1 "Get returns the value of key A"


    testCase "iDic-Pop" <| fun _ ->
        let b = Dictionary() :> IDictionary<string, int>
        b.["A"] <- 1
        let popped  = b.Pop "A"
        let result = b.DoesNotContainKey "A" && popped = 1
        Expect.isTrue result "Pop removed key A"


    // ---------------------------------------------------------
    // Dict:
    // ---------------------------------------------------------

    testCase "Empty" <| fun _ ->
        let b = Dict()
        let result = b.IsEmpty
        Expect.isTrue result "Empty returns true for empty dictionary"


    testCase "Pop" <| fun _ ->
        let b = Dict()
        b.["A"] <- 1
        let popped  = b.Pop "A"
        let result = b.DoesNotContainKey "A" && popped = 1
        Expect.isTrue result "Pop removed key A"


    testCase "Item" <| fun _ ->
        let b = Dict()
        b.["A"] <- 1
        let result = b.Item "A"
        Expect.equal result 1 "Item returns value of key A"


    testCase "Item fails" <| fun _ ->
        let b = Dict()
        b.["A"] <- 1
        Expect.throws (fun () -> b.Get "B" |> ignore ) "Item throws exception when key does not exist"

    testCase "Add fails" <| fun _ ->
        let b = Dict<string,int>()
        Expect.throws (fun () ->b.Set null 1 ) "Add throws exception when key does not exist"


    testCase "Remove" <| fun _ ->
        let b = Dict()
        b.["A"] <- 1
        let removed  = b.Remove "A"
        let result = b.DoesNotContainKey "A"
        Expect.isTrue (result&&removed) "Remove deletes key A"

    testCase "Remove2" <| fun _ ->
        let b = Dict()
        // b.["A"] <- 1
        let removed  = b.Remove "A"
        let result = b.DoesNotContainKey "A"
        Expect.isTrue (result&& not removed) "Remove missing key A"


    testCase "Clear" <| fun _ ->
        let b = Dict()
        b.["A"] <- 1
        b.["B"] <- 2
        b.Clear()
        let resultA = b.DoesNotContainKey "A"
        let resultB = b.DoesNotContainKey "B"
        Expect.isTrue resultA "Clear removes key A"
        Expect.isTrue resultB "Clear removes key B"

    testCase "ContainsKey" <| fun _ ->
        let b = Dict()
        b.["A"] <- 1
        let result = b.ContainsKey "A"
        Expect.isTrue result "ContainsKey returns true for existing key"

    testCase "Count" <| fun _ ->
        let b = Dict()
        b.["A"] <- 1
        b.["B"] <- 2
        let result = b.Count
        Expect.equal result 2 "Count returns the number of key-value pairs"

    testCase "Keys" <| fun _ ->
        let b = Dict()
        b.["A"] <- 1
        b.["B"] <- 2
        let keys = b.Keys |> Seq.toList
        Expect.equal keys ["A"; "B"] "Keys returns the keys"


    testCase "Values" <| fun _ ->
        let b = Dict()
        b.["A"] <- 1
        b.["B"] <- 2
        let values = b.Values |> Seq.toList
        Expect.equal values [1; 2] "Values returns the values"


    testCase "SetIfKeyAbsent - key does not exist" <| fun _ ->
        let b = Dict()
        let result = b.SetIfKeyAbsent "A" 1
        Expect.isTrue result "SetIfKeyAbsent should return true when key does not exist"
        Expect.equal (b.Get "A") 1 "SetIfKeyAbsent should set the value when key does not exist"

    testCase "SetIfKeyAbsent - key exists" <| fun _ ->
        let b = Dict()
        b.["A"] <- 1
        let result = b.SetIfKeyAbsent "A" 2
        Expect.isFalse result "SetIfKeyAbsent should return false when key exists"
        Expect.equal (b.Get "A") 1 "SetIfKeyAbsent should not change the value when key exists"

    testCase "AddIfKeyAbsent - key does not exist" <| fun _ ->
        let b = Dict()
        let result = b.AddIfKeyAbsent "A" 1
        Expect.isTrue result "AddIfKeyAbsent should return true when key does not exist"
        Expect.equal (b.Get "A") 1 "AddIfKeyAbsent should set the value when key does not exist"

    testCase "AddIfKeyAbsent - key exists" <| fun _ ->
        let b = Dict()
        b.["A"] <- 1
        let result = b.AddIfKeyAbsent "A" 2
        Expect.isFalse result "AddIfKeyAbsent should return false when key exists"
        Expect.equal (b.Get "A") 1 "AddIfKeyAbsent should not change the value when key exists"

    testCase "GetOrSetDefault - key does not exist" <| fun _ ->
        let b = Dict()
        let result = b.GetOrSetDefault (fun _ -> 1) "A"
        Expect.equal result 1 "GetOrSetDefault should return the default value when key does not exist"
        Expect.equal (b.Get "A") 1 "GetOrSetDefault should set the default value when key does not exist"

    testCase "GetOrSetDefault - key exists" <| fun _ ->
        let b = Dict()
        b.["A"] <- 1
        let result = b.GetOrSetDefault (fun _ -> 2) "A"
        Expect.equal result 1 "GetOrSetDefault should return the existing value when key exists"
        Expect.equal (b.Get "A") 1 "GetOrSetDefault should not change the value when key exists"

    testCase "GetOrSetDefaultValue - key does not exist" <| fun _ ->
        let b = Dict()
        let result = b.GetOrSetDefaultValue 1 "A"
        Expect.equal result 1 "GetOrSetDefaultValue should return the default value when key does not exist"
        Expect.equal (b.Get "A") 1 "GetOrSetDefaultValue should set the default value when key does not exist"

    testCase "GetOrSetDefaultValue - key exists" <| fun _ ->
        let b = Dict()
        b.["A"] <- 1
        let result = b.GetOrSetDefaultValue 2 "A"
        Expect.equal result 1 "GetOrSetDefaultValue should return the existing value when key exists"
        Expect.equal (b.Get "A") 1 "GetOrSetDefaultValue should not change the value when key exists"

    testCase "TryGetValue" <| fun _ ->
        let b = Dict()
        b.["A"] <- 1
        let ok, value = b.TryGetValue "A"
        Expect.isTrue ok "TryGetValue should return true when the key exists"
        Expect.equal value 1 "TryGetValue should return the value when the key exists"


    testCase "TryGetValue - key does not exist" <| fun _ ->
        let b = Dict()
        let ok, value = b.TryGetValue "A"
        Expect.isFalse ok "TryGetValue should return false when the key does not exist"
        Expect.equal value 0 "TryGetValue should return the default value when the key does not exist"

    testCase "AsString - empty dictionary" <| fun _ ->
        let b = Dict<string, int>()
        let result = b.AsString
        Expect.equal result "empty Dict<String,Int32>" "AsString should return the correct string for an empty dictionary"

    testCase "AsString - single item" <| fun _ ->
        let b = Dict<string, int>()
        b.["A"] <- 1
        let result = b.AsString.Replace("\r\n", "\n").Replace("\n", "$")
        Expect.equal result "Dict<String,Int32> with 1 item:$  A : 1$" "AsString should return the correct string for a dictionary with one item"

    testCase "AsString - multiple items" <| fun _ ->
        let b = Dict<string, int>()
        b.["A"] <- 1
        b.["B"] <- 2
        b.["C"] <- 3
        b.["D"] <- 4
        b.["E"] <- 5
        b.["F"] <- 6
        let result = b.AsString.Replace("\r\n", "\n").Replace("\n", "$")
        Expect.equal result "Dict<String,Int32> with 6 items:$  A : 1$  B : 2$  C : 3$  D : 4$  E : 5$  ...$" "AsString should return the correct string for a dictionary with multiple items"


    // testCase "iEqualityComparer" <| fun _ ->
    //     let comparer = StringComparer.OrdinalIgnoreCase // not supported in Fable
    //     let b = Dict<string, int>(comparer)
    //     b.["A"] <- 1
    //     let result = b.ContainsKey "a"
    //     Expect.isTrue result "iEqualityComparer should allow case-insensitive key lookup"

    testCase "iEqualityComparer with object expression" <| fun _ ->
        let comparer =
            { new IEqualityComparer<string> with
                member _.Equals(x, y) = x.ToLowerInvariant() = y.ToLowerInvariant()
                member _.GetHashCode(obj) = obj.ToLowerInvariant().GetHashCode() }
        let b = Dict<string, int>(comparer)
        b.["A"] <- 1
        let result = b.ContainsKey "a"
        Expect.isTrue result "iEqualityComparer should allow case-insensitive key lookup with object expression"



    testCase "KV-Add" <| fun _ ->
        let b = Dict<string, int>()
        let kvp = KeyValuePair("A", 1)
        let iColl = (b :> ICollection<KeyValuePair<string, int>>)
        iColl.Add(kvp)
        Expect.equal (b.Get "A") 1 "Add should add the key-value pair to the dictionary"

    testCase "KV-Clear" <| fun _ ->
        let b = Dict<string, int>()
        b.Add("A", 1)
        (b :> ICollection<KeyValuePair<string, int>>).Clear()
        Expect.isFalse (b.ContainsKey "A") "Clear should remove all key-value pairs from the dictionary"


    // TODO still fails: https://fable.io/repl/#?code=PYBwpgdgBAygngZwC5gLYDoDCwA2OwDGSAlsBAugOKRgBOxBAUKJFAGICGARvlsLWHQApBAEkIKWqEaN8SKABMoAXigARBiTIdacADzEJAGkNIAfAAoAlLLDyCKxVABcZqKOx5CWiHoDSYHAAahw4AK5gAAocxLQGxlCmZmaMCugA2gCMALpQegC0UAAMMrbyAqjAAG5gnjgWANZVzgHBoRHRsfFIJhJmRooKzhpEpBA6+qa95laqjFALiQBmg3wSMeStUE3oWwBke6vpO625qjsh4WBQSAAWrPOLT08KaQBKaNXXJ4GPz1BgHAIMB-f6LJahYGgxhoYhIEQwJAcFCoSDyaxQABEFS+4kiOA4BGuqiWYQgozIjSqRgIRjAAEcrABvARIMK0aA4mp1Kk0qwAXwA3JjQWCoKU5Nsqo5WpcOjFaBZMgMijZJVywAo-EFHAR0B9KjUqTYQPQJEtoAASTGy9pRBVQADuHAQUA1QygLM+NS1QX5IvV3s1akc7yDSpNZqQFqg1q2CFuwDCOCUXGuAHMyGABlwwvJiK6CBxoGm3UGlBBgI7nJ73Wp-UA&html=DwCwLgtgNgfAsAKAAQqaApgQwCb2ag4CdMTJcMABwFp0BHAVwEsA3AXgCIBhAewDsw6AdQAqAT0roOSAMb9BAzoIAeYAPThoAbhkhMAJwDOJNgzAAzagA4OeQhqy5EhAEY9sYu6mBq3HvD6asEA&css=Q

    testCase "KV-Remove" <| fun _ ->
        let b = Dict<string, int>()
        b.Add("A", 1)
        let result = (b :> ICollection<KeyValuePair<string, int>>).Remove(KeyValuePair("A", 1))
        Expect.isTrue result "Remove should return true when the key-value pair is removed"
        Expect.isFalse (b.ContainsKey "A") "Remove should remove the key-value pair from the dictionary"

    testCase "KV-Contains" <| fun _ ->
        let b = Dict<string, int>()
        b.Add("A", 1)
        let result = (b :> ICollection<KeyValuePair<string, int>>).Contains(KeyValuePair("A", 1))
        Expect.isTrue result "Contains should return true when the key-value pair is in the dictionary"

    testCase "KV-CopyTo" <| fun _ ->
        let b = Dict<string, int>()
        b.Add("A", 1)
        let arr = Array.zeroCreate<KeyValuePair<string, int>> 1
        (b :> ICollection<KeyValuePair<string, int>>).CopyTo(arr, 0)
        Expect.equal arr.[0] (KeyValuePair("A", 1)) "CopyTo should copy the key-value pairs to the array"

    testCase "KV-IsReadOnly" <| fun _ ->
        let b = Dict<string, int>()
        let result = (b :> ICollection<KeyValuePair<string, int>>).IsReadOnly
        Expect.isFalse result "IsReadOnly should return false"

    testCase "KV-Count" <| fun _ ->
        let b = Dict<string, int>()
        b.Add("A", 1)
        let result = (b :> ICollection<KeyValuePair<string, int>>).Count
        Expect.equal result 1 "Count should return the number of key-value pairs in the dictionary"


    testCase "iDictionary - Add" <| fun _ ->
        let b = Dict<string,int>() :> IDictionary<string,int>
        b.Add("A", 1)
        let hasKey = b.ContainsKey("A")
        Expect.isTrue hasKey "IDictionary Add should add key-value pair"

    testCase "iDictionary - Remove" <| fun _ ->
        let b = Dict<string,int>() :> IDictionary<string,int>
        b.Add("A", 1)
        b.Remove("A") |> ignore
        let removed = not (b.ContainsKey("A"))
        Expect.isTrue removed "IDictionary Remove should remove key"

    testCase "iDictionary - Clear" <| fun _ ->
        let b = Dict<string,int>() :> IDictionary<string,int>
        b.Add("A", 1)
        b.Add("B", 2)
        b.Clear()
        let cleared = b.Count = 0
        Expect.isTrue cleared "IDictionary Clear should remove all items"

    testCase "iDictionary - Keys" <| fun _ ->
        let b = Dict<string,int>() :> IDictionary<string,int>
        b.Add("A", 1)
        b.Add("B", 2)
        let keys = b.Keys |> Seq.toList
        Expect.equal keys ["A"; "B"] "IDictionary Keys should return all keys"

    testCase "iDictionary - Values" <| fun _ ->
        let b = Dict<string,int>() :> IDictionary<string,int>
        b.Add("A", 1)
        b.Add("B", 2)
        let values = b.Values |> Seq.toList
        Expect.equal values [1; 2] "IDictionary Values should return all values"

    testCase "iDictionary - TryGetValue exists" <| fun _ ->
        let b = Dict<string,int>() :> IDictionary<string,int>
        b.Add("A", 1)
        let success, value = b.TryGetValue("A")
        Expect.isTrue success "TryGetValue should return true for existing key"
        Expect.equal value 1 "TryGetValue should return correct value"

    testCase "iDictionary - TryGetValue missing" <| fun _ ->
        let b = Dict<string,int>() :> IDictionary<string,int>
        let success, value = b.TryGetValue("missing")
        Expect.isFalse success "TryGetValue should return false for missing key"
        Expect.equal value 0 "TryGetValue should return default value"

    testCase "Remove - key does not exist" <| fun _ ->
        let b = Dict<string,int>()
        let result = b.Remove "NonExistentKey"
        Expect.isFalse result "Remove should return false when key does not exist"

    testCase "Remove - key exists" <| fun _ ->
        let b = Dict<string,int>()
        b.["A"] <- 1
        let result = b.Remove "A"
        Expect.isTrue result "Remove should return true when key exists"
        Expect.isFalse (b.ContainsKey "A") "Remove should remove the key"

    testCase "Remove - multiple keys" <| fun _ ->
        let b = Dict<string,int>()
        b.["A"] <- 1
        b.["B"] <- 2
        b.["C"] <- 3
        let result1 = b.Remove "A"
        let result2 = b.Remove "B"
        Expect.isTrue (result1 && result2) "Remove should return true for existing keys"
        Expect.equal b.Count 1 "Remove should decrease count accordingly"

    testCase "Get - key exists" <| fun _ ->
        let b = Dict<string,int>()
        b.["A"] <- 1
        let result = b["A" ]
        Expect.equal result 1 "Get should return value for existing key"

    testCase "Get - key does not exist" <| fun _ ->
        let b = Dict<string,int>()
        Expect.throws (fun () -> b.Get "A" |> ignore) "Get should throw for missing key"

    testCase "Set - add new key" <| fun _ ->
        let b = Dict<string,int>()
        b.Set "A" 1
        let result = b["A" ]
        Expect.equal result 1 "Set should add new key-value pair"

    testCase "Set - update existing key" <| fun _ ->
        let b = Dict<string,int>()
        b.["A"] <- 1
        b.Set "A" 2
        let result = b.Get "A"
        Expect.equal result 2 "Set should update value for existing key"

    testCase "Set - null key" <| fun _ ->
        let b = Dict<string,int>()
        Expect.throws (fun () -> b[null ] <- 1) "Set should throw for null key"



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

    testCase "DefaultDict" <| fun _ ->
        let b = DefaultDict(fun _ -> 0)
        let result = b.Get "A"
        Expect.equal result 0 "DefaultDict should return the default value when the key does not exist"

    testCase "DefaultDict - key exists" <| fun _ ->
        let b = DefaultDict(fun _ -> 0)
        b.["A"] <- 1
        let result = b.Get "A"
        Expect.equal result 1 "DefaultDict should return the existing value when the key exists"

    testCase "DefaultDict - default value incr" <| fun _ ->
        let b = DefaultDict(fun _ -> ref 0)
        incr (b.["A"])
        Expect.equal b.["A"].Value  1 "DefaultDict should return the default value when the key does not exist"

    testCase "DefaultDict - key exists - default value" <| fun _ ->
        let b = DefaultDict(fun _ -> ref 0)
        b.["A"] <- ref 1
        incr (b.Get "A")
        Expect.equal b.["A"].Value  2 "DefaultDict should return the existing value when the key exists"

    testCase "DefaultDict - default value - ref" <| fun _ ->
        let b = DefaultDict(fun _ -> ref 0)
        incr (b.Get "A")
        Expect.equal b.["A"].Value  1 "DefaultDict should return the default value when the key does not exist"

    testCase "DefaultDict - Pop" <| fun _ ->
        let b = DefaultDict(fun _ -> 0)
        b.["A"] <- 1
        let popped  = b.Pop "A"
        let result = b.DoesNotContainKey "A" && popped = 1
        Expect.isTrue result "Pop removed key A"


    testCase "DefaultDict - default value" <| fun _ ->
            let b = DefaultDict(fun _ -> 0)
            let result = b.Get "A"
            Expect.equal result 0 "DefaultDict should return the default value when the key does not exist"

    testCase "DefaultDict - set value" <| fun _ ->
            let b = DefaultDict(fun _ -> 0)
            b.["A"] <- 1
            let result = b.Get "A"
            Expect.equal result 1 "DefaultDict should allow setting a value for a key"

    testCase "DefaultDict - remove key" <| fun _ ->
            let b = DefaultDict(fun _ -> 0)
            b.["A"] <- 1
            b.Remove "A" |> ignore
            let result = b.ContainsKey "A"
            Expect.isFalse result "DefaultDict should remove the key"

    testCase "DefaultDict - clear" <| fun _ ->
            let b = DefaultDict(fun _ -> 0)
            b.["A"] <- 1
            b.["B"] <- 2
            b.Clear()
            let resultA = b.ContainsKey "A"
            let resultB = b.ContainsKey "B"
            Expect.isFalse resultA "DefaultDict should clear all keys"
            Expect.isFalse resultB "DefaultDict should clear all keys"

    testCase "DefaultDict - contains key" <| fun _ ->
            let b = DefaultDict(fun _ -> 0)
            b.["A"] <- 1
            let result = b.ContainsKey "A"
            Expect.isTrue result "DefaultDict should return true for existing key"

    testCase "DefaultDict - count" <| fun _ ->
            let b = DefaultDict(fun _ -> 0)
            b.["A"] <- 1
            b.["B"] <- 2
            let result = b.Count
            Expect.equal result 2 "DefaultDict should return the number of key-value pairs"

    testCase "DefaultDict - keys" <| fun _ ->
            let b = DefaultDict(fun _ -> 0)
            b.["A"] <- 1
            b.["B"] <- 2
            let keys = b.Keys |> Seq.toList
            Expect.equal keys ["A"; "B"] "DefaultDict should return the keys"

    testCase "DefaultDict - values" <| fun _ ->
            let b = DefaultDict(fun _ -> 0)
            b.["A"] <- 1
            b.["B"] <- 2
            let values = b.Values |> Seq.toList
            Expect.equal values [1; 2] "DefaultDict should return the values"

    testCase "DefaultDict - pop key" <| fun _ ->
            let b = DefaultDict(fun _ -> 0)
            b.["A"] <- 1
            let value = b.Pop "A"
            Expect.equal value 1 "DefaultDict should return the value when popping a key"
            let result = b.ContainsKey "A"
            Expect.isFalse result "DefaultDict should remove the key when popping"

    testCase "DefaultDict - AsString" <| fun _ ->
            let b = DefaultDict(fun _ -> 0)
            b.["A"] <- 1
            b.["B"] <- 2
            let result = b.AsString.Replace("\r\n", "\n").Replace("\n", "$")
            Expect.equal result "DefaultDict<String,Int32> with 2 items:$  A : 1$  B : 2$" "DefaultDict should return the correct string representation with items"

    testCase "DefaultDict - KV-Add" <| fun _ ->
        let b = DefaultDict(fun _ -> 0)
        let kvp = KeyValuePair("A", 1)
        let iColl = (b :> ICollection<KeyValuePair<string, int>>)
        iColl.Add(kvp)
        Expect.equal (b.Get "A") 1 "Add should add the key-value pair to the dictionary"

    testCase "DefaultDict - KV-Clear" <| fun _ ->
        let b = DefaultDict(fun _ -> 0)
        b.Add("A", 1)
        (b :> ICollection<KeyValuePair<string, int>>).Clear()
        Expect.isFalse (b.ContainsKey "A") "Clear should remove all key-value pairs"

    testCase "DefaultDict - KV-Remove" <| fun _ ->
        let b = DefaultDict(fun _ -> 0)
        b.Add("A", 1)
        let result = (b :> ICollection<KeyValuePair<string, int>>).Remove(KeyValuePair("A", 1))
        Expect.isTrue result "Remove should return true when pair removed"
        Expect.isFalse (b.ContainsKey "A") "Remove should remove the key-value pair"

    testCase "DefaultDict - KV-Contains" <| fun _ ->
        let b = DefaultDict(fun _ -> 0)
        b.Add("A", 1)
        let result = (b :> ICollection<KeyValuePair<string, int>>).Contains(KeyValuePair("A", 1))
        Expect.isTrue result "Contains should return true for existing pair"

    testCase "DefaultDict - KV-CopyTo" <| fun _ ->
        let b = DefaultDict(fun _ -> 0)
        b.Add("A", 1)
        let arr = Array.zeroCreate<KeyValuePair<string, int>> 1
        (b :> ICollection<KeyValuePair<string, int>>).CopyTo(arr, 0)
        Expect.equal arr.[0] (KeyValuePair("A", 1)) "CopyTo should copy pairs to array"

    testCase "DefaultDict - KV-IsReadOnly" <| fun _ ->
        let b = DefaultDict(fun _ -> 0)
        let result = (b :> ICollection<KeyValuePair<string, int>>).IsReadOnly
        Expect.isFalse result "IsReadOnly should return false"

    testCase "DefaultDict - KV-Count" <| fun _ ->
        let b = DefaultDict(fun _ -> 0)
        b.Add("A", 1)
        let result = (b :> ICollection<KeyValuePair<string, int>>).Count
        Expect.equal result 1 "Count should return number of pairs"

    testCase "Dict ReadOnly collection" <| fun _ ->
        let b = Dict<string,int>()
        b.["A"] <- 1
        b.["B"] <- 2
        let ro = b//.AsReadOnly() is not supported by Fable
        let result = ro :> IReadOnlyCollection<KeyValuePair<string,int>>
        Expect.equal result.Count 2 "ReadOnly collection count should match original"

    testCase "Dict ReadOnly collection content" <| fun _ ->
        let b = Dict<string,int>()
        b.["A"] <- 1
        b.["B"] <- 2
        let ro = b//.AsReadOnly() is not supported by Fable
        let pairs = ro |> Seq.map (fun kvp -> kvp.Key, kvp.Value) |> Set.ofSeq
        let expected = set [("A",1); ("B",2)]
        Expect.equal pairs expected "ReadOnly collection should have same content"

    testCase "DefaultDict ReadOnly collection" <| fun _ ->
        let b = DefaultDict(fun _ -> 8)
        b.["A"] <- 1
        b.["B"] <- 2
        let result = b :> IReadOnlyCollection<KeyValuePair<string,int>>
        Expect.equal result.Count 2 "ReadOnly collection count should match original"

    testCase "DefaultDict ReadOnly collection content" <| fun _ ->
        let b = DefaultDict(fun _ -> 8)
        b.["A"] <- 1
        b.["B"] <- 2
        let ro = b
        let pairs = ro |> Seq.map (fun kvp -> kvp.Key, kvp.Value) |> Set.ofSeq
        let expected = set [("A",1); ("B",2)]
        Expect.equal pairs expected "ReadOnly collection should have same content"

    // IDictionary interface is removed from DefaultDict becaus the semantics don't fit
    // don't add IDictionary because of TryGetValue might return might no Value while get would.
    // this is not consistent with the IDictionary interface

    // testCase "iDictionary DefDict - Get item" <| fun _ ->
    //     let b = DefaultDict(fun _ -> 0) :> IDictionary<string,int>
    //     let d = b.["A"]
    //     let hasKey = b.ContainsKey("A")
    //     Expect.isTrue (hasKey && d=0) "IDictionary Get should add key-value pair"

    // testCase "iDictionary DefDict - Get method" <| fun _ ->
    //     let b = DefaultDict(fun _ -> 0) :> IDictionary<string,int>
    //     let d = b.Get "A"
    //     let hasKey = b.ContainsKey("A")
    //     Expect.isTrue (hasKey && d=0) "IDictionary Get should add key-value pair"


    // testCase "iDictionary DefDict - Add" <| fun _ ->
    //     let b = DefaultDict(fun _ -> 0) :> IDictionary<string,int>
    //     b.Add("A", 1)
    //     let hasKey = b.ContainsKey("A")
    //     Expect.isTrue hasKey "IDictionary Add should add key-value pair"

    // testCase "iDictionary DefDict - Remove" <| fun _ ->
    //     let b = DefaultDict(fun _ -> 0) :> IDictionary<string,int>
    //     b.Add("A", 1)
    //     b.Remove("A") |> ignore
    //     let removed = not (b.ContainsKey("A"))
    //     Expect.isTrue removed "IDictionary Remove should remove key"

    // testCase "iDictionary DefDict - Clear" <| fun _ ->
    //     let b = DefaultDict(fun _ -> 0) :> IDictionary<string,int>
    //     b.Add("A", 1)
    //     b.Add("B", 2)
    //     b.Clear()
    //     let cleared = b.Count = 0
    //     Expect.isTrue cleared "IDictionary Clear should remove all items"

    // testCase "iDictionary DefDict - TryGetValue exists for add" <| fun _ ->
    //     let b = DefaultDict(fun _ -> 3) :> IDictionary<string,int>
    //     b.Add("A", 8)
    //     let success, value = b.TryGetValue("A")
    //     Expect.isTrue success "TryGetValue should return true for existing key"
    //     Expect.equal value 8 "TryGetValue should return correct value"

    // testCase "iDictionary DefDict - TryGetValue exists for <- " <| fun _ ->
    //     let dd = DefaultDict(fun _ -> 3)
    //     dd["A"] <- 8
    //     let b = dd :> IDictionary<string,int>
    //     let success, value = b.TryGetValue("A")
    //     Expect.isTrue success "TryGetValue should return true for existing key"
    //     Expect.equal value 8 "TryGetValue should return correct value"


    // testCase "iDictionary DefDict - TryGetValue missing" <| fun _ ->
    //     let b = DefaultDict(fun _ -> 0) :> IDictionary<string,int>
    //     let success, _ = b.TryGetValue("missing")
    //     Expect.isFalse success "TryGetValue should return true even for missing key"





    // ---------------------------------------------------------
    // Dict Module Tests:
    // ---------------------------------------------------------

    testCase "Dict.memoize - caches function results" <| fun _ ->
        let callCount = ref 0
        let expensiveFunc x =
            incr callCount
            x * 2

        let memoizedFunc = Dict.memoize expensiveFunc

        // First call should execute the function
        let result1 = memoizedFunc 5
        Expect.equal result1 10 "Function should return correct result"
        Expect.equal !callCount 1 "Function should be called once"

        // Second call with same input should use cached result
        let result2 = memoizedFunc 5
        Expect.equal result2 10 "Function should return correct result"
        Expect.equal !callCount 1 "Function should not be called again"

        // Call with different input should execute function again
        let result3 = memoizedFunc 7
        Expect.equal result3 14 "Function should return correct result"
        Expect.equal !callCount 2 "Function should be called for new input"


    testCase "Dict.memoize - null" <| fun _ ->
        let unitf () = 5
        let memoizedFunc = Dict.memoize unitf
        let result = memoizedFunc ()
        Expect.equal result 5 "Function should return correct result"

        let nullF (_x:obj) = 6
        let memoizedFunc = Dict.memoize nullF
        let result = memoizedFunc null
        Expect.equal result 6 "Function should return correct result"

        let noneF (_x:Option<int>) = 7
        let memoizedFunc = Dict.memoize noneF
        let result = memoizedFunc None
        Expect.equal result 7 "Function should return correct result"


    testCase "Dict.get - retrieves value for existing key" <| fun _ ->
        let dict = Dict<string, int>()
        dict.["test"] <- 42

        let result = Dict.get "test" dict
        Expect.equal result 42 "Should retrieve correct value"

    testCase "Dict.get - throws for non-existent key" <| fun _ ->
        let dict = Dict<string, int>()
        Expect.throws (fun () -> Dict.get "missing" dict |> ignore)
            "Should throw KeyNotFoundException"

    testCase "Dict.set - sets value for key" <| fun _ ->
        let dict = Dict<string, int>()
        Dict.set "test" 42 dict

        Expect.equal dict.["test"] 42 "Should set correct value"

    testCase "Dict.tryGet - returns Some for existing key" <| fun _ ->
        let dict = Dict<string, int>()
        dict.["test"] <- 42

        let result = Dict.tryGet "test" dict
        Expect.equal result (Some 42) "Should return Some with correct value"

    testCase "Dict.tryGet - returns None for non-existent key" <| fun _ ->
        let dict = Dict<string, int>()

        let result = Dict.tryGet "missing" dict
        Expect.equal result None "Should return None"

    testCase "Dict.create - creates dictionary from key-value pairs" <| fun _ ->
        let pairs = [("a", 1); ("b", 2); ("c", 3)]
        let dict = Dict.create pairs

        Expect.equal dict.Count 3 "Dictionary should have correct count"
        Expect.equal dict.["a"] 1 "Dictionary should contain correct values"
        Expect.equal dict.["b"] 2 "Dictionary should contain correct values"
        Expect.equal dict.["c"] 3 "Dictionary should contain correct values"

    testCase "Dict.setIfKeyAbsent - adds when key doesn't exist" <| fun _ ->
        let dict = Dict<string, int>()

        let result = Dict.setIfKeyAbsent "test" 42 dict
        Expect.isTrue result "Should return true when key doesn't exist"
        Expect.equal dict.["test"] 42 "Should set the value"

    testCase "Dict.setIfKeyAbsent - doesn't add when key exists" <| fun _ ->
        let dict = Dict<string, int>()
        dict.["test"] <- 42

        let result = Dict.setIfKeyAbsent "test" 84 dict
        Expect.isFalse result "Should return false when key exists"
        Expect.equal dict.["test"] 42 "Should not change the value"

    testCase "Dict.addIfKeyAbsent - adds when key doesn't exist" <| fun _ ->
        let dict = Dict<string, int>()

        let result = Dict.addIfKeyAbsent "test" 42 dict
        Expect.isTrue result "Should return true when key doesn't exist"
        Expect.equal dict.["test"] 42 "Should add the value"

    testCase "Dict.addIfKeyAbsent - doesn't add when key exists" <| fun _ ->
        let dict = Dict<string, int>()
        dict.["test"] <- 42

        let result = Dict.addIfKeyAbsent "test" 84 dict
        Expect.isFalse result "Should return false when key exists"
        Expect.equal dict.["test"] 42 "Should not change the value"

    testCase "Dict.getOrSetDefault - returns existing value for existing key" <| fun _ ->
        let dict = Dict<string, int>()
        dict.["test"] <- 42

        let result = Dict.getOrSetDefault (fun _ -> 99) "test" dict
        Expect.equal result 42 "Should return existing value"
        Expect.equal dict.["test"] 42 "Should not change existing value"

    testCase "Dict.getOrSetDefault - sets default for missing key" <| fun _ ->
        let dict = Dict<string, int>()

        let result = Dict.getOrSetDefault (fun _k -> 4) "test" dict
        Expect.equal result 4 "Should return default value based on key"
        Expect.equal dict.["test"] 4 "Should set default value in dictionary"

    testCase "Dict.getOrSetDefaultValue - returns existing value for existing key" <| fun _ ->
        let dict = Dict<string, int>()
        dict.["test"] <- 42

        let result = Dict.getOrSetDefaultValue 99 "test" dict
        Expect.equal result 42 "Should return existing value"
        Expect.equal dict.["test"] 42 "Should not change existing value"

    testCase "Dict.getOrSetDefaultValue - sets default for missing key" <| fun _ ->
        let dict = Dict<string, int>()

        let result = Dict.getOrSetDefaultValue 99 "test" dict
        Expect.equal result 99 "Should return default value"
        Expect.equal dict.["test"] 99 "Should set default value in dictionary"

    testCase "Dict.tryPop - returns Some and removes for existing key" <| fun _ ->
        let dict = Dict<string, int>()
        dict.["test"] <- 42

        let result = Dict.tryPop "test" dict
        Expect.equal result (Some 42) "Should return Some with value"
        Expect.isFalse (dict.ContainsKey "test") "Should remove key from dictionary"

    testCase "Dict.tryPop - returns None for non-existent key" <| fun _ ->
        let dict = Dict<string, int>()

        let result = Dict.tryPop "missing" dict
        Expect.equal result None "Should return None for missing key"

    testCase "Dict.pop - returns value and removes for existing key" <| fun _ ->
        let dict = Dict<string, int>()
        dict.["test"] <- 42

        let result = Dict.pop "test" dict
        Expect.equal result 42 "Should return value"
        Expect.isFalse (dict.ContainsKey "test") "Should remove key from dictionary"

    testCase "Dict.pop - throws for non-existent key" <| fun _ ->
        let dict = Dict<string, int>()

        Expect.throws (fun () -> Dict.pop "missing" dict |> ignore)
            "Should throw KeyNotFoundException"

    testCase "Dict.items - returns sequence of key-value pairs" <| fun _ ->
        let dict = Dict<string, int>()
        dict.["a"] <- 1
        dict.["b"] <- 2

        let items = Dict.items dict |> Seq.toList |> List.sortBy fst
        Expect.equal items [("a", 1); ("b", 2)] "Should return correct key-value pairs"

    testCase "Dict.values - returns sequence of values" <| fun _ ->
        let dict = Dict<string, int>()
        dict.["a"] <- 1
        dict.["b"] <- 2

        let values = Dict.values dict |> Seq.toList |> List.sort
        Expect.equal values [1; 2] "Should return correct values"

    testCase "Dict.keys - returns sequence of keys" <| fun _ ->
        let dict = Dict<string, int>()
        dict.["a"] <- 1
        dict.["b"] <- 2

        let keys = Dict.keys dict |> Seq.toList |> List.sort
        Expect.equal keys ["a"; "b"] "Should return correct keys"

    testCase "Dict.iter - iterates over dictionary" <| fun _ ->
        let dict = Dict<string, int>()
        dict.["a"] <- 1
        dict.["b"] <- 2

        let result = ref []
        Dict.iter (fun k v -> result := (k, v) :: !result) dict
        let sorted = !result |> List.sortBy fst

        Expect.equal sorted [("a", 1); ("b", 2)] "Should iterate over all key-value pairs"

    testCase "Dict.map - maps dictionary to sequence" <| fun _ ->
        let dict = Dict<string, int>()
        dict.["a"] <- 1
        dict.["b"] <- 2

        let result = Dict.map (fun k v -> k + string v) dict |> Seq.toList |> List.sort
        Expect.equal result ["a1"; "b2"] "Should map key-value pairs correctly"


    // =============================================================
    // Fable/JS parity tests:
    // Tests for functions with FABLE_COMPILER_JAVASCRIPT directives
    // to ensure .NET and JS runtimes behave the same way.
    // =============================================================

    // ---------------------------------------------------------
    // Dict - Core operations (routed through IJSMap in Fable)
    // ---------------------------------------------------------

    testCase "Dict-Fable - Get on empty dict throws" <| fun _ ->
        let d = Dict<string, int>()
        Expect.throws (fun () -> d.Get "anything" |> ignore) "Get on empty dict should throw"

    testCase "Dict-Fable - Set and Get with int keys (value type)" <| fun _ ->
        let d = Dict<int, string>()
        d.Set 1 "one"
        d.Set 2 "two"
        d.Set 3 "three"
        Expect.equal (d.Get 1) "one" "Int key 1"
        Expect.equal (d.Get 2) "two" "Int key 2"
        Expect.equal (d.Get 3) "three" "Int key 3"

    testCase "Dict-Fable - Set and Get with float keys (value type)" <| fun _ ->
        let d = Dict<float, string>()
        d.Set 1.5 "a"
        d.Set 2.5 "b"
        Expect.equal (d.Get 1.5) "a" "Float key 1.5"
        Expect.equal (d.Get 2.5) "b" "Float key 2.5"

    testCase "Dict-Fable - Set and Get with bool keys (value type)" <| fun _ ->
        let d = Dict<bool, string>()
        d.Set true "yes"
        d.Set false "no"
        Expect.equal (d.Get true) "yes" "Bool key true"
        Expect.equal (d.Get false) "no" "Bool key false"
        Expect.equal d.Count 2 "Should have 2 entries"

    testCase "Dict-Fable - Set and Get with tuple keys" <| fun _ ->
        let d = Dict<(int * string), float>()
        d.Set (1, "a") 10.0
        d.Set (2, "b") 20.0
        Expect.equal (d.Get (1, "a")) 10.0 "Tuple key (1,a)"
        Expect.equal (d.Get (2, "b")) 20.0 "Tuple key (2,b)"

    testCase "Dict-Fable - Set overwrites existing value" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        d.Set "A" 2
        Expect.equal (d.Get "A") 2 "Set should overwrite"
        Expect.equal d.Count 1 "Count should still be 1"

    testCase "Dict-Fable - Item indexer set overwrites" <| fun _ ->
        let d = Dict<string, int>()
        d.["X"] <- 10
        d.["X"] <- 20
        Expect.equal d.["X"] 20 "Item indexer should overwrite"
        Expect.equal d.Count 1 "Count should still be 1"

    testCase "Dict-Fable - null key throws on Set" <| fun _ ->
        let d = Dict<string, int>()
        Expect.throws (fun () -> d.Set null 1) "Set null key should throw"

    testCase "Dict-Fable - null key throws on Get" <| fun _ ->
        let d = Dict<string, int>()
        Expect.throws (fun () -> d.Get null |> ignore) "Get null key should throw"

    testCase "Dict-Fable - null key throws on Item get" <| fun _ ->
        let d = Dict<string, int>()
        Expect.throws (fun () -> d.[null] |> ignore) "Item get null key should throw"

    testCase "Dict-Fable - null key throws on Item set" <| fun _ ->
        let d = Dict<string, int>()
        Expect.throws (fun () -> d.[null] <- 1) "Item set null key should throw"

    testCase "Dict-Fable - null key throws on Pop" <| fun _ ->
        let d = Dict<string, int>()
        Expect.throws (fun () -> d.Pop null |> ignore) "Pop null key should throw"

    testCase "Dict-Fable - null key throws on TryPop" <| fun _ ->
        let d = Dict<string, int>()
        Expect.throws (fun () -> d.TryPop null |> ignore) "TryPop null key should throw"

    testCase "Dict-Fable - null key throws on SetIfKeyAbsent" <| fun _ ->
        let d = Dict<string, int>()
        Expect.throws (fun () -> d.SetIfKeyAbsent null 1 |> ignore) "SetIfKeyAbsent null key should throw"

    testCase "Dict-Fable - null key throws on AddIfKeyAbsent" <| fun _ ->
        let d = Dict<string, int>()
        Expect.throws (fun () -> d.AddIfKeyAbsent null 1 |> ignore) "AddIfKeyAbsent null key should throw"

    testCase "Dict-Fable - null key throws on GetOrSetDefault" <| fun _ ->
        let d = Dict<string, int>()
        Expect.throws (fun () -> d.GetOrSetDefault (fun _ -> 0) null |> ignore) "GetOrSetDefault null key should throw"

    testCase "Dict-Fable - null key throws on GetOrSetDefaultValue" <| fun _ ->
        let d = Dict<string, int>()
        Expect.throws (fun () -> d.GetOrSetDefaultValue 0 null |> ignore) "GetOrSetDefaultValue null key should throw"

    testCase "Dict-Fable - ContainsKey on empty" <| fun _ ->
        let d = Dict<string, int>()
        Expect.isFalse (d.ContainsKey "A") "Empty dict should not contain any key"

    testCase "Dict-Fable - ContainsKey after Set" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        Expect.isTrue (d.ContainsKey "A") "Should contain key after Set"
        Expect.isFalse (d.ContainsKey "B") "Should not contain absent key"

    testCase "Dict-Fable - DoesNotContainKey" <| fun _ ->
        let d = Dict<string, int>()
        Expect.isTrue (d.DoesNotContainKey "A") "Empty dict DoesNotContainKey"
        d.Set "A" 1
        Expect.isFalse (d.DoesNotContainKey "A") "Should not say DoesNotContainKey for present key"

    testCase "Dict-Fable - Remove returns false for missing key" <| fun _ ->
        let d = Dict<string, int>()
        Expect.isFalse (d.Remove "missing") "Remove missing key returns false"

    testCase "Dict-Fable - Remove returns true and removes key" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        let removed = d.Remove "A"
        Expect.isTrue removed "Remove existing key returns true"
        Expect.isFalse (d.ContainsKey "A") "Key should be gone after Remove"
        Expect.equal d.Count 0 "Count should be 0 after removing only key"

    testCase "Dict-Fable - Pop existing key" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 42
        let v = d.Pop "A"
        Expect.equal v 42 "Pop should return value"
        Expect.isFalse (d.ContainsKey "A") "Pop should remove key"

    testCase "Dict-Fable - Pop missing key throws" <| fun _ ->
        let d = Dict<string, int>()
        Expect.throws (fun () -> d.Pop "missing" |> ignore) "Pop missing key should throw"

    testCase "Dict-Fable - TryPop existing key" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 42
        let v = d.TryPop "A"
        Expect.equal v (Some 42) "TryPop existing key returns Some"
        Expect.isFalse (d.ContainsKey "A") "TryPop should remove key"

    testCase "Dict-Fable - TryPop missing key returns None" <| fun _ ->
        let d = Dict<string, int>()
        let v = d.TryPop "missing"
        Expect.equal v None "TryPop missing key returns None"

    testCase "Dict-Fable - Clear on empty dict" <| fun _ ->
        let d = Dict<string, int>()
        d.Clear()
        Expect.equal d.Count 0 "Clear on empty dict should work"

    testCase "Dict-Fable - Clear removes all entries" <| fun _ ->
        let d = Dict<string, int>()
        for i in 1..100 do
            d.Set (string i) i
        Expect.equal d.Count 100 "Should have 100 entries"
        d.Clear()
        Expect.equal d.Count 0 "Clear should remove all entries"
        Expect.isTrue d.IsEmpty "Should be empty after Clear"

    testCase "Dict-Fable - IsEmpty and IsNotEmpty" <| fun _ ->
        let d = Dict<string, int>()
        Expect.isTrue d.IsEmpty "New dict is empty"
        Expect.isFalse d.IsNotEmpty "New dict is not not-empty"
        d.Set "A" 1
        Expect.isFalse d.IsEmpty "Dict with entry is not empty"
        Expect.isTrue d.IsNotEmpty "Dict with entry is not-empty"

    testCase "Dict-Fable - Count tracks additions and removals" <| fun _ ->
        let d = Dict<string, int>()
        Expect.equal d.Count 0 "Initial count is 0"
        d.Set "A" 1
        Expect.equal d.Count 1 "Count after 1 add"
        d.Set "B" 2
        Expect.equal d.Count 2 "Count after 2 adds"
        d.Set "A" 99  // overwrite, no new key
        Expect.equal d.Count 2 "Count after overwrite stays same"
        d.Remove "A" |> ignore
        Expect.equal d.Count 1 "Count after remove"

    testCase "Dict-Fable - Keys returns all keys" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "X" 1
        d.Set "Y" 2
        d.Set "Z" 3
        let keys = d.Keys |> Seq.toList |> List.sort
        Expect.equal keys ["X"; "Y"; "Z"] "Keys should return all keys"

    testCase "Dict-Fable - Values returns all values" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "X" 10
        d.Set "Y" 20
        let values = d.Values |> Seq.toList |> List.sort
        Expect.equal values [10; 20] "Values should return all values"

    testCase "Dict-Fable - Items returns tuples" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        d.Set "B" 2
        let items = d.Items |> Seq.toList |> List.sortBy fst
        Expect.equal items [("A", 1); ("B", 2)] "Items should return key-value tuples"

    testCase "Dict-Fable - TryGetValue existing" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "K" 99
        let ok, v = d.TryGetValue "K"
        Expect.isTrue ok "TryGetValue should return true for existing key"
        Expect.equal v 99 "TryGetValue should return correct value"

    testCase "Dict-Fable - TryGetValue missing" <| fun _ ->
        let d = Dict<string, int>()
        let ok, v = d.TryGetValue "missing"
        Expect.isFalse ok "TryGetValue should return false for missing key"
        Expect.equal v 0 "TryGetValue should return default int value"

    testCase "Dict-Fable - TryGetValue missing string value" <| fun _ ->
        let d = Dict<int, string>()
        let ok, v = d.TryGetValue 42
        Expect.isFalse ok "TryGetValue should return false for missing key"
        Expect.equal v null "TryGetValue should return null for missing string value"

    testCase "Dict-Fable - null string value is allowed" <| fun _ ->
        let d = Dict<string, string>()
        d.Set "key" null
        Expect.equal (d.Get "key") null "Null string value should be stored and retrieved"
        Expect.isTrue (d.ContainsKey "key") "Key with null value should exist"

    testCase "Dict-Fable - empty string key" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "" 42
        Expect.equal (d.Get "") 42 "Empty string key should work"
        Expect.isTrue (d.ContainsKey "") "Empty string key should be found"

    testCase "Dict-Fable - many entries" <| fun _ ->
        let d = Dict<int, int>()
        for i in 0..999 do
            d.Set i (i * i)
        Expect.equal d.Count 1000 "Should have 1000 entries"
        Expect.equal (d.Get 0) 0 "First entry"
        Expect.equal (d.Get 999) (999 * 999) "Last entry"
        Expect.equal (d.Get 500) (500 * 500) "Middle entry"

    testCase "Dict-Fable - duplicate Add via indexer" <| fun _ ->
        let d = Dict<string, int>()
        d.["A"] <- 1
        d.["A"] <- 2
        Expect.equal d.["A"] 2 "Second set should win"
        Expect.equal d.Count 1 "No duplicate keys"

    testCase "Dict-Fable - Add method on duplicate key throws" <| fun _ ->
        let d = Dict<string, int>()
        d.Add("A", 1)
        // Add via IDictionary should throw on duplicate
        let iDict = d :> IDictionary<string, int>
        Expect.throws (fun () -> iDict.Add("A", 2)) "IDictionary.Add should throw on duplicate key"

    testCase "Dict-Fable - enumerate empty dict" <| fun _ ->
        let d = Dict<string, int>()
        let items = d |> Seq.toList
        Expect.equal items [] "Enumerating empty dict should yield empty list"

    testCase "Dict-Fable - enumerate dict" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        d.Set "B" 2
        let items = d |> Seq.map (fun kvp -> kvp.Key, kvp.Value) |> Seq.toList |> List.sortBy fst
        Expect.equal items [("A", 1); ("B", 2)] "Enumerating dict should yield all entries"

    testCase "Dict-Fable - ContainsValue" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 42
        Expect.isTrue (d.ContainsValue 42) "ContainsValue should find existing value"
        Expect.isFalse (d.ContainsValue 99) "ContainsValue should not find missing value"

    testCase "Dict-Fable - KeysSeq and ValuesSeq" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        d.Set "B" 2
        let ks = d.KeysSeq |> Seq.toList |> List.sort
        let vs = d.ValuesSeq |> Seq.toList |> List.sort
        Expect.equal ks ["A"; "B"] "KeysSeq should return all keys"
        Expect.equal vs [1; 2] "ValuesSeq should return all values"

    testCase "Dict-Fable - SetIfKeyAbsent on empty" <| fun _ ->
        let d = Dict<string, int>()
        let r = d.SetIfKeyAbsent "A" 1
        Expect.isTrue r "Should return true on empty dict"
        Expect.equal (d.Get "A") 1 "Value should be set"

    testCase "Dict-Fable - SetIfKeyAbsent does not overwrite" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        let r = d.SetIfKeyAbsent "A" 2
        Expect.isFalse r "Should return false when key exists"
        Expect.equal (d.Get "A") 1 "Original value should be preserved"

    testCase "Dict-Fable - GetOrSetDefault creates and returns default" <| fun _ ->
        let d = Dict<string, int>()
        let v = d.GetOrSetDefault (fun k -> k.Length) "hello"
        Expect.equal v 5 "Default function should use the key"
        Expect.equal (d.Get "hello") 5 "Value should be stored"

    testCase "Dict-Fable - GetOrSetDefault does not overwrite existing" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "hello" 42
        let v = d.GetOrSetDefault (fun k -> k.Length) "hello"
        Expect.equal v 42 "Should return existing value"

    testCase "Dict-Fable - GetOrSetDefaultValue creates value" <| fun _ ->
        let d = Dict<string, int>()
        let v = d.GetOrSetDefaultValue 99 "key"
        Expect.equal v 99 "Should return default value"
        Expect.equal (d.Get "key") 99 "Should be stored"

    testCase "Dict-Fable - reference type values" <| fun _ ->
        let d = Dict<string, int list>()
        d.Set "A" [1; 2; 3]
        d.Set "B" []
        Expect.equal (d.Get "A") [1; 2; 3] "List value should be stored"
        Expect.equal (d.Get "B") [] "Empty list value should be stored"

    testCase "Dict-Fable - create from pairs" <| fun _ ->
        let d = Dict.create [("a", 1); ("b", 2); ("c", 3)]
        Expect.equal d.Count 3 "Should have 3 items"
        Expect.equal (d.Get "a") 1 "Value a"
        Expect.equal (d.Get "b") 2 "Value b"
        Expect.equal (d.Get "c") 3 "Value c"

    testCase "Dict-Fable - create from empty list" <| fun _ ->
        let d = Dict.create []
        Expect.equal d.Count 0 "Should be empty"
        Expect.isTrue d.IsEmpty "Should be empty"

    // ---------------------------------------------------------
    // Dict - AsString / ToString (FABLE_COMPILER branching)
    // ---------------------------------------------------------

    testCase "Dict-Fable - AsString empty" <| fun _ ->
        let d = Dict<string, int>()
        let s = d.AsString
        Expect.isTrue (s.Contains "Dict") "AsString should contain Dict"
        Expect.isTrue (s.Contains "0" || s.Contains "empty") "AsString on empty should indicate empty"

    testCase "Dict-Fable - AsString with items" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        let s = d.AsString
        Expect.isTrue (s.Contains "A") "AsString should contain key"
        Expect.isTrue (s.Contains "1") "AsString should contain value"

    testCase "Dict-Fable - AsString with more than 5 items shows ellipsis" <| fun _ ->
        let d = Dict<string, int>()
        for i in 1..7 do
            d.Set (string (char (64 + i))) i
        let s = d.AsString
        Expect.isTrue (s.Contains "...") "AsString with >5 items should show ellipsis"

    testCase "Dict-Fable - AsString with exactly 5 items no ellipsis" <| fun _ ->
        let d = Dict<string, int>()
        for i in 1..5 do
            d.Set (string (char (64 + i))) i
        let s = d.AsString
        Expect.isFalse (s.Contains "...") "AsString with exactly 5 items should not show ellipsis"

    testCase "Dict-Fable - ToString() empty" <| fun _ ->
        let d = Dict<string, int>()
        let s = d.ToString()
        Expect.isTrue (s.Contains "Dict") "ToString should contain Dict"
        Expect.isTrue (s.Contains "empty") "ToString on empty should say empty"

    testCase "Dict-Fable - ToString() with items" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        d.Set "B" 2
        let s = d.ToString()
        Expect.isTrue (s.Contains "2") "ToString should show count"

    testCase "Dict-Fable - ToString(n) with 0 entries to print" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        let s = d.ToString(0)
        Expect.isTrue (s.Contains "Dict") "ToString(0) should contain Dict"
        Expect.isFalse (s.Contains "A : 1") "ToString(0) should not show entries"

    testCase "Dict-Fable - ToString(n) with limited entries" <| fun _ ->
        let d = Dict<string, int>()
        for i in 1..10 do
            d.Set (string (char (64 + i))) i
        let s = d.ToString(3)
        Expect.isTrue (s.Contains "...") "ToString(3) with 10 items should show ellipsis"

    // ---------------------------------------------------------
    // Dict - ICollection interface (routed differently in Fable)
    // ---------------------------------------------------------

    testCase "Dict-Fable - ICollection Add and Count" <| fun _ ->
        let d = Dict<string, int>()
        let coll = d :> ICollection<KeyValuePair<string, int>>
        coll.Add(KeyValuePair("A", 1))
        coll.Add(KeyValuePair("B", 2))
        Expect.equal coll.Count 2 "ICollection count should match"

    testCase "Dict-Fable - ICollection Contains" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        let coll = d :> ICollection<KeyValuePair<string, int>>
        Expect.isTrue (coll.Contains(KeyValuePair("A", 1))) "ICollection should contain existing pair"
        // Contains checks key only in this implementation
        Expect.isTrue (coll.Contains(KeyValuePair("A", 999))) "ICollection Contains checks key only"

    testCase "Dict-Fable - ICollection Remove" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        let coll = d :> ICollection<KeyValuePair<string, int>>
        let removed = coll.Remove(KeyValuePair("A", 1))
        Expect.isTrue removed "ICollection Remove should return true"
        Expect.equal d.Count 0 "Count should be 0 after remove"

    testCase "Dict-Fable - ICollection Clear" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        d.Set "B" 2
        let coll = d :> ICollection<KeyValuePair<string, int>>
        coll.Clear()
        Expect.equal d.Count 0 "Clear via ICollection should remove all"

    testCase "Dict-Fable - ICollection IsReadOnly" <| fun _ ->
        let d = Dict<string, int>()
        let coll = d :> ICollection<KeyValuePair<string, int>>
        Expect.isFalse coll.IsReadOnly "IsReadOnly should be false"

    // ---------------------------------------------------------
    // Dict - IDictionary interface
    // ---------------------------------------------------------

    testCase "Dict-Fable - IDictionary Item get and set" <| fun _ ->
        let d = Dict<string, int>() :> IDictionary<string, int>
        d.["A"] <- 1
        Expect.equal d.["A"] 1 "IDictionary indexer should work"

    testCase "Dict-Fable - IDictionary TryGetValue existing" <| fun _ ->
        let d = Dict<string, int>() :> IDictionary<string, int>
        d.Add("A", 1)
        let ok, v = d.TryGetValue "A"
        Expect.isTrue ok "TryGetValue should succeed"
        Expect.equal v 1 "TryGetValue should return value"

    testCase "Dict-Fable - IDictionary TryGetValue missing" <| fun _ ->
        let d = Dict<string, int>() :> IDictionary<string, int>
        let ok, _ = d.TryGetValue "missing"
        Expect.isFalse ok "TryGetValue should fail for missing key"

    // ---------------------------------------------------------
    // Dict - IReadOnlyDictionary interface
    // ---------------------------------------------------------

    testCase "Dict-Fable - IReadOnlyDictionary Item" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        let rod = d :> IReadOnlyDictionary<string, int>
        Expect.equal rod.["A"] 1 "IReadOnlyDictionary indexer should work"

    testCase "Dict-Fable - IReadOnlyDictionary ContainsKey" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        let rod = d :> IReadOnlyDictionary<string, int>
        Expect.isTrue (rod.ContainsKey "A") "Should contain key"
        Expect.isFalse (rod.ContainsKey "B") "Should not contain missing key"

    testCase "Dict-Fable - IReadOnlyDictionary Count" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        d.Set "B" 2
        let rod = d :> IReadOnlyDictionary<string, int>
        Expect.equal rod.Count 2 "IReadOnlyDictionary count should match"

    testCase "Dict-Fable - IReadOnlyDictionary Keys and Values" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        d.Set "B" 2
        let rod = d :> IReadOnlyDictionary<string, int>
        let keys = rod.Keys |> Seq.toList |> List.sort
        let values = rod.Values |> Seq.toList |> List.sort
        Expect.equal keys ["A"; "B"] "Keys via IReadOnlyDictionary"
        Expect.equal values [1; 2] "Values via IReadOnlyDictionary"

    // =============================================================
    // DefaultDict - Fable/JS parity tests
    // =============================================================

    testCase "DefaultDict-Fable - Get on missing key creates default" <| fun _ ->
        let d = DefaultDict(fun _ -> 42)
        let v = d.Get "missing"
        Expect.equal v 42 "Should return default value"
        Expect.isTrue (d.ContainsKey "missing") "Key should be created"
        Expect.equal d.Count 1 "Count should be 1"

    testCase "DefaultDict-Fable - Get with key-dependent default" <| fun _ ->
        let d = DefaultDict(fun k -> String.length k)
        let v1 = d.Get "hi"
        let v2 = d.Get "hello"
        Expect.equal v1 2 "Default for 'hi'"
        Expect.equal v2 5 "Default for 'hello'"

    testCase "DefaultDict-Fable - Item indexer creates default" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        let v = d.["new_key"]
        Expect.equal v 0 "Indexer should return default"
        Expect.isTrue (d.ContainsKey "new_key") "Key should exist after indexer access"

    testCase "DefaultDict-Fable - Item indexer set" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.["A"] <- 99
        Expect.equal d.["A"] 99 "Indexer set should store value"

    testCase "DefaultDict-Fable - Item indexer set overwrites" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.["A"] <- 1
        d.["A"] <- 2
        Expect.equal d.["A"] 2 "Indexer set should overwrite"
        Expect.equal d.Count 1 "No duplicate keys"

    testCase "DefaultDict-Fable - Set and Get with int keys" <| fun _ ->
        let d = DefaultDict(fun _ -> "default")
        d.Set 1 "one"
        d.Set 2 "two"
        Expect.equal (d.Get 1) "one" "Int key 1"
        Expect.equal (d.Get 2) "two" "Int key 2"
        Expect.equal (d.Get 3) "default" "Missing int key gets default"

    testCase "DefaultDict-Fable - null key throws on Get" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        Expect.throws (fun () -> d.Get null |> ignore) "Get null key should throw"

    testCase "DefaultDict-Fable - null key throws on Set" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        Expect.throws (fun () -> d.Set null 1) "Set null key should throw"

    testCase "DefaultDict-Fable - null key throws on Item get" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        Expect.throws (fun () -> d.[null] |> ignore) "Item get null key should throw"

    testCase "DefaultDict-Fable - null key throws on Item set" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        Expect.throws (fun () -> d.[null] <- 1) "Item set null key should throw"

    testCase "DefaultDict-Fable - null key throws on Pop" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        Expect.throws (fun () -> d.Pop null |> ignore) "Pop null key should throw"

    testCase "DefaultDict-Fable - null key throws on TryPop" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        Expect.throws (fun () -> d.TryPop null |> ignore) "TryPop null key should throw"

    testCase "DefaultDict-Fable - Pop existing key" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 42
        let v = d.Pop "A"
        Expect.equal v 42 "Pop should return stored value"
        Expect.isFalse (d.ContainsKey "A") "Pop should remove key"

    testCase "DefaultDict-Fable - Pop missing key throws (does not create default)" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        Expect.throws (fun () -> d.Pop "missing" |> ignore) "Pop missing key should throw, not create default"

    testCase "DefaultDict-Fable - TryPop existing key" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 42
        let v = d.TryPop "A"
        Expect.equal v (Some 42) "TryPop should return Some"
        Expect.isFalse (d.ContainsKey "A") "TryPop should remove key"

    testCase "DefaultDict-Fable - TryPop missing key returns None (no default creation)" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        let v = d.TryPop "missing"
        Expect.equal v None "TryPop missing should return None"
        Expect.isFalse (d.ContainsKey "missing") "TryPop should not create key"

    testCase "DefaultDict-Fable - TryGetValue does not create default" <| fun _ ->
        let d = DefaultDict(fun _ -> 42)
        let ok, _ = d.TryGetValue "missing"
        Expect.isFalse ok "TryGetValue should return false for missing key"
        Expect.isFalse (d.ContainsKey "missing") "TryGetValue should NOT create key"
        Expect.equal d.Count 0 "Count should still be 0"

    testCase "DefaultDict-Fable - TryGetValue existing key" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 99
        let ok, v = d.TryGetValue "A"
        Expect.isTrue ok "TryGetValue should return true"
        Expect.equal v 99 "TryGetValue should return value"

    testCase "DefaultDict-Fable - ContainsKey on empty" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        Expect.isFalse (d.ContainsKey "A") "Empty DefaultDict should not contain any key"

    testCase "DefaultDict-Fable - DoesNotContainKey" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        Expect.isTrue (d.DoesNotContainKey "A") "Empty DefaultDict DoesNotContainKey"
        d.Set "A" 1
        Expect.isFalse (d.DoesNotContainKey "A") "Should not say DoesNotContainKey for present key"

    testCase "DefaultDict-Fable - Remove existing key" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 1
        let removed = d.Remove "A"
        Expect.isTrue removed "Remove existing key should return true"
        Expect.isFalse (d.ContainsKey "A") "Key should be gone"

    testCase "DefaultDict-Fable - Remove missing key" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        let removed = d.Remove "missing"
        Expect.isFalse removed "Remove missing key should return false"

    testCase "DefaultDict-Fable - Clear" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        for i in 1..50 do d.Set (string i) i
        Expect.equal d.Count 50 "Should have 50 entries"
        d.Clear()
        Expect.equal d.Count 0 "Clear should remove all"

    testCase "DefaultDict-Fable - Clear preserves default function" <| fun _ ->
        let d = DefaultDict(fun _ -> 42)
        d.Set "A" 1
        d.Clear()
        let v = d.Get "B"
        Expect.equal v 42 "Default function should still work after Clear"

    testCase "DefaultDict-Fable - Count" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        Expect.equal d.Count 0 "Initial count 0"
        d.Set "A" 1
        Expect.equal d.Count 1 "Count after set"
        let _ = d.Get "B" // creates default
        Expect.equal d.Count 2 "Count after Get on missing key"
        d.Remove "A" |> ignore
        Expect.equal d.Count 1 "Count after remove"

    testCase "DefaultDict-Fable - Keys and Values" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "X" 10
        d.Set "Y" 20
        let keys = d.Keys |> Seq.toList |> List.sort
        let values = d.Values |> Seq.toList |> List.sort
        Expect.equal keys ["X"; "Y"] "Keys"
        Expect.equal values [10; 20] "Values"

    testCase "DefaultDict-Fable - Items" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 1
        d.Set "B" 2
        let items = d.Items |> Seq.toList |> List.sortBy fst
        Expect.equal items [("A", 1); ("B", 2)] "Items should return tuples"

    testCase "DefaultDict-Fable - empty string key" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "" 42
        Expect.equal (d.Get "") 42 "Empty string key should work"

    testCase "DefaultDict-Fable - null value allowed" <| fun _ ->
        let d = DefaultDict(fun _ -> "default")
        d.Set "key" null
        Expect.equal (d.Get "key") null "Null value should be stored and retrieved"

    testCase "DefaultDict-Fable - reference type default (list)" <| fun _ ->
        let d = DefaultDict(fun _ -> ResizeArray<int>())
        d.Get("A").Add(1)
        d.Get("A").Add(2)
        d.Get("B").Add(10)
        Expect.equal (d.Get("A") |> Seq.toList) [1; 2] "List for A"
        Expect.equal (d.Get("B") |> Seq.toList) [10] "List for B"

    testCase "DefaultDict-Fable - ref cell default (value type wrapper)" <| fun _ ->
        let d = DefaultDict(fun _ -> ref 0)
        incr d.["counter1"]
        incr d.["counter1"]
        incr d.["counter2"]
        Expect.equal d.["counter1"].Value 2 "Counter1 should be 2"
        Expect.equal d.["counter2"].Value 1 "Counter2 should be 1"

    testCase "DefaultDict-Fable - int key with default" <| fun _ ->
        let d = DefaultDict(fun k -> k * 10)
        Expect.equal (d.Get 5) 50 "Default for key 5"
        Expect.equal (d.Get 0) 0 "Default for key 0"
        Expect.equal (d.Get -3) -30 "Default for key -3"

    testCase "DefaultDict-Fable - many entries" <| fun _ ->
        let d = DefaultDict(fun k -> k * k)
        for i in 0..999 do
            let _ = d.Get i
            ()
        Expect.equal d.Count 1000 "Should have 1000 entries"
        Expect.equal (d.Get 0) 0 "Value at 0"
        Expect.equal (d.Get 999) (999 * 999) "Value at 999"

    testCase "DefaultDict-Fable - create from pairs" <| fun _ ->
        let d = DefaultDict.create (fun _ -> 0) [("a", 1); ("b", 2)]
        Expect.equal d.Count 2 "Should have 2 items"
        Expect.equal (d.Get "a") 1 "Existing value a"
        Expect.equal (d.Get "b") 2 "Existing value b"
        Expect.equal (d.Get "c") 0 "Missing key gets default"

    testCase "DefaultDict-Fable - create from empty" <| fun _ ->
        let d = DefaultDict.create (fun _ -> 99) []
        Expect.equal d.Count 0 "Should be empty"
        Expect.equal (d.Get "any") 99 "Missing key gets default"

    testCase "DefaultDict-Fable - Add method" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Add("A", 1)
        Expect.equal (d.Get "A") 1 "Add should store value"

    testCase "DefaultDict-Fable - ContainsValue" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 42
        Expect.isTrue (d.ContainsValue 42) "Should find existing value"
        Expect.isFalse (d.ContainsValue 99) "Should not find missing value"

    testCase "DefaultDict-Fable - enumerate empty" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        let items = d |> Seq.toList
        Expect.equal items [] "Enumerating empty DefaultDict should yield empty list"

    testCase "DefaultDict-Fable - enumerate with items" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 1
        d.Set "B" 2
        let items = d |> Seq.map (fun kvp -> kvp.Key, kvp.Value) |> Seq.toList |> List.sortBy fst
        Expect.equal items [("A", 1); ("B", 2)] "Should enumerate all entries"

    // DefaultDict - AsString / ToString (FABLE_COMPILER branching)

    testCase "DefaultDict-Fable - AsString empty" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        let s = d.AsString
        Expect.isTrue (s.Contains "DefaultDict") "AsString should contain DefaultDict"
        Expect.isTrue (s.Contains "empty") "AsString on empty should say empty"

    testCase "DefaultDict-Fable - AsString with items" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 1
        let s = d.AsString
        Expect.isTrue (s.Contains "A") "AsString should contain key"
        Expect.isTrue (s.Contains "1") "AsString should contain value"

    testCase "DefaultDict-Fable - AsString with more than 5 items shows ellipsis" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        for i in 1..7 do
            d.Set (string (char (64 + i))) i
        let s = d.AsString
        Expect.isTrue (s.Contains "...") "AsString with >5 items should show ellipsis"

    testCase "DefaultDict-Fable - AsString with exactly 5 items no ellipsis" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        for i in 1..5 do
            d.Set (string (char (64 + i))) i
        let s = d.AsString
        Expect.isFalse (s.Contains "...") "AsString with exactly 5 items should not show ellipsis"

    testCase "DefaultDict-Fable - ToString() empty" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        let s = d.ToString()
        Expect.isTrue (s.Contains "DefaultDict") "ToString should contain DefaultDict"
        Expect.isTrue (s.Contains "empty") "ToString on empty should say empty"

    testCase "DefaultDict-Fable - ToString() with items" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 1
        d.Set "B" 2
        let s = d.ToString()
        Expect.isTrue (s.Contains "2") "ToString should show count"

    testCase "DefaultDict-Fable - ToString(0) hides entries" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 1
        let s = d.ToString(0)
        Expect.isTrue (s.Contains "DefaultDict") "Should contain DefaultDict"
        Expect.isFalse (s.Contains "A : 1") "Should not show entries"

    testCase "DefaultDict-Fable - ToString(n) with limited entries" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        for i in 1..10 do
            d.Set (string (char (64 + i))) i
        let s = d.ToString(3)
        Expect.isTrue (s.Contains "...") "ToString(3) with 10 items should show ellipsis"

    // DefaultDict - ICollection interface

    testCase "DefaultDict-Fable - ICollection Add" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        let coll = d :> ICollection<KeyValuePair<string, int>>
        coll.Add(KeyValuePair("A", 1))
        Expect.equal d.Count 1 "ICollection Add should work"
        Expect.equal (d.Get "A") 1 "Value should be accessible"

    testCase "DefaultDict-Fable - ICollection Contains" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 1
        let coll = d :> ICollection<KeyValuePair<string, int>>
        Expect.isTrue (coll.Contains(KeyValuePair("A", 1))) "Should contain existing pair"

    testCase "DefaultDict-Fable - ICollection Remove" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 1
        let coll = d :> ICollection<KeyValuePair<string, int>>
        let removed = coll.Remove(KeyValuePair("A", 1))
        Expect.isTrue removed "Should remove"
        Expect.equal d.Count 0 "Count should be 0"

    testCase "DefaultDict-Fable - ICollection Clear" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 1
        d.Set "B" 2
        let coll = d :> ICollection<KeyValuePair<string, int>>
        coll.Clear()
        Expect.equal d.Count 0 "Should be empty after clear"

    testCase "DefaultDict-Fable - ICollection IsReadOnly" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        let coll = d :> ICollection<KeyValuePair<string, int>>
        Expect.isFalse coll.IsReadOnly "Should not be read-only"

    testCase "DefaultDict-Fable - IReadOnlyCollection Count" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        d.Set "A" 1
        d.Set "B" 2
        let roc = d :> IReadOnlyCollection<KeyValuePair<string, int>>
        Expect.equal roc.Count 2 "IReadOnlyCollection count should match"

    // =============================================================
    // IDictionary Extension Methods - Fable/JS parity tests
    // =============================================================

    testCase "IDic-Fable - SetValue and GetValue" <| fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        d.SetValue "K" 42
        Expect.equal (d.GetValue "K") 42 "SetValue/GetValue round-trip"

    testCase "IDic-Fable - GetValue missing key throws" <| fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        Expect.throws (fun () -> d.GetValue "missing" |> ignore) "GetValue missing key should throw"

    testCase "IDic-Fable - Pop existing" <| fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        d.["A"] <- 1
        let v = d.Pop "A"
        Expect.equal v 1 "Pop should return value"
        Expect.isFalse (d.ContainsKey "A") "Pop should remove key"

    testCase "IDic-Fable - Pop missing throws" <| fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        Expect.throws (fun () -> d.Pop "missing" |> ignore) "Pop missing should throw"

    testCase "IDic-Fable - TryPop existing" <| fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        d.["A"] <- 1
        let v = d.TryPop "A"
        Expect.equal v (Some 1) "TryPop should return Some"
        Expect.isFalse (d.ContainsKey "A") "TryPop should remove key"

    testCase "IDic-Fable - TryPop missing returns None" <| fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        let v = d.TryPop "missing"
        Expect.equal v None "TryPop missing should return None"

    testCase "IDic-Fable - Items empty" <| fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        let items = d.Items |> Seq.toList
        Expect.equal items [] "Items on empty should be empty"

    testCase "IDic-Fable - Items with entries" <| fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        d.["A"] <- 1
        d.["B"] <- 2
        let items = d.Items |> Seq.toList |> List.sortBy fst
        Expect.equal items [("A", 1); ("B", 2)] "Items should return tuples"

    testCase "IDic-Fable - KeysSeq and ValuesSeq" <| fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        d.["A"] <- 1
        d.["B"] <- 2
        let ks = d.KeysSeq |> Seq.toList |> List.sort
        let vs = d.ValuesSeq |> Seq.toList |> List.sort
        Expect.equal ks ["A"; "B"] "KeysSeq"
        Expect.equal vs [1; 2] "ValuesSeq"

    testCase "IDic-Fable - DoesNotContainKey" <| fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        Expect.isTrue (d.DoesNotContainKey "A") "Empty dict DoesNotContainKey"
        d.["A"] <- 1
        Expect.isFalse (d.DoesNotContainKey "A") "Present key"

    testCase "IDic-Fable - AsString empty" <| fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        let s = d.AsString
        Expect.isTrue (s.Contains "empty") "AsString on empty should say empty"

    testCase "IDic-Fable - AsString with items" <| fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        d.["A"] <- 1
        let s = d.AsString
        Expect.isTrue (s.Contains "A") "AsString should contain key"
        Expect.isTrue (s.Contains "1") "AsString should contain value"

    testCase "IDic-Fable - AsString with more than 5 items shows ellipsis" <| fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        for i in 1..7 do
            d.[string (char (64 + i))] <- i
        let s = d.AsString
        Expect.isTrue (s.Contains "...") "Should show ellipsis for >5 items"

    testCase "IDic-Fable - ToString(n) with 0 entries" <| fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        d.["A"] <- 1
        let s = d.ToString(0)
        Expect.isFalse (s.Contains "A : 1") "Should not show entries with 0 entriesToPrint"

    testCase "IDic-Fable - ToString(n) with limited entries shows ellipsis" <| fun _ ->
        let d = Dictionary<string, int>() :> IDictionary<string, int>
        for i in 1..10 do
            d.[string (char (64 + i))] <- i
        let s = d.ToString(3)
        Expect.isTrue (s.Contains "...") "Should show ellipsis"

    // IDictionary extensions used via Dict (cast to IDictionary)

    testCase "IDic-Fable - Dict cast to IDictionary Pop" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 42
        let iDict = d :> IDictionary<string, int>
        let v = iDict.Pop "A"
        Expect.equal v 42 "Pop via IDictionary on Dict"
        Expect.isFalse (d.ContainsKey "A") "Should be removed from Dict too"

    testCase "IDic-Fable - Dict cast to IDictionary TryPop" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 42
        let iDict = d :> IDictionary<string, int>
        let v = iDict.TryPop "A"
        Expect.equal v (Some 42) "TryPop via IDictionary on Dict"
        Expect.equal d.Count 0 "Should be removed"

    testCase "IDic-Fable - Dict cast to IDictionary Items" <| fun _ ->
        let d = Dict<string, int>()
        d.Set "A" 1
        d.Set "B" 2
        let iDict = d :> IDictionary<string, int>
        let items = iDict.Items |> Seq.toList |> List.sortBy fst
        Expect.equal items [("A", 1); ("B", 2)] "Items via IDictionary on Dict"

    // Edge cases: value types as values

    testCase "Dict-Fable - struct tuple values" <| fun _ ->
        let d = Dict<string, struct(int * int)>()
        d.Set "point" (struct(1, 2))
        let struct(x, y) = d.Get "point"
        Expect.equal x 1 "Struct tuple x"
        Expect.equal y 2 "Struct tuple y"

    testCase "DefaultDict-Fable - zero default for int" <| fun _ ->
        let d = DefaultDict(fun _ -> 0)
        Expect.equal (d.Get "any") 0 "Default int should be 0"

    testCase "DefaultDict-Fable - empty string default" <| fun _ ->
        let d = DefaultDict(fun _ -> "")
        Expect.equal (d.Get "any") "" "Default string should be empty"

    testCase "DefaultDict-Fable - false default for bool" <| fun _ ->
        let d = DefaultDict(fun _ -> false)
        Expect.equal (d.Get "any") false "Default bool should be false"














  ]


















