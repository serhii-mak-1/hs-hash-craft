using HashCraft.API.Tools.Sha1HashGenerator;
using HashCraft.API.Tools;
using System.Linq;
using HashCraft.API.Tools.Sha1HashGenerator.Exceptions;

namespace HashCraft.UnitTests.Tools;

public class Sha1GeneratorTests
{
    private readonly IHashGenerator _sha1Generator;

    public Sha1GeneratorTests()
    {
        _sha1Generator = new Sha1Generator();
    }

    [Fact]
    public void GenerateHash_ValidInput_HexFormat_ReturnsHashString()
    {
        // Act
        string actualHash = _sha1Generator.GenerateHash(EncodeStyle.Hex);

        // Assert
        Assert.NotNull(actualHash);
        Assert.NotEmpty(actualHash);
        Assert.Equal(40, actualHash.Length);
    }

    [Fact]
    public void GenerateHash_ValidInput_Base64Format_ReturnsHashString()
    {
        // Act
        string actualHash = _sha1Generator.GenerateHash(EncodeStyle.Base64);

        // Assert
        Assert.NotNull(actualHash);
        Assert.NotEmpty(actualHash);
        Assert.Equal(28, actualHash.Length);
    }

    [Fact]
    public void GenerateHash_ValidInput_DigFormat_ReturnsHashString()
    {
        // Act
        string actualHash = _sha1Generator.GenerateHash(EncodeStyle.Dig);

        // Assert
        Assert.NotNull(actualHash);
        Assert.NotEmpty(actualHash);
        Assert.True(actualHash.All(char.IsDigit));
        Assert.Equal(60, actualHash.Length);
    }

    [Fact]
    public void GenerateHash_InvalidEncodeStyle_ThrowsFormatException()
    {
        // Arrange
        EncodeStyle invalidEncode = (EncodeStyle)999;

        // Act & Assert
        Assert.Throws<GenerateHashException>(() => _sha1Generator.GenerateHash(invalidEncode));
    }

    [Fact]
    public void GenerateHashBulk_ValidHexInput_ReturnsArrayOfHashes()
    {
        // Arrange
        int size = 10;

        // Act
        string[] hashes = _sha1Generator.GenerateHashBatch(size, EncodeStyle.Hex);

        // Assert
        Assert.NotNull(hashes);
        Assert.Equal(size, hashes.Length);
        Assert.All(hashes, h => Assert.Equal(40, h.Length));
    }

    [Fact]
    public void GenerateHashBulk_ValidBase64Input_ReturnsArrayOfHashesInBase64()
    {
        // Arrange
        int size = 5;

        // Act
        string[] hashes = _sha1Generator.GenerateHashBatch(size, EncodeStyle.Base64);

        // Assert
        Assert.NotNull(hashes);
        Assert.Equal(size, hashes.Length);
        Assert.All(hashes, h => Assert.Equal(28, h.Length));
    }
}