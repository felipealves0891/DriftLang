using System;
using Drift.Core;
using Drift.Core.Ast.Types;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Declarations;
using Drift.Core.Nodes.Expressions;
using Drift.Core.Nodes.Helpers;
using Drift.Core.Nodes.Statements;
using Drift.Core.Types;
using Drift.Semantic.Symbols;

namespace Drift.Semantic.Rules.Helpers;

public class ReturnTypeResolver
{
    private readonly IDataType _typeVoid;
    private readonly SymbolTable _symbolTable;

    public ReturnTypeResolver(SymbolTable symbolTable)
    {
        _symbolTable = symbolTable;
        _typeVoid = DriftEnv.TypeRegistry.Resolve("void");
    }

    public IDataType Resolve(DriftNode node)
    {
        if (node is IdentifierNode identifier)
            return _symbolTable.Resolve(identifier.Value)?.Type ?? _typeVoid;
        else if (node is IDriftValue literal)
            return literal.Type;
        else if (node is StatementNode statement)
            return ResolveStatement(statement);
        else if (node is ExpressionNode expression)
            return ResolveExpression(expression);
        else
            return _typeVoid;
    }

    private IDataType ResolveStatement(StatementNode statement)
    {
        if (statement is VariableDeclaration declaration)
            return declaration.Type;
        else if (statement is AssignmentStatement assignment)
            return _symbolTable.Resolve(assignment.Identifier)?.Type ?? _typeVoid;
        else
            return ResolveBlockStatement(statement);
    }

    private IDataType ResolveBlockStatement(StatementNode statement)
    {
        var returns = ReturnStatementFinder.Find(statement);
        var distinct = returns.Distinct().ToArray();
        if (distinct.Count() == 1)
            return ResolveExpression(distinct.First().Expression);

        return _typeVoid;
    }

    private IDataType ResolveExpression(ExpressionNode node)
    {
        if (node is ValueExpression value)
            return Resolve((DriftNode)value.Value);
        else if (node is ArrayAccessExpression arrayAccess)
            return ResolveArrayAccessExpression(arrayAccess);
        else if (node is FunctionCallExpression functionCall)
            return ResolveFunctionCallExpression(functionCall);
        else if (node is UnaryExpression unary)
            return Resolve(unary.Value);
        else if (node is BinaryExpression binary)
            return ResolveBinaryExpression(binary);
        else if (node is StructAccessExpression structAccess)
            return ResolveStructAccessExpression(structAccess);
        else if (node is TryExpression tryExpression)
            return ResolveTryExpression(tryExpression);
        else if (node is ArrayExpression array)
            return ResolveArrayExpression(array);
        else if (node is ModuleAccessExpression moduleAccess)
            return ResolveModuleAccessExpression(moduleAccess);
        else
            return _typeVoid;
    }

    private IDataType ResolveModuleAccessExpression(ModuleAccessExpression moduleAccess)
    {
        var definition = (FunctionCallExpression)moduleAccess.Expression;
        var symbol = _symbolTable.Resolve(definition.Identifier, moduleAccess.Module);
        if (!symbol.HasValue)
            return _typeVoid;

        return symbol.Value.Type;
    }

    private IDataType ResolveArrayAccessExpression(ArrayAccessExpression expression)
    {
        var identifier = expression.Identifier;
        var symbol = _symbolTable.Resolve(identifier);
        if (!symbol.HasValue)
            return _typeVoid;

        var composite = (CompositeType)symbol.Value.Type;
        return composite.Compose;
    }

    private IDataType ResolveFunctionCallExpression(FunctionCallExpression expression)
    {
        var identifier = expression.Identifier;
        var symbol = _symbolTable.Resolve(identifier);
        if (!symbol.HasValue)
            return _typeVoid;

        return symbol.Value.Type;
    }

    private IDataType ResolveBinaryExpression(BinaryExpression expression)
    {
        var lType = Resolve(expression.Left);
        var rType = Resolve(expression.Right);
        var resultType = lType.Equals(rType) ? lType : _typeVoid;

        return expression.Operator switch
        {
            "+" => resultType,
            "-" => resultType,
            "*" => resultType,
            "/" => resultType,
            "==" => DriftEnv.TypeRegistry.Resolve("bool"),
            ">" => DriftEnv.TypeRegistry.Resolve("bool"),
            ">=" => DriftEnv.TypeRegistry.Resolve("bool"),
            "<=" => DriftEnv.TypeRegistry.Resolve("bool"),
            "<" => DriftEnv.TypeRegistry.Resolve("bool"),
            "and" => DriftEnv.TypeRegistry.Resolve("bool"),
            "or" => DriftEnv.TypeRegistry.Resolve("bool"),
            "." => DriftEnv.TypeRegistry.Resolve("string"),
            _ => _typeVoid
        };
    }

    private IDataType ResolveStructAccessExpression(StructAccessExpression structAccess)
    {
        var identifier = structAccess.Instance;
        var symbol = _symbolTable.Resolve(identifier);
        if (!symbol.HasValue)
            return _typeVoid;

        var complex = (ComplexType)symbol.Value.Type;
        if (complex.Properties.TryGetValue(structAccess.Property, out var dataType))
            return dataType;
        else
            return _typeVoid;
    }

    private IDataType ResolveTryExpression(TryExpression tryExpression)
    {
        var success = tryExpression.Success.Type;
        var error = tryExpression.Error.Type;
        return success.Equals(error) ? success : _typeVoid;
    }

    private IDataType ResolveArrayExpression(ArrayExpression array)
    {
        var types = array.Expressions.Select(ResolveExpression).Where(x => x is not null).ToArray();
        IDataType? baseType = null;

        foreach (var type in types)
        {
            if (baseType is null)
                baseType = type;

            if (baseType != type)
                baseType = DriftEnv.TypeRegistry.Resolve("any");
        }

        var typeName = baseType?.Name + "[]";
        return DriftEnv.TypeRegistry.Resolve(typeName);
    }

}
