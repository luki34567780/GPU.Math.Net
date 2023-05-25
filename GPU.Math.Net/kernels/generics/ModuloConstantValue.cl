__kernel void ModuloConstantValue(__global const TYPENAMEHERE* in, const TYPENAMEHERE value, __global TYPENAMEHERE* result) {
    int index = get_global_id(0);

    result[index] = in[index] % value;
}