# EventBooking API — Frontend Integration Guide

This document describes all public API endpoints, request/response shapes, authentication, headers, query parameters and error formats needed by the front-end (Angular) to integrate with the backend.

Base URL
- When running locally the base URL is typically: `https://localhost:{PORT}/` (see `EventBooking.Api/launchSettings.json` or `Program.cs`).
- All endpoints are relative to `/api`.

Authentication
- Most endpoints require a JWT Bearer token in the `Authorization` header: `Authorization: Bearer {accessToken}`.
- Obtain the token by calling `POST /api/auth/login` with `LoginDto`.
- `Auth` endpoints are public (no authorization required).

Global headers
- `Content-Type: application/json`
- `Accept: application/json`

Error format
- The API uses a global exception middleware which returns structured JSON for validation errors and simple objects for 404/500.

Validation error (400) example:
```json
{
  "type": "https://example.com/validation-error",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Email": ["Email is required"],
    "Password": ["Password must be at least 8 characters"]
  }
}
```

Not found (404) example:
```json
{
  "type": "https://example.com/not-found",
  "title": "Resource not found",
  "status": 404
}
```

Internal server error (500) example:
```json
{
  "type": "https://example.com/internal-server-error",
  "title": "An unexpected error occurred.",
  "status": 500
}
```

-------------------------------------------------------------------------------
Auth
-------------------------------------------------------------------------------

POST /api/auth/login
- Description: Authenticate a user (customer) and receive JWT access token.
- Public: yes
- Request body (JSON): `LoginDto`
```json
{
  "email": "user@example.com",
  "password": "PlainTextPassword"
}
```
- Response 200 (JSON): `LoginResponseDto`
```json
{
  "accessToken": "eyJ...",
  "tokenType": "Bearer",
  "expiresIn": 3600,
  "customer": {
    "id": "00000000-0000-0000-0000-000000000000",
    "firstName": "John",
    "lastName": "Doe",
    "email": "user@example.com",
    "phoneNumber": "+1234567890",
    "createdAt": "2025-01-01T12:00:00Z"
  }
}
```
- Response 401 for invalid credentials:
```json
{
  "type": "https://example.com/authentication-error",
  "title": "Invalid credentials",
  "status": 401
}
```

POST /api/auth/register
- Description: Register a new customer account.
- Public: yes
- Request body (JSON): `RegisterDto`
```json
{
  "firstName": "Jane",
  "lastName": "Smith",
  "email": "jane@example.com",
  "phoneNumber": "+1234567890",
  "password": "PlainTextPassword"
}
```
- Response 201 (Created): returns created `CustomerDto` (without password)
```json
{
  "id": "11111111-1111-1111-1111-111111111111",
  "firstName": "Jane",
  "lastName": "Smith",
  "email": "jane@example.com",
  "phoneNumber": "+1234567890",
  "createdAt": "2025-01-02T09:00:00Z"
}
```

Notes
- Save `accessToken` returned by login and include it in `Authorization` header for protected endpoints.

-------------------------------------------------------------------------------
Customers
(Controller: `CustomersController`, requires `Authorization`)
-------------------------------------------------------------------------------

GET /api/customers?page={page}&pageSize={pageSize}
- Description: Get paged list of customers.
- Query params:
  - `page` (int, default 1)
  - `pageSize` (int, default 20)
- Request headers: `Authorization: Bearer {token}`
- Response 200 (JSON): Paged response (example)
```json
{
  "page": 1,
  "pageSize": 20,
  "total": 42,
  "items": [
    {
      "id": "1111-...",
      "firstName": "Jane",
      "lastName": "Smith",
      "email": "jane@example.com",
      "phoneNumber": "+1234",
      "createdAt": "2025-01-02T09:00:00Z"
    }
  ]
}
```

GET /api/customers/{id}
- Description: Get a single customer by id.
- Response 200: `CustomerDto`
- Response 404: Not found JSON (see Error format)

POST /api/customers
- Description: Create a new customer.
- Request body: `CreateCustomerDto`
```json
{
  "firstName": "Alice",
  "lastName": "Cooper",
  "email": "alice@example.com",
  "phoneNumber": "+1987654321",
  "password": "PlainTextPassword"
}
```
- Response 201: Created `CustomerDto`

PUT /api/customers/{id}
- Description: Update an existing customer (partial allowed per DTO)
- Request body: `UpdateCustomerDto` (fields optional)
```json
{
  "firstName": "Alice",
  "phoneNumber": "+1122334455"
}
```
- Response 200: Updated `CustomerDto` or 404 if not found

DELETE /api/customers/{id}
- Description: Delete a customer
- Response 204 No Content on success

-------------------------------------------------------------------------------
Events
(Controller: `EventsController`, requires `Authorization`)
-------------------------------------------------------------------------------

GET /api/events?search={term}&page={page}&pageSize={pageSize}
- Description: Get events, optionally filtered by search term and paged.
- Query params:
  - `search` (string, optional) - search term
  - `page`, `pageSize` (paging)
- Response 200 example (paged list of `EventDto`):
```json
{
  "page": 1,
  "pageSize": 20,
  "total": 12,
  "items": [
    {
      "id": "2222-...",
      "name": "Rock Concert",
      "description": "Evening concert",
      "venue": "Main Hall",
      "startDate": "2025-06-01T19:00:00Z",
      "endDate": "2025-06-01T22:00:00Z",
      "capacity": 500,
      "isActive": true
    }
  ]
}
```

GET /api/events/{id}
- Response 200: `EventDto`
- Response 404: Not found

