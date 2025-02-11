using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using Xunit.Abstractions;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using ArchUnitNET.Fluent.Syntax.Elements.Types;

namespace RiverBooks.OrderProcessing.Tests;

public class InfrastructureDependencyTest(ITestOutputHelper outputHelper)
{
    private static readonly Architecture  Architecture = new ArchLoader().LoadAssemblies(typeof(AssemblyInfo).Assembly).Build();
    private readonly ITestOutputHelper _outputHelper = outputHelper;

    [Fact]
    public void DomainTypesShouldNotRefereneceInfrastructure()
    {
        var domainTypes = Types().That()
            .ResideInNamespace("RiverBooks.OrderProcessing.Domain.*", useRegularExpressions: true)
            .As("OrderProcessing Domain Types");

        var infraTypes = Types().That()
            .ResideInNamespace("RiverBooks.OrderProcessing.Infrastructure", useRegularExpressions: true)
            .As("OrderProcessing Infrastructure Type");

        var rule = domainTypes.Should().NotDependOnAny(infraTypes);

        // PrintTypes(domainTypes, infraTypes);

        rule.Check(Architecture);
    }

    /// <summary>
    /// Used for debugging purposes
    /// </summary>
    /// <param name="domainTypes"></param>
    /// <param name="infraTypes"></param>
    private void PrintTypes(GivenTypesConjunctionWithDescription domainTypes, GivenTypesConjunctionWithDescription infraTypes)
    {
        foreach (var domainClass in domainTypes.GetObjects(Architecture))
        {
            _outputHelper.WriteLine($"Domain Type: {domainClass.FullName}");
            foreach (var dependency in domainClass.Dependencies)
            {
                var targetType = dependency.Target;
                if(infraTypes.GetObjects(Architecture).Any(infraClass => infraClass.Equals(targetType)))
                {
                    _outputHelper.WriteLine($"  Depends on Infrastructure: {targetType.FullName}");
                }
            }
        }

        foreach (var iType in infraTypes.GetObjects(Architecture))
        {
            _outputHelper.WriteLine($"Infrastructure Type: {iType.FullName}");
        }
    }
}