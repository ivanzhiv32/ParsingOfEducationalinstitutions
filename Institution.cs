using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingOfEducationalinstitutions
{
    class Institution
    {
        int Id { get; set; }
        string Name { get; set; }
        string Adress { get; set; }
        string Site { get; set; }
        string Department { get; set; }
        string Founder { get; set; }

        public Institution(int id)
        {
            Id = id;
        }

        public void Parse(string html)
        {

        }
    }
}
