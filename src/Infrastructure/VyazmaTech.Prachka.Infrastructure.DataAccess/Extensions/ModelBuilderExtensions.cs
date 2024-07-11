using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Extensions;

internal static class ModelBuilderExtensions
{
    public static void UseSnakeCaseNamingConvention(this ModelBuilder builder)
    {
        foreach (IMutableEntityType entity in builder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName()!.ToSnakeCase());

            foreach (IMutableProperty property in entity.GetProperties())
            {
                property.SetColumnName(property.Name.ToSnakeCase());
            }

            foreach (IMutableKey key in entity.GetKeys())
            {
                key.SetName(key.GetName()!.ToSnakeCase());
            }

            foreach (IMutableForeignKey key in entity.GetForeignKeys())
            {
                key.SetConstraintName(key.GetConstraintName()!.ToSnakeCase());
            }

            foreach (IMutableIndex index in entity.GetIndexes())
            {
                index.SetDatabaseName(index.GetDatabaseName()!.ToSnakeCase());
            }
        }
    }

    public static string ToSnakeCase(this string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return s;
        }

        Match startUnderscores = Regex.Match(s, @"^_+");
        return startUnderscores + Regex.Replace(s, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}