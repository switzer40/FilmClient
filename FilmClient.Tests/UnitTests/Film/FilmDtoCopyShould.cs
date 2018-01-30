using FilmClient.Pages.Film;
using Xunit;

namespace FilmClient.Tests.UnitTests.Film
{
    public class FilmDtoCopyShould
    {
        [Fact]
        public void ProduceFaithfulCopyOfOriginal()
        {
            // Arrange
            var title = "Pretty Woman";
            var year = (short)1990;
            var length = (short)109;
            var original = new FilmDto(title, year, length);
            var copy = new FilmDto();

            // Act
            copy.Copy(original);

            // Assert
            Assert.True(copy.Equals(original));
        }
    }
}
