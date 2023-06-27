using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Repositories
{
    public interface ITranslationRepository : IRepository
    {
        /// <summary>
        /// Get tRanslation groups
        /// </summary>
        /// <returns></returns>
        IEnumerable<RefTranslationGroup> GetGroupList();

        /// <summary>
        ///  Get Translation Name
        /// </summary>
        /// <returns></returns>
        IEnumerable<RefTranslation> GetTranslationList();


    }
}
