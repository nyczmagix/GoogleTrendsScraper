using CommandLine;
using GoogleTrendsScraper.Lib.PageObjects;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace GoogleTrendsScraper.Console.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var cmdArgs = new CommandArgs();
            Parser.Default.ParseArgumentsStrict(args, cmdArgs);

            var storiesCount =
                cmdArgs.Stories.HasValue
                ? cmdArgs.Stories.Value
                : Convert.ToInt32(ConfigurationManager.AppSettings["storiesCount"]);

            var googleTrendsUrl = cmdArgs.GoogleTrendsUrl ?? Convert.ToString(ConfigurationManager.AppSettings["googleTrendsUrl"]);

            var outputDir = cmdArgs.OutputDir ?? Convert.ToString(ConfigurationManager.AppSettings["outputDir"]);

            var outputFilename = cmdArgs.OutputFilename ?? Convert.ToString(ConfigurationManager.AppSettings["outputFilename"]);

            using (var webdriver = new ChromeDriver())
            {
                var stories = new TrendsPage(webdriver).GetStories(storiesCount);
                DumpListToFile(stories, outputDir);
            }
        }

        /// <summary>
        /// Dump a list into a file based on the specified location
        /// </summary>
        static void DumpListToFile(IEnumerable<string> list, string outputDir, string filename = null)
        {
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            filename = filename ?? DateTimeOffset.Now.ToUnixTimeSeconds().ToString() + ".txt";

            if (File.Exists(filename))
                File.Move(filename, Path.GetFileNameWithoutExtension(filename) + ".old");

            File.WriteAllLines(Path.Combine(outputDir, filename), list.ToArray<string>());
        }
    }
}
