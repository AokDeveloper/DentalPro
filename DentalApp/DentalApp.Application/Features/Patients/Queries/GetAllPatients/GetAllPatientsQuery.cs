using DentalApp.Application.Common.Interfaces;
using DentalApp.Application.Features.Appointments.Queries;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace DentalApp.Application.Features.Patients.Queries.GetAllPatients
{
    // 1. İSTEK (Parametre yok, hepsini istiyoruz)
    public record GetAllPatientsQuery : IRequest<GetAllPatientsResponse>, ILoggableQuery;


}


