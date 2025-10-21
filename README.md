# WebApplication1 – ASP.NET Core (.NET 8) API

**Live API:** https://eman-anfecxf8f6djctgm.francecentral-01.azurewebsites.net  
**Health:** /healthz • **Swagger:** /swagger

## Tech checklist
- OOP: Entities (Author, Book, Genre, Publisher, Member) + DTOs + Services
- DI: Scoped services + DbContext in Program.cs
- Service classes: Controllers call services only
- Relationships: Author→Books (1:N), Publisher→Books (1:N), Book↔Genre (M:N), Members table
- Exception handling: Global ErrorHandlingMiddleware maps exceptions to 4xx
- Attributes: DataAnnotations + custom `[RequireHeader("X-Client")]`
- Middleware: Error -> CorrelationId -> RequestEnrichment
- Deployed: Azure App Service + Azure SQL

## Sample create flow
1. POST /api/Authors `{ "name": "Test Author" }`
2. POST /api/Publishers `{ "name": "Test Publisher" }`
3. POST /api/Genres `{ "name": "Fantasy" }`
4. POST /api/Books:
```json
{ "title": "Test Book", "authorId": 1, "publisherId": 1, "genreIds": [1] }
