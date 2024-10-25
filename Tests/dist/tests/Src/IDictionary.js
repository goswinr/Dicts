import { printf } from "../fable_modules/fable-library-js.4.22.0/String.js";
import { defaultOf, count } from "../fable_modules/fable-library-js.4.22.0/Util.js";
import { tryGetValue } from "../fable_modules/fable-library-js.4.22.0/MapUtil.js";
import { FSharpRef } from "../fable_modules/fable-library-js.4.22.0/Types.js";
import { singleton, collect, delay } from "../fable_modules/fable-library-js.4.22.0/Seq.js";

/**
 * Raise ArgumentNullException with F# printf string formatting
 */
export function System_ArgumentNullException__ArgumentNullException_Raise_Static_1DA990F7(msg) {
    return msg.cont((s) => {
        throw new Error(s);
    });
}

/**
 * Raise KeyNotFoundException with F# printf string formatting
 */
export function System_Collections_Generic_KeyNotFoundException__KeyNotFoundException_Raise_Static_1DA990F7(msg) {
    return msg.cont((s) => {
        throw new Error(s);
    });
}

/**
 * Set/add value at key, with nicer error messages.
 * Same as <c>Dic.addValue key value</c>
 */
export function System_Collections_Generic_IDictionary$2__IDictionary$2_Set(d, k, v) {
    try {
        d.set(k, v);
    }
    catch (matchValue) {
        System_Collections_Generic_KeyNotFoundException__KeyNotFoundException_Raise_Static_1DA990F7(printf("Dic: IDictionary.Set failed to find key \'%A\' in %A of %d items (for value: \'%A\')"))(k)(d)(count(d))(v);
    }
}

/**
 * Set/add value at key, with nicer error messages.
 * Same as <c>Dic.setValue key value</c>
 */
export function System_Collections_Generic_IDictionary$2__IDictionary$2_Add$0027(d, k, v) {
    try {
        d.set(k, v);
    }
    catch (matchValue) {
        System_Collections_Generic_KeyNotFoundException__KeyNotFoundException_Raise_Static_1DA990F7(printf("Dic: IDictionary.SetValue failed to find key \'%A\' in %A of %d items (for value: \'%A\')"))(k)(d)(count(d))(v);
    }
}

/**
 * Get value at key, with nicer error messages.
 */
export function System_Collections_Generic_IDictionary$2__IDictionary$2_Get_1505(d, k) {
    let patternInput;
    let outArg = defaultOf();
    patternInput = [tryGetValue(d, k, new FSharpRef(() => outArg, (v) => {
        outArg = v;
    })), outArg];
    if (patternInput[0]) {
        return patternInput[1];
    }
    else {
        return System_Collections_Generic_KeyNotFoundException__KeyNotFoundException_Raise_Static_1DA990F7(printf("Dic: IDictionary.GetValue failed to find key %A in %A of %d items"))(k)(d)(count(d));
    }
}

/**
 * Get a value and remove it from Dictionary, like *.pop() in Python.
 */
export function System_Collections_Generic_IDictionary$2__IDictionary$2_Pop_1505(d, k) {
    let patternInput;
    let outArg = defaultOf();
    patternInput = [tryGetValue(d, k, new FSharpRef(() => outArg, (v) => {
        outArg = v;
    })), outArg];
    if (patternInput[0]) {
        d.delete(k);
        return patternInput[1];
    }
    else {
        return System_Collections_Generic_KeyNotFoundException__KeyNotFoundException_Raise_Static_1DA990F7(printf("Dic: IDictionary.Pop(key): Failed to pop key %A in %A of %d items"))(k)(d)(count(d));
    }
}

/**
 * Returns a lazy seq of key and value tuples
 */
export function System_Collections_Generic_IDictionary$2__IDictionary$2_get_Items(d) {
    return delay(() => collect((matchValue) => {
        const activePatternResult = matchValue;
        return singleton([activePatternResult[0], activePatternResult[1]]);
    }, d));
}

