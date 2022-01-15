namespace Messy;
public class ViolatingGreeter2
{
    // Violates S125
    // Sections of code should not be commented out
    // https://sonarcloud.io/organizations/default/rules?open=csharpsquid%3AS125&rule_key=csharpsquid%3AS125
    // private static string Faker() => "Faker baker";

    // Violates S3400
    // Methods should not return constants
    // https://sonarcloud.io/organizations/default/rules?open=csharpsquid%3AS3400&rule_key=csharpsquid%3AS3400
    private static string GetGreeting() => "Hello";

    // Violates CA1822
    // Mark members as static
    // https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1822
    public string Generate(string name) => $"{GetGreeting()} '{(name)}'!";
}
