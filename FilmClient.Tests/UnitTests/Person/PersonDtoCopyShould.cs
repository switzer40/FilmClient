using FilmClient.Pages.Person;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FilmClient.Tests.UnitTests.Person
{
    public class PersonDtoCopyShould
    {
        [Fact]
        public void ProduceAFaithfulCopyOfOriginal()
        {
            // Arrange
            var firstMidName = "Julia";
            var lastName = "Roberts";
            var birthdate = "1967-10-28";
            var original = new PersonDto(lastName, birthdate, firstMidName);
            var copy = new PersonDto
                ();

            // Act
            copy.Copy(original);

            // Assert
            Assert.True(copy.Equals(original));
        }
    }
}
