using Domain.DomainObjects;
using Domain.Handlers;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OrderParser
{
    public class CommandLineConfig
    {
        private readonly ILogger<CommandLineConfig> _logger;
        private readonly IHandler<ShippingLabelHandler, ShippingLabelHandlerOptions> _shippingLabelHandler;
        private readonly IHandler<OrderHandler, OrderHandlerOptions> _orderHandler;

        public CommandLineConfig(
            ILogger<CommandLineConfig> logger, 
            IShippingLabelHandler shippingLabelHandler, 
            IOrderHandler orderHandler
            )
        {
            _logger = logger;
            _shippingLabelHandler = shippingLabelHandler;
            _orderHandler = orderHandler;
        }

        public CommandLineApplication Configure()
        {
            var commandLineApplication = new CommandLineApplication(throwOnUnexpectedArg: false);

            var inputOption = new CommandOption("-i | --input", CommandOptionType.SingleValue) { Description = "Input file (fullname)" };
            var outputOption = new CommandOption("-o | --output", CommandOptionType.SingleValue) { Description = "Output file (fullname)" };
            var headerLineNoOption = new CommandOption("-h | --header-line-no", CommandOptionType.SingleValue) { Description = "Num. header lines. Default = 1" };
            var worksheetNameOption = new CommandOption("-w | --ws-name", CommandOptionType.SingleValue) { Description = "Worksheet name" };
            
            var orderParserCommand = commandLineApplication.Command("OrderParser", target =>
            {
                var inputCommandOption = target.Option(inputOption.Template, inputOption.Description, inputOption.OptionType);
                var outputCommandOption = target.Option(outputOption.Template, outputOption.Description, outputOption.OptionType);
                var headerLineNoCommandOption = target.Option(headerLineNoOption.Template, headerLineNoOption.Description, headerLineNoOption.OptionType);
                var worksheetCommandOption = target.Option(worksheetNameOption.Template, worksheetNameOption.Description, worksheetNameOption.OptionType);
                var delimiterCommandOption = target.Option("-d | --delimiter", "Product[Name/Price] delimiter. Default=','", CommandOptionType.SingleValue);
              
                target.HelpOption("-? | -h | --help");

                target.OnExecute(async () => await ExecuteAsync(_orderHandler, new OrderHandlerOptions 
                {
                    InputFile = new FileInfo(inputCommandOption.Value()),
                    OutputFile = new FileInfo(outputCommandOption.Value()),
                    HeaderLineNo = headerLineNoCommandOption.HasValue() ? int.Parse(headerLineNoCommandOption.Value()) : 1,
                    WorksheetName = worksheetCommandOption.Value(),
                    Delimiter = delimiterCommandOption.HasValue() ? delimiterCommandOption.Value() : ","
                }));
            });

            var shippingLabelParserCommand = commandLineApplication.Command("ShippingLabelParser", target =>
            {
                var inputCommandOption = target.Option(inputOption.Template, inputOption.Description, inputOption.OptionType);
                var outputCommandOption = target.Option(outputOption.Template, outputOption.Description, outputOption.OptionType);
                var headerLineNoCommandOption = target.Option(headerLineNoOption.Template, headerLineNoOption.Description, headerLineNoOption.OptionType);
                var worksheetCommandOption = target.Option(worksheetNameOption.Template, worksheetNameOption.Description, worksheetNameOption.OptionType);
                var pointInTimeCommandOption = target.Option("-p | --point-in-time-file", "Point-in-time file (fullname)", CommandOptionType.SingleValue);

                target.HelpOption("-? | -h | --help");

                target.OnExecute(async () => await ExecuteAsync(_shippingLabelHandler, new ShippingLabelHandlerOptions 
                {
                    InputFile = new FileInfo(inputCommandOption.Value()),
                    OutputFile = new FileInfo(outputCommandOption.Value()),
                    HeaderLineNo = headerLineNoCommandOption.HasValue() ? int.Parse(headerLineNoCommandOption.Value()) : 1,
                    WorksheetName = worksheetCommandOption.Value(),
                    PointInTimeFile = new FileInfo(pointInTimeCommandOption.Value())
                }));
            });


            commandLineApplication.HelpOption("-? | -h | --help");
            
            return commandLineApplication;
        }

        private async Task<int> ExecuteAsync<T, K>(IHandler<T, K> handler, K options)
        {
            try
            {
                await handler.RunAsync(options);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured during processing..");

                return -1;
            }

            return 0;
        }
    }
}