using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentCommandValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("Hasta seçimi zorunludur.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Randevu tarihi zorunludur.")
                .GreaterThan(DateTime.UtcNow).WithMessage("Geçmiş bir tarihe randevu oluşturulamaz.");

            // İsteğe bağlı not uzunluğu kontrolü
            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Not alanı en fazla 500 karakter olabilir.");
        }
    }
}