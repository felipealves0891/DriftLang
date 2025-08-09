namespace Drift.Analyzers.CodeGen;

public enum Operation
{
    // --- Aritmética e lógica ---
    BinOp,
    UnaryOp,
    LogicOp,
    CmpOp,

    // --- Variáveis e escopo ---
    VarDecl,
    VarStore,
    VarLoad,
    VarDel,
    ScopePush,
    ScopePop,

    // --- Controle de fluxo ---
    Jmp,
    JmpTrue,
    JmpFalse,
    Label,
    Nop,

    // --- Funções e chamadas ---
    FuncBegin,
    FuncEnd,
    Call,
    Ret,
    ArgLoad,
    CallBuiltin,

    // --- Estruturas de dados ---
    ListNew,
    DictNew,
    IndexGet,
    IndexSet,
    DictHas,
    Len,

    // --- Objetos / Estruturas ---
    ObjNew,
    FieldGet,
    FieldSet,
    MethodCall,

    // --- Concorrência / eventos ---
    Sleep,
    Spawn,
    Await,
    Yield,
    EventWait,
    EventRaise,

    // --- Erros / Exceções ---
    Throw,
    TryBegin,
    TryEnd,
    CatchBegin,
    CatchEnd,
    FinallyBegin,
    FinallyEnd,
    Raise,

    // --- Metaprogramação / Internals ---
    Eval,
    ReflectFields,
    TypeOf,
    DumpStack,
    DumpScope,

    // --- Sistema / I/O / Host ---
    FileOpen,
    FileRead,
    FileWrite,
    Import,
    SysCall,
    HostCall
}
