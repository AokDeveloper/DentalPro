using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Doctors.Commands.CreateDoctor
{
   
        public record CreateDoctorCommand : IRequest<Guid>
        {
        public string TCKN { get; init; }
        public string FirstName { get; init; }
            public string LastName { get; init; }
                 
    }
}
