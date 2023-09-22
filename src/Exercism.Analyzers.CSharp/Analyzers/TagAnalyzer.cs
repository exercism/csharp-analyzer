using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class TagAnalyzer : Analyzer
{
    public override void VisitForStatement(ForStatementSyntax node)
    {
        AddTag(Tag.ConstructFor);
        base.VisitForStatement(node);
    }

    public override void VisitForEachStatement(ForEachStatementSyntax node)
    {
        AddTag(Tag.ConstructForeach);
        base.VisitForEachStatement(node);
    }

    public override void VisitIfStatement(IfStatementSyntax node)
    {
        AddTag(Tag.ConstructIf);
        base.VisitIfStatement(node);
    }

    public override void VisitSwitchStatement(SwitchStatementSyntax node)
    {
        AddTag(Tag.ConstructSwitch);
        base.VisitSwitchStatement(node);
    }

    public override void VisitSwitchExpression(SwitchExpressionSyntax node)
    {
        AddTag(Tag.ConstructSwitchExpression);
        base.VisitSwitchExpression(node);
    }

    public override void VisitParameter(ParameterSyntax node)
    {
        if (node.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.ThisKeyword)))
            AddTag(Tag.ConstructExtensionMethod);

        AddTag(Tag.ConstructParameter);

        base.VisitParameter(node);
    }

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        if (node.TypeParameterList != null)
            AddTag(Tag.ConstructGenericMethod);

        AddTag(Tag.ConstructMethod);

        base.VisitMethodDeclaration(node);
    }

    public override void VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        if (node.TypeParameterList != null)
            AddTag(Tag.ConstructGenericType);

        AddTag(Tag.ConstructClass);

        base.VisitClassDeclaration(node);
    }

    public override void VisitStructDeclaration(StructDeclarationSyntax node)
    {
        if (node.TypeParameterList != null)
            AddTag(Tag.ConstructGenericType);

        AddTag(Tag.ConstructStruct);

        base.VisitStructDeclaration(node);
    }

    public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
    {
        if (node.TypeParameterList != null)
            AddTag(Tag.ConstructGenericType);

        AddTag(Tag.ConstructRecord);

        base.VisitRecordDeclaration(node);
    }

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        AddTag(Tag.ConstructInvocation);

        if (node.Expression is IdentifierNameSyntax identifierName)
        {
            var parent = node.AncestorsAndSelf()
                .FirstOrDefault(node => node is MethodDeclarationSyntax or LocalFunctionStatementSyntax);

            if (parent is MethodDeclarationSyntax methodDeclaration &&
                methodDeclaration.Identifier.Text == identifierName.Identifier.Text ||
                parent is LocalFunctionStatementSyntax localFunctionStatement &&
                localFunctionStatement.Identifier.Text == identifierName.Identifier.Text)
            {
                AddTag(Tag.TechniqueRecursion);
            }
        }

        base.VisitInvocationExpression(node);
    }

    public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
    {
        AddTag(Tag.ConstructInterface);

        base.VisitInterfaceDeclaration(node);
    }

    public override void VisitConditionalExpression(ConditionalExpressionSyntax node)
    {
        AddTag(Tag.ConstructTernary);

        base.VisitConditionalExpression(node);
    }

    public override void VisitBinaryExpression(BinaryExpressionSyntax node)
    {
        switch (node.Kind())
        {
            case SyntaxKind.LogicalAndExpression:
                AddTags(Tag.ConstructBoolean, Tag.ConstructLogicalAnd, Tag.TechniqueBooleanLogic);
                break;
            case SyntaxKind.LogicalOrExpression:
                AddTags(Tag.ConstructBoolean, Tag.ConstructLogicalOr, Tag.TechniqueBooleanLogic);
                break;
            case SyntaxKind.LogicalNotExpression:
                AddTags(Tag.ConstructBoolean, Tag.ConstructLogicalNot, Tag.TechniqueBooleanLogic);
                break;
            case SyntaxKind.BitwiseAndExpression:
                AddTags(Tag.TechniqueBitManipulation, Tag.ConstructBitwiseAnd);
                break;
            case SyntaxKind.BitwiseOrExpression:
                AddTags(Tag.TechniqueBitManipulation, Tag.ConstructBitwiseOr);
                break;
            case SyntaxKind.BitwiseNotExpression:
                AddTags(Tag.TechniqueBitManipulation, Tag.ConstructBitwiseNot);
                break;
            case SyntaxKind.ExclusiveOrExpression:
                AddTags(Tag.TechniqueBitManipulation, Tag.ConstructBitwiseXor);
                break;
            case SyntaxKind.ExclusiveOrAssignmentExpression:
                AddTags(Tag.TechniqueBitManipulation, Tag.ConstructBitwiseXor, Tag.TechniqueCompoundAssignment);
                break;
            case SyntaxKind.LeftShiftExpression:
                AddTags(Tag.TechniqueBitManipulation, Tag.ConstructLeftShift);
                break;
            case SyntaxKind.LeftShiftAssignmentExpression:
                AddTags(Tag.TechniqueBitManipulation, Tag.ConstructLeftShift, Tag.TechniqueCompoundAssignment);
                break;
            case SyntaxKind.RightShiftExpression:
                AddTags(Tag.TechniqueBitManipulation, Tag.ConstructRightShift);
                break;
            case SyntaxKind.RightShiftAssignmentExpression:
                AddTags(Tag.TechniqueBitManipulation, Tag.ConstructRightShift, Tag.TechniqueCompoundAssignment);
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
                AddTag(Tag.ConstructVisibilityModifiers);
                break;
            case SyntaxKind.ReturnKeyword:
                AddTag(Tag.ConstructReturn);
                break;
            case SyntaxKind.BreakKeyword:
                AddTag(Tag.ConstructBreak);
                break;
            case SyntaxKind.ContinueKeyword:
                AddTag(Tag.ConstructContinue);
                break;
        }

        base.VisitToken(token);
    }

    public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
    {
        AddTag(Tag.ConstructProperty);

        var getAccessor =
            node.AccessorList?.Accessors.FirstOrDefault(accessor =>
                accessor.IsKind(SyntaxKind.GetAccessorDeclaration));
        var setAccessor =
            node.AccessorList?.Accessors.FirstOrDefault(accessor =>
                accessor.IsKind(SyntaxKind.SetAccessorDeclaration));

        if (getAccessor != null)
        {
            AddTag(Tag.ConstructGetter);
        }

        if (setAccessor != null)
        {
            AddTag(Tag.ConstructSetter);
        }

        if (getAccessor is {Body: null} && setAccessor is {Body: null})
        {
            AddTag(Tag.UsesAutoImplementedProperty);
        }

        base.VisitPropertyDeclaration(node);
    }

    public override void VisitLocalFunctionStatement(LocalFunctionStatementSyntax node)
    {
        AddTag(Tag.ConstructLocalFunction);
        
        base.VisitLocalFunctionStatement(node);
    }

    public override void VisitLiteralExpression(LiteralExpressionSyntax node)
    {
        var typeInfo = SemanticModel.GetTypeInfo(node);
        
        switch (typeInfo.ConvertedType?.SpecialType)
        {
            case SpecialType.System_Int16:
                AddTags(Tag.ConstructNumber, Tag.ConstructIntegralNumber, Tag.UsesShort);
                break;
            case SpecialType.System_Int32:
                AddTags(Tag.ConstructNumber, Tag.ConstructIntegralNumber, Tag.UsesInt);
                break;
            case SpecialType.System_Int64:
                AddTags(Tag.ConstructNumber, Tag.ConstructIntegralNumber, Tag.UsesLong);
                break;
            case SpecialType.System_Byte:
                AddTags(Tag.ConstructNumber, Tag.ConstructIntegralNumber, Tag.UsesByte);
                break;
            case SpecialType.System_UInt16:
                AddTags(Tag.ConstructNumber, Tag.ConstructIntegralNumber, Tag.UsesUshort);
                break;
            case SpecialType.System_UInt32:
                AddTags(Tag.ConstructNumber, Tag.ConstructIntegralNumber, Tag.UsesUint);
                break;
            case SpecialType.System_UInt64:
                AddTags(Tag.ConstructNumber, Tag.ConstructIntegralNumber, Tag.UsesUlong);
                break;
            case SpecialType.System_SByte:
                AddTags(Tag.ConstructNumber, Tag.ConstructIntegralNumber, Tag.UsesSbyte);
                break;
            case SpecialType.System_Single:
                AddTags(Tag.ConstructNumber, Tag.ConstructFloatingPointNumber, Tag.UsesFloat);
                break;
            case SpecialType.System_Double:
                AddTags(Tag.ConstructNumber, Tag.ConstructFloatingPointNumber, Tag.UsesDouble);
                break;
            case SpecialType.System_Decimal:
                AddTags(Tag.ConstructNumber, Tag.ConstructFloatingPointNumber, Tag.UsesDecimal);
                break;
            case SpecialType.System_String:
                AddTag(Tag.ConstructString);
                break;
            case SpecialType.System_Boolean:
                AddTag(Tag.ConstructBoolean);
                break;        }

        base.VisitLiteralExpression(node);
    }

    public override void VisitInterpolatedStringExpression(InterpolatedStringExpressionSyntax node)
    {   
        AddTag(Tag.ConstructStringInterpolation);

        base.VisitInterpolatedStringExpression(node);
    }

    public override void VisitLockStatement(LockStatementSyntax node)
    {
        AddTag(Tag.TechniqueLocks);
        
        base.VisitLockStatement(node);
    }

    private static class Tag
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

        // Constructs - types
        public const string ConstructBoolean = "construct:boolean";
        public const string ConstructString = "construct:string";
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

        // Constructs - notation
        public const string ConstructHexadecimalNumber = "construct:hexadecimal-number";
        public const string ConstructOctalNumber = "construct:octal-number";
        public const string ConstructBinaryNumber = "construct:binary-number";
        public const string ConstructScientificNumber = "construct:scientific-number";
        public const string ConstructMultilineString = "construct-multiline-string";
        public const string ConstructStringInterpolation = "construct-string-interpolation";

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
        public const string UsesDateTimeAddDays = "uses:DateTime.AddDays";
        public const string UsesDateTimeAddSeconds = "uses:DateTime.AddSeconds";
        public const string UsesDateTimePlusTimeSpan = "uses:DateTime.Plus(TimeSpan)";
        public const string UsesDateTimeIsLeapYear = "uses:DateTime.IsLeapYear";
        public const string UsesMathPow = "uses:Math.Pow";
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