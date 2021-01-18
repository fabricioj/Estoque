using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.App.TagHelpers
{
    [HtmlTargetElement("input", Attributes = ReadOnlyAttributeName)]
    public class InputReadOnlyTagHelper : TagHelper
    {
        public const string ReadOnlyAttributeName = "readonly-when";

        [HtmlAttributeName(ReadOnlyAttributeName)]
        public bool ReadOnly { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (ReadOnly)
            {
                output.Attributes.Add("readonly", "readonly");
            }
        }
    }
}
