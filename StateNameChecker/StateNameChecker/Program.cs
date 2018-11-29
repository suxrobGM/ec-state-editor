using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;

namespace StateNameChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            //string currentProgramPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string currentProgramPath = @"C:\Users\SuxrobGM\Documents\Paradox Interactive\Hearts of Iron IV\mod\Test\history\states";
            string[] files = Directory.GetFiles(currentProgramPath, "*.txt", SearchOption.TopDirectoryOnly);

            foreach (var file in files)
            {
                string fileName = Path.GetFileName(file);
                if(Regex.IsMatch(fileName, @"^\d+ - \D+") || Regex.IsMatch(fileName, @"^\d+ -\D+") || Regex.IsMatch(fileName, @"^\d+- \D+"))
                {
                    Console.WriteLine(fileName);
                }
            }

            Console.WriteLine("Finished! press anyone key to continue...");
            Console.ReadLine();
        }
    }
}
