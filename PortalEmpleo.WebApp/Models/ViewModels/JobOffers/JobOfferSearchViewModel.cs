namespace PortalEmpleo.WebApp.Models.ViewModels.JobOffers
{
    public class JobOfferSearchViewModel
    {
        public string? SearchTerm { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public JobOfferListViewModel Results { get; set; }
    }
}
