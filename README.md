# JWT Authentication API

A comprehensive .NET 8 Web API project implementing JWT (JSON Web Token) authentication with refresh tokens, role-based authorization, and user management.

## 🚀 Features

- **JWT Authentication**: Secure token-based authentication system
- **Refresh Tokens**: Automatic token refresh mechanism for extended sessions
- **Role-Based Authorization**: Support for user roles and permissions
- **User Management**: User registration, login, and profile management
- **Entity Framework Core**: Database operations with SQL Server
- **ASP.NET Core Identity**: Built-in user and role management
- **Swagger Documentation**: Interactive API documentation
- **CORS Support**: Cross-origin resource sharing configuration
- **Cookie-Based Refresh Tokens**: Secure refresh token storage

## 🛠️ Technology Stack

- **.NET 8**: Latest framework version
- **ASP.NET Core Web API**: RESTful API framework
- **Entity Framework Core**: Object-relational mapping
- **SQL Server**: Database management system
- **ASP.NET Core Identity**: User authentication and authorization
- **JWT Bearer Authentication**: Token-based authentication
- **Swagger/OpenAPI**: API documentation

## 📋 Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB or Full SQL Server)
- Visual Studio 2022 or VS Code
- SQL Server Management Studio (optional)

## 🔧 Installation

1. **Clone the repository**

   ```bash
   git clone https://github.com/ziadhanii/JWT.git
   cd JWT
   ```

2. **Restore NuGet packages**

   ```bash
   dotnet restore
   ```

3. **Update database connection string**

   Edit `appsettings.json` and update the connection string:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=JWT;User Id=SA;Password=YourPassword;TrustServerCertificate=true;"
     }
   }
   ```

4. **Apply database migrations**

   ```bash
   dotnet ef database update
   ```

5. **Run the application**

   ```bash
   dotnet run
   ```

The API will be available at `https://localhost:7XXX` and `http://localhost:5XXX` (ports may vary).

## 📚 API Endpoints

### Authentication Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/auth/register` | Register a new user | No |
| POST | `/api/auth/login` | Login user | No |
| POST | `/api/auth/add-to-role` | Add user to role | No |
| GET | `/api/auth/refresh-token` | Refresh JWT token | No |
| POST | `/api/auth/revoke-token` | Revoke refresh token | No |

### Secured Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/secured` | Protected endpoint | Yes |

## 🔐 Authentication Flow

1. **User Registration**: Create a new user account
2. **User Login**: Authenticate and receive JWT + refresh token
3. **Token Usage**: Include JWT in Authorization header for protected endpoints
4. **Token Refresh**: Use refresh token to get new JWT when expired
5. **Token Revocation**: Revoke refresh tokens when needed

## 📝 Request/Response Examples

### Register User

```http
POST /api/auth/register
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "password": "SecurePassword123!",
  "confirmPassword": "SecurePassword123!"
}
```

### Login User

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "john.doe@example.com",
  "password": "SecurePassword123!"
}
```

### Response

```json
{
  "message": "Login successful",
  "isAuthenticated": true,
  "username": "john.doe@example.com",
  "email": "john.doe@example.com",
  "roles": ["User"],
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresOn": "2024-01-01T12:00:00Z"
}
```

### Access Protected Endpoint

```http
GET /api/secured
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## ⚙️ Configuration

### JWT Settings

Update `appsettings.json` with your JWT configuration:

```json
{
  "JWT": {
    "Key": "your-secret-key-here",
    "Issuer": "YourAppName",
    "Audience": "YourAppName",
    "DurationInMinutes": 60
  }
}
```

### Database Configuration

Configure your SQL Server connection:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=JWT;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

## 🗂️ Project Structure

```text
JWT/
├── Controllers/
│   ├── AuthController.cs          # Authentication endpoints
│   ├── BaseApiController.cs       # Base controller
│   └── SecuredController.cs       # Protected endpoints
├── Data/
│   └── ApplicationDbContext.cs    # Database context
├── Helpers/
│   └── JWT.cs                     # JWT helper class
├── Models/
│   ├── ApplicationUser.cs         # User entity
│   ├── AuthModel.cs              # Authentication response model
│   ├── RefreshToken.cs           # Refresh token entity
│   ├── RegisterModel.cs          # Registration request model
│   └── TokenRequestModel.cs      # Login request model
├── Services/
│   ├── AuthService.cs            # Authentication service implementation
│   └── IAuthService.cs           # Authentication service interface
├── Migrations/                    # Entity Framework migrations
├── Properties/
│   └── launchSettings.json       # Launch configuration
├── appsettings.json              # Application settings
└── Program.cs                    # Application entry point
```

## 🔒 Security Features

- **Password Hashing**: ASP.NET Core Identity handles secure password hashing
- **JWT Validation**: Token signature, expiration, and claim validation
- **Refresh Token Rotation**: Secure refresh token mechanism
- **CORS Configuration**: Controlled cross-origin access
- **HTTPS Enforcement**: Secure communication
- **Cookie Security**: HttpOnly and Secure cookie settings

## 📊 Database Schema

The project uses Entity Framework Core with the following main entities:

- **ApplicationUser**: Extended IdentityUser with additional fields
- **RefreshToken**: Stores refresh tokens with expiration
- **AspNetRoles**: Identity roles
- **AspNetUserRoles**: User-role relationships

## 🚀 Development

### Running in Development Mode

```bash
dotnet run --environment Development
```

### Database Migrations

Add new migration:

```bash
dotnet ef migrations add MigrationName
```

Update database:

```bash
dotnet ef database update
```

### API Documentation

When running in development mode, Swagger UI is available at:

- `https://localhost:7XXX/swagger`

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👤 Author

Ziad Hany

- GitHub: [@ziadhanii](https://github.com/ziadhanii)

## 📞 Support

If you have any questions or issues, please open an issue on GitHub or contact the author.
