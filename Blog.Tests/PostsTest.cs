using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;

namespace Blog.Tests
{
    public class PostsTest
    {
        [Fact]
        public async Task Test1()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var response = await httpClient.GetAsync("");
            var result = await response.Content.ReadAsStringAsync();
        }
    }
}