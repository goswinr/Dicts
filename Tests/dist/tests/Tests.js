import { Expect_isTrue, Test_testCase, Test_testList } from "./fable_modules/Fable.Mocha.2.17.0/Mocha.fs.js";
import { Dict$2__DoesNotContainKey_2B595, Dict$2__Pop_2B595, Dict$2__set_Item_5BDDA1, Dict$2_$ctor } from "./Src/Dict.js";
import { singleton } from "./fable_modules/fable-library-js.4.21.0/List.js";

export const tests = Test_testList("Module.fs Tests", singleton(Test_testCase("Pop", () => {
    const b = Dict$2_$ctor();
    Dict$2__set_Item_5BDDA1(b, "A", 1);
    Dict$2__Pop_2B595(b, "A");
    Expect_isTrue(Dict$2__DoesNotContainKey_2B595(b, "A"))("Pop removed key A");
})));

