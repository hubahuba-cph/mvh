using Domain.DomainObjects;
using ExcelDataReader;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Mappers
{
    public interface IOrderMapper
    {
        IEnumerable<Order> Map(IExcelDataReader dataReader, string delimiter);
    }
}