namespace PhoneFit.BackendServiceReference
{
    // Mirrors PhoneFitService.VariantInfo — flat DTO without LINQ associations.
    [System.Runtime.Serialization.DataContractAttribute(Name = "VariantInfo", Namespace = "http://schemas.datacontract.org/2004/07/PhoneFitService")]
    public partial class VariantInfo
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int VariantID { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PhoneModelID { get; set; }

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

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int LowStockLevel { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsActive { get; set; }
    }

    // Mirrors PhoneFitService.SpecificationInfo — flat DTO without PhoneModel EntityRef.
    [System.Runtime.Serialization.DataContractAttribute(Name = "SpecificationInfo", Namespace = "http://schemas.datacontract.org/2004/07/PhoneFitService")]
    public partial class SpecificationInfo
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int SpecificationID { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PhoneModelID { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Processor { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<decimal> ScreenSize { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ScreenType { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> RefreshRate { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> BatteryCapacity { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<decimal> RearCameraMP { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<decimal> FrontCameraMP { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Supports5G { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool DualSIM { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool ExpandableStorage { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string WaterResistance { get; set; }
    }
}
