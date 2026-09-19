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

        public OrderInvoice PlaceOrder(int userID)
        {
            var user =
                (from account in db.UserAccounts
                 where account.UserID == userID
                 && account.UserIsActive == true
                 select account).SingleOrDefault();

            if (user == null)
            {
                return null;
            }

            var cart =
                (from c in db.Carts
                 where c.UserID == userID
                 && c.IsActive == true
                 select c).SingleOrDefault();

            if (cart == null)
            {
                return null;
            }

            var cartItems =
                (from item in db.CartItems
                 where item.CartID == cart.CartID
                 select item).ToList();

            if (cartItems.Count == 0)
            {
                return null;
            }

            decimal subtotal = 0;
            var lines = new List<OrderLineInfo>();
            var notes = new List<string>();

            foreach (var item in cartItems)
            {
                var variant =
                    (from v in db.PhoneVariants
                     where v.VariantID == item.VariantID
                     && v.IsActive == true
                     select v).SingleOrDefault();

                if (variant == null || variant.StockQuantity < item.Quantity)
                {
                    return null;
                }

                var model =
                    (from m in db.PhoneModels
                     where m.PhoneModelID == variant.PhoneModelID
                     select m).SingleOrDefault();

                decimal lineTotal = variant.Price * item.Quantity;
                subtotal += lineTotal;

                lines.Add(new OrderLineInfo
                {
                    VariantID = variant.VariantID,
                    ModelName = model != null ? model.ModelName : "Phone",
                    VariantDescription =
                        variant.RAMGB + "GB / " + variant.StorageGB + "GB / " + variant.Colour,
                    Quantity = item.Quantity,
                    UnitPrice = variant.Price,
                    LineTotal = lineTotal
                });
            }

            // Transaction rule 1: loyalty discount (5%) if customer has prior orders.
            bool hasHistory =
                (from o in db.CustomerOrders
                 where o.UserID == userID
                 select o).Any();

            decimal discountAmount = 0;
            if (hasHistory)
            {
                discountAmount = Math.Round(subtotal * 0.05m, 2);
                notes.Add("Loyalty/history incentive: 5% discount applied.");
            }
            else
            {
                notes.Add("Loyalty/history incentive: not applied (first order).");
            }

            decimal amountAfterDiscount = subtotal - discountAmount;

            // Transaction rule 2: free shipping over R1000, else flat R99.
            decimal shippingAmount = amountAfterDiscount >= 1000m ? 0m : 99m;
            notes.Add(shippingAmount == 0m
                ? "Free shipping: order qualifies (R1 000+ after discount)."
                : "Shipping: R99 flat rate (under free-shipping threshold).");

            // Transaction rule 3: VAT / tax at 15% on goods after discount (not shipping).
            decimal taxAmount = Math.Round(amountAfterDiscount * 0.15m, 2);
            notes.Add("Tax/VAT: 15% applied to discounted merchandise subtotal.");

            decimal totalAmount = amountAfterDiscount + shippingAmount + taxAmount;

            var order = new CustomerOrder
            {
                UserID = userID,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                OrderStatus = "Received"
            };

            db.CustomerOrders.InsertOnSubmit(order);
            db.SubmitChanges();

            foreach (var item in cartItems)
            {
                var variant =
                    (from v in db.PhoneVariants
                     where v.VariantID == item.VariantID
                     select v).SingleOrDefault();

                db.OrderItems.InsertOnSubmit(new OrderItem
                {
                    OrderID = order.OrderID,
                    VariantID = item.VariantID,
                    Quantity = item.Quantity,
                    UnitPrice = variant.Price
                });

                variant.StockQuantity -= item.Quantity;
            }

            db.CartItems.DeleteAllOnSubmit(cartItems);
            cart.IsActive = false;
            db.SubmitChanges();

            return new OrderInvoice
            {
                OrderID = order.OrderID,
                UserID = userID,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus,
                Subtotal = subtotal,
                DiscountAmount = discountAmount,
                ShippingAmount = shippingAmount,
                TaxAmount = taxAmount,
                TotalAmount = totalAmount,
                TransactionNotes = string.Join(" ", notes),
                Lines = lines
            };
        }

        public List<OrderSummary> GetOrdersForUser(int userID)
        {
            return
                (from o in db.CustomerOrders
                 where o.UserID == userID
                 orderby o.OrderDate descending
                 select new OrderSummary
                 {
                     OrderID = o.OrderID,
                     OrderDate = o.OrderDate,
                     OrderStatus = o.OrderStatus,
                     TotalAmount = o.TotalAmount
                 }).ToList();
        }

        public OrderInvoice GetInvoice(int userID, int orderID)
        {
            var order =
                (from o in db.CustomerOrders
                 where o.OrderID == orderID
                 && o.UserID == userID
                 select o).SingleOrDefault();

            if (order == null)
            {
                return null;
            }

            var lines =
                (from oi in db.OrderItems
                 join v in db.PhoneVariants on oi.VariantID equals v.VariantID
                 join m in db.PhoneModels on v.PhoneModelID equals m.PhoneModelID
                 where oi.OrderID == orderID
                 select new OrderLineInfo
                 {
                     VariantID = oi.VariantID,
                     ModelName = m.ModelName,
                     VariantDescription =
                         v.RAMGB + "GB / " + v.StorageGB + "GB / " + v.Colour,
                     Quantity = oi.Quantity,
                     UnitPrice = oi.UnitPrice,
                     LineTotal = oi.UnitPrice * oi.Quantity
                 }).ToList();

            decimal subtotal = lines.Sum(l => l.LineTotal);
            // Reconstruct display rules from stored total where possible is hard;
            // show merchandise subtotal and stored total with explanatory notes.
            return new OrderInvoice
            {
                OrderID = order.OrderID,
                UserID = order.UserID,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus,
                Subtotal = subtotal,
                DiscountAmount = 0,
                ShippingAmount = 0,
                TaxAmount = 0,
                TotalAmount = order.TotalAmount,
                TransactionNotes =
                    "Invoice total includes tax, shipping and any discounts applied at checkout.",
                Lines = lines
            };
        }

        public ReportSummary GetReportSummary(DateTime fromDate, DateTime toDate)
        {
            DateTime from = fromDate == DateTime.MinValue
                ? DateTime.Now.AddDays(-30).Date
                : fromDate.Date;
            DateTime toExclusive = toDate == DateTime.MinValue
                ? DateTime.Now.Date.AddDays(1)
                : toDate.Date.AddDays(1);

            var ordersInRange =
                (from o in db.CustomerOrders
                 where o.OrderDate >= from && o.OrderDate < toExclusive
                 select o).ToList();

            var orderIds = ordersInRange.Select(o => o.OrderID).ToList();

            var soldVariantIds =
                (from oi in db.OrderItems
                 where orderIds.Contains(oi.OrderID)
                 select oi.VariantID).Distinct().ToList();

            var stock =
                (from v in db.PhoneVariants
                 join m in db.PhoneModels on v.PhoneModelID equals m.PhoneModelID
                 where soldVariantIds.Contains(v.VariantID)
                 select new StockOnHandInfo
                 {
                     ModelName = m.ModelName,
                     VariantDescription =
                         v.RAMGB + "GB / " + v.StorageGB + "GB / " + v.Colour,
                     StockQuantity = v.StockQuantity
                 }).ToList();

            var usersPerDay =
                (from u in db.UserAccounts
                 where u.UserAccountCreated >= from && u.UserAccountCreated < toExclusive
                 group u by u.UserAccountCreated.Date into g
                 orderby g.Key
                 select new UsersPerDayInfo
                 {
                     Day = g.Key,
                     UserCount = g.Count()
                 }).ToList();

            int customerRoleId =
                (from r in db.Roles
                 where r.RoleName == "Customer"
                 select r.RoleID).FirstOrDefault();

            return new ReportSummary
            {
                DistinctProductsSold = soldVariantIds.Count,
                RegisteredUsersInRange =
                    (from u in db.UserAccounts
                     where u.UserAccountCreated >= from && u.UserAccountCreated < toExclusive
                     select u).Count(),
                OrdersInRange = ordersInRange.Count,
                RevenueInRange = ordersInRange.Sum(o => o.TotalAmount),
                ActiveCustomerAccounts =
                    (from u in db.UserAccounts
                     where u.RoleID == customerRoleId && u.UserIsActive
                     select u).Count(),
                TotalRegisteredUsers = db.UserAccounts.Count(),
                StockOnHandForSoldProducts = stock,
                UsersRegisteredPerDay = usersPerDay
            };
        }

        public List<BrandInfo> GetBrands()
        {
            return
                (from b in db.Brands
                 where b.BrandIsActive == true
                 orderby b.BrandName
                 select new BrandInfo
                 {
                     BrandID = b.BrandID,
                     BrandName = b.BrandName
                 }).ToList();
        }

        public List<ProductAdminInfo> GetAllProductsForAdmin()
        {
            var phones =
                (from phone in db.PhoneModels
                 join brand in db.Brands on phone.BrandID equals brand.BrandID
                 orderby phone.ModelName
                 select new { phone, brand }).ToList();

            var result = new List<ProductAdminInfo>();
            foreach (var row in phones)
            {
                result.Add(ToProductAdminInfo(row.phone, row.brand.BrandName));
            }
            return result;
        }

        public ProductAdminInfo GetProductForAdmin(int phoneModelID)
        {
            var row =
                (from phone in db.PhoneModels
                 join brand in db.Brands on phone.BrandID equals brand.BrandID
                 where phone.PhoneModelID == phoneModelID
                 select new { phone, brand }).SingleOrDefault();

            if (row == null)
            {
                return null;
            }

            return ToProductAdminInfo(row.phone, row.brand.BrandName);
        }

        public int AddProduct(ProductSaveInfo product)
        {
            if (product == null
                || string.IsNullOrWhiteSpace(product.ModelName)
                || string.IsNullOrWhiteSpace(product.OperatingSystem)
                || string.IsNullOrWhiteSpace(product.Description))
            {
                return 0;
            }

            int brandId = ResolveBrandId(product.BrandID, product.BrandName);
            if (brandId <= 0)
            {
                return 0;
            }

            string imagePath = string.IsNullOrWhiteSpace(product.ImagePath)
                ? "assets/img/phones/placeholder.png"
                : product.ImagePath.Trim();

            var model = new PhoneModel
            {
                BrandID = brandId,
                ModelName = product.ModelName.Trim(),
                OperatingSystem = product.OperatingSystem.Trim(),
                ReleaseYear = product.ReleaseYear > 0 ? product.ReleaseYear : (int?)null,
                Description = product.Description.Trim(),
                ImagePath = imagePath,
                IsActive = product.IsActive,
                DateAdded = DateTime.Now
            };

            db.PhoneModels.InsertOnSubmit(model);
            db.SubmitChanges();

            var variant = new PhoneVariant
            {
                PhoneModelID = model.PhoneModelID,
                RAMGB = product.RAMGB > 0 ? product.RAMGB : 8,
                StorageGB = product.StorageGB > 0 ? product.StorageGB : 128,
                Colour = string.IsNullOrWhiteSpace(product.Colour) ? "Black" : product.Colour.Trim(),
                Price = product.Price > 0 ? product.Price : 4999m,
                StockQuantity = product.StockQuantity >= 0 ? product.StockQuantity : 10,
                LowStockLevel = 5,
                IsActive = product.IsActive
            };

            db.PhoneVariants.InsertOnSubmit(variant);
            db.SubmitChanges();

            return model.PhoneModelID;
        }

        public bool UpdateProduct(ProductSaveInfo product)
        {
            if (product == null || product.PhoneModelID <= 0
                || string.IsNullOrWhiteSpace(product.ModelName)
                || string.IsNullOrWhiteSpace(product.OperatingSystem)
                || string.IsNullOrWhiteSpace(product.Description))
            {
                return false;
            }

            var model =
                (from p in db.PhoneModels
                 where p.PhoneModelID == product.PhoneModelID
                 select p).SingleOrDefault();

            if (model == null)
            {
                return false;
            }

            int brandId = ResolveBrandId(product.BrandID, product.BrandName);
            if (brandId <= 0)
            {
                return false;
            }

            model.BrandID = brandId;
            model.ModelName = product.ModelName.Trim();
            model.OperatingSystem = product.OperatingSystem.Trim();
            model.ReleaseYear = product.ReleaseYear > 0 ? product.ReleaseYear : (int?)null;
            model.Description = product.Description.Trim();
            model.ImagePath = string.IsNullOrWhiteSpace(product.ImagePath)
                ? model.ImagePath
                : product.ImagePath.Trim();
            model.IsActive = product.IsActive;

            PhoneVariant variant = null;
            if (product.VariantID > 0)
            {
                variant =
                    (from v in db.PhoneVariants
                     where v.VariantID == product.VariantID
                     && v.PhoneModelID == product.PhoneModelID
                     select v).SingleOrDefault();
            }

            if (variant == null)
            {
                variant =
                    (from v in db.PhoneVariants
                     where v.PhoneModelID == product.PhoneModelID
                     orderby v.VariantID
                     select v).FirstOrDefault();
            }

            if (variant == null)
            {
                variant = new PhoneVariant
                {
                    PhoneModelID = model.PhoneModelID,
                    LowStockLevel = 5
                };
                db.PhoneVariants.InsertOnSubmit(variant);
            }

            variant.RAMGB = product.RAMGB > 0 ? product.RAMGB : variant.RAMGB;
            variant.StorageGB = product.StorageGB > 0 ? product.StorageGB : variant.StorageGB;
            variant.Colour = string.IsNullOrWhiteSpace(product.Colour) ? variant.Colour : product.Colour.Trim();
            if (string.IsNullOrWhiteSpace(variant.Colour))
            {
                variant.Colour = "Black";
            }
            variant.Price = product.Price >= 0 ? product.Price : variant.Price;
            variant.StockQuantity = product.StockQuantity >= 0 ? product.StockQuantity : variant.StockQuantity;
            variant.IsActive = product.IsActive;

            db.SubmitChanges();
            return true;
        }

        public bool DeleteProduct(int phoneModelID)
        {
            var model =
                (from p in db.PhoneModels
                 where p.PhoneModelID == phoneModelID
                 select p).SingleOrDefault();

            if (model == null)
            {
                return false;
            }

            // Soft-delete so order history / FK rows stay intact.
            model.IsActive = false;

            var variants =
                (from v in db.PhoneVariants
                 where v.PhoneModelID == phoneModelID
                 select v).ToList();

            foreach (var variant in variants)
            {
                variant.IsActive = false;
            }

            db.SubmitChanges();
            return true;
        }

        public int EnsureMinimumCatalogue(int minimumCount)
        {
            int target = minimumCount < 20 ? 20 : minimumCount;
            int current = db.PhoneModels.Count();
            if (current >= target)
            {
                return current;
            }

            EnsureSeedBrands();

            var brandIds =
                (from b in db.Brands
                 where b.BrandIsActive
                 select new { b.BrandID, b.BrandName }).ToList();

            Func<string, int> brandIdOf = name =>
            {
                var match = brandIds.FirstOrDefault(b =>
                    string.Equals(b.BrandName, name, StringComparison.OrdinalIgnoreCase));
                return match != null ? match.BrandID : brandIds[0].BrandID;
            };

            var seed = new[]
            {
                new { Brand = "Apple", Model = "iPhone 13", OS = "iOS", Year = 2021, Price = 12999m, Desc = "Reliable Apple flagship with strong cameras and long software support." },
                new { Brand = "Apple", Model = "iPhone 14", OS = "iOS", Year = 2022, Price = 14999m, Desc = "Improved durability and crash detection in a familiar iPhone design." },
                new { Brand = "Apple", Model = "iPhone 15", OS = "iOS", Year = 2023, Price = 17999m, Desc = "USB-C iPhone with Dynamic Island and brighter display." },
                new { Brand = "Apple", Model = "iPhone SE (2022)", OS = "iOS", Year = 2022, Price = 7999m, Desc = "Compact Touch ID iPhone with modern A-series performance." },
                new { Brand = "Samsung", Model = "Galaxy S23", OS = "Android", Year = 2023, Price = 15999m, Desc = "Compact Samsung flagship with excellent cameras." },
                new { Brand = "Samsung", Model = "Galaxy S24", OS = "Android", Year = 2024, Price = 17999m, Desc = "AI-assisted Galaxy flagship with bright AMOLED display." },
                new { Brand = "Samsung", Model = "Galaxy A54", OS = "Android", Year = 2023, Price = 6999m, Desc = "Mid-range Galaxy with solid battery life and clean software." },
                new { Brand = "Samsung", Model = "Galaxy A35", OS = "Android", Year = 2024, Price = 5999m, Desc = "Affordable Samsung all-rounder for everyday use." },
                new { Brand = "Google", Model = "Pixel 7", OS = "Android", Year = 2022, Price = 9999m, Desc = "Google camera phone with clean Pixel software." },
                new { Brand = "Google", Model = "Pixel 8", OS = "Android", Year = 2023, Price = 12999m, Desc = "Tensor-powered Pixel with strong computational photography." },
                new { Brand = "Google", Model = "Pixel 8a", OS = "Android", Year = 2024, Price = 8999m, Desc = "Value Pixel with flagship camera features." },
                new { Brand = "Xiaomi", Model = "Redmi Note 13", OS = "Android", Year = 2024, Price = 4499m, Desc = "Budget Xiaomi phone with high refresh display." },
                new { Brand = "Xiaomi", Model = "Xiaomi 14", OS = "Android", Year = 2024, Price = 13999m, Desc = "Leica-tuned Xiaomi flagship with fast charging." },
                new { Brand = "Huawei", Model = "Pura 70", OS = "HarmonyOS", Year = 2024, Price = 16999m, Desc = "Huawei imaging phone with premium build." },
                new { Brand = "OnePlus", Model = "Nord 3", OS = "Android", Year = 2023, Price = 7499m, Desc = "Fast-charging mid-range OnePlus with smooth OxygenOS feel." },
                new { Brand = "OnePlus", Model = "12R", OS = "Android", Year = 2024, Price = 11999m, Desc = "Performance-focused OnePlus with bright display." },
                new { Brand = "Motorola", Model = "Edge 40", OS = "Android", Year = 2023, Price = 8999m, Desc = "Slim Motorola with clean near-stock Android." },
                new { Brand = "Nokia", Model = "G60", OS = "Android", Year = 2022, Price = 3999m, Desc = "Durable Nokia with long software update promise." },
                new { Brand = "Sony", Model = "Xperia 5 V", OS = "Android", Year = 2023, Price = 15999m, Desc = "Compact cinema-oriented Sony smartphone." },
                new { Brand = "Oppo", Model = "Reno 11", OS = "Android", Year = 2024, Price = 8499m, Desc = "Stylish Oppo with strong selfie camera focus." },
                new { Brand = "Samsung", Model = "Galaxy Z Flip5", OS = "Android", Year = 2023, Price = 21999m, Desc = "Foldable Galaxy with large cover screen." },
                new { Brand = "Apple", Model = "iPhone 15 Pro", OS = "iOS", Year = 2023, Price = 24999m, Desc = "Titanium Pro iPhone with Action button and USB-C." }
            };

            int added = 0;
            foreach (var item in seed)
            {
                bool exists =
                    (from p in db.PhoneModels
                     where p.ModelName == item.Model
                     select p).Any();

                if (exists)
                {
                    continue;
                }

                if (db.PhoneModels.Count() >= target)
                {
                    break;
                }

                var model = new PhoneModel
                {
                    BrandID = brandIdOf(item.Brand),
                    ModelName = item.Model,
                    OperatingSystem = item.OS,
                    ReleaseYear = item.Year,
                    Description = item.Desc,
                    ImagePath = "assets/img/phones/placeholder.png",
                    IsActive = true,
                    DateAdded = DateTime.Now
                };
                db.PhoneModels.InsertOnSubmit(model);
                db.SubmitChanges();

                db.PhoneVariants.InsertOnSubmit(new PhoneVariant
                {
                    PhoneModelID = model.PhoneModelID,
                    RAMGB = 8,
                    StorageGB = 128,
                    Colour = "Black",
                    Price = item.Price,
                    StockQuantity = 25,
                    LowStockLevel = 5,
                    IsActive = true
                });
                db.SubmitChanges();
                added++;
            }

            return db.PhoneModels.Count();
        }

        private void EnsureSeedBrands()
        {
            string[] names =
            {
                "Apple", "Samsung", "Google", "Xiaomi", "Huawei",
                "OnePlus", "Motorola", "Nokia", "Sony", "Oppo"
            };

            foreach (string name in names)
            {
                bool exists =
                    (from b in db.Brands
                     where b.BrandName == name
                     select b).Any();

                if (!exists)
                {
                    db.Brands.InsertOnSubmit(new Brand
                    {
                        BrandName = name,
                        BrandDescription = name + " smartphones",
                        BrandIsActive = true
                    });
                }
            }

            db.SubmitChanges();
        }

        private int ResolveBrandId(int brandId, string brandName)
        {
            if (brandId > 0)
            {
                var existing =
                    (from b in db.Brands
                     where b.BrandID == brandId
                     select b).SingleOrDefault();
                if (existing != null)
                {
                    return existing.BrandID;
                }
            }

            if (string.IsNullOrWhiteSpace(brandName))
            {
                return 0;
            }

            string name = brandName.Trim();
            var byName =
                (from b in db.Brands
                 where b.BrandName == name
                 select b).SingleOrDefault();

            if (byName != null)
            {
                return byName.BrandID;
            }

            var created = new Brand
            {
                BrandName = name,
                BrandDescription = name + " smartphones",
                BrandIsActive = true
            };
            db.Brands.InsertOnSubmit(created);
            db.SubmitChanges();
            return created.BrandID;
        }

        private ProductAdminInfo ToProductAdminInfo(PhoneModel phone, string brandName)
        {
            var variant =
                (from v in db.PhoneVariants
                 where v.PhoneModelID == phone.PhoneModelID
                 orderby v.IsActive descending, v.Price ascending
                 select v).FirstOrDefault();

            int stock =
                (from v in db.PhoneVariants
                 where v.PhoneModelID == phone.PhoneModelID && v.IsActive
                 select (int?)v.StockQuantity).Sum() ?? 0;

            return new ProductAdminInfo
            {
                PhoneModelID = phone.PhoneModelID,
                BrandID = phone.BrandID,
                BrandName = brandName,
                ModelName = phone.ModelName,
                OperatingSystem = phone.OperatingSystem,
                ReleaseYear = phone.ReleaseYear ?? 0,
                Description = phone.Description,
                ImagePath = phone.ImagePath,
                IsActive = phone.IsActive,
                DateAdded = phone.DateAdded,
                StartingPrice = variant != null ? variant.Price : 0,
                StockQuantity = stock,
                VariantID = variant != null ? variant.VariantID : 0,
                RAMGB = variant != null ? variant.RAMGB : 8,
                StorageGB = variant != null ? variant.StorageGB : 128,
                Colour = variant != null ? variant.Colour : "Black"
            };
        }
    }
}
