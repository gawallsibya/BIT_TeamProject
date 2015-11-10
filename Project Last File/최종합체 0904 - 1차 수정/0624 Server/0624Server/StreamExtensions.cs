using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;

namespace _0624Server
{
    public static class StreamExtensions
    {
        public static void CopyTo(this Stream input, Stream output)
        {
            byte[] numArray = new byte[2048];
            int num = 0;
            while (true)
            {
                int num1 = input.Read(numArray, 0, 2048);
                num = num1;
                bool flag = num1 > 0;
                if (!flag)
                {
                    break;
                }
                output.Write(numArray, 0, num);
            }
        }
    }
}
