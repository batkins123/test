namespace GSAWindowsParser
{
    using Microsoft.CSharp;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.IO;

    public class Generator
    {
        public static void GenerateCSharpCode(CodeCompileUnit compileunit, string file)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            string sourceFile;
            if (provider.FileExtension[0] == '.')
                sourceFile = file + provider.FileExtension;
            else
                sourceFile = file + "." + provider.FileExtension;
            using (StreamWriter sw = new StreamWriter(sourceFile, false))
            {
                IndentedTextWriter tw = new IndentedTextWriter(sw, "    ");
                provider.GenerateCodeFromCompileUnit(compileunit, tw, new CodeGeneratorOptions());
                tw.Close();
            }
        }
    }
}