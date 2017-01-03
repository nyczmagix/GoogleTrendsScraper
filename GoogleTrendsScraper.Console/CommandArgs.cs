using CommandLine;

namespace GoogleTrendsScraper.Console
{
    public class CommandArgs
    {
        [Option('s', "stories", Required = false,
            HelpText = "Number of stories to extract.")]
        public int? Stories { get; set; }

        [Option('u', "url", Required = false,
            HelpText = "The URL to Google Trends.")]
        public string GoogleTrendsUrl { get; set; }

        [Option('o', "outputDir", Required = false,
            HelpText = "The output location to write the file.")]
        public string OutputDir { get; set; }
    }
}
