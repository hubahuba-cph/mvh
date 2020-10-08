using Domain.DomainObjects;
using Domain.Mappers;
using Domain.OutputWriters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Domain.Handlers
{
    public interface IOrderHandler : IHandler<OrderHandler, OrderHandlerOptions>
    {

    }

    public class OrderHandler : BaseHandler<Order>, IOrderHandler
    {
        private readonly IOrderMapper _mapper;
        private readonly IOutputWriter<Order> _outputWriter;

        public OrderHandler(ILogger<OrderHandler> logger, IOrderMapper mapper, IOutputWriter<Order> outputWriter) : base(logger)
        {
            _mapper = mapper;
            _outputWriter = outputWriter;
        }

        public async Task RunAsync(OrderHandlerOptions options)
        {
            var list = ProcessFiles(options, (orders) => true, (reader) => _mapper.Map(reader, options.Delimiter));

            _logger.LogInformation($"Writing file: {options.OutputFile.FullName}");
            await _outputWriter.OutputAsync(options, list);
        }
     }
}
