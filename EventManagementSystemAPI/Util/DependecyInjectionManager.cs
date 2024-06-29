using EventManagementSystemAPI.Filters;
using EventMS.Application.Port;
using EventMS.Application.Ports;
using EventMS.Application.UseCases;
using EventMS.Application.UseCases.UserUseCases;
using EventMS.Domain.Interfaces;
using EventMS.Infrastructure.Repositories;
using Microsoft.AspNetCore.Hosting;

namespace EventManagementSystemAPI.Util
{
    public static class DependencyInjectionManager
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        public static void AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<IGetAllEventsUseCase, GetAllEventsUseCase>();
            services.AddScoped<ICreateEventUseCase, CreateEventUseCase>();
            services.AddScoped<IUpdateEventUseCase, UpdateEventUseCase>();
            services.AddScoped<IDeleteEventUseCase, DeleteEventUseCase>();
            services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
            services.AddScoped<IGetEventByIdUseCase, GetEventByIdUseCase>();

            services.AddScoped<IGetAllUsersUseCase, GetAllUsersUseCase>();
            services.AddScoped<IGetUserByIdUseCase, GetUserByIdUseCase>();

            
            services.AddScoped<ITypeTicketRepository, TypeTicketRepository>();
            services.AddScoped<IDeleteTicketsByIdUseCase, DeleteTicketsByIdUseCase>();
        }

    }
}
