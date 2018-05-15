using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace JEG.CLasses{
    public class JEGClass{
        private Random rnd;
        private StringBuilder mem;
        private readonly string input,output;
        private readonly bool verbse;
        private StreamWriter sw;
        private XmlWriter xmlWriter;
        public JEGClass(string InputFile,string OutPutFile,bool Verbose){
            this.verbse = Verbose;
            this.input = InputFile;
            this.output = OutPutFile;
            rnd = new Random();
        }
        
        private void ParseComplex(XElement complex){
            var name = complex.Attribute("name");
            if(name != null){
                string Name = name.Value;
                int Repeat = 1;
                var repeat = complex.Attribute("repeat");
                if (repeat != null)
                 Repeat = int.Parse(repeat.Value);
                 GenerateComplexBase(complex,Repeat);
            }else{
                throw new Exception("Terminate : Complex has no name ...");
            }
        }
        private void ParseInt(XElement simple){
            var name = simple.Attribute("name");
            if(name != null){
            var Min = simple.Attribute("min");
            var Max = simple.Attribute("max");
            int min=0,max=int.MaxValue;
             if(Min != null)
                min = int.Parse(Min.Value);
             if(Max != null)
                max = int.Parse(Max.Value);
            int val = rnd.Next(min,max);
            if(verbse)
                Console.WriteLine($"{name.Value} : {val}");
            xmlWriter.WriteElementString(name.Value,val.ToString());
            }
        }

        private void ParseChar(XElement simple){
            const string chars = 
            @"qazwsxedcrfvtgbyhnujmik,ol.p;/'[]\`1234567890-=QAZXSWEDCVFRTGBNHYUJM,KIOL.P!@#$%^7*()";
            var name = simple.Attribute("name");
            if(name != null){
             int inx = rnd.Next(0,chars.Length);
             if(verbse)
                Console.WriteLine($"{name.Value} : {chars[inx]}");
            xmlWriter.WriteElementString(name.Value,chars[inx]+"");
            }
        }

        private void ParseString(XElement simple){
            const string chars = 
            @"qazwsxedcrfvtgbyhnujmik,ol.p;/'[]\`1234567890-=QAZXSWEDCVFRTGBNHYUJM,KIOL.P!@#$%^7*()";
            var name = simple.Attribute("name");
            if(name != null){
                int len = 10;
                string tmp = "";
                var Len = simple.Attribute("length");
                if(Len != null)
                    len = int.Parse(Len.Value);
                    for(int i =0;i<len;i++)
                     tmp += chars[rnd.Next(0,chars.Length)];
                     if(verbse)
                        Console.WriteLine($"{name.Value} : {tmp}");
            xmlWriter.WriteElementString(name.Value,tmp);
            }
        }

        private void ParseReal(XElement simple){
            var name = simple.Attribute("name");
            if(name != null){
            var Min = simple.Attribute("min");
            var Max = simple.Attribute("max");
            double min=0,max=double.MaxValue;
             if(Min != null)
                min = double.Parse(Min.Value);
             if(Max != null)
                max = double.Parse(Max.Value);
            double val = rnd.NextDouble();
            if(verbse)
                Console.WriteLine($"{name.Value} : {val}");
            xmlWriter.WriteElementString(name.Value,val.ToString());
            }
        }

        private void ParseDate(XElement simple){
            var name = simple.Attribute("name");
            if(name != null){
            var dt = new DateTime();
            if(verbse)
                Console.WriteLine($"{name.Value} : {dt.ToShortDateString()}");
            xmlWriter.WriteElementString(name.Value,dt.ToShortDateString());
            }
        }
        private void ParseTime(XElement simple){
            var name = simple.Attribute("name");
            if(name != null){
            var dt = new DateTime();
            if(verbse)
                Console.WriteLine($"{name.Value} : {dt.ToShortTimeString()}");
            xmlWriter.WriteElementString(name.Value,dt.ToShortTimeString());
            }
        }
        private void ParseDateTime(XElement simple){
            var name = simple.Attribute("name");
            if(name != null){
            var dt = new DateTime();
            if(verbse)
                Console.WriteLine($"{name.Value} : {dt.ToString()}");
            xmlWriter.WriteElementString(name.Value,dt.ToString());
            }
        }
        private void ParseNow(XElement simple){
            ParseDateTime(simple);
        }

        private void ParseBool(XElement simple){
            var name = simple.Attribute("name");
            if(name != null){
             bool tst =  rnd.Next(1,100) % 2 == 0;
             if(verbse)
                Console.WriteLine($"{name.Value} : {tst}");
            xmlWriter.WriteElementString(name.Value,tst.ToString());
            }
        }
        
        private void ParseSimple(XElement simple){
            var tp = simple.Attribute("type");
            if(tp != null){
            switch (tp.Value){
                case "int":ParseInt(simple);break;
                case "date":ParseDate(simple);break;
                case "datetime":ParseDateTime(simple);break;
                case "time":ParseTime(simple);break;
                case "now":ParseNow(simple);break;
                case "bool":ParseBool(simple);break;
                case "real":ParseReal(simple);break;
                case "char":ParseChar(simple);break;
                case "string":ParseString(simple);break;
            }
            }else{
                throw new Exception("Simple has no type");
            }
        }
        private void ParseChild(XElement child){
            if (child.Name == "simple")
              ParseSimple(child);
            else if (child.Name == "complex")
              ParseComplex(child);
              else {
                  throw new Exception("Unknown element");
              }
        }

         private void GenerateComplexBase(XElement element,int repeat){
                var childern = element.Descendants();
                var name = element.Attribute("name");
                xmlWriter.WriteStartElement(name.Value);
            for(int i=0;i<repeat;i++){
                if(verbse)
                    Console.WriteLine($"{i} ***************************** ");
                foreach(var child in childern){
                    ParseChild(child);
                }
            }
                xmlWriter.WriteEndElement();
         }
        
        private void Generate(XElement element,int repeat){
                var childern = element.Descendants();
            for(int i=0;i<repeat;i++){
                var name = element.Attribute("name");
                xmlWriter.WriteStartElement(name.Value);
                if(verbse)
                    Console.WriteLine($"{i} ***************************** ");
                foreach(var child in childern){
                    ParseChild(child);
                }
                xmlWriter.WriteEndElement();
            }
        }
        
        private void ParseEvent(XElement element){
            var name = element.Attribute("name");
            var repeat = element.Attribute("repeat");
            if(name != null){
                string Name = name.Value;
                int Repeat = 1;
                if (repeat != null)
                 Repeat = int.Parse(repeat.Value);
                 if(verbse)
                    Console.WriteLine($"{name} / {repeat}");
                 Generate(element,Repeat);
            }else{
                throw new Exception("Terminate : Event has no name ...");
            }
        }
        private void initialOutput(){
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Events");
        }
        public void StartGenerate(){
            mem = new StringBuilder();

            var sr = new StreamReader(input);
            var doc = XDocument.Load(sr);
            var events = from e in doc.Descendants("event") 
            select e;
            if(verbse)
                Console.WriteLine($"{events.Count()} Events detected.");
             if(File.Exists(output))
                        File.Delete(output);
                 sw = new StreamWriter(output);
                 var xs = new XmlWriterSettings();
                 xs.Indent = true;
                 xmlWriter = XmlWriter.Create(sw,xs);
                 initialOutput();
            foreach(XElement e in events)
             {
                
                 ParseEvent(e);
                 
             }
             xmlWriter.WriteEndElement();
                 xmlWriter.Flush();
                 sw.Flush();
                 xmlWriter.Close();
                 sw.Close();
        }
    }
}