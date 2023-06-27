using DTO.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Managers
{
    public interface IBroadCastService
    {
        /// <summary>
        /// Broadcast
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sub"></param>
        void Subscribe(int id, PushSubscriptionModel sub);
               
        /// <summary>
        /// UnSubscribe
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sub"></param>
        void Unsubscribe(int id, PushSubscriptionModel sub);
        

        /// <summary>
        /// Broadcast
        /// </summary>
        /// <param name="id"></param>
        /// <param name="notification"></param>
        /// <returns></returns>
        bool Broadcast(int id, Notification notification);


        /// <summary>
        /// Bordcast User Id List
        /// </summary>
        /// <param name="idList"></param>
        /// <param name="notification"></param>
        void Broadcast(IEnumerable<int> idList, Notification notification);

    }
}
