import { structuralHash, defaultOf, safeHash, equals, toIterator, getEnumerator } from "../fable_modules/fable-library-js.4.21.0/Util.js";
import { removeInPlace } from "../fable_modules/fable-library-js.4.21.0/Array.js";
import { map, delay, contains } from "../fable_modules/fable-library-js.4.21.0/Seq.js";
import { containsValue, tryGetValue, addToDict } from "../fable_modules/fable-library-js.4.21.0/MapUtil.js";
import { FSharpRef } from "../fable_modules/fable-library-js.4.21.0/Types.js";
import { class_type } from "../fable_modules/fable-library-js.4.21.0/Reflection.js";
import { Dictionary } from "../fable_modules/fable-library-js.4.21.0/MutableMap.js";
import { System_Collections_Generic_KeyNotFoundException__KeyNotFoundException_Raise_Static_1DA990F7, System_ArgumentNullException__ArgumentNullException_Raise_Static_1DA990F7 } from "./DefaultDict.js";
import { printf } from "../fable_modules/fable-library-js.4.21.0/String.js";

export class Dict$2 {
    constructor(dic) {
        this.dic = dic;
    }
    GetEnumerator() {
        const _ = this;
        return getEnumerator(_.dic);
    }
    [Symbol.iterator]() {
        return toIterator(getEnumerator(this));
    }
    "System.Collections.IEnumerable.GetEnumerator"() {
        const __ = this;
        return getEnumerator(__.dic);
    }
    "System.Collections.ICollection.get_Count"() {
        const _ = this;
        return _.dic.size | 0;
    }
    "System.Collections.ICollection.CopyToZ2AA303D5"(arr, i) {
        const _ = this;
        _.dic["System.Collections.ICollection.CopyToZ2AA303D5"](arr, i);
    }
    "System.Collections.ICollection.get_IsSynchronized"() {
        const _ = this;
        return _.dic["System.Collections.ICollection.get_IsSynchronized"]();
    }
    "System.Collections.ICollection.get_SyncRoot"() {
        const _ = this;
        return _.dic["System.Collections.ICollection.get_SyncRoot"]();
    }
    "System.Collections.Generic.ICollection`1.Add2B595"(x) {
        const _ = this;
        void (_.dic.push(x));
    }
    "System.Collections.Generic.ICollection`1.Clear"() {
        const _ = this;
        _.dic.clear();
    }
    "System.Collections.Generic.ICollection`1.Remove2B595"(kvp) {
        const _ = this;
        return removeInPlace(kvp, _.dic, {
            Equals: equals,
            GetHashCode: safeHash,
        });
    }
    "System.Collections.Generic.ICollection`1.Contains2B595"(kvp) {
        const _ = this;
        return contains(kvp, _.dic, {
            Equals: equals,
            GetHashCode: safeHash,
        });
    }
    "System.Collections.Generic.ICollection`1.CopyToZ3B4C077E"(arr, i) {
        const _ = this;
        _.dic["System.Collections.Generic.ICollection`1.CopyToZ3B4C077E"](arr, i);
    }
    "System.Collections.Generic.ICollection`1.get_IsReadOnly"() {
        return false;
    }
    "System.Collections.Generic.ICollection`1.get_Count"() {
        const _ = this;
        return _.dic.size | 0;
    }
    "System.Collections.Generic.IDictionary`2.get_Item2B595"(k) {
        const _ = this;
        return Dict$2__get$0027_2B595(_, k);
    }
    "System.Collections.Generic.IDictionary`2.set_Item5BDDA1"(k, v) {
        const _ = this;
        Dict$2__set$0027(_, k, v);
    }
    "System.Collections.Generic.IDictionary`2.get_Keys"() {
        const _ = this;
        return _.dic.keys();
    }
    "System.Collections.Generic.IDictionary`2.get_Values"() {
        const _ = this;
        return _.dic.values();
    }
    "System.Collections.Generic.IDictionary`2.Add5BDDA1"(k, v) {
        const _ = this;
        addToDict(_.dic, k, v);
    }
    "System.Collections.Generic.IDictionary`2.ContainsKey2B595"(k) {
        const _ = this;
        return _.dic.has(k);
    }
    "System.Collections.Generic.IDictionary`2.TryGetValue6DC89625"(key, refValue) {
        const _ = this;
        let out = defaultOf();
        const found = tryGetValue(_.dic, key, new FSharpRef(() => out, (v) => {
            out = v;
        }));
        refValue.contents = out;
        return found;
    }
    "System.Collections.Generic.IDictionary`2.Remove2B595"(key) {
        const _ = this;
        return _.dic.delete(key);
    }
    "System.Collections.Generic.IReadOnlyCollection`1.get_Count"() {
        const _ = this;
        return _.dic.size | 0;
    }
    "System.Collections.Generic.IReadOnlyDictionary`2.get_Item2B595"(k) {
        const _ = this;
        return Dict$2__get$0027_2B595(_, k);
    }
    "System.Collections.Generic.IReadOnlyDictionary`2.get_Keys"() {
        const _ = this;
        return _.dic.keys();
    }
    "System.Collections.Generic.IReadOnlyDictionary`2.get_Values"() {
        const _ = this;
        return _.dic.values();
    }
    "System.Collections.Generic.IReadOnlyDictionary`2.ContainsKey2B595"(key) {
        const _ = this;
        return _.dic.has(key);
    }
    "System.Collections.Generic.IReadOnlyDictionary`2.TryGetValue6DC89625"(key, refValue) {
        const _ = this;
        let out = defaultOf();
        const found = tryGetValue(_.dic, key, new FSharpRef(() => out, (v) => {
            out = v;
        }));
        refValue.contents = out;
        return found;
    }
}

