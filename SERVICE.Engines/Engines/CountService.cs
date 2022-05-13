using CORE.ApplicationCore.UnitOfWork;
using GazeteKapiMVC5Core.DataAccessLayer.Models;
using GazeteKapiMVC5Core.DataAccessLayer;
using SERVICE.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE.Engine.Engines
{
    public class CountService : ICountService
    {
        private readonly IUnitOfWork<NewsAppContext> _unitOfWork;
        public CountService(IUnitOfWork<NewsAppContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;       
        }

        public int CountCategories()
        {
            IEnumerable<Categories> categoryCount = _unitOfWork.GetRepository<Categories>().Filter(null, x => x.OrderBy(y => y.Id), "", null, null);

            int count = categoryCount.Count();

            return count;
        }

        public int CountGuests()
        {
            IEnumerable<Guest> guestCount = _unitOfWork.GetRepository<Guest>().Filter(null, x => x.OrderBy(y => y.Id), "", null, null);

            int count = guestCount.Count();

            return count;
        }

        public int CountNews()
        {
            IEnumerable<News> newsCount = _unitOfWork.GetRepository<News>().Filter(null, x => x.OrderBy(y => y.Id), "", null, null);

            int count = newsCount.Count();

            return count;
        }

        public int CountTags()
        {
            IEnumerable<Tags> tagsCount = _unitOfWork.GetRepository<Tags>().Filter(null, x => x.OrderBy(y => y.Id), "", null, null);

            int count = tagsCount.Count();

            return count;
        }

        public int CountUsers()
        {
            IEnumerable<Users> usersCount = _unitOfWork.GetRepository<Users>().Filter(null, x => x.OrderBy(y => y.Id), "", null, null);

            int count = usersCount.Count();

            return count;
        }
    }
}
