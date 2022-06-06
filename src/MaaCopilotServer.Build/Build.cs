using System.Diagnostics.CodeAnalysis;
using Nuke.Common;
using Nuke.Common.Execution;
using Serilog;

[CheckBuildProjectConfigurations]
class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Default);

    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    Target Default => _ => _
        .Executes(() =>
        {
            Log.Error("请指定一个 Target");
        });
}
