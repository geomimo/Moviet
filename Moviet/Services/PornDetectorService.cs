using Moviet.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
            return Run_cmd("..\\PornImageDetection\\porn_image_detection.py", image_path);
        }

        private int Run_cmd(string cmd, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "C:\\Users\\geomimo\\anaconda3\\python.exe";
            start.Arguments = string.Format("{0} {1}", cmd, args);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    return Int32.Parse(result);
                }
            }
        }
    }
}
