using Contracts.Managers;
using DTO.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPush;

namespace LINK_UI.App_start 
{
    public class MyBroadCastService : IBroadCastService
    {
        private static List<(int, PushSubscription)> Subscriptions { get; set; } = new List<(int, PushSubscription)>();

        private readonly VapidDetails _vapidDetails = null; 

        public MyBroadCastService(VapidDetails vapidDetails)
        {
            _vapidDetails = vapidDetails; 
        }

        public void Subscribe(int id, PushSubscriptionModel sub)
        {
            var item = Subscriptions.FirstOrDefault(s => s.Item1 == id && s.Item2.Endpoint == sub.Endpoint);

            try
            {
                if (item.Item2 != null)
                    Subscriptions.Remove(item);

                Subscriptions.Add((id, new PushSubscription {
                    Auth = sub.Auth,
                    Endpoint = sub.Endpoint,
                    P256DH = sub.P256DH
                }));
            }
            catch (Exception ex)
            {
            }
        }

        public void Unsubscribe(int id, PushSubscriptionModel sub)
        {
            var item = Subscriptions.FirstOrDefault(s => s.Item1 == id && s.Item2.Endpoint == sub.Endpoint);

            if (item.Item2 != null)
                Subscriptions.Remove(item);
        }

        public bool AddAndBroadcast(int id, Notification notification)
        {
            var client = new WebPushClient();
            var serializedMessage = JsonConvert.SerializeObject(notification);

            var currentSubscriptions = Subscriptions.Where(x => x.Item1 == id);

            foreach (var item in currentSubscriptions)
            {
                try
                {
                    client.SendNotification(item.Item2, serializedMessage, _vapidDetails);
                }
                catch (Exception ex)
                {
                }
            }

            return true;

        }

        public bool Broadcast(int id, Notification notification)
        {
            var client = new WebPushClient();
            var serializedMessage = JsonConvert.SerializeObject(notification);

            var currentSubscriptions = Subscriptions.Where(x => x.Item1 == id);

            foreach (var item in currentSubscriptions)
            {
                try
                {
                    client.SendNotification(item.Item2, serializedMessage, _vapidDetails);
                }
                catch (Exception ex)
                {
                }
            }

            return true;

        }

        public void Broadcast(IEnumerable<int> idList, Notification notification)
        {
            foreach (int id in idList)
                Broadcast(id, notification);

        }

    }
}
