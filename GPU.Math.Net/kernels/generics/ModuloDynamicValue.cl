__kernel void ModuloDynamicValue(__global const TYPENAMEHERE* in, __global const TYPENAMEHERE* in2, __global TYPENAMEHERE* result) {
    int index = get_global_id(0);

    result[index] = in[index] % in2[index];
}