export function Dict$2_$reflection(gen0, gen1) {
    return class_type("Dic.Dict`2", [gen0, gen1], Dict$2);
}

function Dict$2_$ctor_138B901B(dic) {
    return new Dict$2(dic);
}

/**
 * Create a new empty Dict<'K,'V>.
 * A Dict is a thin wrapper over System.Collections.Generic.Dictionary<'K,'V>) with nicer Error messages on accessing missing keys.
 */
export function Dict$2_$ctor() {
    return Dict$2_$ctor_138B901B(new Dictionary([], {
        Equals: equals,
        GetHashCode: structuralHash,
    }));
}

/**
 * Create a new empty Dict<'K,'V> with an IEqualityComparer like HashIdentity.Structural.
 * A Dict is a thin wrapper over System.Collections.Generic.Dictionary<'K,'V>) with nicer Error messages on accessing missing keys.
 */
export function Dict$2_$ctor_Z79760D57(iEqualityComparer) {
    return Dict$2_$ctor_138B901B(new Dictionary([], iEqualityComparer));
}

/**
 * Constructs a new Dict by using the supplied Dictionary<'K,'V> directly, without any copying of items
 */
export function Dict$2_CreateDirectly_138B901B(dic) {
    if (dic == null) {
        System_ArgumentNullException__ArgumentNullException_Raise_Static_1DA990F7(printf("Dictionary in Dict.CreateDirectly is null"));
    }
    return Dict$2_$ctor_138B901B(dic);
}

/**
 * Access the underlying Collections.Generic.Dictionary<'K,'V>)
 * ATTENTION! This is not even a shallow copy, mutating it will also change this instance of Dict!
 * use #nowarn "44" to disable the obsolete warning
 */
export function Dict$2__get_Dictionary(_) {
    return _.dic;
}

/**
 * For Index operator .[i]: get or set the value for given key
 */
export function Dict$2__get_Item_2B595(_, k) {
    return Dict$2__get$0027_2B595(_, k);
}

/**
 * For Index operator .[i]: get or set the value for given key
 */
export function Dict$2__set_Item_5BDDA1(_, k, v) {
    Dict$2__set$0027(_, k, v);
}

/**
 * Get value for given key
 */
export function Dict$2__Get_2B595(_, key) {
    return Dict$2__get$0027_2B595(_, key);
}

/**
 * Set value for given key, same as <c>Dict.add key value</c>
 */
