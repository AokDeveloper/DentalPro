using DentalApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Common.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(AppUser user, string doctorId = null);
    }
}