namespace Tests

open Dict

#if FABLE_COMPILER
open Fable.Mocha
#else
open Expecto
#endif

open System

open Microsoft.FSharp.Data.UnitSystems.SI

module Tests =

  let tests  =
    testList "Module.fs Tests" [

        testCase "isNanOrInf should return true for NaN" <| fun _ ->
          let result = Dict.isNanOrInf Double.NaN
          Expect.isTrue result "Expected NaN to be identified as NaN or Infinity"

        testCase "isNanOrInf should return true for Infinity" <| fun _ ->
          let result = Dict.isNanOrInf Double.PositiveInfinity
          Expect.isTrue result "Expected Infinity to be identified as NaN or Infinity"

        testCase "isNanOrInf should return false for a normal number" <| fun _ ->
          let result = Dict.isNanOrInf 1.0
          Expect.isFalse result "Expected 1.0 to not be identified as NaN or Infinity"

        testCase "isNan should return true for NaN" <| fun _ ->
          let result = Dict.isNan Double.NaN
          Expect.isTrue result "Expected NaN to be identified as NaN"

        testCase "isNan should return false for a normal number" <| fun _ ->
          let result = Dict.isNan 1.0
          Expect.isFalse result "Expected 1.0 to not be identified as NaN"

        testCase "clamp should return the value if within range" <| fun _ ->
          let result = Dict.clamp 0.0 10.0 5.0
          Expect.equal result 5.0 "Expected 5.0 to be within the range 0.0 to 10.0"

        testCase "clamp should return minVal if value is below range" <| fun _ ->
          let result = Dict.clamp 0.0 10.0 -1.0
          Expect.equal result 0.0 "Expected -1.0 to be clamped to 0.0"

        testCase "clamp should return maxVal if value is above range" <| fun _ ->
          let result = Dict.clamp 0.0 10.0 11.0
          Expect.equal result 10.0 "Expected 11.0 to be clamped to 10.0"

        testCase "clamp01 should return the value if within range" <| fun _ ->
          let result = Dict.clamp01 0.5
          Expect.equal result 0.5 "Expected 0.5 to be within the range 0.0 to 1.0"

        testCase "clamp01 should return 0.0 if value is below range" <| fun _ ->
          let result = Dict.clamp01 -0.1
          Expect.equal result 0.0 "Expected -0.1 to be clamped to 0.0"

        testCase "clamp01 should return 1.0 if value is above range" <| fun _ ->
          let result = Dict.clamp01 1.1
          Expect.equal result 1.0 "Expected 1.1 to be clamped to 1.0"

        testCase "acos should return the correct value for a valid input" <| fun _ ->
          let result = Dict.acos 0.5
          Expect.floatClose Accuracy.high result (Math.Acos 0.5) "Expected acos(0.5) to match Math.Acos(0.5)"

        testCase "acos should throw for NaN input" <| fun _ ->
          Expect.throws (fun _ -> Dict.acos Double.NaN |> ignore) "Expected exception for NaN input"

        testCase "acos should throw for input less than -1.0001" <| fun _ ->
          Expect.throws (fun _ -> Dict.acos -1.0002 |> ignore) "Expected exception for input less than -1.0001"

        testCase "acos should throw for input greater than 1.0001" <| fun _ ->
          Expect.throws (fun _ -> Dict.acos 1.0002 |> ignore) "Expected exception for input greater than 1.0001"

        testCase "asin should return the correct value for a valid input" <| fun _ ->
          let result = Dict.asin 0.5
          Expect.floatClose Accuracy.high result (Math.Asin 0.5) "Expected asin(0.5) to match Math.Asin(0.5)"

        testCase "asin should throw for NaN input" <| fun _ ->
          Expect.throws (fun _ -> Dict.asin Double.NaN |> ignore) "Expected exception for NaN input"

        testCase "asin should throw for input less than -1.0001" <| fun _ ->
          Expect.throws (fun _ -> Dict.asin -1.0002 |> ignore) "Expected exception for input less than -1.0001"

        testCase "asin should throw for input greater than 1.0001" <| fun _ ->
          Expect.throws (fun _ -> Dict.asin 1.0002 |> ignore) "Expected exception for input greater than 1.0001"


        testCase "isNanOrInf should return true for NaN" <| fun _ ->
                let result = Dict.isNanOrInf Double.NaN
                Expect.isTrue result "Expected NaN to be identified as NaN or Infinity"

        testCase "isNanOrInf should return true for Infinity" <| fun _ ->
                let result = Dict.isNanOrInf Double.PositiveInfinity
                Expect.isTrue result "Expected Infinity to be identified as NaN or Infinity"

        testCase "isNanOrInf should return false for a normal number" <| fun _ ->
                let result = Dict.isNanOrInf 1.0
                Expect.isFalse result "Expected 1.0 to not be identified as NaN or Infinity"

        testCase "isNan should return true for NaN" <| fun _ ->
                let result = Dict.isNan Double.NaN
                Expect.isTrue result "Expected NaN to be identified as NaN"

        testCase "isNan should return false for a normal number" <| fun _ ->
                let result = Dict.isNan 1.0
                Expect.isFalse result "Expected 1.0 to not be identified as NaN"

        testCase "clamp should return the value if within range" <| fun _ ->
                let result = Dict.clamp 0.0 10.0 5.0
                Expect.equal result 5.0 "Expected 5.0 to be within the range 0.0 to 10.0"

        testCase "clamp should return minVal if value is below range" <| fun _ ->
                let result = Dict.clamp 0.0 10.0 -1.0
                Expect.equal result 0.0 "Expected -1.0 to be clamped to 0.0"

        testCase "clamp should return maxVal if value is above range" <| fun _ ->
                let result = Dict.clamp 0.0 10.0 11.0
                Expect.equal result 10.0 "Expected 11.0 to be clamped to 10.0"

        testCase "clamp01 should return the value if within range" <| fun _ ->
                let result = Dict.clamp01 0.5
                Expect.equal result 0.5 "Expected 0.5 to be within the range 0.0 to 1.0"

        testCase "clamp01 should return 0.0 if value is below range" <| fun _ ->
                let result = Dict.clamp01 -0.1
                Expect.equal result 0.0 "Expected -0.1 to be clamped to 0.0"

        testCase "clamp01 should return 1.0 if value is above range" <| fun _ ->
                let result = Dict.clamp01 1.1
                Expect.equal result 1.0 "Expected 1.1 to be clamped to 1.0"

        testCase "acos should return the correct value for a valid input" <| fun _ ->
                let result = Dict.acos 0.5
                Expect.floatClose Accuracy.high result (Math.Acos 0.5) "Expected acos(0.5) to match Math.Acos(0.5)"

        testCase "acos should throw for NaN input" <| fun _ ->
                Expect.throws (fun _ -> Dict.acos Double.NaN |> ignore) "Expected exception for NaN input"

        testCase "acos should throw for input less than -1.0001" <| fun _ ->
                Expect.throws (fun _ -> Dict.acos -1.0002 |> ignore) "Expected exception for input less than -1.0001"

        testCase "acos should throw for input greater than 1.0001" <| fun _ ->
                Expect.throws (fun _ -> Dict.acos 1.0002 |> ignore) "Expected exception for input greater than 1.0001"

        testCase "asin should return the correct value for a valid input" <| fun _ ->
                let result = Dict.asin 0.5
                Expect.floatClose Accuracy.high result (Math.Asin 0.5) "Expected asin(0.5) to match Math.Asin(0.5)"

        testCase "asin should throw for NaN input" <| fun _ ->
                Expect.throws (fun _ -> Dict.asin Double.NaN |> ignore) "Expected exception for NaN input"

        testCase "asin should throw for input less than -1.0001" <| fun _ ->
                Expect.throws (fun _ -> Dict.asin -1.0002 |> ignore) "Expected exception for input less than -1.0001"

        testCase "asin should throw for input greater than 1.0001" <| fun _ ->
                Expect.throws (fun _ -> Dict.asin 1.0002 |> ignore) "Expected exception for input greater than 1.0001"

        testCase "toRadians should convert degrees to radians correctly" <| fun _ ->
                let result = Dict.toRadians 180.0
                Expect.floatClose Accuracy.high result Math.PI "Expected 180 degrees to be PI radians"

        testCase "toDegrees should convert radians to degrees correctly" <| fun _ ->
                let result = Dict.toDegrees Math.PI
                Expect.floatClose Accuracy.high result 180.0 "Expected PI radians to be 180 degrees"

        testCase "isInRange should return true if value is within range" <| fun _ ->
                let result = Dict.isInRange 0.0 10.0 5.0
                Expect.isTrue result "Expected 5.0 to be within the range 0.0 to 10.0"

        testCase "isInRange should return false if value is outside range" <| fun _ ->
                let result = Dict.isInRange 0.0 10.0 11.0
                Expect.isFalse result "Expected 11.0 to be outside the range 0.0 to 10.0"

        testCase "isNotInRange should return true if value is outside range" <| fun _ ->
                let result = Dict.isNotInRange 0.0 10.0 11.0
                Expect.isTrue result "Expected 11.0 to be outside the range 0.0 to 10.0"

        testCase "isNotInRange should return false if value is within range" <| fun _ ->
                let result = Dict.isNotInRange 0.0 10.0 5.0
                Expect.isFalse result "Expected 5.0 to be within the range 0.0 to 10.0"

        testCase "equalsWithRelativeTolerance should return true for equal values within tolerance" <| fun _ ->
                let result = Dict.equalsWithRelativeTolerance 0.1 1.0 1.05
                Expect.isTrue result "Expected 1.0 and 1.05 to be equal within 10% tolerance"

        testCase "equalsWithRelativeTolerance should return false for values outside tolerance" <| fun _ ->
                let result = Dict.equalsWithRelativeTolerance 0.1 1.0 1.2
                Expect.isFalse result "Expected 1.0 and 1.2 to not be equal within 10% tolerance"

        testCase "equalsWithTolerance should return true for equal values within tolerance" <| fun _ ->
                let result = Dict.equalsWithTolerance 0.1 1.0 1.05
                Expect.isTrue result "Expected 1.0 and 1.05 to be equal within 0.1 tolerance"

        testCase "equalsWithTolerance should return false for values outside tolerance" <| fun _ ->
                let result = Dict.equalsWithTolerance 0.1 1.0 1.2
                Expect.isFalse result "Expected 1.0 and 1.2 to not be equal within 0.1 tolerance"

        testCase "interpolate should return correct interpolated value" <| fun _ ->
                let result = Dict.interpolate 0.0 10.0 0.5
                Expect.equal result 5.0 "Expected interpolation between 0.0 and 10.0 at 0.5 to be 5.0"

        testCase "reciprocal should return correct reciprocal value" <| fun _ ->
                let result = Dict.reciprocal 2.0
                Expect.equal result 0.5 "Expected reciprocal of 2.0 to be 0.5"

        testCase "inverse should return correct inverse value" <| fun _ ->
                let result = Dict.inverse 2.0
                Expect.equal result 0.5 "Expected inverse of 2.0 to be 0.5"

        testCase "normalize should return correct normalized value" <| fun _ ->
                let result = Dict.normalize 0.0 10.0 5.0
                Expect.floatClose Accuracy.high result 0.5 "Expected normalization of 5.0 between 0.0 and 10.0 to be 0.5"

        testCase "roundDownToNextLogSteps should return correct rounded value" <| fun _ ->
                let result = Dict.roundDownToNextLogSteps 10 1.0
                Expect.equal result 1.0 "Expected rounded value to be 1.0"

        testCase "steppedIntF should return correct stepped int value" <| fun _ ->
                let result = Dict.steppedIntF 0.1 1.05
                Expect.equal result 10 "Expected stepped int value to be 10"

        testCase "steppedInt should return correct stepped int value" <| fun _ ->
                let result = Dict.steppedInt 10 105.0
                Expect.equal result 100 "Expected stepped int value to be 100"

        testCase "steppedFloat should return correct stepped float value" <| fun _ ->
                let result = Dict.steppedFloat 0.1 1.05
                Expect.equal result 1.0 "Expected stepped float value to be 1.0"

        testCase "floatRange should return correct range" <| fun _ ->
                let result = Dict.floatRange (0.0, 0.1, 0.2) |> Seq.toList
                Expect.equal result [0.0; 0.1; 0.2] "Expected float range to be [0.0; 0.1; 0.2]"

        testCase "randomStandardDeviation should return a value" <| fun _ ->
                let result = Dict.randomStandardDeviation (0.0, 1.0)
                Expect.isTrue (result >= -3.0 && result <= 3.0) "Expected random value to be within 3 standard deviations"

        testCase "tryParseFloat should return Some for valid float string" <| fun _ ->
                let result = Dict.tryParseFloat "1.23"
                Expect.equal result (Some 1.23) "Expected Some 1.23 for valid float string"

        testCase "tryParseFloat should return None for invalid float string" <| fun _ ->
                let result = Dict.tryParseFloat "abc"
                Expect.equal result None "Expected None for invalid float string"

        testCase "tryParseFloatDe should return Some for valid float string with comma" <| fun _ ->
                let result = Dict.tryParseFloatDe "1,23"
                Expect.equal result (Some 1.23) "Expected Some 1.23 for valid float string with comma"

        testCase "tryParseFloatDe should return None for invalid float string" <| fun _ ->
                let result = Dict.tryParseFloatDe "abc"
                Expect.equal result None "Expected None for invalid float string"

        testCase "tryParseFloatTolerant should return Some for valid float string with non-numeric characters" <| fun _ ->
                let result = Dict.tryParseFloatTolerant "1.23abc"
                Expect.equal result (Some 1.23) "Expected Some 1.23 for valid float string with non-numeric characters"

        testCase "tryParseFloatTolerant should return None for invalid float string" <| fun _ ->
                let result = Dict.tryParseFloatTolerant "abc"
                Expect.equal result None "Expected None for invalid float string"

        testCase "tryParseFloatDeTolerant should return Some for valid float string with comma and non-numeric characters" <| fun _ ->
                let result = Dict.tryParseFloatDeTolerant "1,23abc"
                Expect.equal result (Some 1.23) "Expected Some 1.23 for valid float string with comma and non-numeric characters"

        testCase "tryParseFloatDeTolerant should return None for invalid float string" <| fun _ ->
                let result = Dict.tryParseFloatDeTolerant "abc"
                Expect.equal result None "Expected None for invalid float string"


        testCase "interpolateTable should interpolate correctly within the table range" <| fun _ ->
            let table = [
                (0.0<UnitSymbols.A>, 0.0<UnitSymbols.J>);
                (1.0<UnitSymbols.A>, 2.0<UnitSymbols.J>);
                (2.0<UnitSymbols.A>, 4.0<UnitSymbols.J>) ] |> List.toArray
            let interpolate = Dict.interpolateTable table
            let result = interpolate 1.5<UnitSymbols.A>
            Expect.equal result 3.0<UnitSymbols.J> "Expected interpolation at 1.5 to be 3.0"

        testCase "interpolateTable should interpolate correctly within the table range" <| fun _ ->
            let table = [
                (0.0<UnitSymbols.A>, 0.0<UnitSymbols.J>);
                (1.0<UnitSymbols.A>, 2.0<UnitSymbols.J>);
                (2.0<UnitSymbols.A>, 4.0<UnitSymbols.J>) ] |> List.toArray
            let interpolate = Dict.interpolateTable table
            let result = interpolate 3.0<UnitSymbols.A>
            Expect.equal result 6.0<UnitSymbols.J> "Expected interpolation at 3.0 to be 6.0"

        testCase "interpolateTable should interpolate correctly within the table range" <| fun _ ->
            let table = [
                (0.0<UnitSymbols.A>, 0.0<UnitSymbols.J>);
                (1.0<UnitSymbols.A>, 2.0<UnitSymbols.J>);
                (2.0<UnitSymbols.A>, 4.0<UnitSymbols.J>) ] |> List.toArray
            let interpolate = Dict.interpolateTable table
            let result = interpolate -0.5<UnitSymbols.A>
            Expect.equal result -1.<UnitSymbols.J> "Expected interpolation at -0.5 to be -1"

    ]














