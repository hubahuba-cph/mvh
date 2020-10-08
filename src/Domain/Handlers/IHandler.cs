using System;
using System.Threading.Tasks;

namespace Domain.Handlers
{
    public interface IHandler<T,K>
    {
        Task RunAsync(K options);
    }
}
