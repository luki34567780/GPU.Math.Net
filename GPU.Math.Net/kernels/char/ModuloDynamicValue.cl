__kernel void ModuloDynamicValue(__global const char* in, __global const char* in2, __global char* result) {
    int index = get_global_id(0);

    result[index] = in[index] % in2[index];
}