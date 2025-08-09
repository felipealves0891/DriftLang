using System;
using Drift.Analyzers.Core;
using Drift.Analyzers.Core.Nodes;
using Drift.Analyzers.Core.Nodes.Declarations;
using Drift.Analyzers.Core.Nodes.Expressions;
using Drift.Analyzers.Core.Nodes.Statements;
using Drift.Analyzers.Core.Types;
using Drift.Analyzers.Semantic.Rules.Helpers;
using Drift.Analyzers.Semantic.Symbols;

namespace Drift.Analyzers.Semantic.Rules;

public class TypeRule : BaseRule
{
    private readonly Queue<Action> _nextScope;
    private ReturnTypeResolver _typeResolver = null!;

    public TypeRule()
        : base()
    {
        _nextScope = new Queue<Action>();
        AddHandler<ModuleAccessExpression>(ModuleAccessExpressionApply);
        AddHandler<UnaryExpression>(UnaryExpressionApply);
        AddHandler<IfStatement>(IfStatementApply);
        AddHandler<EmitStatement>(EmitStatementApply);
        AddHandler<OnDeclaration>(OnDeclarationApply);
        AddHandler<FunctionCallExpression>(FunctionCallExpressionApply);
        AddHandler<VariableDeclaration>(VariableDeclarationApply);
        AddHandler<FunctionDeclaration>(FunctionDeclarationApply);
        AddHandler<AssignmentStatement>(AssignmentStatementApply);
    }

    public override void PrepareApply(SymbolTable table)
    {
        if (_typeResolver is null)
            _typeResolver = new ReturnTypeResolver(table);
    }

    private void ModuleAccessExpressionApply(DriftNode node)
    {
        var moduleAccess = (ModuleAccessExpression)node;
        var call = (FunctionCallExpression)moduleAccess.Expression;
        var expectedParameters = Table.ResolveParameters(call.Identifier, moduleAccess.Module).ToArray();
        if (call.Arguments.Count() != expectedParameters.Length)
        {
            var argumentsCount = call.Arguments.Count();
            var parametersCount = expectedParameters.Length;

            Aggregator.AddError($"The {call.Identifier} function expected {parametersCount} parameters, but received {argumentsCount},", call.Location);
            return;
        }

        for (int i = 0; i < expectedParameters.Count(); i++)
        {
            var argument = call.Arguments[i.ToString()];
            var parameter = expectedParameters[i];

            call.Arguments.Remove(i.ToString());
            call.Arguments[parameter.Identifier] = argument;

            var passed = _typeResolver.Resolve(argument);
            var expected = parameter.Type;
            if (!passed.Equals(expected))
                Aggregator.AddErrorIncompatibleTypes(passed.Name, expected.Name, node.Location);
        }
    }

    private void UnaryExpressionApply(DriftNode node)
    {
        var unary = (UnaryExpression)node;
        if (unary.Indicator == "not")
        {
            var valueType = _typeResolver.Resolve(unary.Value);
            if (valueType.Name != TypeNames.Boolean)
                Aggregator.AddErrorIncompatibleTypes(TypeNames.Boolean, valueType.Name, node.Location);
        }
        else if (unary.Indicator == "-")
        { 
            var valueType = _typeResolver.Resolve(unary.Value);
            if (valueType.Name != TypeNames.Integer && valueType.Name != TypeNames.Float)
                Aggregator.AddErrorIncompatibleTypes($"{TypeNames.Integer} or {TypeNames.Float}", valueType.Name, node.Location);
        }
    }

    private void IfStatementApply(DriftNode node)
    {
        var statement = (IfStatement)node;
        var expressionResultType = _typeResolver.Resolve(statement.Expression);
        if (expressionResultType.Name != TypeNames.Boolean)
            Aggregator.AddErrorIncompatibleTypes(TypeNames.Boolean, expressionResultType.Name, node.Location);
    }

