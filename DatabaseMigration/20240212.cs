using FluentMigrator;

namespace TemplateLinq2DbFastEndpoints.DatabaseMigration;

[Migration(MigrationVersion)]
public sealed class Migration20240212 : Migration {
    private const long MigrationVersion = 2024021200;

    public override void Up()
    {
        Execute.Sql(
"""
CREATE TABLE IF NOT EXISTS public.cars (
    id uuid NOT NULL,
    make text,
    model text,
    CONSTRAINT pk_cars PRIMARY KEY (id)
);
"""
        );

        Console.WriteLine(@$"Migration '{MigrationVersion}' completed successfully");
    }

    public override void Down() =>
        throw new NotImplementedException("Can not downgrade this migration");
}
