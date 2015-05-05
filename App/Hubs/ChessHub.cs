using Microsoft.AspNet.SignalR;
public class ChessHub : Hub
{
    public void Send(string name, string message)
    {
        // Call the broadcastMessage method to update clients.
        Clients.All.broadcastMessage(name, message);
    }
}