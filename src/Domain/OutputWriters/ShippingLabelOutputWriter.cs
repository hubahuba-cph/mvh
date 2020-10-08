using CsvHelper;
using CsvHelper.Configuration;
using Domain.DomainObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Domain.OutputWriters
{
    public class ShippingLabelOutputWriter : IOutputWriter<ShippingLabel>
    {
        private readonly Factory _csvhelperFactory;
        private readonly CsvConfiguration _csvConfiguration;

        public ShippingLabelOutputWriter(Factory csvHelperFactory, CsvConfiguration csvConfiguration)
        {
            _csvhelperFactory = csvHelperFactory;
            _csvConfiguration = csvConfiguration;
        }

        public async Task OutputAsync(Options options, IEnumerable<ShippingLabel> list)
        {
            var shippingLabelHandlerOptions = options as ShippingLabelHandlerOptions;

            shippingLabelHandlerOptions.OutputFile.Directory.Create();

            using (var sw = new StreamWriter(options.OutputFile.FullName))
            {
                using (var cw = _csvhelperFactory.CreateWriter(sw, _csvConfiguration))
                {
                    await cw.WriteRecordsAsync(list);
                }
            }
        }
    }
}
