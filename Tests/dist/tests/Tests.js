import { Expect_throws, AccuracyModule_high, Expect_floatClose, Expect_isFalse, Expect_isTrue, Test_testCase, Test_testList } from "./fable_modules/Fable.Mocha.2.17.0/Mocha.fs.js";
import { tryParse, min, isInfinity } from "./fable_modules/fable-library-js.4.21.0/Double.js";
import { DictFail_failNotPos, DictFail_failInfNaNv, DictFail_failBig36v, DictFail_failTiny18v, DictFail_failNaNv, DictFail_failBig36, DictFail_failTiny18, DictFail_fail11, DictFail_failNaN, DictFail_failClamp } from "./Src/Dict.js";
import { equals as equals_1, int32ToString, structuralHash, assertEqual } from "./fable_modules/fable-library-js.4.21.0/Util.js";
import { ofArray, contains } from "./fable_modules/fable-library-js.4.21.0/List.js";
import { option_type, list_type, equals, class_type, decimal_type, string_type, bool_type, int32_type, float64_type } from "./fable_modules/fable-library-js.4.21.0/Reflection.js";
import { replace, printf, toText } from "./fable_modules/fable-library-js.4.21.0/String.js";
import { Dict_interpolateTable_16FC164C, Dict_tryParseFloatDeTolerant_Z721C83C5, Dict_tryParseFloatTolerant_Z721C83C5, Dict_randomStandardDeviation_B9AD360, Dict_floatRange_Z1EAE0AEB, Dict_roundDownToNextLogSteps } from "./Src/Dict.js";
import { toList } from "./fable_modules/fable-library-js.4.21.0/Seq.js";
import { toString, seqToString } from "./fable_modules/fable-library-js.4.21.0/Types.js";
import { FSharpRef } from "./fable_modules/fable-library-js.4.21.0/Types.js";

