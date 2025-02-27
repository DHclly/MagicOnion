using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace MagicOnion.Generator.Tests;

public static class CompilationHelper
{
    public static (Compilation Compilation, SemanticModel SemanticModel) Create(string code)
    {
        var syntaxTree = SyntaxFactory.ParseSyntaxTree(code, CSharpParseOptions.Default);
        var assemblyName = Guid.NewGuid().ToString();
        var refAsmDir = Path.GetDirectoryName(typeof(object).Assembly.Location);
        var references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(Path.Combine(refAsmDir, "System.Private.CoreLib.dll")),
            MetadataReference.CreateFromFile(Path.Combine(refAsmDir, "System.Runtime.Extensions.dll")),
            MetadataReference.CreateFromFile(Path.Combine(refAsmDir, "System.Collections.dll")),
            MetadataReference.CreateFromFile(Path.Combine(refAsmDir, "System.Linq.dll")),
            MetadataReference.CreateFromFile(Path.Combine(refAsmDir, "System.Console.dll")),
            MetadataReference.CreateFromFile(Path.Combine(refAsmDir, "System.Runtime.dll")),
            MetadataReference.CreateFromFile(Path.Combine(refAsmDir, "System.Memory.dll")),
            MetadataReference.CreateFromFile(Path.Combine(refAsmDir, "netstandard.dll")),
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
        };
        var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

        var compilation = CSharpCompilation.Create(assemblyName, new [] { syntaxTree }, references, compilationOptions);
        if (compilation.GetDiagnostics().Any(x => x.Severity == DiagnosticSeverity.Error))
        {
            throw new InvalidOperationException("Failed to compile the source code. \n" + string.Join(Environment.NewLine, compilation.GetDiagnostics().Select(x => x.ToString())));
        }
        return (compilation, compilation.GetSemanticModel(syntaxTree));
    }
}
