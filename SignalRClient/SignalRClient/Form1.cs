using Microsoft.AspNetCore.SignalR.Client;
namespace SignalRClient
{
    public partial class Form1 : Form
    {
        HubConnection signalR;
        public Form1()
        {
            InitializeComponent();
            signalR = new HubConnectionBuilder().WithUrl("https://localhost:7060/ChatHub").Build();
            signalR.Closed += SignalR_Closed;

        }

        private async Task SignalR_Closed(Exception arg)
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await signalR.StartAsync();
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            signalR.On<string, string>("ReceiveAllMessage", new Action<string, string>((x, y) => ReceiveAllMessage(x, y)));
            signalR.On<string, string, string>("ReceiveGroupMessage", new Action<string, string, string>((x, y, z) => ReceiveGroupMessage(x, y, z)));
            signalR.On<string>("ReceiveAckAddGroup", new Action<string>(x => ReceiveAckAddGroup(x)));
            signalR.On<string>("ReceiveAckRemoveGroup", new Action<string>(x => ReceiveAckRemoveGroup(x)));
            try
            {
                await signalR.StartAsync();
                listBoxContent.Items.Add("Connection started");
                btnConnect.Enabled = false;
                btnSendAll.Enabled = true;
            }
            catch (Exception ex)
            {
                listBoxContent.Items.Add(ex.Message);
            }
        }



      
        private async void btnSendAll_Click(object sender, EventArgs e)
        {
            try
            {
                await signalR.InvokeAsync("SendAllMessage", txtID.Text, txtMessage.Text);
            }catch(Exception ex)
            {

            }
        }
        private async void btnSendGroup_Click(object sender, EventArgs e)
        {
            foreach (var listBoxItem in listBoxGroup.Items)
            {
                await signalR.InvokeAsync("SendGroupMessage", listBoxItem.ToString(), txtID.Text, txtMessage.Text);
            }

        }

        private async void btnAddGroup_Click(object sender, EventArgs e)
        {
            try
            {
                    await signalR.InvokeAsync("AddToGroup", txtGroup.Text, txtID.Text);
                    listBoxGroup.Items.Add(txtGroup.Text);
            }
            catch (Exception ex)
            {

            }
        }

        private async void btnRemoveGroup_Click(object sender, EventArgs e)
        {
            try
            {
                await signalR.InvokeAsync("RemoveFromGroup",  txtGroup.Text, txtID.Text);
                listBoxGroup.Items.Remove(txtGroup.Text);
            }
            catch (Exception ex)
            {

            }
        }

    

        public void ReceiveAllMessage(string user, string message)
        {
            listBoxContent.Items.Add($"전체발송 {user}:, {message}");
        }

        public void ReceiveGroupMessage(string group, string user, string message)
        {
            listBoxContent.Items.Add($"그룹발송[{group}] {user}: {message}");
        }


        public void ReceiveAckAddGroup(string Message)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                listBoxContent.Items.Add($"{Message}");
            }
            ));
        }
        public void ReceiveAckRemoveGroup(string Message)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                listBoxContent.Items.Add($"{Message}");
            }
            ));
        }

    }
}