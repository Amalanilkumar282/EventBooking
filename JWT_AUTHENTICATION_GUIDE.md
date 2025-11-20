# JWT Authentication Implementation Guide ??

## What We've Implemented

Your EventBooking API now has **JWT (JSON Web Token) authentication**! This means users must log in to get a token, then include that token with every API request.

---

## ?? Beginner-Friendly Explanation

### What is JWT Authentication?

Think of JWT like a **movie ticket**:
1. You buy a ticket at the box office (login)
2. You show your ticket to enter the theater (access protected endpoints)
3. The ticket expires after the movie (token has expiration time)
4. If someone steals your ticket, they can use it (that's why HTTPS is important!)

### How It Works

```
???????????????      1. Login        ???????????????
?             ? ???????????????????> ?             ?
?   Client    ?  Email + Password    ?   Server    ?
? (Frontend)  ?                      ?   (API)     ?
?             ? <??????????????????? ?             ?
???????????????   2. JWT Token       ???????????????
       ?
       ? 3. Store token
       ?    (localStorage, sessionStorage, or cookie)
       ?
       ?         4. API Request with token
       ?         Authorization: Bearer eyJhbGc...
       ?
??????????????? ???????????????????> ???????????????
?   Client    ?                      ?   Server    ?
?             ? <??????????????????? ?             ?
???????????????   5. Response        ???????????????
```

---

## ??? What Changed in Your Code

### 1. **Customer Entity - Added PasswordHash**
**File:** `EventBooking.Domain/Entities/Customer.cs`

```csharp
public string PasswordHash { get; set; } = string.Empty;
```

**Why?** We store passwords as **hashed** values, not plain text. BCrypt creates a secure hash that can't be reversed.

### 2. **New DTOs for Authentication**

**LoginDto** - For login requests
```csharp
public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}
```

**RegisterDto** - For new user registration
```csharp
public class RegisterDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
```

**LoginResponseDto** - Returned after successful login
```csharp
public class LoginResponseDto
{
    public string AccessToken { get; set; }  // The JWT token
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; }       // Token lifetime in seconds
    public CustomerDto Customer { get; set; }
}
```

### 3. **Authentication Services**

**ITokenService** - Generates JWT tokens
```csharp
string GenerateAccessToken(string customerId, string email);
```

**IAuthService** - Handles login and registration
```csharp
Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
Task<CustomerDto> RegisterAsync(RegisterDto registerDto);
```

### 4. **AuthController - Public Endpoints**
**File:** `EventBooking.Api/Controllers/AuthController.cs`

Two endpoints that DON'T require authentication:

- **POST /api/auth/login** - Returns JWT token
- **POST /api/auth/register** - Creates new customer account

### 5. **Protected Controllers**

All these controllers now require authentication (have `[Authorize]` attribute):
- EventsController
- CustomersController
- TicketTypesController
- BookingsController

### 6. **JWT Configuration**
**File:** `EventBooking.Api/appsettings.json`

```json
"JwtSettings": {
  "SecretKey": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
  "Issuer": "EventBookingApi",
  "Audience": "EventBookingClient",
  "ExpiryMinutes": "60"
}
```

**?? IMPORTANT:** Change `SecretKey` to a strong, random value in production!

---

## ?? How to Test

### Step 1: Start Your API
```bash
cd EventBooking.Api
dotnet run
```

### Step 2: Open Swagger UI
Navigate to: `https://localhost:7XXX/openapi/ui`

### Step 3: Register a New User

Click on **POST /api/auth/register** and use this JSON:

```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "phoneNumber": "555-1234",
  "password": "MyPassword123"
}
```

**Response:** You'll get the created customer (without password)

### Step 4: Login

Click on **POST /api/auth/login** and use:

```json
{
  "email": "john.doe@example.com",
  "password": "MyPassword123"
}
```

**Response:** You'll get a JWT token!
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "tokenType": "Bearer",
  "expiresIn": 3600,
  "customer": {
    "id": "...",
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@example.com"
  }
}
```

### Step 5: Authorize in Swagger

1. Copy the `accessToken` value
2. Click the **?? Authorize** button at the top
3. Enter: `Bearer YOUR_TOKEN_HERE` (include "Bearer ")
4. Click **Authorize**

### Step 6: Test Protected Endpoints

Now you can call any API endpoint! Try **GET /api/events**

**Without token:** You'll get `401 Unauthorized`  
**With token:** You'll get the events list! ?

---

## ?? Test with Seed Data

We've created 6 test customers, all with the **same password**:

| Email | Password (plain) |
|-------|-----------------|
| alice.smith@example.com | Password123 |
| bob.johnson@example.com | Password123 |
| carol.williams@example.com | Password123 |
| david.brown@example.com | Password123 |
| eve.davis@example.com | Password123 |
| frank.miller@example.com | Password123 |

Try logging in with any of these!

---

## ?? Understanding the JWT Token

A JWT has 3 parts separated by dots (`.`):

```
eyJhbGc.eyJzdWI.SflKxwRJ
 Header  Payload Signature
