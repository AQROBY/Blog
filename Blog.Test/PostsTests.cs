using Blog.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Blog.Test
{
    public class PostsTests
    {
        private HttpClient _httpClient;

        public PostsTests()
        {
            var webAppFactory = new CustomWebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }

        User user = new User { Id = 1, Name = "Robert", Email = "robert@yahoo.com", Password = "pass" };

        [Fact]
        public async Task TestPost_ShouldAddPost()
        {
            Post post = new Post
            {
                Id = 1,
                Title = "Game",
                Contents = "Witcher 3",
                Created_at = DateTime.Now,
                Modified_at = DateTime.Now,
                UserId = user.Id
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Posts", stringContent);
            Assert.Contains("Created", response.ReasonPhrase);
        }

        [Fact]
        public async Task TestPost_TitleMissing_ShouldNotAddPost()
        {
            Post post = new Post
            {
                Id = 1,
                Contents = "Witcher 3",
                Created_at = DateTime.Now,
                Modified_at = DateTime.Now,
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Posts", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Title field is required", result);
        }

        [Fact]
        public async Task TestPost_ContentMissing_ShouldNotAddPost()
        {
            Post post = new Post
            {
                Id = 1,
                Title = "Game",
                Created_at = DateTime.Now,
                Modified_at = DateTime.Now,
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Posts", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Contents field is required", result);
        }

        [Fact]
        public async Task TestGet_ShouldGetOneAndOnlyPost()
        {
            Post post = new Post
            {
                Id = 1,
                Title = "Game",
                Contents = "Witcher 3",
                Created_at = DateTime.Now,
                Modified_at = DateTime.Now,
            };
            Post post2 = new Post
            {
                Id = 1,
                Title = "Game",
                Contents = "Witcher 4",
                Created_at = DateTime.Now,
                Modified_at = DateTime.Now,
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
            var addPost = await _httpClient.PostAsync("Posts", stringContent);
            Assert.Contains("Created", addPost.ReasonPhrase);
            var stringContent2 = new StringContent(JsonConvert.SerializeObject(post2), Encoding.UTF8, "application/json");
            var addPost2 = await _httpClient.PostAsync("Posts", stringContent2);
            Assert.Contains("Created", addPost2.ReasonPhrase);

            var get = await _httpClient.GetAsync("Posts/1");
            var result = await get.Content.ReadAsStringAsync();
            Assert.Contains("Witcher 3", result);
            Assert.DoesNotContain("Witcher 4", result);
        }

        [Fact]
        public async Task TestGet_ShouldGetAllPosts()
        {
            Post post = new Post
            {
                Id = 1,
                Title = "Game",
                Contents = "Witcher 3",
                Created_at = DateTime.Now,
                Modified_at = DateTime.Now,
            };
            Post post2 = new Post
            {
                Id = 1,
                Title = "Game",
                Contents = "Witcher 4",
                Created_at = DateTime.Now,
                Modified_at = DateTime.Now,
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
            var addPost = await _httpClient.PostAsync("Posts", stringContent);
            var stringContent2 = new StringContent(JsonConvert.SerializeObject(post2), Encoding.UTF8, "application/json");
            var addPost2 = await _httpClient.PostAsync("Posts", stringContent);
            var get = await _httpClient.GetAsync("Posts");
            var result = await get.Content.ReadAsStringAsync();
            Assert.Equal(2, result.Split("{").Length - 1);
        }

        [Fact]
        public async Task TestGet_ShouldReturnNotFound()
        {
            var get = await _httpClient.GetAsync("Posts/1");
            var result = await get.Content.ReadAsStringAsync();
            Assert.Equal("Post with the id 1 does not exist", result);
        }

        [Fact]
        public async Task TestDelete_ShouldReturnNotFound()
        {
            var delete = await _httpClient.DeleteAsync("Posts/1");
            var result = await delete.Content.ReadAsStringAsync();
            Assert.Equal("Post with the id 1 does not exist", result);
        }

        [Fact]
        public async Task TestDelete_ShouldDeletePost()
        {
            Post post = new Post
            {
                Id = 1,
                Title = "Game",
                Contents = "Witcher 3",
                Created_at = DateTime.Now,
                Modified_at = DateTime.Now,
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Posts", stringContent);
            Assert.Contains("Created", response.ReasonPhrase);

            var delete = await _httpClient.DeleteAsync("Posts/1");
            var result = await delete.Content.ReadAsStringAsync();
            Assert.Equal("Post with the id 1 deleted successfully", result);

            var get = await _httpClient.GetAsync("Posts");
            var getResult = await get.Content.ReadAsStringAsync();
            Assert.Equal(0, getResult.Split("{").Length - 1);
        }

        [Fact]
        public async Task TestUpdate_ShouldReturnNotFound()
        {
            Post post = new Post
            {
                Id = 1,
                Title = "Game",
                Contents = "Witcher 3",
                Created_at = DateTime.Now,
                Modified_at = DateTime.Now,
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("Posts/1", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Post with the id 1 does not exist", result);
        }

        [Fact]
        public async Task TestUpdate_TitleMissing_ShouldNotUpdatePost()
        {
            Post post = new Post
            {
                Id = 1,
                Contents = "Witcher 3",
                Created_at = DateTime.Now,
                Modified_at = DateTime.Now,
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("Posts/1", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Title field is required", result);
        }

        [Fact]
        public async Task TestUpdate_ContentMissing_ShouldNotUpdatePost()
        {
            Post post = new Post
            {
                Id = 1,
                Title = "Game",
                Created_at = DateTime.Now,
                Modified_at = DateTime.Now,
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("Posts/1", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Contents field is required", result);
        }

        [Fact]
        public async Task TestUpdate_ShouldUpdatePost()
        {
            Post post = new Post
            {
                Id = 1,
                Title = "Game",
                Contents = "Witcher 3",
                Created_at = DateTime.Now,
                Modified_at = DateTime.Now,
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
            var add = await _httpClient.PostAsync("Posts", stringContent);
            Post postUpdate = new Post
            {
                Id = 1,
                Title = "Game",
                Contents = "Witcher 4",
                Created_at = DateTime.Now,
                Modified_at = DateTime.Now,
            };
            var stringContentUpdate = new StringContent(JsonConvert.SerializeObject(postUpdate), Encoding.UTF8, "application/json");
            var update = await _httpClient.PutAsync("Posts/1", stringContentUpdate);
            var result = await update.Content.ReadAsStringAsync();
            Assert.Contains("Witcher 4", result);
            Assert.DoesNotContain("Witcher 3", result);

            var get = await _httpClient.GetAsync("Posts/1");
            var getResult = await get.Content.ReadAsStringAsync();
            Assert.Contains("Witcher 4", getResult);
            Assert.DoesNotContain("Witcher 3", getResult);
        }
    }
}