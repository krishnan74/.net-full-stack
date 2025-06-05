using Microsoft.AspNetCore.SignalR;

namespace Notify.Hubs;

public class DocumentHub : Hub
{
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task NotifyNewDocument(string documentTitle, string uploadedBy)
    {
        await Clients.All.SendAsync("ReceiveDocumentNotification", new
        {
            Title = documentTitle,
            UploadedBy = uploadedBy,
            UploadedAt = DateTime.UtcNow
        });
    }
} 