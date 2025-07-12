namespace Drift.Lexer;

public enum TokenType
{
    EOF = -1,

    /// <summary>
    /// Caracter (;)
    /// </summary>
    SEMICOLON,

    /// <summary>
    /// Identificador alfanumerico
    /// </summary>
    IDENTIFIER,

    /// <summary>
    /// Caracter (=)
    /// </summary>
    ASSIGNMENT,

    /// <summary>
    /// Caracter '('
    /// </summary>
    OPEN_PAREN,

    /// <summary>
    /// Caracter (,)
    /// </summary>
    COMMA,

    /// <summary>
    /// Caracter ')'
    /// </summary>
    CLOSE_PAREN,

    /// <summary>
    /// Textos entre 'Exemplo'
    /// </summary>
    STRING_LITERAL,

    /// <summary>
    /// Numeros inteiros
    /// </summary>
    INTEGER_LITERAL,

    /// <summary>
    /// Numeros com ponto flutuante
    /// </summary>
    FLOAT_LITERAL,

    /// <summary>
    /// Identificadores true ou false
    /// </summary>
    BOOLEAN_LITERAL,

    /// <summary>
    /// Caracter ({)
    /// </summary>
    OPEN_BRACE,

    /// <summary>
    /// Caracter (})
    /// </summary>
    CLOSE_BRACE,

    /// <summary>
    /// Caracter ([)
    /// </summary>
    OPEN_BRACKET,

    /// <summary>
    /// Caracter (])
    /// </summary>
    CLOSE_BRACKET,

    /// <summary>
    /// '<=', '<', '>=', '>', '*', '+', '-', '/', '%', '!'
    /// </summary>
    OPERATOR,

    /// <summary>
    /// Identificador ->
    /// </summary>
    ARROW,

    /// <summary>
    /// Caracter (.)
    /// </summary>
    ACCESS,

    /// <summary>
    /// Caracter (:)
    /// </summary>
    COLON,

    /// <summary>
    /// Caracter (|)
    /// </summary>
    PIPE,

    /// <summary>
    /// Caracter (@)
    /// </summary>
    AT,

    /// <summary>
    /// Keywoord
    /// </summary>
    LET,

    /// <summary>
    /// Keywoord
    /// </summary>
    IF,

    /// <summary>
    /// Keywoord
    /// </summary>
    ELSE,

    /// <summary>
    /// Keywoord
    /// </summary>
    ON,

    /// <summary>
    /// Keywoord
    /// </summary>
    TRUE,

    /// <summary>
    /// Keywoord
    /// </summary>
    FALSE,

    /// <summary>
    /// Keywoord
    /// </summary>
    FUNCTION,

    /// <summary>
    /// Keywoord
    /// </summary>
    RETURN,

    /// <summary>
    /// Keywoord
    /// </summary>
    WHEN,

    /// <summary>
    /// Keywoord
    /// </summary>
    FOR,

    /// <summary>
    /// Keywoord
    /// </summary>
    TO,

    /// <summary>
    /// Keywoord
    /// </summary>
    IN,

    /// <summary>
    /// Keywoord
    /// </summary>
    WHILE,

    /// <summary>
    /// Keywoord
    /// </summary>
    CONST,

    /// <summary>
    /// Keywoord
    /// </summary>
    ASYNC,

    /// <summary>
    /// Keywoord
    /// </summary>
    BIND,

    /// <summary>
    /// Keywoord
    /// </summary>
    SUCCESS,

    /// <summary>
    /// Keywoord
    /// </summary>
    ERROR,

    /// <summary>
    /// Keywoord
    /// </summary>
    TRY,

    /// <summary>
    /// Keywoord
    /// </summary>
    TYPE,

    /// <summary>
    /// Keywoord
    /// </summary>
    SUB,

    /// <summary>
    /// Keywoord
    /// </summary>
    EMIT,

    /// <summary>
    /// Keywoord
    /// </summary>
    EVENT,

    /// <summary>
    /// Keywoord
    /// </summary>
    AND,

    /// <summary>
    /// Keywoord
    /// </summary>
    OR,
    
    /// <summary>
    /// Keywoord
    /// </summary>
    NOT,
    
    /// <summary>
    /// Keywoord
    /// </summary>
    EXPORT,
        
    /// <summary>
    /// Keywoord
    /// </summary>
    MODULE
} 