using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.VersionTableInfo;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.Mapping;
using LinqToDB.Metadata;
using TemplateLinq2DbFastEndpoints.Database;

namespace TemplateLinq2DbFastEndpoints;

public static class ConfigureServices
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, string connectionString)
    {
        var ms = new MappingSchema();
        ms.AddMetadataReader(new SnakeCaseNamingConventionMetadataReader());

        var dataOptions = new DataOptions()
            .UseMappingSchema(ms)
            .UseConnectionString(connectionString);

        services.AddLinqToDBContext<IDbContext, DbContext>((_, _) => dataOptions);
        
        // setup migrations
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(_ => connectionString)
                .ScanIn(typeof(Program).GetTypeInfo().Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        // set migration schema tracker table name via MigratorVersionTable (z_schema_version)
        services.AddScoped<IVersionTableMetaData, MigratorVersionTable>();

        return services;
    }
}

/// <summary>
/// Convert table and column names to snake_case if <see cref="TableAttribute"/> and <see cref="ColumnAttribute"/>
/// have not been set.
/// Adapted from https://github.com/linq2db/linq2db/issues/2501#issuecomment-831285987
/// </summary>
public sealed class SnakeCaseNamingConventionMetadataReader : IMetadataReader
{
    private readonly AttributeReader _reader = new();

    public MappingAttribute[] GetAttributes(Type type)
    {
        var tableAttributeExists = _reader.GetAttributes(type).Any(i => i is TableAttribute);

        if (tableAttributeExists)
            return Array.Empty<MappingAttribute>();

        return new MappingAttribute[] {new TableAttribute(type.Name.ToSnakeCase())};
    }

    public MappingAttribute[] GetAttributes(Type type, MemberInfo memberInfo)
    {
        var columnAttributeExists = _reader.GetAttributes(type, memberInfo).Any(i => i is ColumnAttribute);

        if (columnAttributeExists)
            return Array.Empty<MappingAttribute>();

        return new MappingAttribute[] {new ColumnAttribute(memberInfo.Name.ToSnakeCase())};
    }

    public MemberInfo[] GetDynamicColumns(Type type)
        => Array.Empty<MemberInfo>();

    public string GetObjectID() => $".{nameof(SnakeCaseNamingConventionMetadataReader)}.";
}

[VersionTableMetaData]
public sealed class MigratorVersionTable : DefaultVersionTableMetaData
{
    public override string TableName => "z_schema_version";
}
