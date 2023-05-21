__kernel void ModuloDynamicValue(__global const double* in, __global const double* in2, __global double* result) {
    int index = get_global_id(0);

    result[index] = fmod(in[index], in2[index]);
}