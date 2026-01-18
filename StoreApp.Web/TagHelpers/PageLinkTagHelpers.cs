using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using StoreApp.Web.Models;

namespace StoreApp.Web.TagHelpers;
[HtmlTargetElement("div",Attributes ="Page-model")]
public class PageLinkTagHelpers:TagHelper
{
    private IUrlHelperFactory _urlHelperFactory;
    public PageLinkTagHelpers(IUrlHelperFactory urlHelperFactory)
    {
        _urlHelperFactory=urlHelperFactory;
    }
    [ViewContext]
    public ViewContext? ViewContext { get; set; }
    public PageInfo? PageModel { get; set; }
    public string? PageAction { get; set; }
    public string PageClass { get; set; }=string.Empty;
    public string PageClassLink { get; set; } = string.Empty;
    public string PageClassActive { get; set; } = string.Empty;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
       if(ViewContext !=null && PageModel != null)
        {
            IUrlHelper UrlHelper=_urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder div=new TagBuilder("div");
            //UrlHelper.Action(PageAction,new {page=1});
            for (int i = 1; i <= PageModel.Totalpages; i++)
            {
               TagBuilder link=new TagBuilder("a");
               link.Attributes["href"]= UrlHelper.Action(PageAction, new { page = i });
               link.AddCssClass(PageClass);
               link.AddCssClass(i==PageModel.CurrentPage ? PageClassActive :PageClassLink);
               link.InnerHtml.Append(i.ToString());
               div.InnerHtml.AppendHtml(link);
            }
            output.Content.AppendHtml(div.InnerHtml);
        }
    }
}
