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

            using (var webdriver = new ChromeDriver())
            {
                var stories = new TrendsPage(webdriver).GetStories(storiesCount);
                DumpStoriesToFile(stories, outputDir);
            }
        }

        /// <summary>
        /// Dump a list into a file based on the specified location
        /// </summary>
        static void DumpStoriesToFile(IEnumerable<string> stories, string outputDir)
        {
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            string file = DateTimeOffset.Now.ToUnixTimeSeconds().ToString() + ".txt";
            if (File.Exists(file))
                File.Move(file, Path.GetFileNameWithoutExtension(file) + ".old");

            File.WriteAllLines(Path.Combine(outputDir, file), stories.ToArray<string>());
        }
    }
}
