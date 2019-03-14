using DiffProject.Services.Interfaces;
using DiffProject.Services.Models;
using DiffProject.Services.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DiifProject.Test
{
    public class ComparisonServiceTest
    {
        [Fact]
        public async void ShouldReturnObjectWithIdAndProperStatus()
        {
            Random rnd = new Random();
            var id = rnd.Next();
            var mockComparisonRepository = new Mock<IComparisonRepository>();
            mockComparisonRepository
                .Setup(x => x.SaveResult(It.IsAny<ProcessResult>()))
                .ReturnsAsync((ProcessResult processResult) =>
                {
                    return new ProcessResult
                    {
                        ProcessResultId = id,
                        status = processResult.status,
                        ContentId = processResult.ContentId
                    };
                });


            var comparisonService = new ComparisonService(mockComparisonRepository.Object);
            var comparisonResult = await comparisonService.CreateNewComparisonAsync("teste");

            Assert.Equal(id, comparisonResult.ProcessResultId);
            Assert.Equal(StatusEnum.NEW, comparisonResult.status);
            Assert.Equal("teste", comparisonResult.ContentId);
        }

        [Fact]
        public async void ShouldUpdateObjectWithProperStatus()
        {
            Random rnd = new Random();
            var id = rnd.Next();
            var mockComparisonRepository = new Mock<IComparisonRepository>();

            mockComparisonRepository
                .Setup(x => x.UpdateResultByContentId(It.IsAny<ProcessResult>(), It.IsAny<string>()))
                .ReturnsAsync((ProcessResult processResult, string contentId) =>
                {
                    return new ProcessResult
                    {
                        ProcessResultId = id,
                        status = processResult.status,
                        ContentId = contentId
                    };
                });

            mockComparisonRepository
               .Setup(x => x.GetResultByContentId(It.IsAny<string>()))
               .ReturnsAsync((string contentId) =>
               {
                   return new ProcessResult
                   {
                       ProcessResultId = id,
                       ContentId = contentId
                   };
               });


            var comparisonService = new ComparisonService(mockComparisonRepository.Object);
            var comparisonResult = await comparisonService.UpdateComparisonToProcessingAsync("teste");

            Assert.Equal(id, comparisonResult.ProcessResultId);
            Assert.Equal(StatusEnum.PROCESSING, comparisonResult.status);
            Assert.Equal("teste", comparisonResult.ContentId);
        }

        [Fact]
        public async void ShouldUpdateObjectWithProperStatusAndCompareValues()
        {
            Random rnd = new Random();
            var id = rnd.Next();
            var mockComparisonRepository = new Mock<IComparisonRepository>();
            mockComparisonRepository
                .Setup(x => x.UpdateResultByContentId(It.IsAny<ProcessResult>(), It.IsAny<string>()))
                .ReturnsAsync((ProcessResult processResult, string contentId) => processResult);
            mockComparisonRepository
               .Setup(x => x.GetResultByContentId(It.IsAny<string>()))
               .ReturnsAsync((string contentId) =>
               {
                   return new ProcessResult
                   {
                       ProcessResultId = id,
                       ContentId = "teste"
                   };
               });
            var itemToProcess = new ItemToProcess
            {
                Hash = "70A4B9F4707D258F559F91615297A3EC",
                Size = 200
            };

            var comparisonService = new ComparisonService(mockComparisonRepository.Object);
            var comparisonResult = await comparisonService.SaveProcessResult(itemToProcess, itemToProcess);

            Assert.Equal(id, comparisonResult.ProcessResultId);
            Assert.Equal(StatusEnum.DONE, comparisonResult.status);
            Assert.Equal("teste", comparisonResult.ContentId);
            Assert.True(comparisonResult.IsEqual);
            Assert.True(comparisonResult.IsEqualSize);
        }
    }
}
