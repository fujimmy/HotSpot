using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HotSpot.Models;
using System.Runtime.Caching;

namespace HotSpot.Controllers
{
    public class HotSpotController : Controller
    {
        // GET: HotSpot
        public async Task<ActionResult> Index(string sarea)
        {
            //場站下拉設定
            ViewBag.Sarea = await this.GetDropDown(await this.GetSarea(), sarea);
            ViewBag.SelectedSarea = sarea;

            var SpotSource = await this.GetSpotData();
            SpotSource = SpotSource.AsQueryable();

            if (!string.IsNullOrWhiteSpace(sarea))
            {
                SpotSource = SpotSource.Where(w => w.sarea == sarea);
            }
            return View(SpotSource.OrderBy(o => o.sarea).ToList());
        }

        private async Task<IEnumerable<Spot>> GetSpotData()
        {
            string cacheName = "UBikeSpot";

            ObjectCache cache = MemoryCache.Default;
            CacheItem cacheContents = cache.GetCacheItem(cacheName);

            try
            {
                return await RetriveSpotData(cacheName);
            }
            catch (Exception)
            {

                return cacheContents.Value as IEnumerable<Spot>;
            }
        }

        private async Task<IEnumerable<Spot>> RetriveSpotData(string cacheName)
        {
            string targetURI = "http://data.ntpc.gov.tw/od/data/api/54DDDC93-589C-4858-9C95-18B2046CC1FC?$format=json";

            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = Int32.MaxValue;

            var response = await client.GetStringAsync(targetURI);

            var collection = JsonConvert.DeserializeObject<IEnumerable<Spot>>(response);

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now.AddMinutes(30);//保留30分鐘

            ObjectCache cacheitem = MemoryCache.Default;
            cacheitem.Add(cacheName, collection, policy);
            return collection;
        }

        /// <summary>
        /// 取得場站區域
        /// </summary>
        /// <returns></returns>
        private async Task<List<string>> GetSarea()
        {
            try
            {
                var source = await this.GetSpotData();
                var sarea = source.OrderBy(o => o.sarea)
                    .Select(s => s.sarea).Distinct();
                return sarea.ToList();
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// 轉換成下拉選單
        /// </summary>
        /// <param name="sarea"></param>
        /// <returns></returns>
        private async Task<IEnumerable<SelectListItem>> GetDropDown(IEnumerable<string> source, string selectitem)
        {
            var dropdown = source.Select(item => new SelectListItem()
                {
                    Text = item,
                    Value = item,
                    Selected = !string.IsNullOrWhiteSpace(selectitem) && item.Equals(selectitem, StringComparison.OrdinalIgnoreCase)
                });
            return dropdown;
        }
    }
}