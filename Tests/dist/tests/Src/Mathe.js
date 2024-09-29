import { interpolate, toText } from "../fable_modules/fable-library-js.4.21.0/String.js";
import { class_type } from "../fable_modules/fable-library-js.4.21.0/Reflection.js";
import { nonSeeded } from "../fable_modules/fable-library-js.4.21.0/Random.js";
import { count, disposeSafe, getEnumerator } from "../fable_modules/fable-library-js.4.21.0/Util.js";
import { empty, singleton, append, enumerateWhile, delay, zip, findIndex, windowed } from "../fable_modules/fable-library-js.4.21.0/Seq.js";
import { item } from "../fable_modules/fable-library-js.4.21.0/Array.js";
import { tryParse, isInfinity } from "../fable_modules/fable-library-js.4.21.0/Double.js";
import { doubleToInt64Bits, int64BitsToDouble } from "../fable_modules/fable-library-js.4.21.0/BitConverter.js";
import { op_Addition, toInt64 } from "../fable_modules/fable-library-js.4.21.0/BigInt.js";
import { StringBuilder__get_Length, StringBuilder__Append_244C7CD6, StringBuilder_$ctor_Z524259A4 } from "../fable_modules/fable-library-js.4.21.0/System.Text.js";
import { FSharpRef, toString } from "../fable_modules/fable-library-js.4.21.0/Types.js";

export function DictFail_fail11(x, func) {
    throw new Error(toText(interpolate("Dict.%P(): input must be between -1.00001 and +1.00001 but is %8f%P()", [func, x])));
}

export function DictFail_failClamp(maxVal, minVal) {
    throw new Error(`Dict.clamp: max value ${maxVal} must be bigger than min ${minVal}`);
}

export function DictFail_failInfNaN(func) {
    throw new Error(`Dict.${func}: given input is NaN or Infinity`);
}

export function DictFail_failInfNaNv(func, name) {
    throw new Error(`Dict.${func}: given input '${name}' is NaN`);
}

export function DictFail_failNaN(func) {
    throw new Error(`Dict.${func}: given input is NaN`);
}

export function DictFail_failNaNv(func, name) {
    throw new Error(`Dict.${func}: given input '${name}' is NaN`);
}

export function DictFail_failTiny18(func) {
    throw new Error(`Dict.${func}: given input is smaller than + or - 1-e18.`);
}

export function DictFail_failTiny18v(func, name) {
    throw new Error(`Dict.${func}: given input '${name}' is smaller than + or - 1-e18.`);
}

export function DictFail_failBig36(func) {
    throw new Error(`Dict.${func}: given input is bigger than + or - 1e36.`);
}

export function DictFail_failBig36v(func, name) {
    throw new Error(`Dict.${func}: given input '${name}' is bigger than + or - 1e36.`);
}

export function DictFail_failNotPos(func, name, v) {
    throw new Error(`Dict.${func}: given input '${name}' must be positive but is ${v}.`);
}

export class Dict {
    constructor() {
    }
}

export function Dict_$reflection() {
    return class_type("Dict.Dict", undefined, Dict);
}

function Dict_$ctor() {
    return new Dict();
}

(() => {
    Dict.rand = nonSeeded();
})();

/**
 * Returns a function to find linear interpolations in one table.
 * The input table is a sorted (increasing) array of tuples of input and respective output values.
 */
