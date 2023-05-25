__kernel void Abs(__global const TYPENAMEHERE* in, __global TYPENAMEHERE* results) {
    int index = get_global_id(0);

    results[index] = convert_TYPENAMEHERE(fabs(convert_double(in[index])));
}