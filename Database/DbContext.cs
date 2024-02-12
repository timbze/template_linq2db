using LinqToDB;
using LinqToDB.Data;

namespace TemplateLinq2DbFastEndpoints.Database;

public sealed class DbContext(DataOptions options) : DataConnection(options), IDbContext
{
    public ITable<Car> Cars => this.GetTable<Car>();
}