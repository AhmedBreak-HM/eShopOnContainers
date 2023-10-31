using Matgr.UI.Models;
using Matgr.UI.Models.Dtos;

namespace Matgr.UI.Services
{
    public interface IBaseService : IDisposable
    {
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