export function Dict_interpolateTable_16FC164C(table) {
    const enumerator = getEnumerator(windowed(2, table));
    try {
        while (enumerator["System.Collections.IEnumerator.MoveNext"]()) {
            const tn = enumerator["System.Collections.Generic.IEnumerator`1.get_Current"]();
            const t = item(0, tn);
            const n = item(1, tn);
            if (t[0] >= n[0]) {
                throw new Error(`Dict.interpolateTable: Table input is not sorted increasing ${t} >= ${n}:
${table}`);
            }
        }
    }
    finally {
        disposeSafe(enumerator);
    }
    return (x) => {
        if (x < table[0][0]) {
            const stepIn = table[1][0] - table[0][0];
            const stepOut = table[1][1] - table[0][1];
            const distIn = table[0][0] - x;
            if (Math.abs(stepIn) < 1E-16) {
                throw new Error(`Dict.interpolateTable: Table query ${x} is smaller than first element in table ${table[0]} and the first two Input elements are almost the same, so a meaningful prediction is not possible.`);
            }
            else {
                const sc = distIn / stepIn;
                return table[0][1] - (stepOut * sc);
            }
        }
        else if (x > table[count(table) - 1][0]) {
            const li = (count(table) - 1) | 0;
            const stepIn_1 = table[li][0] - table[li - 1][0];
            const stepOut_1 = table[li][1] - table[li - 1][1];
            const distIn_1 = x - table[li][0];
            if (Math.abs(stepIn_1) < 1E-16) {
                throw new Error(`Dict.interpolateTable: Table query ${x} is bigger than last element in table ${table[li]} and the last two Input elements are almost the same, so a meaningful prediction is not possible.`);
            }
            else {
                const sc_1 = distIn_1 / stepIn_1;
                return table[li][1] + (stepOut_1 * sc_1);
            }
        }
        else {
            const hiIdx = findIndex((v) => (v[0] >= x), table) | 0;
            if (hiIdx === 0) {
                return table[0][1];
            }
            else {
                const patternInput = table[hiIdx - 1];
                const lowOut = patternInput[1];
                const low = patternInput[0];
                const patternInput_1 = table[hiIdx];
                const hig = patternInput_1[0];
                const delta = hig - low;
                return (Math.abs(delta) < 1E-16) ? ((low + hig) * 0.5) : (lowOut + ((patternInput_1[1] - lowOut) * ((x - low) / delta)));
            }
        }
    };
}

/**
 * Returns a function to find linear interpolations from two sorted(increasing) Lists of input and respective output values.
 * Includes a check that both list have the same length
 */
export function Dict_interpolateLists(input, output) {
    if (count(input) !== count(output)) {
        throw new Error(`Dict.interpolateLists: Tables length don't match:
${input}
and:
${output}`);
    }
    return Dict_interpolateTable_16FC164C(Array.from(zip(input, output)));
}

/**
 * Rounds to the next bigger (away from zero) number on logarithmic scale.
 * Define scale by giving amount of steps(int) to double or half a value.
 * Fails if input is zero or less than 1e-24.
 */
export function Dict_roundUpToNextLogSteps(stepsToDouble, x) {
    let arg1__3;
    if (Number.isNaN(x)) {
        DictFail_failNaN("roundUpToNextLogSteps");
    }
    if (stepsToDouble < 0) {
        DictFail_failNotPos("roundUpToNextLogSteps", "stepsToDouble", stepsToDouble);
    }
    if (Math.abs(x) < 1E-24) {
        DictFail_failTiny18("roundUpToNextLogSteps");
    }
    let logBase;
    const arg1__1 = 1 / stepsToDouble;
    logBase = Math.pow(2, arg1__1);
    if (x > 0) {
        const arg1__2 = Math.ceil(Math.log(x) / Math.log(logBase));
        return Math.pow(logBase, arg1__2);
    }
    else {
        return -((arg1__3 = Math.ceil(Math.log(Math.abs(x)) / Math.log(logBase)), Math.pow(logBase, arg1__3)));
    }
}

/**
 * Rounds to the next smaller (closer to zero) number on logarithmic scale
 * Define scale by giving amount of steps(int) to double or half a value.
 * Fails if input is zero or less than 1e-24.
 */
export function Dict_roundDownToNextLogSteps(stepsToDouble, x) {
    let arg1__3;
    if (Number.isNaN(x)) {
        DictFail_failNaN("roundDownToNextLogSteps");
    }
    if (stepsToDouble < 0) {
        DictFail_failNotPos("roundDownToNextLogSteps", "stepsToDouble", stepsToDouble);
    }
    if (Math.abs(x) < 1E-24) {
        DictFail_failTiny18("roundDownToNextLogSteps");
    }
    let logBase;
    const arg1__1 = 1 / stepsToDouble;
    logBase = Math.pow(2, arg1__1);
    if (x > 0) {
        const arg1__2 = Math.floor(Math.log(x) / Math.log(logBase));
        return Math.pow(logBase, arg1__2);
    }
    else {
        return -((arg1__3 = Math.floor(Math.log(Math.abs(x)) / Math.log(logBase)), Math.pow(logBase, arg1__3)));
    }
}

/**
 * This float range function ensures that the end is always included.
 * The F# build in range fails for example on [0.0 .. 0.1 .. 0.2 ] , it equals [0.0 .. 0.1 .. 0.3 ]
 * It increases the stop value by the smallest step possible 15 times, to ensure end value is included in returned seq.
 */
