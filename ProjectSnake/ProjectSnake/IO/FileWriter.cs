using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSnake.IO
    {
    public class FileWriter
        {
        string path = @"..\..\..\scores.txt";
        public void Write(string message)
            {
            using (StreamWriter writer = new StreamWriter(path, false))
                {
                writer.Write(message);

                }
            }

        public void WriteLine(string message)
            {
            using (StreamWriter writer = new StreamWriter(path, false))
                {
                writer.WriteLine(message);

                }
            }
        }
    }
