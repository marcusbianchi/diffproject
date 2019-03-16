using DiffProject.Services.Services;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace DiffProject.Test
{
    public class HashServiceTest
    {
        [Fact]
        public void ShouldReturnHashedMD5StringSmallText()
        {
            var hashedText = "98499B32EF9A462F64CC2F48C76BBBE8";
            var small_1 = File.ReadAllText(@".\TestSamples\Small_1.txt", Encoding.UTF8);
            var hashService = new HashService();
            var hash = hashService.CreateHash(small_1);
            Assert.Equal(hashedText, hash);
        }

        [Fact]
        public void ShouldReturnHashedMD5StringBigText()
        {
            var hashedText = "70A4B9F4707D258F559F91615297A3EC";
            var big_1 = File.ReadAllText(@".\TestSamples\big_1.txt", Encoding.UTF8);
            var hashService = new HashService();
            var hash = hashService.CreateHash(big_1);
            Assert.Equal(hashedText, hash);
        }

        [Fact]
        public void ShouldReturnDiffHashesForDiffTexts()
        {
            var big_2 = File.ReadAllText(@".\TestSamples\big_2.txt", Encoding.UTF8);
            var big_1 = File.ReadAllText(@".\TestSamples\big_1.txt", Encoding.UTF8);
            var hashService = new HashService();
            var hash_2 = hashService.CreateHash(big_2);
            var hash_1 = hashService.CreateHash(big_1);
            Assert.NotEqual(big_1, big_2);
        }
    }
}
