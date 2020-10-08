using Domain.DomainObjects;
using Domain.OutputWriters;
using ExcelDataReader;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace Domain.Handlers
{
    public abstract class BaseHandler<T>
    {
        protected readonly ILogger _logger;

        public BaseHandler(ILogger logger)
        {
            _logger = logger;
        }

        public IEnumerable<T> ProcessFiles(Options options, Func<IEnumerable<T>, bool> inclusionFilterDelegate, Func<IExcelDataReader, IEnumerable<T>> mapperDelegate)
        {            
            _logger.LogInformation($"Parsing file: {options.InputFile.FullName}");

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = options.InputFile.Open(FileMode.Open))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        if (reader.Name == options.WorksheetName)
                        {
                            var list = new List<T>();
                            var rowNum = 1;

                            while(reader.Read())
                            {
                                if(rowNum > options.HeaderLineNo)
                                {
                                    var item = mapperDelegate.Invoke(reader);

                                    if(inclusionFilterDelegate.Invoke(item))
                                    {
                                        list.AddRange(item);
                                    }
                                }

                                rowNum++;
                            }

                            return list;
                        }
                    } while (reader.NextResult());
                }
            }

            return null;
        }
    }
}
