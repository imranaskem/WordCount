using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace wordCount
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Path.GetFullPath(args[0]);

            var Urls = new List<string>();


            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Urls.Add(line);
                }
            }

            var outputStream = new Dictionary<string, int>();

            foreach (var url in Urls)
            {
                var web = new HtmlWeb();
                var doc = web.Load(url);

                var wordsCount = new List<string>();

                var introNodes = doc.DocumentNode.SelectNodes("//div[@class='section intro']");

                if (introNodes != null)
                {
                    foreach (var item in introNodes)
                    {
                        var words = item.InnerText.Trim().Split(' ');

                        wordsCount.AddRange(words);

                    }
                }

                var pNodes = doc.DocumentNode.SelectNodes("//div[@class='grid g15 m20']");

                if (pNodes != null)
                {
                    var pNodeList = pNodes.Descendants().Where(node => node.Name == "p");

                    foreach (var item in pNodeList)
                    {
                        var words = item.InnerText.Trim().Split(' ');

                        wordsCount.AddRange(words);
                    }
                }              
                
                Console.WriteLine($"URL: {url} Length: {wordsCount.Count}");

                outputStream.Add(url, wordsCount.Count);
            }

            using (StreamWriter writer = new StreamWriter("output.csv"))
            {
                foreach (var item in outputStream)
                {
                    writer.WriteLine($"{item.Key},{item.Value}");
                }
            }

            Console.WriteLine("CSV written to output.csv");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
