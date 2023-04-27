using System.Text.RegularExpressions;
using GraphQlClientGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Mono.Options;

namespace GraphQlApiClientCodeGenerator;

class Program
{
    private static async Task Main(string[] args)
    {
        // Set default values
        string graphQlUri = @"https://wfm-mtcloud.ptxcloud.com:3005/graphql";
        string filePath = @"C:\Users\ohad.cohen\Desktop\Ohad\TestA123.cs";
        string regionName = "data classes";

        // Parse command line arguments
        var options = new OptionSet
        {
            { "f|file=", "The file path to generate the code.", f => filePath = f },
            { "r|region=", "The region name to extract classes from.", r => regionName = r },
            { "u|uri=", "The GraphQL API endpoint URI.", u => graphQlUri = u }
        };

        // It is to use the parameters if the user brought
        List<string> extraArgs = options.Parse(args);

        try
        {
            await GenerateSchemaFile(graphQlUri, filePath, regionName);

            Console.WriteLine("Code generation complete.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private static async Task GenerateSchemaFile(string garphQlUri, string filePath, string regionName)
    {
        try
        {
            var schema = await GraphQlGenerator.RetrieveSchema(HttpMethod.Get, garphQlUri);

            var csharpCode =
                new GraphQlGenerator().GenerateFullClientCSharpFile(schema, "Infrastructure.Common.Interfaces.WFM");

            csharpCode = ExtractClassesToInterfaces(csharpCode, regionName);

            Console.WriteLine($"Writing generated code to file: {filePath}");
            await File.WriteAllTextAsync(filePath, csharpCode);
            Console.WriteLine("File written successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private static string ExtractClassesToInterfaces(string csharpCode, string regionName)
    {
        // Extract the region of the code that contains the data classes
        string regionCode = ExtractRegion(csharpCode, regionName);

        // Parse the region code and retrieve all class declarations
        SyntaxTree tree = CSharpSyntaxTree.ParseText(regionCode);
        var classDecls = tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();

        if (!classDecls.Any())
        {
            throw new ArgumentException("No classes found in code");
        }

        // Convert each class declaration into an interface declaration
        var interfaceDecls = classDecls.Select(classDecl =>
        {
            var interfaceName = $"I{classDecl.Identifier}";
            var interfaceMembers = classDecl.Members.Select(member =>
            {
                if (member is PropertyDeclarationSyntax propertyDecl)
                {
                    // Convert property to interface property
                    var interfaceProperty = SyntaxFactory.PropertyDeclaration(
                        propertyDecl.Type, propertyDecl.Identifier)
                        .AddAccessorListAccessors(
                            SyntaxFactory.AccessorDeclaration(
                                SyntaxKind.GetAccessorDeclaration)
                            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)))
                        .AddAccessorListAccessors(
                            SyntaxFactory.AccessorDeclaration(
                                SyntaxKind.SetAccessorDeclaration)
                            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));
                    return interfaceProperty.WithModifiers(
                        SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.None)));
                }

                // Use member as-is in the interface
                return member.WithModifiers(
                    SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.None)));
            });
            return SyntaxFactory.InterfaceDeclaration(interfaceName)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddMembers(interfaceMembers.ToArray());
        });

        // Modify the class declarations to implement the new interfaces
        var newClassDecls = classDecls.Select(classDecl =>
            classDecl.AddBaseListTypes(
                SyntaxFactory.SimpleBaseType(
                    SyntaxFactory.IdentifierName($"I{classDecl.Identifier}"))));

        // Create the new syntax tree with the interface and class declarations
        SyntaxTree newTree = SyntaxFactory.SyntaxTree(
            SyntaxFactory.CompilationUnit()
                .AddMembers(interfaceDecls.ToArray())
                .AddMembers(newClassDecls.ToArray())
                .NormalizeWhitespace());

        // Replace the original code with the modified code
        string newCode = newTree.ToString();
        string modifiedCode = ReplaceRegion(csharpCode, newCode, regionName);

        // Use NormalizeWhitespace to format the modified code
        CSharpParseOptions parseOptions = new CSharpParseOptions().WithDocumentationMode(DocumentationMode.Parse); // set options as desired
        SyntaxNode formattedNode = CSharpSyntaxTree.ParseText(modifiedCode, parseOptions).GetRoot();
        return formattedNode.ToFullString();
    }

    private static string ExtractRegion(string csharpCode, string regionName)
    {
        string pattern = $"#region\\s+{regionName}(.*?)#endregion";
        Match match = Regex.Match(csharpCode, pattern, RegexOptions.Singleline);
        return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
    }

    public static string ReplaceRegion(string cSharpCode, string cSharpNewCode, string regionName)
    {
        // Construct the regular expression pattern for the region
        string pattern = $@"#region\s+{Regex.Escape(regionName)}\b(?:.|\n)+?#endregion";

        // Replace the region with the new code
        return Regex.Replace(cSharpCode, pattern, $"#region {regionName}\n{cSharpNewCode}\n#endregion");
    }
}

