using System.Threading.Tasks;
using ReaFx.DataAccess.Common.Services;
using DevFun.Common.Entities;

namespace DevFun.Common.Services
{
    public interface IDevJokeService : IEntityService<DevJoke, int>
    {
        Task<DevJoke> GetRandomJoke();
        
    }
}