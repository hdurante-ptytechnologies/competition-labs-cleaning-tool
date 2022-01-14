using CompetitionLabsCleaningTool.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CompetitionLabsCleaningTool.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IApiClient _apiClient;
        private readonly IConfigurationRoot _configuration;

        public ProductsService(IApiClient apiClient, IConfigurationRoot configuration)
        {
            _apiClient = apiClient;
            _configuration = configuration;
        }

        public async Task DeleteAll()
        {
            try
            {
                var spaces = _configuration.GetSection("CompetitionLabsHttpClientSettings:spaces").Get<CompetiotionLabsSpace[]>().ToList();
                foreach (var space in spaces)
                {
                    _apiClient.SetApiKey(space.Key);
                    var productIds = await _apiClient.Get(space, 700, 0);
                    if (productIds.Count > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine();
                        Console.WriteLine($"Deleting {productIds.Count} products from {space.Name}");
                        Console.WriteLine();
                        int i = 0;
                        foreach (var id in productIds)
                        {
                            var res = await _apiClient.Delete(space, id);
                            if (res)
                            {
                                i++;
                                Console.Write("\r{0}   ", productIds.Count - i);
                            }
                        }
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success!");
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
            }

        }
    }
}
