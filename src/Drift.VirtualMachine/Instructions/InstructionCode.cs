namespace Drift.VirtualMachine.Instructions;

public enum InstructionCode
{
    // --- Aritmética e lógica ---
    PUSH_CONST,
    ADD, SUB, MUL, DIV, MOD,
    NEG,
    AND, OR, NOT, XOR,
    EQ, NEQ, LT, LTE, GT, GTE,

    // --- Variáveis e escopo ---
    DECLARE_VAR,
    STORE_VAR,
    LOAD_VAR,
    DELETE_VAR,
    PUSH_SCOPE,
    POP_SCOPE,

    // --- Controle de fluxo ---
    JUMP,
    JUMP_IF_TRUE,
    JUMP_IF_FALSE,
    LABEL,
    NOOP,

    // --- Funções e chamadas ---
    FUNC,
    END_FUNC,
    CALL_FUNC,
    RETURN,
    LOAD_ARG,
    CALL_BUILTIN,

    // --- Estruturas de dados ---
    MAKE_LIST,
    MAKE_DICT,
    GET_INDEX,
    SET_INDEX,
    HAS_KEY,
    LEN,

    // --- Objetos / Estruturas ---
    NEW_OBJECT,
    GET_FIELD,
    SET_FIELD,
    CALL_METHOD,

    // --- Concorrência / eventos ---
    SLEEP,
    SPAWN_FUNC,
    AWAIT,
    YIELD,
    WAIT_EVENT,
    RAISE_EVENT,

    // --- Erros / Exceções ---
    THROW,
    TRY_BEGIN,
    TRY_END,
    CATCH_BEGIN,
    CATCH_END,
    FINALLY_BEGIN,
    FINALLY_END,
    RAISE,

    // --- Metaprogramação / Internals ---
    EVAL,
    REFLECT_FIELDS,
    TYPE_OF,
    DUMP_STACK,
    DUMP_SCOPE,

    // --- Sistema / I/O / Host ---
    OPEN_FILE,
    READ_FILE,
    WRITE_FILE,
    IMPORT,
    SYSTEM_CALL,
    HOST_CALL
}