export const tests = Test_testList("Module.fs Tests", ofArray([Test_testCase("isNanOrInf should return true for NaN", () => {
    Expect_isTrue(isInfinity(NaN) ? true : Number.isNaN(NaN))("Expected NaN to be identified as NaN or Infinity");
}), Test_testCase("isNanOrInf should return true for Infinity", () => {
    Expect_isTrue(isInfinity(Infinity) ? true : Number.isNaN(Infinity))("Expected Infinity to be identified as NaN or Infinity");
}), Test_testCase("isNanOrInf should return false for a normal number", () => {
    Expect_isFalse(isInfinity(1) ? true : Number.isNaN(1))("Expected 1.0 to not be identified as NaN or Infinity");
}), Test_testCase("isNan should return true for NaN", () => {
    Expect_isTrue(Number.isNaN(NaN))("Expected NaN to be identified as NaN");
}), Test_testCase("isNan should return false for a normal number", () => {
    Expect_isFalse(Number.isNaN(1))("Expected 1.0 to not be identified as NaN");
}), Test_testCase("clamp should return the value if within range", () => {
    let copyOfStruct, arg, arg_1;
    const actual = (10 < 0) ? DictFail_failClamp(10, 0) : ((5 > 10) ? 10 : ((5 < 0) ? 0 : 5));
    if ((actual === 5) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual, 5, "Expected 5.0 to be within the range 0.0 to 10.0");
    }
    else {
        throw new Error(contains((copyOfStruct = actual, float64_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg = (5).toString(), (arg_1 = actual.toString(), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg)(arg_1)("Expected 5.0 to be within the range 0.0 to 10.0")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(5)(actual)("Expected 5.0 to be within the range 0.0 to 10.0"));
    }
}), Test_testCase("clamp should return minVal if value is below range", () => {
    let copyOfStruct_1, arg_6, arg_1_1;
    const actual_1 = (10 < 0) ? DictFail_failClamp(10, 0) : ((-1 > 10) ? 10 : ((-1 < 0) ? 0 : -1));
    if ((actual_1 === 0) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_1, 0, "Expected -1.0 to be clamped to 0.0");
    }
    else {
        throw new Error(contains((copyOfStruct_1 = actual_1, float64_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_6 = (0).toString(), (arg_1_1 = actual_1.toString(), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_6)(arg_1_1)("Expected -1.0 to be clamped to 0.0")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(0)(actual_1)("Expected -1.0 to be clamped to 0.0"));
    }
}), Test_testCase("clamp should return maxVal if value is above range", () => {
    let copyOfStruct_2, arg_7, arg_1_2;
    const actual_2 = (10 < 0) ? DictFail_failClamp(10, 0) : ((11 > 10) ? 10 : ((11 < 0) ? 0 : 11));
    if ((actual_2 === 10) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_2, 10, "Expected 11.0 to be clamped to 10.0");
    }
    else {
        throw new Error(contains((copyOfStruct_2 = actual_2, float64_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_7 = (10).toString(), (arg_1_2 = actual_2.toString(), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_7)(arg_1_2)("Expected 11.0 to be clamped to 10.0")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(10)(actual_2)("Expected 11.0 to be clamped to 10.0"));
    }
}), Test_testCase("clamp01 should return the value if within range", () => {
    let copyOfStruct_3, arg_8, arg_1_3;
    const actual_3 = (0.5 > 1) ? 1 : ((0.5 < 0) ? 0 : 0.5);
    if ((actual_3 === 0.5) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_3, 0.5, "Expected 0.5 to be within the range 0.0 to 1.0");
    }
    else {
        throw new Error(contains((copyOfStruct_3 = actual_3, float64_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_8 = (0.5).toString(), (arg_1_3 = actual_3.toString(), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_8)(arg_1_3)("Expected 0.5 to be within the range 0.0 to 1.0")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(0.5)(actual_3)("Expected 0.5 to be within the range 0.0 to 1.0"));
    }
}), Test_testCase("clamp01 should return 0.0 if value is below range", () => {
    let copyOfStruct_4, arg_9, arg_1_4;
    const actual_4 = (-0.1 > 1) ? 1 : ((-0.1 < 0) ? 0 : -0.1);
    if ((actual_4 === 0) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_4, 0, "Expected -0.1 to be clamped to 0.0");
    }
    else {
        throw new Error(contains((copyOfStruct_4 = actual_4, float64_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_9 = (0).toString(), (arg_1_4 = actual_4.toString(), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_9)(arg_1_4)("Expected -0.1 to be clamped to 0.0")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(0)(actual_4)("Expected -0.1 to be clamped to 0.0"));
    }
}), Test_testCase("clamp01 should return 1.0 if value is above range", () => {
    let copyOfStruct_5, arg_10, arg_1_5;
    const actual_5 = (1.1 > 1) ? 1 : ((1.1 < 0) ? 0 : 1.1);
    if ((actual_5 === 1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_5, 1, "Expected 1.1 to be clamped to 1.0");
    }
    else {
        throw new Error(contains((copyOfStruct_5 = actual_5, float64_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_10 = (1).toString(), (arg_1_5 = actual_5.toString(), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_10)(arg_1_5)("Expected 1.1 to be clamped to 1.0")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(1)(actual_5)("Expected 1.1 to be clamped to 1.0"));
    }
}), Test_testCase("acos should return the correct value for a valid input", () => {
    let d, x_13;
    Expect_floatClose(AccuracyModule_high, Number.isNaN(0.5) ? DictFail_failNaN("acos") : ((0.5 < -1.0001) ? DictFail_fail11(0.5, "acos") : ((0.5 > 1.0001) ? DictFail_fail11(0.5, "acos") : ((d = ((x_13 = 0.5, (1 < -1) ? DictFail_failClamp(1, -1) : ((x_13 > 1) ? 1 : ((x_13 < -1) ? -1 : x_13)))), Math.acos(d))))), Math.acos(0.5), "Expected acos(0.5) to match Math.Acos(0.5)");
}), Test_testCase("acos should throw for NaN input", () => {
    Expect_throws(() => {
        let d_1, x_15;
        Number.isNaN(NaN) ? DictFail_failNaN("acos") : ((NaN < -1.0001) ? DictFail_fail11(NaN, "acos") : ((NaN > 1.0001) ? DictFail_fail11(NaN, "acos") : ((d_1 = ((x_15 = NaN, (1 < -1) ? DictFail_failClamp(1, -1) : ((x_15 > 1) ? 1 : ((x_15 < -1) ? -1 : x_15)))), Math.acos(d_1)))));
    }, "Expected exception for NaN input");
}), Test_testCase("acos should throw for input less than -1.0001", () => {
    Expect_throws(() => {
        let d_2, x_17;
        Number.isNaN(-1.0002) ? DictFail_failNaN("acos") : ((-1.0002 < -1.0001) ? DictFail_fail11(-1.0002, "acos") : ((-1.0002 > 1.0001) ? DictFail_fail11(-1.0002, "acos") : ((d_2 = ((x_17 = -1.0002, (1 < -1) ? DictFail_failClamp(1, -1) : ((x_17 > 1) ? 1 : ((x_17 < -1) ? -1 : x_17)))), Math.acos(d_2)))));
    }, "Expected exception for input less than -1.0001");
}), Test_testCase("acos should throw for input greater than 1.0001", () => {
    Expect_throws(() => {
        let d_3, x_19;
        Number.isNaN(1.0002) ? DictFail_failNaN("acos") : ((1.0002 < -1.0001) ? DictFail_fail11(1.0002, "acos") : ((1.0002 > 1.0001) ? DictFail_fail11(1.0002, "acos") : ((d_3 = ((x_19 = 1.0002, (1 < -1) ? DictFail_failClamp(1, -1) : ((x_19 > 1) ? 1 : ((x_19 < -1) ? -1 : x_19)))), Math.acos(d_3)))));
    }, "Expected exception for input greater than 1.0001");
}), Test_testCase("asin should return the correct value for a valid input", () => {
    let d_4, x_21;
    Expect_floatClose(AccuracyModule_high, Number.isNaN(0.5) ? DictFail_failNaN("asin") : ((0.5 < -1.0001) ? DictFail_fail11(0.5, "asin") : ((0.5 > 1.0001) ? DictFail_fail11(0.5, "asin") : ((d_4 = ((x_21 = 0.5, (1 < -1) ? DictFail_failClamp(1, -1) : ((x_21 > 1) ? 1 : ((x_21 < -1) ? -1 : x_21)))), Math.asin(d_4))))), Math.asin(0.5), "Expected asin(0.5) to match Math.Asin(0.5)");
}), Test_testCase("asin should throw for NaN input", () => {
    Expect_throws(() => {
        let d_5, x_23;
        Number.isNaN(NaN) ? DictFail_failNaN("asin") : ((NaN < -1.0001) ? DictFail_fail11(NaN, "asin") : ((NaN > 1.0001) ? DictFail_fail11(NaN, "asin") : ((d_5 = ((x_23 = NaN, (1 < -1) ? DictFail_failClamp(1, -1) : ((x_23 > 1) ? 1 : ((x_23 < -1) ? -1 : x_23)))), Math.asin(d_5)))));
    }, "Expected exception for NaN input");
}), Test_testCase("asin should throw for input less than -1.0001", () => {
    Expect_throws(() => {
        let d_6, x_25;
        Number.isNaN(-1.0002) ? DictFail_failNaN("asin") : ((-1.0002 < -1.0001) ? DictFail_fail11(-1.0002, "asin") : ((-1.0002 > 1.0001) ? DictFail_fail11(-1.0002, "asin") : ((d_6 = ((x_25 = -1.0002, (1 < -1) ? DictFail_failClamp(1, -1) : ((x_25 > 1) ? 1 : ((x_25 < -1) ? -1 : x_25)))), Math.asin(d_6)))));
    }, "Expected exception for input less than -1.0001");
}), Test_testCase("asin should throw for input greater than 1.0001", () => {
    Expect_throws(() => {
        let d_7, x_27;
        Number.isNaN(1.0002) ? DictFail_failNaN("asin") : ((1.0002 < -1.0001) ? DictFail_fail11(1.0002, "asin") : ((1.0002 > 1.0001) ? DictFail_fail11(1.0002, "asin") : ((d_7 = ((x_27 = 1.0002, (1 < -1) ? DictFail_failClamp(1, -1) : ((x_27 > 1) ? 1 : ((x_27 < -1) ? -1 : x_27)))), Math.asin(d_7)))));
    }, "Expected exception for input greater than 1.0001");
}), Test_testCase("isNanOrInf should return true for NaN", () => {
    Expect_isTrue(isInfinity(NaN) ? true : Number.isNaN(NaN))("Expected NaN to be identified as NaN or Infinity");
}), Test_testCase("isNanOrInf should return true for Infinity", () => {
    Expect_isTrue(isInfinity(Infinity) ? true : Number.isNaN(Infinity))("Expected Infinity to be identified as NaN or Infinity");
}), Test_testCase("isNanOrInf should return false for a normal number", () => {
    Expect_isFalse(isInfinity(1) ? true : Number.isNaN(1))("Expected 1.0 to not be identified as NaN or Infinity");
}), Test_testCase("isNan should return true for NaN", () => {
    Expect_isTrue(Number.isNaN(NaN))("Expected NaN to be identified as NaN");
}), Test_testCase("isNan should return false for a normal number", () => {
    Expect_isFalse(Number.isNaN(1))("Expected 1.0 to not be identified as NaN");
}), Test_testCase("clamp should return the value if within range", () => {
    let copyOfStruct_6, arg_11, arg_1_6;
    const actual_6 = (10 < 0) ? DictFail_failClamp(10, 0) : ((5 > 10) ? 10 : ((5 < 0) ? 0 : 5));
    if ((actual_6 === 5) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_6, 5, "Expected 5.0 to be within the range 0.0 to 10.0");
    }
    else {
        throw new Error(contains((copyOfStruct_6 = actual_6, float64_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_11 = (5).toString(), (arg_1_6 = actual_6.toString(), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_11)(arg_1_6)("Expected 5.0 to be within the range 0.0 to 10.0")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(5)(actual_6)("Expected 5.0 to be within the range 0.0 to 10.0"));
    }
}), Test_testCase("clamp should return minVal if value is below range", () => {
    let copyOfStruct_7, arg_12, arg_1_7;
    const actual_7 = (10 < 0) ? DictFail_failClamp(10, 0) : ((-1 > 10) ? 10 : ((-1 < 0) ? 0 : -1));
    if ((actual_7 === 0) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_7, 0, "Expected -1.0 to be clamped to 0.0");
    }
    else {
        throw new Error(contains((copyOfStruct_7 = actual_7, float64_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_12 = (0).toString(), (arg_1_7 = actual_7.toString(), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_12)(arg_1_7)("Expected -1.0 to be clamped to 0.0")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(0)(actual_7)("Expected -1.0 to be clamped to 0.0"));
    }
}), Test_testCase("clamp should return maxVal if value is above range", () => {
    let copyOfStruct_8, arg_13, arg_1_8;
    const actual_8 = (10 < 0) ? DictFail_failClamp(10, 0) : ((11 > 10) ? 10 : ((11 < 0) ? 0 : 11));
    if ((actual_8 === 10) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_8, 10, "Expected 11.0 to be clamped to 10.0");
    }
    else {
        throw new Error(contains((copyOfStruct_8 = actual_8, float64_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_13 = (10).toString(), (arg_1_8 = actual_8.toString(), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_13)(arg_1_8)("Expected 11.0 to be clamped to 10.0")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(10)(actual_8)("Expected 11.0 to be clamped to 10.0"));
    }
}), Test_testCase("clamp01 should return the value if within range", () => {
    let copyOfStruct_9, arg_14, arg_1_9;
    const actual_9 = (0.5 > 1) ? 1 : ((0.5 < 0) ? 0 : 0.5);
    if ((actual_9 === 0.5) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_9, 0.5, "Expected 0.5 to be within the range 0.0 to 1.0");
    }
    else {
        throw new Error(contains((copyOfStruct_9 = actual_9, float64_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_14 = (0.5).toString(), (arg_1_9 = actual_9.toString(), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_14)(arg_1_9)("Expected 0.5 to be within the range 0.0 to 1.0")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(0.5)(actual_9)("Expected 0.5 to be within the range 0.0 to 1.0"));
    }
}), Test_testCase("clamp01 should return 0.0 if value is below range", () => {
    let copyOfStruct_10, arg_15, arg_1_10;
    const actual_10 = (-0.1 > 1) ? 1 : ((-0.1 < 0) ? 0 : -0.1);
    if ((actual_10 === 0) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_10, 0, "Expected -0.1 to be clamped to 0.0");
    }
    else {
        throw new Error(contains((copyOfStruct_10 = actual_10, float64_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_15 = (0).toString(), (arg_1_10 = actual_10.toString(), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_15)(arg_1_10)("Expected -0.1 to be clamped to 0.0")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(0)(actual_10)("Expected -0.1 to be clamped to 0.0"));
    }
}), Test_testCase("clamp01 should return 1.0 if value is above range", () => {
    let copyOfStruct_11, arg_16, arg_1_11;
    const actual_11 = (1.1 > 1) ? 1 : ((1.1 < 0) ? 0 : 1.1);
    if ((actual_11 === 1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_11, 1, "Expected 1.1 to be clamped to 1.0");
    }
    else {
        throw new Error(contains((copyOfStruct_11 = actual_11, float64_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_16 = (1).toString(), (arg_1_11 = actual_11.toString(), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_16)(arg_1_11)("Expected 1.1 to be clamped to 1.0")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(1)(actual_11)("Expected 1.1 to be clamped to 1.0"));
    }
}), Test_testCase("acos should return the correct value for a valid input", () => {
    let d_8, x_41;
    Expect_floatClose(AccuracyModule_high, Number.isNaN(0.5) ? DictFail_failNaN("acos") : ((0.5 < -1.0001) ? DictFail_fail11(0.5, "acos") : ((0.5 > 1.0001) ? DictFail_fail11(0.5, "acos") : ((d_8 = ((x_41 = 0.5, (1 < -1) ? DictFail_failClamp(1, -1) : ((x_41 > 1) ? 1 : ((x_41 < -1) ? -1 : x_41)))), Math.acos(d_8))))), Math.acos(0.5), "Expected acos(0.5) to match Math.Acos(0.5)");
}), Test_testCase("acos should throw for NaN input", () => {
    Expect_throws(() => {
        let d_9, x_43;
        Number.isNaN(NaN) ? DictFail_failNaN("acos") : ((NaN < -1.0001) ? DictFail_fail11(NaN, "acos") : ((NaN > 1.0001) ? DictFail_fail11(NaN, "acos") : ((d_9 = ((x_43 = NaN, (1 < -1) ? DictFail_failClamp(1, -1) : ((x_43 > 1) ? 1 : ((x_43 < -1) ? -1 : x_43)))), Math.acos(d_9)))));
    }, "Expected exception for NaN input");
}), Test_testCase("acos should throw for input less than -1.0001", () => {
    Expect_throws(() => {
        let d_10, x_45;
        Number.isNaN(-1.0002) ? DictFail_failNaN("acos") : ((-1.0002 < -1.0001) ? DictFail_fail11(-1.0002, "acos") : ((-1.0002 > 1.0001) ? DictFail_fail11(-1.0002, "acos") : ((d_10 = ((x_45 = -1.0002, (1 < -1) ? DictFail_failClamp(1, -1) : ((x_45 > 1) ? 1 : ((x_45 < -1) ? -1 : x_45)))), Math.acos(d_10)))));
    }, "Expected exception for input less than -1.0001");
}), Test_testCase("acos should throw for input greater than 1.0001", () => {
    Expect_throws(() => {
        let d_11, x_47;
        Number.isNaN(1.0002) ? DictFail_failNaN("acos") : ((1.0002 < -1.0001) ? DictFail_fail11(1.0002, "acos") : ((1.0002 > 1.0001) ? DictFail_fail11(1.0002, "acos") : ((d_11 = ((x_47 = 1.0002, (1 < -1) ? DictFail_failClamp(1, -1) : ((x_47 > 1) ? 1 : ((x_47 < -1) ? -1 : x_47)))), Math.acos(d_11)))));
    }, "Expected exception for input greater than 1.0001");
}), Test_testCase("asin should return the correct value for a valid input", () => {
    let d_12, x_49;
    Expect_floatClose(AccuracyModule_high, Number.isNaN(0.5) ? DictFail_failNaN("asin") : ((0.5 < -1.0001) ? DictFail_fail11(0.5, "asin") : ((0.5 > 1.0001) ? DictFail_fail11(0.5, "asin") : ((d_12 = ((x_49 = 0.5, (1 < -1) ? DictFail_failClamp(1, -1) : ((x_49 > 1) ? 1 : ((x_49 < -1) ? -1 : x_49)))), Math.asin(d_12))))), Math.asin(0.5), "Expected asin(0.5) to match Math.Asin(0.5)");
}), Test_testCase("asin should throw for NaN input", () => {
    Expect_throws(() => {
        let d_13, x_51;
        Number.isNaN(NaN) ? DictFail_failNaN("asin") : ((NaN < -1.0001) ? DictFail_fail11(NaN, "asin") : ((NaN > 1.0001) ? DictFail_fail11(NaN, "asin") : ((d_13 = ((x_51 = NaN, (1 < -1) ? DictFail_failClamp(1, -1) : ((x_51 > 1) ? 1 : ((x_51 < -1) ? -1 : x_51)))), Math.asin(d_13)))));
    }, "Expected exception for NaN input");
}), Test_testCase("asin should throw for input less than -1.0001", () => {
    Expect_throws(() => {
        let d_14, x_53;
        Number.isNaN(-1.0002) ? DictFail_failNaN("asin") : ((-1.0002 < -1.0001) ? DictFail_fail11(-1.0002, "asin") : ((-1.0002 > 1.0001) ? DictFail_fail11(-1.0002, "asin") : ((d_14 = ((x_53 = -1.0002, (1 < -1) ? DictFail_failClamp(1, -1) : ((x_53 > 1) ? 1 : ((x_53 < -1) ? -1 : x_53)))), Math.asin(d_14)))));
    }, "Expected exception for input less than -1.0001");
}), Test_testCase("asin should throw for input greater than 1.0001", () => {
    Expect_throws(() => {
        let d_15, x_55;
        Number.isNaN(1.0002) ? DictFail_failNaN("asin") : ((1.0002 < -1.0001) ? DictFail_fail11(1.0002, "asin") : ((1.0002 > 1.0001) ? DictFail_fail11(1.0002, "asin") : ((d_15 = ((x_55 = 1.0002, (1 < -1) ? DictFail_failClamp(1, -1) : ((x_55 > 1) ? 1 : ((x_55 < -1) ? -1 : x_55)))), Math.asin(d_15)))));
    }, "Expected exception for input greater than 1.0001");
}), Test_testCase("toRadians should convert degrees to radians correctly", () => {
    Expect_floatClose(AccuracyModule_high, 0.017453292519943295 * 180, 3.141592653589793, "Expected 180 degrees to be PI radians");
}), Test_testCase("toDegrees should convert radians to degrees correctly", () => {
    Expect_floatClose(AccuracyModule_high, 57.29577951308232 * 3.141592653589793, 180, "Expected PI radians to be 180 degrees");
}), Test_testCase("isInRange should return true if value is within range", () => {
    Expect_isTrue((5 >= 0) && (5 <= 10))("Expected 5.0 to be within the range 0.0 to 10.0");
}), Test_testCase("isInRange should return false if value is outside range", () => {
    Expect_isFalse((11 >= 0) && (11 <= 10))("Expected 11.0 to be outside the range 0.0 to 10.0");
}), Test_testCase("isNotInRange should return true if value is outside range", () => {
    Expect_isTrue((11 < 0) ? true : (11 > 10))("Expected 11.0 to be outside the range 0.0 to 10.0");
}), Test_testCase("isNotInRange should return false if value is within range", () => {
    Expect_isFalse((5 < 0) ? true : (5 > 10))("Expected 5.0 to be within the range 0.0 to 10.0");
}), Test_testCase("equalsWithRelativeTolerance should return true for equal values within tolerance", () => {
    let mi;
    Expect_isTrue((mi = min(Math.abs(1), Math.abs(1.05)), Math.abs(1 - 1.05) < (0.1 * mi)))("Expected 1.0 and 1.05 to be equal within 10% tolerance");
}), Test_testCase("equalsWithRelativeTolerance should return false for values outside tolerance", () => {
    let mi_1;
    Expect_isFalse((mi_1 = min(Math.abs(1), Math.abs(1.2)), Math.abs(1 - 1.2) < (0.1 * mi_1)))("Expected 1.0 and 1.2 to not be equal within 10% tolerance");
}), Test_testCase("equalsWithTolerance should return true for equal values within tolerance", () => {
    Expect_isTrue(Math.abs(1 - 1.05) < 0.1)("Expected 1.0 and 1.05 to be equal within 0.1 tolerance");
}), Test_testCase("equalsWithTolerance should return false for values outside tolerance", () => {
    Expect_isFalse(Math.abs(1 - 1.2) < 0.1)("Expected 1.0 and 1.2 to not be equal within 0.1 tolerance");
}), Test_testCase("interpolate should return correct interpolated value", () => {
    let copyOfStruct_12, arg_17, arg_1_12;
    const actual_12 = 0 + ((10 - 0) * 0.5);
    if ((actual_12 === 5) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_12, 5, "Expected interpolation between 0.0 and 10.0 at 0.5 to be 5.0");
    }
    else {
        throw new Error(contains((copyOfStruct_12 = actual_12, float64_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_17 = (5).toString(), (arg_1_12 = actual_12.toString(), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_17)(arg_1_12)("Expected interpolation between 0.0 and 10.0 at 0.5 to be 5.0")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(5)(actual_12)("Expected interpolation between 0.0 and 10.0 at 0.5 to be 5.0"));
    }
}), Test_testCase("reciprocal should return correct reciprocal value", () => {
    let a, copyOfStruct_13, arg_18, arg_1_13;
    let actual_13;
    actual_13 = ((Number.isNaN(2) ? DictFail_failNaN("reciprocal") : undefined, (a = Math.abs(2), ((a < 1E-18) ? DictFail_failTiny18("reciprocal") : undefined, ((a > 1E+36) ? DictFail_failBig36("reciprocal") : undefined, 1 / 2)))));
    if ((actual_13 === 0.5) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_13, 0.5, "Expected reciprocal of 2.0 to be 0.5");
    }
    else {
        throw new Error(contains((copyOfStruct_13 = actual_13, float64_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_18 = (0.5).toString(), (arg_1_13 = actual_13.toString(), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_18)(arg_1_13)("Expected reciprocal of 2.0 to be 0.5")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(0.5)(actual_13)("Expected reciprocal of 2.0 to be 0.5"));
    }
}), Test_testCase("inverse should return correct inverse value", () => {
    let a_1, copyOfStruct_14, arg_19, arg_1_14;
    let actual_14;
    actual_14 = ((Number.isNaN(2) ? DictFail_failNaN("inverse") : undefined, (a_1 = Math.abs(2), ((a_1 < 1E-18) ? DictFail_failTiny18("inverse") : undefined, ((a_1 > 1E+36) ? DictFail_failBig36("inverse") : undefined, 1 / 2)))));
    if ((actual_14 === 0.5) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_14, 0.5, "Expected inverse of 2.0 to be 0.5");
    }
    else {
        throw new Error(contains((copyOfStruct_14 = actual_14, float64_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_19 = (0.5).toString(), (arg_1_14 = actual_14.toString(), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_19)(arg_1_14)("Expected inverse of 2.0 to be 0.5")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(0.5)(actual_14)("Expected inverse of 2.0 to be 0.5"));
    }
}), Test_testCase("normalize should return correct normalized value", () => {
    let rangeLength, a_2;
    Expect_floatClose(AccuracyModule_high, (Number.isNaN(0) ? DictFail_failNaNv("normalize", "rangeStart") : undefined, (Number.isNaN(10) ? DictFail_failNaNv("normalize", "rangeStop") : undefined, (Number.isNaN(5) ? DictFail_failNaNv("normalize", "valueAt") : undefined, (rangeLength = (0 - 10), (a_2 = Math.abs(rangeLength), ((a_2 < 1E-18) ? DictFail_failTiny18v("normalize", "rangeStart-rangeStop") : undefined, ((a_2 > 1E+36) ? DictFail_failBig36v("normalize", "rangeStart-rangeStop") : undefined, (0 - 5) / rangeLength))))))), 0.5, "Expected normalization of 5.0 between 0.0 and 10.0 to be 0.5");
}), Test_testCase("roundDownToNextLogSteps should return correct rounded value", () => {
    let copyOfStruct_15, arg_20, arg_1_15;
    const actual_15 = Dict_roundDownToNextLogSteps(10, 1);
    if ((actual_15 === 1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_15, 1, "Expected rounded value to be 1.0");
    }
    else {
        throw new Error(contains((copyOfStruct_15 = actual_15, float64_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_20 = (1).toString(), (arg_1_15 = actual_15.toString(), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_20)(arg_1_15)("Expected rounded value to be 1.0")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(1)(actual_15)("Expected rounded value to be 1.0"));
    }
}), Test_testCase("steppedIntF should return correct stepped int value", () => {
    let f_31, copyOfStruct_16, arg_21, arg_1_16;
    let actual_16;
    if ((f_31 = 1.05, isInfinity(f_31) ? true : Number.isNaN(f_31))) {
        DictFail_failInfNaNv("steppedIntF", "value");
    }
    if (0.1 < 1E-09) {
        DictFail_failNotPos("steppedIntF", "precision", 0.1);
    }
    actual_16 = ~~(1.05 / 0.1);
    if ((actual_16 === 10) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_16, 10, "Expected stepped int value to be 10");
    }
    else {
        throw new Error(contains((copyOfStruct_16 = actual_16, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_21 = int32ToString(10), (arg_1_16 = int32ToString(actual_16), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_21)(arg_1_16)("Expected stepped int value to be 10")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(10)(actual_16)("Expected stepped int value to be 10"));
    }
}), Test_testCase("steppedInt should return correct stepped int value", () => {
    let f_32, copyOfStruct_17, arg_22, arg_1_17;
    let actual_17;
    if ((f_32 = 105, isInfinity(f_32) ? true : Number.isNaN(f_32))) {
        DictFail_failInfNaNv("steppedInt", "value");
    }
    if (10 < 1) {
        DictFail_failNotPos("steppedInt", "precision", 10);
    }
    actual_17 = (~~(105 / 10) * 10);
    if ((actual_17 === 100) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_17, 100, "Expected stepped int value to be 100");
    }
    else {
        throw new Error(contains((copyOfStruct_17 = actual_17, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_22 = int32ToString(100), (arg_1_17 = int32ToString(actual_17), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_22)(arg_1_17)("Expected stepped int value to be 100")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(100)(actual_17)("Expected stepped int value to be 100"));
    }
}), Test_testCase("steppedFloat should return correct stepped float value", () => {
    let f_33, copyOfStruct_18, arg_23, arg_1_18;
    let actual_18;
    if ((f_33 = 1.05, isInfinity(f_33) ? true : Number.isNaN(f_33))) {
        DictFail_failInfNaNv("steppedFloat", "value");
    }
    if (0.1 < 1E-09) {
        DictFail_failNotPos("steppedFloat", "precision", 0.1);
    }
    actual_18 = (~~(1.05 / 0.1) * 0.1);
    if ((actual_18 === 1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_18, 1, "Expected stepped float value to be 1.0");
    }
    else {
        throw new Error(contains((copyOfStruct_18 = actual_18, float64_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_23 = (1).toString(), (arg_1_18 = actual_18.toString(), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_23)(arg_1_18)("Expected stepped float value to be 1.0")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(1)(actual_18)("Expected stepped float value to be 1.0"));
    }
}), Test_testCase("floatRange should return correct range", () => {
    let copyOfStruct_19, arg_24, arg_1_19;
    const actual_19 = toList(Dict_floatRange_Z1EAE0AEB(0, 0.1, 0.2));
    const expected_19 = ofArray([0, 0.1, 0.2]);
    if (equals_1(actual_19, expected_19) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_19, expected_19, "Expected float range to be [0.0; 0.1; 0.2]");
    }
    else {
        throw new Error(contains((copyOfStruct_19 = actual_19, list_type(float64_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_24 = seqToString(expected_19), (arg_1_19 = seqToString(actual_19), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_24)(arg_1_19)("Expected float range to be [0.0; 0.1; 0.2]")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_19)(actual_19)("Expected float range to be [0.0; 0.1; 0.2]"));
    }
}), Test_testCase("randomStandardDeviation should return a value", () => {
    const result_45 = Dict_randomStandardDeviation_B9AD360(0, 1);
    Expect_isTrue((result_45 >= -3) && (result_45 <= 3))("Expected random value to be within 3 standard deviations");
}), Test_testCase("tryParseFloat should return Some for valid float string", () => {
    let copyOfStruct_20, arg_25, arg_1_20;
    let actual_20;
    let matchValue;
    let outArg = 0;
    matchValue = [tryParse("1.23", new FSharpRef(() => outArg, (v) => {
        outArg = v;
    })), outArg];
    actual_20 = (matchValue[0] ? matchValue[1] : undefined);
    const expected_20 = 1.23;
    if (equals_1(actual_20, expected_20) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_20, expected_20, "Expected Some 1.23 for valid float string");
    }
    else {
        throw new Error(contains((copyOfStruct_20 = actual_20, option_type(float64_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_25 = toString(expected_20), (arg_1_20 = toString(actual_20), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_25)(arg_1_20)("Expected Some 1.23 for valid float string")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_20)(actual_20)("Expected Some 1.23 for valid float string"));
    }
}), Test_testCase("tryParseFloat should return None for invalid float string", () => {
    let copyOfStruct_21, arg_26, arg_1_21;
    let actual_21;
    let matchValue_1;
    let outArg_1 = 0;
    matchValue_1 = [tryParse("abc", new FSharpRef(() => outArg_1, (v_1) => {
        outArg_1 = v_1;
    })), outArg_1];
    actual_21 = (matchValue_1[0] ? matchValue_1[1] : undefined);
    const expected_21 = undefined;
    if (equals_1(actual_21, expected_21) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_21, expected_21, "Expected None for invalid float string");
    }
    else {
        throw new Error(contains((copyOfStruct_21 = actual_21, option_type(float64_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_26 = toString(expected_21), (arg_1_21 = toString(actual_21), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_26)(arg_1_21)("Expected None for invalid float string")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_21)(actual_21)("Expected None for invalid float string"));
    }
}), Test_testCase("tryParseFloatDe should return Some for valid float string with comma", () => {
    let copyOfStruct_22, arg_27, arg_1_22;
    let actual_22;
    let matchValue_2;
    let outArg_2 = 0;
    matchValue_2 = [tryParse(replace("1,23", ",", "."), new FSharpRef(() => outArg_2, (v_2) => {
        outArg_2 = v_2;
    })), outArg_2];
    actual_22 = (matchValue_2[0] ? matchValue_2[1] : undefined);
    const expected_22 = 1.23;
    if (equals_1(actual_22, expected_22) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_22, expected_22, "Expected Some 1.23 for valid float string with comma");
    }
    else {
        throw new Error(contains((copyOfStruct_22 = actual_22, option_type(float64_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_27 = toString(expected_22), (arg_1_22 = toString(actual_22), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_27)(arg_1_22)("Expected Some 1.23 for valid float string with comma")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_22)(actual_22)("Expected Some 1.23 for valid float string with comma"));
    }
}), Test_testCase("tryParseFloatDe should return None for invalid float string", () => {
    let copyOfStruct_23, arg_28, arg_1_23;
    let actual_23;
    let matchValue_3;
    let outArg_3 = 0;
    matchValue_3 = [tryParse(replace("abc", ",", "."), new FSharpRef(() => outArg_3, (v_3) => {
        outArg_3 = v_3;
    })), outArg_3];
    actual_23 = (matchValue_3[0] ? matchValue_3[1] : undefined);
    const expected_23 = undefined;
    if (equals_1(actual_23, expected_23) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_23, expected_23, "Expected None for invalid float string");
    }
    else {
        throw new Error(contains((copyOfStruct_23 = actual_23, option_type(float64_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_28 = toString(expected_23), (arg_1_23 = toString(actual_23), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_28)(arg_1_23)("Expected None for invalid float string")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_23)(actual_23)("Expected None for invalid float string"));
    }
}), Test_testCase("tryParseFloatTolerant should return Some for valid float string with non-numeric characters", () => {
    let copyOfStruct_24, arg_29, arg_1_24;
    const actual_24 = Dict_tryParseFloatTolerant_Z721C83C5("1.23abc");
    const expected_24 = 1.23;
    if (equals_1(actual_24, expected_24) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_24, expected_24, "Expected Some 1.23 for valid float string with non-numeric characters");
    }
    else {
        throw new Error(contains((copyOfStruct_24 = actual_24, option_type(float64_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_29 = toString(expected_24), (arg_1_24 = toString(actual_24), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_29)(arg_1_24)("Expected Some 1.23 for valid float string with non-numeric characters")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_24)(actual_24)("Expected Some 1.23 for valid float string with non-numeric characters"));
    }
}), Test_testCase("tryParseFloatTolerant should return None for invalid float string", () => {
    let copyOfStruct_25, arg_30, arg_1_25;
    const actual_25 = Dict_tryParseFloatTolerant_Z721C83C5("abc");
    const expected_25 = undefined;
    if (equals_1(actual_25, expected_25) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_25, expected_25, "Expected None for invalid float string");
    }
    else {
        throw new Error(contains((copyOfStruct_25 = actual_25, option_type(float64_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_30 = toString(expected_25), (arg_1_25 = toString(actual_25), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_30)(arg_1_25)("Expected None for invalid float string")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_25)(actual_25)("Expected None for invalid float string"));
    }
}), (() => {
    const name_64 = "tryParseFloatDeTolerant should return Some for valid float string with comma and non-numeric characters";
    return Test_testCase(name_64, () => {
        let copyOfStruct_26, arg_31, arg_1_26;
        const actual_26 = Dict_tryParseFloatDeTolerant_Z721C83C5("1,23abc");
        const expected_26 = 1.23;
        if (equals_1(actual_26, expected_26) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
            assertEqual(actual_26, expected_26, "Expected Some 1.23 for valid float string with comma and non-numeric characters");
        }
        else {
            throw new Error(contains((copyOfStruct_26 = actual_26, option_type(float64_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
                Equals: equals,
                GetHashCode: structuralHash,
            }) ? ((arg_31 = toString(expected_26), (arg_1_26 = toString(actual_26), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_31)(arg_1_26)("Expected Some 1.23 for valid float string with comma and non-numeric characters")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_26)(actual_26)("Expected Some 1.23 for valid float string with comma and non-numeric characters"));
        }
    });
})(), Test_testCase("tryParseFloatDeTolerant should return None for invalid float string", () => {
    let copyOfStruct_27, arg_32, arg_1_27;
    const actual_27 = Dict_tryParseFloatDeTolerant_Z721C83C5("abc");
    const expected_27 = undefined;
    if (equals_1(actual_27, expected_27) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_27, expected_27, "Expected None for invalid float string");
    }
    else {
        throw new Error(contains((copyOfStruct_27 = actual_27, option_type(float64_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_32 = toString(expected_27), (arg_1_27 = toString(actual_27), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_32)(arg_1_27)("Expected None for invalid float string")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_27)(actual_27)("Expected None for invalid float string"));
    }
}), Test_testCase("interpolateTable should interpolate correctly within the table range", () => {
    let copyOfStruct_28, arg_33, arg_1_28;
    const actual_28 = Dict_interpolateTable_16FC164C([[0, 0], [1, 2], [2, 4]])(1.5);
    if ((actual_28 === 3) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_28, 3, "Expected interpolation at 1.5 to be 3.0");
    }
    else {
        throw new Error(contains((copyOfStruct_28 = actual_28, float64_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_33 = (3).toString(), (arg_1_28 = actual_28.toString(), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_33)(arg_1_28)("Expected interpolation at 1.5 to be 3.0")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(3)(actual_28)("Expected interpolation at 1.5 to be 3.0"));
    }
}), Test_testCase("interpolateTable should interpolate correctly within the table range", () => {
    let copyOfStruct_29, arg_34, arg_1_29;
    const actual_29 = Dict_interpolateTable_16FC164C([[0, 0], [1, 2], [2, 4]])(3);
    if ((actual_29 === 6) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_29, 6, "Expected interpolation at 3.0 to be 6.0");
    }
    else {
        throw new Error(contains((copyOfStruct_29 = actual_29, float64_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_34 = (6).toString(), (arg_1_29 = actual_29.toString(), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_34)(arg_1_29)("Expected interpolation at 3.0 to be 6.0")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(6)(actual_29)("Expected interpolation at 3.0 to be 6.0"));
    }
}), Test_testCase("interpolateTable should interpolate correctly within the table range", () => {
    let copyOfStruct_30, arg_35, arg_1_30;
    const actual_30 = Dict_interpolateTable_16FC164C([[0, 0], [1, 2], [2, 4]])(-0.5);
    if ((actual_30 === -1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_30, -1, "Expected interpolation at -0.5 to be -1");
    }
    else {
        throw new Error(contains((copyOfStruct_30 = actual_30, float64_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_35 = (-1).toString(), (arg_1_30 = actual_30.toString(), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_35)(arg_1_30)("Expected interpolation at -0.5 to be -1")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(-1)(actual_30)("Expected interpolation at -0.5 to be -1"));
    }
})]));

