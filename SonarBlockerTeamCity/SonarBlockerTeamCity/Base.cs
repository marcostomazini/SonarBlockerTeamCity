using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonarBlockerTeamCity
{
    public class Base
    {
        private StringBuilder sb = new StringBuilder();
        private const string traco = "--------------------------------------------------------------------------------";
        private const string copyright = "--------------------------- marcos.tomazini@gmail.com --------------------------";
        private const string generate = "-------------------------- gerado por Marcos Tomazini --------------------------";

        public void Hello()
        {
            Copyright();
            Console.WriteLine(sb.ToString());
        }

        private string Copyright()
        {
            sb.Clear();
            sb.AppendLine(traco);
            sb.AppendLine(generate);
            sb.AppendLine(copyright);
            sb.AppendLine(traco);

            return sb.ToString();
        }

        public void HelpSystem()
        {
            System.Console.WriteLine("first parameter: url teamcity");            
            Console.BackgroundColor = ConsoleColor.Blue;
            System.Console.WriteLine("sample: url...");
            System.Console.WriteLine("http://svrhomtreetech:8080/repository/download/Develop_TreetechSamDevDB1/SEU_ID_AQUI:id/issues-report.zip%21/issues-report-light.html");
            System.Console.WriteLine("http://svrhomtreetech:8080/repository/download/Develop_TreetechSamDevDB1/{0}:id/issues-report.zip%21/issues-report-light.html"); // {0} parametro a ser passado
            Console.ResetColor();
            System.Console.WriteLine(" ");
            System.Console.WriteLine("second parameter: id changeset");
            Console.BackgroundColor = ConsoleColor.Blue;
            System.Console.WriteLine("sample: 7322");
            System.Console.WriteLine("http://svrhomtreetech:8080/repository/download/Develop_TreetechSamDevDB1/7322:id/issues-report.zip%21/issues-report-light.html"); // {0} parametro a ser passado concatenado
            Console.ResetColor();
            System.Console.WriteLine(" ");
            System.Console.WriteLine("third parameter: url sonar");
            Console.BackgroundColor = ConsoleColor.Blue;
            System.Console.WriteLine("sample: http://svrhomtreetech:5880");            
            Console.ResetColor();
            Console.Read();
        }
    }
}
