using System;
using System.IO;
using JEG.CLasses;
using System.Data;
using System.Timers;

namespace JEG
{
    class Program
    {
        private static void validate(string input,string output,bool verbose){
            if(!File.Exists(input))
             Console.WriteLine($"[{input}]not exists .");
            else
            if (Path.GetExtension(input).ToLower() != ".xml")
                    Console.WriteLine($"[{input}] should be an xml file.");
            else{
                Console.WriteLine($"{input} ---> {output}");
                Console.WriteLine($"verbose ---> {verbose}");
                new JEGClass(input,output,verbose).StartGenerate();
            }
        }

        private static void Parse(string[] args){
            string input="",output="";
            bool verbose = false;
            int i=0;
            while(i<args.Length){
                var  a = args[i];
                switch (a) {
                    case "-i":
                     input = args[i+1];
                     i++;
                     break;
                    case "-o":
                     output = args[i+1];
                     i++;
                     break;
                    case "-v":
                    verbose = true;
                    break;
                    default:
                    Console.WriteLine($"Undefined parameter {a}");
                    return;
                }
                i++;
            }
            validate(input,output,verbose);
        }

        private static void PrintArgs(string[] args){
            foreach (var a in args)
                Console.WriteLine(a);
        }

        static void Main(string[] args)
        {
           
            //Console.Clear();
            Console.WriteLine("JEG (Jenadeleh Event Generator) .");
            if(args.Length > 0){
                var watch = System.Diagnostics.Stopwatch.StartNew();
                Console.WriteLine($"Start time : {DateTime.Now.ToLongTimeString()}");
                Parse(args);
                watch.Stop();
                Console.WriteLine($"Milliseconds : {watch.ElapsedMilliseconds}");
                //PrintArgs(args);

            }else{
                Console.WriteLine("To Generate event use JEG like this : ");
                Console.WriteLine("JEG [-i input-file.xml] [-o output-file] [-v verbose]");
            }
        }
    }
}
