using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using CSharpExtensions = Microsoft.CodeAnalysis.CSharpExtensions;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class TagAnalyzer : CSharpSyntaxWalker
{
    private readonly Analysis _analysis;

    public TagAnalyzer(Analysis analysis) => _analysis = analysis;

    public override void VisitForStatement(ForStatementSyntax node)
    {
        _analysis.AddTag(Tag.ConstructFor);
        base.VisitForStatement(node);
    }

    public override void VisitForEachStatement(ForEachStatementSyntax node)
    {
        _analysis.AddTag(Tag.ConstructForeach);
        base.VisitForEachStatement(node);
    }

    public override void VisitIfStatement(IfStatementSyntax node)
    {
        _analysis.AddTag(Tag.ConstructIf);
        base.VisitIfStatement(node);
    }

    public override void VisitSwitchStatement(SwitchStatementSyntax node)
    {
        _analysis.AddTag(Tag.ConstructSwitch);
        base.VisitSwitchStatement(node);
    }

    public override void VisitSwitchExpression(SwitchExpressionSyntax node)
    {
        _analysis.AddTag(Tag.ConstructSwitchExpression);
        base.VisitSwitchExpression(node);
    }

    public override void VisitParameter(ParameterSyntax node)
    {
        if (node.Modifiers.Any(modifier => CSharpExtensions.IsKind((SyntaxToken)modifier, SyntaxKind.ThisKeyword)))
        {
            _analysis.AddTag(Tag.ConstructExtensionMethod);
        }

        _analysis.AddTag(Tag.ConstructParameter);

        base.VisitParameter(node);
    }

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        if (node.TypeParameterList != null)
        {
            _analysis.AddTag(Tag.ConstructGenericMethod);
        }

        _analysis.AddTag(Tag.ConstructMethod);

        base.VisitMethodDeclaration(node);
    }

    public override void VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        if (node.TypeParameterList != null)
        {
            _analysis.AddTag(Tag.ConstructGenericType);
        }

        _analysis.AddTag(Tag.ConstructClass);

        base.VisitClassDeclaration(node);
    }

    public override void VisitStructDeclaration(StructDeclarationSyntax node)
    {
        if (node.TypeParameterList != null)
        {
            _analysis.AddTag(Tag.ConstructGenericType);
        }

        _analysis.AddTag(Tag.ConstructStruct);

        base.VisitStructDeclaration(node);
    }

    public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
    {
        if (node.TypeParameterList != null)
        {
            _analysis.AddTag(Tag.ConstructGenericType);
        }

        _analysis.AddTag(Tag.ConstructRecord);

        base.VisitRecordDeclaration(node);
    }

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        _analysis.AddTag(Tag.ConstructInvocation);

        if (node.Expression is IdentifierNameSyntax identifierName)
        {
            var parent = node.AncestorsAndSelf()
                .FirstOrDefault(node => node is MethodDeclarationSyntax or LocalFunctionStatementSyntax);

            if (parent is MethodDeclarationSyntax methodDeclaration &&
                methodDeclaration.Identifier.Text == identifierName.Identifier.Text ||
                parent is LocalFunctionStatementSyntax localFunctionStatement &&
                localFunctionStatement.Identifier.Text == identifierName.Identifier.Text)
            {
                _analysis.AddTag(Tag.TechniqueRecursion);
            }
        }

        base.VisitInvocationExpression(node);
    }

    public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
    {
        _analysis.AddTag(Tag.ConstructInterface);

        base.VisitInterfaceDeclaration(node);
    }

    public override void VisitConditionalExpression(ConditionalExpressionSyntax node)
    {
        _analysis.AddTag(Tag.ConstructTernary);

        base.VisitConditionalExpression(node);
    }

    public override void VisitToken(SyntaxToken token)
    {
        var tag = token.Kind() switch
        {
            SyntaxKind.PublicKeyword => Tag.ConstructVisibilityModifiers,
            SyntaxKind.ProtectedKeyword => Tag.ConstructVisibilityModifiers,
            SyntaxKind.PrivateKeyword => Tag.ConstructVisibilityModifiers,
            SyntaxKind.InternalKeyword => Tag.ConstructVisibilityModifiers,
            SyntaxKind.ReturnKeyword => Tag.ConstructReturn,
            SyntaxKind.BreakKeyword => Tag.ConstructBreak,
            SyntaxKind.ContinueKeyword => Tag.ConstructContinue,
            _ => null
        };

        if (tag != null)
        {
            _analysis.AddTag(tag);
        }

        base.VisitToken(token);
    }

    public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
    {
        _analysis.AddTag(Tag.ConstructProperty);

        var getAccessor =
            node.AccessorList?.Accessors.FirstOrDefault(accessor =>
                accessor.IsKind(SyntaxKind.GetAccessorDeclaration));
        var setAccessor =
            node.AccessorList?.Accessors.FirstOrDefault(accessor =>
                accessor.IsKind(SyntaxKind.SetAccessorDeclaration));

        if (getAccessor != null)
        {
            _analysis.AddTag(Tag.ConstructGetter);
        }

        if (setAccessor != null)
        {
            _analysis.AddTag(Tag.ConstructSetter);
        }

        if (getAccessor is {Body: null} && setAccessor is {Body: null})
        {
            _analysis.AddTag(Tag.UsesAutoImplementedProperty);
        }

        base.VisitPropertyDeclaration(node);
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