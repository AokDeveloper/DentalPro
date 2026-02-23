using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Application.Features.Patients.Commands.CreatePatient
{
    public class CreatePatientCommandValidator : AbstractValidator<CreatePatientCommand>
    {
        public CreatePatientCommandValidator()
        {
            RuleFor(v => v.FirstName)
                .NotEmpty().WithMessage("İsim alanı boş geçilemez.")
                .MaximumLength(50);

            RuleFor(v => v.TCKN)
                .NotEmpty().WithMessage("TCKN gereklidir.")
                .Length(11).WithMessage("TCKN 11 haneli olmalıdır.")
                .Matches("^[0-9]*$").WithMessage("TCKN sadece rakamlardan oluşmalıdır.");

            RuleFor(v => v.PhoneNumber)
                .NotEmpty().WithMessage("Telefon numarası gereklidir.");
        }
    }
}
