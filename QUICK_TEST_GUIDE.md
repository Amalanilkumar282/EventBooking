# Quick Test Guide - JWT Authentication ??

## Quick Start Testing (5 Minutes)

### 1. Start the API
```bash
cd EventBooking.Api
dotnet run
```

### 2. Open Swagger
Browser: `https://localhost:7XXX/openapi/ui` (check console for actual port)

---

## Test Scenario 1: Login with Seed Data

### Use Pre-Created Test Account
**Email:** `alice.smith@example.com`  
**Password:** `Password123`

### Login Request (POST /api/auth/login)
```json
{
  "email": "alice.smith@example.com",
  "password": "Password123"
}
```

### Expected Response (200 OK)
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI...",
  "tokenType": "Bearer",
  "expiresIn": 3600,
  "customer": {
    "id": "some-guid",
    "firstName": "Alice",
    "lastName": "Smith",
    "email": "alice.smith@example.com",
    "phoneNumber": "555-0001"
  }
}
```

---

## Test Scenario 2: Register New User

### Registration Request (POST /api/auth/register)
```json
{
  "firstName": "Test",
  "lastName": "User",
  "email": "test@example.com",
  "phoneNumber": "555-9999",
  "password": "Test123456"
}
```

### Expected Response (201 Created)
```json
{
  "id": "new-guid",
  "firstName": "Test",
  "lastName": "User",
  "email": "test@example.com",
  "phoneNumber": "555-9999"
}
```

---

## Test Scenario 3: Access Protected Endpoint

### Step 1: Get Token
Login first (see Scenario 1)

### Step 2: Authorize in Swagger
1. Click ?? **Authorize** button (top right)
2. Enter: `Bearer <paste-your-token-here>`
3. Click **Authorize**
4. Click **Close**

### Step 3: Call Protected Endpoint
Try **GET /api/events**

### Expected Response (200 OK)
```json
[
  {
    "id": "...",
    "name": "Rock Fest 2026",
    "description": "Annual open-air rock music festival...",
    "venue": "Riverside Park",
    // ... more events
  }
]
```

---

## Test Scenario 4: Test Without Token

### Try Without Authorization
1. Click ?? **Authorize** and click **Logout**
2. Try **GET /api/events**

### Expected Response (401 Unauthorized)
```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "status": 401
}
```

---

## Test with cURL (Command Line)

### 1. Login
```bash
curl -X POST https://localhost:7XXX/api/auth/login \
  -H "Content-Type: application/json" \
  -d "{\"email\":\"alice.smith@example.com\",\"password\":\"Password123\"}" \
  -k
```

Save the `accessToken` from response.

### 2. Call Protected Endpoint
```bash
curl -X GET https://localhost:7XXX/api/events \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -k
```

---

## Test with Postman

### 1. Create New Request
- Method: POST
- URL: `https://localhost:7XXX/api/auth/login`

### 2. Set Headers
- Key: `Content-Type`
- Value: `application/json`

### 3. Set Body (raw JSON)
```json
{
  "email": "alice.smith@example.com",
  "password": "Password123"
}
```

### 4. Send Request
Copy the `accessToken` from response

### 5. Use Token for Other Requests
For any protected endpoint:
- Go to **Authorization** tab
- Type: **Bearer Token**
- Token: Paste your token

---

## All Test Accounts (Seed Data)

| Email | Password | First Name | Last Name |
|-------|----------|------------|-----------|
| alice.smith@example.com | Password123 | Alice | Smith |
| bob.johnson@example.com | Password123 | Bob | Johnson |
| carol.williams@example.com | Password123 | Carol | Williams |
| david.brown@example.com | Password123 | David | Brown |
| eve.davis@example.com | Password123 | Eve | Davis |
| frank.miller@example.com | Password123 | Frank | Miller |

---

## Error Testing

### Test Invalid Password
```json
{
  "email": "alice.smith@example.com",
  "password": "WrongPassword"
}
```
**Expected:** 401 Unauthorized

### Test Non-Existent Email
```json
{
  "email": "notfound@example.com",
  "password": "Password123"
}
```
**Expected:** 401 Unauthorized

### Test Missing Fields
```json
{
  "email": "alice.smith@example.com"
}
```
**Expected:** 400 Bad Request with validation errors

### Test Duplicate Registration
Try registering with `alice.smith@example.com` again  
**Expected:** 400 Bad Request - "Customer with email already exists"

---

## Validation Testing

### Test Short Password (Registration)
```json
{
  "firstName": "Test",
  "lastName": "User",
  "email": "test@example.com",
  "password": "12345"
}
```
**Expected:** 400 Bad Request - "Password must be at least 6 characters"

### Test Invalid Email Format
```json
{
  "firstName": "Test",
  "lastName": "User",
  "email": "not-an-email",
  "password": "Password123"
}
```
**Expected:** 400 Bad Request - "Invalid email format"

---

## Token Expiration Testing

### Check Token Expiry
Tokens expire after 60 minutes (default).

To test immediately, change in `appsettings.json`:
```json
"ExpiryMinutes": "1"  // Expires in 1 minute
```

Wait 1 minute and try calling a protected endpoint.  
**Expected:** 401 Unauthorized

---

## Debugging Tips

### View Token Contents
1. Copy your access token
2. Go to [https://jwt.io](https://jwt.io)
3. Paste in "Encoded" section
4. See decoded payload with user info

### Check Logs
Watch the console output for:
- Database queries
- Authentication attempts
- Validation errors

### Swagger Authorization Not Working?
1. Make sure you included "Bearer " prefix
2. Token format: `Bearer eyJhbGci...`
3. No quotes around the token
4. Token must not be expired

---

## Success Checklist ?

- [ ] Can register new user
- [ ] Can login with seed account
- [ ] Receive valid JWT token
- [ ] Can access protected endpoints with token
- [ ] Get 401 without token
- [ ] Invalid credentials return 401
- [ ] Validation errors return 400
- [ ] Duplicate email returns error
- [ ] Token visible in Swagger UI
- [ ] All controllers require auth (except /auth endpoints)

---

## Next Steps After Testing

1. **Change the Secret Key** in `appsettings.json` for production
2. **Enable HTTPS** for all environments
3. **Implement Refresh Tokens** for better UX
4. **Add Roles** (Admin, Customer) for authorization
5. **Add Email Verification** for new accounts
6. **Implement Password Reset** functionality

---

## Need Help?

Check the main guide: `JWT_AUTHENTICATION_GUIDE.md`

Common issues:
- **401 errors:** Check token format and expiration
- **Build errors:** Run `dotnet build` and check for missing packages
- **Database errors:** Ensure migration was applied: `dotnet ef database update`
- **Swagger issues:** Clear browser cache and restart API

Happy Testing! ??
