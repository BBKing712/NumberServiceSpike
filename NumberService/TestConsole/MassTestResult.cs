
namespace TestConsole
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class MassTestResult
    {
    public MassTestResult()
        {
            this.MassTestMeasures = new List<MassTestMeasure>();
        }
 

        public long CountOfDefinitions { get; set; }
        public long CountOfInformations { get; set; }
        public long CountOfErstelleNummerInformationen { get; set; }
        public List<MassTestMeasure> MassTestMeasures { get; set; }

    }
}
