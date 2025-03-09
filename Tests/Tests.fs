module TestList

open Dicts
open System.Collections.Generic

#if FABLE_COMPILER
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



















  ]


















