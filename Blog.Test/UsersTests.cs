using Blog.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Blog.Test
{
    public class UsersTests
    {
        private HttpClient _httpClient;

        public UsersTests()
        {
            var webAppFactory = new CustomWebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }

        [Fact]
        public async Task TestUser_ShouldAddUser()
        {
            User user = new User
            {
                Id = 1,
                Name = "Robert",
                Email = "robert@yahoo.com",
                Password = "dotnet"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Users", stringContent);
            Assert.Contains("Created", response.ReasonPhrase);
        }

        [Fact]
        public async Task TestUser_NameMissing_ShouldNotAddUser()
        {
            User user = new User
            {
                Id = 1,
                Email = "robert@yahoo.com",
                Password = "dotnet"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Users", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Name is required", result);
        }

        [Fact]
        public async Task TestUser_EmailMissing_ShouldNotAddUser()
        {
            User user = new User
            {
                Id = 1,
                Name = "Robert",
                Password = "dotnet"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Users", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Email is required", result);
        }

        [Fact]
        public async Task TestUser_EmailWrongFormat_ShouldNotAddUser()
        {
            User user = new User
            {
                Id = 1,
                Email = "robert",
                Name = "Robert",
                Password = "dotnet"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Users", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid Email Address", result);
        }

        [Fact]
        public async Task TestUser_PasswordMissing_ShouldNotAddUser()
        {
            User user = new User
            {
                Id = 1,
                Email = "robert@yahoo.com",
                Name = "Robert",
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Users", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Password is required", result);
        }

        [Fact]
        public async Task TestUser_PasswordTooShort_ShouldNotAddUser()
        {
            User user = new User
            {
                Id = 1,
                Email = "robert@yahoo.com",
                Name = "Robert",
                Password = "short"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Users", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Password must be at least 6 characters long", result);
        }

        [Fact]
        public async Task TestUser_PasswordTooLong_ShouldNotAddUser()
        {
            User user = new User
            {
                Id = 1,
                Email = "robert@yahoo.com",
                Name = "Robert",
                Password = "thispasswordiswaywaywaytoolongforvalidationdonotvalidatesoreallydonotvaliatestopstops" +
                "reallynowjustdonotvalidate"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Users", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Password must be at least 6 characters long", result);
        }

        [Fact]
        public async Task TestGet_ShouldGetOneAndOnlyUser()
        {
            User user = new User
            {
                Name = "Robert",
                Email = "robert@yahoo.com",
                Password = "dotnet"
            };
            User user2 = new User
            {
                Name = "Denis",
                Email = "denis@yahoo.com",
                Password = "dotnet"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var addPost = await _httpClient.PostAsync("Users", stringContent);
            var stringContent2 = new StringContent(JsonConvert.SerializeObject(user2), Encoding.UTF8, "application/json");
            var addPost2 = await _httpClient.PostAsync("Users", stringContent);
            var get = await _httpClient.GetAsync("Users/1");
            var result = await get.Content.ReadAsStringAsync();
            Assert.Contains("Robert", result);
            Assert.Equal(1, result.Split("{").Length - 1);
        }

        [Fact]
        public async Task TestGet_ShouldGetAllUsers()
        {
            User user = new User
            {
                Name = "Robert",
                Email = "robert@yahoo.com",
                Password = "dotnet"
            };
            User user2 = new User
            {
                Name = "Denis",
                Email = "denis@yahoo.com",
                Password = "dotnet"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var addPost = await _httpClient.PostAsync("Users", stringContent);
            var stringContent2 = new StringContent(JsonConvert.SerializeObject(user2), Encoding.UTF8, "application/json");
            var addPost2 = await _httpClient.PostAsync("Users", stringContent);
            var get = await _httpClient.GetAsync("Users");
            var result = await get.Content.ReadAsStringAsync();
            Assert.Equal(2, result.Split("{").Length - 1);
        }

        [Fact]
        public async Task TestGet_ShouldReturnNotFound()
        {
            var get = await _httpClient.GetAsync("Users/1");
            var result = await get.Content.ReadAsStringAsync();
            Assert.Equal("User with the id 1 does not exist", result);
        }

        [Fact]
        public async Task TestDelete_ShouldReturnNotFound()
        {
            var delete = await _httpClient.DeleteAsync("Users/1");
            var result = await delete.Content.ReadAsStringAsync();
            Assert.Equal("User with the id 1 does not exist", result);
        }

        [Fact]
        public async Task TestDelete_ShouldDeleteUser()
        {
            User user = new User
            {
                Name = "Robert",
                Email = "robert@yahoo.com",
                Password = "dotnet"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Users", stringContent);
            var delete = await _httpClient.DeleteAsync("Users/1");
            var result = await delete.Content.ReadAsStringAsync();
            Assert.Equal("User with the id 1 deleted successfully", result);

            var get = await _httpClient.GetAsync("Users");
            var getResult = await get.Content.ReadAsStringAsync();
            Assert.Equal(0, getResult.Split("{").Length - 1);
        }

        [Fact]
        public async Task TestDeleteAllUsersPosts_ShouldDeleteUser()
        {
            User user = new User
            {
                Name = "Robert",
                Email = "robert@yahoo.com",
                Password = "dotnet"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Users", stringContent);

            Post post1 = new Post
            {
                Id = 1,
                Title = "Game",
                Contents = "Witcher 3",
                Modified_at = DateTime.Now,
                UserId = 1
            };
            var postStringContent = new StringContent(JsonConvert.SerializeObject(post1), Encoding.UTF8, "application/json");
            var postResponse = await _httpClient.PostAsync("Posts", postStringContent);

            Post post2 = new Post
            {
                Id = 1,
                Title = "Game",
                Contents = "Witcher 4",
                Modified_at = DateTime.Now,
                UserId = 1
            };
            var post2StringContent = new StringContent(JsonConvert.SerializeObject(post2), Encoding.UTF8, "application/json");
            var post2Response = await _httpClient.PostAsync("Posts", post2StringContent);

            var delete = await _httpClient.DeleteAsync("Users/1");
            var result = await delete.Content.ReadAsStringAsync();
            Assert.Equal("User with the id 1 deleted successfully", result);

            var get = await _httpClient.GetAsync("Posts");
            var getResult = await get.Content.ReadAsStringAsync();
            Assert.Equal(0, getResult.Split("{").Length - 1);
        }

        [Fact]
        public async Task TestUpdate_ShouldReturnNotFound()
        {
            User user = new User
            {
                Name = "Robert",
                Email = "robert@yahoo.com",
                Password = "dotnet"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("Users/1", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("User with the id 1 does not exist", result);
        }

        [Fact]
        public async Task TestUpdate_NameMissing_ShouldNotUpdateUser()
        {
            User user = new User
            {
                Email = "robert@yahoo.com",
                Password = "dotnet"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("Users/1", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Name is required", result);
        }

        [Fact]
        public async Task TestUpdate_EmailMissing_ShouldNotUpdateUser()
        {
            User user = new User
            {
                Name = "Robert",
                Password = "dotnet"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("Users/1", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Email is required", result);
        }

        [Fact]
        public async Task TestUpdate_EmailWrongFormat_ShouldNotUpdateUser()
        {
            User user = new User
            {
                Name = "Robert",
                Email = "robert",
                Password = "dotnet"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("Users/1", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid Email Address", result);
        }

        [Fact]
        public async Task TestUpdate_PasswordMissing_ShouldNotUpdateUser()
        {
            User user = new User
            {
                Name = "Robert",
                Email = "robert@yahoo.com"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("Users/1", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Password is required", result);
        }

        [Fact]
        public async Task TestUpdate_PasswordTooShort_ShouldNotUpdateUser()
        {
            User user = new User
            {
                Name = "Robert",
                Email = "robert@yahoo.com",
                Password = "nein"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("Users/1", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Password must be at least 6 characters long", result);
        }

        [Fact]
        public async Task TestUpdate_PasswordTooLong_ShouldNotUpdateUser()
        {
            User user = new User
            {
                Name = "Robert",
                Email = "robert@yahoo.com",
                Password = "thispasswordiswaywaywaytoolongforvalidationdonotvalidatesoreallydonotvaliatestopstops" +
                "reallynowjustdonotvalidate"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("Users/1", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Password must be at least 6 characters long", result);
        }

        [Fact]
        public async Task TestUpdate_ShouldUpdateUser()
        {
            User user = new User
            {
                Name = "Robert",
                Email = "robert@yahoo.com",
                Password = "dotnet"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var add = await _httpClient.PostAsync("Users", stringContent);
            User userToUpdate = new User
            {
                Name = "Denis",
                Email = "denis@yahoo.com",
                Password = "dotnet"
            };
            var stringContentUpdate = new StringContent(JsonConvert.SerializeObject(userToUpdate), Encoding.UTF8,
                "application/json");
            var update = await _httpClient.PutAsync("Users/1", stringContentUpdate);
            var result = await update.Content.ReadAsStringAsync();
            Assert.Contains("Denis", result);
            Assert.DoesNotContain("Robert", result);

            var get = await _httpClient.GetAsync("Users/1");
            var getResult = await get.Content.ReadAsStringAsync();
            Assert.Contains("Denis", getResult);
            Assert.DoesNotContain("Robert", getResult);
        }
    }
}
