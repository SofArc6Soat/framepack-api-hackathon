using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: TestFramework("Framepack_WebApi.Tests.TestInitialization", "Framepack-WebApi.Tests")]

namespace Framepack_WebApi.Tests;

public class TestInitialization : TestFramework
{
    public TestInitialization(IMessageSink messageSink) : base(messageSink) => System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

    protected override ITestFrameworkDiscoverer CreateDiscoverer(IAssemblyInfo assemblyInfo) => new XunitTestFrameworkDiscoverer(assemblyInfo, SourceInformationProvider, DiagnosticMessageSink);

    protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName) => new XunitTestFrameworkExecutor(assemblyName, SourceInformationProvider, DiagnosticMessageSink);
}