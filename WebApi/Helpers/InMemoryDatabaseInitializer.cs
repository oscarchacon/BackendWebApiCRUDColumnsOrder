using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Entities;
using Entities.Models;
using Microsoft.Extensions.Configuration;

namespace WebApi.Helpers
{
    internal static class InMemoryDatabaseInitializer
    {
        internal static void Initialize(RepositoryContext repositoryContext, IConfiguration configuration, string contentRootPath)
        {
            repositoryContext.Database.EnsureCreated();

            if (repositoryContext.Entities.Any())
            {
                return;
            }

            var seedEntities = GetSeedEntities(configuration, contentRootPath);
            if (!seedEntities.Any())
            {
                return;
            }

            repositoryContext.Entities.AddRange(seedEntities);
            repositoryContext.SaveChanges();
        }

        private static List<Entity> GetSeedEntities(IConfiguration configuration, string contentRootPath)
        {
            var filePath = configuration["SeedData:File"];
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return new List<Entity>();
            }

            var resolvedPath = Path.IsPathRooted(filePath)
                ? filePath
                : Path.Combine(contentRootPath, filePath);

            if (!File.Exists(resolvedPath))
            {
                return new List<Entity>();
            }

            var rawJson = File.ReadAllText(resolvedPath);
            if (string.IsNullOrWhiteSpace(rawJson))
            {
                return new List<Entity>();
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var seedFile = JsonSerializer.Deserialize<SeedDataFile>(rawJson, options);
            if (seedFile?.Entities == null)
            {
                return new List<Entity>();
            }

            return seedFile.Entities
                .Where(entity => !string.IsNullOrWhiteSpace(entity.Name))
                .Select(entity => new Entity
                {
                    Id = entity.Id.GetValueOrDefault(Guid.NewGuid()),
                    Name = entity.Name,
                    Description = entity.Description,
                    RegisterDate = entity.RegisterDate.GetValueOrDefault(DateTime.UtcNow)
                })
                .ToList();
        }

        private class SeedDataFile
        {
            public List<SeedEntity> Entities { get; set; }
        }

        private class SeedEntity
        {
            public Guid? Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime? RegisterDate { get; set; }
        }
    }
}
