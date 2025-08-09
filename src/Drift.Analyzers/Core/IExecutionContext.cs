using System;
using System.Linq.Expressions;
using Drift.Analyzers.Core.Nodes;
using Drift.Analyzers.Core.Nodes.Declarations;
using Drift.Analyzers.Core.Nodes.Expressions;
using Drift.Analyzers.Core.Nodes.Statements;

namespace Drift.Analyzers.Core;

public interface IExecutionContext
{
    IDrift Get(string name);
    void Bind(string name, ExpressionNode node);
    void Set(string name, IDrift value);
    void Declare(string name);

    IDriftFunction CreateFunction(FunctionDeclaration block);
    IDriftFunction CreateFunction(OnDeclaration block);
    IDriftFunction CreateFunction(BlockStatement block);

    void Event(string name);
    void Publish(string name, IDictionary<string, IDriftValue> arguments);
    void Subscribe(string name, IDriftFunction function);

    IDisposable EnterScope();
    void ExitScope();

    IExecutionContext Clone();

    IDriftModule CreateModule(ModuleDeclaration declaration);
    void Expose(string name);
    bool Exposed(string name);
}
