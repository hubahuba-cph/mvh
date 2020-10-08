using Domain.DomainObjects;
using ExcelDataReader;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mappers
{
    public class ShippingLabelMapper : IShippingLabelMapper
    {
        private readonly ILogger<ShippingLabelMapper> _logger;

        public ShippingLabelMapper(ILogger<ShippingLabelMapper> logger)
        {
            _logger = logger;
        }

        public IEnumerable<ShippingLabel> Map(IExcelDataReader dataReader)
        {
            _logger.LogInformation($"Map using OrderMapper.");

            yield return new ShippingLabel
            {
                Tmstmp = DateTime.Parse(dataReader.GetString(0)),
                Name = dataReader.GetString(1),
                Recipient = dataReader.GetString(2),
                StreetName = dataReader.GetString(3),
                StreetNo = dataReader.GetValue(4) == null ? string.Empty : dataReader.GetString(4),
                AreaCode = dataReader.GetDouble(5).ToString(),
                AreaName = dataReader.GetString(6),
                Units = dataReader.GetDouble(7).ToString(),
                Email = dataReader.GetString(8),
                Phone = dataReader.GetValue(9).ToString()
            };
        }
    }
}
