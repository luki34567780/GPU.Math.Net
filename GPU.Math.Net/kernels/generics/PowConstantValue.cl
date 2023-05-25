__kernel void PowConstantValue(__global const TYPENAMEHERE* in, const TYPENAMEHERE exponent, __global TYPENAMEHERE* result) {
    int index = get_global_id(0);

    result[index] = convert_TYPENAMEHERE(pow(convert_double(in[index]), convert_double(exponent)));
}