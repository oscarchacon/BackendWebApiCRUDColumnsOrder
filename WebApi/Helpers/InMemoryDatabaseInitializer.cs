using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WebApi.Helpers
{
    internal static class InMemoryDatabaseInitializer
    {
        internal static void Initialize(RepositoryContext repositoryContext, IConfiguration configuration, string contentRootPath)
        {
            repositoryContext.Database.EnsureCreated();

            var seedSections = LoadSeedSections(configuration, contentRootPath);
            if (seedSections.Count == 0)
            {
                return;
            }

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var hasChanges = false;

            foreach (var entityType in repositoryContext.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;
                if (clrType == null)
                {
                    continue;
                }

                if (!seedSections.TryGetValue(clrType.Name, out var section))
                {
                    continue;
                }

                var listType = typeof(List<>).MakeGenericType(clrType);
                var deserialized = JsonSerializer.Deserialize(section.GetRawText(), listType, jsonOptions) as IEnumerable;
                if (deserialized == null)
                {
                    continue;
                }

                var rows = deserialized.Cast<object>().ToList();
                if (rows.Count == 0)
                {
                    continue;
                }

                repositoryContext.AddRange(rows);
                hasChanges = true;
            }

            if (hasChanges)
            {
                repositoryContext.SaveChanges();
            }
        }

        private static Dictionary<string, JsonElement> LoadSeedSections(IConfiguration configuration, string contentRootPath)
        {
            var filePath = configuration["SeedData:File"];
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);
            }

            var resolvedPath = Path.IsPathRooted(filePath)
                ? filePath
                : Path.Combine(contentRootPath, filePath);

            if (!File.Exists(resolvedPath))
            {
                return new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);
            }

            var rawJson = File.ReadAllText(resolvedPath);
            if (string.IsNullOrWhiteSpace(rawJson))
            {
                return new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);
            }

            using var document = JsonDocument.Parse(rawJson);
            if (document.RootElement.ValueKind != JsonValueKind.Object)
            {
                return new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);
            }

            var result = new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);
            foreach (var property in document.RootElement.EnumerateObject())
            {
                if (property.Value.ValueKind == JsonValueKind.Array)
                {
                    result[property.Name] = property.Value.Clone();
                }
            }

            return result;
        }
    }
}
