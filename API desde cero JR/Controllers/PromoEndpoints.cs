using Microsoft.EntityFrameworkCore;
using API_desde_cero_JR.Data;
using API_desde_cero_JR.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace API_desde_cero_JR.Controllers;

public static class PromoEndpoints
{
    public static void MapPromoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Promo").WithTags(nameof(Promo));

        group.MapGet("/", async (API_desde_cero_JRContext db) =>
        {
            return await db.Promo.ToListAsync();
        })
        .WithName("GetAllPromos")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Promo>, NotFound>> (int promoid, API_desde_cero_JRContext db) =>
        {
            return await db.Promo.AsNoTracking()
                .FirstOrDefaultAsync(model => model.PromoId == promoid)
                is Promo model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetPromoById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int promoid, Promo promo, API_desde_cero_JRContext db) =>
        {
            var affected = await db.Promo
                .Where(model => model.PromoId == promoid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.PromoId, promo.PromoId)
                    .SetProperty(m => m.Descripcion, promo.Descripcion)
                    .SetProperty(m => m.FechaPromo, promo.FechaPromo)
                    .SetProperty(m => m.BurgerId, promo.BurgerId)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdatePromo")
        .WithOpenApi();

        group.MapPost("/", async (Promo promo, API_desde_cero_JRContext db) =>
        {
            db.Promo.Add(promo);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Promo/{promo.PromoId}",promo);
        })
        .WithName("CreatePromo")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int promoid, API_desde_cero_JRContext db) =>
        {
            var affected = await db.Promo
                .Where(model => model.PromoId == promoid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePromo")
        .WithOpenApi();
    }
}
