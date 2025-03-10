﻿using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using Krompaco.RecordCollector.Web.Extensions;
using Markdig;

namespace Krompaco.RecordCollector.Web.Views.Shared
{
    public class BoardBlock
    {
        public static string GetTypeOfTest(int id)
        {
            var d = new Dictionary<int, string>
            {
                { 1, "Lighthouse Performance" },
                { 2, "404 page" },
                { 4, "Lighthouse SEO" },
                { 5, "Lighthouse Best Practices" },
                { 6, "W3C HTML" },
                { 7, "W3C CSS" },
                { 9, "Standard files" },
                { 10, "Lighthouse A11y" },
                { 15, "Sitespeed.io" },
                { 17, "Yellow Lab Tools" },
                { 18, "Pa11y" },
                { 20, "Integrity & Security" },
                { 21, "HTTP & Tech" },
                { 22, "Carbon Calculator" },
            };

            if (d.ContainsKey(id))
            {
                return d[id];
            }

            return "Unknown test";
        }

        public static double GetAverage(int forSiteId, List<Site> sites, List<Test> flatResults)
        {
            var siteResults = flatResults.Where(x => x.SiteId == forSiteId).ToList();
            return !siteResults.Any() ? 0d : siteResults.Average(x => x.Rating);
        }

        public static string GetFileName(Site site)
        {
            var pageFileName = GetSiteName(site.Url);
            return pageFileName
                .Replace(".", "-")
                .Replace("/", "-") + ".html";
        }

        public static string GetSiteName(string input)
        {
            return input
                .TrimEnd("/".ToCharArray())
                .Replace("https://www.", string.Empty)
                .Replace("http://www.", string.Empty)
                .Replace("https://", string.Empty)
                .Replace("http://", string.Empty);
        }

        public static string GetSiteNameHtml(string input)
        {
            return WebUtility.HtmlEncode(GetSiteName(input))
                .Replace(".", "&thinsp;.")
                .Replace("/", "/&thinsp;");
        }

        public static string GetHtmlFromRating(Test result)
        {
            var md = result.Report;

            if (!string.IsNullOrWhiteSpace(result.ReportA11y) && result.TypeOfTest != 9 && result.TypeOfTest != 21)
            {
                md += Environment.NewLine + Environment.NewLine + result.ReportA11y;
            }

            if (!string.IsNullOrWhiteSpace(result.ReportPerf) && result.TypeOfTest != 9 && result.TypeOfTest != 21)
            {
                md += Environment.NewLine + Environment.NewLine + result.ReportPerf;
            }

            if (!string.IsNullOrWhiteSpace(result.ReportSec) && result.TypeOfTest != 9 && result.TypeOfTest != 21)
            {
                md += Environment.NewLine + Environment.NewLine + result.ReportSec;
            }

            if (!string.IsNullOrWhiteSpace(result.ReportStand) && result.TypeOfTest != 9)
            {
                md += Environment.NewLine + Environment.NewLine + result.ReportStand;
            }

            var pl = new MarkdownPipelineBuilder()
                .Use<HtmlTableWithWrapperExtension>()
                .UseAbbreviations()
                .UseCitations()
                .UseDefinitionLists()
                .UseEmphasisExtras()
                .UseFigures()
                .UseFooters()
                .UseFootnotes()
                .UseGridTables()
                .UseMathematics()
                .UseMediaLinks()
                .UsePipeTables()
                .UseListExtras()
                .UseTaskLists()
                .UseDiagrams()
                .UseAutoLinks()
                .UseGenericAttributes()
                .Build();

            return Markdown.ToHtml(md, pl)
                .Replace("<li>", "<li class=\"text-xs\">")
                .Replace("<ul>", "<ul class=\"list-disc list-outside ml-4 space-y-1\">")
                .Replace("org.w3c.css.parser.analyzer.ParseException", "analyzer.ParseException")
                .Replace("Look up", " Look up");
        }

