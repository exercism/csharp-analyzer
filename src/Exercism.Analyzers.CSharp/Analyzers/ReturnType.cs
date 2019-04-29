namespace Exercism.Analyzers.CSharp.Analyzers
{
    public enum ReturnType
    {
        Unknown,
        ImmediateValue,
        VariableAssignment,
        ParameterAssigment
    }
}