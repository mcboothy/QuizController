using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace QuizController.ViewModels
{
    public class MainViewModel : ComponentBase
    {
        private HubConnection hubConnection; //for connecting to SignalR
        private readonly HttpClient _httpClient = new HttpClient(); //HttpClient for posting messages

        private readonly string functionAppBaseUri = "https://boothy-quiz.azurewebsites.net/api/"; //URL for function app. Leave this as is for now.

        protected override async Task OnInitializedAsync() //actions to do when the page is initialized
        {
            try
            {
                Messages.Add(new ClientMessage() { Message = "Connecting.." });
                //create a hub connection to the function app as we'll go via the function for everything SignalR.
                hubConnection = new HubConnectionBuilder()
                    .WithUrl(functionAppBaseUri)
                    .Build();

                Messages.Add(new ClientMessage() { Message = "About to start" });

                //Registers handler that will be invoked when the hub method with the specified method name is invoked.
                hubConnection.On<ClientMessage>("messages", (clientMessage) =>
                {
                    Messages.Add(clientMessage);
                    StateHasChanged(); //This tells Blazor that the UI needs to be updated
                });

                await hubConnection.StartAsync(); //start connection!
            }
            catch (Exception ex)
            {
                Error = $"Failed to setup hub {ex.Message}";
            }
        }

        //send our message to the function app
        public async Task SendAsync()
        {

            var msg = new ClientMessage
            {
                Name = UserInput,
                Message = MessageInput
            };

            await _httpClient.PostAsJsonAsync($"{functionAppBaseUri}messages", msg); // post to the function app
            MessageInput = string.Empty; // clear the message from the textbox
            StateHasChanged(); //update the UI
        }

        //Check we're connected
        public bool IsConnected =>
            hubConnection.State == HubConnectionState.Connected;

        public List<ClientMessage> Messages { get; private set; }
            = new List<ClientMessage>(); //List of messages to display

        public HubConnectionState State { get { return hubConnection.State; } }

        public string UserInput { get; set; }

        public string MessageInput { get; set; }
        public string Error { get; private set; }

        public class ClientMessage
        {
            public string Name { get; set; }
            public string Message { get; set; }
        }
    }
}
