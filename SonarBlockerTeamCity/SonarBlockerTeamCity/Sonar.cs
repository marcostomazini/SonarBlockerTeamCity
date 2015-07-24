using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonarBlockerTeamCity
{
    public class Sonar
    {
        public string k { get; set; }
        public string r { get; set; }
        public int l { get; set; }
        public bool @new { get; set; }
        public string s { get; set; }
    }

    public class SonarRules
    {
        public SonarRulesDescription rule { get; set; }
    }

    public class SonarRulesDescription
    {
        public string htmlDesc { get; set; }
        public string name { get; set; }
    }
}
