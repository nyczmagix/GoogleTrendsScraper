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

            var appSettings = ConfigurationManager.AppSettings;
            var storiesCount =
                cmdArgs.Stories.HasValue
                ? cmdArgs.Stories.Value
                : Convert.ToInt32(appSettings["storiesCount"]);

            var googleTrendsUrl = cmdArgs.GoogleTrendsUrl ?? Convert.ToString(appSettings["googleTrendsUrl"]);

            var outputDir = cmdArgs.OutputDir ?? Convert.ToString(appSettings["outputDir"]);

            var filename = cmdArgs.Filename ?? Convert.ToString(appSettings["filename"]);

            using (var webdriver = new ChromeDriver())
            {
                var stories = new TrendsPage(webdriver).GetStories(storiesCount);
                DumpListToFile(stories, outputDir, filename);
            }
        }

        /// <summary>
        /// Dump a list into a file based on the specified location
        /// </summary>
        static void DumpListToFile(IEnumerable<string> list, string outputDir, string filename = "")
        {
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            filename = Path.Combine(outputDir,
                string.IsNullOrEmpty(filename)
                ? DateTimeOffset.Now.ToUnixTimeSeconds().ToString() + ".txt"
                : filename);

            // delete file if one currently exists
            if (File.Exists(filename))
                File.Delete(filename);

            File.WriteAllLines(filename, list.ToArray<string>());
        }
    }
}
