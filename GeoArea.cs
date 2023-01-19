using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingOfEducationalinstitutions
{
    class GeoArea
    {
        int Id { get; set; }
        string Name { get; set; }
        List<Institution> Institutions { get; set; }

        public GeoArea(int id)
        {
            Id = id;
        }

        public void Parse(string html)
        {

        }
    }
}
