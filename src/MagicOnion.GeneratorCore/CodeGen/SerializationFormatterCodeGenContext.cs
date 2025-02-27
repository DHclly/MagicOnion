using System.CodeDom.Compiler;
using MagicOnion.Generator.CodeAnalysis;

namespace MagicOnion.Generator.CodeGen;

public class SerializationFormatterCodeGenContext
{
    readonly StringWriter underlyingWriter;

    public string Namespace { get; }
    public string FormatterNamespace { get; }
    public string InitializerName { get; }
    public IReadOnlyList<ISerializationFormatterRegisterInfo> FormatterRegistrations { get; }

    public IndentedTextWriter TextWriter { get; }

    public SerializationFormatterCodeGenContext(string @namespace, string formatterNamespace, string initializerName, IReadOnlyList<ISerializationFormatterRegisterInfo> formatterRegistrations)
    {
        Namespace = @namespace;
        FormatterNamespace = formatterNamespace;
        InitializerName = initializerName;
        FormatterRegistrations = formatterRegistrations;

        underlyingWriter = new StringWriter();
        TextWriter = new IndentedTextWriter(underlyingWriter);
    }

    public string GetWrittenText() => underlyingWriter.ToString();
}
