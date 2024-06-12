using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace SF_ECMS_Utils.Helpers;

public static class CSVUtility {

    public static string EscapeString(string value) {

        return "\"" + value.Replace("\"", "\"\"") + "\"";

    }

    public static string UnEscapeString(string value) {

        if (value.StartsWith("\"")) {
            value = value.Remove(0, 1);
        }

        value = value.Replace("\"\"", "\"");

        if (value.EndsWith("\"")) {
            value = value.Remove(value.Length - 1);
        }

        return value;

    }

    public static List<string> Serialize(List<Dictionary<string, string>> linesAsDictionaries) {

        bool haveWrittenCSVHeaders = false;
        List<string> csvFileContents = [];

        linesAsDictionaries.ForEach(linesAsDictionary => {

            if (!haveWrittenCSVHeaders) {

                csvFileContents.Add(
                    string.Join(
                        ",",
                        linesAsDictionary.Keys.Select(v => CSVUtility.EscapeString(v))
                    )
                );

                haveWrittenCSVHeaders = true;

            }

            csvFileContents.Add(
                string.Join(
                    ",",
                    linesAsDictionary.Values.Select(v => CSVUtility.EscapeString(v))
                )
            );

        });

        return csvFileContents;

    }

}
