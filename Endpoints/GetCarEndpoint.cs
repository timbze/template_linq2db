using FastEndpoints;
using LinqToDB;
using TemplateLinq2DbFastEndpoints.Database;

namespace TemplateLinq2DbFastEndpoints.Endpoints;

public sealed class GetCarEndpoint(IDbContext db) : Endpoint<GetCarRequest, Car>
{
    public override void Configure()
    {
        Get("/cars/{Id}");
    }

    public override async Task HandleAsync(GetCarRequest req, CancellationToken ct)
    {
        var car = await db.Cars.Where(i => i.Id == req.Id).FirstOrDefaultAsync(token: ct);
        if (car is null)
            await SendNotFoundAsync(ct);
        else
            await SendAsync(car, cancellation: ct);
    }
}