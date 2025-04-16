using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.IO;
using CsvHelper;
using System.Globalization;

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
        private List<string> linksReady = new List<string>();
        private List<string> unitsMeasure = new List<string>();

        private DataBase dataBase = new DataBase();

        public Parser()
        {
            linksReady = dataBase.GetLinksReady();
        }

        public Parser(List<string> linksReady)
        {
            this.linksReady = linksReady;
        }
        public void Start()
        {
            linksReady = dataBase.GetLinksReady();
            Console.WriteLine("Ссылки получены\n");

            for (int year = 2023; year > 2014; year--)
            {
                YearReport yearReport = new YearReport(year);
                if (linksReady.Contains(GenerateLink(yearReport))) continue;
                ParseYearReport(yearReport);
            }
        }
        public void ParseYearReport(YearReport yearReport)
        {
            string link = GenerateLink(yearReport);

            if (linksReady.Contains(link)) throw new Exception("Данные о выбранном регионе уже собраны");
            if (yearReport.Year > 2023 || yearReport.Year < 2015) throw new Exception("Веб-ресурс не содержит данные за выбранную дату");

            var getRequest = new GetRequest(link);
            getRequest.Run();

            if (yearReport.Year > 2017)
            {
                var document = new HtmlParser().ParseDocument(getRequest.Response);

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
            
            int year = dataBase.AddYearReport(yearReport);
            Console.Write("Данные " + yearReport.Year + " года добавлены\n");

            Regex regex = new Regex(@"id=(\d{5})'");
            MatchCollection matches = regex.Matches(getRequest.Response);

            foreach (Match match in matches)
            {
                Region region = new Region(Int32.Parse(match.Groups[1].Value), yearReport.Year);
                if (!linksReady.Contains(GenerateLink(region))) yearReport.Regions.Add(region);
            }

            foreach(Region region in yearReport.Regions)
            {
                ParseRegion(region, year);
            }
            //Parallel.ForEach(yearReport.Regions, region =>
            //{
            //    ParseRegion(region, idYearReport);
            //});

            dataBase.AddLinkReady(link);
            linksReady.Add(link);
        }

        public void ParseRegion(Region region, int year)
        {
            string link = GenerateLink(region);

            if (linksReady.Contains(link)) throw new Exception("Данные о выбранном регионе уже собраны");

            var getRequest = new GetRequest(link, region.Year);
            getRequest.Run();

            MatchCollection matchName = Regex.Matches(getRequest.Response, @"color:#678;.>(.*)</div>");
            region.Name = matchName[0].Groups[1].Value;

            int idRegion = dataBase.AddRegion(region);
            Console.Write("Справочные данные " + region.Name + " добавлены\n");

            var parser = new HtmlParser();
            var document = parser.ParseDocument(getRequest.Response);

            var table = document.GetElementById("statistic_info_chart_kont");
            var values = table.GetElementsByClassName("val");
            region.CountAllStudents = Convert.ToInt32(values[0].TextContent.Replace(" ", ""));
            region.CountFullTimeStudents = Convert.ToInt32(values[1].TextContent.Replace(" ", ""));
            if (region.Year > 2017) region.CountFreeFormStudents = Convert.ToInt32(values[2].TextContent.Replace(" ", ""));
            else region.CountFreeFormStudents = 0;

            bool idRegionReport = dataBase.AddRegionReport(region, year);
            Console.Write("Годовой отчет " + region.Name + " добавлен\n");

            MatchCollection matchesInstitution = Regex.Matches(getRequest.Response, @"<td id=(\d*)");

            foreach (Match match in matchesInstitution)
            {
                Institution institution = new Institution(Int32.Parse(match.Groups[1].Value), region.Year);
                if (!linksReady.Contains(GenerateLink(institution))) region.Institutions.Add(institution);
            }

            Parallel.ForEach(region.Institutions, institution =>
            {
                ParseInstitution(institution, idRegion, year);
            });

            dataBase.AddLinkReady(link);
            linksReady.Add(link);
        }

        public void ParseInstitution(Institution institution, int idRegion, int year)
        {
            string link = GenerateLink(institution);

            if (linksReady.Contains(link)) throw new Exception("Данные о выбранном институте уже собраны");

            var getRequest = new GetRequest(link, institution.Year);
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

            bool is_exist = dataBase.AddInstitution(institution, idRegion);
            Console.Write("Справочные данные института добавлены\n");

            int idInstitutionReport = dataBase.AddInstitutionReport(institution.Id, year);

            ParseMainTables(document, year, idInstitutionReport);
            ParseSecondaryTable(document, idInstitutionReport);
            ParseBranchOfScience(document, idInstitutionReport);
            //ParseUgn(document, idInstitution);
            
            Console.Write("Данные годового отчета института добавлены\n");

            dataBase.AddLinkReady(link);
            linksReady.Add(link);
        }

        /// <summary>
        /// Сбор данных из основных таблиц сайта
        /// </summary>
        /// <param name="document"></param>
        /// <param name="idInstitutionReport"></param>
        public void ParseMainTables(AngleSharp.Html.Dom.IHtmlDocument document, int year, int idInstitutionReport)
        {
            double value;
            string name, unit_measure, number;

            var tables = document.GetElementsByClassName("napde");

            foreach (var table in tables)
            {
                var rows = table.QuerySelectorAll("tr");

                foreach (var row in rows.Skip(1))
                {
                    var cells = row.QuerySelectorAll("td");

                    number = cells[0].TextContent.Replace(".", ",");
                    name = cells[1].TextContent;
                    unit_measure = cells[2].TextContent;

                    try
                    {
                        value = Convert.ToDouble(cells[3].TextContent.Replace(".", ","));
                    }
                    catch
                    {
                        value = 0;
                    }

                    //institution.Indicators.Add(new Indicator(number, name, unit_measure, value));
                    //int idUnitMeasure = dataBase.AddUnitMeasure(unit_measure);
                    int idIndicator = dataBase.AddIndicator(name, number, unit_measure);
                    dataBase.AddValueIndicator(idInstitutionReport, year, idIndicator, value);
                }
            }
        }

        /// <summary>
        /// Сбор данных из таблицы с дополнительными характеристиками
        /// </summary>
        /// <param name="document"></param>
        /// <param name="idInstitutionReport"></param>
        public void ParseSecondaryTable(AngleSharp.Html.Dom.IHtmlDocument document, int idInstitutionReport)
        {
            double value;
            string name, unit_measure, number;

            var table_dop = document.GetElementById("analis_dop");
            if (table_dop == null) return;

            var rows_dop = table_dop.QuerySelectorAll("tr");
            foreach (var row in rows_dop.Skip(3))
            {
                var cells = row.QuerySelectorAll("td");
                if (cells.Count() < 3)
                {
                    continue;
                }
                else if (cells.Count() == 4)
                {
                    number = cells[0].TextContent.Replace(".", ",");
                    name = cells[1].TextContent;
                    unit_measure = cells[2].TextContent;
                    try
                    {
                        value = Convert.ToDouble(cells[3].TextContent.Replace(".", ","));
                    }
                    catch
                    {
                        value = 0;
                    }
                }
                else
                {
                    number = "0";
                    name = cells[0].TextContent;
                    unit_measure = cells[1].TextContent;
                    try
                    {
                        value = Convert.ToDouble(cells[2].TextContent.Replace(".", ","));
                    }
                    catch
                    {
                        value = 0;
                    }
                }

                //institution.Indicators.Add(new Indicator(number, name, unit_measure, value));
                int idUnitMeasure = dataBase.AddUnitMeasure(unit_measure);
                int idNameIndicator = dataBase.AddIndicator(name, number, idUnitMeasure);
                dataBase.AddValueIndicator(idInstitutionReport, idNameIndicator, value);
            }
        }
        /// <summary>
        /// Сбор данных об отраслях наук
        /// </summary>
        /// <param name="document"></param>
        /// <param name="idInstitutionReport"></param>
        public void ParseBranchOfScience(AngleSharp.Html.Dom.IHtmlDocument document, int idInstitutionReport)
        {
            int value;
            string name;

            var table_branches = document.GetElementById("kont_by_otr");
            if (table_branches == null) return;

            var rows_branches = table_branches.QuerySelectorAll("tr");
            foreach (var row in rows_branches.Skip(1))
            {
                var cells = row.QuerySelectorAll("td");
                if (cells.Count() != 3)
                {
                    continue;
                }
                else
                {
                    name = cells[0].TextContent;
                    try
                    {
                        value = Convert.ToInt32(cells[2].QuerySelector("span").TextContent);
                    }
                    catch
                    {
                        value = 0;
                    }
                }

                int idBranch = dataBase.AddBranchScience(name);
                dataBase.AddDistributionBranch(idInstitutionReport, idBranch, value);
            }
        }

        /// <summary>
        /// Сбор данных о специальностях, реализуемых в образовательной организации
        /// </summary>
        /// <param name="document"></param>
        /// <param name="idInstitution"></param>
        public void ParseUgn(AngleSharp.Html.Dom.IHtmlDocument document, int idInstitution)
        {
            string name; 
            int value;

            var table_reg = document.GetElementById("analis_reg");
            if (table_reg == null) return;

            var rows_reg = table_reg.QuerySelectorAll("tr");
            foreach (var row in rows_reg.Skip(2))
            {
                var cells = row.QuerySelectorAll("td");
                if (cells.Count() < 6)
                {
                    continue;
                }
                else
                {
                    name = cells[0].TextContent;
                    try
                    {
                        value = Convert.ToInt32(cells[1].TextContent.Replace(".", ","));
                    }
                    catch
                    {
                        value = 0;
                    }
                }

                int idUgn = dataBase.AddUgn(name);
                //institution.Ugns.Add(new Ugn(idUgn, name));
                dataBase.AddDistributionUgn(idInstitution, idUgn, value);
            }
        }
        public void ParseReviews()
        {
            string filePath = @"C:\Users\Admin\PycharmProjects\parserFeedback\feedbacks.csv";

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>().ToList();

                // Обработка записей
            }
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
