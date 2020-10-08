using System;
using System.Threading.Tasks;

namespace DomainObjects
{
    public interface IHandler<T>
    {
        Task RunAsync();
    }
}
