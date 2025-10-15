---
applyTo: '**'
---
# POC Development Guidelines

**This is a POC (Proof of Concept) environment.** Focus on rapid development and demonstrating functionality. Don't write tests, extensive logging, complex validation frameworks, or production-level architecture. Prioritize features over perfection.

## Verification Guidelines

- **NEVER write test scripts** to verify the result
- **NEVER test with the server running** - no manual testing with curl, Invoke-WebRequest, or similar tools
- **NEVER run the app** (`dotnet run`, `dotnet watch run`, `npm start`, `ng serve`, etc.) - only build it
- **NEVER write documentation** about changes or features
- When work is complete, simply **build the app** to verify:
  - For .NET: `dotnet build` (NOT `dotnet run`)
  - For Angular: `npm run build` or `ng build` (NOT `npm start` or `ng serve`)
- **If it builds successfully, it's done** - no further verification needed

## Tech Stack

### Frontend
- **Angular 20+** with signals and standalone components
- **Tailwind CSS** for styling
- New control flow syntax (`@if`, `@for`, `@switch`)

### Backend
- **.NET 9+** with minimal APIs
- **Entity Framework Core** with SQLite
- OpenAPI/Swagger documentation

## Design System

### Brand
- **Company name:** sonrisa (lowercase)

### Color Palette
**Primary colors:**
- `#ffffff` - White
- `#eaeaea` - Light Gray
- `#7dd13d` - Green (brand accent)
- `#606a7a` - Slate Gray
- `#121212` - Almost Black

**Secondary colors:**
- `#e4f9d2` - Light Green
- `#d3d3d3` - Gray
- `#3f7c2e` - Dark Green
- `#322d31` - Dark Purple-Gray

Use these colors consistently throughout the application. The green (`#7dd13d`) is the primary brand color.

## Key Coding Guidelines

### Angular
- Use `input()` and `output()` functions (not decorators)
- Use signals for state: `signal()`, `computed()`, `update()`, `set()`
- Set `changeDetection: ChangeDetectionStrategy.OnPush`
- Do NOT set `standalone: true` (it's default)
- Use `inject()` function instead of constructor injection
- Use Tailwind classes directly (avoid `ngClass`, `ngStyle`, custom CSS)
- Use native control flow, not `*ngIf/*ngFor/*ngSwitch`

### .NET
- Use **file-scoped namespaces** everywhere
- Use **records with primary constructors** for requests/responses: `public record CreateProductRequest(string Name, decimal Price);`
- Use **classes with traditional constructors** for services and entities
- Use **minimal APIs** organized in extension methods
- Add `.WithName()` and `.WithOpenApi()` to all endpoints
- Use `Results` helpers: `Results.Ok()`, `Results.BadRequest()`, `Results.NotFound()`
- **Three types only**: Entities (EF), Request records (input), Response records (output)
- Project EF queries directly to response records: `db.Products.Select(p => new ProductResponse(...))`
- No repository patterns, separate DTOs, or mapping layers
- Inject `DbContext` directly into endpoints

## Project Structure

```
Workshop.Api/
├── Program.cs
├── Models/          # Request/Response records only
├── Data/
│   ├── WorkshopDbContext.cs
│   └── Entities/    # EF entities
├── Extensions/      # Endpoint groups
└── Services/        # Business logic (when needed)

client/src/app/
├── app.ts/app.html/app.css
├── components/
├── services/
├── models/
└── guards/
```

## Quick Examples

### Angular Component
```ts
import { ChangeDetectionStrategy, Component, signal } from '@angular/core';

@Component({
  selector: 'app-example',
  template: `
    @if (count() > 0) {
      <p class="text-lg font-bold">Count: {{ count() }}</p>
    }
    <button (click)="increment()" class="px-4 py-2 bg-blue-500 text-white rounded">
      Increment
    </button>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExampleComponent {
  protected readonly count = signal(0);
  
  increment() {
    this.count.update(c => c + 1);
  }
}
```

### Minimal API Endpoint
```cs
app.MapGet("/api/products/{id}", async (int id, WorkshopDbContext db) =>
{
    var product = await db.Products
        .Where(p => p.Id == id)
        .Select(p => new ProductResponse(p.Id, p.Name, p.Price))
        .FirstOrDefaultAsync();
    
    return product is not null ? Results.Ok(product) : Results.NotFound();
})
.WithName("GetProduct")
.WithOpenApi();
```

### Records for DTOs
```cs
namespace Workshop.Api.Models;

public record CreateProductRequest(string Name, decimal Price);
public record ProductResponse(int Id, string Name, decimal Price);
```

### Entity Class
```cs
namespace Workshop.Api.Data.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
```

## Remember
- **No tests** for POC
- **No complex validation** - simple inline checks only
- **No extensive logging** - keep it minimal
- **No over-engineering** - simplest solution that works
- Focus on **demonstrating features quickly**