POST /api/events
- Request body: `CreateEventDto`
```json
{
  "name": "Jazz Night",
  "description": "A night of smooth jazz",
  "venue": "City Theater",
  "startDate": "2025-08-10T19:30:00Z",
  "endDate": "2025-08-10T22:00:00Z",
  "capacity": 200
}
```
- Response 201: Created `EventDto`

PUT /api/events/{id}
- Request body: `UpdateEventDto` (partial update)
```json
{
  "name": "Updated Event Name",
  "capacity": 250,
  "isActive": true
}
```
- Response 200: Updated `EventDto` or 404

-------------------------------------------------------------------------------
Ticket Types
(Controller: `TicketTypesController`, requires `Authorization`)
-------------------------------------------------------------------------------

GET /api/tickettypes
- Description: Get all ticket types
- Response 200 (list of `TicketTypeDto`)

GET /api/tickettypes/{id}
- Response 200: `TicketTypeDto` or 404

POST /api/tickettypes
- Request body: `CreateTicketTypeDto`
```json
{
  "eventId": "2222-...",
  "name": "VIP",
  "description": "Front row seating",
  "price": 99.99,
  "quantity": 50
}
```
- Response 201: Created `TicketTypeDto`

PUT /api/tickettypes/{id}
- Request body: `UpdateTicketTypeDto`
```json
{
  "name": "Standard",
  "price": 39.99,
  "quantity": 150,
  "isActive": true
}
```
- Response 200: Updated `TicketTypeDto` or 404

-------------------------------------------------------------------------------
Bookings
(Controller: `BookingsController`, requires `Authorization`)
-------------------------------------------------------------------------------

GET /api/bookings?page={page}&pageSize={pageSize}
- Description: Get paged bookings
- Response 200 example (paged list of `BookingDto`):
```json
{
  "page": 1,
  "pageSize": 20,
  "total": 5,
  "items": [
    {
      "id": "3333-...",
      "eventId": "2222-...",
      "customerId": "1111-...",
      "ticketTypeId": "4444-...",
      "seats": 2,
      "status": "Confirmed",
      "createdAt": "2025-02-01T15:30:00Z"
    }
  ]
}
```

GET /api/bookings/{id}
- Response 200: `BookingDto` or 404

POST /api/bookings
- Request body: `CreateBookingDto`
```json
{
  "eventId": "2222-...",
  "customerId": "1111-...",
  "ticketTypeId": "4444-...",
  "seats": 2
}
```
- Response 201: Created `BookingDto` (includes `id`)

PUT /api/bookings/{id}
- Request body: `UpdateBookingDto` (partial)
```json
{
  "seats": 3,
  "status": "Cancelled"
}
```
- Response 200: Updated `BookingDto` or 404

DELETE /api/bookings/{id}
- Response 204 No Content on success

-------------------------------------------------------------------------------
Test endpoints (for development)
- `GET /api/test/ping` - public - returns simple health check
- `GET /api/test/check-passwords` - public - debug details about password hashes
- `POST /api/test/test-bcrypt` - public - test bcrypt verification

-------------------------------------------------------------------------------
Angular integration notes (quick)
- Use Angular `HttpClient` for requests. Pass token in headers:
  - Example:
```ts
const headers = new HttpHeaders({
  'Content-Type': 'application/json',
  'Authorization': `Bearer ${token}`
});
this.http.get("/api/events?page=1&pageSize=20", { headers });
```
- For POST/PUT send JSON body; ensure you set `Content-Type` header.
- Handle 400 responses by showing validation errors. The `errors` object maps field names to arrays of messages.
- For 401 handle by redirecting to login and clearing stored token.

-------------------------------------------------------------------------------
Common DTO shapes (reference)

`LoginDto`
```json
{ "email":"string", "password":"string" }
```

`RegisterDto` / `CreateCustomerDto`
```json
{ "firstName":"string", "lastName":"string", "email":"string", "phoneNumber":"string?", "password":"string" }
```

`CustomerDto`
```json
{ "id":"guid", "firstName":"string", "lastName":"string", "email":"string", "phoneNumber":"string?", "createdAt":"datetime" }
```

`CreateEventDto` / `EventDto` (partial)
```json
{ "id":"guid?", "name":"string", "description":"string?", "venue":"string?", "startDate":"datetime", "endDate":"datetime?", "capacity":123, "isActive":true }
```

`CreateTicketTypeDto` / `TicketTypeDto` (partial)
```json
{ "id":"guid?", "eventId":"guid", "name":"string", "description":"string?", "price":12.34, "quantity": 100, "isActive":true }
```

`CreateBookingDto` / `BookingDto` (partial)
```json
{ "id":"guid?", "eventId":"guid", "customerId":"guid", "ticketTypeId":"guid?", "seats":2, "status":"string", "createdAt":"datetime" }
```

-------------------------------------------------------------------------------
Appendix: Example curl calls

Login
```bash
curl -X POST "https://localhost:5001/api/auth/login" -H "Content-Type: application/json" -d '{"email":"user@example.com","password":"pass"}'
```

Get events (authorized)
```bash
curl -H "Authorization: Bearer ${TOKEN}" "https://localhost:5001/api/events?page=1&pageSize=20"
```

Create booking (authorized)
```bash
curl -X POST "https://localhost:5001/api/bookings" -H "Authorization: Bearer ${TOKEN}" -H "Content-Type: application/json" -d '{"eventId":"2222-...","customerId":"1111-...","seats":2}'
```

-------------------------------------------------------------------------------
End of document

If you want this as a file inside the `EventBooking.Api` project directory instead of repository root, tell me and I will move it to `EventBooking.Api/API_DOCUMENTATION.md`.
