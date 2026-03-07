namespace Tests

module Main =

    #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
        open Fable.Mocha

        TestList.tests
        |> Mocha.runTests
        |> ignore
    #else
        open Expecto
        [<EntryPoint>]
        let main argv =
            TestList.tests
            |> runTestsWithCLIArgs [] [||]
    #endif