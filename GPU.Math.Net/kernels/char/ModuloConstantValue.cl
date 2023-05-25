__kernel void ModuloConstantValue(__global const char* in, const char value, __global char* result) {
    int index = get_global_id(0);

    result[index] = in[index] % value;
}