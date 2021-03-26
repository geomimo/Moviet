using Moviet.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Moviet.Services
{
    public class PornDetectorService : IPornDetectorService
    {
        public bool IsPorn(string image_path, string text)
        {
            // 0: safe, 1: porn
            int ImageResult = CheckImage(image_path);

            int TextResult = CheckText(text);

            return (ImageResult == 1) || (TextResult == 1);
        }

        private int CheckText(string text)
        {
            string[] pornWords = File.ReadAllLines(".\\pornwords.txt");
            int threshold = 2;
            int counter = 0;
            foreach(var word in pornWords)
            {
                if (text.Contains(word))
                {
                    counter++;
                }

                if(counter == threshold)
                {
                    return 1;
                }
            }

            return 0;
            
        }

        private int CheckImage(string image_path)
        {
            // 0: safe, 1: porn
            var py_path = Path.GetFullPath("..\\PornImageDetection\\porn_image_detection.py");
            var image_full_path = Path.GetFullPath("wwwroot\\"+image_path);
            return Run_cmd(py_path, image_full_path);
        }

        private int Run_cmd(string cmd, string args)
        {

            var start = new ProcessStartInfo
            {
                FileName = @"cmd.exe",
                //Arguments = string.Format("C:\\Users\\geomimo\\anaconda3\\python.exe {0} {1}", cmd, args),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                CreateNoWindow = true
            };

            var process = Process.Start(start);

            using (StreamWriter sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("conda activate");
                    sw.WriteLine(string.Format("C:\\Users\\geomimo\\anaconda3\\python.exe {0} {1}", cmd, args));
                }
            }

            List<string> results = new List<string>();
            
            using (StreamReader reader = process.StandardOutput)
            {
                while (!reader.EndOfStream)
                {
                    results.Add(reader.ReadLine());
                }
            }
            


            return Int32.Parse(results[results.Count - 3]);



        }
    }
}
