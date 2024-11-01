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
        let result = b.Get "A"
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
        let b = Dict()
        Expect.throws (fun () -> b.Set null 1 ) "Add throws exception when key does not exist"


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


    (*
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

    // testCase "KV-Remove" <| fun _ ->
    //     let b = Dict<string, int>()
    //     b.Add("A", 1)
    //     let result = (b :> ICollection<KeyValuePair<string, int>>).Remove(KeyValuePair("A", 1))
    //     Expect.isTrue result "Remove should return true when the key-value pair is removed"
    //     Expect.isFalse (b.ContainsKey "A") "Remove should remove the key-value pair from the dictionary"

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
    *)

    // ---------------------------------------------------------
    // DefaultDict:
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

    testCase "DefaultDict - default value" <| fun _ ->
        let b = DefaultDict(fun _ -> ref 0)
        incr (b.Get "A")
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


  ]


















