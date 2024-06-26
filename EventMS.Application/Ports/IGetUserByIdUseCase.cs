using EventMS.Domain.Entities;
using System.Threading.Tasks;

namespace EventMS.Application.Ports
{
    public interface IGetUserByIdUseCase
    {
        Task<User> ExecuteAsync(string userId);
    }
}