export function Dict_floatRange_Z1EAE0AEB(start, step, stop) {
    let f, f_1, f_2;
    if ((f = start, isInfinity(f) ? true : Number.isNaN(f))) {
        DictFail_failInfNaNv("floatRange", "start");
    }
    if ((f_1 = stop, isInfinity(f_1) ? true : Number.isNaN(f_1))) {
        DictFail_failInfNaNv("floatRange", "stop");
    }
    if ((f_2 = step, isInfinity(f_2) ? true : Number.isNaN(f_2))) {
        DictFail_failInfNaNv("floatRange", "step");
    }
    if (Math.abs(step) < 1E-36) {
        throw new Error(`Dict.floatRange: step-size cannot be zero: start: ${start} step: ${step} stop: ${stop}`);
    }
    const steps = int64BitsToDouble(toInt64(op_Addition(15n, doubleToInt64Bits(stop - start)))) / step;
    if (steps < 1E-36) {
        throw new Error(`Dict.floatRange: stop value can never be reached: start: ${start} step: ${step} stop: ${stop}`);
    }
    return delay(() => {
        let i = 0;
        return enumerateWhile(() => (i <= steps), delay(() => append(singleton(start + (i * step)), delay(() => {
            i = (i + 1);
            return empty();
        }))));
    });
}

/**
 * Given mean and standardDeviation returns a random value from this Gaussian distribution.
 * If mean is 0.0 and the standard deviation is 1.0 then 99% of values are within -2.3 to +2.3 ; 70% within -1 to +1
 */
export function Dict_randomStandardDeviation_B9AD360(mean, standardDeviation) {
    let f, f_1;
    if ((f = mean, isInfinity(f) ? true : Number.isNaN(f))) {
        DictFail_failInfNaNv("randomStandardDeviation", "mean");
    }
    if ((f_1 = standardDeviation, isInfinity(f_1) ? true : Number.isNaN(f_1))) {
        DictFail_failInfNaNv("randomStandardDeviation", "standardDeviation");
    }
    const u1 = 1 - Dict.rand.NextDouble();
    const u2 = Dict.rand.NextDouble();
    return mean + (standardDeviation * (Math.sqrt(-2 * Math.log(u1)) * Math.sin((2 * 3.141592653589793) * u2)));
}

/**
 * A very tolerant custom float parser for English locale.
 * Ignores all non numeric characters ( expect a minus '-' before any digit )
 * and considers a period '.' as decimal separator.
 * Does NOT allow for scientific notation !
 */
export function Dict_tryParseFloatTolerant_Z721C83C5(s) {
    const sb = StringBuilder_$ctor_Z524259A4(s.length);
    for (let i = 0; i <= (s.length - 1); i++) {
        const c = s[i];
        if ((c >= "0") && (c <= "9")) {
            StringBuilder__Append_244C7CD6(sb, c);
        }
        else if (c === ".") {
            StringBuilder__Append_244C7CD6(sb, c);
        }
        else if ((c === "-") && (StringBuilder__get_Length(sb) === 0)) {
            StringBuilder__Append_244C7CD6(sb, c);
        }
    }
    let matchValue;
    let outArg = 0;
    matchValue = [tryParse(toString(sb), new FSharpRef(() => outArg, (v) => {
        outArg = v;
    })), outArg];
    if (matchValue[0]) {
        return matchValue[1];
    }
    else {
        return undefined;
    }
}

/**
 * A very tolerant custom float parser for NON English locales.
 * Ignores all non numeric characters ( expect a minus '-' before any digit )
 * and considers a comma ',' as decimal separator.
 * Does NOT allow for scientific notation !
 */
export function Dict_tryParseFloatDeTolerant_Z721C83C5(s) {
    const sb = StringBuilder_$ctor_Z524259A4(s.length);
    for (let i = 0; i <= (s.length - 1); i++) {
        const c = s[i];
        if ((c >= "0") && (c <= "9")) {
            StringBuilder__Append_244C7CD6(sb, c);
        }
        else if (c === ",") {
            StringBuilder__Append_244C7CD6(sb, ".");
        }
        else if ((c === "-") && (StringBuilder__get_Length(sb) === 0)) {
            StringBuilder__Append_244C7CD6(sb, c);
        }
    }
    let matchValue;
    let outArg = 0;
    matchValue = [tryParse(toString(sb), new FSharpRef(() => outArg, (v) => {
        outArg = v;
    })), outArg];
    if (matchValue[0]) {
        return matchValue[1];
    }
    else {
        return undefined;
    }
}

