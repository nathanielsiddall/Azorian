# Azorian

Sample ASP.NET Core application.

## Role-based Permission System

This project now includes a dynamic role-based permission system:

- **Roles** are created by users with the `Admin` role.
- Users can be assigned to zero or more roles using `/api/roles/{roleId}/users/{userId}`.
- An `AuditLog` records creation, modification, deletion and assignment actions involving roles.
- Users without any assigned role are allowed and can still exist in the system.

See `Controllers/RolesController.cs` and `Controllers/UsersController.cs` for available endpoints.
