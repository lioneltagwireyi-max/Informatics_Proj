USE PhoneFitDB;
GO

/*
  Ensures Admin and Customer roles exist, migrates legacy Manager → Admin,
  and seeds demo accounts so both login paths can be demonstrated.

  Demo passwords (MD5 hex, matching SecrecyHash.hashFunction):
    admin@phonefit.local    / Admin123!
    customer@phonefit.local / Customer123!
*/

-- Prefer Admin as the staff role name (schema comment: Customer or Admin/Manager).
IF EXISTS (SELECT 1 FROM [Role] WHERE RoleName = 'Manager')
   AND NOT EXISTS (SELECT 1 FROM [Role] WHERE RoleName = 'Admin')
BEGIN
    UPDATE [Role]
    SET RoleName = 'Admin',
        RoleDescription = 'Administers users, smartphone products, prices, stock, orders and business reports.'
    WHERE RoleName = 'Manager';
END
GO

IF NOT EXISTS (SELECT 1 FROM [Role] WHERE RoleName = 'Customer')
BEGIN
    INSERT INTO [Role](RoleName, RoleDescription)
    VALUES (
        'Customer',
        'Browses smartphones, receives recommendations, manages a cart and places orders.'
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM [Role] WHERE RoleName = 'Admin')
BEGIN
    INSERT INTO [Role](RoleName, RoleDescription)
    VALUES (
        'Admin',
        'Administers users, smartphone products, prices, stock, orders and business reports.'
    );
END
GO

-- Keep a Manager alias only if Admin already existed and Manager is still referenced.
-- (No-op when Manager was renamed above.)

DECLARE @AdminRoleID INT =
    (SELECT RoleID FROM [Role] WHERE RoleName = 'Admin');
DECLARE @CustomerRoleID INT =
    (SELECT RoleID FROM [Role] WHERE RoleName = 'Customer');

IF @AdminRoleID IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM UserAccount WHERE UserEmail = 'admin@phonefit.local')
BEGIN
    INSERT INTO UserAccount (
        RoleID,
        UserEmail,
        UserPasswordHash,
        UserFirstName,
        UserSurname,
        UserPhoneNumber,
        UserIsActive
    )
    VALUES (
        @AdminRoleID,
        'admin@phonefit.local',
        '2637A5C30AF69A7BAD877FDB65FBD78B', -- Admin123!
        'Demo',
        'Admin',
        NULL,
        1
    );
END

IF @CustomerRoleID IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM UserAccount WHERE UserEmail = 'customer@phonefit.local')
BEGIN
    INSERT INTO UserAccount (
        RoleID,
        UserEmail,
        UserPasswordHash,
        UserFirstName,
        UserSurname,
        UserPhoneNumber,
        UserIsActive
    )
    VALUES (
        @CustomerRoleID,
        'customer@phonefit.local',
        'EC9E050C0FBB94573950FD9167CF9BAA', -- Customer123!
        'Demo',
        'Customer',
        NULL,
        1
    );
END
GO
