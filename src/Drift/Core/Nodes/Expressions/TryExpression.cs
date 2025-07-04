using System;
using Drift.Core.Nodes.Statements;
using Drift.Core.Location;
using Drift.Core.Helpers;
using Drift.Core.Nodes.Literals;

namespace Drift.Core.Nodes.Expressions;

public class TryExpression : ExpressionNode
{
    public TryExpression(
        ExpressionNode expression,
        ActionStatement success,
        ActionStatement error,
        SourceLocation location) : base(location)
    {
        Expression = expression;
        Expression.Parent = this;

        Success = success;
        Success.Parent = this;

        Error = error;
        Error.Parent = this;
    }

    public ExpressionNode Expression { get; }
    public ActionStatement Success { get; }
    public ActionStatement Error { get; }

    public override DriftNode[] Children => [Expression, Success, Error];

    public override IDriftValue Evaluate(IExecutionContext context)
    {
        try
        {
            var value = Expression.Evaluate(context);
            using (context.EnterScope())
                return ExecutionAction(Success, context, value);

        }
        catch (ErrorFlow error)
        {
            using (context.EnterScope())
                return ExecutionAction(Error, context, error.Value);
        }
        catch (Exception error)
        {
            using (context.EnterScope())
                return ExecutionAction(Error, context, new StringLiteral(error.Message, new()));
        }
    }

    private IDriftValue ExecutionAction(
        ActionStatement action,
        IExecutionContext context,
        IDriftValue value)
    { 
        var interpreter = context.CreateInterpreter(action);
        var requerides = action.Parameters.FirstOrDefault();
        var parameters = new Dictionary<string, IDriftValue>();
        if (requerides is not null)
            parameters[requerides.Identifier] = value;

        if (parameters is null)
            return interpreter.Invoke(new Dictionary<string, IDriftValue>()) ?? null!;
        else
            return interpreter.Invoke(parameters) ?? null!;
    }

    public override string ToString()
    {
        return $"try {Expression} {{\n\tsuccess{Success},\n\terror{Error}\n}}";
    }
}
