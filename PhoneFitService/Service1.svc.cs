using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PhoneFitService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        PhoneFitDataDataContext db = new PhoneFitDataDataContext();

        public LoginInfo LoginUser(string email, string passwordHash)
        {
            // Authenticate the users credentials
            var user =
                (from account in db.UserAccounts
                 where account.UserEmail == email
                 && account.UserPasswordHash == passwordHash
                 select account).SingleOrDefault();

            if (user == null)
            {
                // Case - User not found
                return new LoginInfo
                {
                    UserID = 0,
                    RoleName = null,
                    LoginStatus = "Invalid"
                };
            }

            if (user.UserIsActive == false)
            {
                // Case - User account is inactive
                return new LoginInfo
                {
                    UserID = 0,
                    RoleName = null,
                    LoginStatus = "Inactive"
                };
            }

            // Get the users role
            var userRole =
                (from role in db.Roles
                 where role.RoleID == user.RoleID
                 select role).SingleOrDefault();

            if (userRole == null)
            {
                // Case - Role not found
                return new LoginInfo
                {
                    UserID = 0,
                    RoleName = null,
                    LoginStatus = "Invalid"
                };
            }

            return new LoginInfo
            {
                // Case - User successfully authenticated
                UserID = user.UserID,
                RoleName = userRole.RoleName,
                LoginStatus = "Success"
            };
        }

        public int RegisterUser(UserAccount userAccount)
        {
           var checkEmail =
                (from user in db.UserAccounts
                 where user.UserEmail == userAccount.UserEmail
                 select user).SingleOrDefault();

            if (checkEmail == null)
            {
                var customerRole =
                    (from role in db.Roles
                     where role.RoleName == "Customer"
                     select role).SingleOrDefault();

                if (customerRole == null)
                {
                    return 1;   // Customer role was not found
                }

                var newUser = new UserAccount
                {
                    RoleID = customerRole.RoleID,
                    UserEmail = userAccount.UserEmail,
                    UserPasswordHash = userAccount.UserPasswordHash,
                    UserFirstName = userAccount.UserFirstName,
                    UserSurname = userAccount.UserSurname,
                    UserPhoneNumber = userAccount.UserPhoneNumber,
                    UserIsActive = true,
                    UserAccountCreated = DateTime.Now
                };

                db.UserAccounts.InsertOnSubmit(newUser);

                try
                {
                    db.SubmitChanges();
                    return 0;   // Account registered successfully
                }
                catch (Exception)
                {
                    return 1; // Registration or database operation failed
                }
            }
            else
            {
                return 2; // Email already exists
            }
        }

        public List<PhoneCatalogue> GetActivePhones()
        {
            List<PhoneCatalogue> phoneList = new List<PhoneCatalogue>();

            var activePhones =
                (from phone in db.PhoneModels
                 where phone.IsActive == true
                 select phone).ToList();

            foreach(PhoneModel phone in activePhones)
            {
                var brand =
                    (from b in db.Brands
                     where b.BrandID == phone.BrandID
                     select b).SingleOrDefault();

                var variant =
                    (from v in db.PhoneVariants
                     where v.PhoneModelID == phone.PhoneModelID
                     && v.IsActive == true
                     orderby v.Price ascending
                     select v).FirstOrDefault();

                PhoneCatalogue cataloguePhone = new PhoneCatalogue
                {
                    PhoneModelID = phone.PhoneModelID,
                    BrandName = brand.BrandName,
                    ModelName = phone.ModelName,
                    Description = phone.Description,
                    ImagePath = phone.ImagePath,
                    StartingPrice = variant.Price,
                    StockQuantity = variant.StockQuantity
                };

                phoneList.Add(cataloguePhone);
            }

            return phoneList;
        }

        public PhoneCatalogue GetPhoneByID(int phoneModelID)
        {
            var phone =
                (from p in db.PhoneModels
                 where p.PhoneModelID == phoneModelID
                 && p.IsActive == true
                 select p).SingleOrDefault();

            if (phone == null)
            {
                return null;
            }

            var brand =
                (from b in db.Brands
                 where b.BrandID == phone.BrandID
                 select b).SingleOrDefault();

            var variant =
                (from v in db.PhoneVariants
                 where v.PhoneModelID == phone.PhoneModelID
                 && v.IsActive == true
                 orderby v.Price ascending
                 select v).FirstOrDefault();

            PhoneCatalogue selectedPhone = new PhoneCatalogue
            {
                PhoneModelID = phone.PhoneModelID,
                BrandName = brand.BrandName,
                ModelName = phone.ModelName,
                Description = phone.Description,
                ImagePath = phone.ImagePath,
                StartingPrice = variant.Price,
                StockQuantity = variant.StockQuantity
            };

            return selectedPhone;
        }

        public List<PhoneVariant> GetVariantsByPhoneID(int phoneModelID)
        {
            var variants =
                (from variant in db.PhoneVariants
                 where variant.PhoneModelID == phoneModelID
                 && variant.IsActive == true
                 orderby variant.Price ascending
                 select variant).ToList();

            return variants;
        }

        public PhoneSpecification GetSpecificationByPhoneID(int phoneModelID)
        {
            var specification =
                (from spec in db.PhoneSpecifications
                 where spec.PhoneModelID == phoneModelID
                 select spec).SingleOrDefault();

            return specification;
        }

        public bool AddToCart(int userID, int variantID, int quantity)
        {
            // A cart item must have a positive quantity.
            if (quantity <= 0)
            {
                return false;
            }

            // Finf the user who ia trying to add the product
            var user =
                (from account in db.UserAccounts
                 where account.UserID == userID
                 && account.UserIsActive == true
                 select account).SingleOrDefault();

            // User does not exist or account is inactive
            if (user == null)
            {
                return false;
            }

            // Find the variant selected by the customer
            var variant =
                (from phoneVariant in db.PhoneVariants
                 where phoneVariant.VariantID == variantID
                 && phoneVariant.IsActive == true
                 select phoneVariant).SingleOrDefault();

            // Variant does not exist or is inactive
            if (variant == null)
            {
                return false;
            }

            // Confirm that the phone model linked to the variant is active.
            var phoneModel =
                (from phone in db.PhoneModels
                 where phone.PhoneModelID == variant.PhoneModelID
                 && phone.IsActive == true
                 select phone).SingleOrDefault();

            if (phoneModel == null)
            {
                return false;
            }

            // The requested quantity must not exceed available stock.
            if (quantity > variant.StockQuantity)
            {
                return false;
            }

            // Find the user's active cart.
            var activeCart =
                (from cart in db.Carts
                 where cart.UserID == userID
                 && cart.IsActive == true
                 select cart).SingleOrDefault();

            // Create a new cart if the user does not have an active cart.
            if (activeCart == null)
            {
                activeCart = new Cart
                {
                    UserID = userID,
                    DateCreated = DateTime.Now,
                    IsActive = true
                };

                db.Carts.InsertOnSubmit(activeCart);
            }

            // Find out whether this variant is already in the active cart.
            var existingCartItem =
                (from item in db.CartItems
                 where item.Cart == activeCart
                 && item.VariantID == variantID
                 select item).SingleOrDefault();

            if (existingCartItem == null)
            {
                // Add the variant as a new cart item.
                CartItem newCartItem = new CartItem
                {
                    Cart = activeCart,
                    VariantID = variantID,
                    Quantity = quantity,
                    DateAdded = DateTime.Now
                };

                db.CartItems.InsertOnSubmit(newCartItem);
            }
            else
            {
                // Increase the quantity if the variant is already in the cart.
                int updatedQuantity = existingCartItem.Quantity + quantity;

                if (updatedQuantity > variant.StockQuantity)
                {
                    return false;
                }

                existingCartItem.Quantity = updatedQuantity;
            }

            try
            {
                db.SubmitChanges();
                return true;
            }
            catch (Exception e)
            {
                e.GetBaseException();
                return false;
            }
        }

        public List<CartItemInfo> GetCartItems(int userID)
        {
            // empty list that will store the customers cart items
            List<CartItemInfo> cartItemList = new List<CartItemInfo>();

            // find the customers active cart
            var activeCart =
                (from cart in db.Carts
                 where cart.UserID == userID
                 && cart.IsActive == true
                 select cart).SingleOrDefault();

            if (activeCart == null)
            {
                return cartItemList;
            }

            // find all cartitems that belong to the active cart
            var cartItems = (
                from item in db.CartItems
                where item.CartID == activeCart.CartID
                select item).ToList();

            // Go through each cartItem
            foreach(CartItem item in cartItems)
            {
                // find the variant related to the item
                var variant = (
                    from phoneVariant in db.PhoneVariants
                    where phoneVariant.VariantID == item.VariantID
                    select phoneVariant).SingleOrDefault();

                // only continue if the variant exists
                if(variant != null)
                {
                    // find the phone model related to the variant
                    var phone = (
                        from phoneModel in db.PhoneModels
                        where phoneModel.PhoneModelID == variant.PhoneModelID
                        select phoneModel).SingleOrDefault();

                    if(phone != null)
                    {
                        // create the object that will be sent to the frontend
                        CartItemInfo cartItemInfo = new CartItemInfo
                        {
                            CartItemID = item.CartItemID,
                            VariantID = variant.VariantID,
                            PhoneModelID = phone.PhoneModelID,
                            ModelName = phone.ModelName,
                            ImagePath = phone.ImagePath,
                            VariantDescription = variant.RAMGB + " GB RAM · " + variant.StorageGB + " GB Storage · " + variant.Colour,
                            UnitPrice = variant.Price,
                            Quantity = item.Quantity,
                            LineTotal = variant.Price * item.Quantity,
                            StockQuantity = variant.StockQuantity
                        };

                        // add the cartiteminfo to the list
                        cartItemList.Add(cartItemInfo);
                    }
                }
            }
            
            // retuen all customer cart items to the frontend
            return cartItemList;
        }

        public bool ChangeCartItemQuantity(int userID, int cartItemID, int quantityChange)
        {
            // Quantity can only be increased by 1 or decreased by 1
            if (quantityChange != 1 && quantityChange != -1)
            {
                return false;
            }

            // Find the logged in customers active cart
            var activeCart = (
                from cart in db.Carts
                where cart.UserID == userID &&
                cart.IsActive == true
                select cart).SingleOrDefault();

            // Customer does not have an active cart
            if (activeCart == null)
            {
                return false;
            }

            // Find the selected item inside the customers active cart
            var cartItem = (
                from item in db.CartItems
                where item.CartID == activeCart.CartID &&
                item.CartItemID == cartItemID
                select item).SingleOrDefault();

            // Item does not exist inside the cart
            if(cartItem == null)
            {
                return false;
            }

            // Find the phone variant linked to the cart item
            var variant =
                (from phoneVariant in db.PhoneVariants
                 where phoneVariant.VariantID == cartItem.VariantID
                 && phoneVariant.IsActive == true
                 select phoneVariant).SingleOrDefault();

            // variant does not exist or is inactive
            if (variant == null)
            {
                return false;
            }

            // Calculate new quantity
            int newQuantity = cartItem.Quantity + quantityChange;

            // Remove the item if its quantity reaches zero.
            if (newQuantity <= 0)
            {
                db.CartItems.DeleteOnSubmit(cartItem);
            }
            else
            {
                // Do not allow the cart quantity to exceed stock.
                if (newQuantity > variant.StockQuantity)
                {
                    return false;
                }

                // Update the quantity
                cartItem.Quantity = newQuantity;
            }

            try
            {
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveCartItem(int userID, int cartItemID)
        {
            // find the logged in customers active cart
            var activeCart = (
                 from cart in db.Carts
                 where cart.UserID == userID &&
                 cart.IsActive == true
                 select cart).SingleOrDefault();

            // customer does not have an active cart
            if(activeCart == null)
            {
                return false;
            }

            // find the selected item inside the customers active cart
            var cartItem = (
                from item in db.CartItems
                where item.CartID == activeCart.CartID &&
                item.CartItemID == cartItemID
                select item).SingleOrDefault();

            // item not found in customer cart
            if(cartItem == null)
            {
                return false;
            }

            db.CartItems.DeleteOnSubmit(cartItem);

            try
            {
                db.SubmitChanges();
                return true;
            }
            catch(Exception e)
            {
                e.GetBaseException();
                return false;
            }
        }

        public bool ClearCart(int userID)
        {
            // find the logged in customers active cart
            var activeCart =
                (from cart in db.Carts
                 where cart.UserID == userID
                 && cart.IsActive == true
                 select cart).SingleOrDefault();

            // customer has no active cart
            if (activeCart == null)
            {
                return false;
            }

            // find all the items inside the cart
            var cartItems =
                (from item in db.CartItems
                 where item.CartID == activeCart.CartID
                 select item).ToList();

            // cart is already empty
            if (cartItems.Count == 0)
            {
                return true;
            }

            db.CartItems.DeleteAllOnSubmit(cartItems);

            try
            {
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<UserInfo> GetUsers()
        {
            var users =
                (from account in db.UserAccounts
                 join role in db.Roles
                     on account.RoleID equals role.RoleID
                 orderby account.UserAccountCreated descending
                 select new UserInfo
                 {
                     UserID = account.UserID,

                     FullName =
                         account.UserFirstName + " " +
                         account.UserSurname,

                     UserEmail = account.UserEmail,

                     UserPhoneNumber =
                         account.UserPhoneNumber,

                     RoleName = role.RoleName,

                     UserIsActive =
                         account.UserIsActive,

                     UserAccountCreated =
                         account.UserAccountCreated
                 }).ToList();

            return users;
        }

    }
}
