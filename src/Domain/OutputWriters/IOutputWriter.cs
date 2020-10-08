using Domain.DomainObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.OutputWriters
{
    public interface IOutputWriter<T>
    {
        Task OutputAsync(Options options, IEnumerable<T> list);
    }
}
