using System;
using Drift.Core;
using Drift.Core.Ast.Types;
using Drift.Core.Nodes;
using Drift.Core.Nodes.Declarations;
using Drift.Core.Types;
using Drift.Lexer;
using Drift.Parser.Helpers;

namespace Drift.Parser.NodeParser.Declarations;

public class StructDeclarationParser : INodeParser
{
    public bool IsParse(TokenType before, TokenType current, TokenType next)
    {
        return current == TokenType.TYPE;
    }

    public DriftNode Parse(ITokenSource source, DriftNode? parent = null)
    {
        var start = source.Current.Location;
        source.Advance(TokenType.IDENTIFIER);
        var identifier = source.Current;
        source.Advance(TokenType.OPEN_BRACE);

        var properties = new List<StructFieldDeclaration>();
        while (source.Current.Type != TokenType.CLOSE_BRACE)
        {
            source.Advance(TokenType.IDENTIFIER);
            var name = source.Current;
            source.Advance(TokenType.COLON);
            var type = GrammarHelper.TypeParser(source);
            properties.Add(new StructFieldDeclaration(name.Source, type, name.Location.Join(source.Current.Location)));
            source.Advance([TokenType.CLOSE_BRACE, TokenType.COMMA]);
        }

        var end = source.Current.Location;
        source.Advance();

        var dic = new Dictionary<string, IDataType>();
        foreach (var property in properties)
            dic.Add(property.Identifier, property.Type);

        DriftEnv.TypeRegistry.Register(new ComplexType(identifier.Source, dic));
        return new StructDeclaration(identifier.Source, properties.ToArray(), start.Join(end));
    }
}
