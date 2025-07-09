using System.Diagnostics;
using Drift.Compiler;
using Drift.Core;
using Drift.Core.Nodes;
using Drift.Lexer;
using Drift.Lexer.Reader;
using Drift.Parser;
using Drift.Runtime;
using Drift.Semantic.Analyzers;
using Drift.Semantic.Rules;
using Serilog;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(@"D:\Source\Scripts\logs\execution.txt", rollingInterval: RollingInterval.Minute)
            .CreateLogger();
try
{
    var compiler = new DriftCompiler();
    var executionTime = compiler.Compile(@"D:\Source\Scripts\main_old.dft");

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

