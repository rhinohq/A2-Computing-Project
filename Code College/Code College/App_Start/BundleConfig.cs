using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Code_College.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection Bundles)
        {
            BundleTable.EnableOptimizations = true;

            var ScriptBundle = new ScriptBundle("~/bundles/js");
            var StyleBundle = new StyleBundle("~/bundles/css");

            ScriptBundle.IncludeDirectory("~/Scripts/", "*.js");
            StyleBundle.IncludeDirectory("~/Content/", "*.css");

            Bundles.Add(ScriptBundle);
            Bundles.Add(StyleBundle);
        }
    }
}