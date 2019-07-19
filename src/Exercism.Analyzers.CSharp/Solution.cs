using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp
{
    internal class Solution
    {
        private readonly List<SolutionComment> _comments = new List<SolutionComment>();

        public string Name { get; }
        public string Slug { get; }
        public SyntaxNode SyntaxRoot { get; }

        public Solution(string slug, string name, SyntaxNode syntaxRoot) =>
            (Slug, Name, SyntaxRoot) = (slug, name, syntaxRoot);

        protected Solution(Solution solution) : this(solution.Slug, solution.Slug, solution.SyntaxRoot)
        {
        }

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

        private static SolutionAnalysis ToSolutionAnalysis(SolutionStatus status, SolutionComment[] comments) =>
            new SolutionAnalysis(status, comments);
    }
}