module TestList

open Dic

#if FABLE_COMPILER
open Fable.Mocha
#else
open Expecto
#endif

open System

let tests  =
  testList "Module.fs Tests" [

    testCase "Pop" <| fun _ ->
        let b = Dict()
        b.["A"] <- 1
        b.Pop "A" |> ignore<int>
        let result = b.DoesNotContainKey "A"
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



  ]














