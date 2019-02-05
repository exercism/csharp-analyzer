using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers.Gigasecond
{
    public class GigasecondApprovalAnalyzer : ApprovalAnalyzer
    {
        public override async Task<bool> CanBeApproved(Compilation compilation)
        {
            var syntaxTree = compilation.GetImplementationSyntaxTree(Exercise.Gigasecond);
            if (syntaxTree == null)
                return false;

            var root = await syntaxTree.GetRootAsync();
            var addMethod = root
                .GetClassDeclaration("Gigasecond")
                .GetMethodDeclaration("Add");
            
            if (addMethod == null)
                return false;

            var operation =
                GetOperationFromBody(compilation, syntaxTree, addMethod) ??
                GetOperationFromExpressionBody(compilation, syntaxTree, addMethod);

            return OperationInvokesDateTimeAddSecondsMethod(operation);
        }

        private static IOperation GetOperationFromBody(Compilation compilation, SyntaxTree syntaxTree,
            MethodDeclarationSyntax addMethod)
        {
            if (addMethod.Body == null || addMethod.Body.Statements.Count != 1)
                return null;
            
            var semanticModel = compilation.GetSemanticModel(syntaxTree);
            var returnOperation = (IReturnOperation)semanticModel.GetOperation(addMethod.Body.Statements[0]);
            return returnOperation.ReturnedValue;
        }

        private static IOperation GetOperationFromExpressionBody(Compilation compilation, SyntaxTree syntaxTree,
            MethodDeclarationSyntax addMethod)
        {
            var semanticModel = compilation.GetSemanticModel(syntaxTree);
            return semanticModel.GetOperation(addMethod.ExpressionBody.Expression);
        }

        private static bool OperationInvokesDateTimeAddSecondsMethod(IOperation operation) =>
            operation is IInvocationOperation invocationOperation &&
            invocationOperation.TargetMethod.ToDisplayString() == "System.DateTime.AddSeconds(double)";
    }
}