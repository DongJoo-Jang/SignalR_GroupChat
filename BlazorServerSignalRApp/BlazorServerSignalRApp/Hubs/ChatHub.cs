using Microsoft.AspNetCore.SignalR;

namespace BlazorServerSignalRApp.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendAllMessage(string user , string message)
        {
            await Clients.All.SendAsync("ReceiveAllMessage", user, message);
        }

        public async Task SendGroupMessage(string group, string user, string message)
        {
            await Clients.Group(group).SendAsync("ReceiveGroupMessage",group ,user, message);
        }

        public async Task AddToGroup(string groupName, string ID)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("ReceiveAckAddGroup", $"{ID}님이 그룹[{groupName}]에 참여했습니다.");
        }

        public async Task RemoveFromGroup(string groupName, string ID)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("ReceiveAckRemoveGroup", $"{ID}님이 그룹[{groupName}]에서 나갔습니다.");
        }

    }
}
