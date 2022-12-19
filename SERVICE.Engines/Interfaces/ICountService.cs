using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE.Engine.Interfaces
{
    public interface ICountService
    {
        int CountNews();
        int CountCategories();
        int CountTags();
        int CountGuests();
        //Task<int> CountAgencyNews();
        int CountUsers();
        int CountSiteClick();
        int CountVideo();

    }
}
