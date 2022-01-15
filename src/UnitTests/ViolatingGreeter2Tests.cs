using FluentAssertions;
using Messy;
using Xunit;

namespace UnitTests;

public class ViolatingGreeter2Tests
{
    [Fact]
    public void GreetsProperly()
    {
        var sut = new ViolatingGreeter2();

        sut.Generate("Test").Should().Be("Hello 'Test'!");
    }
}
