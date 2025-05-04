using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System.Text;

namespace Polemonium.WebApp.Web.Helpers
{
    public class PHtmlHelper
    {
        public static HtmlString IconLinkBoxUpRight()
        {
            string icon = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-box-arrow-up-right\" viewBox=\"0 0 16 16\">\r\n  <path fill-rule=\"evenodd\" d=\"M8.636 3.5a.5.5 0 0 0-.5-.5H1.5A1.5 1.5 0 0 0 0 4.5v10A1.5 1.5 0 0 0 1.5 16h10a1.5 1.5 0 0 0 1.5-1.5V7.864a.5.5 0 0 0-1 0V14.5a.5.5 0 0 1-.5.5h-10a.5.5 0 0 1-.5-.5v-10a.5.5 0 0 1 .5-.5h6.636a.5.5 0 0 0 .5-.5\"/>\r\n  <path fill-rule=\"evenodd\" d=\"M16 .5a.5.5 0 0 0-.5-.5h-5a.5.5 0 0 0 0 1h3.793L6.146 9.146a.5.5 0 1 0 .708.708L15 1.707V5.5a.5.5 0 0 0 1 0z\"/>\r\n</svg>";

            string result = $"<div class=\"icon\">{icon}</div>";

            return new HtmlString(result);
        }

        public static HtmlString IconChrome()
        {
            string icon = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-browser-chrome\" viewBox=\"0 0 16 16\">\r\n  <path fill-rule=\"evenodd\" d=\"M16 8a8 8 0 0 1-7.022 7.94l1.902-7.098a3 3 0 0 0 .05-1.492A3 3 0 0 0 10.237 6h5.511A8 8 0 0 1 16 8M0 8a8 8 0 0 0 7.927 8l1.426-5.321a3 3 0 0 1-.723.255 3 3 0 0 1-1.743-.147 3 3 0 0 1-1.043-.7L.633 4.876A8 8 0 0 0 0 8m5.004-.167L1.108 3.936A8.003 8.003 0 0 1 15.418 5H8.066a3 3 0 0 0-1.252.243 2.99 2.99 0 0 0-1.81 2.59M8 10a2 2 0 1 0 0-4 2 2 0 0 0 0 4\"/>\r\n</svg>";

            string result = $"<div class=\"icon\">{icon}</div>";

            return new HtmlString(result);
        }

        public static HtmlString IconEdge()
        {
            string icon = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-browser-edge\" viewBox=\"0 0 16 16\">\r\n  <path d=\"M9.482 9.341c-.069.062-.17.153-.17.309 0 .162.107.325.3.456.877.613 2.521.54 2.592.538h.002c.667 0 1.32-.18 1.894-.519A3.84 3.84 0 0 0 16 6.819c.018-1.316-.44-2.218-.666-2.664l-.04-.08C13.963 1.487 11.106 0 8 0A8 8 0 0 0 .473 5.29C1.488 4.048 3.183 3.262 5 3.262c2.83 0 5.01 1.885 5.01 4.797h-.004v.002c0 .338-.168.832-.487 1.244l.006-.006z\"/>\r\n  <path d=\"M.01 7.753a8.14 8.14 0 0 0 .753 3.641 8 8 0 0 0 6.495 4.564 5 5 0 0 1-.785-.377h-.01l-.12-.075a5.5 5.5 0 0 1-1.56-1.463A5.543 5.543 0 0 1 6.81 5.8l.01-.004.025-.012c.208-.098.62-.292 1.167-.285q.194.001.384.033a4 4 0 0 0-.993-.698l-.01-.005C6.348 4.282 5.199 4.263 5 4.263c-2.44 0-4.824 1.634-4.99 3.49m10.263 7.912q.133-.04.265-.084-.153.047-.307.086z\"/>\r\n  <path d=\"M10.228 15.667a5 5 0 0 0 .303-.086l.082-.025a8.02 8.02 0 0 0 4.162-3.3.25.25 0 0 0-.331-.35q-.322.168-.663.294a6.4 6.4 0 0 1-2.243.4c-2.957 0-5.532-2.031-5.532-4.644q.003-.203.046-.399a4.54 4.54 0 0 0-.46 5.898l.003.005c.315.441.707.821 1.158 1.121h.003l.144.09c.877.55 1.721 1.078 3.328.996\"/>\r\n</svg>";

            string result = $"<div class=\"icon\">{icon}</div>";

            return new HtmlString(result);
        }

        public static HtmlString IconGithub()
        {
            string icon = "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-github\" viewBox=\"0 0 16 16\">\r\n  <path d=\"M8 0C3.58 0 0 3.58 0 8c0 3.54 2.29 6.53 5.47 7.59.4.07.55-.17.55-.38 0-.19-.01-.82-.01-1.49-2.01.37-2.53-.49-2.69-.94-.09-.23-.48-.94-.82-1.13-.28-.15-.68-.52-.01-.53.63-.01 1.08.58 1.23.82.72 1.21 1.87.87 2.33.66.07-.52.28-.87.51-1.07-1.78-.2-3.64-.89-3.64-3.95 0-.87.31-1.59.82-2.15-.08-.2-.36-1.02.08-2.12 0 0 .67-.21 2.2.82.64-.18 1.32-.27 2-.27s1.36.09 2 .27c1.53-1.04 2.2-.82 2.2-.82.44 1.1.16 1.92.08 2.12.51.56.82 1.27.82 2.15 0 3.07-1.87 3.75-3.65 3.95.29.25.54.73.54 1.48 0 1.07-.01 1.93-.01 2.2 0 .21.15.46.55.38A8.01 8.01 0 0 0 16 8c0-4.42-3.58-8-8-8\"/>\r\n</svg>";

            string result = $"<div class=\"icon\">{icon}</div>";

            return new HtmlString(result);
        }

        public static HtmlString IconSearch()
        {
            string icon = "<svg xmlns=\"http://www.w3.org/2000/svg\" height=\"24px\" viewBox=\"0 -960 960 960\" width=\"24px\" fill=\"#e3e3e3\"><path d=\"M784-120 532-372q-30 24-69 38t-83 14q-109 0-184.5-75.5T120-580q0-109 75.5-184.5T380-840q109 0 184.5 75.5T640-580q0 44-14 83t-38 69l252 252-56 56ZM380-400q75 0 127.5-52.5T560-580q0-75-52.5-127.5T380-760q-75 0-127.5 52.5T200-580q0 75 52.5 127.5T380-400Z\"/></svg>";

            string result = $"<div class=\"icon\">{icon}</div>";

            return new HtmlString(result);
        }

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
