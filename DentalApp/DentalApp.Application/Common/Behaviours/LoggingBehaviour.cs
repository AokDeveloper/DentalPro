using DentalApp.Application.Common.Interfaces;
using DentalApp.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DentalApp.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : notnull
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationDbContext _context;

        public LoggingBehaviour(ICurrentUserService currentUserService, IApplicationDbContext context)
        {
            _currentUserService = currentUserService;
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // 1. Önce asıl işi çalıştır ve CEVABI AL
            // (Burada veritabanına gidip veriyi çekiyor)
            var response = await next();

            // 2. Eğer bu sorgu loglanabilir bir sorguysa
            if (request is ILoggableQuery)
            {
                try
                {
                    var accessLog = new AccessLog
                    {
                        UserId = _currentUserService.UserId ?? "Anonymous",
                        QueryName = typeof(TRequest).Name,

                        // Soru: Hangi ID'yi istedi?
                        QueryParameters = JsonSerializer.Serialize(request),

                        // Cevap: Kullanıcıya ne döndük? (DTO'nun kendisi)
                        ResponseData = JsonSerializer.Serialize(response),

                        AccessedOn = DateTime.UtcNow
                    };

                    _context.AccessLogs.Add(accessLog);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch
                {
                    // Loglama hatası akışı bozmasın
                }
            }

            return response;
        }
    }
}