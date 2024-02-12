using FastEndpoints;
using LinqToDB;
using TemplateLinq2DbFastEndpoints.Database;

namespace TemplateLinq2DbFastEndpoints.Endpoints;

public sealed class CreateCarEndpoint(IDbContext db) : Endpoint<Car>
{
    public override void Configure()
    {
        Post("/cars");
    }

    public override async Task HandleAsync(Car req, CancellationToken ct)
    {
        var carIdAlreadyExists = await db.Cars.AnyAsync(i => i.Id == req.Id, token: ct);
        if (carIdAlreadyExists)
        {
            await SendAsync(null, StatusCodes.Status409Conflict, ct);
            return;
        }
        
        await db.InsertAsync(req, token: ct);
        await SendAsync(null, StatusCodes.Status201Created, ct);
    }
}