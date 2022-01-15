# Lab-Code Quality
Lab with simple samples of what you can do to start taking small steps increasing your code quality. E.g using analyzers.

## IDE Configuration
**Goal:** *Share code-formatting and rules cross IDEs.*

In order to get the same formatting etc. cross different IDEs, configuration should be added to the `.editorconfig` file. E.g. formatting, analysis exclusion rules etc.

## Share build properties cross all projects
**Goal:** *Unify build properties cross all projects in the solution, e.g. enabling nullabke reference types*

To have the same rules etc. when it comes to building projects a `Directory.Build.props` is used.

## Use a more restrictive analysis level & Warnings as errors
**Goal:** *Configure the analysis level so that analyzers are more restrictive and treat all warnings as errors.*

Analyzers are enabled by default in .NET 6 projects, but in order to be more restrictive, the level need to be adjusted. Configure the analyzers to use: `recommended` rules instead of the default configured rules [by configuring the](https://docs.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#analysislevel) `AnalysisLevel` property.

In order to act on all warnings, all warnings should be treated as errors. [Enable this using the](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-options/errors-warnings#treatwarningsaserrors) `TreatWarningsAsErrors` property. Explicit deviations can be done by configuring in code or `.editorconfig`.

Provide a `Label` to grant some semantics.

*Directory.build.props*

```xml
<PropertyGroup Label="Code Quality">
  <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  <AnalysisLevel>latest-recommended</AnalysisLevel>
</PropertyGroup>
```

There are more interesting properties to configure depending on what combinations of properties you use. E.g.:

- [OptimizeImplicitlyTriggeredBuild](https://docs.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#optimizeimplicitlytriggeredbuild)
- [CodeAnalysisTreatWarningsAsErrors](https://docs.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#codeanalysistreatwarningsaserrors)

## More Analyzers
**Goal:** *Provide a base line of analyzers that increases the code quality in order to show how you can use both default included and custom analyzers.*

When using .NET6+, by default the [Microsoft.CodeAnalysis.NetAnalyzers](https://github.com/dotnet/roslyn-analyzers#microsoftcodeanalysisnetanalyzers) is included.

Including more analyzers is as easy as adding a NuGet package reference.

### SonarAnalyzer
There's about [380+ rules concerning CSharp](https://rules.sonarsource.com/csharp). You can use it via the [NuGet package](https://www.nuget.org/packages/SonarAnalyzer.CSharp/) or via [Sonars free lint extension](https://www.sonarlint.org/) to e.g. Visual Studio, Visual Studio Code, JetBrains Rider, etc.

```bash
dotnet add package SonarAnalyzer.CSharp
```

## Configure analysis violation severities
**Goal:** *Make the code compile by configuring the sevirity of violated rules.*

The sample code consists of two files:

- ViolatingGreeter.cs
- ViolatingGreeter2.cs

They both contain code that has been commented with what violations they cause.


### Control severity Using .editorconfig
Each violation has been configured (in `.editorconfig`) to have a lower severity which causes the build to succeed.


*ViolatingGreeter.cs*

```csharp
namespace Messy;

// Violates S1118
// Utility classes should not have public constructors
// https://sonarcloud.io/organizations/default/rules?languages=cs&q=S1118
public class ViolatingGreeter
{
    public static string Generate(string name) => $"Hello {(name)}.";
}
```

*.editorconfig*

```yaml
# ROSLYN
# Mark members as static - https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1822
dotnet_diagnostic.CA1822.severity = Suggestion
```

### Inline exlusion
You can also exclude an violation using various inline constructs. [One solution](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/suppress-warnings#use-a-preprocessor-directive) is to make use of pre-processor `#pragma` directive:

*ViolatingGreeter.cs*

```csharp
#pragma warning disable S1118
public class ViolatingGreeter
#pragma warning restore S1118
```

Another solution is to decorate the class using [SupressMessageAttribute](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/suppress-warnings#use-the-suppressmessageattribute):

*ViolatingGreeter.cs*

```csharp
[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1118", MessageId = "Utility classes should not have public constructors")]
public class ViolatingGreeter
{
    //...
}
```

Some IDEs can help you generate both `pragma` and `SupressMessage` rules exclusion. E.g. JetBrains Rider via:

```
Alt + Enter --> context menu --> Inspection: "Add a 'protected constructor' ..." --> Disable with #pragma
```

## Analysis in Azure DevOps using SonarCloud
To be written.

## Code Coverage

**Goal:** *Get an indicator if there are enough tests in the project."

When using the `XUnit` template via `dotnet new` for generating an XUnit test project an reference to `coverlet.collector` is included.

```bash
dotnet new xunit -n UnitTests -o src/UnitTests
```

This provides the possibility to have code coverage data generated for you.

### Code Coverage locally

```bash
dotnet test src/ --collect 'XPlat Code Coverage'
```

This produces `src/UnitTests/TestResults/{guid}/coverage.cobertura.xml`, which isn't human friendly. In order to make sense of it, you can use: [ReportGenerator](https://github.com/danielpalme/ReportGenerator)

Install it as a local `dotnet tool` and run it:

```bash
dotnet new tool-manifest

dotnet tool install dotnet-reportgenerator-globaltool

dotnet reportgenerator -reports:"src/**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:TextSummary
```

*coveragereport/Summary.txt*

```
Summary
  Generated on: 2021-12-22 - 20:02:59
  Parser: CoberturaParser
  Assemblies: 1
  Classes: 2
  Files: 2
  Line coverage: 66.6%
  Covered lines: 2
  Uncovered lines: 1
  Coverable lines: 3
  Total lines: 27

Messy                       66.6%
  Messy.ViolatingGreeter   100.0%
  Messy.ViolatingGreeter2    0.0%
```

### Code Coverage in Azure DevOps
To be written.
