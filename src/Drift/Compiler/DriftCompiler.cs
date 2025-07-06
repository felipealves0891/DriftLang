using System;
using System.Diagnostics;
using Drift.Lexer;
using Drift.Lexer.Reader;
using Drift.Parser;
using Drift.Runtime;
using Drift.Runtime.Contexts;
using Drift.Semantic;
using Drift.Semantic.Analyzers;
using Drift.Semantic.Diagnostic;
using Drift.Semantic.Rules;

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
