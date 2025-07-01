//TODO: Implementar a pipeline

using System.Diagnostics;
using Drift.Lexer;
using Drift.Lexer.Reader;
using Drift.Parser;
using Drift.Runtime;
using Drift.Semantic.Analyzers;
using Drift.Semantic.Rules;

var sw = Stopwatch.StartNew();
//var source = new DriftStreamReader(@"D:\Source\Scripts\Teste.dft");

var code = @"
event userLoggedIn(user: string);
event dataReceived(payload: string);

on userLoggedIn(user: string) {
    println('O Usuario {0} logou!', [user]);
}

on dataReceived(payload: string) {
    println('O Usuario {0} logou!', [payload]);
}

# Declaração de variável
let variavel: int = 5;
const constante: string = 'teste';
let assincrono: float = 0.01;
let variavelNegativa: int = -5;
let variavelMaisComplexa: int = variavel + 2 * 10 - 20 / 2 - variavelNegativa;

variavelNegativa = -10;

# Bind reativo com expressão
bind verdade: bool -> variavel == 5 and variavelNegativa == -10;

# Declaração de função
fun double(x: int): int {
	if x > 10 {
		return x * 2;
	} else {
		return x + 2;
	}
}

# Bind reativo com expressão
bind doubled: int -> double(variavel);

# Bloco if/else
if doubled > 10 {
    emit dataReceived('Doubled is greater than 10');
} else {
    emit dataReceived('Doubled is 10 or less');
}

if doubled < 10 {
    emit dataReceived('Doubled is greater than 10');
}

# While loop
let counter: int = 0;
let invalido: bool = true;
";

var source = new DriftTextReader(code, "memory");

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

var interpreter = new DriftInterpreter(script);
interpreter.Interpret();

sw.Stop();

Console.WriteLine("Execution Time: {0}", sw.Elapsed);
Console.WriteLine();
Console.WriteLine(diagnostic);
