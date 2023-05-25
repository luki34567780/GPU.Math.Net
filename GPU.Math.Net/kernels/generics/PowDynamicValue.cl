__kernel void PowDynamicValue(__global const TYPENAMEHERE* in, __global const TYPENAMEHERE* exponents, __global TYPENAMEHERE* results) {
    int index = get_global_id(0);

    results[index] = convert_TYPENAMEHERE(pow(convert_double(in[index]), convert_double(exponents[index])));
}