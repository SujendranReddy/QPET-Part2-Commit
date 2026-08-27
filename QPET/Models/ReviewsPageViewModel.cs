namespace QPET.Models
{
    public class ReviewsPageViewModel
    {
        public List<QPET.Domain.Entities.Review> Reviews
        { get; set; }
            = new List<QPET.Domain.Entities.Review>();

        public List<QPET.Domain.Entities.Branch> Branches
        { get; set; }
            = new List<QPET.Domain.Entities.Branch>();

        public ReviewViewModel Review { get; set; }
            = new ReviewViewModel();
    }
}