namespace PhoneFit.BackendServiceReference
{
    [System.Runtime.Serialization.DataContractAttribute(Name = "OrderLineInfo", Namespace = "http://schemas.datacontract.org/2004/07/PhoneFitService")]
    public partial class OrderLineInfo
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int VariantID { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ModelName { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string VariantDescription { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Quantity { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal UnitPrice { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal LineTotal { get; set; }
    }

    [System.Runtime.Serialization.DataContractAttribute(Name = "OrderSummary", Namespace = "http://schemas.datacontract.org/2004/07/PhoneFitService")]
    public partial class OrderSummary
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int OrderID { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime OrderDate { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string OrderStatus { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal TotalAmount { get; set; }
    }

    [System.Runtime.Serialization.DataContractAttribute(Name = "OrderInvoice", Namespace = "http://schemas.datacontract.org/2004/07/PhoneFitService")]
    public partial class OrderInvoice
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int OrderID { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int UserID { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime OrderDate { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string OrderStatus { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal Subtotal { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal DiscountAmount { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal ShippingAmount { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal TaxAmount { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal TotalAmount { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TransactionNotes { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public OrderLineInfo[] Lines { get; set; }
    }

    [System.Runtime.Serialization.DataContractAttribute(Name = "StockOnHandInfo", Namespace = "http://schemas.datacontract.org/2004/07/PhoneFitService")]
    public partial class StockOnHandInfo
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ModelName { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string VariantDescription { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int StockQuantity { get; set; }
    }

    [System.Runtime.Serialization.DataContractAttribute(Name = "UsersPerDayInfo", Namespace = "http://schemas.datacontract.org/2004/07/PhoneFitService")]
    public partial class UsersPerDayInfo
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Day { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int UserCount { get; set; }
    }

    [System.Runtime.Serialization.DataContractAttribute(Name = "ReportSummary", Namespace = "http://schemas.datacontract.org/2004/07/PhoneFitService")]
    public partial class ReportSummary
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int DistinctProductsSold { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int RegisteredUsersInRange { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int OrdersInRange { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal RevenueInRange { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ActiveCustomerAccounts { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int TotalRegisteredUsers { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public StockOnHandInfo[] StockOnHandForSoldProducts { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public UsersPerDayInfo[] UsersRegisteredPerDay { get; set; }
    }
}
