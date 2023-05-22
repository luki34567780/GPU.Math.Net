using OpenCL.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPU.Math.Net
{
    public class OperationKernelManager
    {
        private static object _cacheLock = new();
        public static int Counter = 0;
        private static Dictionary<GPU, Dictionary<Type, Dictionary<string, Kernel>>> _cache = new();
        private static Dictionary<Type, string> CNameLookupTable = new()
        {
            { typeof(ushort), "ushort" },
            { typeof(float), "float" },
            { typeof(double), "double" },
            { typeof(Half), "half" }
        };

        public static Kernel GetKernel(GPU gpu, Type type, string op)
        {
            lock (_cacheLock)
            {
                if (_cache.ContainsKey(gpu))
                {
                    if (_cache[gpu].ContainsKey(type))
                    {
                        if (_cache[gpu][type].ContainsKey(op))
                        {
                            return _cache[gpu][type][op];
                        }
                    }
                }
                Counter++;
                string code;
                string? raw = null;

                // if a specific kernel for this combination exists compile it
                var specialPath = $"kernels/{type.Name}/{op}.cl";
                var genericOperationPath = $"kernels/{op}.cl";

                if (File.Exists(specialPath))
                {
                    code = File.ReadAllText(specialPath);
                }
                else if (File.Exists(genericOperationPath))
                {
                    raw = File.ReadAllText(genericOperationPath);
                }
                else
                {
                    raw = File.ReadAllText("kernels/generic.cl");
                }

                if (raw != null)
                {
                    string cName;

                    if (CNameLookupTable.ContainsKey(type))
                        cName = CNameLookupTable[type];
                    else
                        cName = type.Name;

                    code = raw.Replace("TYPENAMEHERE", cName);
                }

                var program = Cl.CreateProgramWithSource(gpu.Context, 1, new[] { code }, null, out var error);

                if (error != ErrorCode.Success)
                    throw new Cl.Exception(error);

                Cl.BuildProgram(program, 1, new[] { gpu.Device }, null, null, IntPtr.Zero);
                var kernel = Cl.CreateKernel(program, op, out error);

                if (error != ErrorCode.Success)
                {
                    var test = code.Any(x => x == '#');
                    throw new Cl.Exception(error);
                }

                if (!_cache.ContainsKey(gpu))
                {
                    var kernelDict = new Dictionary<string, Kernel>() { { op, kernel } };
                    var typeDict = new Dictionary<Type, Dictionary<string, Kernel>>() { { type, kernelDict } };
                    _cache.Add(gpu, typeDict);
                }
                else if (!_cache[gpu].ContainsKey(type))
                {
                    var kernelDict = new Dictionary<string, Kernel>() { { op, kernel } };
                    _cache[gpu].Add(type, kernelDict);
                }
                else
                {
                    _cache[gpu][type].Add(op, kernel);
                }

                return kernel;
            }
        }
    }
}
