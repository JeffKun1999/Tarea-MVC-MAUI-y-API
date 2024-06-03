using Microsoft.EntityFrameworkCore;
using API_desde_cero_JR.Data;
using API_desde_cero_JR.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace API_desde_cero_JR.Controllers;

public static class BurgerEndpoints
{
    public static void MapBurgerEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Burger").WithTags(nameof(Burger));
        //

        group.MapGet("/", async (API_desde_cero_JRContext db) =>
        {
            return await db.Burger.ToListAsync();
        })
        .WithName("GetAllBurgers")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Burger>, NotFound>> (int id, API_desde_cero_JRContext db) =>
        {
            return await db.Burger.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Burger model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetBurgerById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Burger burger, API_desde_cero_JRContext db) =>
        {
            var affected = await db.Burger
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, burger.Id)
                    .SetProperty(m => m.Name, burger.Name)
                    .SetProperty(m => m.WithCheese, burger.WithCheese)
                    .SetProperty(m => m.Precio, burger.Precio)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateBurger")
        .WithOpenApi();

        group.MapPost("/", async (Burger burger, API_desde_cero_JRContext db) =>
        {
            db.Burger.Add(burger);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Burger/{burger.Id}",burger);
        })
        .WithName("CreateBurger")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, API_desde_cero_JRContext db) =>
        {
            var affected = await db.Burger
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteBurger")
        .WithOpenApi();
    }
}
