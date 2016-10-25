using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebProject.Models
{
    public class Fan
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }
        public int PazamInClub { get; set; }
        public string UserID { get; set; }
        public string Address { get; set; }
    }
}