export function Dict$2__set(_, key, value) {
    Dict$2__set$0027(_, key, value);
}

/**
 * Set value for given key, same as <c>Dict.set key value</c>
 */
export function Dict$2__add(_, key, value) {
    Dict$2__set$0027(_, key, value);
}

/**
 * Set value only if key does not exist yet.
 * Returns false if key already exist, does not set value in this case
 * Same as <c>Dict.addOnce key value</c>
 */
export function Dict$2__setIfKeyAbsent(_, key, value) {
    if (equals(key, defaultOf())) {
        return System_ArgumentNullException__ArgumentNullException_Raise_Static_1DA990F7(printf("Dict.setOnce key is null "));
    }
    else if (_.dic.has(key)) {
        return false;
    }
    else {
        _.dic.set(key, value);
        return true;
    }
}

/**
 * Set value only if key does not exist yet.
 * Returns false if key already exist, does not set value in this case
 * Same as <c>Dict.setOnce key value</c>
 */
export function Dict$2__addIfKeyAbsent(_, key, value) {
    if (equals(key, defaultOf())) {
        return System_ArgumentNullException__ArgumentNullException_Raise_Static_1DA990F7(printf("Dict.addOnce key is null "));
    }
    else if (_.dic.has(key)) {
        return false;
    }
    else {
        _.dic.set(key, value);
        return true;
    }
}

/**
 * If the key ist not present calls the default function, set it as value at the key and return the value.
 * This function is an alternative to the DefaultDic type. Use it if you need to provide a custom implementation of the default function depending on the key.
 */
export function Dict$2__getOrSetDefault(_, getDefault, key) {
    if (equals(key, defaultOf())) {
        return System_ArgumentNullException__ArgumentNullException_Raise_Static_1DA990F7(printf("Dict.getOrSetDefault key is null "));
    }
    else {
        let matchValue_1;
        let outArg = defaultOf();
        matchValue_1 = [tryGetValue(_.dic, key, new FSharpRef(() => outArg, (v) => {
            outArg = v;
        })), outArg];
        if (matchValue_1[0]) {
            return matchValue_1[1];
        }
        else {
            const v_2 = getDefault(key);
            _.dic.set(key, v_2);
            return v_2;
        }
    }
}

/**
 * If the key ist not present set it as value at the key and return the value.
 */
export function Dict$2__getOrSetDefaultValue(_, defaultValue, key) {
    if (equals(key, defaultOf())) {
        return System_ArgumentNullException__ArgumentNullException_Raise_Static_1DA990F7(printf("Dict.getOrSetDefaultValue key is null "));
    }
    else {
        let matchValue_1;
        let outArg = defaultOf();
        matchValue_1 = [tryGetValue(_.dic, key, new FSharpRef(() => outArg, (v) => {
            outArg = v;
        })), outArg];
        if (matchValue_1[0]) {
            return matchValue_1[1];
        }
        else {
            const v_2 = defaultValue;
            _.dic.set(key, v_2);
            return v_2;
        }
    }
}

/**
 * Get a value and remove key and value it from Dictionary, like *.pop() in Python.
 * Will fail if key does not exist
 */
export function Dict$2__Pop_2B595(_, key) {
    if (equals(key, defaultOf())) {
        return System_ArgumentNullException__ArgumentNullException_Raise_Static_1DA990F7(printf("Dict.Pop(key) key is null"));
    }
    else {
        let patternInput;
        let outArg = defaultOf();
        patternInput = [tryGetValue(_.dic, key, new FSharpRef(() => outArg, (v) => {
            outArg = v;
        })), outArg];
        if (patternInput[0]) {
            _.dic.delete(key);
            return patternInput[1];
        }
        else {
            return System_Collections_Generic_KeyNotFoundException__KeyNotFoundException_Raise_Static_1DA990F7(printf("Dict.Pop(key): Failed to pop key %A in %A of %d items"))(key)(_.dic)(_.dic.size);
        }
    }
}

/**
 * Returns a (lazy) sequence of key and value tuples
 */
