using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
namespace NetCrudStarter.Model
{
    public enum OrderDir
    {
        ASC,
        DESC
    }

    public class PageModel
    {

        [FromQuery(Name = "f")]
        public Dictionary<string, string> FilterDict { get; set; } = []; 
        [FromQuery(Name = "include-graph")]
        public string? IncludeGraph { get; set; }
        [FromQuery(Name = "page-start")]
        public int PageStart { get; set; }
        [FromQuery(Name = "page-items")]
        public int PageItems { get; set; }
        [FromQuery(Name="sort")]
        public List<string> SortList { get; set; }

        public PageModel(int? pageStart, int? pageItems, string orderField)
        {
            this.PageStart = pageStart != null ? (int) pageStart : 1;
            this.PageItems = pageItems != null ? (int) pageItems : 20;
            SortList = [orderField];
        }
        public PageModel()
        {
            FilterDict = [];
             SortList = [];
            this.PageStart = 1;
            this.PageItems = Int32.MaxValue;
        }
    }
}
