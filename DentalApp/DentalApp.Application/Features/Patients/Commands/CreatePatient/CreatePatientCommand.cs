
using MediatR;

namespace DentalApp.Application.Features.Patients.Commands.CreatePatient
{
    public record CreatePatientCommand: IRequest<Guid>
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string TCKN { get; init; }
        public string PhoneNumber { get; init; }
        public DateOnly BirthDate { get; set; }
        public Guid SupervisorId { get; set; }

    }

}
