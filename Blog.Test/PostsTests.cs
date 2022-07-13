using Blog.Models;
using Microsoft.AspNetCore.Mvc.Testing;
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
                Owner = "Eu"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Posts", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Created", response.ReasonPhrase);
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
                Owner = "Eu"
            };
            Post post2 = new Post
            {
                Id = 1,
                Title = "Game",
                Contents = "Witcher 4",
                Created_at = DateTime.Now,
                Modified_at = DateTime.Now,
                Owner = "Eu"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
            var addPost = await _httpClient.PostAsync("Posts", stringContent);
            var stringContent2 = new StringContent(JsonConvert.SerializeObject(post2), Encoding.UTF8, "application/json");
            var addPost2 = await _httpClient.PostAsync("Posts", stringContent);
            var get = await _httpClient.GetAsync("Posts/1");
            var result = await get.Content.ReadAsStringAsync();
            Assert.Contains("Witcher 3", result);
            Assert.Equal(1, result.Split("{").Length - 1);
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
                Owner = "Eu"
            };
            Post post2 = new Post
            {
                Id = 1,
                Title = "Game",
                Contents = "Witcher 4",
                Created_at = DateTime.Now,
                Modified_at = DateTime.Now,
                Owner = "Eu"
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
                Owner = "Eu"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Posts", stringContent);
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
                Owner = "Eu"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("Posts/1", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Post with the id 1 does not exist", result);
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
                Owner = "Eu"
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
                Owner = "Eu"
            };
            var stringContentUpdate = new StringContent(JsonConvert.SerializeObject(postUpdate), Encoding.UTF8, "application/json");
            var update = await _httpClient.PutAsync("Posts/1", stringContentUpdate);
            var result = await update.Content.ReadAsStringAsync();
            Assert.Equal("Post with the id 1 updated successfully", result);

            var get = await _httpClient.GetAsync("Posts/1");
            var getResult = await get.Content.ReadAsStringAsync();
            Assert.Contains("Witcher 4", getResult);
            Assert.DoesNotContain("Witcher 3", getResult);
        }
    }
}