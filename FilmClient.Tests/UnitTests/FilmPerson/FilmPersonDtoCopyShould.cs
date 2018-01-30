using FilmAPI.Common.Constants;
using FilmClient.Pages.FilmPerson;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FilmClient.Tests.UnitTests.FilmPerson
{
    public class FilmPersonDtoCopyShould
    {
        [Fact]
        public void ProduceAFaithfulCopyOfOriginal()
        {
            // Arrange
            var title = "Pretty Woman";
            var year = (short)1990;
            var lastName = "Roberts";
            var birthdate = "1967-10-28";
            var role = FilmConstants.Role_Actor;
            var original = new FilmPersonDto(title,
                                             year,
                                             lastName,
                                             birthdate,
                                             role);
            var copy = new FilmPersonDto();

            //Act
            copy.Copy(original);

            // Assert
            Assert.True(copy.Equals(original));
        }
    }
}
