#pragma checksum "C:\hswork\workFile\jira_dashboard\trunk\Views\Home\EngagementByProject.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c453da202a3d44120b6014725c42aad814c9e0b4"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_EngagementByProject), @"mvc.1.0.view", @"/Views/Home/EngagementByProject.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/EngagementByProject.cshtml", typeof(AspNetCore.Views_Home_EngagementByProject))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\hswork\workFile\jira_dashboard\trunk\Views\_ViewImports.cshtml"
using JiraDashboard;

#line default
#line hidden
#line 2 "C:\hswork\workFile\jira_dashboard\trunk\Views\_ViewImports.cshtml"
using JiraDashboard.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c453da202a3d44120b6014725c42aad814c9e0b4", @"/Views/Home/EngagementByProject.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"00146bb0bbd6f6588305b1d4bb0a6e58b5bfdb26", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_EngagementByProject : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "C:\hswork\workFile\jira_dashboard\trunk\Views\Home\EngagementByProject.cshtml"
  
    ViewData["Title"] = "EngagementByProject";

#line default
#line hidden
            BeginContext(57, 592, true);
            WriteLiteral(@"

<div style=""width:440px;float: left;"">
    <table class=""table-responsive"">
        <tr>
            <th style=""background-color:#DDEBF7; width:250px"">
                Project
                <button id=""projectFilterButton"" type=""button"" class=""btn btn-primary"" data-toggle=""modal"" data-target=""#projectFilter"">
                    +
                </button>
            </th>
            <th style=""background-color:#DDEBF7; width:80px"">Resources</th>
            <th style=""background-color:#DDEBF7; width:110px"">Total Logged Hours(Month)</th>
        </tr>
    </table>
");
            EndContext();
#line 20 "C:\hswork\workFile\jira_dashboard\trunk\Views\Home\EngagementByProject.cshtml"
     foreach (var item in Model.LoggedHoursByProjectDisplay)
    {

#line default
#line hidden
            BeginContext(718, 163, true);
            WriteLiteral("        <details>\r\n            <summary>\r\n                <table class=\"table-active\">\r\n                    <tr>\r\n                        <td style=\"width:250px\">+");
            EndContext();
            BeginContext(882, 18, false);
#line 26 "C:\hswork\workFile\jira_dashboard\trunk\Views\Home\EngagementByProject.cshtml"
                                            Write(item[0][0].Project);

#line default
#line hidden
            EndContext();
            BeginContext(900, 54, true);
            WriteLiteral("</td>\r\n                        <td style=\"width:80px\">");
            EndContext();
            BeginContext(955, 13, false);
#line 27 "C:\hswork\workFile\jira_dashboard\trunk\Views\Home\EngagementByProject.cshtml"
                                          Write(item[1].Count);

#line default
#line hidden
            EndContext();
            BeginContext(968, 55, true);
            WriteLiteral("</td>\r\n                        <td style=\"width:110px\">");
            EndContext();
            BeginContext(1024, 32, false);
#line 28 "C:\hswork\workFile\jira_dashboard\trunk\Views\Home\EngagementByProject.cshtml"
                                           Write(item[0][0].TotalLoggedHoursMonth);

#line default
#line hidden
            EndContext();
            BeginContext(1056, 130, true);
            WriteLiteral("</td>\r\n                    </tr>\r\n                </table>\r\n            </summary>\r\n            <table class=\"table-responsive\">\r\n");
            EndContext();
#line 33 "C:\hswork\workFile\jira_dashboard\trunk\Views\Home\EngagementByProject.cshtml"
                 foreach (var i in item[1])
                {

#line default
#line hidden
            BeginContext(1250, 74, true);
            WriteLiteral("                    <tr>\r\n                        <td style=\"width:250px\">");
            EndContext();
            BeginContext(1325, 10, false);
#line 36 "C:\hswork\workFile\jira_dashboard\trunk\Views\Home\EngagementByProject.cshtml"
                                           Write(i.Resource);

#line default
#line hidden
            EndContext();
            BeginContext(1335, 110, true);
            WriteLiteral("</td>\r\n                        <td style=\"width:80px\">1</td>\r\n                        <td style=\"width:110px\">");
            EndContext();
            BeginContext(1446, 23, false);
#line 38 "C:\hswork\workFile\jira_dashboard\trunk\Views\Home\EngagementByProject.cshtml"
                                           Write(i.TotalLoggedHoursMonth);

#line default
#line hidden
            EndContext();
            BeginContext(1469, 34, true);
            WriteLiteral("</td>\r\n                    </tr>\r\n");
            EndContext();
#line 40 "C:\hswork\workFile\jira_dashboard\trunk\Views\Home\EngagementByProject.cshtml"
                }

#line default
#line hidden
            BeginContext(1522, 42, true);
            WriteLiteral("            </table>\r\n        </details>\r\n");
            EndContext();
#line 43 "C:\hswork\workFile\jira_dashboard\trunk\Views\Home\EngagementByProject.cshtml"
    }

#line default
#line hidden
            BeginContext(1571, 193, true);
            WriteLiteral("    <table class=\"table-responsive\">\r\n        <tr>\r\n            <th style=\"background-color:#DDEBF7; width:250px\">Grand Total</th>\r\n            <th style=\"background-color:#DDEBF7; width:80px\">");
            EndContext();
            BeginContext(1765, 33, false);
#line 47 "C:\hswork\workFile\jira_dashboard\trunk\Views\Home\EngagementByProject.cshtml"
                                                        Write(Model.EngagementByProjectTotal[0]);

#line default
#line hidden
            EndContext();
            BeginContext(1798, 69, true);
            WriteLiteral("</th>\r\n            <th style=\"background-color:#DDEBF7; width:110px\">");
            EndContext();
            BeginContext(1868, 33, false);
#line 48 "C:\hswork\workFile\jira_dashboard\trunk\Views\Home\EngagementByProject.cshtml"
                                                         Write(Model.EngagementByProjectTotal[1]);

#line default
#line hidden
            EndContext();
            BeginContext(1901, 50, true);
            WriteLiteral("</th>\r\n        </tr>\r\n    </table>\r\n</div>\r\n\r\n\r\n\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
