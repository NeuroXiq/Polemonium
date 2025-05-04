using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System.Text;

namespace Polemonium.WebApp.Web.Helpers
{
    public class PHtmlHelper
    {
        public static string IconThumbUp()
        {
            string icon = "<svg xmlns=\"http://www.w3.org/2000/svg\" height=\"24px\" viewBox=\"0 -960 960 960\" width=\"24px\" fill=\"#e3e3e3\"><path d=\"M720-120H280v-520l280-280 50 50q7 7 11.5 19t4.5 23v14l-44 174h258q32 0 56 24t24 56v80q0 7-2 15t-4 15L794-168q-9 20-30 34t-44 14Zm-360-80h360l120-280v-80H480l54-220-174 174v406Zm0-406v406-406Zm-80-34v80H160v360h120v80H80v-520h200Z\"/></svg>";

            return $"<div class=\"icon icon-thumb-up\">{icon}</div>";
        }

        public static string IconThumbDown()
        {
            string icon = "<svg xmlns=\"http://www.w3.org/2000/svg\" height=\"24px\" viewBox=\"0 -960 960 960\" width=\"24px\" fill=\"#e3e3e3\"><path d=\"M240-840h440v520L400-40l-50-50q-7-7-11.5-19t-4.5-23v-14l44-174H120q-32 0-56-24t-24-56v-80q0-7 2-15t4-15l120-282q9-20 30-34t44-14Zm360 80H240L120-480v80h360l-54 220 174-174v-406Zm0 406v-406 406Zm80 34v-80h120v-360H680v-80h200v520H680Z\"/></svg>\r\n";
            return $"<div class=\"icon icon-thumb-down\">{icon}</div>";
        }

        public static HtmlString IconPlus()
        {
            string icon = "<svg xmlns=\"http://www.w3.org/2000/svg\" height=\"24px\" viewBox=\"0 -960 960 960\" width=\"24px\" fill=\"#e3e3e3\"><path d=\"M440-440H200v-80h240v-240h80v240h240v80H520v240h-80v-240Z\"/></svg>";

            string result = $"<div class=\"icon\">{icon}</div>";

            return new HtmlString(result);
        }

        public static HtmlString Pagination(int pageCount, int currentPage, Func<int, string> href)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class=\"pagination\">");
            for (int i = 1; i <= pageCount ; i++)
            {
                sb.AppendLine(PaginationButton(i, currentPage == i, href));
            }
            sb.AppendLine("</div>");
            return new HtmlString(sb.ToString());
        }

        private static string PaginationButton(int page, bool isCurrent, Func<int, string> href)
        {
            string css = isCurrent ? "btn-pagination current" : "btn-pagination";
            return $"<a class=\"{css}\" href=\"{href(page)}\">{page}</a>";
        }
    }
}
