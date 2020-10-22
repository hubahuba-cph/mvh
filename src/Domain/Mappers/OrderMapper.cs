using Domain.DomainObjects;
using ExcelDataReader;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Mappers
{
    public class OrderMapper : IOrderMapper
    {
        private readonly ILogger<OrderMapper> _logger;

        public OrderMapper(ILogger<OrderMapper> logger)
        {
            _logger = logger;
        }
        public IEnumerable<Order> Map(IExcelDataReader dataReader, string delimiter)
        {
            _logger.LogInformation($"Map using OrderMapper.");

            var quantity = dataReader.GetValue(70) == null ? 1 : (int)dataReader.GetDouble(70);

            _logger.LogInformation("Order, quantity: {0}", quantity);
            if (quantity > 1)
            {
                var productNameColumnContent = dataReader.GetString(57);
                var hasDelimiters = productNameColumnContent.IndexOf(delimiter) > 0;

                _logger.LogInformation("Split productname: {0}", hasDelimiters);
                if (hasDelimiters)
                {
                    var productNames = productNameColumnContent.Split(delimiter);
                    var prices = (dataReader.GetFieldType(62) == typeof(string) ? dataReader.GetString(62) : dataReader.GetDouble(62).ToString()).Split(delimiter);

                    for (int i = 0; i < quantity; i++)
                    {
                        var result = Read(dataReader);

                        result.ProductName = productNames[i];
                        result.ProductSalesPrice = prices[i];

                        yield return result;
                    }
                }
                else
                {
                    yield return Read(dataReader);
                }
            }
            else
            {
                yield return Read(dataReader);
            }
        }

        public Order Read(IExcelDataReader dataReader) =>
            new Order
            {
                OrderNo = dataReader.GetString(0),
                OrderDate = DateTime.Parse(dataReader.GetString(1)),
                RecurringInvoiceActive = (int)dataReader.GetDouble(2),
                RecurringInvoiceRepeatTimes = string.Empty,
                RecurringInvoiceEndDate = string.Empty,
                RecurringInvoiceSendMethod = string.Empty,
                RecurringInvoiceSendFrequency = string.Empty,
                RecurringInvoiceSendFrequencyUnit = string.Empty,
                NextRecurringInvoiceDate = string.Empty,
                SalesPersonEmployeeNo = string.Empty,
                SalesPersonName = string.Empty,
                ProjectCode = string.Empty,
                SubprojectCode = string.Empty,
                ProjectName = dataReader.GetString(13),
                ProjectManagerCode = string.Empty,
                ProjectManagerName = string.Empty,
                ProjectBillable = string.Empty,
                ProjectStartDate = string.Empty,
                ProjectEndDate = string.Empty,
                ProjectStatus = string.Empty,
                ProjectContactPerson = string.Empty,
                DepartmentCode = string.Empty,
                DepartmentName = string.Empty,
                DepartmentManagerCode = string.Empty,
                DepartmentManagerName = string.Empty,
                CustomerNo = string.Empty,
                ContactName = dataReader.GetString(26),
                ContactGroup = string.Empty,
                CustomerSince = string.Empty,
                IsVatFree = (int)dataReader.GetDouble(29),
                Phone = dataReader.GetFieldType(30) == typeof(string) ? dataReader.GetString(30) : dataReader.GetDouble(30).ToString(),
                Email = dataReader.GetString(31),
                Web = dataReader.GetString(32),
                OrganizationNo = dataReader.GetString(33),
                MailAddress1 = dataReader.GetString(34),
                MailAddress2 = dataReader.GetString(35),
                MailPostcode = (int)dataReader.GetDouble(36),
                MailCity = dataReader.GetString(37),
                MailCountry = dataReader.GetString(38),
                DeliveryAddress1 = dataReader.GetString(39),
                DeliveryAddress2 = dataReader.GetString(40),
                DeliveryPostcode = dataReader.GetFieldType(41) == typeof(string) ? dataReader.GetString(41) : dataReader.GetValue(41) == null ? string.Empty : dataReader.GetValue(41).ToString(),
                DeliveryCity = dataReader.GetString(42),
                DeliveryCountry = dataReader.GetString(43),
                BankAccount = dataReader.GetString(44),
                IBAN = dataReader.GetString(45),
                SWIFT = dataReader.GetString(46),
                InvoiceDelivery = dataReader.GetString(47),
                ContactPersonFirstName = dataReader.GetString(48),
                ContactPersonLastName = dataReader.GetString(49),
                ContactPersonPhone = dataReader.GetFieldType(50) == typeof(string) ? dataReader.GetString(50) : dataReader.GetDouble(50).ToString(),
                ContactPersonEmail = dataReader.GetString(51),
                Reference = dataReader.GetString(52),
                PaymentTerms = dataReader.GetString(53),
                MergeWithPreviousOrder = dataReader.GetString(54),
                Currency = dataReader.GetString(55),
                ProductCode = (int)dataReader.GetDouble(56),
                ProductName = dataReader.GetString(57),
                ProductGroup = dataReader.GetString(58),
                ProductDescription = dataReader.GetString(59),
                ProductType = dataReader.GetString(60),
                ProductUnit = dataReader.GetString(61),
                ProductSalesPrice = dataReader.GetFieldType(62) == typeof(string) ? dataReader.GetString(62) : dataReader.GetDouble(62).ToString(),
                ProductCostPrice = dataReader.GetString(63),
                ProductSalesAccount = dataReader.GetDouble(64),
                ProductSalesAccountName = dataReader.GetString(65),
                ProductAltSalesAccount = dataReader.GetDouble(66),
                ProductAltSalesAccountName = dataReader.GetString(67),
                ProductGTIN = dataReader.GetString(68),
                Discount = dataReader.GetDouble(69),
                Quantity = dataReader.GetValue(70) == null ? 1 : (int)dataReader.GetDouble(70),
                Description = dataReader.GetString(71),
                OrderLineUnitPrice = dataReader.GetString(72),
                SortOrder = dataReader.GetString(73),
            };
    }
}