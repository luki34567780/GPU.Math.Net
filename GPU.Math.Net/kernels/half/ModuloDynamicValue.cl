__kernel void ModuloDynamicValue(__global const half* in, __global const half* in2, __global half* result) {
    int index = get_global_id(0);

    result[index] = fmod(in[index], in2[index]);
}