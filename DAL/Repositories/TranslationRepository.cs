using Contracts.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories
{
    public class TranslationRepository : Repository,ITranslationRepository
    {

        public TranslationRepository(API_DBContext context):base(context)
        {
        }

        public IEnumerable<RefTranslationGroup> GetGroupList()
        {
            return _context.RefTranslationGroups; 
        }

        public IEnumerable<RefTranslation> GetTranslationList()
        {
            return _context.RefTranslations.Include(x => x.TranslationGroup); 
        }
    }
}
