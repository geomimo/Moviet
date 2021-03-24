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
            var py_path = Path.GetFullPath("../PornImageDetection/porn_image_detection.py");
            var image_full_path = Path.GetFullPath("wwwroot\\"+image_path);
            return Run_cmd(py_path, image_full_path);
        }

        private int Run_cmd(string cmd, string args)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "C:/Users/geomimo/anaconda3/python.exe",
                    Arguments = string.Format("{0} {1}", cmd, args),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                }
            };
          
            process.Start();
            Thread.Sleep(3000);

            StreamReader myStreamReader = process.StandardOutput;
            string output = myStreamReader.ReadToEnd();
            process.WaitForExit();
            process.Close();


            return Int32.Parse(output.Split('/')[0]);



        }
    }
}
