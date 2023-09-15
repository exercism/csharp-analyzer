using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Exercises;

internal class ExerciseAnalyzer
{
    protected readonly Analysis Analysis = Analysis.Empty;

    public Analysis Analyze(Solution solution)
    {
        // We start with the exercise-specific analysis to have those comments
        // listed first, which are likely the most relevant comments to the student
        AnalyzeExerciseSpecific(solution);
        AnalyzeCommon(solution);

        return Analysis;
    }

    private void AnalyzeCommon(Solution solution)
    {
        var syntaxWalker = new SyntaxWalker(solution.Compilation, Analysis);
        var tagIdentifier = new TagIdentifier(Analysis);

        foreach (var syntaxTree in solution.Compilation.SyntaxTrees)
        {
            syntaxWalker.Visit(syntaxTree.GetRoot());
            tagIdentifier.Visit(syntaxTree.GetRoot());
        }
    }

    protected virtual void AnalyzeExerciseSpecific(Solution solution)
    {
    }

    private static class Comments
    {
        public static readonly Comment HasMainMethod = new("csharp.general.has_main_method", CommentType.Essential);

        public static readonly Comment UseNullCoalescingOperatorNotNullCheck =
            new("csharp.general.use_null_coalescing_operator_not_null_check", CommentType.Actionable);

        public static readonly Comment UseStringInterpolationNotStringFormat =
            new("csharp.general.use_string_interpolation_not_string_format", CommentType.Actionable);

        public static readonly Comment UseStringInterpolationNotStringConcatenation =
            new("csharp.general.use_string_interpolation_not_string_concatenation", CommentType.Informative);

        public static readonly Comment RemoveThrowNotImplementedException =
            new("csharp.general.remove_throw_not_implemented_exception", CommentType.Essential);

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

    private class SyntaxWalker : CSharpSyntaxWalker
    {
        private readonly Analysis _analysis;
        private readonly Compilation _compilation;

        public SyntaxWalker(Compilation compilation, Analysis analysis) =>
            (_compilation, _analysis) = (compilation, analysis);

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if (node.Body is {Statements: [ReturnStatementSyntax]})
            {
                _analysis.AddComment(Comments.UseExpressionBodiedMember(node.Identifier.Text));
            }

            base.VisitMethodDeclaration(node);
        }
    }

    private class TagIdentifier : CSharpSyntaxRewriter
    {
        private readonly Analysis _analysis;

        public TagIdentifier(Analysis analysis) => _analysis = analysis;

        public override SyntaxNode VisitForStatement(ForStatementSyntax node)
        {
            _analysis.AddTag(Tag.ConstructFor);
            return base.VisitForStatement(node);
        }

        public override SyntaxNode VisitForEachStatement(ForEachStatementSyntax node)
        {
            _analysis.AddTag(Tag.ConstructForeach);
            return base.VisitForEachStatement(node);
        }

        public override SyntaxNode VisitIfStatement(IfStatementSyntax node)
        {
            _analysis.AddTag(Tag.ConstructIf);
            return base.VisitIfStatement(node);
        }

        public override SyntaxNode VisitSwitchStatement(SwitchStatementSyntax node)
        {
            _analysis.AddTag(Tag.ConstructSwitch);
            return base.VisitSwitchStatement(node);
        }

        public override SyntaxNode VisitSwitchExpression(SwitchExpressionSyntax node)
        {
            _analysis.AddTag(Tag.ConstructSwitchExpression);
            return base.VisitSwitchExpression(node);
        }

        public override SyntaxNode VisitParameter(ParameterSyntax node)
        {
            if (node.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.ThisKeyword)))
            {
                _analysis.AddTag(Tag.ConstructExtensionMethod);
            }

            _analysis.AddTag(Tag.ConstructParameter);

            return base.VisitParameter(node);
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if (node.TypeParameterList != null)
            {
                _analysis.AddTag(Tag.ConstructGenericMethod);
            }

            _analysis.AddTag(Tag.ConstructMethod);

            return base.VisitMethodDeclaration(node);
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (node.TypeParameterList != null)
            {
                _analysis.AddTag(Tag.ConstructGenericType);
            }

            _analysis.AddTag(Tag.ConstructClass);

            return base.VisitClassDeclaration(node);
        }

        public override SyntaxNode VisitStructDeclaration(StructDeclarationSyntax node)
        {
            if (node.TypeParameterList != null)
            {
                _analysis.AddTag(Tag.ConstructGenericType);
            }

            _analysis.AddTag(Tag.ConstructStruct);

            return base.VisitStructDeclaration(node);
        }

        public override SyntaxNode VisitRecordDeclaration(RecordDeclarationSyntax node)
        {
            if (node.TypeParameterList != null)
            {
                _analysis.AddTag(Tag.ConstructGenericType);
            }

            _analysis.AddTag(Tag.ConstructRecord);

            return base.VisitRecordDeclaration(node);
        }

        public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
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

            return base.VisitInvocationExpression(node);
        }

        public override SyntaxNode VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            _analysis.AddTag(Tag.ConstructInterface);

            return base.VisitInterfaceDeclaration(node);
        }

        public override SyntaxNode VisitConditionalExpression(ConditionalExpressionSyntax node)
        {
            _analysis.AddTag(Tag.ConstructTernary);

            return base.VisitConditionalExpression(node);
        }

        public override SyntaxToken VisitToken(SyntaxToken token)
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

            return base.VisitToken(token);
        }

        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node)
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

            return base.VisitPropertyDeclaration(node);
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
}