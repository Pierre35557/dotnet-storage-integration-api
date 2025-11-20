# ApiResponseMiddleware Documentation

## Overview
`ApiResponseMiddleware` is responsible for standardizing API error responses across the entire application.  
It intercepts unhandled exceptions, logs them, maps them to structured error responses, and returns a unified
`ApiErrorResponse` JSON payload.

This middleware ensures:
- Consistent error structure for all API endpoints
- Centralized exception handling
- Logging of unexpected and system-level exceptions
- Automatic conversion of known exceptions into appropriate HTTP status codes

---

## Purpose
The middleware wraps the entire request pipeline and handles exceptions that bubble up from controllers,
services, or the framework itself. This avoids leaking raw exceptions and ensures clients always receive a
predictable response format.

It also bypasses documentation routes such as:
- `/openapi`
- `/swagger`
- `/scalar`

These are skipped to avoid altering Swagger/OpenAPI/Scalar responses.

---

## Exception Mapping
Exceptions are mapped to API responses using the internal
`MapExceptionToResponse` method.

### Mapped exceptions include:

| Exception Type            | Status Code | Message                                   |
|--------------------------|-------------|--------------------------------------------|
| `InvalidOperationException` | 400 Bad Request | The requested operation is invalid.         |
| `ArgumentNullException`     | 400 Bad Request | A required value was missing.               |
| `ArgumentException`         | 400 Bad Request | One or more arguments are invalid.          |
| `ValidationException`       | 400 Bad Request | Validation failed for one or more fields.   |
| Any other exception         | 500 Internal Server Error | An unexpected error occurred. |

The middleware logs the error, including the exception type, request method, path, and message.

---

## Returned Error Structure
All errors are wrapped in `ApiErrorResponse`:

```json
{
  "success": false,
  "message": "An unexpected error occurred.",
  "statusCode": 500,
  "errors": null
}
```

---

## JSON Response Content-Type
All error responses are returned as:

```
Content-Type: application/json
```

---

## When to Use
This middleware should be registered early in the pipeline (right after exception logging if applicable).  
It replaces the default ASP.NET Core exception handler to provide a custom response schema.

---

## Registration Example

Add the following to `Program.cs`:

```csharp
app.UseMiddleware<ApiResponseMiddleware>();
```

Ensure it appears **before** authentication/authorization if you want to handle errors from those components as well.

---

## Benefits
- Eliminates duplicated try/catch logic in controllers
- Guarantees consistent client-facing error responses
- Supports domain and validation error bubbling
- Extensible exception mapping

---

## Extending the Middleware
You can add additional exception cases in `MapExceptionToResponse`:

```csharp
CustomException => (
    StatusCodes.Status409Conflict,
    "A conflict occurred.",
    new[] { ex.Message }
),
```

---

## Summary
`ApiResponseMiddleware` is a core part of the API infrastructure, ensuring reliability, predictability,
and maintainability by enforcing a unified error handling strategy across all endpoints.
