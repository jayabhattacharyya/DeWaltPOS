using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using DeWaltPOS.Models;

namespace DeWalt.Code
{
    public static class Extensions
    {

        /// <summary>
        /// With a reference datetime, produces a new datetime that is the date of the specified future day of the week.
        /// 
        /// The future day date is returned even if the start date day is equal to the required day. This functionality
        /// can be overridden by specifiying True for optional holdIfSameDay.
        /// </summary>
        /// <param name="refDate"></param>
        /// <param name="day">The future day of the week for which a new datetime is required.</param>
        /// <param name="holdIfSameDay">The datetime of the refernce date is returned if the day of week matches the required day.</param>
        /// <returns></returns>
        public static DateTime GetFutureDayDate(this DateTime refDate, DayOfWeek day, bool holdIfSameDay = false)
        { /* calculate the nearest future day date */
            if (holdIfSameDay && refDate.DayOfWeek == day)
                return refDate;
            DateTime futureDateTime = refDate;
            while (futureDateTime.DayOfWeek != day)
                futureDateTime = futureDateTime.AddDays(1);
            return futureDateTime;
        }

        /// <summary>
        /// With a reference datetime, produces a new datetime that is the date of the specified past day of the week.
        /// 
        /// The past day date is returned even if the start date day is equal to the required day. This functionality
        /// can be overridden by specifiying True for optional holdIfSameDay.
        /// </summary>
        /// <param name="refDate"></param>
        /// <param name="day">The past day of the week for which a new datetime is required.</param>
        /// <param name="holdIfSameDay">The datetime of the refernce date is returned if the day of week matches the required day.</param>
        /// <returns></returns>
        public static DateTime GetPastDayDate(this DateTime refDate, DayOfWeek day, bool holdIfSameDay = false)
        { /* calculate the nearest past day date */
            if (holdIfSameDay && refDate.DayOfWeek == day)
                return refDate;
            DateTime pastDateTime = refDate;
            while (pastDateTime.DayOfWeek != day)
                pastDateTime = pastDateTime.AddDays(-1);
            return pastDateTime;
        }


        public static IQueryable<Product> ProductFull(this DeWaltPOSEntities entities)
        {
            return entities.Products
                .Include("icms_Collection")
                .Include("PanelType1")
                .Include("RelatedProducts")
                .Include("ProductPlaceholder1")
                .Include("ProductType1");
        }



        public static MvcHtmlString ToSelectOptions(this IEnumerable<Product> products)
        {
            if (products == null)
                return new MvcHtmlString("");
            StringBuilder options = new StringBuilder();
            foreach (Product p in products)
                options.Append(string.Format("<option value=\"{0}\">{1}: {2}</option>", p.id, p.sku, p.Description == null ? "" : p.Description));
            return new MvcHtmlString(options.ToString());
        }
    }
}