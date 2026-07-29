using DentalApp.Application.Features.Appointments.Queries.AppointmentsByPatientId;
using DentalApp.Application.Features.Appointments.Queries.GetAllAppointments;
using FastEndpoints;
using MediatR;

namespace DentalApp.WebApi.Endpoints.Appointments
{
    public class GetCompletedAppointmentsEndpoint : Endpoint<GetCompletedAppointmentsQuery, AppointmentsByPatientIdResponse>
    {
        private readonly IMediator _mediator;

        public GetCompletedAppointmentsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            // 2. DEĞİŞİM: RESTful Route Tasarımı
            // URL'deki {PatientId} ismi, GetCompletedAppointmentsQuery içindeki parametre ile birebir aynı olmalı.
            Get("/api/patients/{PatientId}/completed-appointments");

            // Güvenlik gereği yorum satırındaki AllowAnonymous'u sildik, token zorunlu.

            Summary(s =>
            {
                s.Summary = "Hastanın Tamamlanan Randevularını Listele";
                s.Description = "Belirtilen hasta ID'sine ait tamamlanmış randevuları ve bitirme notlarını getirir.";
                s.Response(200, "Randevular başarıyla listelendi.");
                s.Response(404, "Hasta bulunamadı veya yetkisiz erişim.");
            });
        }

        // 3. DEĞİŞİM: Parametre olarak MediatR Query'sini doğrudan alıyoruz
        public override async Task HandleAsync(GetCompletedAppointmentsQuery req, CancellationToken ct)
        {
            // FastEndpoints, URL'deki {PatientId} değerini otomatik olarak 'req' nesnesinin içine doldurdu bile!
            // Artık manuel Route<Guid> okumasına gerek yok.
            var result = await _mediator.Send(req, ct);

            await SendOkAsync(result, ct);
        }
    }
}