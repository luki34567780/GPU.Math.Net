__kernel void Log2(__global const TYPENAMEHERE* in, __global TYPENAMEHERE* results) {
    int index = get_global_id(0);

    results[index] = convert_TYPENAMEHERE(log2(convert_double(in[index])));
}