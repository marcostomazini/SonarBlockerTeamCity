using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace SonarBlockerTeamCity
{
    class Program
    {
        private static Base _base = new Base();
        private static string url = string.Empty;
        private static string urlSonar = ("{0}/api/rules/show?key=");
        private static string id = string.Empty;

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
            id = args[1];
            urlSonar = string.Format(urlSonar, args[2]);

            _base.Hello();

            var urlFull = string.Format(url, id);


            var sonarMetricas = new List<Sonar>();
            using (WebClient client = new WebClient())
            {
                var sourceCodePage = client.DownloadData(urlFull);
                string pageHtmlReturn = Encoding.UTF8.GetString(sourceCodePage);

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

            foreach (var item in blocker)
            {
                System.Console.WriteLine("Critical: " + item.r);                

                using (WebClient client = new WebClient())
                {

                    var sourceCodePage = client.DownloadData(urlSonar + item.r.Remove(0, 1));
                    string pageHtmlReturn = Encoding.UTF8.GetString(sourceCodePage);

                    var sonarRules = JsonConvert.DeserializeObject<SonarRules>(pageHtmlReturn);

                    throw new Exception("Sonar >>> Qualidade Critica em ::: " + sonarRules.rule.name);
                }

                throw new Exception("Sonar >>> Qualidade Critica em " + item.r.Remove(0, 1));
            }
        }
    }
}
