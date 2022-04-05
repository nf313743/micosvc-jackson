using System;
using System.Collections.Generic;
using CommandsService.Models;
using CommandsService.SyncDataService.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient =
                    serviceScope.ServiceProvider.GetRequiredService<IPlatformDataClient>();

                var platforms = grpcClient.ReturnAllPlatorms();

                SeedData(
                    serviceScope.ServiceProvider.GetRequiredService<ICommandRepo>(),
                    platforms
                );
            }
        }

        private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("Seeding new platforms...");

            foreach (var plat in platforms)
            {
                if (!repo.ExternalPlatformExists(plat.ExternalId))
                {
                    repo.CreatePlatform(plat);
                }
            }
            repo.SaveChanges();
        }
    }
}
