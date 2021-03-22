namespace API.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Data.Models;

    public static class NumberInformationJSONGenerator
    {
        private const string Quote = "\"";

        public static string GenerateJSON(ICollection<NummerDefinitionQuelle> nummerDefinitionQuelles, object[] quellen)
        {
            if (nummerDefinitionQuelles == null)
            {
                throw new ArgumentNullException(nameof(nummerDefinitionQuelles));
            }

            if (nummerDefinitionQuelles.Count == 0)
            {
                throw new ArgumentException(nameof(nummerDefinitionQuelles) + "ist leer");
            }

            if (quellen == null)
            {
                throw new ArgumentNullException(nameof(quellen));
            }

            if (quellen.Count() == 0)
            {
                throw new ArgumentException(nameof(quellen) + "ist leer");
            }

            if (nummerDefinitionQuelles.Count != quellen.Count())
            {
                throw new Exception(nameof(nummerDefinitionQuelles) + "und" + nameof(quellen) + "haben unterschiedliche Größen");
            }

            StringBuilder json = new StringBuilder();
            json.Append("{");
            int index = 0;
            foreach (var item in nummerDefinitionQuelles.OrderBy(e => e.Position))
            {
                object quelle = quellen[index];
                string quelleAlsString = string.Empty;
                switch (item.DatentypenId)
                {
                    case 1:
                        // String
                        quelleAlsString = Quote + quelle.ToString() + Quote;
                        break;
                    case 2:
                        // Integer
                        quelleAlsString = quelle.ToString();
                        break;
                    case 3:
                        // GUID
                        quelleAlsString = Quote + quelle.ToString() + Quote;
                        break;

                    default:
                        break;
                }

                json.Append(Quote + item.Bezeichnung + Quote + ": " + quelleAlsString + ", ");
                index++;
            }

            json.Append("}");

            return json.ToString();
        }
    }
}
