using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DentalApp.Domain.Entities
{
    // IdentityUser'dan miras alıyoruz, böylece şifreleme, email onayı vb. hazır geliyor.
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        // İleride buraya "DoctorId" vb. ekleyebiliriz.
    }
}