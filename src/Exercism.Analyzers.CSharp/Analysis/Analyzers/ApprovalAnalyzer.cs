using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    public abstract class ApprovalAnalyzer
    {
        public abstract Task<bool> CanBeApproved(Compilation compilation);
    }
}