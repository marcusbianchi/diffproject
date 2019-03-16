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
    public class ComparisonServiceTest
    {
        [Fact]
        public  void ShouldReturnObjectWithIdAndProperStatus()
        {
            Random rnd = new Random();
            var id = rnd.Next();
            var mockProcessResultRepository = new Mock<IProcessResultRepository>();
            mockProcessResultRepository
                .Setup(x => x.SaveResult(It.IsAny<ProcessResult>()))
                .Returns((ProcessResult processResult) =>
                {
                    return new ProcessResult
                    {
                        ProcessResultId = id,
                        status = processResult.status,
                        ContentId = processResult.ContentId
                    };
                });
            var mockDiffenceSearchService = new Mock<IDiffenceSearchService>();
            var comparisonService = new ComparisonService(mockProcessResultRepository.Object, mockDiffenceSearchService.Object);
            var comparisonResult =  comparisonService.CreateNewComparison("teste");
            Assert.Equal(id, comparisonResult.ProcessResultId);
            Assert.Equal(StatusEnum.NEW, comparisonResult.status);
            Assert.Equal("teste", comparisonResult.ContentId);
        }

        [Fact]
        public  void ShouldUpdateObjectWithProperStatus()
        {
            Random rnd = new Random();
            var id = rnd.Next();
            var mockProcessResultRepository = new Mock<IProcessResultRepository>();
            mockProcessResultRepository
                .Setup(x => x.UpdateResultByContentId(It.IsAny<ProcessResult>(), It.IsAny<string>()))
                .Returns((ProcessResult processResult, string contentId) =>
                {
                    return new ProcessResult
                    {
                        ProcessResultId = id,
                        status = processResult.status,
                        ContentId = contentId
                    };
                });
            mockProcessResultRepository
               .Setup(x => x.GetResultByContentId(It.IsAny<string>()))
               .Returns((string contentId) =>
               {
                   return new ProcessResult
                   {
                       ProcessResultId = id,
                       ContentId = contentId
                   };
               });
            var mockDiffenceSearchService = new Mock<IDiffenceSearchService>();
            var comparisonService = new ComparisonService(mockProcessResultRepository.Object, mockDiffenceSearchService.Object);
            var comparisonResult = comparisonService.UpdateComparisonToProcessing("teste",StatusEnum.PROCESSED_FIRST);
            Assert.Equal(id, comparisonResult.ProcessResultId);
            Assert.Equal(StatusEnum.PROCESSED_FIRST, comparisonResult.status);
            Assert.Equal("teste", comparisonResult.ContentId);
        }

        [Fact]
        public  void ShouldUpdateObjectWithProperStatusAndCompareValues()
        {
            Random rnd = new Random();
            var id = rnd.Next();
            var mockComparisonRepository = new Mock<IProcessResultRepository>();
            mockComparisonRepository
                .Setup(x => x.UpdateResultByContentId(It.IsAny<ProcessResult>(), It.IsAny<string>()))
                .Returns((ProcessResult processResult, string contentId) => processResult);
            mockComparisonRepository
               .Setup(x => x.GetResultByContentId(It.IsAny<string>()))
               .Returns((string contentId) =>
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
            var mockDiffenceSearchService = new Mock<IDiffenceSearchService>();

            var comparisonService = new ComparisonService(mockComparisonRepository.Object, mockDiffenceSearchService.Object);
            var comparisonResult =  comparisonService.SaveProcessResult(itemToProcess, itemToProcess);

            Assert.Equal(id, comparisonResult.ProcessResultId);
            Assert.Equal(StatusEnum.DONE, comparisonResult.status);
            Assert.Equal("teste", comparisonResult.ContentId);
            Assert.True(comparisonResult.IsEqual);
            Assert.True(comparisonResult.IsEqualSize);
        }
    }
}
