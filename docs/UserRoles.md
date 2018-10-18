# Different categories of Users

The User model has a UserRole property.  The UserRole property specifies the 4 possible roles for each user:

1. DISABLED  
   This kind of user has no permissions in the system.  It is used for users that are no longer with the company, or otherwise shouldn't have access anymore.  We do not provide for users to be deleted, so mark them as DISABLED instead.
2. READONLY  
   This kind of user may not change data in the system.  They can run reports already created by others and view data as it exists.  This User Role is to give access to an auditor, for example.
3. STANDARD  
   This kind of user has full access to the data entry and reporting features of the system.  They cannot perform administrator actions, such as modifying other users or viewing system logs.
4. ADMIN  
   This kind of user has no limits.  They have access to all of the data entry and reporting features, just like the STANDARD role.  They can also create other users, change their passwords, and change their Role, even to ADMIN.  This role can view the system logs.
