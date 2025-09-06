# LiveSchool API

A comprehensive REST API for managing an online school platform with role-based authentication and authorization.

## 🚀 Features

- **User Authentication & Authorization**: JWT-based authentication with role-based access control
- **Role Management**: Admin, Teacher, and Pupil roles with granular permissions
- **Class Management**: Create, read, update, and delete classes with enrollment
- **User Management**: Student and teacher registration and management
- **Email Services**: Email confirmation and password reset functionality
- **Permission System**: Resource-based permission control (Classes, Pupils, Teachers, Lessons, Users, System)

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: ASP.NET Core Identity with JWT Bearer tokens
- **Email**: FluentEmail with SMTP support
- **Documentation**: Swagger/OpenAPI

## 📋 Prerequisites

- .NET 8.0 SDK
- SQL Server (Local or Remote)
- SMTP server for email functionality

## ⚙️ Configuration

### Database Connection
Update `appsettings.json` with your database connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=liveSchool;User Id=your_user;Password=your_password;TrustServerCertificate=true"
  }
}
```

### Email Configuration
Configure email settings in `appsettings.json`:

```json
{
  "EmailSettings": {
    "DefaultFromEmail": "noreply@yourschool.com",
    "SMTPSetting": {
      "Host": "smtp.yourserver.com",
      "Port": 587
    }
  }
}
```

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone <repository-url>
cd WebAPI
```

### 2. Install Dependencies
```bash
dotnet restore
```

### 3. Update Configuration
- Update database connection string
- Configure email settings
- Set environment variables if needed

### 4. Run Migrations
```bash
dotnet ef database update
```

### 5. Run the Application
```bash
dotnet run
```

The API will be available at `https://localhost:5001` (or your configured port).

## 📚 API Endpoints

### Authentication Endpoints

#### POST `/api/auth/register`
Register a new user (Teacher or Pupil).

