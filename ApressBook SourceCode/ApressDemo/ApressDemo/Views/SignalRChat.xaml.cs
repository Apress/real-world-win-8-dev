using Microsoft.AspNet.SignalR.Client.Hubs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ApressDemo.Views
{
    public sealed partial class SignalRChat : ApressDemo.Common.LayoutAwarePage
    {
        #region "Members"

        // Reference to the SignalR Server.
        IHubProxy SignalRChatHub;

        // Custom delegate.
        public delegate void SignalRServerHandler(object sender, SignalREventArgs e);

        // Custom event to act when something happens on SignalR Server.
        event SignalRServerHandler SignalRServerNotification;

        #endregion

        #region "Constructor"

        public SignalRChat()
        {
            this.InitializeComponent();
        }

        #endregion

        #region "Event Handlers"

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            HubConnection chatConnection = new HubConnection("http://localhost:1141/");
            SignalRChatHub = chatConnection.CreateHubProxy("ChatHub");

            // Wire-up to listen to custom event from SignalR Hub.
            SignalRServerNotification += new SignalRServerHandler(SignalRHub_SignalRServerNotification);


            // Start connection back to Hub.
            try
            {
                await chatConnection.Start();

                if (chatConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                {
                    // Join the ChatRoom.
                    await SignalRChatHub.Invoke("JoinChat", "DeviceID123", "ChatUserSam");

                    // Listen to chat events on SignalR Server & wire them up appropriately.
                    SignalRChatHub.On<string>("addChatMessage", message =>
                    {
                        SignalREventArgs chatArgs = new SignalREventArgs();
                        chatArgs.ChatMessageFromServer = message;

                        // Raise custom event & let it bubble up.
                        SignalRServerNotification(this, chatArgs);
                    });

                }
            }
            catch (Exception)
            {
                // Do some error handling.
            }
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // Tell Chatrrom you are leaving.
            await SignalRChatHub.Invoke("LeaveChat", "DeviceID123", "ChatUserSam");

            // Unwire event handler.
            SignalRServerNotification -= new SignalRServerHandler(SignalRHub_SignalRServerNotification);
        }

        private async void postChatMessage_Click(object sender, RoutedEventArgs e)
        {
            if (this.chatMessage.Text.Trim() != string.Empty)
            {
                // Broadcast message to ChatRoom
                await SignalRChatHub.Invoke("PushMessageToClients", this.chatMessage.Text.Trim());

                this.chatMessage.Text = string.Empty;
            }
        }

        private async void SignalRHub_SignalRServerNotification(object sender, SignalREventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // Add to local ChatRoom.
                this.chatDialogue.Text += "\r\n" + e.ChatMessageFromServer;
            });
        }

        #endregion
    }
}


public class SignalREventArgs : EventArgs
{
    // Custom args.
    public string ChatMessageFromServer { get; set; }
}

