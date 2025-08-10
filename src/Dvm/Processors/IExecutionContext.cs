using System;

namespace Dvm.Processors;

public interface IExecutionContext
{
    public void Push(object constant);
    public object[] Pop(int number);

    public void Declare(string name);
    public void Store(string name);
    public void Load(string name);
    public void Delete(string name);

}