        public static void CreateSitePages(List<Site> sites, List<Test> flatResults, string contentPath)
        {
            var sitesPath = Path.Combine(contentPath, "sites");

            if (!Directory.Exists(sitesPath))
            {
                Directory.CreateDirectory(sitesPath);
            }

            foreach (var site in sites)
            {
                var pageFullPath = Path.Combine(sitesPath, GetFileName(site));

                if (File.Exists(pageFullPath))
                {
                    return;
                }

                var siteResults = flatResults.Where(x => x.SiteId == site.Id).ToList();

                if (!siteResults.Any())
                {
                    continue;
                }

                var pageHtml = $"<p class=\"inline-block mt-4 bg-green-100 text-xl tracking-tight font-bold md:text-2xl px-2 py-1 rounded-md\">{siteResults.Average(x => x.Rating).ToString("0.00", new CultureInfo("en-US"))}</p><p class=\"mt-4 font-medium text-lg\">Results collected {siteResults.First().Date:yyyy-MM-dd} from <a href=\"{WebUtility.HtmlEncode(site.Url)}\" class=\"link-primary outline-primary\">{WebUtility.HtmlEncode(site.Url)}</a></p>";

                foreach (var result in siteResults)
                {
                    pageHtml +=
                        $"<div class=\"mt-10\"><h2 class=\"mt-6 pt-6 border-t border-gray-800 text-xl tracking-tight font-bold md:text-2xl\">{WebUtility.HtmlEncode(GetTypeOfTest(result.TypeOfTest))}</h2><p class=\"inline-block my-3 bg-orange-300 px-2 py-1 rounded-md\">{result.Rating.ToString("0.00", new CultureInfo("en-US"))}</p>{GetHtmlFromRating(result)}</div>";
                }

                var pageFileContent = @"{
    ""title"": """ + GetSiteNameHtml(site.Url) + @""",
    ""date"": """ + DateTime.UtcNow.ToString("yyyy-MM-dd") + @""",
    ""description"": ""Webperf ratings for " + GetSiteName(site.Url) + @".""
}
" + pageHtml;

                using var sw = new StreamWriter(File.Open(pageFullPath, FileMode.Create), new UTF8Encoding(false));
                sw.WriteLine(pageFileContent);
            }
        }

        // Models used in JSON files
        public class SiteRoot
        {
            public SiteRoot()
            {
                this.Sites = new List<Site>();
            }

            [JsonPropertyName("sites")]
            public List<Site> Sites { get; set; }
        }

        public class Site
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("url")]
            public string Url { get; set; }
        }

        public class TestRoot
        {
            public TestRoot()
            {
                this.Tests = new List<Test>();
            }

            [JsonPropertyName("tests")]
            public List<Test> Tests { get; set; }
        }

        public class Test
        {
            [JsonPropertyName("site_id")]
            public int SiteId { get; set; }

            [JsonPropertyName("type_of_test")]
            public int TypeOfTest { get; set; }

            [JsonPropertyName("rating")]
            public double Rating { get; set; }

            [JsonPropertyName("rating_sec")]
            public double RatingSec { get; set; }

            [JsonPropertyName("rating_perf")]
            public double RatingPerf { get; set; }

            [JsonPropertyName("rating_a11y")]
            public double RatingA11y { get; set; }

            [JsonPropertyName("rating_stand")]
            public double RatingStand { get; set; }

            [JsonPropertyName("date")]
            public DateTime Date { get; set; }

            [JsonPropertyName("report")]
            public string Report { get; set; }

            [JsonPropertyName("report_sec")]
            public string ReportSec { get; set; }

            [JsonPropertyName("report_perf")]
            public string ReportPerf { get; set; }

            [JsonPropertyName("report_a11y")]
            public string ReportA11y { get; set; }

            [JsonPropertyName("report_stand")]
            public string ReportStand { get; set; }

            [JsonPropertyName("data")]
            public string Data { get; set; }
        }
    }
}
