using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParsingOfEducationalinstitutions
{
    class Institution
    {
        private const string _path = "https://monitoring.miccedu.ru/iam/2021/_vpo/inst.php?id=";
        public int Id { get; set; }
        string Name { get; set; }
        string Adress { get; set; }
        string Site { get; set; }
        string Department { get; set; }
        string Founder { get; set; }

        public Institution(int id)
        {
            Id = id;
        }

        public void Parse()
        {
            var getRequest = new GetRequest(_path + Convert.ToString(Id));

            getRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            getRequest.Useragent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 YaBrowser/22.11.5.715 Yowser/2.5 Safari/537.36";
            getRequest.Referer = "https://monitoring.miccedu.ru/?m=vpo&year=2021";
            getRequest.Host = "monitoring.miccedu.ru";

            getRequest.Run();

            var matchName = Regex.Matches(getRequest.Response, @"Cambria;.>(.*)</b></td></tr><tr><td class='tt'>Р", RegexOptions.Singleline);
            Name = matchName[0].Groups[1].Value;

            var matchAdress = Regex.Matches(getRequest.Response, @"coordinates=.*>(.*)</span><s");
            Adress = matchAdress[0].Groups[1].Value;

            var matchSite = Regex.Matches(getRequest.Response, @"<td class='tt'>.*ть</td>\s*<td>(.*)</td></tr>");
            Site = matchSite[0].Groups[1].Value;

            var matchDepartment = Regex.Matches(getRequest.Response, @"<td class='tt'>.*\)</td>\s*<td>(.*)</td></tr>", RegexOptions.Singleline);
            Department = matchDepartment[0].Groups[1].Value;

            var matchFounder = Regex.Matches(getRequest.Response, @"<td class='tt'>.*</td>\s*<td .*'>(.*)</td>");
            Founder = matchFounder[0].Groups[1].Value;

            Console.WriteLine(Name);

            //List<string> listRegex = new List<string> {
            //    @"<b style=.font-family:Cambria;.>(.*)</b>",
            //    @"coordinates=.*>(.*)</span><s",
            //    @"<td class='tt'>.*ть</td>\s*<td>(.*)</td></tr>",
            //    @"<td class='tt'>.*т</td>\s*<td>(.*)</td></tr>",
            //    @"<td class='tt'>.*\)</td>\s*<td>(.*)</td></tr>",
            //    @"<td class='tt'>.*</td>\s*<td .*'>(.*)</td>"
            //};
        }
    }
}
