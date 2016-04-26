using System.Web.Optimization;

namespace Code_College.App_Start
{
    // Set up bundling
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection Bundles)
        {
            // Enable optimizations that remove whitespace and size of files without compromising functionality
            BundleTable.EnableOptimizations = true;

            // Login and sign up bundles
            ScriptBundle ScriptBundle = new ScriptBundle("~/bundles/js");
            StyleBundle StyleBundle = new StyleBundle("~/bundles/css");

            // Editor bundles for code editor in exercise view
            ScriptBundle EditorScripts = new ScriptBundle("~/bundles/editorjs");
            StyleBundle EditorStyles = new StyleBundle("~/bundles/editorcss");
            
            ScriptBundle.IncludeDirectory("~/Scripts/", "*.js");
            StyleBundle.IncludeDirectory("~/Content/", "*.css");
            
            EditorScripts.IncludeDirectory("~/Scripts/Editor/", "*.js");
            EditorStyles.IncludeDirectory("~/Content/Editor/", "*.css");
            EditorStyles.IncludeDirectory("~/Content/Editor/Themes/", "*.css");

            Bundles.Add(ScriptBundle);
            Bundles.Add(StyleBundle);

            Bundles.Add(EditorScripts);
            Bundles.Add(EditorStyles);
        }
    }
}