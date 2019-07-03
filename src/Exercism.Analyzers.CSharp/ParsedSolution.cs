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

        public void AddComment(SolutionComment comment) =>
            _comments.Add(comment);

        public bool HasComments() => _comments.Any();

        public SolutionAnalysis ContinueAnalysis() => null;
        
        public SolutionAnalysis Approve() =>
            ToSolutionAnalysis(SolutionStatus.Approve, _comments.ToArray());
        
        public SolutionAnalysis Disapprove() =>
            ToSolutionAnalysis(SolutionStatus.Disapprove, _comments.ToArray());

        public SolutionAnalysis ReferToMentor() =>
            ToSolutionAnalysis(SolutionStatus.ReferToMentor, Array.Empty<SolutionComment>());

        private SolutionAnalysis ToSolutionAnalysis(SolutionStatus status, SolutionComment[] comments) =>
            new SolutionAnalysis(Solution, new SolutionAnalysisResult(status, comments));
    }
}