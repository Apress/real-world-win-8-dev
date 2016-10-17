using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNet.SignalR;

namespace SignalRServer
{
    public class ChatHub : Hub
    {
        // List of connected Windows 8 devices on server .. feel free to persist or use this list however.
        private static List<string> Windows8DeviceList = new List<string>();

        // SignalR method call to add a new Windows 8 client connection & join chatroom.
        public void JoinChat(string deviceID, string chatUserName)
        {
            Windows8DeviceList.Add(deviceID);

            // Broadcast new chat user to all.
            Clients.All.addChatMessage(chatUserName + " just joined chatroom!");

            // Put connecting clients in a group.
            // Clients.Group("SomeChatRoomName", null);
        }

        // Disconnect given Windows 8 device client & leave chatroom.
        public void LeaveChat(string deviceID, string chatUserName)
        {
            Windows8DeviceList.Remove(deviceID);

            // Broadcast chat user exit to all.
            Clients.All.addChatMessage(chatUserName + " just left chatroom!");
        }

        // Get chatty.
        public void PushMessageToClients(string message)
        {
            // Push to all connected clients.
            Clients.All.addChatMessage(message);

            // Guess what the next few lines do ...

            // Invoke a method on the calling client only.
            // Clients.Caller.addChatMessage(message)
           
            // Communicate to a Group.
            // Clients.OthersInGroup("SomeChatRoomName").addChatMessage(message);
        }
    }
}