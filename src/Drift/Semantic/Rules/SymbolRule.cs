using Drift.Core;
using Drift.Core.Ast.Types;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Declarations;
using Drift.Core.Nodes.Expressions;
using Drift.Core.Nodes.Helpers;
using Drift.Core.Nodes.Statements;
using Drift.Core.Types;
using Drift.Semantic.Symbols;

namespace Drift.Semantic.Rules;

public class SymbolRule : BaseRule
{
    private readonly List<Symbol> _parameters;

    public SymbolRule()
    {
        _parameters = new List<Symbol>();
        AddHandler<ExportStatement>(ExportStatementApply);
        AddHandler<BindDeclaration>(BindDeclarationApply);
        AddHandler<EventDeclaration>(EventDeclarationApply);
        AddHandler<FunctionDeclaration>(FunctionDeclarationApply);
        AddHandler<OnDeclaration>(OnDeclarationApply);
        AddHandler<StructDeclaration>(StructDeclarationApply);
        AddHandler<VariableDeclaration>(VariableDeclarationApply);
        AddHandler<ArrayAccessExpression>(ArrayAccessExpressionApply);
        AddHandler<StructAccessExpression>(StructAccessExpressionApply);
        AddHandler<IdentifierNode>(IdentifierNodeApply);
        AddHandler<ForToStatement>(ForToStatementApply);
        AddHandler<ForInStatement>(ForInStatementApply);
        AddHandler<ActionStatement>(ActionStatementApply);
    }

    public override void PrepareScope()
    {
        foreach (var parameter in _parameters)
            Table.Declare(parameter);

        _parameters.Clear();
    }

    private void ExportStatementApply(DriftNode node)
    {
        var export = (ExportStatement)node;
        FunctionDeclarationApply(export.Declaration);
    }

    private void VariableDeclarationApply(DriftNode node)
    {
        var variable = (VariableDeclaration)node;
        var symbol = new Symbol(variable.Identifier, variable.Immutable, variable.Type, variable.Location);
        if (!Table.Declare(symbol))
            Aggregator.AddErrorAlreadyDeclaration(symbol.Identifier, symbol.Location);
    }

    private void StructDeclarationApply(DriftNode node)
    {
        var declaration = (StructDeclaration)node;
        var symbol = new Symbol(declaration.Identifier, DriftEnv.TypeRegistry.Resolve(declaration.Identifier), declaration.Location);
        if (!Table.Declare(symbol))
            Aggregator.AddErrorAlreadyDeclaration(symbol.Identifier, symbol.Location);
    }

    private void OnDeclarationApply(DriftNode node)
    {
        var on = (OnDeclaration)node;
        var symbol = Table.Resolve(on.Identifier);
        if (symbol is null)
            Aggregator.AddErrorWasNotDeclared(on.Identifier, on.Location);

        foreach (var parameter in on.Parameters)
            _parameters.Add(new Symbol(parameter.Identifier, parameter.Type, parameter.Location));

    }

    private void FunctionDeclarationApply(DriftNode node)
    {
        var fun = (FunctionDeclaration)node;
        var symbol = new Symbol(fun.Identifier, fun.Type, fun.Location);

        foreach (var parameter in fun.Parameters)
        {
            var child = new Symbol(parameter.Identifier, parameter.Type, parameter.Location);
            symbol.Dependences.Add(new SymbolDependence(child));
            _parameters.Add(child);
        }
        
        if (!Table.Declare(symbol))
            Aggregator.AddErrorAlreadyDeclaration(symbol.Identifier, symbol.Location);
    }

    private void EventDeclarationApply(DriftNode node)
    {
        var ev = (EventDeclaration)node;
        var symbol = new Symbol(ev.Identifier, DriftEnv.TypeRegistry.Resolve("void"), ev.Location);

        foreach (var parameter in ev.Parameters)
        {
            var child = new Symbol(parameter.Identifier, parameter.Type, parameter.Location);
            symbol.Dependences.Add(new SymbolDependence(child));
            _parameters.Add(child);
        }
        
        if (!Table.Declare(symbol))
            Aggregator.AddErrorAlreadyDeclaration(symbol.Identifier, symbol.Location);
    }

    private void BindDeclarationApply(DriftNode node)
    {
        var bind = (BindDeclaration)node;
        var symbol = new Symbol(bind.Identifier, bind.Type, bind.Location);
        if (!Table.Declare(symbol))
            Aggregator.AddErrorAlreadyDeclaration(symbol.Identifier, symbol.Location);
    }

    private void ArrayAccessExpressionApply(DriftNode node)
    {
        var arrayAccess = (ArrayAccessExpression)node;
        var symbol = Table.Resolve(arrayAccess.Identifier);
        if (symbol is null)
        {
            Aggregator.AddErrorWasNotDeclared(arrayAccess.Identifier, arrayAccess.Location);
            return;
        }

        var type = symbol.Value.Type;
        if (type is not CompositeType || !type.Name.EndsWith("[]"))
        {
            Aggregator.AddError($"The symbol {symbol.Value.Identifier} is not an array for attempting to access with index", arrayAccess.Location);
            return;
        }
    }

    private void StructAccessExpressionApply(DriftNode node)
    {
        var structAccess = (StructAccessExpression)node;
        var symbol = Table.Resolve(structAccess.Instance);
        if (!symbol.HasValue)
        {
            Aggregator.AddErrorWasNotDeclared(structAccess.Instance, structAccess.Location);
            return;
        }

        var type = symbol.Value.Type as ComplexType;
        if (type is null)
        {
            Aggregator.AddError("Attempt to access property of non-complex type", structAccess.Location);
            return;
        }

        if (!type.Properties.ContainsKey(structAccess.Property))
        {
            Aggregator.AddError($"The type {type.Name} does not have a property named {structAccess.Property}", structAccess.Location);
            return;
        }
    }

    private void IdentifierNodeApply(DriftNode node)
    {
        var identifier = (IdentifierNode)node;
        var symbol = Table.Resolve(identifier.Value);
        if (!symbol.HasValue)
            Aggregator.AddErrorWasNotDeclared(identifier.Value, identifier.Location);
        else
            identifier.Type = symbol.Value.Type;
    }

    private void ForToStatementApply(DriftNode node)
    {
        var forTo = (ForToStatement)node;
        var control = forTo.Declaration;
        _parameters.Add(new Symbol(control.Identifier, control.Type, control.Location));
    }

    private void ForInStatementApply(DriftNode node)
    {
        var forIn = (ForInStatement)node;
        var control = forIn.Declaration;
        _parameters.Add(new Symbol(control.Identifier, control.Type, control.Location));
    }
    
    private void ActionStatementApply(DriftNode node)
    {
        var action = (ActionStatement)node;
        foreach (var parameter in action.Parameters)
            _parameters.Add(new Symbol(parameter.Identifier, parameter.Type, parameter.Location));
    }

}
