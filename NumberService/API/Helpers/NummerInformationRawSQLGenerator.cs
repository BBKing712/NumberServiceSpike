
namespace API.Helpers
{
    using API.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class NummerInformationRawSQLGenerator
    {
        private const string quotationmark = "'";
        public static string GenersateRawSQL(long nummer_definition_id, ICollection<NummerDefinitionQuelle> nummerDefinitionQuelles, object[] quellen)
        {
            //string rAWsql = "";
            if (nummerDefinitionQuelles == null) throw new ArgumentNullException(nameof(nummerDefinitionQuelles));
            if (nummerDefinitionQuelles.Count == 0) throw new ArgumentException(nameof(nummerDefinitionQuelles) + "ist leer");
            if (quellen == null) throw new ArgumentNullException(nameof(quellen));
            if (quellen.Count() == 0) throw new ArgumentException(nameof(quellen) + "ist leer");
            if (nummerDefinitionQuelles.Count != quellen.Count()) throw new Exception(nameof(nummerDefinitionQuelles) + "und" + nameof(quellen) + "haben unterschiedliche Größen");

            StringBuilder rAWsql = new StringBuilder();
            rAWsql.Append("Select * from nummer_information");
            rAWsql.Append(" Where nummer_definition_id = " + nummer_definition_id.ToString());
            int index = 0;
            foreach (var item in nummerDefinitionQuelles.OrderBy(e => e.NummerDefinitionQuellePos))
            {
                object quelle = quellen[index];
                string quelleAlsString = "";
                switch (item.NummerDefinitionQuelleDatentypId)
                {
                    case 1:
                        //String
                        quelleAlsString = quotationmark + quelle.ToString() + quotationmark;
                        break;
                    case 2:
                        //Integer
                        quelleAlsString = quelle.ToString();
                        break;
                    case 3:
                        //GUID
                        quelleAlsString = quotationmark + quelle.ToString() + quotationmark;
                        break;

                    default:
                        break;
                }

                //JSON_VALUE(Nnmmer_information_quelle, '$.Wert1') = 'abc';
                rAWsql.Append(" And JSON_VALUE(Nnmmer_information_quelle, '$." + item.NummerDefinitionQuelleBezeichnung + "') = " + quelleAlsString);
                index++;
            }


            return rAWsql.ToString();
        }
    }
}
