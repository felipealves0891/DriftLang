using Serilog;
using Drift.Runtime.StackFrame;
using Drift.Analyzers.Core;
using Drift.Compiler;
using Drift.Analyzers.Core.Nodes;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(@"D:\Source\Scripts\logs\execution.txt", rollingInterval: RollingInterval.Minute)
            .CreateLogger();

DriftEnv.StackFrame = DriftStackFrame.StackFrame;

try
{
    var compiler = new DriftCompiler();
    var executionTime = compiler.Compile(@"D:\Source\Scripts\main.dft");

    Console.WriteLine("Execution Time: {0}", executionTime);
    Console.WriteLine(compiler.Diagnostic);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    foreach (var frame in DriftEnv.StackFrame)
    {
        var identifier = frame is IIdentifier withIdentifier ? withIdentifier.Identifier : "...";
        Console.WriteLine(" at {0}: {1}", identifier, frame.Location);
    }
}

