#pragma checksum "C:\Users\gussa\OneDrive\Documentos\Curso ASP.NET\ProjetoCore\Views\Transacao\Dashboard.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5edcf2ecbe8dee248621d992cf4ebaf9130fd732"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Transacao_Dashboard), @"mvc.1.0.view", @"/Views/Transacao/Dashboard.cshtml")]
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
#nullable restore
#line 1 "C:\Users\gussa\OneDrive\Documentos\Curso ASP.NET\ProjetoCore\Views\_ViewImports.cshtml"
using ProjetoCore;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\gussa\OneDrive\Documentos\Curso ASP.NET\ProjetoCore\Views\_ViewImports.cshtml"
using ProjetoCore.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5edcf2ecbe8dee248621d992cf4ebaf9130fd732", @"/Views/Transacao/Dashboard.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bc1562796a1a7e75d8b7b001550569ef1c43e61e", @"/Views/_ViewImports.cshtml")]
    public class Views_Transacao_Dashboard : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<h3> Meu Dashboard </h3>

<script src=""https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.5.0/Chart.min.js""></script>
<canvas id=""pie-chart"" height=""130""></canvas>
<script>
    new Chart(document.getElementById(""pie-chart""), {
        type: 'pie',
        data: {
            labels: [");
#nullable restore
#line 9 "C:\Users\gussa\OneDrive\Documentos\Curso ASP.NET\ProjetoCore\Views\Transacao\Dashboard.cshtml"
                Write(Html.Raw(ViewBag.Labels));

#line default
#line hidden
#nullable disable
            WriteLiteral("],\r\n            datasets: [{\r\n                label: \"Population (millions)\",\r\n                backgroundColor: [");
#nullable restore
#line 12 "C:\Users\gussa\OneDrive\Documentos\Curso ASP.NET\ProjetoCore\Views\Transacao\Dashboard.cshtml"
                             Write(Html.Raw(ViewBag.Cores));

#line default
#line hidden
#nullable disable
            WriteLiteral("],\r\n                data: [");
#nullable restore
#line 13 "C:\Users\gussa\OneDrive\Documentos\Curso ASP.NET\ProjetoCore\Views\Transacao\Dashboard.cshtml"
                  Write(Html.Raw(ViewBag.Valores));

#line default
#line hidden
#nullable disable
            WriteLiteral("]\r\n            }]\r\n        },\r\n        options: {\r\n            title: {\r\n                display: true,\r\n                text: \'Predicted world population (millions) in 2050\'\r\n            }\r\n        }\r\n    });\r\n</script>");
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
