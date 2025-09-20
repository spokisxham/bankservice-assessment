# Bank Withdrawal Assessment

## Overview
This C# ASP.NET Core project that improves a bank withdrawal operation by applying clean architecture, dependency injection, and event-driven design using AWS SNS.

## Key Improvements
-  Layered architecture (Controller → Service → Publisher)
-  Dependency injection for flexibility and testability
-  Transactional database operations for correctness
-  Event publishing via AWS SNS for  observability
-  Structured logging using ILogger
-  DTOs and interfaces for clean separation of concerns
-  .gitignore to avoid committing unnecessary files

## Endpoint
`POST /api/bank/withdraw`

## Technologies Used
- ASP.NET Core
- Microsoft.Data.SqlClient
- AWS SDK for .NET (SNS)
- Newtonsoft.Json

## SQL Note
 I would replace the inline SQL statements with stored procedures to improve performance, security, and maintainability. Stored procedures offer better  concurrency control. For this assessment, I’ve kept the SQL inline to maintain clarity and focus on architectural improvements.

## Assessment Notes
This implementation preserves the original business functionality while improving:


- Easier to understand and update — the code is organized into layers
- Handles errors more safely — problems are logged and won’t crash the system
- Works well even with more users — designed to scale as the system grows
- Tracks what happen — events and logs help monitor and audit activity
- Can run in different environments — flexible and cost-aware design

