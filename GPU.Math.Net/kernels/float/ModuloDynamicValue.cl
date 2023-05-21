__kernel void ModuloDynamicValue(__global const float* in, __global const float* in2, __global float* result) {
    int index = get_global_id(0);

    result[index] = fmod(in[index], in2[index]);
}