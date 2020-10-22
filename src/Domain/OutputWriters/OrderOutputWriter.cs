using CsvHelper;
using CsvHelper.Configuration;
using Domain.DomainObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.OutputWriters
{
    public class OrderOutputWriter : IOutputWriter<Order>
    {
        private readonly ILogger<OrderOutputWriter> _logger;
        private readonly Factory _csvHelperFactory;
        private readonly CsvConfiguration _csvConfiguration;

        public OrderOutputWriter(ILogger<OrderOutputWriter> logger, Factory csvHelperFactory, CsvConfiguration csvConfiguration)
        {
            _logger = logger;
            _csvHelperFactory = csvHelperFactory;
            _csvConfiguration = csvConfiguration;
        }

        public async Task OutputAsync(Options options, IEnumerable<Order> list)
        {
            var orderHandlerOptions = options as OrderHandlerOptions;

            orderHandlerOptions.OutputFile.Directory.Create();
            using (var sw = new StreamWriter(options.OutputFile.FullName))
            {
                using (var cw = _csvHelperFactory.CreateWriter(sw, _csvConfiguration))
                {
                    var distinctOrders = list.Select(i => i.OrderNo).Distinct();
                    
                    foreach (var order in distinctOrders)
                    {
                        _logger.LogInformation($"Writing order: {order} to file");
                        var currentOrder = list.Where(i => i.OrderNo == order);
                        var isFirst = true;

                        foreach (var record in currentOrder)
                        {
                            if (isFirst)
                            {
                                var obj = currentOrder.First();
                                cw.WriteRecord(new Order
                                {
                                    OrderNo = obj.OrderNo,
                                    OrderDate = obj.OrderDate,
                                    RecurringInvoiceActive = obj.RecurringInvoiceActive,
                                    RecurringInvoiceRepeatTimes = obj.RecurringInvoiceRepeatTimes,
                                    RecurringInvoiceEndDate = obj.RecurringInvoiceEndDate,
                                    RecurringInvoiceSendMethod = obj.RecurringInvoiceSendMethod,
                                    RecurringInvoiceSendFrequency = obj.RecurringInvoiceSendFrequency,
                                    RecurringInvoiceSendFrequencyUnit = obj.RecurringInvoiceSendFrequencyUnit,
                                    NextRecurringInvoiceDate = obj.NextRecurringInvoiceDate,
                                    SalesPersonEmployeeNo = obj.SalesPersonEmployeeNo,
                                    SalesPersonName = obj.SalesPersonName,
                                    ProjectCode = obj.ProjectCode,
                                    SubprojectCode = obj.SubprojectCode,
                                    ProjectName = obj.ProjectName,
                                    ProjectManagerCode = obj.ProjectManagerCode,
                                    ProjectManagerName = obj.ProjectManagerName,
                                    ProjectBillable = obj.ProjectBillable,
                                    ProjectStartDate = obj.ProjectStartDate,
                                    ProjectEndDate = obj.ProjectEndDate,
                                    ProjectStatus = obj.ProjectStatus,
                                    ProjectContactPerson = obj.ProjectContactPerson,
                                    DepartmentCode = obj.DepartmentCode,
                                    DepartmentName = obj.DepartmentName,
                                    DepartmentManagerCode = obj.DepartmentManagerCode,
                                    DepartmentManagerName = obj.DepartmentManagerName,
                                    CustomerNo = obj.CustomerNo,
                                    ContactName = obj.ContactName,
                                    ContactGroup = obj.ContactGroup,
                                    CustomerSince = obj.CustomerSince,
                                    IsVatFree = obj.IsVatFree,
                                    Phone = obj.Phone,
                                    Email = obj.Email,
                                    Web = obj.Web,
                                    OrganizationNo = obj.OrganizationNo,
                                    MailAddress1 = obj.MailAddress1,
                                    MailAddress2 = obj.MailAddress2,
                                    MailPostcode = obj.MailPostcode,
                                    MailCity = obj.MailCity,
                                    MailCountry = obj.MailCountry,
                                    DeliveryAddress1 = obj.DeliveryAddress1,
                                    DeliveryAddress2 = obj.DeliveryAddress2,
                                    DeliveryPostcode = obj.DeliveryPostcode,
                                    DeliveryCity = obj.DeliveryCity,
                                    DeliveryCountry = obj.DeliveryCountry,
                                    BankAccount = obj.BankAccount,
                                    IBAN = obj.IBAN,
                                    SWIFT = obj.SWIFT,
                                    InvoiceDelivery = obj.InvoiceDelivery,
                                    ContactPersonFirstName = obj.ContactPersonFirstName,
                                    ContactPersonLastName = obj.ContactPersonLastName,
                                    ContactPersonPhone = obj.ContactPersonPhone,
                                    ContactPersonEmail = obj.ContactPersonEmail,
                                    Reference = obj.Reference,
                                    PaymentTerms = obj.PaymentTerms,
                                    MergeWithPreviousOrder = obj.MergeWithPreviousOrder,
                                    Currency = obj.Currency
                                });
                                await cw.NextRecordAsync();
                            }

                            cw.WriteRecord(new Order
                            {
                                OrderNo = record.OrderNo,
                                OrderDate = record.OrderDate,
                                ProductCode = record.ProductCode,
                                ProductName = record.ProductName,
                                ProductGroup = record.ProductGroup,
                                ProductDescription = record.ProductDescription,
                                ProductType = record.ProductType,
                                ProductUnit = record.ProductUnit,
                                ProductSalesPrice = record.ProductSalesPrice,
                                ProductCostPrice = record.ProductCostPrice,
                                ProductSalesAccount = record.ProductSalesAccount,
                                ProductSalesAccountName = record.ProductSalesAccountName,
                                ProductAltSalesAccount = record.ProductAltSalesAccount,
                                ProductAltSalesAccountName = record.ProductAltSalesAccountName,
                                ProductGTIN = record.ProductGTIN,
                                Discount = record.Discount,
                                Quantity = record.Quantity,
                                Description = record.Description,
                                OrderLineUnitPrice = record.OrderLineUnitPrice,
                                SortOrder = record.SortOrder
                            });
                            await cw.NextRecordAsync();

                            isFirst = false;
                        }
                    }
                }
            }
        }
    }
}
