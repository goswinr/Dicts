namespace Tests

module Main =

    #if FABLE_COMPILER

        open Fable.Mocha
        Mocha.runTests Tests.Tests.tests |> ignore

    #else

        open Expecto
        [<EntryPoint>]
        let main argv =
            let a = runTestsWithCLIArgs [] [||] Tests.Tests.tests
            let b = 1 // runTestsWithCLIArgs [] [||] Tests.Module.tests
            a ||| b

    #endif