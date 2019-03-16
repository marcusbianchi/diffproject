using DiffProject.Services.Interfaces;
using DiffProject.Services.Models;
using DiffProject.Services.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DiffProject.Test
{
    public class ProcessDataServiceTest
    {

        [Fact]
        public async void ShouldReturnTrueIfIsNewDocument()
        {
            Random rnd = new Random();
            var id = rnd.Next();
            var mockItemToProcessRepository = new Mock<IItemToProcessRepository>();
            var mockHashService = new Mock<IHashService>();
            mockHashService.Setup(x => x.CreateHash(It.IsAny<string>())).Returns("70A4B9F4707D258F559F91615297A3EC");
            var mockComparisonService = new Mock<IComparisonService>();
            mockItemToProcessRepository.Setup(x => x.GetDataFromDbById(It.IsAny<String>())).ReturnsAsync(() => null);
            var mockomparisonRepository = new Mock<IProcessResultRepository>();
            mockItemToProcessRepository.Setup(x => x.GetDataFromDbById(It.IsAny<String>())).ReturnsAsync(() => null);
            var processDataService = new ProcessDataService(mockItemToProcessRepository.Object, mockHashService.Object, mockComparisonService.Object, mockomparisonRepository.Object);
            var result = await processDataService.ProcessDataAsync("Teste", "testeID", "left");

            Assert.True(result);
        }

        [Fact]
        public async void ShouldReturnTrueIfIsDiferentSideDocument()
        {
            Random rnd = new Random();
            var id = rnd.Next();
            var mockItemToProcessRepository = new Mock<IItemToProcessRepository>();
            var mockHashService = new Mock<IHashService>();
            mockHashService.Setup(x => x.CreateHash(It.IsAny<string>())).Returns("70A4B9F4707D258F559F91615297A3EC");
            var mockComparisonService = new Mock<IComparisonService>();
            mockItemToProcessRepository.Setup(x => x.GetDataFromDbById(It.IsAny<String>())).ReturnsAsync(() => new ItemToProcess
            {
                ContentId = "Teste",
                Direction = "right",
                ItemToProcessId = id
            });
            var mockomparisonRepository = new Mock<IProcessResultRepository>();
            var processDataService = new ProcessDataService(mockItemToProcessRepository.Object, mockHashService.Object, mockComparisonService.Object, mockomparisonRepository.Object);
            var result = await processDataService.ProcessDataAsync("Teste", "testeID", "left");

            Assert.True(result);
        }

        [Fact]
        public async void ShouldReturnFalseIfIsSameSideDocument()
        {
            Random rnd = new Random();
            var id = rnd.Next();
            var mockItemToProcessRepository = new Mock<IItemToProcessRepository>();
            var mockHashService = new Mock<IHashService>();
            mockHashService.Setup(x => x.CreateHash(It.IsAny<string>())).Returns("70A4B9F4707D258F559F91615297A3EC");
            var mockComparisonService = new Mock<IComparisonService>();
            mockItemToProcessRepository.Setup(x => x.GetDataFromDbById(It.IsAny<String>())).ReturnsAsync(() =>
            new ItemToProcess
            {
                ContentId = "Teste",
                Direction = "left",
                ItemToProcessId = id
            });
            var mockomparisonRepository = new Mock<IProcessResultRepository>();
            mockomparisonRepository.Setup(x => x.GetResultByContentId(It.IsAny<string>())).Returns(() =>new ProcessResult
            {
                ContentId = "Teste"
            });
            var processDataService = new ProcessDataService(mockItemToProcessRepository.Object, mockHashService.Object, mockComparisonService.Object, mockomparisonRepository.Object);
            var result = await processDataService.ProcessDataAsync("Teste", "testeID", "left");

            Assert.False(result);
        }
    }
}
