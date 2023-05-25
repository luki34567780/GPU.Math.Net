//#define TYPENAMEHERE long long

__kernel void Add(__global const TYPENAMEHERE* a, __global const TYPENAMEHERE* b, __global TYPENAMEHERE* result) {
    int index = get_global_id(0);
    
    result[index] = a[index] + b[index];
}