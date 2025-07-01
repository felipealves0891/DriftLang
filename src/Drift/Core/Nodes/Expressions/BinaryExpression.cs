using System;
using System.Numerics;
using Drift.Core.Location;
using Drift.Core.Nodes.Helpers;
using Drift.Core.Nodes.Literals;

namespace Drift.Core.Nodes.Expressions;

public class BinaryExpression : ExpressionNode
{
    public BinaryExpression(
        DriftNode left,
        string op,
        DriftNode right,
        SourceLocation location) : base(location)
    {
        Left = left;
        Left.Parent = this;
        Operator = op;
        Right = right;
        Right.Parent = this;
    }

    public DriftNode Left { get; }
    public string Operator { get; }
    public DriftNode Right { get; }

    public override DriftNode[] Children => [Left, Right];

    public override IDriftValue Evaluate(IExecutionContext context)
    {
        var left = GetValue(Left, context);
        var right = GetValue(Right, context);
        if (left is null)
            left = GetValue(Left, context);
            
        return Operator switch
        {
            "+" => Sum(left, right),
            "-" => Subtract(left, right),
            "*" => Multiply(left, right),
            "/" => Division(left, right),
            "==" => IsEqual(left, right),
            ">" => IsGreaterThan(left, right),
            ">=" => IsGreaterThanOrEqual(left, right),
            "<=" => IsLessThanOrEqual(left, right),
            "<" => IsLessThan(left, right),
            "and" => new BooleanLiteral(((BooleanLiteral)left).Value && ((BooleanLiteral)Right).Value, Location),
            "or" => new BooleanLiteral(((BooleanLiteral)left).Value || ((BooleanLiteral)Right).Value, Location),
            _ => throw new Exception()
        };
    }

    private IDriftValue GetValue(DriftNode node, IExecutionContext context)
    {
        if (node is ExpressionNode expression)
            return expression.Evaluate(context);

        if (node is IdentifierNode identifierNode)
            return (IDriftValue)context.Get(identifierNode.Value);

        return (IDriftValue)node;
    }

    private IDriftValue IsLessThan(IDriftValue left, IDriftValue right)
    {
        return Compare(left, right, (a, b) => a < b);
    }

    private IDriftValue IsLessThanOrEqual(IDriftValue left, IDriftValue right)
    {
        return Compare(left, right, (a, b) => a <= b);
    }

    private IDriftValue IsGreaterThan(IDriftValue left, IDriftValue right)
    {
        return Compare(left, right, (a, b) => a > b);
    }

    private IDriftValue IsGreaterThanOrEqual(IDriftValue left, IDriftValue right)
    {
        return Compare(left, right, (a, b) => a >= b);
    }

    private IDriftValue IsEqual(IDriftValue left, IDriftValue right)
    {
        return new BooleanLiteral(left.Unwrap == right.Unwrap, Location);
    }

    private IDriftValue Division(IDriftValue left, IDriftValue right)
    {
        return Calculate(left, right, (a, b) => a / b);
    }

    private IDriftValue Multiply(IDriftValue left, IDriftValue right)
    {
        return Calculate(left, right, (a, b) => a * b);
    }

    private IDriftValue Subtract(IDriftValue left, IDriftValue right)
    {
        return Calculate(left, right, (a, b) => a - b);
    }

    private IDriftValue Sum(IDriftValue left, IDriftValue right)
    {
        return Calculate(left, right, (a, b) => a + b);
    }

    private IDriftValue Calculate(
        IDriftValue left,
        IDriftValue right,
        Func<decimal, decimal, decimal> executor)
    { 
        if (left is IntegerLiteral a1 && right is IntegerLiteral b1)
            return new IntegerLiteral(executor(a1.Value, b1.Value), Location);

        if (left is FloatLiteral a2 && right is FloatLiteral b2)
            return new FloatLiteral(executor(a2.Value, b2.Value), Location);

        if (left is IntegerLiteral a3 && right is FloatLiteral b3)
            return new FloatLiteral(executor(a3.Value, b3.Value), Location);

        if (left is IntegerLiteral a4 && right is FloatLiteral b4)
            return new FloatLiteral(executor(a4.Value, b4.Value), Location);

        throw new InvalidDataException("Passed values ​​do not support the add operation");
    }

    private IDriftValue Compare(
        IDriftValue left,
        IDriftValue right,
        Func<decimal, decimal, bool> executor)
    { 
        if (left is IntegerLiteral a1 && right is IntegerLiteral b1)
            return new BooleanLiteral(executor(a1.Value, b1.Value), Location);

        if (left is FloatLiteral a2 && right is FloatLiteral b2)
            return new BooleanLiteral(executor(a2.Value, b2.Value), Location);

        if (left is IntegerLiteral a3 && right is FloatLiteral b3)
            return new BooleanLiteral(executor(a3.Value, b3.Value), Location);

        if (left is IntegerLiteral a4 && right is FloatLiteral b4)
            return new BooleanLiteral(executor(a4.Value, b4.Value), Location);

        throw new InvalidDataException("Passed values ​​do not support the add operation");
    }

    public override string ToString()
    {
        return $"{Left} {Operator} {Right}";
    }
}
