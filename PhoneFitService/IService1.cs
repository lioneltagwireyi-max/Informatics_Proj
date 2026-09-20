using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PhoneFitService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        int RegisterUser(UserAccount userAccount);

        [OperationContract]
        LoginInfo LoginUser(string email, string passwordHash);

        [OperationContract]
        List<PhoneCatalogue> GetActivePhones();

        [OperationContract]
        PhoneCatalogue GetPhoneByID(int phoneModelID);

        [OperationContract]
        List<PhoneVariant> GetVariantsByPhoneID(int phoneModelID);

        [OperationContract]
        PhoneSpecification GetSpecificationByPhoneID(int phoneModelID);

        [OperationContract]
        bool AddToCart(int userID, int variantID, int quantity);

        [OperationContract]
        List<CartItemInfo> GetCartItems(int userID);

        [OperationContract]
        bool ChangeCartItemQuantity(int userID, int cartItemID, int quantityChange);

        [OperationContract]
        bool RemoveCartItem(int userID, int cartItemID);

        [OperationContract]
        bool ClearCart(int userID);

        [OperationContract]
        List<UserInfo> GetUsers();

        [OperationContract]
        OrderInvoice PlaceOrder(int userID);

        [OperationContract]
        List<OrderSummary> GetOrdersForUser(int userID);

        [OperationContract]
        OrderInvoice GetInvoice(int userID, int orderID);

        [OperationContract]
        ReportSummary GetReportSummary(DateTime fromDate, DateTime toDate);

        [OperationContract]
        List<BrandInfo> GetBrands();

        [OperationContract]
        List<ProductAdminInfo> GetAllProductsForAdmin();

        [OperationContract]
        ProductAdminInfo GetProductForAdmin(int phoneModelID);

        [OperationContract]
        int AddProduct(ProductSaveInfo product);

        [OperationContract]
        bool UpdateProduct(ProductSaveInfo product);

        [OperationContract]
        bool DeleteProduct(int phoneModelID);

        [OperationContract]
        int EnsureMinimumCatalogue(int minimumCount);
    }

    [DataContract]
    public class BrandInfo
    {
        [DataMember]
        public int BrandID { get; set; }

        [DataMember]
        public string BrandName { get; set; }
    }

    [DataContract]
    public class ProductAdminInfo
    {
        [DataMember]
        public int PhoneModelID { get; set; }

        [DataMember]
        public int BrandID { get; set; }

        [DataMember]
        public string BrandName { get; set; }

        [DataMember]
        public string ModelName { get; set; }

        [DataMember]
        public string OperatingSystem { get; set; }

        [DataMember]
        public int ReleaseYear { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string ImagePath { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public DateTime DateAdded { get; set; }

        [DataMember]
        public decimal StartingPrice { get; set; }

        [DataMember]
        public int StockQuantity { get; set; }

        [DataMember]
        public int VariantID { get; set; }

        [DataMember]
        public int RAMGB { get; set; }

        [DataMember]
        public int StorageGB { get; set; }

        [DataMember]
        public string Colour { get; set; }
    }

    [DataContract]
    public class ProductSaveInfo
    {
        [DataMember]
        public int PhoneModelID { get; set; }

        [DataMember]
        public int BrandID { get; set; }

        [DataMember]
        public string BrandName { get; set; }

        [DataMember]
        public string ModelName { get; set; }

        [DataMember]
        public string OperatingSystem { get; set; }

        [DataMember]
        public int ReleaseYear { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string ImagePath { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public int VariantID { get; set; }

        [DataMember]
        public int RAMGB { get; set; }

        [DataMember]
        public int StorageGB { get; set; }

        [DataMember]
        public string Colour { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public int StockQuantity { get; set; }
    }

    [DataContract]
    public class OrderLineInfo
    {
        [DataMember]
        public int VariantID { get; set; }

        [DataMember]
        public string ModelName { get; set; }

        [DataMember]
        public string VariantDescription { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public decimal UnitPrice { get; set; }

        [DataMember]
        public decimal LineTotal { get; set; }
    }

    [DataContract]
    public class OrderSummary
    {
        [DataMember]
        public int OrderID { get; set; }

        [DataMember]
        public DateTime OrderDate { get; set; }

        [DataMember]
        public string OrderStatus { get; set; }

        [DataMember]
        public decimal TotalAmount { get; set; }
    }

    [DataContract]
    public class OrderInvoice
    {
        [DataMember]
        public int OrderID { get; set; }

        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public DateTime OrderDate { get; set; }

        [DataMember]
        public string OrderStatus { get; set; }

        [DataMember]
        public decimal Subtotal { get; set; }

        [DataMember]
        public decimal DiscountAmount { get; set; }

        [DataMember]
        public decimal ShippingAmount { get; set; }

        [DataMember]
        public decimal TaxAmount { get; set; }

        [DataMember]
        public decimal TotalAmount { get; set; }

        [DataMember]
        public string TransactionNotes { get; set; }

        [DataMember]
        public List<OrderLineInfo> Lines { get; set; }
    }

    [DataContract]
    public class ReportSummary
    {
        [DataMember]
        public int DistinctProductsSold { get; set; }

        [DataMember]
        public int RegisteredUsersInRange { get; set; }

        [DataMember]
        public int OrdersInRange { get; set; }

        [DataMember]
        public decimal RevenueInRange { get; set; }

        [DataMember]
        public int ActiveCustomerAccounts { get; set; }

        [DataMember]
        public int TotalRegisteredUsers { get; set; }

        [DataMember]
        public List<StockOnHandInfo> StockOnHandForSoldProducts { get; set; }

        [DataMember]
        public List<UsersPerDayInfo> UsersRegisteredPerDay { get; set; }
    }

    [DataContract]
    public class StockOnHandInfo
    {
        [DataMember]
        public string ModelName { get; set; }

        [DataMember]
        public string VariantDescription { get; set; }

        [DataMember]
        public int StockQuantity { get; set; }
    }

    [DataContract]
    public class UsersPerDayInfo
    {
        [DataMember]
        public DateTime Day { get; set; }

        [DataMember]
        public int UserCount { get; set; }
    }

    [DataContract]
    public class LoginInfo
    {
        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public string RoleName { get; set; }

        [DataMember]
        public string LoginStatus { get; set; }
    }

    [DataContract]
    public class CartItemInfo
    {
        [DataMember]
        public int CartItemID { get; set; }

        [DataMember]
        public int VariantID { get; set; }

        [DataMember]
        public int PhoneModelID { get; set; }

        [DataMember]
        public string ModelName { get; set; }

        [DataMember]
        public string ImagePath { get; set; }

        [DataMember]
        public string VariantDescription { get; set; }

        [DataMember]
        public decimal UnitPrice { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public decimal LineTotal { get; set; }

        [DataMember]
        public int StockQuantity { get; set; }
    }

    // Catalogue class
    [DataContract]
    public class PhoneCatalogue
    {
        [DataMember]
        public int PhoneModelID { get; set; }

        [DataMember]
        public string BrandName { get; set; }

        [DataMember]
        public string ModelName { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string ImagePath { get; set; }

        [DataMember]
        public decimal StartingPrice { get; set; }

        [DataMember]
        public int StockQuantity { get; set; }
    }

    [DataContract]
    public class UserInfo
    {
        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string UserEmail { get; set; }

        [DataMember]
        public string UserPhoneNumber { get; set; }

        [DataMember]
        public string RoleName { get; set; }

        [DataMember]
        public bool UserIsActive { get; set; }

        [DataMember]
        public DateTime UserAccountCreated { get; set; }
    }
}
