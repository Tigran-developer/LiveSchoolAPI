# Authorization System Documentation

## Overview
This project now includes a comprehensive authorization system that allows you to control access to endpoints based on user roles and permissions.

## Available Authorization Attributes

### 1. RequirePermissionAttribute
The most flexible attribute that checks both roles and user properties.

```csharp
[RequirePermission(allowedRoles: new[] { "Admin", "Teacher" }, requireAdmin: false, requireTeacher: true, requireStudent: false)]
```

**Parameters:**
- `allowedRoles`: Array of role names that are allowed
- `requireAdmin`: Whether the user must have `isAdmin = true`
- `requireTeacher`: Whether the user must have `isTeacher = true`
- `requireStudent`: Whether the user must have `isStudent = true`

### 2. PermissionAttribute
Resource-based permission checking with action levels.

```csharp
[Permission("Classes", "Read")]     // Read access to Classes resource
[Permission("Pupils", "Write")]     // Write access to Pupils resource
[Permission("Users", "Manage")]     // Full management access to Users resource
```

**Parameters:**
- `resource`: The resource being accessed (Classes, Pupils, Teachers, Lessons, Users, System)
- `action`: The action level (Read, Write, Delete, Manage)
- `allowedRoles`: Optional array of specific roles

### 3. RequireRoleAttribute
Simple role-based authorization.

```csharp
[RequireRole("Admin")]
[RequireRole("Admin", "Teacher")]
```

## Permission Matrix

| Resource | Admin | Teacher | Pupil |
|----------|-------|---------|-------|
| Classes  | Full Access | Read/Write | Read Only |
| Pupils   | Full Access | Read Only | Read Own |
| Teachers | Full Access | Read Only | No Access |
| Lessons  | Full Access | Full Access | Read Only |
| Users    | Full Access | No Access | No Access |
| System   | Full Access | No Access | No Access |

## Usage Examples

### Controller Level Authorization
```csharp
[Route("api/[controller]")]
[ApiController]
[RequireRole("Admin")] // All methods in this controller require Admin role
public class AdminController : ControllerBase
{
    // All methods inherit Admin requirement
}
```

### Method Level Authorization
```csharp
[HttpGet]
[Permission("Classes", "Read")]
public IActionResult GetClasses()
{
    // Only users with read permission to Classes can access
}

[HttpPost]
[Permission("Classes", "Write")]
public IActionResult CreateClass()
{
    // Only users with write permission to Classes can access
}

[HttpDelete]
[RequireRole("Admin")]
public IActionResult DeleteClass()
{
    // Only Admin role can access
}
```

### Complex Permission Checks
```csharp
[HttpPut]
[RequirePermission(
    allowedRoles: new[] { "Admin", "Teacher" }, 
    requireTeacher: true
)]
public IActionResult UpdateClass()
{
    // User must be either Admin OR Teacher, AND must have isTeacher = true
}
```

## Implementation Details

### How It Works
1. **Authentication**: The system first checks if the user is authenticated
2. **Role Check**: Verifies if the user has the required roles
3. **Property Check**: Validates user properties (isAdmin, isTeacher, isStudent)
4. **Resource Permission**: Checks specific resource and action permissions
5. **Access Control**: Grants or denies access based on all checks

### Security Features
- **Role-based Access Control (RBAC)**: Traditional role-based permissions
- **Attribute-based Access Control (ABAC)**: Property-based permissions
- **Resource-based Permissions**: Granular control over specific resources
- **Action-level Control**: Different permissions for Read/Write/Delete/Manage operations

## Best Practices

### 1. Use the Right Attribute
- **RequireRole**: For simple role checks
- **Permission**: For resource-based access control
- **RequirePermission**: For complex permission combinations

### 2. Apply at the Right Level
- **Controller level**: When all methods have the same permission requirements
- **Method level**: When different methods need different permissions

### 3. Follow the Principle of Least Privilege
- Start with restrictive permissions
- Grant only the minimum access needed
- Use specific resource permissions instead of broad role access

### 4. Test Your Permissions
- Test with different user types
- Verify both positive and negative cases
- Ensure admin users can access everything
- Confirm regular users are properly restricted

## Migration Guide

### Before (No Authorization)
```csharp
[HttpGet]
public IActionResult GetClasses()
{
    // Anyone could access this
}
```

### After (With Authorization)
```csharp
[HttpGet]
[Permission("Classes", "Read")]
public IActionResult GetClasses()
{
    // Only users with read permission to Classes can access
}
```

## Troubleshooting

### Common Issues
1. **403 Forbidden**: User doesn't have the required permissions
2. **401 Unauthorized**: User is not authenticated
3. **Permission Denied**: User lacks specific resource/action permissions

### Debug Tips
1. Check user roles in the database
2. Verify user properties (isAdmin, isTeacher, isStudent)
3. Ensure the user is properly authenticated
4. Check if the permission attribute is correctly applied

## Future Enhancements
- Custom permission claims
- Dynamic permission loading from database
- Permission caching for performance
- Audit logging for access attempts
- Permission inheritance and hierarchies
