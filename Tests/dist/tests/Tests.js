import { Expect_isFalse, Expect_throws, Expect_isTrue, Test_testCase, Test_testList } from "./fable_modules/Fable.Mocha.2.17.0/Mocha.fs.js";
import { Dict$2__GetOrSetDefaultValue, Dict$2__GetOrSetDefault, Dict$2__AddIfKeyAbsent, Dict$2__SetIfKeyAbsent, Dict$2__get_Values, Dict$2__get_Keys, Dict$2__get_Count, Dict$2__ContainsKey_2B595, Dict$2__Clear, Dict$2__Remove_2B595, Dict$2__Set, Dict$2__Get_2B595, Dict$2__Add$0027, Dict$2__get_Item_2B595, Dict$2__DoesNotContainKey_2B595, Dict$2__Pop_2B595, Dict$2__set_Item_5BDDA1, Dict$2_$ctor, Dict$2__get_IsEmpty } from "./Src/Dict.js";
import { count, safeHash, clear, equals as equals_1, defaultOf, int32ToString, structuralHash, assertEqual } from "./fable_modules/fable-library-js.4.22.0/Util.js";
import { ofArray, contains } from "./fable_modules/fable-library-js.4.22.0/List.js";
import { list_type, equals, class_type, decimal_type, string_type, float64_type, bool_type, int32_type } from "./fable_modules/fable-library-js.4.22.0/Reflection.js";
import { printf, toText } from "./fable_modules/fable-library-js.4.22.0/String.js";
import { contains as contains_1, toList } from "./fable_modules/fable-library-js.4.22.0/Seq.js";
import { toString, seqToString } from "./fable_modules/fable-library-js.4.22.0/Types.js";
import { item, fill, removeInPlace } from "./fable_modules/fable-library-js.4.22.0/Array.js";

