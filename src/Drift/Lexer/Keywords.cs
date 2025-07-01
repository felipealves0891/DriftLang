using System;

namespace Drift.Lexer;

public static class Keywords
{
    public static byte[] LET => [(byte)'l', (byte)'e', (byte)'t'];

    public static byte[] IF => [(byte)'i', (byte)'f'];

    public static byte[] ELSE => [(byte)'e', (byte)'l', (byte)'s', (byte)'e'];

    public static byte[] ON => [(byte)'o', (byte)'n'];

    public static byte[] TRUE => [(byte)'t', (byte)'r', (byte)'u', (byte)'e'];

    public static byte[] FALSE => [(byte)'f', (byte)'a', (byte)'l', (byte)'s', (byte)'e'];

    public static byte[] FUN => [(byte)'f', (byte)'u', (byte)'n'];

    public static byte[] RETURN => [(byte)'r', (byte)'e', (byte)'t', (byte)'u', (byte)'r', (byte)'n'];

    public static byte[] WHEN => [(byte)'w', (byte)'h', (byte)'e', (byte)'n'];

    public static byte[] FOR => [(byte)'f', (byte)'o', (byte)'r'];
    
    public static byte[] TO => [(byte)'t', (byte)'o'];

    public static byte[] IN => [(byte)'i', (byte)'n'];

    public static byte[] WHILE => [(byte)'w', (byte)'h', (byte)'i', (byte)'l', (byte)'e'];

    public static byte[] CONST => [(byte)'c', (byte)'o', (byte)'n', (byte)'s', (byte)'t'];

    public static byte[] ASYNC => [(byte)'a', (byte)'s', (byte)'y', (byte)'n', (byte)'c'];

    public static byte[] BIND => [(byte)'b', (byte)'i', (byte)'n', (byte)'d'];

    public static byte[] SUCCESS => [(byte)'s', (byte)'u', (byte)'c', (byte)'c', (byte)'e', (byte)'s', (byte)'s'];

    public static byte[] ERROR => [(byte)'e', (byte)'r', (byte)'r', (byte)'o', (byte)'r'];

    public static byte[] TRY => [(byte)'t', (byte)'r', (byte)'y'];

    public static byte[] TYPE => [(byte)'t', (byte)'y', (byte)'p', (byte)'e'];

    public static byte[] EMIT => [(byte)'e', (byte)'m', (byte)'i', (byte)'t'];

    public static byte[] EVENT => [(byte)'e', (byte)'v', (byte)'e', (byte)'n', (byte)'t'];

    public static byte[] SUB => [(byte)'s', (byte)'u', (byte)'b'];

    public static byte[] AND => [(byte)'a', (byte)'n', (byte)'d'];

    public static byte[] OR => [(byte)'o', (byte)'r'];

    public static byte[] NOT => [(byte)'n', (byte)'o', (byte)'t'];



    public static TokenType IsKeyword(byte[] chars)
    {
        if (LET.SequenceEqual(chars)) return TokenType.LET;
        if (IF.SequenceEqual(chars)) return TokenType.IF;
        if (ELSE.SequenceEqual(chars)) return TokenType.ELSE;
        if (ON.SequenceEqual(chars)) return TokenType.ON;
        if (TRUE.SequenceEqual(chars)) return TokenType.TRUE;
        if (FALSE.SequenceEqual(chars)) return TokenType.FALSE;
        if (FUN.SequenceEqual(chars)) return TokenType.FUNCTION;
        if (RETURN.SequenceEqual(chars)) return TokenType.RETURN;
        if (WHEN.SequenceEqual(chars)) return TokenType.WHEN;
        if (FOR.SequenceEqual(chars)) return TokenType.FOR;
        if (TO.SequenceEqual(chars)) return TokenType.TO;
        if (IN.SequenceEqual(chars)) return TokenType.IN;
        if (WHILE.SequenceEqual(chars)) return TokenType.WHILE;
        if (CONST.SequenceEqual(chars)) return TokenType.CONST;
        if (ASYNC.SequenceEqual(chars)) return TokenType.ASYNC;
        if (BIND.SequenceEqual(chars)) return TokenType.BIND;
        if (SUCCESS.SequenceEqual(chars)) return TokenType.SUCCESS;
        if (ERROR.SequenceEqual(chars)) return TokenType.ERROR;
        if (TRY.SequenceEqual(chars)) return TokenType.TRY;
        if (TYPE.SequenceEqual(chars)) return TokenType.TYPE;
        if (SUB.SequenceEqual(chars)) return TokenType.SUB;
        if (EMIT.SequenceEqual(chars)) return TokenType.EMIT;
        if (EVENT.SequenceEqual(chars)) return TokenType.EVENT;
        if (AND.SequenceEqual(chars)) return TokenType.AND;
        if (OR.SequenceEqual(chars)) return TokenType.OR;
        if (NOT.SequenceEqual(chars)) return TokenType.NOT;
        return TokenType.IDENTIFIER;
    }
}
