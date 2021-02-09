using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Helpers
{
    public static class NumberInformationJSONGenerator
    {
        private const string quote = "\"";

        public static string GenerateJSON(ICollection<NummerDefinitionQuelle> nummerDefinitionQuelles, object[] quellen)
        {
            if (nummerDefinitionQuelles == null) throw new ArgumentNullException(nameof(nummerDefinitionQuelles));
            if (nummerDefinitionQuelles.Count == 0) throw new ArgumentException(nameof(nummerDefinitionQuelles) + "ist leer");
            if (quellen == null) throw new ArgumentNullException(nameof(quellen));
            if (quellen.Count() == 0) throw new ArgumentException(nameof(quellen) + "ist leer");
            if (nummerDefinitionQuelles.Count != quellen.Count()) throw new Exception(nameof(nummerDefinitionQuelles) + "und"+ nameof(quellen)+ "haben unterschiedliche Größen");

            StringBuilder json = new StringBuilder();
            json.Append("{");
            int index = 0;
            foreach (var item in nummerDefinitionQuelles.OrderBy(e => e.NummerDefinitionQuellePos))
            {
                object quelle = quellen[index];
                string quelleAlsString = "";
                switch (item.NummerDefinitionQuelleDatentypId)
                {
                    case 1:
                        //String
                        quelleAlsString = quote + quelle.ToString() + quote;
                        break;
                    case 2:
                        //Integer
                        quelleAlsString = quelle.ToString();
                        break;
                    case 3:
                        //GUID
                        quelleAlsString = quote + quelle.ToString() + quote;
                        break;

                    default:
                        break;
                }

                json.Append(quote + item.NummerDefinitionQuelleBezeichnung + quote + ": " + quelleAlsString + ", ");
                index++;

            }
            json.Append("}");


            return json.ToString();
        }
    }
}
