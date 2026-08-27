using QPET.Domain.Entities;
using System.Xml.Linq;

namespace QPET.Models
{
    public class AdminEnquiryListViewModel
{
    public IEnumerable<QPET.Domain.Entities.Enquiry> Enquiries
    { get; set; }
        = new List<QPET.Domain.Entities.Enquiry>();

    public QPET.Domain.Entities.EnquiryStatus? SelectedStatus
    { get; set; }
}
}