**Request Body:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "password": "password123",
  "confirmPassword": "password123",
  "phone": "+1234567890",
  "userType": "Teacher",
  "initials": "JD",
  "clientUrl": "https://yourfrontend.com/confirm"
}
```

**Response:** 200 OK on successful registration

#### POST `/api/auth/authenticate`
Authenticate user and get user data with roles.

**Request Body:**
```json
{
  "emailPhone": "john.doe@example.com",
  "password": "password123"
}
```

**Response:**
```json
{
  "id": "user-id",
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "phone": "+1234567890",
  "roles": ["Teacher"]
}
```

#### POST `/api/auth/forgot-password`
Request password reset.

**Request Body:**
```json
{
  "email": "john.doe@example.com",
  "clientUrl": "https://yourfrontend.com/reset"
}
```

#### POST `/api/auth/reset-password`
Reset password with token.

**Request Body:**
```json
{
  "email": "john.doe@example.com",
  "token": "reset-token",
  "password": "newpassword123"
}
```

#### GET `/api/auth/confirm-email`
Confirm email address.

**Query Parameters:**
- `email`: User's email address
- `token`: Confirmation token

#### POST `/api/auth/check-email-exist`
Check if email already exists (Admin/Teacher only).

**Query Parameters:**
- `email`: Email to check

**Required Role:** Admin or Teacher

### Class Management Endpoints

#### GET `/api/classes/browse_classes`
Get available classes for a student.

**Query Parameters:**
- `userId`: Student's user ID

**Required Permission:** Classes - Read

**Response:** List of available classes with teacher and difficulty information

#### GET `/api/classes/booked_classes`
Get classes enrolled by a student.

**Query Parameters:**
- `studentId`: Student's ID

**Required Permission:** Classes - Read

#### GET `/api/classes/class/{id}`
Get class details by ID.

**Path Parameters:**
- `id`: Class ID (Guid)

**Required Permission:** Classes - Read

#### POST `/api/classes/add_class`
Create a new class.

**Required Permission:** Classes - Write

**Request Body:**
```json
{
  "title": "Advanced Mathematics",
  "description": "Advanced level mathematics course",
  "subject": "Mathematics",
  "teacherId": "teacher-guid",
  "startTime": "2024-01-15T09:00:00Z",
  "endTime": "2024-01-15T10:30:00Z",
  "durationInMinutes": 90,
  "maxParticipants": 25,
  "price": 150.00,
  "difficultyId": 2,
  "isOnline": true
}
```

#### PUT `/api/classes/{id}`
Update class information.

**Path Parameters:**
- `id`: Class ID

**Required Permission:** Classes - Write

#### DELETE `/api/classes/{id}`
Delete a class (soft delete).

**Path Parameters:**
- `id`: Class ID

**Required Permission:** Classes - Delete

#### POST `/api/classes/book_class`
Book a class for a student.

**Required Permission:** Classes - Write

**Request Body:**
```json
{
  "classId": "class-guid",
  "studentId": "student-guid"
}
```

### Pupil Management Endpoints

#### GET `/api/pupils`
Get all pupils.

**Required Permission:** Pupils - Read

#### GET `/api/pupils/student`
Get pupil by user ID.

**Query Parameters:**
- `userId`: User's ID

**Required Permission:** Pupils - Read

#### POST `/api/pupils`
Create a new pupil.

**Required Permission:** Pupils - Write

#### PUT `/api/pupils/{id}`
Update pupil information.

**Path Parameters:**
- `id`: Pupil ID

**Required Permission:** Pupils - Write

#### DELETE `/api/pupils/{id}`
Delete a pupil.

**Path Parameters:**
- `id`: Pupil ID

**Required Permission:** Pupils - Delete

### Teacher Management Endpoints

#### GET `/api/teachers`
Get all teachers.

**Required Permission:** Teachers - Read

#### GET `/api/teachers/{id}`
Get teacher by ID.

**Path Parameters:**
- `id`: Teacher ID

**Required Permission:** Teachers - Read

#### POST `/api/teachers`
Create a new teacher.

**Required Permission:** Teachers - Write

#### PUT `/api/teachers/{id}`
Update teacher information.

**Path Parameters:**
- `id`: Teacher ID

**Required Permission:** Teachers - Write

#### DELETE `/api/teachers/{id}`
Delete a teacher.

**Path Parameters:**
- `id`: Teacher ID

**Required Permission:** Teachers - Delete

### Email Endpoints

#### GET `/api/email/singleemail`
Send a test email (Admin only).

**Required Role:** Admin

### Testing Endpoints

#### GET `/api/testauth/public`
Public endpoint - no authentication required.

#### GET `/api/testauth/admin-only`
Admin only endpoint.

**Required Role:** Admin

#### GET `/api/testauth/teacher-or-admin`
Teacher or Admin endpoint.

**Required Role:** Admin or Teacher

#### GET `/api/testauth/admin-property`
Admin role required.

**Required Role:** Admin

#### GET `/api/testauth/teacher-property`
Teacher role required.

**Required Role:** Teacher

#### GET `/api/testauth/classes-read`
Classes read permission required.

**Required Permission:** Classes - Read

#### GET `/api/testauth/classes-write`
Classes write permission required.

**Required Permission:** Classes - Write

#### GET `/api/testauth/pupils-manage`
Pupils manage permission required.

**Required Permission:** Pupils - Manage

#### GET `/api/testauth/system-access`
System access permission required.

**Required Permission:** System - Read

#### GET `/api/testauth/complex-permission`
Complex permission endpoint.

**Required Role:** Admin or Teacher

#### GET `/api/testauth/user-info`
Get current user information.

## 🔐 Authentication & Authorization

### JWT Bearer Token
Include the JWT token in the Authorization header:
```
Authorization: Bearer <your-jwt-token>
```

### Role-Based Access Control
- **Admin**: Full access to all resources
- **Teacher**: Access to classes, pupils, and lessons they manage
- **Pupil**: Access to their own data and enrolled classes

### Permission System
Resources with permission levels:
- **Classes**: Read, Write, Delete
- **Pupils**: Read, Write, Delete, Manage
- **Teachers**: Read, Write, Delete
- **Lessons**: Read, Write, Delete
- **Users**: Read, Write, Delete, Manage
- **System**: Read, Write, Delete, Manage

## 📊 Data Models

### User
```json
{
  "id": "string",
  "userName": "string",
  "email": "string",
  "firstName": "string",
  "lastName": "string",
  "phone": "string",
  "initials": "string"
}
```

### Class
```json
{
  "id": "guid",
  "title": "string",
  "description": "string",
  "subject": "string",
  "teacherId": "guid",
  "startTime": "datetime",
  "endTime": "datetime",
  "durationInMinutes": "int",
  "maxParticipants": "int",
  "price": "decimal",
  "difficultyId": "int",
  "isOnline": "boolean"
}
```

### Pupil
```json
{
  "id": "guid",
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "phone": "string",
  "userId": "string"
}
```

### Teacher
```json
{
  "id": "guid",
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "phone": "string",
  "userId": "string"
}
```

## 🚨 Error Handling

The API returns appropriate HTTP status codes:

- **200 OK**: Request successful
- **201 Created**: Resource created successfully
- **400 Bad Request**: Invalid request data
- **401 Unauthorized**: Authentication required
- **403 Forbidden**: Insufficient permissions
- **404 Not Found**: Resource not found
- **500 Internal Server Error**: Server error

Error responses include detailed error messages:
```json
{
  "errors": ["Error description 1", "Error description 2"]
}
```

## 🔧 Development

### Running in Development Mode
```bash
dotnet run --environment Development
```

### Swagger Documentation
Access Swagger UI at `/swagger` when running in development mode.

### Database Migrations
```bash
# Create new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

### Testing
```bash
# Run tests
dotnet test

# Build project
dotnet build
```

## 📝 Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `ASPNETCORE_ENVIRONMENT` | Environment (Development/Production) | Development |
| `ConnectionStrings__DefaultConnection` | Database connection string | - |
| `EmailSettings__DefaultFromEmail` | Default sender email | - |
| `EmailSettings__SMTPSetting__Host` | SMTP server host | - |
| `EmailSettings__SMTPSetting__Port` | SMTP server port | 25 |

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License.

## 🆘 Support

For support and questions:
- Create an issue in the repository
- Contact the development team
- Check the documentation

## 🔄 Changelog

See [CHANGELOG.md](./CHANGELOG.md) for detailed information about recent changes and updates.
