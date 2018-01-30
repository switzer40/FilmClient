using FilmAPI.Common.Constants;
using FilmClient.Pages.Medium;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FilmClient.Tests.UnitTests.Medium
{
     public class MediumDtoCopyShould
    {
        [Fact]
        public void ProduceAFaithfulCopyOfOriginal()
        {
            // Arrange
            var title = "Pretty Woman";
            var year = (short)1990;
            var mediumType = FilmConstants.MediumType_DVD;
            var location = FilmConstants.Location_Left;
            var original = new MediumDto(title, year, mediumType, location, true);
            var copy = new MediumDto();

            // Act
            copy.Copy(original);

            // Assert
            Assert.True(copy.Equals(original));
        }
    }
}
