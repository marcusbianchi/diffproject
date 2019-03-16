using DiffProject.Services.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DiffProject.Test
{
    public class DiferenceAPIIntegrationTest : IClassFixture<WebApplicationFactory<DiffProject.WebAPI.Startup>>
    {
        private readonly WebApplicationFactory<DiffProject.WebAPI.Startup> _factory;
        public DiferenceAPIIntegrationTest(WebApplicationFactory<DiffProject.WebAPI.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async void ShouldAcceptAnyPost()
        {
            // Arrange
            Random rnd = new Random();
            var id = rnd.Next();
            var client = _factory.CreateClient();
            var small_1 = File.ReadAllText(@".\TestSamples\Small_1.txt", Encoding.UTF8);
            var content = new StringContent(small_1, Encoding.UTF8);
            var responsePost = await client.PostAsync("/v1/" + id + "/left", content);
            await Task.Delay(500);
            var responseGet = await (await client.GetAsync("/v1/" + id)).Content.ReadAsStringAsync();
            var  responseBody = JsonConvert.DeserializeObject<ProcessResult>(responseGet);
            Assert.True(responsePost.IsSuccessStatusCode);
            Assert.Equal(StatusEnum.PROCESSED_FIRST, responseBody.status);
        }

        [Fact]
        public async void ShouldRefuseSameIdIfOcupied()
        {
            Random rnd = new Random();
            var id = rnd.Next();
            var client = _factory.CreateClient();
            var small_1 = File.ReadAllText(@".\TestSamples\Small_1.txt", Encoding.UTF8);
            var content = new StringContent(small_1, Encoding.UTF8);
            var responsePostFirst = await client.PostAsync("/v1/" + id + "/left", content);
            await Task.Delay(500);
            var responsePostSecond = await client.PostAsync("/v1/" + id + "/left", content);
            Assert.False(responsePostSecond.IsSuccessStatusCode);
        }

        [Fact]
        public async void ShouldIndicateIfSameSizeAndSameContent()
        {
            Random rnd = new Random();
            var id = rnd.Next();
            var client = _factory.CreateClient();
            var small_1 = File.ReadAllText(@".\TestSamples\Small_1.txt", Encoding.UTF8);
            var content = new StringContent(small_1, Encoding.UTF8);
            var responsePostFirst = await client.PostAsync("/v1/" + id + "/left", content);
            await Task.Delay(500);
            var responsePostSecond = await client.PostAsync("/v1/" + id + "/right", content);
            await Task.Delay(500);
            var responseGet = await (await client.GetAsync("/v1/" + id)).Content.ReadAsStringAsync();
            var responseBody = JsonConvert.DeserializeObject<ProcessResult>(responseGet);            
            Assert.Equal(StatusEnum.DONE, responseBody.status);
            Assert.True(responseBody.IsEqual);
            Assert.True(responseBody.IsEqualSize);
            Assert.Equal(0, responseBody.Differences.Count);
        }

        [Fact]
        public async void ShouldIndicateIfSameSizeAndDiferentContent()
        {
            Random rnd = new Random();
            var id = rnd.Next();
            var client = _factory.CreateClient();
            var small_1 = File.ReadAllText(@".\TestSamples\Small_1.txt", Encoding.UTF8);
            var small_3 = File.ReadAllText(@".\TestSamples\Small_3.txt", Encoding.UTF8);
            var content1 = new StringContent(small_1, Encoding.UTF8);
            var content3 = new StringContent(small_3, Encoding.UTF8);
            var responsePostFirst = await client.PostAsync("/v1/" + id + "/left", content1);
            await Task.Delay(500);
            var responsePostSecond = await client.PostAsync("/v1/" + id + "/right", content3);
            await Task.Delay(500);
            var responseGet = await (await client.GetAsync("/v1/" + id)).Content.ReadAsStringAsync();
            var responseBody = JsonConvert.DeserializeObject<ProcessResult>(responseGet);
            Assert.Equal(StatusEnum.DONE, responseBody.status);
            Assert.False(responseBody.IsEqual);
            Assert.True(responseBody.IsEqualSize);
            Assert.Equal(1, responseBody.Differences.Count);
        }

        [Fact]
        public async void ShouldIndicateIfDiferentSizeAndDiferentContent()
        {
            Random rnd = new Random();
            var id = rnd.Next();
            var client = _factory.CreateClient();
            var small_1 = File.ReadAllText(@".\TestSamples\Small_1.txt", Encoding.UTF8);
            var small_2 = File.ReadAllText(@".\TestSamples\Small_2.txt", Encoding.UTF8);
            var content1 = new StringContent(small_1, Encoding.UTF8);
            var content2 = new StringContent(small_2, Encoding.UTF8);
            var responsePostFirst = await client.PostAsync("/v1/" + id + "/left", content1);
            await Task.Delay(500);
            var responsePostSecond = await client.PostAsync("/v1/" + id + "/right", content2);
            await Task.Delay(500);
            var responseGet = await (await client.GetAsync("/v1/" + id)).Content.ReadAsStringAsync();
            var responseBody = JsonConvert.DeserializeObject<ProcessResult>(responseGet);
            Assert.Equal(StatusEnum.DONE, responseBody.status);
            Assert.False(responseBody.IsEqual);
            Assert.False(responseBody.IsEqualSize);
            Assert.Null(responseBody.Differences);

        }

        [Fact]
        public async void ShouldIndicateIfDifferentSizeAndDiferentContentBigFile()
        {
            Random rnd = new Random();
            var id = rnd.Next();
            var client = _factory.CreateClient();
            var small_1 = File.ReadAllText(@".\TestSamples\big_1.txt", Encoding.UTF8);
            var small_2 = File.ReadAllText(@".\TestSamples\big_2.txt", Encoding.UTF8);
            var content1 = new StringContent(small_1, Encoding.UTF8);
            var content2 = new StringContent(small_2, Encoding.UTF8);
            var responsePostFirst = await client.PostAsync("/v1/" + id + "/left", content1);
            await Task.Delay(500);
            var responsePostSecond = await client.PostAsync("/v1/" + id + "/right", content2);
            await Task.Delay(500);
            var responseGet = await (await client.GetAsync("/v1/" + id)).Content.ReadAsStringAsync();
            var responseBody = JsonConvert.DeserializeObject<ProcessResult>(responseGet);
            Assert.Equal(StatusEnum.DONE, responseBody.status);
            Assert.False(responseBody.IsEqual);
            Assert.False(responseBody.IsEqualSize);
            Assert.Null(responseBody.Differences);

        }

        [Fact]
        public async void ShouldIndicateIfSameSizeAndDiferentContentBigFile()
        {
            Random rnd = new Random();
            var id = rnd.Next();
            var client = _factory.CreateClient();
            var small_1 = File.ReadAllText(@".\TestSamples\big_1.txt", Encoding.UTF8);
            var small_2 = File.ReadAllText(@".\TestSamples\big_3.txt", Encoding.UTF8);
            var content1 = new StringContent(small_1, Encoding.UTF8);
            var content2 = new StringContent(small_2, Encoding.UTF8);
            var responsePostFirst = await client.PostAsync("/v1/" + id + "/left", content1);
            await Task.Delay(2000);
            var responsePostSecond = await client.PostAsync("/v1/" + id + "/right", content2);
            await Task.Delay(2000);
            var responseGet = await (await client.GetAsync("/v1/" + id)).Content.ReadAsStringAsync();
            var responseBody = JsonConvert.DeserializeObject<ProcessResult>(responseGet);
            Assert.Equal(StatusEnum.DONE, responseBody.status);
            Assert.False(responseBody.IsEqual);
            Assert.True(responseBody.IsEqualSize);
            Assert.Equal(2, responseBody.Differences.Count);

        }
    }

}
