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
        public async Task<ActionResult> Index()
        {
            var SpotSource = await this.GetSpotData();
            ViewData.Model = SpotSource;
            return View();
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
            policy.AbsoluteExpiration = DateTime.Now.AddMinutes(30);//30分鐘後重新抓取

            ObjectCache cacheitem = MemoryCache.Default;
            cacheitem.Add(cacheName, collection, policy);

            return collection;
        }
    }
}