using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParsingOfEducationalinstitutions
{
    class GeoArea
    {
        private const string _path = "https://monitoring.miccedu.ru/iam/2021/_vpo/material.php?type=2&id=";
        int Id { get; set; }
        string Name { get; set; }
        public List<Institution> Institutions { get; set; }

        public GeoArea(int id)
        {
            Id = id;
            Institutions = new List<Institution>();
        }

        public void Parse()
        {
            var getRequest = new GetRequest(_path+Convert.ToString(Id));

            getRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            getRequest.Useragent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 YaBrowser/22.11.5.715 Yowser/2.5 Safari/537.36";
            getRequest.Referer = "https://monitoring.miccedu.ru/?m=vpo&year=2021";
            getRequest.Host = "monitoring.miccedu.ru";

            getRequest.Run();

            var matchName = Regex.Matches(getRequest.Response, @"color:#678;.>(.*)</div>");
            Name = matchName[0].Groups[1].Value;

            Regex regexId = new Regex(@"<td id=(\d*)");
            MatchCollection matches = regexId.Matches(getRequest.Response);

            foreach (Match match in matches)
            {
                Institution institution = new Institution(Int32.Parse(match.Groups[1].Value));
                Institutions.Add(institution);
                //Console.WriteLine(institution.Id);
            }

        }
    }
}
