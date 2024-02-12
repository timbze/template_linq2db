using FastEndpoints;
using LinqToDB;
using TemplateLinq2DbFastEndpoints.Database;

namespace TemplateLinq2DbFastEndpoints.Endpoints;

// return type of List<Car> should ideally be List<CarApi> or similar. API type should be separate from db type for best practice.
public sealed class GetCarListEndpoint(IDbContext db) : EndpointWithoutRequest<List<Car>>
{
    public override void Configure()
    {
        Get("/cars");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var cars = await db.Cars.ToListAsync(ct);
        await SendAsync(cars, cancellation: ct);
    }
}