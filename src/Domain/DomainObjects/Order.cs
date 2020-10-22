using System;

namespace Domain.DomainObjects
{
    public class Order
    {
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public int RecurringInvoiceActive { get; internal set; }
        public string RecurringInvoiceRepeatTimes { get; internal set; }
        public string RecurringInvoiceEndDate { get; internal set; }
        public string RecurringInvoiceSendMethod { get; internal set; }
        public string RecurringInvoiceSendFrequency { get; internal set; }
        public string RecurringInvoiceSendFrequencyUnit { get; internal set; }
        public string NextRecurringInvoiceDate { get; internal set; }
        public string SalesPersonEmployeeNo { get; internal set; }
        public string SalesPersonName { get; internal set; }
        public string ProjectCode { get; internal set; }
        public string SubprojectCode { get; internal set; }
        public string ProjectName { get; internal set; }
        public string ProjectManagerCode { get; internal set; }
        public string ProjectManagerName { get; internal set; }
        public string ProjectBillable { get; internal set; }
        public string ProjectStartDate { get; internal set; }
        public string ProjectEndDate { get; internal set; }
        public string ProjectStatus { get; internal set; }
        public string ProjectContactPerson { get; internal set; }
        public string DepartmentCode { get; internal set; }
        public string DepartmentName { get; internal set; }
        public string DepartmentManagerCode { get; internal set; }
        public string DepartmentManagerName { get; internal set; }
        public string CustomerNo { get; internal set; }
        public object ContactName { get; internal set; }
        public string ContactGroup { get; internal set; }
        public string CustomerSince { get; internal set; }
        public int IsVatFree { get; internal set; }
        public string Phone { get; internal set; }
        public string Email { get; internal set; }
        public string Web { get; internal set; }
        public string OrganizationNo { get; internal set; }
        public string MailAddress1 { get; internal set; }
        public string MailAddress2 { get; internal set; }
        public int MailPostcode { get; internal set; }
        public string MailCity { get; internal set; }
        public string MailCountry { get; internal set; }
        public string DeliveryAddress1 { get; internal set; }
        public string DeliveryAddress2 { get; internal set; }
        public string DeliveryPostcode { get; internal set; }
        public string DeliveryCity { get; internal set; }
        public string DeliveryCountry { get; internal set; }
        public string BankAccount { get; internal set; }
        public string IBAN { get; internal set; }
        public string SWIFT { get; internal set; }
        public string InvoiceDelivery { get; internal set; }
        public string ContactPersonFirstName { get; internal set; }
        public string ContactPersonLastName { get; internal set; }
        public string ContactPersonPhone { get; internal set; }
        public string ContactPersonEmail { get; internal set; }
        public string Reference { get; internal set; }
        public string PaymentTerms { get; internal set; }
        public string MergeWithPreviousOrder { get; internal set; }
        public string Currency { get; internal set; }
        public int ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string ProductGroup { get; internal set; }
        public string ProductDescription { get; internal set; }
        public string ProductType { get; internal set; }
        public string ProductUnit { get; internal set; }
        public string ProductSalesPrice { get; internal set; }
        public string ProductCostPrice { get; internal set; }
        public double ProductSalesAccount { get; internal set; }
        public string ProductSalesAccountName { get; internal set; }
        public double ProductAltSalesAccount { get; internal set; }
        public string ProductAltSalesAccountName { get; internal set; }
        public string ProductGTIN { get; internal set; }
        public double Discount { get; internal set; }
        public int Quantity { get; internal set; }
        public string Description { get; internal set; }
        public string OrderLineUnitPrice { get; internal set; }
        public string SortOrder { get; internal set; }
    }
}
