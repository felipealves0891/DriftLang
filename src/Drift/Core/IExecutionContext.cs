using System;
using System.Linq.Expressions;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Declarations;
using Drift.Core.Nodes.Expressions;
using Drift.Core.Nodes.Statements;
using Drift.Runtime;

namespace Drift.Core;

public interface IExecutionContext
{
    IDrift Get(string name);
    void Bind(string name, ExpressionNode node);
    void Set(string name, IDrift value);
    void Declare(string name);
    
    IDriftFunction CreateInterpreter(FunctionDeclaration block);
    IDriftFunction CreateInterpreter(OnDeclaration block);
    IDriftFunction CreateInterpreter(BlockStatement block);

    void Event(string name);
    void Publish(string name, IDictionary<string, IDriftValue> arguments);
    void Subscribe(string name, IDriftFunction function);

    IDisposable EnterScope();
    void ExitScope();
}
