__kernel void ModuloConstantValue(__global const int* in, const int value, __global int* result) {
    int index = get_global_id(0);

    result[index] = in[index] % value;
}