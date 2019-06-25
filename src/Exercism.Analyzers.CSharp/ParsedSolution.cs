using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp
{
    internal class ParsedSolution
    {
        private readonly List<SolutionComment> _comments = new List<SolutionComment>();
    
        public Solution Solution { get; }

        public SyntaxNode SyntaxRoot { get; }

        public ParsedSolution(Solution solution, SyntaxNode syntaxRoot) =>
            (Solution, SyntaxRoot) = (solution, syntaxRoot);

        public void AddComment(string comment) =>
            _comments.Add(new SolutionComment(comment));

        public bool HasComments() => _comments.Any();

        public SolutionAnalysis ContinueAnalysis() => null;
        
        public SolutionAnalysis DisapproveWithComment() =>
            ToSolutionAnalysis(SolutionStatus.DisapproveWithComment, _comments.ToArray());
        
        public SolutionAnalysis ApproveWithComment() =>
            ToSolutionAnalysis(SolutionStatus.ApproveWithComment, _comments.ToArray());

        public SolutionAnalysis ApproveAsOptimal() =>
            ToSolutionAnalysis(SolutionStatus.ApproveAsOptimal, Array.Empty<SolutionComment>());

        public SolutionAnalysis ReferToMentor() =>
            ToSolutionAnalysis(SolutionStatus.ReferToMentor, Array.Empty<SolutionComment>());

        private SolutionAnalysis ToSolutionAnalysis(SolutionStatus status, SolutionComment[] comments) =>
            new SolutionAnalysis(Solution, new SolutionAnalysisResult(status, comments));
    }
}