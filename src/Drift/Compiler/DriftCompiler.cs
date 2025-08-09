using System;
using System.Diagnostics;
using Drift.Analyzers.Lexer;
using Drift.Analyzers.Lexer.Reader;
using Drift.Analyzers.Parser;
using Drift.Runtime;
using Drift.Runtime.Contexts;
using Drift.Analyzers.Semantic;
using Drift.Analyzers.Semantic.Analyzers;
using Drift.Analyzers.Semantic.Diagnostic;
using Drift.Analyzers.Semantic.Rules;

namespace Drift.Compiler;

public class DriftCompiler
{
    private readonly DefaultSemanticAnalizer _semanticAnalizer;
    
    public DriftCompiler()
    {
        _semanticAnalizer = new DefaultSemanticAnalizer([
            new SymbolRule(),
            new TypeRule(),
            new ConsistencyRule()
        ]);
    }

    public DiagnosticAggregator? Diagnostic { get; private set; }

    public long Compile(string file)
    {
        var source = new DriftStreamReader(file);
        var tokenizer = new Tokenizer(source);
        var parser = new DriftParser(tokenizer);
        var script = parser.Parse();

        Diagnostic = _semanticAnalizer.Analyze(script);
        if (Diagnostic.Errors.Count() > 0)
            return -1;

        var sw = Stopwatch.StartNew();
        var interpreter = new DriftInterpreter(new DriftExecutionContext(), script);
        interpreter.Interpret();
        sw.Stop();

        return sw.ElapsedMilliseconds;
    }
}
