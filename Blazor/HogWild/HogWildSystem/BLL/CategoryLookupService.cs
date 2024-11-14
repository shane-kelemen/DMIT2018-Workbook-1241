using HogWildSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HogWildSystem.ViewModels;

namespace HogWildSystem.BLL
{
    public class CategoryLookupService
    {
        #region Fields

        private readonly HogWildContext _hogWildContext;

        #endregion

        //  Constructor for the WorkingVersionsService class.
        internal CategoryLookupService(HogWildContext hogWildContext)
        {
            //  Initialize the _hogWildContext field with the provided HogWoldContext instance.
            _hogWildContext = hogWildContext;
        }

        public List<LookupView> GetLookups(string categoryName)
        {
            return _hogWildContext.Lookups
                .Where(x => x.Category.CategoryName == categoryName
                            && !x.RemoveFromViewFlag)
                .OrderBy(x => x.Name)
                .Select(x => new LookupView
                {
                    LookupID = x.LookupID,
                    CategoryID = x.CategoryID,
                    Name = x.Name,
                    RemoveFromViewFlag = x.RemoveFromViewFlag
                }).ToList();
        }
    }
}