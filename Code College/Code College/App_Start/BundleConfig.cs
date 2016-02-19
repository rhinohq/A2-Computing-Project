using System.Web.Optimization;

namespace Code_College.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection Bundles)
        {
            BundleTable.EnableOptimizations = true;

            ScriptBundle ScriptBundle = new ScriptBundle("~/bundles/js");
            StyleBundle StyleBundle = new StyleBundle("~/bundles/css");

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