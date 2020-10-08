using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Domain.DomainObjects
{
    public class Options
    {
        public string WorksheetName { get; set; }
        public virtual string OutputFileName { get; internal set; }
        public FileInfo InputFile { get; set; }
        public FileInfo OutputFile { get; set; }
        public int HeaderLineNo { get; set; }
    }

    public class OrderHandlerOptions : Options
    {
        public string Delimiter { get; set; }
    }

    public class ShippingLabelHandlerOptions : Options
    {
        public FileInfo PointInTimeFile { get; set; }
    }
}
