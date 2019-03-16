using DiffProject.Services.Services;
using Xunit;

namespace DiffProject.Test
{
    public class DiffenceSearchServiceTeste
    {
        [Fact]
        public void ShouldReturnNoDiffIfEquals()
        {
            var text1 = "teste";
            var diffService = new DiffenceSearchService();
            var diffs = diffService.GetDifferences(text1, text1, text1.Length);
            Assert.Equal(0, diffs.Count);
        }

        [Fact]
        public void ShouldReturnOneDiffIf()
        {
            var text1 = "teste1";
            var text2 = "teste2";
            var diffService = new DiffenceSearchService();
            var diffs = diffService.GetDifferences(text1, text2, text1.Length);
            Assert.Equal(1, diffs.Count);
            Assert.Equal(5,diffs[0].DifferenceStartPosition);
            Assert.Equal(1, diffs[0].DifferenceSize);
        }

        [Fact]
        public void ShouldReturnTwoDiffIf()
        {
            var text1 = "taste1";
            var text2 = "teste2";
            var diffService = new DiffenceSearchService();
            var diffs = diffService.GetDifferences(text1, text2, text1.Length);
            Assert.Equal(2, diffs.Count);
            Assert.Equal(1, diffs[0].DifferenceStartPosition);
            Assert.Equal(1, diffs[0].DifferenceSize);
            Assert.Equal(5, diffs[1].DifferenceStartPosition);
            Assert.Equal(1, diffs[1].DifferenceSize);
        }
    }
}
