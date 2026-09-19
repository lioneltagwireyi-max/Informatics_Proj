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
