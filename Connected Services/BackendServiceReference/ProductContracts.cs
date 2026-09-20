namespace PhoneFit.BackendServiceReference
{
    [System.Runtime.Serialization.DataContractAttribute(Name = "BrandInfo", Namespace = "http://schemas.datacontract.org/2004/07/PhoneFitService")]
    public partial class BrandInfo
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int BrandID { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string BrandName { get; set; }
    }

    [System.Runtime.Serialization.DataContractAttribute(Name = "ProductAdminInfo", Namespace = "http://schemas.datacontract.org/2004/07/PhoneFitService")]
    public partial class ProductAdminInfo
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PhoneModelID { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int BrandID { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string BrandName { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ModelName { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string OperatingSystem { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ReleaseYear { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ImagePath { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsActive { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime DateAdded { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal StartingPrice { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int StockQuantity { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int VariantID { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int RAMGB { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int StorageGB { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Colour { get; set; }
    }

    [System.Runtime.Serialization.DataContractAttribute(Name = "ProductSaveInfo", Namespace = "http://schemas.datacontract.org/2004/07/PhoneFitService")]
    public partial class ProductSaveInfo
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PhoneModelID { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int BrandID { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string BrandName { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ModelName { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string OperatingSystem { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ReleaseYear { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ImagePath { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsActive { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int VariantID { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int RAMGB { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int StorageGB { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Colour { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal Price { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int StockQuantity { get; set; }
    }
}
