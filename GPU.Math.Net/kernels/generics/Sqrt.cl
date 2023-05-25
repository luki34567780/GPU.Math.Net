__kernel void Sqrt(__global const TYPENAMEHERE* a, __global TYPENAMEHERE* result) {
    int index = get_global_id(0);

    result[index] = convert_TYPENAMEHERE(sqrt(convert_double(a[index])));
}
