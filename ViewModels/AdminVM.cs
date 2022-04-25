using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GCRBA.Models;

namespace GCRBA.ViewModels
{
    public class AdminVM
    {
        public User User { get; set; }
        public Company Company { get; set; }
        public List<Company> Companies { get; set; }
        public Location Location { get; set; }
        public List<Location> Locations { get; set; }
        public State State { get; set; }
        public List<State> States { get; set; }
        public NewLocation NewLocation { get; set; }
        public ContactPerson ContactPerson { get; set; }
        public List<ContactPerson> Contacts { get; set; }
        public CategoryItem Category { get; set; }
        public List<CategoryItem> Categories { get; set; }
        public SaleSpecial Special { get; set; }
        public List<SaleSpecial> Specials { get; set; }
        public Button Button { get; set; }
        public MemberRequest MemberRequest { get; set; }
        public List<MemberRequest> MemberRequests { get; set; }
    }
}