export function Dict$2__get_Items(_) {
    return delay(() => map((kvp) => [kvp[0], kvp[1]], _.dic));
}

/**
 * Returns a (lazy) sequence of values
 */
export function Dict$2__get_ValuesSeq(_) {
    return delay(() => map((kvp) => kvp[1], _.dic));
}

/**
 * Returns a (lazy) sequence of Keys
 */
export function Dict$2__get_KeysSeq(_) {
    return delay(() => map((kvp) => kvp[0], _.dic));
}

/**
 * Determines whether the Dictionary does not contains the specified key.
 * not(dic.ContainsKey(key))
 */
export function Dict$2__DoesNotContainKey_2B595(_, key) {
    return !_.dic.has(key);
}

/**
 * Determines whether the Dictionary does not contains the specified key.
 * not(dic.ContainsKey(key))
 */
export function Dict$2__DoesNotContain_2B595(_, key) {
    return !_.dic.has(key);
}

/**
 * Gets the number of key/value pairs contained in the Dictionary
 */
export function Dict$2__get_Count(_) {
    return _.dic.size;
}

/**
 * Gets a collection containing the keys in the Dictionary
 * same as on System.Collections.Generic.Dictionary<'K,'V>
 */
export function Dict$2__get_Keys(_) {
    return _.dic.keys();
}

/**
 * Gets a collection containing the values in the Dictionary
 * same as on System.Collections.Generic.Dictionary<'K,'V>
 */
export function Dict$2__get_Values(_) {
    return _.dic.values();
}

/**
 * Add the specified key and value to the Dictionary.
 */
export function Dict$2__Add_5BDDA1(_, key, value) {
    Dict$2__set$0027(_, key, value);
}

/**
 * Removes all keys and values from the Dictionary
 */
export function Dict$2__Clear(_) {
    _.dic.clear();
}

/**
 * Determines whether the Dictionary contains the specified key.
 */
export function Dict$2__ContainsKey_2B595(_, key) {
    return _.dic.has(key);
}

/**
 * Determines whether the Dictionary contains a specific value.
 */
export function Dict$2__ContainsValue_2B594(_, value) {
    return containsValue(value, _.dic);
}

/**
 * Removes the value with the specified key from the Dictionary.
 * See also .Pop(key) method that gets the contained value too.
 */
export function Dict$2__Remove_2B595(_, key) {
    return _.dic.delete(key);
}

/**
 * Lookup an element in the Dict, assigning it to <c>refValue</c> if the element is in the Dict and return true. Otherwise returning <c>false</c> .
 */
export function Dict$2__TryGetValue_6DC89625(_, key, refValue) {
    let out = defaultOf();
    const found = tryGetValue(_.dic, key, new FSharpRef(() => out, (v) => {
        out = v;
    }));
    refValue.contents = out;
    return found;
}

/**
 * Returns an enumerator that iterates through the Dictionary.
 */
export function Dict$2__GetEnumerator(_) {
    return getEnumerator(_.dic);
}

export function Dict$2__get$0027_2B595(this$, key) {
    if (equals(key, defaultOf())) {
        return System_ArgumentNullException__ArgumentNullException_Raise_Static_1DA990F7(printf("Dict.get: key is null "));
    }
    else {
        let matchValue_1;
        let outArg = defaultOf();
        matchValue_1 = [tryGetValue(this$.dic, key, new FSharpRef(() => outArg, (v) => {
            outArg = v;
        })), outArg];
        if (matchValue_1[0]) {
            return matchValue_1[1];
        }
        else {
            return System_Collections_Generic_KeyNotFoundException__KeyNotFoundException_Raise_Static_1DA990F7(printf("Dict.get failed to find key %A in %A of %d items"))(key)(this$.dic)(this$.dic.size);
        }
    }
}

export function Dict$2__set$0027(this$, key, value) {
    if (equals(key, defaultOf())) {
        System_ArgumentNullException__ArgumentNullException_Raise_Static_1DA990F7(printf("Dict.set key is null for value %A"))(value);
    }
    else {
        this$.dic.set(key, value);
    }
}

