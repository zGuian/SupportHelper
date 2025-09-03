using Microsoft.AspNetCore.Mvc.Rendering;

namespace SupportHelper.FrontEnd.MVC.Models
{
    public class SearchModel
    {
        public string? Hostname { get; set; }
        public string? DomainName { get; set; }
        public string? OperationalSystem { get; set; }
        public DateTime? LastUpdate { get; set; }

        public List<SelectListItem> SelectDomains;

        public SearchModel()
        {
            SelectDomains =
            [
                new() { Value = null, Text = "Selecione um valor" },
                new() { Value = "Tbad", Text = "Tbad" },
                new() { Value = "Americas", Text = "Americas" }
            ];
        }
    }
}
