using System;
using System.Net.Http;
using System.Text.Json;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using StorySpoiler.Models;
using System.Net;
using System.Linq;
using System.ComponentModel;


namespace StorySpoiler
{
    [TestFixture]
    public class StoryTests
    {
        private RestClient client;
        private const string baseUrl = "https://d3s5nxhwblsjbi.cloudfront.net";
        private static string createdStoryId;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            string token = JwtToken("Liily77", "123456");

            var options = new RestClientOptions(baseUrl)
            {
                Authenticator = new JwtAuthenticator(token)
            }; 
            client = new RestClient(options);
        }

        private string JwtToken(string username, string password)
        {
            var loginClient = new RestClient(baseUrl);
            var request = new RestRequest("/api/User/Authentication", Method.Post);
            request.AddJsonBody(new { username, password } );

            var response = loginClient.Execute(request);

            var json = JsonSerializer.Deserialize<JsonElement>(response.Content);

            return json.GetProperty("accessToken").GetString();
        }

        [Order(1)]
        [Test]
        public void GetStory() {

            var sortdata = new StoryDTO
            {
                Title = "Title",
                Description = "Description",
                Url = ""
            };

            var reguest = new RestRequest("/api/Story/Create", Method.Post); 
            reguest.AddJsonBody(sortdata);
            var response = this.client.Execute(reguest);

            Console.WriteLine("Raw JSON:");
            Console.WriteLine(response.Content);

            var createdResponse = JsonSerializer.Deserialize<APIResponseDTO>(response.Content);

            if (createdResponse == null)
            {
                Console.WriteLine("DTO is null!");
            }
            else
            {
                Console.WriteLine($"Id: {createdResponse.Id}, Msg: {createdResponse.Msg}");
            }

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(createdResponse.Id, Is.Not.Null);
            Assert.That(createdResponse.Msg, Is.EqualTo("Successfully created!"));

            createdStoryId = createdResponse.Id;
            Console.WriteLine(createdStoryId);

        }

        [Order(2)]
        [Test]
        public void EditTheStorySpoiler()
        {

            var editData = new StoryDTO
            {
                Title = "Edited title",
                Description = "Edited description",
                Url = ""
            };

            var reguest = new RestRequest($"/api/Story/Edit/{createdStoryId}", Method.Put);
            reguest.AddJsonBody(editData);

           var response = this.client.Execute(reguest);

            var editedResponse = JsonSerializer.Deserialize<APIResponseDTO>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(editedResponse.Msg, Is.EqualTo("Successfully edited"));

        }

        [Order(3)]
        [Test]
        public void GetAllStorySpoilers() 
        {
            var request = new RestRequest("/api/Story/All", Method.Get);

            var response = this.client.Execute(request);

            var responseItems = JsonSerializer.Deserialize<List<APIResponseDTO>>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(responseItems, Is.Not.Null);
            Assert.That(responseItems, Is.Not.Empty);

        }

        [Order(4)]
        [Test]
        public void DeleteStorySpoiler()
        {
            var request = new RestRequest($"/api/Story/Delete/{createdStoryId}", Method.Delete);

            var response = this.client.Execute(request);

            var deleteResponse = JsonSerializer.Deserialize<APIResponseDTO>(response.Content);
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(deleteResponse.Msg, Is.EqualTo("Deleted successfully!"));

                    }

        [Order(5)]
        [Test]
        public void CreatestorySpoilerWitoutRequiredFields() 
        {
            var createData = new StoryDTO
            {
                Title = "",
                Description = "",
                Url = ""
            };

            var reguest = new RestRequest("/api/Story/Create", Method.Post);
            reguest.AddJsonBody(createData);

            var response = this.client.Execute(reguest);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

        }

        [Order(6)]
        [Test]
        public void EditNonExistingStorySpoiler()
        {
            string fakeId = "123";
            var editData = new StoryDTO
            {
                Title = "Edited title",
                Description = "Edited description",
                Url = ""
            };

            var reguest = new RestRequest($"/api/Story/Edit/{fakeId}", Method.Put);
            reguest.AddJsonBody(editData);

            var response = this.client.Execute(reguest);
            var deleteResponse = JsonSerializer.Deserialize<APIResponseDTO>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

           Assert.That(deleteResponse.Msg, Does.Contain("No spoilers..."));
        }

        [Order(7)]
        [Test]
        public void DeleteNonExistingStorySpoiler() 
        {
            string fakeId = "123";
            var request = new RestRequest($"/api/Story/Delete/{fakeId}", Method.Put);

            var response = this.client.Execute(request);

            var deleteResponse = JsonSerializer.Deserialize<APIResponseDTO>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(deleteResponse.Msg, Is.EqualTo("Unable to delete this story spoiler!"));

        }

        

        [OneTimeTearDown]
        public void OneTimeTearDown() { 
         
            client?.Dispose();
        
        }
    }
}