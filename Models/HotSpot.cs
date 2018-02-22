using System.ComponentModel.DataAnnotations;

namespace HotSpot.Models
{
    public class Spot
    {       
            [Display(Name = "站點代號")]
            public string sno { get; set; }

            [Display(Name = "場站名稱")]
            public string sna { get; set; }

            [Display(Name = "場站總停車格")]
            public string tot { get; set; }

            [Display(Name = "可借車位數")]
            public string sbi { get; set; }

            [Display(Name = "場站區域")]
            public string sarea { get; set; }

            [Display(Name = "資料更新時間")]
            public string mday { get; set; }

            [Display(Name = "緯度")]
            public string lat { get; set; }

            [Display(Name = "經度")]
            public string lng { get; set; }

            [Display(Name = "地址")]
            public string ar { get; set; }

            [Display(Name = "場站區域")]
            public string sareaen { get; set; }

            [Display(Name = "場站名稱")]
            public string snaen { get; set; }

            [Display(Name = "地址")]
            public string aren { get; set; }

            [Display(Name = "可還空位數")]
            public string bemp { get; set; }

            [Display(Name = "是否暫停營運")]
            public string act { get; set; }        
    }
}