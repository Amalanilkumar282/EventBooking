# API Endpoints Documentation

## Authentication APIs

### POST /api/auth/login

Request Body:
```json
{
  "email": "string",
  "password": "string"
}
```

Response:
```json
{
  "accessToken": "string",
  "tokenType": "Bearer",
  "expiresIn": 3600,
  "customer": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "firstName": "string",
    "lastName": "string",
    "email": "string",
    "phoneNumber": "string",
    "createdAt": "2024-01-01T00:00:00Z"
  }
}
```

### POST /api/auth/register

Request Body:
```json
{
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "phoneNumber": "string",
  "password": "string"
}
```

Response:
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "phoneNumber": "string",
  "createdAt": "2024-01-01T00:00:00Z"
}
```

## Booking APIs

### GET /api/bookings?page=1&pageSize=20

Request Body:
No body needed

Response:
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "ticketTypeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "seats": 2,
    "totalPrice": 100.00,
    "status": "Pending",
    "createdAt": "2024-01-01T00:00:00Z"
  }
]
```

### GET /api/bookings/{id}

Request Body:
No body needed

Response:
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "ticketTypeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "seats": 2,
  "totalPrice": 100.00,
  "status": "Pending",
  "createdAt": "2024-01-01T00:00:00Z"
}
```

### POST /api/bookings

Request Body:
```json
{
  "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "ticketTypeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "seats": 2
}
```

Response:
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "ticketTypeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "seats": 2,
  "totalPrice": 100.00,
  "status": "Pending",
  "createdAt": "2024-01-01T00:00:00Z"
}
```

### PUT /api/bookings/{id}

Request Body:
```json
{
  "seats": 3,
  "status": "Confirmed"
}
```

Response:
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "ticketTypeId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "seats": 3,
  "totalPrice": 150.00,
  "status": "Confirmed",
  "createdAt": "2024-01-01T00:00:00Z"
}
```

### DELETE /api/bookings/{id}

Request Body:
No body needed

Response:
```json
No Content (204)
```

## Customer APIs

### GET /api/customers?page=1&pageSize=20

Request Body:
No body needed

Response:
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "firstName": "string",
    "lastName": "string",
    "email": "string",
    "phoneNumber": "string",
    "createdAt": "2024-01-01T00:00:00Z"
  }
]
```

### GET /api/customers/{id}

Request Body:
No body needed

Response:
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "phoneNumber": "string",
  "createdAt": "2024-01-01T00:00:00Z"
}
```

### POST /api/customers

Request Body:
```json
{
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "phoneNumber": "string",
  "password": "string"
}
```

Response:
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "phoneNumber": "string",
  "createdAt": "2024-01-01T00:00:00Z"
}
```

### PUT /api/customers/{id}

Request Body:
```json
{
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "phoneNumber": "string"
}
```

Response:
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "phoneNumber": "string",
  "createdAt": "2024-01-01T00:00:00Z"
}
```

### DELETE /api/customers/{id}

Request Body:
No body needed

Response:
```json
No Content (204)
```

## Event APIs

### GET /api/events?search=term&page=1&pageSize=20

Request Body:
No body needed

Response:
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "string",
    "description": "string",
    "venue": "string",
    "startDate": "2024-01-01T00:00:00Z",
    "endDate": "2024-01-02T00:00:00Z",
    "capacity": 100,
    "isActive": true,
    "createdAt": "2024-01-01T00:00:00Z"
  }
]
```

### GET /api/events/{id}

Request Body:
No body needed

Response:
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "string",
  "description": "string",
  "venue": "string",
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-01-02T00:00:00Z",
  "capacity": 100,
  "isActive": true,
  "createdAt": "2024-01-01T00:00:00Z"
}
```

### POST /api/events

Request Body:
```json
{
  "name": "string",
  "description": "string",
  "venue": "string",
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-01-02T00:00:00Z",
  "capacity": 100
}
```

Response:
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "string",
  "description": "string",
  "venue": "string",
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-01-02T00:00:00Z",
  "capacity": 100,
  "isActive": true,
  "createdAt": "2024-01-01T00:00:00Z"
}
```

### PUT /api/events/{id}

Request Body:
```json
{
  "name": "string",
  "description": "string",
  "venue": "string",
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-01-02T00:00:00Z",
  "capacity": 100,
  "isActive": true
}
```

Response:
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "string",
  "description": "string",
  "venue": "string",
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-01-02T00:00:00Z",
  "capacity": 100,
  "isActive": true,
  "createdAt": "2024-01-01T00:00:00Z"
}
```

### DELETE /api/events/{id}

Request Body:
No body needed

Response:
```json
No Content (204)
```

## Ticket Type APIs

### GET /api/tickettypes

Request Body:
No body needed

Response:
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "string",
    "description": "string",
    "price": 50.00,
    "quantity": 100,
    "sold": 25,
    "isActive": true
  }
]
```

### GET /api/tickettypes/{id}

Request Body:
No body needed

Response:
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "string",
  "description": "string",
  "price": 50.00,
  "quantity": 100,
  "sold": 25,
  "isActive": true
}
```

### POST /api/tickettypes

Request Body:
```json
{
  "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "string",
  "description": "string",
  "price": 50.00,
  "quantity": 100
}
```

Response:
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "string",
  "description": "string",
  "price": 50.00,
  "quantity": 100,
  "sold": 0,
  "isActive": true
}
```

### PUT /api/tickettypes/{id}

Request Body:
```json
{
  "name": "string",
  "description": "string",
  "price": 60.00,
  "quantity": 150,
  "isActive": true
}
```

Response:
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "eventId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "string",
  "description": "string",
  "price": 60.00,
  "quantity": 150,
  "sold": 25,
  "isActive": true
}
```

### DELETE /api/tickettypes/{id}

Request Body:
No body needed

Response:
```json
No Content (204)
```
