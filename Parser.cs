using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

namespace ParsingOfEducationalinstitutions
{
    enum TypeData
    {
        Year,
        Region,
        Institution
    }
    class Parser
    {
        public List<string> linksReady = new List<string>();

        public Parser()
        {
        }

        public Parser(List<string> linksReady)
        {
            this.linksReady = linksReady;
        }
        public List<YearReport> ParseAllData()
        {
            List<YearReport> listYearReports = new List<YearReport>();

            for (int year = 2022; year > 2014; year--)
            {
                YearReport yearReport = new YearReport(year);
                if (linksReady.Contains(GenerateLink(yearReport))) continue;
                ParseYearReport(yearReport);
                listYearReports.Add(yearReport);
            }

            return listYearReports;
        }
        public void ParseYearReport(YearReport yearReport)
        {
            string link = GenerateLink(yearReport);

            if (linksReady.Contains(link)) throw new Exception("Данные о выбранном регионе уже собраны");
            if (yearReport.Year > 2022 || yearReport.Year < 2015) throw new Exception("Веб-ресурс не содержит данные за выбранную дату");

            var getRequest = new GetRequest(link);
            getRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            getRequest.Useragent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 YaBrowser/22.11.5.715 Yowser/2.5 Safari/537.36";
            getRequest.Referer = "https://monitoring.miccedu.ru/?m=vpo";
            getRequest.Host = "monitoring.miccedu.ru";
            getRequest.Run();

            if (yearReport.Year > 2017)
            {
                var parser = new HtmlParser();
                var document = parser.ParseDocument(getRequest.Response);

                var table = document.GetElementById("statistic_info_chart_kont");
                var values = table.GetElementsByClassName("val");

                yearReport.CountAllStudents = Convert.ToInt32(values[0].TextContent.Replace(" ", ""));
                yearReport.CountFullTimeStudents = Convert.ToInt32(values[1].TextContent.Replace(" ", ""));
                yearReport.CountFreeFormStudents = Convert.ToInt32(values[2].TextContent.Replace(" ", ""));
            }
            else
            {
                yearReport.CountAllStudents = 0;
                yearReport.CountFullTimeStudents = 0;
                yearReport.CountFreeFormStudents = 0;
            }


            Regex regex = new Regex(@"id=(\d{5})'");
            MatchCollection matches = regex.Matches(getRequest.Response);

            foreach (Match match in matches)
            {
                Region region = new Region(Int32.Parse(match.Groups[1].Value), yearReport.Year);
                if (!linksReady.Contains(GenerateLink(region))) yearReport.Regions.Add(region);
            }

            Parallel.ForEach(yearReport.Regions, region =>
            {
                ParseRegion(region);
            });

            linksReady.Add(link);
        }

        public void ParseRegion(Region region)
        {
            string link = GenerateLink(region);

            if (linksReady.Contains(link)) throw new Exception("Данные о выбранном регионе уже собраны");

            var getRequest = new GetRequest(link);

            getRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            getRequest.Useragent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 YaBrowser/22.11.5.715 Yowser/2.5 Safari/537.36";
            getRequest.Referer = "https://monitoring.miccedu.ru/?m=vpo&year=" + Convert.ToString(region.Year);
            getRequest.Host = "monitoring.miccedu.ru";

            getRequest.Run();

            MatchCollection matchName = Regex.Matches(getRequest.Response, @"color:#678;.>(.*)</div>");
            region.Name = matchName[0].Groups[1].Value;

            var parser = new HtmlParser();
            var document = parser.ParseDocument(getRequest.Response);

            var table = document.GetElementById("statistic_info_chart_kont");
            var values = table.GetElementsByClassName("val");
            region.CountAllStudents = Convert.ToInt32(values[0].TextContent.Replace(" ", ""));
            region.CountFullTimeStudents = Convert.ToInt32(values[1].TextContent.Replace(" ", ""));
            if (region.Year > 2017) region.CountFreeFormStudents = Convert.ToInt32(values[2].TextContent.Replace(" ", ""));
            else region.CountFreeFormStudents = 0;

            MatchCollection matchesInstitution = Regex.Matches(getRequest.Response, @"<td id=(\d*)");

            foreach (Match match in matchesInstitution)
            {
                Institution institution = new Institution(Int32.Parse(match.Groups[1].Value), region.Year);
                if (!linksReady.Contains(GenerateLink(institution))) region.Institutions.Add(institution);
            }

            Parallel.ForEach(region.Institutions, institution =>
            {
                ParseInstitution(institution);
            });

            linksReady.Add(link);
        }

        public void ParseInstitution(Institution institution)
        {
            string link = GenerateLink(institution);

            if (linksReady.Contains(link)) throw new Exception("Данные о выбранном институте уже собраны");

            var getRequest = new GetRequest(link);
            getRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            getRequest.Useragent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 YaBrowser/22.11.5.715 Yowser/2.5 Safari/537.36";
            getRequest.Referer = "https://monitoring.miccedu.ru/?m=vpo&year=" + Convert.ToString(institution.Year);
            getRequest.Host = "monitoring.miccedu.ru";
            getRequest.Run();

            var parser = new HtmlParser();
            var document = parser.ParseDocument(getRequest.Response);

            var table_info = document.GetElementById("info");
            var rows_info = table_info.QuerySelectorAll("tr");

            institution.Name = rows_info[0].QuerySelectorAll("td")[1].TextContent;
            institution.Adress = rows_info[1].GetElementsByTagName("span")[0].TextContent;
            institution.Department = rows_info[2].QuerySelectorAll("td")[1].TextContent;
            institution.Site = rows_info[3].QuerySelectorAll("td")[1].TextContent;
            institution.Founder = rows_info[4].QuerySelectorAll("td")[1].TextContent;

            var tables = document.GetElementsByClassName("napde");

            double number, value;
            string name, unit_measure;

            foreach (var table in tables)
            {
                var rows = table.QuerySelectorAll("tr");

                foreach (var row in rows.Skip(1))
                {
                    var cells = row.QuerySelectorAll("td");

                    number = Convert.ToDouble(cells[0].TextContent.Replace(".", ","));
                    name = cells[1].TextContent;
                    unit_measure = cells[2].TextContent;

                    try
                    {
                        value = Convert.ToDouble(cells[3].TextContent.Replace(".", ","));
                    }
                    catch
                    {
                        value = Double.NaN;
                    }
                    //if (cells[3].TextContent == "-") value = Double.NaN;
                    //else value = Convert.ToDouble(cells[3].TextContent.Replace(".", ","));

                    institution.Indicators.Add(new Indicator(number, name, unit_measure, value));
                }
            }

            linksReady.Add(link);
        }

        public string GenerateLink(Institution institution)
        {
            return "https://monitoring.miccedu.ru/iam/" + Convert.ToString(institution.Year) + "/_vpo/inst.php?id=" + Convert.ToString(institution.Id);
        }
        public string GenerateLink(Region region)
        {
            return "https://monitoring.miccedu.ru/iam/" + Convert.ToString(region.Year) + "/_vpo/material.php?type=2&id=" + Convert.ToString(region.Id);
        }
        public string GenerateLink(YearReport year)
        {
            return "https://monitoring.miccedu.ru/?m=vpo&year=" + Convert.ToString(year.Year);
        }
    }
}
