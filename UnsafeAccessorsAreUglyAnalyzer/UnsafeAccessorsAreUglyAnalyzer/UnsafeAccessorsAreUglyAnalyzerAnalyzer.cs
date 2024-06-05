using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace UnsafeAccessorsAreUglyAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UnsafeAccessorsAreUglyAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "UnsafeAccessorsAreUglyAnalyzer";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "UnsafeAccessor";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.Attribute);
        }

        private static void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
        {
            var attribute = (AttributeSyntax)context.Node;

            if (attribute.Name.ToString() == "UnsafeAccessor")
            {
                var diagnostic = Diagnostic.Create(Rule, attribute.GetLocation(), DiagnosticSeverity.Error);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