export const tests = Test_testList("Module.fs Tests", ofArray([Test_testCase("Empty", () => {
    Expect_isTrue(Dict$2__get_IsEmpty(Dict$2_$ctor()))("Empty returns true for empty dictionary");
}), Test_testCase("Pop", () => {
    const b_1 = Dict$2_$ctor();
    Dict$2__set_Item_5BDDA1(b_1, "A", 1);
    const popped = Dict$2__Pop_2B595(b_1, "A") | 0;
    Expect_isTrue(Dict$2__DoesNotContainKey_2B595(b_1, "A") && (popped === 1))("Pop removed key A");
}), Test_testCase("Item", () => {
    let copyOfStruct, arg, arg_1;
    const b_2 = Dict$2_$ctor();
    Dict$2__set_Item_5BDDA1(b_2, "A", 1);
    const actual = Dict$2__get_Item_2B595(b_2, "A") | 0;
    if ((actual === 1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual, 1, "Item returns value of key A");
    }
    else {
        throw new Error(contains((copyOfStruct = actual, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg = int32ToString(1), (arg_1 = int32ToString(actual), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg)(arg_1)("Item returns value of key A")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(1)(actual)("Item returns value of key A"));
    }
}), Test_testCase("Add", () => {
    let copyOfStruct_1, arg_6, arg_1_1;
    const b_3 = Dict$2_$ctor();
    Dict$2__Add$0027(b_3, "A", 1);
    const actual_1 = Dict$2__Get_2B595(b_3, "A") | 0;
    if ((actual_1 === 1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_1, 1, "Add adds key A with value 1");
    }
    else {
        throw new Error(contains((copyOfStruct_1 = actual_1, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_6 = int32ToString(1), (arg_1_1 = int32ToString(actual_1), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_6)(arg_1_1)("Add adds key A with value 1")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(1)(actual_1)("Add adds key A with value 1"));
    }
}), Test_testCase("Item fails", () => {
    const b_4 = Dict$2_$ctor();
    Dict$2__set_Item_5BDDA1(b_4, "A", 1);
    Expect_throws(() => {
        Dict$2__Get_2B595(b_4, "B");
    }, "Item throws exception when key does not exist");
}), Test_testCase("Add fails", () => {
    const b_5 = Dict$2_$ctor();
    Expect_throws(() => {
        Dict$2__Set(b_5, defaultOf(), 1);
    }, "Add throws exception when key does not exist");
}), Test_testCase("Remove", () => {
    const b_6 = Dict$2_$ctor();
    Dict$2__set_Item_5BDDA1(b_6, "A", 1);
    const removed = Dict$2__Remove_2B595(b_6, "A");
    Expect_isTrue(Dict$2__DoesNotContainKey_2B595(b_6, "A") && removed)("Remove deletes key A");
}), Test_testCase("Remove2", () => {
    const b_7 = Dict$2_$ctor();
    const removed_1 = Dict$2__Remove_2B595(b_7, "A");
    Expect_isTrue(Dict$2__DoesNotContainKey_2B595(b_7, "A") && !removed_1)("Remove missing key A");
}), Test_testCase("Clear", () => {
    const b_8 = Dict$2_$ctor();
    Dict$2__set_Item_5BDDA1(b_8, "A", 1);
    Dict$2__set_Item_5BDDA1(b_8, "B", 2);
    Dict$2__Clear(b_8);
    const resultA = Dict$2__DoesNotContainKey_2B595(b_8, "A");
    const resultB = Dict$2__DoesNotContainKey_2B595(b_8, "B");
    Expect_isTrue(resultA)("Clear removes key A");
    Expect_isTrue(resultB)("Clear removes key B");
}), Test_testCase("ContainsKey", () => {
    const b_9 = Dict$2_$ctor();
    Dict$2__set_Item_5BDDA1(b_9, "A", 1);
    Expect_isTrue(Dict$2__ContainsKey_2B595(b_9, "A"))("ContainsKey returns true for existing key");
}), Test_testCase("Count", () => {
    let copyOfStruct_2, arg_7, arg_1_2;
    const b_10 = Dict$2_$ctor();
    Dict$2__set_Item_5BDDA1(b_10, "A", 1);
    Dict$2__set_Item_5BDDA1(b_10, "B", 2);
    const actual_2 = Dict$2__get_Count(b_10) | 0;
    if ((actual_2 === 2) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_2, 2, "Count returns the number of key-value pairs");
    }
    else {
        throw new Error(contains((copyOfStruct_2 = actual_2, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_7 = int32ToString(2), (arg_1_2 = int32ToString(actual_2), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_7)(arg_1_2)("Count returns the number of key-value pairs")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(2)(actual_2)("Count returns the number of key-value pairs"));
    }
}), Test_testCase("Keys", () => {
    let copyOfStruct_3, arg_8, arg_1_3;
    const b_11 = Dict$2_$ctor();
    Dict$2__set_Item_5BDDA1(b_11, "A", 1);
    Dict$2__set_Item_5BDDA1(b_11, "B", 2);
    const actual_3 = toList(Dict$2__get_Keys(b_11));
    const expected_3 = ofArray(["A", "B"]);
    if (equals_1(actual_3, expected_3) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_3, expected_3, "Keys returns the keys");
    }
    else {
        throw new Error(contains((copyOfStruct_3 = actual_3, list_type(string_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_8 = seqToString(expected_3), (arg_1_3 = seqToString(actual_3), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_8)(arg_1_3)("Keys returns the keys")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_3)(actual_3)("Keys returns the keys"));
    }
}), Test_testCase("Values", () => {
    let copyOfStruct_4, arg_9, arg_1_4;
    const b_12 = Dict$2_$ctor();
    Dict$2__set_Item_5BDDA1(b_12, "A", 1);
    Dict$2__set_Item_5BDDA1(b_12, "B", 2);
    const actual_4 = toList(Dict$2__get_Values(b_12));
    const expected_4 = ofArray([1, 2]);
    if (equals_1(actual_4, expected_4) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_4, expected_4, "Values returns the values");
    }
    else {
        throw new Error(contains((copyOfStruct_4 = actual_4, list_type(int32_type)), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_9 = seqToString(expected_4), (arg_1_4 = seqToString(actual_4), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_9)(arg_1_4)("Values returns the values")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_4)(actual_4)("Values returns the values"));
    }
}), Test_testCase("SetIfKeyAbsent - key does not exist", () => {
    let copyOfStruct_5, arg_10, arg_1_5;
    const b_13 = Dict$2_$ctor();
    Expect_isTrue(Dict$2__SetIfKeyAbsent(b_13, "A", 1))("SetIfKeyAbsent should return true when key does not exist");
    const actual_5 = Dict$2__Get_2B595(b_13, "A") | 0;
    if ((actual_5 === 1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_5, 1, "SetIfKeyAbsent should set the value when key does not exist");
    }
    else {
        throw new Error(contains((copyOfStruct_5 = actual_5, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_10 = int32ToString(1), (arg_1_5 = int32ToString(actual_5), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_10)(arg_1_5)("SetIfKeyAbsent should set the value when key does not exist")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(1)(actual_5)("SetIfKeyAbsent should set the value when key does not exist"));
    }
}), Test_testCase("SetIfKeyAbsent - key exists", () => {
    let copyOfStruct_6, arg_11, arg_1_6;
    const b_14 = Dict$2_$ctor();
    Dict$2__set_Item_5BDDA1(b_14, "A", 1);
    Expect_isFalse(Dict$2__SetIfKeyAbsent(b_14, "A", 2))("SetIfKeyAbsent should return false when key exists");
    const actual_6 = Dict$2__Get_2B595(b_14, "A") | 0;
    if ((actual_6 === 1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_6, 1, "SetIfKeyAbsent should not change the value when key exists");
    }
    else {
        throw new Error(contains((copyOfStruct_6 = actual_6, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_11 = int32ToString(1), (arg_1_6 = int32ToString(actual_6), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_11)(arg_1_6)("SetIfKeyAbsent should not change the value when key exists")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(1)(actual_6)("SetIfKeyAbsent should not change the value when key exists"));
    }
}), Test_testCase("AddIfKeyAbsent - key does not exist", () => {
    let copyOfStruct_7, arg_12, arg_1_7;
    const b_15 = Dict$2_$ctor();
    Expect_isTrue(Dict$2__AddIfKeyAbsent(b_15, "A", 1))("AddIfKeyAbsent should return true when key does not exist");
    const actual_7 = Dict$2__Get_2B595(b_15, "A") | 0;
    if ((actual_7 === 1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_7, 1, "AddIfKeyAbsent should set the value when key does not exist");
    }
    else {
        throw new Error(contains((copyOfStruct_7 = actual_7, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_12 = int32ToString(1), (arg_1_7 = int32ToString(actual_7), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_12)(arg_1_7)("AddIfKeyAbsent should set the value when key does not exist")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(1)(actual_7)("AddIfKeyAbsent should set the value when key does not exist"));
    }
}), Test_testCase("AddIfKeyAbsent - key exists", () => {
    let copyOfStruct_8, arg_13, arg_1_8;
    const b_16 = Dict$2_$ctor();
    Dict$2__set_Item_5BDDA1(b_16, "A", 1);
    Expect_isFalse(Dict$2__AddIfKeyAbsent(b_16, "A", 2))("AddIfKeyAbsent should return false when key exists");
    const actual_8 = Dict$2__Get_2B595(b_16, "A") | 0;
    if ((actual_8 === 1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_8, 1, "AddIfKeyAbsent should not change the value when key exists");
    }
    else {
        throw new Error(contains((copyOfStruct_8 = actual_8, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_13 = int32ToString(1), (arg_1_8 = int32ToString(actual_8), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_13)(arg_1_8)("AddIfKeyAbsent should not change the value when key exists")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(1)(actual_8)("AddIfKeyAbsent should not change the value when key exists"));
    }
}), Test_testCase("GetOrSetDefault - key does not exist", () => {
    let copyOfStruct_9, arg_14, arg_1_9, copyOfStruct_10, arg_15, arg_1_10;
    const b_17 = Dict$2_$ctor();
    const actual_9 = Dict$2__GetOrSetDefault(b_17, (_arg_18) => 1, "A") | 0;
    if ((actual_9 === 1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_9, 1, "GetOrSetDefault should return the default value when key does not exist");
    }
    else {
        throw new Error(contains((copyOfStruct_9 = actual_9, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_14 = int32ToString(1), (arg_1_9 = int32ToString(actual_9), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_14)(arg_1_9)("GetOrSetDefault should return the default value when key does not exist")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(1)(actual_9)("GetOrSetDefault should return the default value when key does not exist"));
    }
    const actual_10 = Dict$2__Get_2B595(b_17, "A") | 0;
    if ((actual_10 === 1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_10, 1, "GetOrSetDefault should set the default value when key does not exist");
    }
    else {
        throw new Error(contains((copyOfStruct_10 = actual_10, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_15 = int32ToString(1), (arg_1_10 = int32ToString(actual_10), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_15)(arg_1_10)("GetOrSetDefault should set the default value when key does not exist")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(1)(actual_10)("GetOrSetDefault should set the default value when key does not exist"));
    }
}), Test_testCase("GetOrSetDefault - key exists", () => {
    let copyOfStruct_11, arg_16, arg_1_11, copyOfStruct_12, arg_17, arg_1_12;
    const b_18 = Dict$2_$ctor();
    Dict$2__set_Item_5BDDA1(b_18, "A", 1);
    const actual_11 = Dict$2__GetOrSetDefault(b_18, (_arg_20) => 2, "A") | 0;
    if ((actual_11 === 1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_11, 1, "GetOrSetDefault should return the existing value when key exists");
    }
    else {
        throw new Error(contains((copyOfStruct_11 = actual_11, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_16 = int32ToString(1), (arg_1_11 = int32ToString(actual_11), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_16)(arg_1_11)("GetOrSetDefault should return the existing value when key exists")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(1)(actual_11)("GetOrSetDefault should return the existing value when key exists"));
    }
    const actual_12 = Dict$2__Get_2B595(b_18, "A") | 0;
    if ((actual_12 === 1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_12, 1, "GetOrSetDefault should not change the value when key exists");
    }
    else {
        throw new Error(contains((copyOfStruct_12 = actual_12, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_17 = int32ToString(1), (arg_1_12 = int32ToString(actual_12), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_17)(arg_1_12)("GetOrSetDefault should not change the value when key exists")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(1)(actual_12)("GetOrSetDefault should not change the value when key exists"));
    }
}), Test_testCase("GetOrSetDefaultValue - key does not exist", () => {
    let copyOfStruct_13, arg_18, arg_1_13, copyOfStruct_14, arg_19, arg_1_14;
    const b_19 = Dict$2_$ctor();
    const actual_13 = Dict$2__GetOrSetDefaultValue(b_19, 1, "A") | 0;
    if ((actual_13 === 1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_13, 1, "GetOrSetDefaultValue should return the default value when key does not exist");
    }
    else {
        throw new Error(contains((copyOfStruct_13 = actual_13, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_18 = int32ToString(1), (arg_1_13 = int32ToString(actual_13), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_18)(arg_1_13)("GetOrSetDefaultValue should return the default value when key does not exist")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(1)(actual_13)("GetOrSetDefaultValue should return the default value when key does not exist"));
    }
    const actual_14 = Dict$2__Get_2B595(b_19, "A") | 0;
    if ((actual_14 === 1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_14, 1, "GetOrSetDefaultValue should set the default value when key does not exist");
    }
    else {
        throw new Error(contains((copyOfStruct_14 = actual_14, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_19 = int32ToString(1), (arg_1_14 = int32ToString(actual_14), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_19)(arg_1_14)("GetOrSetDefaultValue should set the default value when key does not exist")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(1)(actual_14)("GetOrSetDefaultValue should set the default value when key does not exist"));
    }
}), Test_testCase("GetOrSetDefaultValue - key exists", () => {
    let copyOfStruct_15, arg_20, arg_1_15, copyOfStruct_16, arg_21, arg_1_16;
    const b_20 = Dict$2_$ctor();
    Dict$2__set_Item_5BDDA1(b_20, "A", 1);
    const actual_15 = Dict$2__GetOrSetDefaultValue(b_20, 2, "A") | 0;
    if ((actual_15 === 1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_15, 1, "GetOrSetDefaultValue should return the existing value when key exists");
    }
    else {
        throw new Error(contains((copyOfStruct_15 = actual_15, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_20 = int32ToString(1), (arg_1_15 = int32ToString(actual_15), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_20)(arg_1_15)("GetOrSetDefaultValue should return the existing value when key exists")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(1)(actual_15)("GetOrSetDefaultValue should return the existing value when key exists"));
    }
    const actual_16 = Dict$2__Get_2B595(b_20, "A") | 0;
    if ((actual_16 === 1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_16, 1, "GetOrSetDefaultValue should not change the value when key exists");
    }
    else {
        throw new Error(contains((copyOfStruct_16 = actual_16, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_21 = int32ToString(1), (arg_1_16 = int32ToString(actual_16), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_21)(arg_1_16)("GetOrSetDefaultValue should not change the value when key exists")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(1)(actual_16)("GetOrSetDefaultValue should not change the value when key exists"));
    }
}), Test_testCase("KV-Add", () => {
    let copyOfStruct_17, arg_22, arg_1_17;
    const b_21 = Dict$2_$ctor();
    void (b_21.push(["A", 1]));
    const actual_17 = Dict$2__Get_2B595(b_21, "A") | 0;
    if ((actual_17 === 1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_17, 1, "Add should add the key-value pair to the dictionary");
    }
    else {
        throw new Error(contains((copyOfStruct_17 = actual_17, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_22 = int32ToString(1), (arg_1_17 = int32ToString(actual_17), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_22)(arg_1_17)("Add should add the key-value pair to the dictionary")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(1)(actual_17)("Add should add the key-value pair to the dictionary"));
    }
}), Test_testCase("KV-Clear", () => {
    const b_22 = Dict$2_$ctor();
    void (b_22.push(["A", 1]));
    clear(b_22);
    Expect_isFalse(Dict$2__ContainsKey_2B595(b_22, "A"))("Clear should remove all key-value pairs from the dictionary");
}), Test_testCase("KV-Remove", () => {
    const b_23 = Dict$2_$ctor();
    void (b_23.push(["A", 1]));
    Expect_isTrue(removeInPlace(["A", 1], b_23, {
        Equals: equals_1,
        GetHashCode: safeHash,
    }))("Remove should return true when the key-value pair is removed");
    Expect_isFalse(Dict$2__ContainsKey_2B595(b_23, "A"))("Remove should remove the key-value pair from the dictionary");
}), Test_testCase("KV-Contains", () => {
    const b_24 = Dict$2_$ctor();
    void (b_24.push(["A", 1]));
    Expect_isTrue(contains_1(["A", 1], b_24, {
        Equals: equals_1,
        GetHashCode: safeHash,
    }))("Contains should return true when the key-value pair is in the dictionary");
}), Test_testCase("KV-CopyTo", () => {
    let copyOfStruct_18, arg_23, arg_1_18;
    const b_25 = Dict$2_$ctor();
    void (b_25.push(["A", 1]));
    const arr = fill(new Array(1), 0, 1, ["", 0]);
    b_25["System.Collections.Generic.ICollection`1.CopyToZ3B4C077E"](arr, 0);
    const actual_18 = item(0, arr);
    const expected_18 = ["A", 1];
    if (equals_1(actual_18, expected_18) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_18, expected_18, "CopyTo should copy the key-value pairs to the array");
    }
    else {
        throw new Error(contains((copyOfStruct_18 = actual_18, class_type("System.Collections.Generic.KeyValuePair`2", [string_type, int32_type])), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_23 = toString(expected_18), (arg_1_18 = toString(actual_18), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_23)(arg_1_18)("CopyTo should copy the key-value pairs to the array")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(expected_18)(actual_18)("CopyTo should copy the key-value pairs to the array"));
    }
}), Test_testCase("KV-IsReadOnly", () => {
    const b_26 = Dict$2_$ctor();
    Expect_isFalse(b_26["System.Collections.Generic.ICollection`1.get_IsReadOnly"]())("IsReadOnly should return false");
}), Test_testCase("KV-Count", () => {
    let copyOfStruct_19, arg_24, arg_1_19;
    const b_27 = Dict$2_$ctor();
    void (b_27.push(["A", 1]));
    const actual_19 = count(b_27) | 0;
    if ((actual_19 === 1) ? true : !(new Function("try {return this===window;}catch(e){ return false;}"))()) {
        assertEqual(actual_19, 1, "Count should return the number of key-value pairs in the dictionary");
    }
    else {
        throw new Error(contains((copyOfStruct_19 = actual_19, int32_type), ofArray([int32_type, bool_type, float64_type, string_type, decimal_type, class_type("System.Guid")]), {
            Equals: equals,
            GetHashCode: structuralHash,
        }) ? ((arg_24 = int32ToString(1), (arg_1_19 = int32ToString(actual_19), toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%s</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%s</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(arg_24)(arg_1_19)("Count should return the number of key-value pairs in the dictionary")))) : toText(printf("<span style=\'color:black\'>Expected:</span> <br /><div style=\'margin-left:20px; color:crimson\'>%A</div><br /><span style=\'color:black\'>Actual:</span> </br ><div style=\'margin-left:20px;color:crimson\'>%A</div><br /><span style=\'color:black\'>Message:</span> </br ><div style=\'margin-left:20px; color:crimson\'>%s</div>"))(1)(actual_19)("Count should return the number of key-value pairs in the dictionary"));
    }
})]));

