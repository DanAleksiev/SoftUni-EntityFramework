using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSnake.IO
    {
    public class FileReader
        {
        string path = "../../../scores.txt";

        public string ReadFile()
            {
            using StreamReader reader = new StreamReader(path);
                {
                return reader.ReadLine();
                }
            }
        }
    }