```

### Header
Contains algorithm info:
```json
{
  "alg": "HS256",
  "typ": "JWT"
}
```

### Payload (Claims)
Contains user information:
```json
{
  "sub": "customer-guid-here",
  "email": "john.doe@example.com",
  "jti": "unique-token-id",
  "exp": 1699999999
}
```

### Signature
Ensures the token wasn't tampered with.

**?? Decode your token:** Visit [jwt.io](https://jwt.io) and paste your token to see inside!

---

## ??? Security Features

### 1. **BCrypt Password Hashing**
Passwords are hashed using BCrypt with a **cost factor of 12**:
```csharp
// When creating user
string hash = BCrypt.Net.BCrypt.HashPassword(password);

// When verifying login
bool isValid = BCrypt.Net.BCrypt.Verify(password, hash);
```

**Why BCrypt?**
- Slow by design (prevents brute force)
- Auto-generates salt
- Future-proof (can increase cost factor)

### 2. **Token Validation**
The API validates:
- ? Token signature (hasn't been tampered)
- ? Token expiration (not expired)
- ? Issuer (came from our API)
- ? Audience (intended for our app)

### 3. **HTTPS Required**
Tokens are sent in headers, so always use HTTPS in production!

---

## ?? How to Use in Your Frontend

### JavaScript/Fetch Example

```javascript
// 1. Login
const loginResponse = await fetch('https://localhost:7XXX/api/auth/login', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    email: 'john.doe@example.com',
    password: 'MyPassword123'
  })
});

const { accessToken } = await loginResponse.json();

// 2. Store token
localStorage.setItem('token', accessToken);

// 3. Use token for API calls
const eventsResponse = await fetch('https://localhost:7XXX/api/events', {
  headers: {
    'Authorization': `Bearer ${localStorage.getItem('token')}`
  }
});

const events = await eventsResponse.json();
```

### React/Axios Example

```javascript
import axios from 'axios';

// Set default authorization header
axios.defaults.baseURL = 'https://localhost:7XXX';
axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;

// Login
const login = async (email, password) => {
  const response = await axios.post('/api/auth/login', { email, password });
  localStorage.setItem('token', response.data.accessToken);
  axios.defaults.headers.common['Authorization'] = `Bearer ${response.data.accessToken}`;
};

// Get events
const getEvents = async () => {
  const response = await axios.get('/api/events');
  return response.data;
};
```

---

## ?? Common Errors and Solutions

### Error: `401 Unauthorized`
**Cause:** No token or invalid token  
**Solution:** Login first and include `Authorization: Bearer <token>` header

### Error: `403 Forbidden`
**Cause:** Token is valid but user doesn't have permission  
**Solution:** Check if endpoint requires specific roles (not implemented yet)

### Error: "Token has expired"
**Cause:** Token lifetime exceeded (default 60 minutes)  
**Solution:** Login again to get a new token

### Error: "Invalid credentials"
**Cause:** Wrong email or password  
**Solution:** Check email and password are correct

---

## ?? Best Practices

### ? DO:
- Store tokens in **httpOnly cookies** (best) or **localStorage** (okay)
- Use **HTTPS** in production
- Set reasonable **expiration times** (1-24 hours)
- Implement **refresh tokens** for long sessions (advanced)
- **Hash passwords** before storing (we do this!)

### ? DON'T:
- Don't store passwords in plain text
- Don't commit real secret keys to Git
- Don't share tokens between users
- Don't store tokens in URL parameters
- Don't use weak passwords

---

## ?? Customization Options

### Change Token Expiration Time
**File:** `appsettings.json`
```json
"ExpiryMinutes": "120"  // 2 hours instead of 1
```

### Change Secret Key (REQUIRED for production!)
**File:** `appsettings.json`
```json
"SecretKey": "Generate-A-Strong-Random-Key-Here-Use-At-Least-32-Characters!"
```

Generate a strong key:
```bash
# PowerShell
-join ((65..90) + (97..122) + (48..57) | Get-Random -Count 32 | % {[char]$_})
```

### Add Custom Claims to Token
**File:** `EventBooking.Infrastructure/Services/TokenService.cs`

```csharp
var claims = new[]
{
    new Claim(JwtRegisteredClaimNames.Sub, customerId),
    new Claim(JwtRegisteredClaimNames.Email, email),
    new Claim("role", "Customer"),  // Add custom claims here!
    new Claim("fullName", $"{firstName} {lastName}")
};
```

---

## ?? Further Learning

### Next Steps:
1. **Refresh Tokens** - Allow users to stay logged in longer
2. **Role-Based Authorization** - Different permissions for Admin/Customer
3. **Email Verification** - Verify email addresses
4. **Password Reset** - Forgot password flow
5. **Two-Factor Authentication (2FA)** - Extra security layer

### Resources:
- [JWT.io](https://jwt.io) - Decode and learn about JWTs
- [BCrypt Explained](https://en.wikipedia.org/wiki/Bcrypt) - How password hashing works
- [OWASP Authentication](https://cheatsheetseries.owasp.org/cheatsheets/Authentication_Cheat_Sheet.html) - Security best practices

---

## ?? Summary

**What you now have:**
- ? Secure password storage with BCrypt
- ? JWT token generation and validation
- ? Login and registration endpoints
- ? Protected API endpoints
- ? Swagger UI with authentication support
- ? Test data with known passwords

**All APIs now require authentication except:**
- POST /api/auth/login
- POST /api/auth/register

**To access any other endpoint:**
1. Login to get a token
2. Include token in Authorization header
3. Token is valid for 60 minutes

Congratulations! Your API is now secured with industry-standard JWT authentication! ??
