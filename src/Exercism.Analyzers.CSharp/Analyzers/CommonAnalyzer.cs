using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class CommonAnalyzer : Analyzer
{
    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        if (node.Body is {Statements: [ReturnStatementSyntax]})
            AddComment(Comments.UseExpressionBodiedMember(node.Identifier.Text));
        
        if (node.Identifier.Text == "Main" && node.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.StaticKeyword)))
            AddComment(Comments.DoNotUseMainMethod);

        base.VisitMethodDeclaration(node);
    }

    public override void VisitBlock(BlockSyntax node)
    {
        if (node.Statements is [.., 
                LocalDeclarationStatementSyntax { Declaration.Variables.Count: 1 } localDeclarationStatement, 
                ReturnStatementSyntax { Expression: IdentifierNameSyntax identifierName }
            ])
        {
            var declaredSymbol = ModelExtensions.GetDeclaredSymbol(SemanticModel, localDeclarationStatement.Declaration.Variables[0]);
            var returnedSymbolInfo = ModelExtensions.GetSymbolInfo(SemanticModel, identifierName);
            if (declaredSymbol!.Equals(returnedSymbolInfo.Symbol, SymbolEqualityComparer.Default))
                AddComment(Comments.DoNotAssignAndReturn);
        }
        
        base.VisitBlock(node);
    }

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        if (node.Expression is MemberAccessExpressionSyntax memberAccessExpression &&
            ConsoleOutputIdentifierNames.Contains(memberAccessExpression.ToString()))
            AddComment(Comments.DoNotWriteToConsole);

        var symbol = SemanticModel.GetSymbolInfo(node).Symbol?.ToString();
        if (symbol == "string.Format(string, object?)")
            AddComment(Comments.UseStringInterpolationNotStringFormat);

        base.VisitInvocationExpression(node);
    }

    public override void VisitBinaryExpression(BinaryExpressionSyntax node)
    {
        var symbol = SemanticModel.GetSymbolInfo(node).Symbol?.ToString();
        if (symbol == "string.operator +(string, string)")
            AddComment(Comments.UseStringInterpolationNotStringConcatenation);
        
        base.VisitBinaryExpression(node);
    }

    public override void VisitConditionalExpression(ConditionalExpressionSyntax node)
    {
        if (node.Condition is BinaryExpressionSyntax { 
                OperatorToken: var token,
                Left: LiteralExpressionSyntax { Token: var leftLiteralToken },
                Right: LiteralExpressionSyntax { Token: var rightLiteralToken }})
            if (token.IsKind(SyntaxKind.EqualsExpression) && rightLiteralToken.IsKind(SyntaxKind.NullLiteralExpression) ||
                token.IsKind(SyntaxKind.NotEqualsExpression) && leftLiteralToken.IsKind(SyntaxKind.NullLiteralExpression))
                AddComment(Comments.UseNullCoalescingOperatorNotNullCheck);
        
        base.VisitConditionalExpression(node);
    }

    private static readonly HashSet<string> ConsoleOutputIdentifierNames = new()
    {
        "Console.Write",
        "Console.WriteLine",
        "Console.Error.Write",
        "Console.Error.WriteLine",
        "Console.Out.Write",
        "Console.Out.WriteLine"
    };

    public CommonAnalyzer(Submission submission) : base(submission)
    {
    }

    private static class Comments
    {
        public static readonly Comment DoNotUseMainMethod = new("csharp.general.has_main_method", CommentType.Essential);

        public static readonly Comment UseNullCoalescingOperatorNotNullCheck =
            new("csharp.general.use_null_coalescing_operator_not_null_check", CommentType.Actionable);

        public static readonly Comment UseStringInterpolationNotStringFormat =
            new("csharp.general.use_string_interpolation_not_string_format", CommentType.Actionable);

        public static readonly Comment UseStringInterpolationNotStringConcatenation =
            new("csharp.general.use_string_interpolation_not_string_concatenation", CommentType.Informative);

        public static readonly Comment DoNotWriteToConsole =
            new("csharp.general.do_not_write_to_console", CommentType.Actionable);

        public static readonly Comment DoNotAssignAndReturn =
            new("csharp.general.do_not_assign_and_return", CommentType.Actionable);

        public static readonly Comment DoNotUseNestedIfStatement =
            new("csharp.general.do_not_use_nested_if_statement", CommentType.Actionable);

        public static Comment UsePrivateVisibility(string field) =>
            new("csharp.general.use_private_visibility", CommentType.Actionable, new CommentParameter("name", field));

        public static Comment ConvertFieldToConst(string field) =>
            new("csharp.general.convert_field_to_const", CommentType.Actionable, new CommentParameter("name", field));

        public static Comment ConvertVariableToConst(string variable) =>
            new("csharp.general.convert_variable_to_const", CommentType.Actionable,
                new CommentParameter("name", variable));

        public static Comment UseExpressionBodiedMember(string method) =>
            new("csharp.general.use_expression_bodied_member", CommentType.Informative,
                new CommentParameter("name", method));

        public static Comment MissingClass(string @class) =>
            new("csharp.general.missing_class", CommentType.Essential, new CommentParameter("name", @class));

        public static Comment MissingMethod(string method) =>
            new("csharp.general.missing_method", CommentType.Essential, new CommentParameter("name", method));

        public static Comment InvalidMethodSignature(string method, string signature) =>
            new("csharp.general.invalid_method_signature", CommentType.Essential, new CommentParameter("name", method),
                new CommentParameter("signature", signature));

        public static Comment MissingProperty(string property) =>
            new("csharp.general.missing_property", CommentType.Essential, new CommentParameter("name", property));

        public static Comment PropertyIsNotAutoProperty(string name) =>
            new("csharp.general.property_is_not_auto_property", CommentType.Actionable,
                new CommentParameter("name", name));

        public static Comment PropertyHasNonPrivateSetter(string name) =>
            new("csharp.general.property_setter_is_not_private", CommentType.Actionable,
                new CommentParameter("name", name));

        public static Comment PropertyBetterUseInitializer(string name) =>
            new("csharp.general.property_better_use_initializer", CommentType.Actionable,
                new CommentParameter("name", name));
    }
}