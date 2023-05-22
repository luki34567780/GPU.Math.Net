__kernel void Add(__global const TYPENAMEHERE* a, __global const TYPENAMEHERE* b, __global TYPENAMEHERE* result) {
    int index = get_global_id(0);
    
    result[index] = a[index] + b[index];
}

__kernel void Subtract(__global const TYPENAMEHERE* a, __global const TYPENAMEHERE* b, __global TYPENAMEHERE* result) {
    int index = get_global_id(0);

    result[index] = a[index] - b[index];
}

__kernel void Multiply(__global const TYPENAMEHERE* a, __global const TYPENAMEHERE* b, __global TYPENAMEHERE* result) {
    int index = get_global_id(0);

    result[index] = a[index] * b[index];
}

__kernel void Divide(__global const TYPENAMEHERE* a, __global const TYPENAMEHERE* b, __global TYPENAMEHERE* result) {
    int index = get_global_id(0);

    result[index] = a[index] / b[index];
}

__kernel void Sqrt(__global const TYPENAMEHERE* a, __global TYPENAMEHERE* result) {
    int index = get_global_id(0);

    result[index] = convert_TYPENAMEHERE(sqrt(convert_double(a[index])));
}

__kernel void RSqrt(__global const TYPENAMEHERE* a, __global TYPENAMEHERE* result) {
    int index = get_global_id(0);

    result[index] = convert_TYPENAMEHERE(rsqrt(convert_double(a[index])));
}

#if !(TYPENAMEHERE == float || TYPENAMEHERE == double || TYPENAMEHERE == half)

__kernel void ModuloConstantValue(__global const TYPENAMEHERE* in, const TYPENAMEHERE value, __global TYPENAMEHERE* result) {
    int index = get_global_id(0);

    result[index] = in[index] % value;
}

__kernel void ModuloDynamicValue(__global const TYPENAMEHERE* in, __global const TYPENAMEHERE* in2, __global TYPENAMEHERE* result) {
    int index = get_global_id(0);

    result[index] = in[index] % in2[index];
}
#endif