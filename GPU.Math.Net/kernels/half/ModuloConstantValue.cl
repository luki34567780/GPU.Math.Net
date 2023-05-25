__kernel void ModuloConstantValue(__global const half* in, const half value, __global half* result) {
    int index = get_global_id(0);

    result[index] = fmod(in[index], value);
}