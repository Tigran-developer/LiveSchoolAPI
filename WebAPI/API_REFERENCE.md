# LiveSchool API - Quick Reference

## 🔐 Authentication Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/auth/register` | Register new user | ❌ |
| POST | `/api/auth/authenticate` | Login user | ❌ |
| POST | `/api/auth/forgot-password` | Request password reset | ❌ |
| POST | `/api/auth/reset-password` | Reset password | ❌ |
| GET | `/api/auth/confirm-email` | Confirm email | ❌ |
| POST | `/api/auth/check-email-exist` | Check email exists | Admin/Teacher |

## 📚 Class Management

| Method | Endpoint | Description | Permission Required |
|--------|----------|-------------|-------------------|
| GET | `/api/classes/browse_classes` | Get available classes | Classes - Read |
| GET | `/api/classes/booked_classes` | Get enrolled classes | Classes - Read |
| GET | `/api/classes/class/{id}` | Get class details | Classes - Read |
| POST | `/api/classes/add_class` | Create new class | Classes - Write |
| PUT | `/api/classes/{id}` | Update class | Classes - Write |
| DELETE | `/api/classes/{id}` | Delete class | Classes - Delete |
| POST | `/api/classes/book_class` | Book class for student | Classes - Write |

## 👥 Pupil Management

| Method | Endpoint | Description | Permission Required |
|--------|----------|-------------|-------------------|
| GET | `/api/pupils` | Get all pupils | Pupils - Read |
| GET | `/api/pupils/student` | Get pupil by user ID | Pupils - Read |
| POST | `/api/pupils` | Create new pupil | Pupils - Write |
| PUT | `/api/pupils/{id}` | Update pupil | Pupils - Write |
| DELETE | `/api/pupils/{id}` | Delete pupil | Pupils - Delete |

## 👨‍🏫 Teacher Management

| Method | Endpoint | Description | Permission Required |
|--------|----------|-------------|-------------------|
| GET | `/api/teachers` | Get all teachers | Teachers - Read |
| GET | `/api/teachers/{id}` | Get teacher by ID | Teachers - Read |
| POST | `/api/teachers` | Create new teacher | Teachers - Write |
| PUT | `/api/teachers/{id}` | Update teacher | Teachers - Write |
| DELETE | `/api/teachers/{id}` | Delete teacher | Teachers - Delete |

## 📧 Email Services

| Method | Endpoint | Description | Role Required |
|--------|----------|-------------|---------------|
| GET | `/api/email/singleemail` | Send test email | Admin |

## 🧪 Testing Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/testauth/public` | Public endpoint | ❌ |
| GET | `/api/testauth/admin-only` | Admin only | Admin |
| GET | `/api/testauth/teacher-or-admin` | Teacher or Admin | Admin/Teacher |
| GET | `/api/testauth/admin-property` | Admin role | Admin |
| GET | `/api/testauth/teacher-property` | Teacher role | Teacher |
| GET | `/api/testauth/classes-read` | Classes read permission | Classes - Read |
| GET | `/api/testauth/classes-write` | Classes write permission | Classes - Write |
| GET | `/api/testauth/pupils-manage` | Pupils manage permission | Pupils - Manage |
| GET | `/api/testauth/system-access` | System access | System - Read |
| GET | `/api/testauth/complex-permission` | Complex permission | Admin/Teacher |
| GET | `/api/testauth/user-info` | Current user info | ✅ |

## 🔑 Permission Levels

### Resources
- **Classes**: Read, Write, Delete
- **Pupils**: Read, Write, Delete, Manage
- **Teachers**: Read, Write, Delete
- **Lessons**: Read, Write, Delete
- **Users**: Read, Write, Delete, Manage
- **System**: Read, Write, Delete, Manage

### Roles
- **Admin**: Full access to all resources
- **Teacher**: Access to classes, pupils, and lessons they manage
- **Pupil**: Access to their own data and enrolled classes

## 📊 Common Response Codes

| Code | Meaning |
|------|---------|
| 200 | OK - Request successful |
| 201 | Created - Resource created |
| 400 | Bad Request - Invalid data |
| 401 | Unauthorized - Authentication required |
| 403 | Forbidden - Insufficient permissions |
| 404 | Not Found - Resource not found |
| 500 | Internal Server Error |

## 🔧 Authentication Header

```
Authorization: Bearer <your-jwt-token>
```

## 📝 Quick Examples

### Register a Teacher
```bash
curl -X POST "https://localhost:5001/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@school.com",
    "password": "password123",
    "confirmPassword": "password123",
    "userType": "Teacher",
    "clientUrl": "https://frontend.com/confirm"
  }'
```

### Login
```bash
curl -X POST "https://localhost:5001/api/auth/authenticate" \
  -H "Content-Type: application/json" \
  -d '{
    "emailPhone": "john.doe@school.com",
    "password": "password123"
  }'
```

### Get Available Classes (with auth)
```bash
curl -X GET "https://localhost:5001/api/classes/browse_classes?userId=user-guid" \
  -H "Authorization: Bearer your-jwt-token"
```

---

**Note**: Replace `https://localhost:5001` with your actual API base URL in production.
