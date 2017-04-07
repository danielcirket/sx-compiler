using System;
using System.Collections.Generic;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Lexer.Abstractions;
using Sx.Compiler.Parser.Abstractions.Nodes;

namespace Sx.Compiler.Parser
{
    public class Parser
    {
        private readonly IErrorSink _errorSink;

        public Node Parse(IEnumerable<IEnumerable<IToken>> tokens)
        {
            var nodes = new List<Node>();

            foreach (var file in tokens)
                nodes.Add(Parse(file));

            return new ScopeDeclarationNode(nodes);
        }
        public Node Parse(IEnumerable<IToken> tokens)
        {
            var nodes = new List<Node>();

            //while (_tokenStream.CurrentToken.TokenType != TokenType.EOF) 
            //{ 
            //    var scopeDelaration = ScopeDeclaration(); 
            //    if (scopeDelaration != null) 
            //    { 
            //        nodes.Add(scopeDelaration); 
            //    } 
            //    else 
            //    { 
            //        nodes.Add(Statement()); 
            //    } 
            //} 

            // Wrap everything in a 'application/module' scope. 
            return new ScopeDeclarationNode(nodes);
        }

        protected Node ScopeDeclaration()
        {
            return null;
        }
        protected Node Statement()
        {
            return null;
        }

        private ScopeDeclarationNode GetStatementsForScope(TokenType start = TokenType.LeftBracket, TokenType end = TokenType.RightBracket)
        {
            //TokenStream.Take(open); 
            //var lines = new List<Ast>(); 
            //while (TokenStream.Current.TokenType != close) 
            //{ 
            //    var statement = getter(); 

            //    lines.Add(statement); 

            //    if (expectSemicolon && StatementExpectsSemiColon(statement)) 
            //    { 
            //        TokenStream.Take(TokenType.SemiColon); 
            //    } 
            //} 

            //TokenStream.Take(close); 

            //return new ScopeDeclr(lines); 
            return null;
        }

        public Parser(/*ILexer lexer*/) : this(/*lexer, */new ErrorSink()) { }
        public Parser(/*ILexer lexer,*/ IErrorSink errorSink)
        {
            //if (lexer == null) 
            //    throw new ArgumentNullException(nameof(lexer)); 

            if (errorSink == null)
                throw new ArgumentNullException(nameof(errorSink));


            _errorSink = errorSink;
            //_tokenStream = new ParsableTokenStream<IToken>(lexer); 
        }
    }
}