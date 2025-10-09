using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Services;

namespace WebApplication1;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // MVC controllers + Swagger
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IGenreService, GenreService>();
        builder.Services.AddScoped<IPublisherService, PublisherService>();
        builder.Services.AddScoped<IMemberService, MemberService>();

        

        // EF Core DbContext (SQL Server) from appsettings ConnectionStrings:Sql
        var cs = builder.Configuration.GetConnectionString("Sql")
                 ?? throw new InvalidOperationException("Missing ConnectionStrings:Sql");
        builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(cs));

        // Per-entity services
        builder.Services.AddScoped<IAuthorService, AuthorService>();
        builder.Services.AddScoped<IBookService, BookService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // Map controllers (Authors/Books, etc.)
        app.MapControllers();

        app.Run();
    }
}