using OpenCL.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPU.Math.Net
{
    internal class ClHelpers
    {
        public static void ThrowOnError(ErrorCode err)
        {
            if (err != ErrorCode.Success)
                throw new Cl.Exception(err);
        }
    }
}
