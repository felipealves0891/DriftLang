using System.Diagnostics;
using Drift.Analyzers.Lexer;
using Drift.Analyzers.Lexer.Reader;
using Drift.Analyzers.Parser;
using Drift.Runtime;
using Drift.Runtime.Contexts;
using Drift.Analyzers.Semantic.Analyzers;
using Drift.Analyzers.Semantic.Diagnostic;
using Drift.Analyzers.Semantic.Rules;
using Drift.Analyzers;

namespace Drift.Compiler;

public class DriftCompiler
{
    public DriftCompiler()
    { } 

    public DiagnosticAggregator? Diagnostic { get; private set; }

    public long Compile(string file)
    {
        var analyzer = new CodeAnalyzer();
        var script = analyzer.Analyze(file);
        if (script is null)
        {
            Diagnostic = analyzer.Diagnostic;
            return -1;
        }
        
        var sw = Stopwatch.StartNew();
        var interpreter = new DriftInterpreter(new DriftExecutionContext(), script);
        interpreter.Interpret();
        sw.Stop();

        return sw.ElapsedMilliseconds;
    }
}
