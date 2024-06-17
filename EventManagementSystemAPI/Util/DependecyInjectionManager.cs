using EventMS.Application.Port;
using EventMS.Application.Ports;
using EventMS.Application.UseCases;
using EventMS.Domain.Interfaces;
using EventMS.Infrastructure.Repositories;

namespace EventManagementSystemAPI.Util
{
    public static class DependencyInjectionManager
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IEventRepository, EventRepository>();
            //1
            services.AddScoped<ITicketRepository, TicketRepository>();

        }

        public static void AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<ICreateEventUseCase, CreateEventUseCase>();
            services.AddScoped<IUpdateEventUseCase, UpdateEventUseCase>();
            services.AddScoped<ICreateTicketUseCase, CreateTicketUseCase>();
        }

    }
}
