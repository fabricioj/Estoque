using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.App.TagHelpers
{
    [HtmlTargetElement("textarea", Attributes = InputReadOnlyTagHelper.ReadOnlyAttributeName)]
    public class TextAreaReadOnlyTagHelper : InputReadOnlyTagHelper
    {

    }
}
