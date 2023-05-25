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
        private static void ThrowOnError(ErrorCode err) => ClHelpers.ThrowOnError(err);
        private static object _cacheLock = new();
        private static Dictionary<GPU, Dictionary<Type, Dictionary<string, Kernel>>> _cache = new();
        private static Dictionary<Kernel, Program> _programStorage = new();
        public static Dictionary<Type, string> CNameLookupTable = new()
        {
            { typeof(ushort), "ushort" },
            { typeof(float), "float" },
            { typeof(double), "double" },
            { typeof(Half), "half" },
            { typeof(ulong), "ulong" },
            { typeof(int), "int" },
            { typeof(byte), "char" },
            { typeof(short), "short" },
        };

        public static void ClearCacheAndDisposeKernels()
        {
            lock(_cacheLock)
            {
                foreach(var gpu in _cache.Keys)
                {
                    foreach(var type in _cache[gpu].Keys)
                    {
                        foreach(var op in _cache[gpu][type].Keys)
                        {
                            var kernel = _cache[gpu][type][op];
                        
                            _programStorage[kernel].Dispose();
                            _programStorage.Remove(kernel);

                            _cache[gpu][type][op].Dispose();
                            _cache[gpu][type].Remove(op);
                        }

                        _cache[gpu].Remove(type);
                    }
                    _cache.Remove(gpu);
                }
            }
        }

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
                
                string cName;

                if (CNameLookupTable.ContainsKey(type))
                    cName = CNameLookupTable[type];
                else
                    cName = type.Name;

                string? code = null;
                string? raw = null;

                // if a specific kernel for this combination exists compile it
                var specialPath = $"kernels/{cName}/{op}.cl";
                var genericOperationPath = $"kernels/generics/{op}.cl";

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

                    code = raw.Replace("TYPENAMEHERE", cName);
                }

                if (code == null)
                    throw new NullReferenceException();

                var program = Cl.CreateProgramWithSource(gpu.Context, 1, new[] { code }, null, out var error);

                ThrowOnError(error);

                error = Cl.BuildProgram(program, 1, new[] { gpu.Device }, null, null, IntPtr.Zero);

                ThrowOnError(error);

                var kernel = Cl.CreateKernel(program, op, out error);

                ThrowOnError(error);

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

                if (!_programStorage.ContainsKey(kernel))
                    _programStorage.Add(kernel, program);

                return kernel;
            }
        }
    }
}
