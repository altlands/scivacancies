using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SciVacancies.Services
{
    public class UpdateConfigService
    {
        public bool VerifyIfChangeEnvInFile(IDictionary vars, string applicationBasePath, char directorySeparatorChar, string devEnvValue, Dictionary<string, string> environmentsSource)
        {
            var sourceFileName = String.IsNullOrEmpty(devEnvValue) ? "config.json" : $"config.{devEnvValue}.json";
            var useNonTemplateConfigFile = false;

            var environments = new List<KeyValuePair<string, string>>();
            foreach (var environment in environmentsSource)
            {
                var env_value = vars.Cast<DictionaryEntry>().FirstOrDefault(e => e.Key.Equals(environment.Value));
                if (env_value.Value != null)
                {
                    Console.WriteLine(environment.Value + ".Value = " + env_value.Value);
                    environments.Add(new KeyValuePair<string, string>(environment.Key, (string)env_value.Value));
                }
                else
                    Console.WriteLine(environment.Value + ".Value = null");
            }


            var fileLines = File.ReadLines(applicationBasePath + directorySeparatorChar + sourceFileName).ToList();

            for (var environmentIndex = 0; environmentIndex < environments.Count; environmentIndex++)
            {
                for (var lineIndex = 0; lineIndex < fileLines.Count; lineIndex++)
                {
                    if (!fileLines[lineIndex].Contains(environments[environmentIndex].Key)) continue;
                    useNonTemplateConfigFile = true;
                    lineIndex = fileLines.Count;
                    environmentIndex = environments.Count;
                }
            }

            Console.WriteLine("useNonTemplateConfigFile: " + useNonTemplateConfigFile);
            if (!useNonTemplateConfigFile) return false;



            var result_ModifiedStrings = new List<string>();

            foreach (string fileLine in fileLines)
            {
                var newLine = fileLine;

                foreach (var environmentPair in environments)
                {
                    if (newLine.Contains(environmentPair.Key))
                    {
                        newLine = newLine.Replace(environmentPair.Key, environmentPair.Value);
                    }
                }

                result_ModifiedStrings.Add(newLine);
            }
            var newFileName = string.IsNullOrEmpty(devEnvValue) ? "config.modified.json" : $"config.{devEnvValue}.modified.json";

            File.WriteAllLines($"{applicationBasePath}{directorySeparatorChar}{newFileName}", result_ModifiedStrings);

            return true;
        }
    }
}
