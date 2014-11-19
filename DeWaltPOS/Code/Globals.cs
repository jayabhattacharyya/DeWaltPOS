using System.Web;
using System.Web.Security;


namespace DeWalt
{
    public class Globals
    {
        public static string ImageTempSavePath = "/Images/Temp/";
        public static string ImageThumbSavePath = "/Images/Thumbnails/";
        public static string ImageThumbSmallPath = "/Images/Thumbsmall/";
        public static string ImageCatalogueSavePath = "/Images/Catalogue/";
        public static string DefaultImage = "/Images/dewalt-old.jpg";
        public static int lang = 1;


        public static bool IsAdmin
        {
            get
            {
                if (HttpContext.Current.User != null)
                {
                    string[] roles = Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name);
                    for (int i = 0; i < roles.Length; i++)
                        if (roles[i] == "Admin" || roles[i] == "SysAdmin")
                            return true;
                }
                return false;
            }
        }
        

        public static bool ICMSEnabled
        {
            get
            { 
#if DEBUG
                return true;
#else
   /* currently inline-cms is always on with admin/sysadmin role */
                return IsAdmin;
#endif
                
             
            }
        }

    }
}