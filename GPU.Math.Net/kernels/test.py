import pyopencl

context = pyopencl.create_some_context()

with open("generic.cl", "r") as f:
    code = f.read()

code = code.replace("TYPENAMEHERE", "float")
program = pyopencl.Program(context, code)
program.build()
