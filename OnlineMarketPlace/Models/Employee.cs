using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMarketPlace
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FathersName { get; set; }
        // should it be so? should be just date ideally
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PassportData { get; set; }

        // additional to just Person, employee data
        // has to be pulled in a specific query
        //
        public int Wage { get; set; }
        public decimal Rating { get; set; }
    }
}
