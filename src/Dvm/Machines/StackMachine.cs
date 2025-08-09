using System;

namespace Dvm.Machines;

public class StackMachine
{
    private readonly List<Instruction> _program;
    private readonly Dictionary<string, object> _variables = new();
    private readonly Dictionary<string, int> _labels = new();
    private readonly Stack<object> _stack = new();

    public StackMachine(List<Instruction> program)
    {
        _program = program;
        PreprocessLabels();
    }

    private void PreprocessLabels()
    {
        for (int i = 0; i < _program.Count; i++)
        {
            if (_program[i].Code == InstructionCode.LABEL
             && _program[i].Arg is string labelName)
                _labels[labelName] = i;
        }
    }

    public void Run()
    {
        for (int ip = 0; ip < _program.Count; ip++)
        {
            var instr = _program[ip];
            switch (instr.Code)
            {
                case InstructionCode.PUSH_CONST:
                    _stack.Push((instr.Arg ?? ""));
                    break;
                case InstructionCode.LOAD_VAR:
                    _stack.Push(_variables[(string)(instr.Arg ?? "")]);
                    break;
                case InstructionCode.STORE_VAR:
                    _variables[(string)(instr.Arg ?? "")] = _stack.Pop();
                    break;
                case InstructionCode.ADD:
                    _stack.Push((object)((dynamic)_stack.Pop() + (dynamic)_stack.Pop()));
                    break;
                case InstructionCode.SUB:
                    {
                        var b = _stack.Pop();
                        var a = _stack.Pop();
                        _stack.Push((object)((dynamic)a - (dynamic)b));
                        break;
                    }
                case InstructionCode.MUL:
                    _stack.Push((object)((dynamic)_stack.Pop() * (dynamic)_stack.Pop()));
                    break;
                case InstructionCode.DIV:
                    {
                        var b = _stack.Pop();
                        var a = _stack.Pop();
                        _stack.Push((object)((dynamic)a / (dynamic)b));
                        break;
                    }
                case InstructionCode.LT:
                    {
                        var b = _stack.Pop();
                        var a = _stack.Pop();
                        _stack.Push((object)((dynamic)a < (dynamic)b));
                        break;
                    }
                case InstructionCode.GT:
                    {
                        var b = _stack.Pop();
                        var a = _stack.Pop();
                        _stack.Push((object)((dynamic)a > (dynamic)b));
                        break;
                    }
                case InstructionCode.EQ:
                    _stack.Push(_stack.Pop().Equals(_stack.Pop()));
                    break;
                case InstructionCode.JUMP:
                    ip = _labels[(string)(instr.Arg ?? "")] - 1;
                    break;
                case InstructionCode.JUMP_IF_FALSE:
                    if (_stack.Pop() is bool cond && !cond)
                        ip = _labels[(string)(instr.Arg ?? "")] - 1;
                    break;
                case InstructionCode.CALL_BUILTIN:
                    if (instr.Arg is string func && func == "print")
                        Console.WriteLine(_stack.Pop());
                    break;
            }
        }
    }
}