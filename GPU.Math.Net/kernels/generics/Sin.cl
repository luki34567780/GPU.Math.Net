__kernel void Sin(__global const TYPENAMEHERE* in, __global TYPENAMEHERE* results) {
    int index = get_group_id(0);
    
    float floatValue = convert_float(in[index]);
    float sinValue = sin(floatValue);
    results[index] = convert_TYPENAMEHERE(sinValue);
}