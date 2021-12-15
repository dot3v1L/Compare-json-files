using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Compare_objects
{
    class Program
    {
        const string pathOneFile = "File-A.json";
        const string pathTwoFile = "File-B.json";

        static void Main(string[] args)
        {

            if (args.Length > 0)
            {
                if (File.Exists(args[0]) && File.Exists(args[1]))
                    Console.WriteLine(CompareJObject(JObject.Parse(File.ReadAllText(args[0])),
                        JObject.Parse(File.ReadAllText(args[1]))));
                else
                    Console.WriteLine("File not found");
            }
            else
            {
                if(File.Exists(pathOneFile) && File.Exists(pathTwoFile))
                    Console.WriteLine(CompareJObject(JObject.Parse(File.ReadAllText(pathOneFile)),
                        JObject.Parse(File.ReadAllText(pathTwoFile))));
                else
                    Console.WriteLine("File not found");
            }
            Console.ReadKey();
        }

        static string CompareJObject(JObject fileA, JObject fileB)
        {
            string result = "";

            foreach (KeyValuePair<string, JToken> pair in fileA)
            {
                if (pair.Value.Type == JTokenType.Object)
                    result += $"{CompareJObject(pair.Value.ToObject<JObject>(), fileB.GetValue(pair.Key).ToObject<JObject>())}";

                else
                {
                    if (fileB.GetValue(pair.Key) == null)
                        result += $"Key {pair.Key}: not found\r\n";
                    else
                    {
                        if(!JToken.DeepEquals(pair.Value, fileB.GetValue(pair.Key)))
                        {
                            result += $"Key {pair.Key}: {fileB.GetValue(pair.Key)} != {pair.Value}\r\n";
                        }
                    }
                }
            }

            return result;
        }
    }
}
