using System;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class TagAnalyzer : Analyzer
{
    public TagAnalyzer(Submission submission) : base(submission)
    {
    }

    public override void VisitForStatement(ForStatementSyntax node)
    {
        AddTags(Tags.ConstructForLoop);
        base.VisitForStatement(node);
    }

    public override void VisitForEachStatement(ForEachStatementSyntax node)
    {
        AddTags(Tags.ConstructForeach, Tags.ConstructEnumeration);
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
        
        if (node.Default is not null)
            AddTags(Tags.ConstructOptionalParameter);

        AddTags(Tags.ConstructParameter);
        base.VisitParameter(node);
    }

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        var symbol = GetDeclaredSymbol(node);
        if (symbol is not null && symbol.ContainingType.GetMembers(symbol.Name).Length > 1)
            AddTags(Tags.ConstructMethodOverloading);
        
        if (node.TypeParameterList != null)
            AddTags(Tags.ConstructGenericMethod);
        
        if (node.ExpressionBody is not null)
            AddTags(Tags.UsesExpressionBodiedMember);

        if (UsesRecursion(node))
            AddTags(Tags.TechniqueRecursion, Tags.ParadigmFunctional);

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

        var symbol = SemanticModel.GetSymbolInfo(node).Symbol;
        if (symbol is not null)
        {
            if (symbol.ContainingNamespace.ToDisplayString() == "System.Linq")
                AddTags(Tags.UsesLinq, Tags.ParadigmFunctional);
        }

        base.VisitInvocationExpression(node);
    }

    public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
    {
        VisitTypeInfo(GetTypeInfo(node.Type));

        base.VisitVariableDeclaration(node);
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
                AddTags(Tags.TechniqueBitManipulation, Tags.TechniqueBitShifting, Tags.ConstructLeftShift);
                break;
            case SyntaxKind.LeftShiftAssignmentExpression:
                AddTags(Tags.TechniqueBitManipulation, Tags.TechniqueBitShifting, Tags.ConstructLeftShift, Tags.TechniqueCompoundAssignment);
                break;
            case SyntaxKind.RightShiftExpression:
                AddTags(Tags.TechniqueBitManipulation, Tags.TechniqueBitShifting, Tags.ConstructRightShift);
                break;
            case SyntaxKind.RightShiftAssignmentExpression:
                AddTags(Tags.TechniqueBitManipulation, Tags.TechniqueBitShifting, Tags.ConstructRightShift, Tags.TechniqueCompoundAssignment);
                break;
            case SyntaxKind.AsExpression:
                AddTags(Tags.ConstructAsCast, Tags.TechniqueTypeConversion);
                break;
            case SyntaxKind.MultiplyExpression:
                AddTags(Tags.ConstructMultiply);
                break;
            case SyntaxKind.DivideExpression:
                AddTags(Tags.ConstructDivide);
                break;
            case SyntaxKind.AddExpression:
                AddTags(Tags.ConstructAdd);
                break;
            case SyntaxKind.SubtractExpression:
                AddTags(Tags.ConstructSubtract);
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

    public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
    {
        VisitTypeInfo(GetTypeInfo(node.Expression));
        
        base.VisitMemberAccessExpression(node);
    }

    public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
    {
        AddTags(Tags.ConstructProperty);
        
        if (node.ExpressionBody is not null)
            AddTags(Tags.UsesExpressionBodiedMember);
        
        VisitTypeInfo(GetTypeInfo(node.Type));

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
        
        if (UsesRecursion(node))
            AddTags(Tags.TechniqueRecursion, Tags.ParadigmFunctional);
        
        base.VisitLocalFunctionStatement(node);
    }

    public override void VisitLiteralExpression(LiteralExpressionSyntax node)
    {
        VisitTypeInfo(GetTypeInfo(node));

        switch (node.Kind())
        {
            case SyntaxKind.NumericLiteralExpression:
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
                break;
            case SyntaxKind.StringLiteralExpression:
                var lineSpan = node.GetLocation().GetLineSpan();
                if (lineSpan.EndLinePosition.Line > lineSpan.StartLinePosition.Line)
                    AddTags(Tags.ConstructMultilineString);

                if (node.Token.Text.StartsWith('@'))
                    AddTags(Tags.ConstructVerbatimString);
                break;
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
        AddTags(Tags.TechniqueLocks, Tags.ConstructLock);
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
        AddTags(Tags.ConstructAssignment, Tags.ParadigmImperative);
        base.VisitAssignmentExpression(node);
    }

    private void VisitTypeSymbol(ITypeSymbol typeSymbol)
    {
        switch (typeSymbol?.SpecialType)
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
            case SpecialType.System_Enum:
                AddTags(Tags.ConstructEnum);
                break;
            case SpecialType.System_MulticastDelegate:
                AddTags(Tags.UsesDelegate);
                break;
            case SpecialType.System_Delegate:
                AddTags(Tags.UsesDelegate);
                break;
            case SpecialType.System_Collections_IEnumerable:
                AddTags(Tags.UsesIEnumerable);
                break;
            case SpecialType.System_Collections_Generic_IEnumerable_T:
                AddTags(Tags.UsesIEnumerable, Tags.ConstructGenericType);
                break;
            case SpecialType.System_Collections_Generic_IList_T:
                AddTags(Tags.UsesIList, Tags.ConstructGenericType);
                break;
            case SpecialType.System_Collections_Generic_ICollection_T:
                AddTags(Tags.UsesICollection, Tags.ConstructGenericType);
                break;
            case SpecialType.System_Collections_IEnumerator:
                AddTags(Tags.UsesIEnumerator);
                break;
            case SpecialType.System_Collections_Generic_IEnumerator_T:
                AddTags(Tags.UsesIEnumerator);
                break;
            case SpecialType.System_Collections_Generic_IReadOnlyList_T:
                AddTags(Tags.UsesIReadOnlyList, Tags.TechniqueImmutability);
                break;
            case SpecialType.System_Collections_Generic_IReadOnlyCollection_T:
                AddTags(Tags.UsesIReadOnlyCollection, Tags.TechniqueImmutability);
                break;
            case SpecialType.System_Nullable_T:
                AddTags(Tags.ConstructNullable);
                break;
            case SpecialType.System_DateTime:
                AddTags(Tags.ConstructDateTime);
                break;
            case SpecialType.System_IDisposable:
                AddTags(Tags.UsesIDisposable, Tags.TechniqueMemoryManagement);
                break;
        }

        if (typeSymbol is not INamedTypeSymbol namedTypeSymbol)
            return;

        if (namedTypeSymbol.IsGenericType)
        {
            switch (namedTypeSymbol.ConstructedFrom.ToDisplayString())
            {
                case "System.Collections.Generic.List<T>":
                    AddTags(Tags.ConstructList, Tags.UsesList);
                    break;
                case "System.Collections.Generic.SortedList<T>":
                    AddTags(Tags.ConstructList, Tags.UsesSortedList, Tags.TechniqueSorting, Tags.TechniqueSortedCollection);
                    break;
                case "System.Collections.Generic.Dictionary<TKey, TValue>":
                    AddTags(Tags.ConstructDictionary, Tags.UsesDictionary);
                    break;
                case "System.Collections.Generic.IDictionary<TKey, TValue>":
                    AddTags(Tags.ConstructDictionary);
                    break;
                case "System.Collections.Generic.SortedDictionary<TKey, TValue>":
                    AddTags(Tags.ConstructDictionary, Tags.UsesSortedDictionary, Tags.TechniqueSorting, Tags.TechniqueSortedCollection);
                    break;
                case "System.Collections.Generic.HashSet<T>":
                    AddTags(Tags.ConstructSet, Tags.UsesHashSet);
                    break;
                case "System.Collections.Generic.SortedSet<T>":
                    AddTags(Tags.ConstructSet, Tags.UsesSortedSet, Tags.TechniqueSorting, Tags.TechniqueSortedCollection);
                    break;
                case "System.Collections.Generic.Stack<T>":
                    AddTags(Tags.ConstructStack, Tags.UsesStack);
                    break;
                case "System.Collections.Generic.Queue<T>":
                    AddTags(Tags.ConstructQueue, Tags.UsesQueue);
                    break;
                case "System.Collections.Generic.LinkedList<T>":
                    AddTags(Tags.ConstructLinkedList, Tags.UsesLinkedList);
                    break;
                case "System.Span<T>":
                    AddTags(Tags.UsesSpan, Tags.TechniquePerformance, Tags.TechniqueMemoryManagement);
                    break;
                case "System.ReadOnlySpan<T>":
                    AddTags(Tags.UsesSpan, Tags.TechniquePerformance, Tags.TechniqueMemoryManagement, Tags.TechniqueImmutability);
                    break;
                case "System.Memory<T>":
                    AddTags(Tags.UsesMemory, Tags.TechniquePerformance, Tags.TechniqueMemoryManagement);
                    break;
                case "System.ReadOnlyMemory<T>":
                    AddTags(Tags.UsesMemory, Tags.TechniquePerformance, Tags.TechniqueMemoryManagement, Tags.TechniqueImmutability);
                    break;
            }
        }
        else
        {
            switch (namedTypeSymbol.ToDisplayString())
            {
                case "System.Text.RegularExpressions.Regex":
                    AddTags(Tags.TechniqueRegularExpression, Tags.UsesRegex);
                    break;
                case "System.Threading.Mutex":    
                    AddTags(Tags.TechniqueMutexes, Tags.UsesMutex);
                    break;
                case "System.Numerics.BigInteger":    
                    AddTags(Tags.ConstructIntegralNumber, Tags.ConstructBigInteger);
                    break;
                case "System.Collections.BitArray":    
                    AddTags(Tags.ConstructBitArray);
                    break;
                case "System.Text.StringBuilder":    
                    AddTags(Tags.UsesStringBuilder);
                    break;
                case "System.Random":    
                    AddTags(Tags.TechniqueRandomess, Tags.UsesRandom);
                    break;
            }
        }
    }

    private void VisitTypeInfo(TypeInfo typeInfo)
    {
        if (typeInfo.ConvertedType != null)
            VisitTypeSymbol(typeInfo.ConvertedType);
    }

    public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
    {
        if (GetSymbol(node.Type) is ITypeSymbol typeSymbol)
            VisitTypeSymbol(typeSymbol);
        else
            VisitTypeInfo(GetTypeInfo(node.Type));

        AddTags(Tags.ConstructConstructor);
        base.VisitObjectCreationExpression(node);
    }

    public override void VisitWhileStatement(WhileStatementSyntax node)
    {
        AddTags(Tags.ConstructWhileLoop);
        base.VisitWhileStatement(node);
    }

    public override void VisitDoStatement(DoStatementSyntax node)
    {
        AddTags(Tags.ConstructDoLoop);
        base.VisitDoStatement(node);
    }

    public override void VisitThrowExpression(ThrowExpressionSyntax node)
    {
        AddTags(Tags.ConstructException);
        base.VisitThrowExpression(node);
    }

    public override void VisitThrowStatement(ThrowStatementSyntax node)
    {
        AddTags(Tags.ConstructException);
        base.VisitThrowStatement(node);
    }

    public override void VisitYieldStatement(YieldStatementSyntax node)
    {
        AddTags(Tags.TechniqueLaziness, Tags.UsesYield);
        base.VisitYieldStatement(node);
    }

    public override void VisitElementAccessExpression(ElementAccessExpressionSyntax node)
    {
        VisitTypeInfo(GetTypeInfo(node.Expression));
        base.VisitElementAccessExpression(node);
    }

    private bool UsesRecursion(SyntaxNode methodOrFunctionNode)
    {
        var methodOrFunctionSymbol = SemanticModel.GetDeclaredSymbol(methodOrFunctionNode);
        if (methodOrFunctionSymbol is null)
            return false;

        return methodOrFunctionNode.DescendantNodes()
            .OfType<InvocationExpressionSyntax>()
            .Select(invocationExpression => GetSymbolInfo(invocationExpression.Expression).Symbol)
            .Any(invokedSymbol => methodOrFunctionSymbol.Equals(invokedSymbol, SymbolEqualityComparer.IncludeNullability));
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
        public const string TechniqueBitShifting = "technique:bit-shifting";
        public const string TechniqueBooleanLogic = "technique:boolean-logic";
        public const string TechniqueLaziness = "technique:laziness";
        public const string TechniqueParallelism = "technique:parallelism";
        public const string TechniqueConcurrency = "technique:concurrency";
        public const string TechniqueImmutability = "technique:immutability";
        public const string TechniqueCompoundAssignment = "technique:compound-assignment";
        public const string TechniquePointers = "technique:pointers";
        public const string TechniquePerformance = "technique:performance";
        public const string TechniqueMemoryManagement = "technique:memory-management";
        public const string TechniqueSorting = "technique:sorting";
        public const string TechniqueSortedCollection = "technique:sorted-collection";
        public const string TechniqueRandomess = "technique:randomness";

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
        public const string ConstructOptionalParameter = "construct:optional-parameter";
        public const string ConstructSwitchExpression = "construct:switch-expression";
        public const string ConstructSwitch = "construct:switch";
        public const string ConstructForeach = "construct:foreach";
        public const string ConstructForLoop = "construct:for-loop";
        public const string ConstructWhileLoop = "construct:while-loop";
        public const string ConstructDoLoop = "construct:do-loop";
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
        public const string ConstructAssignment = "construct:assignment";
        public const string ConstructEnumeration = "construct:enumeration";
        public const string ConstructException = "construct:exception";
        public const string ConstructMultiply = "construct:multiply";
        public const string ConstructDivide = "construct:divide";
        public const string ConstructAdd = "construct:add";
        public const string ConstructSubtract = "construct:subtract";
        public const string ConstructMethodOverloading = "construct:method-overloading";
        public const string ConstructLock = "construct:lock";
        public const string ConstructConstructor = "construct:constructor";

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
        public const string ConstructDictionary = "construct:dictionary";
        public const string ConstructStruct = "construct:struct";
        public const string ConstructRecord = "construct:record";
        public const string ConstructClass = "construct:class";
        public const string ConstructInterface = "construct:interface";
        public const string ConstructTuple = "construct:tuple";
        public const string ConstructDateTime = "construct:date-time";
        public const string ConstructNullable = "construct:nullable";
        public const string ConstructEnum = "construct:enum";
        public const string ConstructLinkedList = "construct:linked-list";
        public const string ConstructBitArray = "construct:bit-array";

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
        public const string UsesYield = "uses:yield";

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
        public const string UsesSpan = "uses:Span<T>";
        public const string UsesMemory = "uses:Memort<T>";
        public const string UsesMutex = "uses:Mutex";
        public const string UsesDelegate = "uses:delegate";
        public const string UsesRegex = "uses:Regex";
        public const string UsesStringBuilder = "uses:StringBuilder";
        public const string UsesRandom = "uses:Random";
        
        // Uses - collections
        public const string UsesList = "uses:List<T>";
        public const string UsesSortedList = "uses:SortedList<T>";
        public const string UsesDictionary = "uses:Dictionary<TKey,TValue>";
        public const string UsesSortedDictionary = "uses:SortedDictionary<TKey,TValue>";
        public const string UsesHashSet = "uses:HashSet<T>";
        public const string UsesSortedSet = "uses:SortedSet<T>";
        public const string UsesStack = "uses:Stack<T>";
        public const string UsesQueue = "uses:Queue<T>";
        public const string UsesLinkedList = "uses:LinkedList<T>";
        
        // Uses - interfaces
        public const string UsesIReadOnlyList = "uses:IReadOnlyList<T>";
        public const string UsesIReadOnlyCollection = "uses:IReadOnlyCollection<T>";
        public const string UsesIList = "uses:IList<T>";
        public const string UsesICollection = "uses:ICollection<T>";
        public const string UsesIDisposable = "uses:IDisposable";
        public const string UsesIEnumerable = "uses:IEnumerable<T>";
        public const string UsesIEnumerator = "uses:IEnumerator<T>";
    }
}
