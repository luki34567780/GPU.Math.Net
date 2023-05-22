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

__kernel void PowConstantValue(__global const TYPENAMEHERE* in, const TYPENAMEHERE exponent, __global TYPENAMEHERE* result) {
    int index = get_global_id(0);

    result[index] = convert_TYPENAMEHERE(pow(convert_double(in[index]), convert_double(exponent)));
}

__kernel void PowDynamicValue(__global const TYPENAMEHERE* in, __global const TYPENAMEHERE* exponents, __global TYPENAMEHERE* results) {
    int index = get_global_id(0);

    results[index] = convert_TYPENAMEHERE(pow(convert_double(in[index]), convert_double(exponents[index])));
}

__kernel void Sin(__global const TYPENAMEHERE* in, __global TYPENAMEHERE* results) {
    int index = get_group_id(0);

    results[index] = convert_TYPENAMEHERE(sin(convert_double(in[index])));
}

__kernel void Cos(__global const TYPENAMEHERE* in, __global TYPENAMEHERE* results) {
    int index = get_global_id(0);

    results[index] = convert_TYPENAMEHERE(cos(convert_double(in[index])));
}

__kernel void Tan(__global const TYPENAMEHERE* in, __global TYPENAMEHERE* results) {
    int index = get_global_id(0);

    results[index] = convert_TYPENAMEHERE(tan(convert_double(in[index])));
}

__kernel void Log(__global const TYPENAMEHERE* in, __global TYPENAMEHERE* results) {
    int index = get_global_id(0);

    results[index] = convert_TYPENAMEHERE(log(convert_double(in[index])));
}

__kernel void Log2(__global const TYPENAMEHERE* in, __global TYPENAMEHERE* results) {
    int index = get_global_id(0);

    results[index] = convert_TYPENAMEHERE(log2(convert_double(in[index])));
}

__kernel void Log10(__global const TYPENAMEHERE* in, __global TYPENAMEHERE* results) {
    int index = get_global_id(0);

    results[index] = convert_TYPENAMEHERE(log10(convert_double(in[index])));
}

__kernel void Abs(__global const TYPENAMEHERE* in, __global TYPENAMEHERE* results) {
    int index = get_global_id(0);

    results[index] = convert_TYPENAMEHERE(fabs(convert_double(in[index])));
}