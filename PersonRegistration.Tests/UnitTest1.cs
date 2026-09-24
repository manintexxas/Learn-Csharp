using System;
using Xunit;
using SmallApp;

namespace SmallApp.Tests
{
public class StringExtensionsTests
{
#region 1. Pengujian IsNull & IsValidEmail

    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData("   ", true)]
    [InlineData("Halo", false)]
    public void IsNull_ChecksEmptyOrWhitespaceText_ReturnsExpectedResult(string? input, bool expectedResult)
    {
        // ACT
        bool result = input.IsNull();

        // ASSERT
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("budi@gmail.com", true)]
    [InlineData("budi.doremi@domain.co.id", true)]
    [InlineData("budi-gmail.com", false)] // Tanpa simbol @
    [InlineData("", false)]              // Teks kosong
    [InlineData(null, false)]            // Nilai null
    public void IsValidEmail_ValidatesEmailFormat_ReturnsValidationStatus(string? email, bool expectedResult)
    {
        // ACT
        bool result = email.IsValidEmail();

        // ASSERT
        Assert.Equal(expectedResult, result);
    }

    #endregion

    #region 2. Pengujian ToTitleCase & TextToNumber

    [Fact]
    public void ToTitleCase_InputNameWithLowercaseOrMixedCase_ConvertsToCapitalizedWords()
    {
        // ARRANGE
        string input = "jOHn dOE";

        // ACT
        string result = input.ToTitleCase();

        // ASSERT
        Assert.Equal("John Doe", result);
    }

    [Theory]
    [InlineData("25", 25)]
    [InlineData(" 100 ", 100)] // Dengan spasi tambahan
    [InlineData("abc", 0)]      // Bukan angka (anomali)
    [InlineData(null, 0)]       // Input null
    public void TextToNumber_ConvertsTextToNumber_ReturnsInteger(string? input, int expectedNumber)
    {
        // ACT
        int result = input.TextToNumber();

        // ASSERT
        Assert.Equal(expectedNumber, result);
    }

    #endregion
}

public class PersonTests
{
    #region 1. Normal Case (Skenario Pendaftaran Berhasil)

    [Fact]
    public void TryCreate_ValidInput_SuccessfullyCreatesPersonObject()
    {
        // ARRANGE (Persiapan data registrasi yang sah)
        var request = new RegisterPersonRequest
        {
            FullName = "budi santoso",
            RawAge = "25",
            Email = "budi@example.com"
        };

        // ACT (Menjalankan fungsi TryCreate)
        bool isSuccess = Person.TryCreate(request, out Person? person, out string? errorMessage);

        // ASSERT (Memastikan hasil sesuai yang diharapkan)
        Assert.True(isSuccess);
        Assert.NotNull(person);
        Assert.Null(errorMessage);
        Assert.Equal("Budi Santoso", person!.FullName); // Nama harus sudah diubah ke Title Case
        Assert.Equal(25, person.Age);
        Assert.Equal("budi@example.com", person.Email);
        Assert.NotEqual(Guid.Empty, person.Id); // ID otomatis tergenerasi
    }

    #endregion

    #region 2. Boundary Case (Skenario Batas Usia)

    [Theory]
    [InlineData("0")]   // Usia batas bawah (terlalu muda)
    [InlineData("-1")]  // Usia minus
    [InlineData("100")] // Usia batas atas (terlalu tua)
    [InlineData("105")] // Usia melampaui batas
    public void TryCreate_AgeOutOfRange_FailsToCreatePerson(string rawAge)
    {
        // ARRANGE
        var request = new RegisterPersonRequest
        {
            FullName = "Siti",
            RawAge = rawAge,
            Email = "siti@example.com"
        };

        // ACT
        bool isSuccess = Person.TryCreate(request, out Person? person, out string? errorMessage);

        // ASSERT
        Assert.False(isSuccess);
        Assert.Null(person);
        Assert.Equal("Invalid Age", errorMessage);
    }

    #endregion

    #region 3. Anomali Input (Skenario Input Null / Rusak)

    [Fact]
    public void TryCreate_NullRequest_FailsAndReturnsErrorMessage()
    {
        // ACT
        bool isSuccess = Person.TryCreate(null, out Person? person, out string? errorMessage);

        // ASSERT
        Assert.False(isSuccess);
        Assert.Null(person);
        Assert.Equal("Request must have value (Not Null)", errorMessage);
    }

    [Fact]
    public void TryCreate_EmptyOrWhitespaceName_FailsToCreatePerson()
    {
        // ARRANGE
        var request = new RegisterPersonRequest
        {
            FullName = "   ", // Teks spasi saja
            RawAge = "20",
            Email = "email@example.com"
        };

        // ACT
        bool isSuccess = Person.TryCreate(request, out Person? person, out string? errorMessage);

        // ASSERT
        Assert.False(isSuccess);
        Assert.Null(person);
        Assert.Equal("Invalid Name", errorMessage);
    }

    [Fact]
    public void TryCreate_InvalidEmail_FailsToCreatePerson()
    {
        // ARRANGE
        var request = new RegisterPersonRequest
        {
            FullName = "Andi",
            RawAge = "20",
            Email = "andi-tanpa-domain"
        };

        // ACT
        bool isSuccess = Person.TryCreate(request, out Person? person, out string? errorMessage);

        // ASSERT
        Assert.False(isSuccess);
        Assert.Null(person);
        Assert.Equal("Invalid Email", errorMessage);
    }

    #endregion
}

}
