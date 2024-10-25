module TestList

open Dic
open System.Collections.Generic

#if FABLE_COMPILER
open Fable.Mocha
#else
open Expecto
#endif

open System

let tests  =
  testList "Module.fs Tests" [

    // Dict:

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

    testCase "Add" <| fun _ ->
        let b = Dict()
        b.Add' "A" 1
        let result = b.Get "A"
        Expect.equal result 1 "Add adds key A with value 1"


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

    testCase "KV-Add" <| fun _ ->
        let b = Dict<string, int>()
        let kvp = KeyValuePair("A", 1)
        let iColl = (b :> ICollection<KeyValuePair<string, int>>)
        iColl.Add(kvp)
        Expect.equal (b.Get "A") 1 "Add should add the key-value pair to the dictionary"

    testCase "KV-Clear" <| fun _ ->
        let b = Dict<string, int>()
        (b :> ICollection<KeyValuePair<string, int>>).Add(KeyValuePair("A", 1))
        (b :> ICollection<KeyValuePair<string, int>>).Clear()
        Expect.isFalse (b.ContainsKey "A") "Clear should remove all key-value pairs from the dictionary"

    testCase "KV-Remove" <| fun _ ->
        let b = Dict<string, int>()
        (b :> ICollection<KeyValuePair<string, int>>).Add(KeyValuePair("A", 1))
        let result = (b :> ICollection<KeyValuePair<string, int>>).Remove(KeyValuePair("A", 1))
        Expect.isTrue result "Remove should return true when the key-value pair is removed"
        Expect.isFalse (b.ContainsKey "A") "Remove should remove the key-value pair from the dictionary"

    testCase "KV-Contains" <| fun _ ->
        let b = Dict<string, int>()
        (b :> ICollection<KeyValuePair<string, int>>).Add(KeyValuePair("A", 1))
        let result = (b :> ICollection<KeyValuePair<string, int>>).Contains(KeyValuePair("A", 1))
        Expect.isTrue result "Contains should return true when the key-value pair is in the dictionary"

    testCase "KV-CopyTo" <| fun _ ->
        let b = Dict<string, int>()
        (b :> ICollection<KeyValuePair<string, int>>).Add(KeyValuePair("A", 1))
        let arr = Array.zeroCreate<KeyValuePair<string, int>> 1
        (b :> ICollection<KeyValuePair<string, int>>).CopyTo(arr, 0)
        Expect.equal arr.[0] (KeyValuePair("A", 1)) "CopyTo should copy the key-value pairs to the array"

    testCase "KV-IsReadOnly" <| fun _ ->
        let b = Dict<string, int>()
        let result = (b :> ICollection<KeyValuePair<string, int>>).IsReadOnly
        Expect.isFalse result "IsReadOnly should return false"

    testCase "KV-Count" <| fun _ ->
        let b = Dict<string, int>()
        (b :> ICollection<KeyValuePair<string, int>>).Add(KeyValuePair("A", 1))
        let result = (b :> ICollection<KeyValuePair<string, int>>).Count
        Expect.equal result 1 "Count should return the number of key-value pairs in the dictionary"

  ]


















