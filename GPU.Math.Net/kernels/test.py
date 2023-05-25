import pyopencl

context = pyopencl.create_some_context()

with open("generic.cl", "r") as f:
    code = f.read()

code = code.replace("TYPENAMEHERE", "int")
program = pyopencl.Program(context, code)
program.build()

print(program.kernel_names)
kernel = program.PowConstantValue
kernels = program.all_kernels()

pass