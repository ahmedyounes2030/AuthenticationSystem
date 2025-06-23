
# AuthenticationSystem

![JWT Logo](https://jwt.io/img/pic_logo.svg)
![Authentication & Security](https://cdn-icons-png.flaticon.com/512/3064/3064197.png)

> **Built by Ahmed Saleh Ghaithan**  
> Modern, enterprise-ready authentication platform for .NET

---

## 🚀 Overview

This project is a production-grade authentication system built with C#/.NET, demonstrating best practices in security, scalability, and software architecture.  
It includes modular APIs for user registration, login, password reset, and robust JWT-based authorization, all designed for real-world business needs.

---

## 🌟 Why Choose This System?

- **Enterprise Security:** Implements PBKDF2 password hashing, JWT with strong signing, refresh/revoke flows, and role/permission claims.
- **Clean Architecture:** Domain-driven, SOLID-principled codebase, separation of concerns, and extensibility.
- **Modern Stack:** Built with ASP.NET Core, Entity Framework, and the latest .NET features.
- **Scalable:** Easily integrates with microservices, cloud platforms, and CI/CD pipelines.
- **Portfolio-Ready:** Code quality, documentation, and testing suitable for professional review.

---

## 🏗️ System Architecture

```mermaid
flowchart TD
    subgraph Presentation Layer
      A[API Controllers]
    end
    subgraph Application Layer
      B[AuthenticationService]
      C[UserService]
    end
    subgraph Infrastructure Layer
      D[UserRepository]
      E[PasswordHasher]
      F[AccessTokenService]
      G[RefreshTokenService]
    end
    subgraph Database
      H[(SQL DB)]
    end

    A --> B
    A --> C
    B --> D
    B --> E
    B --> F
    B --> G
    D --> H
    C --> D
    F --> H
    G --> H
```

---

## 🔐 Authentication & Authorization Flow

### 1. Login

```mermaid
sequenceDiagram
    participant User
    participant API
    participant AuthService
    participant DB

    User->>API: POST /api/auth/login
    API->>AuthService: Login(email, password)
    AuthService->>DB: GetByEmail(email)
    DB-->>AuthService: user data
    AuthService->>AuthService: Verify password hash
    AuthService->>AuthService: Generate JWT & Refresh Token
    AuthService->>DB: Save tokens
    AuthService-->>API: JWT, Refresh Token
    API-->>User: Tokens
```

### 2. Registration

```mermaid
sequenceDiagram
    participant User
    participant API
    participant AuthService
    participant DB

    User->>API: POST /api/auth/register
    API->>AuthService: RegisterUser(userName, password, email)
    AuthService->>DB: Check unique email/username
    AuthService->>AuthService: Hash password (PBKDF2)
    AuthService->>DB: Insert user
    DB-->>AuthService: Success
    AuthService-->>API: UserResponse
    API-->>User: Success
```

### 3. Password Reset

```mermaid
sequenceDiagram
    participant User
    participant API
    participant UserService
    participant DB

    User->>API: POST /api/auth/change-password
    API->>UserService: ChangePassword(email, currentPassword, newPassword)
    UserService->>DB: GetByEmail
    UserService->>UserService: Verify old hash
    UserService->>UserService: Hash new password
    UserService->>DB: Update password hash
    UserService-->>API: Success
    API-->>User: Success message
```

---

## 🛡️ Security Details

### Password Hashing (PBKDF2 + SHA256)
- Each password gets a unique salt and is hashed with 10,000 iterations.
- Based on your code (see `PasswordHasher.cs`):

```csharp
public string Hash(string password)
{
    byte[] salt = RandomNumberGenerator.GetBytes(16);
    byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 10000, SHA256, 32);
    return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
}
```
Verification is done with constant-time equality checks.

### JWT Structure
- Claims: Email, Username, JTI, roles, and encoded permissions.
- Signing: HMAC SHA256.
- Configurable lifetimes for access and refresh tokens.

---

## 📦 API Endpoints

| Endpoint                   | Method | Purpose                |
|----------------------------|--------|------------------------|
| /api/auth/login            | POST   | User login             |
| /api/auth/register         | POST   | User registration      |
| /api/auth/refresh          | POST   | Refresh JWT token      |
| /api/auth/revoke           | POST   | Revoke refresh token   |
| /api/auth/change-password  | POST   | Change password        |

---

## 🛠️ Technologies Used

- **Language:** C# (.NET)
- **Framework:** ASP.NET Core Web API
- **Security:** JWT, PBKDF2, HMAC SHA256
- **Database:** Entity Framework Core
- **Architecture:** Clean/Domains-driven
- **Documentation:** Swagger/OpenAPI

---

## 📈 What Makes This Project Unique?

- Security-first, with modern cryptography and tokenization.
- Designed for extensibility (roles, permissions, claims).
- Suitable for SaaS, enterprise systems, or as a learning reference.
- Code quality and documentation aimed at technical hiring managers.

---


## 🏁 Getting Started

1. Clone repo
2. Set JWT and DB config in `appsettings.json`
3. Open package manager console and run `update-database`
4. Run the system
5. Explore endpoints via Swagger UI
