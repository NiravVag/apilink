using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LINK_UI.App_start
{
    [AllowAnonymous]
    public class UserHub : Hub
    {
        public static List<UserData> UserList = null;

        public class UserData
        {
            public int Id { get; set; }

            public string ConnectionId { get; set; }
        }


        public override Task OnConnectedAsync()
        {

            string userId  = Context.GetHttpContext().Request.Query["userid"];

            if (string.IsNullOrEmpty(userId))
                return null; 

            if (UserList == null)
                UserList = new List<UserData>();

            UserList.Add(new UserData
            {
                ConnectionId = Context.ConnectionId,
                Id = Convert.ToInt32(userId)
            });

            return base.OnConnectedAsync();
        }


        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (UserList.Any(x => x.ConnectionId == Context.ConnectionId))
                UserList = UserList.Where(x => x.ConnectionId != Context.ConnectionId).ToList();

            return base.OnDisconnectedAsync(exception);
        }



    }

    public class EventProgress
    {
        public int Percent { get; set;  }

        public string Message { get; set;  }
        
    }
}
