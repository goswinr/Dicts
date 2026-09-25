namespace Tests

module Main =

    open type Scriptorium.Quill.Runner

    // Scriptorium runs the same tests on .NET and on JavaScript via Fable.
    // Fable calls this entry point too; on JS the exit code is passed to process.exit.
    [<EntryPoint>]
    let main _argv =
        runTests TestList.tests