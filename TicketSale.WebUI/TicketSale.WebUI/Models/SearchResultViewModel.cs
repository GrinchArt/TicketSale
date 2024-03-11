namespace TicketSale.WebUI.Models
{
    public class SearchResultViewModel
    {
        public IEnumerable<RouteScheduleViewModel> RouteSchedules {  get; set; }
        public SearchCriteria Criteria { get; set; }
        public int CurrentPage {  get; set; }
        public int PageCount { get; set; }
    }
}
