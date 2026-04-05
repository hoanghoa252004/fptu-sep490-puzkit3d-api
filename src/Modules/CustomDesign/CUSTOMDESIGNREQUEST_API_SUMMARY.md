# Custom Design Request API - Implementation Summary

## Overview
Hoàn t?t implementation các API cho CustomDesignRequest module theo yêu c?u.

## API Endpoints

### 1. POST /api/custom-design-requests - Create Custom Design Request
- **Role**: Customer only
- **Features**:
  - Auto-generates Code (CDR001, CDR002, ...)
  - CustomerId l?y t? JWT token
  - Status init là `Submitted`
  - UsedSupportConceptDesignTime luôn = 0
- **Validation**:
  - Length, Width, Height >= 10mm
  - DesiredDeliveryDate > hôm nay
  - DesiredQuantity > 0
  - TargetBudget > 0
  - CustomDesignRequirementId ph?i t?n t?i
- **Response**: 201 Created v?i Guid id

### 2. PUT /api/custom-design-requests/{id} - Update Custom Design Request
- **Role**: Customer, Staff
- **Features**:
  - Cho phép update các field (ngo?i tr? Code, CustomerId, UsedSupportConceptDesignTime)
  - Field nào truy?n v? thì update
- **Validation**:
  - Request v?i id ph?i t?n t?i
  - N?u có update Length/Width/Height thì ph?i >= 10mm
  - DesiredDeliveryDate n?u có thì > hôm nay
  - DesiredQuantity n?u có thì > 0
  - TargetBudget n?u có thì > 0
- **Response**: 204 No Content

### 3. DELETE /api/custom-design-requests/{id} - Delete Custom Design Request
- **Role**: Customer only
- **Features**:
  - Xóa request n?u t?n t?i
- **Validation**:
  - Request v?i id ph?i t?n t?i
- **Response**: 204 No Content

### 4. GET /api/custom-design-requests - Get All Custom Design Requests
- **Role**: Customer, Staff, Business Manager
- **Features**:
  - Pagination: pageNumber, pageSize
  - Filter by status (optional, string enum name)
  - Customer: ch? l?y request c?a h?
  - Staff/Manager: l?y t?t c?
- **Query Parameters**:
  - pageNumber (default: 1)
  - pageSize (default: 10)
  - status (optional: string name nh? "Submitted", "Approved", etc.)
- **Response**: 200 OK v?i PagedResult

### 5. GET /api/custom-design-requests/{id} - Get Custom Design Request by ID
- **Role**: Customer, Staff, Business Manager
- **Features**:
  - Customer: ch? xem ???c request c?a h?
  - Staff/Manager: xem ???c t?t c?
  - Return 404 n?u request không t?n t?i ho?c customer không có quy?n xem
- **Response**: 200 OK v?i GetCustomDesignRequestByIdResponseDto

## Project Structure

### Application Layer
```
UseCases/CustomDesignRequests/
??? Commands/
?   ??? CreateRequest/
?   ?   ??? CreateCustomDesignRequestCommand.cs
?   ?   ??? CreateCustomDesignRequestCommandHandler.cs
?   ??? UpdateRequest/
?   ?   ??? UpdateCustomDesignRequestCommand.cs
?   ?   ??? UpdateCustomDesignRequestCommandHandler.cs
?   ??? DeleteRequest/
?       ??? DeleteCustomDesignRequestCommand.cs
?       ??? DeleteCustomDesignRequestCommandHandler.cs
??? Queries/
    ??? GetAllRequests/
    ?   ??? GetAllCustomDesignRequestsQuery.cs
    ?   ??? GetAllCustomDesignRequestsResponseDto.cs
    ?   ??? GetAllCustomDesignRequestsQueryHandler.cs
    ??? GetRequestById/
        ??? GetCustomDesignRequestByIdQuery.cs
        ??? GetCustomDesignRequestByIdResponseDto.cs
        ??? GetCustomDesignRequestByIdQueryHandler.cs

Services/
??? ICustomDesignRequestCodeGenerator.cs
```

### API Layer
```
CustomDesignRequests/
??? CreateRequest/CreateCustomDesignRequest.cs
??? UpdateRequest/UpdateCustomDesignRequest.cs
??? DeleteRequest/DeleteCustomDesignRequest.cs
??? GetAllRequests/GetAllCustomDesignRequests.cs
??? GetRequestById/GetCustomDesignRequestById.cs
```

### Persistence Layer
```
Services/
??? CustomDesignRequestCodeGenerator.cs
Repositories/
??? CustomDesignRequestRepository.cs (?ã t?n t?i)
```

## Key Features Implemented

1. **Command/Query Pattern**: S? d?ng MediatR ICommand, ICommandT, IQuery pattern
2. **Authorization**: Role-based access control (Customer, Staff, Business Manager)
3. **Validation**: Comprehensive validation rules trong handler
4. **Result Pattern**: S? d?ng ResultT<T> ?? handle success/failure
5. **Pagination**: PagedResult<T> cho GetAll endpoint
6. **Code Generation**: Auto-generate code theo pattern CDR001, CDR002, ...
7. **Error Handling**: S? d?ng CustomDesignRequestError class

## Dependencies Updated

- DependencyInjection.cs: ?ã thêm registration cho ICustomDesignRequestCodeGenerator

## Notes

- T?t c? endpoint ???c register qua EndpointGroupExtension.MapCustomDesignRequestGroup()
- Validation rules ???c implement trong handler, không ph?i ? API layer
- Authorization checks ???c handle t?i handler level cho GetAll, GetById
- UsedSupportConceptDesignTime luôn = 0 khi t?o, không th? update t? client
