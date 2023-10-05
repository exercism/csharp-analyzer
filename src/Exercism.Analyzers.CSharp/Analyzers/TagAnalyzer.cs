using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class TagAnalyzer : Analyzer
{
    public TagAnalyzer(Submission submission) : base(submission, SyntaxWalkerDepth.Trivia)
    {
    }

    public override void VisitForStatement(ForStatementSyntax node)
    {
        AddTags(Tags.ConstructForLoop, Tags.TechniqueLooping);
        base.VisitForStatement(node);
    }

    public override void VisitForEachStatement(ForEachStatementSyntax node)
    {
        AddTags(Tags.ConstructForeach, Tags.TechniqueEnumeration, Tags.TechniqueLooping);
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

    public override void VisitIndexerDeclaration(IndexerDeclarationSyntax node)
    {
        AddTags(Tags.ConstructIndexer);
        base.VisitIndexerDeclaration(node);
    }

    public override void VisitParameter(ParameterSyntax node)
    {
        if (node.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.ThisKeyword)))
            AddTags(Tags.ConstructExtensionMethod);
        
        if (node.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.ParamsKeyword)))
            AddTags(Tags.ConstructVarargs);
        
        if (node.Default is not null)
            AddTags(Tags.ConstructOptionalParameter);

        AddTags(Tags.ConstructParameter);
        base.VisitParameter(node);
    }

    public override void VisitArgument(ArgumentSyntax node)
    {
        if (node.NameColon is not null)
            AddTags(Tags.ConstructNamedArgument);

        var symbol = GetSymbol(node.Expression);
        
        switch (symbol)
        {
            case IMethodSymbol:
                AddTags(Tags.ConstructLambda, Tags.TechniqueHigherOrderFunctions, Tags.ParadigmFunctional);
                break;
            case ILocalSymbol {Type: INamedTypeSymbol namedTypeSymbol} when
                IsFunctionalType(namedTypeSymbol):
                AddTags(Tags.TechniqueHigherOrderFunctions, Tags.ParadigmFunctional);
                break;
        }

        base.VisitArgument(node);
    }

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        var symbol = GetDeclaredSymbol(node);
        if (symbol is not null && symbol.ContainingType.GetMembers(symbol.Name).Length > 1)
            AddTags(Tags.ConstructMethodOverloading);
        
        if (node.TypeParameterList != null)
            AddTags(Tags.ConstructGenericMethod);
        
        if (node.ExpressionBody is not null)
            AddTags(Tags.ConstructExpressionBodiedMember);

        if (UsesRecursion(node))
            AddTags(Tags.TechniqueRecursion, Tags.ParadigmFunctional);

        AddTags(Tags.ConstructMethod);
        base.VisitMethodDeclaration(node);
    }

    public override void VisitSimpleBaseType(SimpleBaseTypeSyntax node)
    {
        switch (GetSymbolName(node.Type))
        {
            case "System.IDisposable":
                AddTags(Tags.UsesIDisposable);
                break;
            case "System.IComparable":
                AddTags(Tags.UsesIComparable, Tags.TechniqueCustomComparer);
                break;
        }

        switch (GetConstructedFromSymbolName(node.Type))
        {
            case "System.IComparable<T>":
                AddTags(Tags.UsesIComparable, Tags.TechniqueCustomComparer);
                break;
            case "System.IEquatable<T>":
                AddTags(Tags.UsesIEquatable, Tags.TechniqueEqualityComparison);
                break;
        }
        

        base.VisitSimpleBaseType(node);
    }

    public override void VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        if (node.TypeParameterList != null)
            AddTags(Tags.ConstructGenericType);

        if (GetDeclaredSymbol(node) is INamedTypeSymbol namedTypeSymbol &&
            DerivesFromException(namedTypeSymbol))
            AddTags(Tags.TechniqueExceptions, Tags.ConstructUserDefinedException);

        if (node.BaseList != null)
            AddTags(Tags.TechniqueInheritance);
        
        if (node.Ancestors().OfType<ClassDeclarationSyntax>().Any())
            AddTags(Tags.ConstructNestedType);
        
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
        AddTags(Tags.ConstructTuple, Tags.UsesValueTuple);
        base.VisitTupleExpression(node);
    }

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        AddTags(Tags.ConstructInvocation, Tags.ConstructMethod);

        if (GetSymbol(node) is not null && GetSymbol(node).ContainingNamespace.ToDisplayString() == "System.Linq")
            AddTags(Tags.ConstructLinq, Tags.ParadigmFunctional);
        
        if (GetSymbolName(node) == "object.GetType()")
            AddTags(Tags.ParadigmReflective);

        if (GetConstructedFromSymbolName(node) ==
            "System.Collections.Generic.IEnumerable<TSource>.AsParallel<TSource>()")
            AddTags(Tags.UsesEnumerableAsParallel, Tags.TechniqueParallelism);

        base.VisitInvocationExpression(node);
    }

    public override void VisitTypeOfExpression(TypeOfExpressionSyntax node)
    {
        AddTags(Tags.ParadigmReflective);
        base.VisitTypeOfExpression(node);
    }

    public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
    {
        AddTags(Tags.ConstructNamespace);
        base.VisitNamespaceDeclaration(node);
    }

    public override void VisitFileScopedNamespaceDeclaration(FileScopedNamespaceDeclarationSyntax node)
    {
        AddTags(Tags.ConstructNamespace, Tags.ConstructFileScopedNamespace);
        base.VisitFileScopedNamespaceDeclaration(node);
    }

    public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
    {
        VisitTypeInfo(GetTypeInfo(node.Type));
        AddTags(Tags.ConstructVariable);
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
                AddTags(Tags.TechniqueTypeConversion, Tags.ConstructAsCast);
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
            AddTags(Tags.ConstructExpressionBodiedMember);
        
        VisitTypeInfo(GetTypeInfo(node.Type));

        var accessors = node.AccessorList?.Accessors;
        var getAccessor = accessors?.FirstOrDefault(accessor => accessor.IsKind(SyntaxKind.GetAccessorDeclaration));
        var setAccessor = accessors?.FirstOrDefault(accessor => accessor.IsKind(SyntaxKind.SetAccessorDeclaration));

        if (getAccessor != null)
            AddTags(Tags.ConstructGetter);

        if (setAccessor != null)
            AddTags(Tags.ConstructSetter);

        if (getAccessor is {Body: null} && setAccessor is {Body: null} or null)
            AddTags(Tags.ConstructAutoImplementedProperty);

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
            case SyntaxKind.NullLiteralExpression:
                AddTags(Tags.ConstructNullability, Tags.ConstructNull);
                break;
            case SyntaxKind.NumericLiteralExpression:
                AddTags(Tags.ConstructNumber);
            
                if (node.Token.Text.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                    AddTags(Tags.ConstructHexadecimalNumber);
                else if (node.Token.Text.StartsWith("0b", StringComparison.OrdinalIgnoreCase))
                    AddTags(Tags.ConstructBinaryNumber);
                else if (node.Token.Text.Contains('.', StringComparison.OrdinalIgnoreCase) && 
                         node.Token.Text.Contains('e', StringComparison.OrdinalIgnoreCase))
                    AddTags(Tags.ConstructScientificNotationNumber);
            
                if (node.Token.Text.Contains('_'))
                    AddTags(Tags.ConstructUnderscoredNumber);
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
        AddTags(Tags.ConstructAsyncAwait, Tags.TechniqueConcurrency);
        base.VisitAwaitExpression(node);
    }

    public override void VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
    {
        AddTags(Tags.ConstructLambda, Tags.ParadigmFunctional, Tags.TechniqueHigherOrderFunctions);
        base.VisitSimpleLambdaExpression(node);
    }

    public override void VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
    {
        AddTags(Tags.ConstructLambda, Tags.ParadigmFunctional, Tags.TechniqueHigherOrderFunctions);
        base.VisitParenthesizedLambdaExpression(node);
    }

    public override void VisitCastExpression(CastExpressionSyntax node)
    {
        AddTags(Tags.TechniqueTypeConversion, Tags.ConstructExplicitConversion);
        base.VisitCastExpression(node);
    }

    public override void VisitIsPatternExpression(IsPatternExpressionSyntax node)
    {
        AddTags(Tags.TechniqueTypeConversion, Tags.ConstructIsCast, Tags.ConstructPatternMatching);
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
            AddTags(Tags.ConstructExpressionBodiedMember);
        
        base.VisitConstructorDeclaration(node);
    }

    public override void VisitQueryExpression(QueryExpressionSyntax node)
    {
        AddTags(Tags.ConstructQueryExpression, Tags.ConstructLinq, Tags.ParadigmFunctional, Tags.ParadigmDeclarative);
        base.VisitQueryExpression(node);
    }

    public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
    {
        AddTags(Tags.ConstructField);
        
        if (node.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.ReadOnlyKeyword)))
            AddTags(Tags.ConstructReadOnly);
        
        if (node.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.ConstKeyword)))
            AddTags(Tags.ConstructConst);
        
        base.VisitFieldDeclaration(node);
    }

    public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
    {
        AddTags(Tags.ConstructAssignment, Tags.ParadigmImperative);
        base.VisitAssignmentExpression(node);
    }

    public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
    {
        if (node.AttributeLists.Any(attributeList => attributeList.Attributes.Select(GetSymbolName).Contains("System.FlagsAttribute.FlagsAttribute()")))
            AddTags(Tags.ConstructFlagsEnum);

        AddTags(Tags.ConstructEnum);
        base.VisitEnumDeclaration(node);
    }

    public override void VisitTrivia(SyntaxTrivia trivia)
    {
        if (trivia.IsKind(SyntaxKind.XmlComment))
            AddTags(Tags.ConstructXmlComment, Tags.ConstructComment);
        
        if (trivia.IsKind(SyntaxKind.MultiLineCommentTrivia) ||
            trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
            AddTags(Tags.ConstructComment);
            
        base.VisitTrivia(trivia);
    }

    private void VisitTypeSymbol(ITypeSymbol typeSymbol)
    {
        switch (typeSymbol?.SpecialType)
        {
            case SpecialType.System_Int16:
                AddTags(Tags.ConstructIntegralNumber, Tags.ConstructShort);
                break;
            case SpecialType.System_Int32:
                AddTags(Tags.ConstructIntegralNumber, Tags.ConstructInt);
                break;
            case SpecialType.System_Int64:
                AddTags(Tags.ConstructIntegralNumber, Tags.ConstructLong);
                break;
            case SpecialType.System_Byte:
                AddTags(Tags.ConstructIntegralNumber, Tags.ConstructByte);
                break;
            case SpecialType.System_UInt16:
                AddTags(Tags.ConstructIntegralNumber, Tags.ConstructUshort);
                break;
            case SpecialType.System_UInt32:
                AddTags(Tags.ConstructIntegralNumber, Tags.ConstructUint);
                break;
            case SpecialType.System_UInt64:
                AddTags(Tags.ConstructIntegralNumber, Tags.ConstructUlong);
                break;
            case SpecialType.System_SByte:
                AddTags(Tags.ConstructIntegralNumber, Tags.ConstructSbyte);
                break;
            case SpecialType.System_IntPtr:
                AddTags(Tags.ConstructIntegralNumber, Tags.ConstructNint);
                break;
            case SpecialType.System_UIntPtr:
                AddTags(Tags.ConstructIntegralNumber, Tags.ConstructNuint);
                break;
            case SpecialType.System_Single:
                AddTags(Tags.ConstructFloatingPointNumber, Tags.ConstructFloat);
                break;
            case SpecialType.System_Double:
                AddTags(Tags.ConstructFloatingPointNumber, Tags.ConstructDouble);
                break;
            case SpecialType.System_Decimal:
                AddTags(Tags.ConstructFloatingPointNumber, Tags.ConstructDecimal);
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
                AddTags(Tags.UsesIReadOnlyList, Tags.TechniqueImmutability, Tags.TechniqueImmutableCollection);
                break;
            case SpecialType.System_Collections_Generic_IReadOnlyCollection_T:
                AddTags(Tags.UsesIReadOnlyCollection, Tags.TechniqueImmutability, Tags.TechniqueImmutableCollection);
                break;
            case SpecialType.System_Nullable_T:
                AddTags(Tags.ConstructNullable, Tags.ConstructNullability);
                break;
            case SpecialType.System_DateTime:
                AddTags(Tags.ConstructDateTime, Tags.UsesDateTime);
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
                case "System.Tuple<T1>":
                    AddTags(Tags.ConstructTuple, Tags.UsesTuple);
                    break;
                case "System.Tuple<T1, T2>":
                    AddTags(Tags.ConstructTuple, Tags.UsesTuple);
                    break;
                case "System.Tuple<T1, T2, T3>":
                    AddTags(Tags.ConstructTuple, Tags.UsesTuple);
                    break;
                case "System.Tuple<T1, T2, T3, T4>":
                    AddTags(Tags.ConstructTuple, Tags.UsesTuple);
                    break;
                case "System.Tuple<T1, T2, T3, T4, T5>":
                    AddTags(Tags.ConstructTuple, Tags.UsesTuple);
                    break;
                case "System.Tuple<T1, T2, T3, T4, T5, T6>":
                    AddTags(Tags.ConstructTuple, Tags.UsesTuple);
                    break;
                case "System.Tuple<T1, T2, T3, T4, T5, T6, T7>":
                    AddTags(Tags.ConstructTuple, Tags.UsesTuple);
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
                    AddTags(Tags.TechniqueRandomness, Tags.UsesRandom);
                    break;
                case "System.TimeZoneInfo":
                    AddTags(Tags.ConstructDateTime, Tags.UsesTimeZoneInfo);
                    break;
                case "System.TimeOnly":
                    AddTags(Tags.ConstructDateTime, Tags.UsesTimeOnly);
                    break;
                case "System.DateOnly":
                    AddTags(Tags.ConstructDateTime, Tags.UsesDateOnly);
                    break;
            }
        }
    }

    private void VisitTypeInfo(TypeInfo typeInfo)
    {
        if (typeInfo.ConvertedType != null)
            VisitTypeSymbol(typeInfo.ConvertedType);
        
        if (typeInfo.ConvertedType is not null && typeInfo.Type is not null &&
            !typeInfo.ConvertedType.Equals(typeInfo.Type, SymbolEqualityComparer.Default))
            AddTags(Tags.ConstructImplicitConversion);
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
        AddTags(Tags.ConstructWhileLoop, Tags.TechniqueLooping);
        base.VisitWhileStatement(node);
    }

    public override void VisitDoStatement(DoStatementSyntax node)
    {
        AddTags(Tags.ConstructDoLoop, Tags.TechniqueLooping);
        base.VisitDoStatement(node);
    }

    public override void VisitThrowExpression(ThrowExpressionSyntax node)
    {
        AddTags(Tags.TechniqueExceptions, Tags.ConstructThrow, Tags.ConstructThrowExpression);
        base.VisitThrowExpression(node);
    }

    public override void VisitThrowStatement(ThrowStatementSyntax node)
    {
        AddTags(Tags.TechniqueExceptions, Tags.ConstructThrow);
        base.VisitThrowStatement(node);
    }

    public override void VisitFinallyClause(FinallyClauseSyntax node)
    {
        AddTags(Tags.TechniqueExceptions, Tags.ConstructFinally);
        base.VisitFinallyClause(node);
    }

    public override void VisitTryStatement(TryStatementSyntax node)
    {
        AddTags(Tags.TechniqueExceptions, Tags.ConstructTry);
        base.VisitTryStatement(node);
    }

    public override void VisitCatchClause(CatchClauseSyntax node)
    {
        AddTags(Tags.TechniqueExceptions, Tags.ConstructCatch);
        base.VisitCatchClause(node);
    }

    public override void VisitCatchFilterClause(CatchFilterClauseSyntax node)
    {
        AddTags(Tags.TechniqueExceptions, Tags.ConstructCatchFilter);
        base.VisitCatchFilterClause(node);
    }

    public override void VisitYieldStatement(YieldStatementSyntax node)
    {
        AddTags(Tags.TechniqueLaziness, Tags.ConstructYield);
        base.VisitYieldStatement(node);
    }

    public override void VisitElementAccessExpression(ElementAccessExpressionSyntax node)
    {
        AddTags(Tags.ConstructIndexer);
        VisitTypeInfo(GetTypeInfo(node.Expression));
        base.VisitElementAccessExpression(node);
    }

    public override void VisitAttribute(AttributeSyntax node)
    {
        AddTags(Tags.ConstructAttribute);
        base.VisitAttribute(node);
    }

    public override void VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node)
    {
        if (node.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.ImplicitKeyword)))
            AddTags(Tags.ConstructImplicitConversion);
        else if (node.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.ExplicitKeyword)))
            AddTags(Tags.ConstructExplicitConversion);
        
        AddTags(Tags.ConstructConversionOperator);
        base.VisitConversionOperatorDeclaration(node);
    }

    public override void VisitOperatorDeclaration(OperatorDeclarationSyntax node)
    {
        AddTags(Tags.ConstructOperatorOverloading);
        base.VisitOperatorDeclaration(node);
    }

    public override void VisitInitializerExpression(InitializerExpressionSyntax node)
    {
        AddTags(Tags.ConstructInitializer);

        if (node.IsKind(SyntaxKind.ObjectInitializerExpression))
            AddTags(Tags.ConstructObjectInitializer);
        else if (node.IsKind(SyntaxKind.CollectionInitializerExpression))
            AddTags(Tags.ConstructCollectionInitializer);
            
        base.VisitInitializerExpression(node);
    }

    public override void VisitCheckedExpression(CheckedExpressionSyntax node)
    {
        AddTags(Tags.ConstructOverflow, Tags.ConstructCheckedExpression);
        base.VisitCheckedExpression(node);
    }

    public override void VisitCheckedStatement(CheckedStatementSyntax node)
    {
        AddTags(Tags.ConstructOverflow, Tags.ConstructChecked);
        base.VisitCheckedStatement(node);
    }

    public override void VisitUsingStatement(UsingStatementSyntax node)
    {
        AddTags(Tags.ConstructUsingStatement);
        base.VisitUsingStatement(node);
    }

    public override void VisitUsingDirective(UsingDirectiveSyntax node)
    {
        AddTags(Tags.ConstructUsingDirective);
        base.VisitUsingDirective(node);
    }

    public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
    {
        if (node.UsingKeyword.IsKind(SyntaxKind.UsingKeyword))
            AddTags(Tags.ConstructUsingStatement);

        base.VisitLocalDeclarationStatement(node);
    }

    public override void VisitUnsafeStatement(UnsafeStatementSyntax node)
    {
        AddTags(Tags.TechniqueUnsafe);
        base.VisitUnsafeStatement(node);
    }

    public override void VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
    {
        if (node.IsKind(SyntaxKind.AddressOfExpression))
            AddTags(Tags.TechniquePointers);
        base.VisitPrefixUnaryExpression(node);
    }

    private bool UsesRecursion(SyntaxNode methodOrFunctionNode)
    {
        var methodOrFunctionSymbol = GetDeclaredSymbol(methodOrFunctionNode);
        if (methodOrFunctionSymbol is null)
            return false;

        return methodOrFunctionNode.DescendantNodes()
            .OfType<InvocationExpressionSyntax>()
            .Select(invocationExpression => GetSymbolInfo(invocationExpression.Expression).Symbol)
            .Any(invokedSymbol => methodOrFunctionSymbol.Equals(invokedSymbol, SymbolEqualityComparer.IncludeNullability));
    }

    private bool DerivesFromException(INamedTypeSymbol symbol)
    {
        if (symbol.BaseType == null)
            return false;

        if (symbol.BaseType.ToDisplayString() == "System.Exception")
            return true;

        return DerivesFromException(symbol.BaseType);
    }

    private static bool IsFunctionalType(INamedTypeSymbol symbol) =>
        FunctionalTypeNames.Contains(symbol.ConstructedFrom.ToDisplayString());

    private static readonly HashSet<string> FunctionalTypeNames = new()
    {
        "System.Action<T>",
        "System.Func<T>",
        "System.Func<T, TResult>",
        "System.Func<T1, T2, TResult>",
        "System.Func<T1, T2, T3, TResult>",
        "System.Func<T1, T2, T3, T4, TResult>",
        "System.Func<T1, T2, T3, T4, T5, TResult>",
        "System.Func<T1, T2, T3, T4, T5, T6, TResult>",
        "System.Func<T1, T2, T3, T4, T5, T6, T7, TResult>",
    };

    private static class Tags
    {
        // Paradigms
        public const string ParadigmDeclarative = "paradigm:declarative";
        public const string ParadigmFunctional = "paradigm:functional";
        public const string ParadigmImperative = "paradigm:imperative";
        public const string ParadigmObjectOriented = "paradigm:object-oriented";
        public const string ParadigmReflective = "paradigm:reflective";

        // Techniques
        public const string TechniqueBitManipulation = "technique:bit-manipulation";
        public const string TechniqueBitShifting = "technique:bit-shifting";
        public const string TechniqueBooleanLogic = "technique:boolean-logic";
        public const string TechniqueCompoundAssignment = "technique:compound-assignment";
        public const string TechniqueConcurrency = "technique:concurrency";
        public const string TechniqueCustomComparer = "technique:custom-comparer";
        public const string TechniqueEnumeration = "technique:enumeration";
        public const string TechniqueEqualityComparison = "technique:equality-comparison";
        public const string TechniqueExceptions = "technique:exceptions";
        public const string TechniqueHigherOrderFunctions = "technique:higher-order-functions";
        public const string TechniqueImmutability = "technique:immutability";
        public const string TechniqueImmutableCollection = "technique:immutable-collection";
        public const string TechniqueInheritance = "technique:inheritance";
        public const string TechniqueLaziness = "technique:laziness";
        public const string TechniqueLocks = "technique:locks";
        public const string TechniqueLooping = "technique:looping";
        public const string TechniqueMemoryManagement = "technique:memory-management";
        public const string TechniqueMutexes = "technique:mutexes";
        public const string TechniqueParallelism = "technique:parallelism";
        public const string TechniquePerformance = "technique:performance";
        public const string TechniquePointers = "technique:pointers";
        public const string TechniqueRandomness = "technique:randomness";
        public const string TechniqueRecursion = "technique:recursion";
        public const string TechniqueRegularExpression = "technique:regular-expression";
        public const string TechniqueSortedCollection = "technique:sorted-collection";
        public const string TechniqueSorting = "technique:sorting";
        public const string TechniqueTypeConversion = "technique:type-conversion";
        public const string TechniqueUnsafe = "technique:unsafe";

        // Constructs
        public const string ConstructAdd = "construct:add";
        public const string ConstructAsCast = "construct:as-cast";
        public const string ConstructAssignment = "construct:assignment";
        public const string ConstructAsyncAwait = "construct:async-await";
        public const string ConstructAttribute = "construct:attribute";
        public const string ConstructAutoImplementedProperty = "construct:auto-implemented-property";
        public const string ConstructBitwiseAnd = "construct:bitwise-and";
        public const string ConstructBitwiseNot = "construct:bitwise-not";
        public const string ConstructBitwiseOr = "construct:bitwise-or";
        public const string ConstructBitwiseXor = "construct:bitwise-xor";
        public const string ConstructBreak = "construct:break";
        public const string ConstructCatch = "construct:catch";
        public const string ConstructCatchFilter = "construct:catch-filter";
        public const string ConstructChecked = "construct:checked";
        public const string ConstructCheckedExpression = "construct:checked-expression";
        public const string ConstructCollectionInitializer = "construct:collection-initializer";
        public const string ConstructConst = "construct:const";
        public const string ConstructConstructor = "construct:constructor";
        public const string ConstructContinue = "construct:continue";
        public const string ConstructConversionOperator = "construct:conversion-operator";
        public const string ConstructDivide = "construct:divide";
        public const string ConstructDoLoop = "construct:do-loop";
        public const string ConstructExplicitConversion = "construct:explicit-conversion";
        public const string ConstructExpressionBodiedMember = "construct:expression-bodied-member";
        public const string ConstructExtensionMethod = "construct:extension-method";
        public const string ConstructField = "construct:field";
        public const string ConstructFileScopedNamespace = "construct:file-scoped-namespace";
        public const string ConstructFinally = "construct:finally";
        public const string ConstructForeach = "construct:foreach";
        public const string ConstructForLoop = "construct:for-loop";
        public const string ConstructGenericMethod = "construct:generic-method";
        public const string ConstructGenericType = "construct:generic-type";
        public const string ConstructGetter = "construct:getter";
        public const string ConstructIf = "construct:if";
        public const string ConstructImplicitConversion = "construct:implicit-conversion";
        public const string ConstructIndexer = "construct:indexer";
        public const string ConstructInitializer = "construct:initializer";
        public const string ConstructInvocation = "construct:invocation";
        public const string ConstructIsCast = "construct:is-cast";
        public const string ConstructLambda = "construct:lambda";
        public const string ConstructLeftShift = "construct:left-shift";
        public const string ConstructLinq = "construct:linq";
        public const string ConstructLocalFunction = "construct:local-function";
        public const string ConstructLock = "construct:lock";
        public const string ConstructLogicalAnd = "construct:logical-and";
        public const string ConstructLogicalNot = "construct:logical-not";
        public const string ConstructLogicalOr = "construct:logical-or";
        public const string ConstructMethod = "construct:method";
        public const string ConstructMethodOverloading = "construct:method-overloading";
        public const string ConstructMultiply = "construct:multiply";
        public const string ConstructNamedArgument = "construct:named-argument";
        public const string ConstructNamespace = "construct:namespace";
        public const string ConstructNestedType = "construct:nested-type";
        public const string ConstructObjectInitializer = "construct:object-initializer";
        public const string ConstructOperatorOverloading = "construct:operator-overloading";
        public const string ConstructOptionalParameter = "construct:optional-parameter";
        public const string ConstructOverflow = "construct:overflow";
        public const string ConstructParameter = "construct:parameter";
        public const string ConstructPatternMatching = "construct:pattern-matching";
        public const string ConstructProperty = "construct:property";
        public const string ConstructQueryExpression = "construct:query-expression";
        public const string ConstructReadOnly = "construct:read-only";
        public const string ConstructReturn = "construct:return";
        public const string ConstructRightShift = "construct:right-shift";
        public const string ConstructSetter = "construct:setter";
        public const string ConstructSubtract = "construct:subtract";
        public const string ConstructSwitch = "construct:switch";
        public const string ConstructSwitchExpression = "construct:switch-expression";
        public const string ConstructTernary = "construct:ternary";
        public const string ConstructThrow = "construct:throw";
        public const string ConstructThrowExpression = "construct:throw-expression";
        public const string ConstructTry = "construct:try";
        public const string ConstructTypeInference = "construct:type-inference";
        public const string ConstructUserDefinedException = "construct:user-defined-exception";
        public const string ConstructUsingDirective = "construct:using-directive";
        public const string ConstructUsingStatement = "construct:using-statement";
        public const string ConstructVarargs = "construct:varargs";
        public const string ConstructVariable = "construct:variable";
        public const string ConstructVisibilityModifiers = "construct:visibility-modifiers";
        public const string ConstructWhileLoop = "construct:while-loop";
        public const string ConstructYield = "construct:yield";
        
        // Constructs - types
        public const string ConstructArray = "construct:array";
        public const string ConstructBigInteger = "construct:big-integer";
        public const string ConstructBitArray = "construct:bit-array";
        public const string ConstructBoolean = "construct:boolean";
        public const string ConstructByte = "construct:byte";
        public const string ConstructChar = "construct:char";
        public const string ConstructClass = "construct:class";
        public const string ConstructDateTime = "construct:date-time";
        public const string ConstructDecimal = "construct:decimal";
        public const string ConstructDictionary = "construct:dictionary";
        public const string ConstructDouble = "construct:double";
        public const string ConstructEnum = "construct:enum";
        public const string ConstructFlagsEnum = "construct:flags-enum";
        public const string ConstructFloat = "construct:float";
        public const string ConstructFloatingPointNumber = "construct:floating-point-number";
        public const string ConstructInt = "construct:int";
        public const string ConstructIntegralNumber = "construct:integral-number";
        public const string ConstructInterface = "construct:interface";
        public const string ConstructLinkedList = "construct:linked-list";
        public const string ConstructList = "construct:list";
        public const string ConstructLong = "construct:long";
        public const string ConstructNint = "construct:nint";
        public const string ConstructNuint = "construct:nuint";
        public const string ConstructNull = "construct:null";
        public const string ConstructNullability = "construct:nullability";
        public const string ConstructNullable = "construct:nullable";
        public const string ConstructNumber = "construct:number";
        public const string ConstructQueue = "construct:queue";
        public const string ConstructRecord = "construct:record";
        public const string ConstructSbyte = "construct:sbyte";
        public const string ConstructSet = "construct:set";
        public const string ConstructShort = "construct:short";
        public const string ConstructStack = "construct:stack";
        public const string ConstructString = "construct:string";
        public const string ConstructStruct = "construct:struct";
        public const string ConstructTuple = "construct:tuple";
        public const string ConstructUint = "construct:uint";
        public const string ConstructUlong = "construct:ulong";
        public const string ConstructUshort = "construct:ushort";

        // Constructs - notation
        public const string ConstructBinaryNumber = "construct:binary-number";
        public const string ConstructHexadecimalNumber = "construct:hexadecimal-number";
        public const string ConstructMultilineString = "construct-multiline-string";
        public const string ConstructScientificNotationNumber = "construct:scientific-notation-number";
        public const string ConstructStringInterpolation = "construct-string-interpolation";
        public const string ConstructUnderscoredNumber = "construct:underscored-number";
        public const string ConstructVerbatimString = "construct-verbatim-string";
        
        // Constructs - trivia
        public const string ConstructComment = "construct:comment";
        public const string ConstructXmlComment = "construct:xml-comment";

        // Uses - C#-specific types
        public const string UsesDateOnly = "uses:DateOnly";
        public const string UsesDateTime = "uses:DateTime";
        public const string UsesDelegate = "uses:delegate";
        public const string UsesMemory = "uses:Memory<T>";
        public const string UsesMutex = "uses:Mutex";
        public const string UsesRandom = "uses:Random";
        public const string UsesRegex = "uses:Regex";
        public const string UsesSpan = "uses:Span<T>";
        public const string UsesStringBuilder = "uses:StringBuilder";
        public const string UsesTimeOnly = "uses:TimeOnly";
        public const string UsesTimeZoneInfo = "uses:TimeZoneInfo";
        
        // Uses - collections
        public const string UsesDictionary = "uses:Dictionary<TKey,TValue>";
        public const string UsesHashSet = "uses:HashSet<T>";
        public const string UsesLinkedList = "uses:LinkedList<T>";
        public const string UsesList = "uses:List<T>";
        public const string UsesQueue = "uses:Queue<T>";
        public const string UsesSortedDictionary = "uses:SortedDictionary<TKey,TValue>";
        public const string UsesSortedList = "uses:SortedList<T>";
        public const string UsesSortedSet = "uses:SortedSet<T>";
        public const string UsesStack = "uses:Stack<T>";
        public const string UsesTuple = "uses:Tuple";
        public const string UsesValueTuple = "uses:ValueTuple";
        
        // Uses - interfaces
        public const string UsesICollection = "uses:ICollection<T>";
        public const string UsesIComparable = "uses:IComparable";
        public const string UsesIDisposable = "uses:IDisposable";
        public const string UsesIEnumerable = "uses:IEnumerable<T>";
        public const string UsesIEnumerator = "uses:IEnumerator<T>";
        public const string UsesIEquatable = "uses:IEquatable<T>";
        public const string UsesIList = "uses:IList<T>";
        public const string UsesIReadOnlyCollection = "uses:IReadOnlyCollection<T>";
        public const string UsesIReadOnlyList = "uses:IReadOnlyList<T>";
        
        // Uses - methods
        public const string UsesEnumerableAsParallel = "uses:Enumerable.AsParallel";
    }
}
