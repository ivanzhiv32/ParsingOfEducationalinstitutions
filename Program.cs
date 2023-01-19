using System;
using System.Collections.Generic;
using System.Net;

namespace ParsingOfEducationalinstitutions
{
    class Program
    {
        static void Main(string[] args)
        {
            //var year = "2021";
            //var proxy = new WebProxy("127.0.0.1:8888");
            //var cookieContainer = new CookieContainer();
            var path = $"https://monitoring.miccedu.ru/?m=vpo&year=2021";

            var getRequest = new GetRequest(path);

            getRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            getRequest.Useragent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 YaBrowser/22.11.5.715 Yowser/2.5 Safari/537.36";
            getRequest.Referer = "https://monitoring.miccedu.ru/?m=vpo";
            getRequest.Host = "monitoring.miccedu.ru";

            getRequest.Run();

            WholeCountry country = new WholeCountry();
            country.Parse(getRequest.Response);

            List<GeoArea> geoAreas = country.GeoAreas;

            foreach (var area in geoAreas)
            {
                area.Parse();
                List<Institution> institutions = area.Institutions;
                foreach (var institution in institutions)
                {
                    institution.Parse();
                }
            }

            //List<Institution> institutions = geoAreas.

            Console.ReadKey();
        }
    }
}
