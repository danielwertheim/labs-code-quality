namespace Messy;

// Violates S1118
// Utility classes should not have public constructors
// https://sonarcloud.io/organizations/default/rules?languages=cs&q=S1118
public class ViolatingGreeter
{
    public static string Generate(string name) => $"Hello {(name)}.";
}
