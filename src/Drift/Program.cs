using System.Diagnostics;
using Drift.Lexer;
using Drift.Lexer.Reader;
using Drift.Parser;
using Drift.Runtime;
using Drift.Semantic.Analyzers;
using Drift.Semantic.Rules;

var source = new DriftStreamReader(@"D:\Source\Scripts\main.dft");
var tokenizer = new Tokenizer(source);
var parser = new DriftParser(tokenizer);
var script = parser.Parse();

var analizer = new DefaultSemanticAnalizer([
    new SymbolRule(),
    new TypeRule(),
    new ConsistencyRule()
]);

var diagnostic = analizer.Analyze(script);
if (diagnostic.Errors.Count() > 0)
{
    Console.WriteLine(diagnostic);
    return;
}

var sw = Stopwatch.StartNew();
var interpreter = new DriftInterpreter(script);
interpreter.Interpret();
sw.Stop();

Console.WriteLine("Execution Time: {0}", sw.ElapsedMilliseconds);
Console.WriteLine();
Console.WriteLine(diagnostic);