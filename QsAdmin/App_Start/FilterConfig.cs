using System.Web;
using System.Web.Mvc;

namespace QsAdmin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            // 認証必須にする。
            filters.Add(new AuthorizeAttribute());
        }
    }
}
