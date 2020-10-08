using Domain.DomainObjects;
using Domain.Mappers;
using Domain.Handlers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Domain.OutputWriters;
using System.Linq;
using System.IO;
using System;
using CsvHelper;

namespace Domain.Handlers
{
    public interface IShippingLabelHandler : IHandler<ShippingLabelHandler, ShippingLabelHandlerOptions>
    {

    }

    public class ShippingLabelHandler : BaseHandler<ShippingLabel>, IShippingLabelHandler
    {
        private readonly IShippingLabelMapper _mapper;
        private readonly IOutputWriter<ShippingLabel> _outputWriter;

        public ShippingLabelHandler(ILogger<ShippingLabelHandler> logger, IShippingLabelMapper mapper, IOutputWriter<ShippingLabel> outputWriter) : base(logger)
        {
            _mapper = mapper;
            _outputWriter = outputWriter;
        }

        public async Task RunAsync(ShippingLabelHandlerOptions options)
        {
            DateTime cutOfTmstmp;
            if (options.PointInTimeFile.Exists == false) 
            {
                cutOfTmstmp = DateTime.Now.AddDays(-1);
                File.WriteAllText(options.PointInTimeFile.FullName, cutOfTmstmp.ToString("o"));
            }
            else
            {
                var rc = DateTime.TryParse(File.ReadAllText(options.PointInTimeFile.FullName), out cutOfTmstmp);

                if(rc == false) { throw new InvalidDataException($"Invalid data in {options.PointInTimeFile.FullName}"); }
            }

            var list = ProcessFiles(options, (labels) => (labels.First().Tmstmp > cutOfTmstmp), (reader) => _mapper.Map(reader));

            if(list.Count() == 0)
            {
                _logger.LogWarning("No records found..");
            }
            else
            {
                await _outputWriter.OutputAsync(options, list);
                File.WriteAllText(options.PointInTimeFile.FullName, list.Max(i => i.Tmstmp).ToString("o"));
            }
        }
    }
}
