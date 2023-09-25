using System;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class TagAnalyzer : Analyzer
{
    private readonly Lazy<INamespaceOrTypeSymbol> _linqNamespaceSymbol;
    private INamespaceOrTypeSymbol LinqNamespaceSymbol => _linqNamespaceSymbol.Value;
    
    public TagAnalyzer(Submission submission) : base(submission) => 
        _linqNamespaceSymbol = new Lazy<INamespaceOrTypeSymbol>(() => Compilation.GetTypeByMetadataName("System.Linq.Enumerable")?.ContainingNamespace);

    public override void VisitForStatement(ForStatementSyntax node)
    {
        AddTags(Tags.ConstructFor);
        base.VisitForStatement(node);
    }

    public override void VisitForEachStatement(ForEachStatementSyntax node)
    {
        AddTags(Tags.ConstructForeach);
        base.VisitForEachStatement(node);
    }

    public override void VisitIfStatement(IfStatementSyntax node)
    {
        AddTags(Tags.ConstructIf);
        base.VisitIfStatement(node);
    }

    public override void VisitSwitchStatement(SwitchStatementSyntax node)
    {
        AddTags(Tags.ConstructSwitch);
        base.VisitSwitchStatement(node);
    }

    public override void VisitSwitchExpression(SwitchExpressionSyntax node)
    {
        AddTags(Tags.ConstructSwitchExpression, Tags.ConstructPatternMatching);
        base.VisitSwitchExpression(node);
    }

    public override void VisitParameter(ParameterSyntax node)
    {
        if (node.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.ThisKeyword)))
            AddTags(Tags.ConstructExtensionMethod);

        AddTags(Tags.ConstructParameter);
        base.VisitParameter(node);
    }

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        if (node.TypeParameterList != null)
            AddTags(Tags.ConstructGenericMethod);
        
        if (node.ExpressionBody is not null)
            AddTags(Tags.UsesExpressionBodiedMember);

        AddTags(Tags.ConstructMethod);
        base.VisitMethodDeclaration(node);
    }

    public override void VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        if (node.TypeParameterList != null)
            AddTags(Tags.ConstructGenericType);

        AddTags(Tags.ConstructClass, Tags.ParadigmObjectOriented);
        base.VisitClassDeclaration(node);
    }

    public override void VisitStructDeclaration(StructDeclarationSyntax node)
    {
        if (node.TypeParameterList != null)
            AddTags(Tags.ConstructGenericType);

        AddTags(Tags.ConstructStruct, Tags.ParadigmObjectOriented);
        base.VisitStructDeclaration(node);
    }

    public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
    {
        if (node.TypeParameterList != null)
            AddTags(Tags.ConstructGenericType);

        AddTags(Tags.ConstructRecord, Tags.ParadigmObjectOriented, Tags.ParadigmFunctional);
        base.VisitRecordDeclaration(node);
    }

    public override void VisitTupleExpression(TupleExpressionSyntax node)
    {
        AddTags(Tags.ConstructTuple);
        base.VisitTupleExpression(node);
    }

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        AddTags(Tags.ConstructInvocation);

        // TODO: properly detect recursion
        if (node.Expression is IdentifierNameSyntax identifierName)
        {
            var parent = node.AncestorsAndSelf()
                .FirstOrDefault(node => node is MethodDeclarationSyntax or LocalFunctionStatementSyntax);

            if (parent is MethodDeclarationSyntax methodDeclaration &&
                methodDeclaration.Identifier.Text == identifierName.Identifier.Text ||
                parent is LocalFunctionStatementSyntax localFunctionStatement &&
                localFunctionStatement.Identifier.Text == identifierName.Identifier.Text)
            {
                AddTags(Tags.TechniqueRecursion);
            }
        }
        
        var symbol = SemanticModel.GetSymbolInfo(node).Symbol;
        if (symbol is not null && symbol.ContainingNamespace.Equals(LinqNamespaceSymbol, SymbolEqualityComparer.Default))
            AddTags(Tags.UsesLinq, Tags.ParadigmFunctional);

        base.VisitInvocationExpression(node);
    }

    public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
    {
        AddTags(Tags.ConstructInterface);
        base.VisitInterfaceDeclaration(node);
    }

    public override void VisitConditionalExpression(ConditionalExpressionSyntax node)
    {
        AddTags(Tags.ConstructTernary);
        base.VisitConditionalExpression(node);
    }

    public override void VisitBinaryExpression(BinaryExpressionSyntax node)
    {
        switch (node.Kind())
        {
            case SyntaxKind.LogicalAndExpression:
                AddTags(Tags.ConstructBoolean, Tags.ConstructLogicalAnd, Tags.TechniqueBooleanLogic);
                break;
            case SyntaxKind.LogicalOrExpression:
                AddTags(Tags.ConstructBoolean, Tags.ConstructLogicalOr, Tags.TechniqueBooleanLogic);
                break;
            case SyntaxKind.LogicalNotExpression:
                AddTags(Tags.ConstructBoolean, Tags.ConstructLogicalNot, Tags.TechniqueBooleanLogic);
                break;
            case SyntaxKind.BitwiseAndExpression:
                AddTags(Tags.TechniqueBitManipulation, Tags.ConstructBitwiseAnd);
                break;
            case SyntaxKind.BitwiseOrExpression:
                AddTags(Tags.TechniqueBitManipulation, Tags.ConstructBitwiseOr);
                break;
            case SyntaxKind.BitwiseNotExpression:
                AddTags(Tags.TechniqueBitManipulation, Tags.ConstructBitwiseNot);
                break;
            case SyntaxKind.ExclusiveOrExpression:
                AddTags(Tags.TechniqueBitManipulation, Tags.ConstructBitwiseXor);
                break;
            case SyntaxKind.ExclusiveOrAssignmentExpression:
                AddTags(Tags.TechniqueBitManipulation, Tags.ConstructBitwiseXor, Tags.TechniqueCompoundAssignment);
                break;
            case SyntaxKind.LeftShiftExpression:
                AddTags(Tags.TechniqueBitManipulation, Tags.ConstructLeftShift);
                break;
            case SyntaxKind.LeftShiftAssignmentExpression:
                AddTags(Tags.TechniqueBitManipulation, Tags.ConstructLeftShift, Tags.TechniqueCompoundAssignment);
                break;
            case SyntaxKind.RightShiftExpression:
                AddTags(Tags.TechniqueBitManipulation, Tags.ConstructRightShift);
                break;
            case SyntaxKind.RightShiftAssignmentExpression:
                AddTags(Tags.TechniqueBitManipulation, Tags.ConstructRightShift, Tags.TechniqueCompoundAssignment);
                break;
            case SyntaxKind.AsExpression:
                AddTags(Tags.ConstructAsCast, Tags.TechniqueTypeConversion);
                break;
        }
        
        base.VisitBinaryExpression(node);
    }

    public override void VisitToken(SyntaxToken token)
    {
        switch (token.Kind())
        {
            case SyntaxKind.PublicKeyword:
            case SyntaxKind.ProtectedKeyword:
            case SyntaxKind.PrivateKeyword:
            case SyntaxKind.InternalKeyword:
                AddTags(Tags.ConstructVisibilityModifiers);
                break;
            case SyntaxKind.ReturnKeyword:
                AddTags(Tags.ConstructReturn);
                break;
            case SyntaxKind.BreakKeyword:
                AddTags(Tags.ConstructBreak);
                break;
            case SyntaxKind.ContinueKeyword:
                AddTags(Tags.ConstructContinue);
                break;
        }

        base.VisitToken(token);
    }

    public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
    {
        AddTags(Tags.ConstructProperty);
        
        if (node.ExpressionBody is not null)
            AddTags(Tags.UsesExpressionBodiedMember);

        var accessors = node.AccessorList?.Accessors;
        var getAccessor = accessors?.FirstOrDefault(accessor => accessor.IsKind(SyntaxKind.GetAccessorDeclaration));
        var setAccessor = accessors?.FirstOrDefault(accessor => accessor.IsKind(SyntaxKind.SetAccessorDeclaration));

        if (getAccessor != null)
            AddTags(Tags.ConstructGetter);

        if (setAccessor != null)
            AddTags(Tags.ConstructSetter);

        if (getAccessor is {Body: null} && setAccessor is {Body: null} or null)
            AddTags(Tags.UsesAutoImplementedProperty);

        base.VisitPropertyDeclaration(node);
    }

    public override void VisitLocalFunctionStatement(LocalFunctionStatementSyntax node)
    {
        AddTags(Tags.ConstructLocalFunction);
        base.VisitLocalFunctionStatement(node);
    }

    public override void VisitLiteralExpression(LiteralExpressionSyntax node)
    {
        var typeInfo = SemanticModel.GetTypeInfo(node);
        
        switch (typeInfo.ConvertedType?.SpecialType)
        {
            case SpecialType.System_Int16:
                AddTags(Tags.ConstructIntegralNumber, Tags.UsesShort);
                break;
            case SpecialType.System_Int32:
                AddTags(Tags.ConstructIntegralNumber, Tags.UsesInt);
                break;
            case SpecialType.System_Int64:
                AddTags(Tags.ConstructIntegralNumber, Tags.UsesLong);
                break;
            case SpecialType.System_Byte:
                AddTags(Tags.ConstructIntegralNumber, Tags.UsesByte);
                break;
            case SpecialType.System_UInt16:
                AddTags(Tags.ConstructIntegralNumber, Tags.UsesUshort);
                break;
            case SpecialType.System_UInt32:
                AddTags(Tags.ConstructIntegralNumber, Tags.UsesUint);
                break;
            case SpecialType.System_UInt64:
                AddTags(Tags.ConstructIntegralNumber, Tags.UsesUlong);
                break;
            case SpecialType.System_SByte:
                AddTags(Tags.ConstructIntegralNumber, Tags.UsesSbyte);
                break;
            case SpecialType.System_IntPtr:
                AddTags(Tags.ConstructIntegralNumber, Tags.UsesNint);
                break;
            case SpecialType.System_UIntPtr:
                AddTags(Tags.ConstructIntegralNumber, Tags.UsesNuint);
                break;
            case SpecialType.System_Single:
                AddTags(Tags.ConstructFloatingPointNumber, Tags.UsesFloat);
                break;
            case SpecialType.System_Double:
                AddTags(Tags.ConstructFloatingPointNumber, Tags.UsesDouble);
                break;
            case SpecialType.System_Decimal:
                AddTags(Tags.ConstructFloatingPointNumber, Tags.UsesDecimal);
                break;
            case SpecialType.System_String:
                AddTags(Tags.ConstructString);
                
                var lineSpan = node.GetLocation().GetLineSpan();
                if (lineSpan.EndLinePosition.Line > lineSpan.StartLinePosition.Line)
                    AddTags(Tags.ConstructMultilineString);

                if (node.Token.Text.StartsWith('@'))
                    AddTags(Tags.ConstructVerbatimString);
                
                break;
            case SpecialType.System_Boolean:
                AddTags(Tags.ConstructBoolean);
                break;
            case SpecialType.System_Array:
                AddTags(Tags.ConstructArray);
                break;
            case SpecialType.System_Char:
                AddTags(Tags.ConstructChar);
                break;
        }
        
        if (node.IsKind(SyntaxKind.NumericLiteralExpression))
        {
            AddTags(Tags.ConstructNumber);
            
            if (node.Token.Text.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                AddTags(Tags.ConstructHexadecimalNumber);
            else if (node.Token.Text.StartsWith("0b", StringComparison.OrdinalIgnoreCase))
                AddTags(Tags.ConstructBinaryNumber);
            else if (node.Token.Text.Contains('.', StringComparison.OrdinalIgnoreCase) && 
                     node.Token.Text.Contains('e', StringComparison.OrdinalIgnoreCase))
                AddTags(Tags.ConstructScientificNumber);
            
            if (node.Token.Text.Contains('_'))
                AddTags(Tags.ConstructUnderscoreNumberNotation);
        }

        base.VisitLiteralExpression(node);
    }

    public override void VisitInterpolatedStringExpression(InterpolatedStringExpressionSyntax node)
    {   
        AddTags(Tags.ConstructStringInterpolation);
        base.VisitInterpolatedStringExpression(node);
    }

    public override void VisitLockStatement(LockStatementSyntax node)
    {
        AddTags(Tags.TechniqueLocks);
        base.VisitLockStatement(node);
    }

    public override void VisitAwaitExpression(AwaitExpressionSyntax node)
    {
        AddTags(Tags.ConstructAsyncAwait);
        base.VisitAwaitExpression(node);
    }

    public override void VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
    {
        AddTags(Tags.ConstructLambda, Tags.ParadigmFunctional);
        base.VisitSimpleLambdaExpression(node);
    }

    public override void VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
    {
        AddTags(Tags.ConstructLambda, Tags.ParadigmFunctional);
        base.VisitParenthesizedLambdaExpression(node);
    }

    public override void VisitCastExpression(CastExpressionSyntax node)
    {
        AddTags(Tags.ConstructCast, Tags.TechniqueTypeConversion);
        base.VisitCastExpression(node);
    }

    public override void VisitIsPatternExpression(IsPatternExpressionSyntax node)
    {
        AddTags(Tags.ConstructIsCast, Tags.ConstructPatternMatching);
        base.VisitIsPatternExpression(node);
    }

    public override void VisitIdentifierName(IdentifierNameSyntax node)
    {
        if (node.Identifier.IsKind(SyntaxKind.VarKeyword))
            AddTags(Tags.ConstructTypeInference);
        
        base.VisitIdentifierName(node);
    }

    public override void VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
    {
        if (node.ExpressionBody is not null)
            AddTags(Tags.UsesExpressionBodiedMember);
        
        base.VisitConstructorDeclaration(node);
    }

    public override void VisitQueryExpression(QueryExpressionSyntax node)
    {
        AddTags(Tags.ConstructQueryExpression, Tags.UsesLinq, Tags.ParadigmFunctional, Tags.ParadigmDeclarative);
        base.VisitQueryExpression(node);
    }

    public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
    {
        AddTags(Tags.ConstructField);
        base.VisitFieldDeclaration(node);
    }

    public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
    {
        AddTags(Tags.ParadigmImperative);
        base.VisitAssignmentExpression(node);
    }

    private static class Tags
    {
        // Paradigms
        public const string ParadigmDeclarative = "paradigm:declarative";
        public const string ParadigmFunctional = "paradigm:functional";
        public const string ParadigmImperative = "paradigm:imperative";
        public const string ParadigmObjectOriented = "paradigm:object-oriented";

        // Techniques
        public const string TechniqueRecursion = "technique:recursion";
        public const string TechniqueCustomComparer = "technique:custom-comparer";
        public const string TechniqueLocks = "technique:locks";
        public const string TechniqueMutexes = "technique:mutexes";
        public const string TechniqueAnswerArray = "technique:answer-array";
        public const string TechniqueRegularExpression = "technique:regular-expression";
        public const string TechniqueIteration = "technique:iteration";
        public const string TechniqueMath = "technique:math";
        public const string TechniqueMapToInteger = "technique:map-to-integer";
        public const string TechniqueTypeConversion = "technique:type-conversion";
        public const string TechniqueHigherOrderFunctions = "technique:higher-order-functions";
        public const string TechniqueBitManipulation = "technique:bit-manipulation";
        public const string TechniqueBooleanLogic = "technique:boolean-logic";
        public const string TechniqueLaziness = "technique:laziness";
        public const string TechniqueParallelism = "technique:parallelism";
        public const string TechniqueConcurrency = "technique:concurrency";
        public const string TechniqueImmutability = "technique:immutability";
        public const string TechniqueCompoundAssignment = "technique:compound-assignment";
        public const string TechniquePointers = "technique:pointers";

        // Constructs
        public const string ConstructIf = "construct:if";
        public const string ConstructTernary = "construct:ternary";
        public const string ConstructGenericType = "construct:generic-type";
        public const string ConstructGenericMethod = "construct:generic-method";
        public const string ConstructInvocation = "construct:invocation";
        public const string ConstructMethod = "construct:method";
        public const string ConstructExtensionMethod = "construct:extension-method";
        public const string ConstructLocalFunction = "construct:local-function";
        public const string ConstructParameter = "construct:parameter";
        public const string ConstructSwitchExpression = "construct:switch-expression";
        public const string ConstructSwitch = "construct:switch";
        public const string ConstructForeach = "construct:foreach";
        public const string ConstructFor = "construct:for";
        public const string ConstructLogicalAnd = "construct:logical-and";
        public const string ConstructLogicalOr = "construct:logical-or";
        public const string ConstructLogicalNot = "construct:logical-not";
        public const string ConstructLeftShift = "construct:left-shift";
        public const string ConstructRightShift = "construct:right-shift";
        public const string ConstructBitwiseAnd = "construct:bitwise-and";
        public const string ConstructBitwiseOr = "construct:bitwise-or";
        public const string ConstructBitwiseNot = "construct:bitwise-not";
        public const string ConstructBitwiseXor = "construct:bitwise-xor";
        public const string ConstructAsyncAwait = "construct:async-await";
        public const string ConstructLambda = "construct:lambda";
        public const string ConstructField = "construct:field";
        public const string ConstructProperty = "construct:property";
        public const string ConstructGetter = "construct:getter";
        public const string ConstructSetter = "construct:setter";
        public const string ConstructIsCast = "construct:is-cast";
        public const string ConstructAsCast = "construct:as-cast";
        public const string ConstructCast = "construct:cast";
        public const string ConstructVisibilityModifiers = "construct:visibility-modifiers";
        public const string ConstructPatternMatching = "construct:pattern-matching";
        public const string ConstructBreak = "construct:break";
        public const string ConstructContinue = "construct:continue";
        public const string ConstructReturn = "construct:return";
        public const string ConstructTypeInference = "construct:type-inference";
        public const string ConstructQueryExpression = "construct:query-expression";

        // Constructs - types
        public const string ConstructBoolean = "construct:boolean";
        public const string ConstructString = "construct:string";
        public const string ConstructChar = "construct:char";
        public const string ConstructNumber = "construct:number";
        public const string ConstructIntegralNumber = "construct:integral-number";
        public const string ConstructFloatingPointNumber = "construct:floating-point-number";
        public const string ConstructBigInteger = "construct:big-integer";
        public const string ConstructList = "construct:list";
        public const string ConstructSet = "construct:set";
        public const string ConstructArray = "construct:array";
        public const string ConstructStack = "construct:stack";
        public const string ConstructQueue = "construct:queue";
        public const string ConstructMap = "construct:map";
        public const string ConstructStruct = "construct:struct";
        public const string ConstructRecord = "construct:record";
        public const string ConstructClass = "construct:class";
        public const string ConstructInterface = "construct:interface";
        public const string ConstructTuple = "construct:tuple";

        // Constructs - notation
        public const string ConstructHexadecimalNumber = "construct:hexadecimal-number";
        public const string ConstructBinaryNumber = "construct:binary-number";
        public const string ConstructScientificNumber = "construct:scientific-number";
        public const string ConstructUnderscoreNumberNotation = "construct:underscore-number-notation";
        public const string ConstructMultilineString = "construct-multiline-string";
        public const string ConstructStringInterpolation = "construct-string-interpolation";
        public const string ConstructVerbatimString = "construct-verbatim-string";

        // Uses
        public const string UsesLinq = "uses:linq";
        public const string UsesExpressionBodiedMember = "uses:expression-bodied-member";
        public const string UsesAutoImplementedProperty = "uses:auto-implemented-property";

        // Uses - types
        public const string UsesDecimal = "uses:decimal";
        public const string UsesDouble = "uses:double";
        public const string UsesFloat = "uses:float";
        public const string UsesSbyte = "uses:sbyte";
        public const string UsesByte = "uses:byte";
        public const string UsesShort = "uses:short";
        public const string UsesUshort = "uses:ushort";
        public const string UsesInt = "uses:int";
        public const string UsesUint = "uses:uint";
        public const string UsesLong = "uses:long";
        public const string UsesUlong = "uses:ulong";
        public const string UsesNint = "uses:nint";
        public const string UsesNuint = "uses:nuint";
        public const string UsesSpan = "uses:span";
        public const string UsesMemory = "uses:memory";

        // Uses - members
        public const string UsesUInt64MaxValue = "uses:UInt64.MaxValue";
        public const string UsesEnumerableCount = "uses:Enumerable.Count";
        public const string UsesEnumerableDistinct = "uses:Enumerable.Distinct";
        public const string UsesEnumerableGroupBy = "uses:Enumerable.GroupBy";
        public const string UsesEnumerableAll = "uses:Enumerable.All";
        public const string UsesEnumerableAsParallel = "uses:Enumerable.AsParallel";
        public const string UsesStringToLower = "uses:String.ToLower";
        public const string UsesStringContains = "uses:String.Contains";
        public const string UsesStringContainsWithComparer = "uses:String.Contains(StringComparer)";
        public const string UsesStringSubstring = "uses:String.Substring";
    }
}