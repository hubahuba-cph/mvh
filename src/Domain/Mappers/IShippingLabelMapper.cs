using Domain.DomainObjects;
using ExcelDataReader;
using System.Collections.Generic;

namespace Domain.Mappers
{
    public interface IShippingLabelMapper
    {
        IEnumerable<ShippingLabel> Map(IExcelDataReader dataReader);
    }
}