using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GameStore.Web.Helpers
{
    public static class FilterListHelper
    {
        public static MvcHtmlString CreateList(
            this HtmlHelper html,
            IEnumerable<int> values,
            IEnumerable<string> labels,
            string type,
            string name,
            int[] checkedField)
        {
            var valuesList = values.ToList();
            var labelsList = labels.ToList();
            var ul = new TagBuilder("ul");
            for (var i = 0; i < values.Count(); i++)
            {
                var input = new TagBuilder("input");
                input.MergeAttribute("type", type);
                input.MergeAttribute("name", name);
                input.MergeAttribute("value", valuesList[i].ToString());
                input.MergeAttribute("id", name + valuesList[i]);
                if (checkedField != null && checkedField.Contains(valuesList[i]))
                {
                    input.MergeAttribute("checked", "checked");
                }

                var label = new TagBuilder("label");
                label.MergeAttribute("for", name + valuesList[i]);
                label.SetInnerText(labelsList[i]);
                var li = new TagBuilder("li");
                li.InnerHtml += input.ToString(TagRenderMode.SelfClosing);
                li.InnerHtml += label.ToString();
                ul.InnerHtml += li.ToString();
            }

            return new MvcHtmlString(ul.ToString());
        }
    }
}