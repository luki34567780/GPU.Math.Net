__kernel void Log10(__global const TYPENAMEHERE* in, __global TYPENAMEHERE* results) {
    int index = get_global_id(0);

    results[index] = convert_TYPENAMEHERE(log10(convert_double(in[index])));
}