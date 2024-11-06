# Elite Shop API Documentation

[<img src="https://run.pstmn.io/button.svg" alt="Run In Postman" style="width: 128px; height: 32px;">](https://app.getpostman.com/run-collection/11437081-285def51-93ba-4234-8d01-187d38af0fdd?action=collection%2Ffork&source=rip_markdown&collection-url=entityId%3D11437081-285def51-93ba-4234-8d01-187d38af0fdd%26entityType%3Dcollection%26workspaceId%3D51b43292-c621-476d-8a98-a78699c5d83a)

# Project Structure Documentation

This document outlines the structure of the project.

## Project Overview

```
asp-dotnet-core/
├── Controllers/
│   └── AuthApiController.cs
│
├── Models/
│   ├── BaseModels/
│   │   ├── UuidBaseModel.cs
│   │   └── ShortIntegerBaseModel.cs
│   ├── Domains/
│   │   ├── User.cs
│   │   ├── Role.cs
│   │   └── Address.cs
│   ├── DTOs/
│   │   ├── Requests/
│   │   │   └── UserRequestDto.cs
│   └── Settings/
│       └── RateLimitSetting.cs
│
├── Services/
│   ├── ModelServices/
│   │   ├── Implementations/
│   │   │   └── UserService.cs
│   │   └── Interfaces/
│   │       └── IUserService.cs
│   ├── UtilityServices/
│   │   ├── Implementations/
│   │   │   └── LoginRateLimitService.cs
│   │   └── Interfaces/
│   │       └── ILoginRateLimitService.cs
│
├── Migrations/
│   └── ApplicationDbContextModelSnapshot.cs
│
├── appsettings.json
├── Program.cs
├── bin/
└── obj/
```

## Directory Structure Details

### Controllers/
- **AuthApiController.cs**: Handles authentication-related API endpoints

### Models/
#### BaseModels/
- **UuidBaseModel.cs**: Base model for UUID-based entities
- **ShortIntegerBaseModel.cs**: Base model for short integer-based entities

#### Domains/
- **User.cs**: User entity model
- **Role.cs**: Role entity model
- **Address.cs**: Address entity model

#### DTOs/
##### Requests/
- **UserRequestDto.cs**: DTO for user-related requests

#### Settings/
- **RateLimitSetting.cs**: Configuration settings for rate limiting

### Services/
#### ModelServices/
- **Implementations/UserService.cs**: Implementation of user-related business logic
- **Interfaces/IUserService.cs**: Interfac
