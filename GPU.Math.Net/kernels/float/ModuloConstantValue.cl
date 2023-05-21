__kernel void ModuloConstantValue(__global const float* in, const float value, __global float* result) {
    int index = get_global_id(0);

    result[index] = fmod(in[index], value);
}