    private void EmitStatementApply(DriftNode node)
    {
        var emit = (EmitStatement)node;
        var expectedParameters = Table.ResolveParameters(emit.Identifier).ToArray();
        if (emit.Arguments.Count() != expectedParameters.Length)
        {
            var argumentsCount = emit.Arguments.Count();
            var parametersCount = expectedParameters.Length;

            Aggregator.AddError($"The {emit.Identifier} function expected {parametersCount} parameters, but received {argumentsCount},", emit.Location);
            return;
        }

        for (int i = 0; i < expectedParameters.Count(); i++)
        {
            var argument = emit.Arguments[i.ToString()];
            var parameter = expectedParameters[i];

            emit.Arguments.Remove(i.ToString());
            emit.Arguments[parameter.Identifier] = argument;

            var passed = _typeResolver.Resolve(argument);
            var expected = parameter.Type;
            if (!passed.Equals(expected))
                Aggregator.AddErrorIncompatibleTypes(passed.Name, expected.Name, node.Location);
        }
    }

    private void OnDeclarationApply(DriftNode node)
    {
        var on = (OnDeclaration)node;
        var expectedParameters = Table.ResolveParameters(on.Identifier).ToArray();

        if (on.Parameters.Count() != expectedParameters.Length)
        {
            var argumentsCount = on.Parameters.Count();
            var parametersCount = expectedParameters.Length;

            Aggregator.AddError($"The {on.Identifier} function expected {parametersCount} parameters, but received {argumentsCount},", on.Location);
            return;
        }

        for (int i = 0; i < expectedParameters.Count(); i++)
        {
            var parameter = expectedParameters[i];
            var argument = on.Parameters.FirstOrDefault(x => x.Identifier == parameter.Identifier);
            if (argument is null)
            {
                Aggregator.AddError($"Era esperado o parametro: {parameter.Identifier}", node.Location);
                continue;
            }
            
            var passed = _typeResolver.Resolve(argument);
            var expected = parameter.Type;
            if (!passed.Equals(expected))
                Aggregator.AddErrorIncompatibleTypes(passed.Name, expected.Name, node.Location);
        }
    }

    private void FunctionCallExpressionApply(DriftNode node)
    {
        var call = (FunctionCallExpression)node;
        var expectedParameters = Table.ResolveParameters(call.Identifier).ToArray();
        if (call.Arguments.Count() != expectedParameters.Length)
        {
            var argumentsCount = call.Arguments.Count();
            var parametersCount = expectedParameters.Length;

            Aggregator.AddError($"The {call.Identifier} function expected {parametersCount} parameters, but received {argumentsCount},", call.Location);
            return;
        }

        for (int i = 0; i < expectedParameters.Count(); i++)
        {
            var argument = call.Arguments[i.ToString()];
            var parameter = expectedParameters[i];

            call.Arguments.Remove(i.ToString());
            call.Arguments[parameter.Identifier] = argument;

            var passed = _typeResolver.Resolve(argument);
            var expected = parameter.Type;
            if (!passed.Equals(expected))
                Aggregator.AddErrorIncompatibleTypes(passed.Name, expected.Name, node.Location);
        }
    }

    private void VariableDeclarationApply(DriftNode node)
    {
        var declaration = (VariableDeclaration)node;
        var lType = declaration.Type;
        if (declaration.Assignment is null)
            return;

        var rType = _typeResolver.Resolve(declaration.Assignment);
        if (!lType.Equals(rType))
            Aggregator.AddErrorIncompatibleTypes(lType.Name, rType.Name, node.Location);
    }

    private void FunctionDeclarationApply(DriftNode node)
    {
        var function = (FunctionDeclaration)node;
        var expectedType = function.Type;

        var returns = ReturnStatementFinder.Find(function);
        _nextScope.Enqueue(() =>
        { 
            foreach (var result in returns)
            {
                var resultType = _typeResolver.Resolve(result.Expression);
                if (!expectedType.Equals(resultType))
                    Aggregator.AddErrorIncompatibleTypes(expectedType.Name, resultType.Name, result.Location);
            }
        });        
    }

    private void AssignmentStatementApply(DriftNode node)
    {
        var assignment = (AssignmentStatement)node;
        var lType = _typeResolver.Resolve(assignment);
        var rType = _typeResolver.Resolve(assignment.Expression);        

        if (!lType.Equals(rType))
            Aggregator.AddErrorIncompatibleTypes(lType.Name, rType.Name, node.Location);
    }


    public override void PrepareScope()
    {
        while (_nextScope.TryDequeue(out var action))
            action();
    }
}
