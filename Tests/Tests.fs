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



  ]














