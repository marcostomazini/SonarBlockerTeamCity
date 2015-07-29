using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.IO;

namespace SonarBlockerTeamCity
{
    class Program
    {
        private static Base _base = new Base();
        private static string url = string.Empty;
        private static string path = string.Empty;        
        private static string urlSonar = ("{0}/api/rules/show?key=");

        static void Main(string[] args)
        {
            TextWriterTraceListener writer = new TextWriterTraceListener(System.Console.Out);
            Debug.Listeners.Add(writer);


            if ((args.Length == 1) && ((args[0].ToUpper().Contains("/H")) ||
                                            (args[0].ToUpper().Contains("-H"))))
            {
                _base.HelpSystem();
                return;
            }
            else if (args.Length != 3)
            {
                System.Console.WriteLine("Argument not specified or invalid. See /help");
                Console.Read();
                return;
            }

            url = args[0];
            path = args[0];
            urlSonar = string.Format(urlSonar, args[1]);

            _base.Hello();

            System.Console.WriteLine("Sonar: " + urlSonar);
            System.Console.WriteLine("Url: " + url);

            var sonarMetricas = new List<Sonar>();
            //using (WebClient client = new WebClient())
            //{
            //    var sourceCodePage = client.DownloadData(urlFull);
            //    string pageHtmlReturn = Encoding.UTF8.GetString(sourceCodePage);

            //    var indexPerResource = pageHtmlReturn.IndexOf("issuesPerResource =");
            //    var jsonCutted = pageHtmlReturn.Substring(indexPerResource);

            //    var index = jsonCutted.IndexOf("issuesPerResource =");
            //    var indexEnd = jsonCutted.IndexOf("];");
            //    var totalToCut = (indexEnd + 3) - index;

            //    var jsonFiltered = jsonCutted.Substring(index, totalToCut);


            //    var removeCharInvalids = jsonFiltered // poderia ser usado um expressao regular
            //        .Replace("\n", string.Empty)
            //        .Replace("\r", string.Empty)
            //        .Replace("  ", string.Empty)
            //        .Replace("[],", string.Empty)
            //        .Replace("issuesPerResource =", string.Empty)
            //        .Replace(";", string.Empty)
            //        .Replace("[]", string.Empty);

            //    var listaFull = JsonConvert.DeserializeObject<List<List<Sonar>>>(removeCharInvalids);

            //    foreach (var item in listaFull)
            //    {
            //        foreach (var sitem in item)
            //        {
            //            sonarMetricas.Add(sitem);
            //        }
            //    }
            //}

            //if (!File.Exists(path))
            //{
            //    Console.BackgroundColor = ConsoleColor.Red;
            //    Console.WriteLine("Arquivo não existe");
            //    Environment.Exit(0);
            //}

            using (StreamReader sr = new StreamReader(path))
            {
                string pageHtmlReturn = sr.ReadToEnd();

                var indexPerResource = pageHtmlReturn.IndexOf("issuesPerResource =");
                var jsonCutted = pageHtmlReturn.Substring(indexPerResource);

                var index = jsonCutted.IndexOf("issuesPerResource =");
                var indexEnd = jsonCutted.IndexOf("];");
                var totalToCut = (indexEnd + 3) - index;

                var jsonFiltered = jsonCutted.Substring(index, totalToCut);


                var removeCharInvalids = jsonFiltered // poderia ser usado um expressao regular
                    .Replace("\n", string.Empty)
                    .Replace("\r", string.Empty)
                    .Replace("  ", string.Empty)
                    .Replace("[],", string.Empty)
                    .Replace("issuesPerResource =", string.Empty)
                    .Replace(";", string.Empty)
                    .Replace("[]", string.Empty);

                var listaFull = JsonConvert.DeserializeObject<List<List<Sonar>>>(removeCharInvalids);

                foreach (var item in listaFull)
                {
                    foreach (var sitem in item)
                    {
                        sonarMetricas.Add(sitem);
                    }
                }
            }

            var minor = sonarMetricas.Where(x => x.s == "minor"); 
            var major = sonarMetricas.Where(x => x.s == "major"); 
            var critical = sonarMetricas.Where(x => x.s == "critical"); 
            var blocker = sonarMetricas.Where(x => x.s == "blocker");

            try
            {
                foreach (var item in blocker)
                {
                    System.Console.WriteLine("Critical: " + item.r);

                    using (WebClient client = new WebClient())
                    {

                        var sourceCodePage = client.DownloadData(urlSonar + item.r.Remove(0, 1));
                        string pageHtmlReturn = Encoding.UTF8.GetString(sourceCodePage);

                        var sonarRules = JsonConvert.DeserializeObject<SonarRules>(pageHtmlReturn);

                        throw new Exception("Sonar >>> " + sonarRules.rule.name);
                    }                    
                    throw new Exception("Sonar >>> " + item.r.Remove(0, 1));
                }
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);

                Environment.Exit(1);
            }

            Console.Read();
        }
    }
}
