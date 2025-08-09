using System;
using Drift.Analyzers.Core.Nodes.Statements;
using Drift.Analyzers.Lexer;
using Drift.Analyzers.Lexer.Reader;
using Drift.Analyzers.Parser;
using Drift.Analyzers.Semantic.Analyzers;
using Drift.Analyzers.Semantic.Diagnostic;
using Drift.Analyzers.Semantic.Rules;

namespace Drift.Analyzers;

public class CodeAnalyzer
{
    private readonly DefaultSemanticAnalizer _semanticAnalizer;
    
    public CodeAnalyzer()
    {
        _semanticAnalizer = new DefaultSemanticAnalizer([
            new SymbolRule(),
            new TypeRule(),
            new ConsistencyRule()
        ]);
    }

    public DiagnosticAggregator? Diagnostic { get; private set; }

    public ScriptNode? Analyze(string file)
    {
        var source = new DriftStreamReader(file);
        var tokenizer = new Tokenizer(source);
        var parser = new DriftParser(tokenizer);
        var script = parser.Parse();

        Diagnostic = _semanticAnalizer.Analyze(script);
        if (Diagnostic.Errors.Count() > 0)
            return null;

        return script;
    }   
}
