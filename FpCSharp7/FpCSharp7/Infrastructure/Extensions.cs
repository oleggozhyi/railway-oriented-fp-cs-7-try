using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FpCSharp7.Infrastructure
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);
    }
}
