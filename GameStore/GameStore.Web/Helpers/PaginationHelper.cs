using System.Text;
using System.Web.Mvc;
using GameStore.BusinessLogicLayer.Domain;

namespace GameStore.Web.Helpers
{
    public static class PaginationHelper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PageInfo pageInfo)
        {
            if (pageInfo.TotalPages == 1)
            {
                return null;
            }

            var result = new StringBuilder();
            for (var i = 1; i <= pageInfo.TotalPages; i++)
            {
                var tag = new TagBuilder("button");
                tag.MergeAttribute("type", "submit");
                tag.MergeAttribute("name", "page");
                tag.MergeAttribute("value", i.ToString());
                tag.InnerHtml = i.ToString();
                if (i == pageInfo.PageNumber)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }

                tag.AddCssClass("btn btn-default");
                result.Append(tag);
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}