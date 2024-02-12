using LinqToDB;

namespace TemplateLinq2DbFastEndpoints.Database;

public interface IDbContext : IDataContext
{
    public ITable<Car> Cars { get; }
}