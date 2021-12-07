using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderUtilities
{
    public static class Reader
    {
        public static string ReadAsText(string filename)
        {
            string fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\", filename);
            return File.ReadAllText(fileLocation);
        }
        public static string[] ReadLines(string filename)
        {
            string fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\", filename);
            return File.ReadAllLines(fileLocation);
        }
    }
}
