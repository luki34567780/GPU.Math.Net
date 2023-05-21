__kernel void ModuloConstantValue(__global const double* in, const double value, __global double* result) {
    int index = get_global_id(0);

    result[index] = fmod(in[index], value);
}