using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.Entities
{
    public class AccessLog
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string QueryName { get; set; }
        public string QueryParameters { get; set; }               
        public string? ResponseData { get; set; }   
        public DateTime AccessedOn { get; set; }
    }
}