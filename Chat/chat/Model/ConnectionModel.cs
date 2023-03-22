using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.IO;

namespace Chat.Model
{

    /*
     * 
     * Connection class contains 3 important methods.
     * Listen, Connect and ProcessPacket
     * Listen and Connect both create new Tasks when first executed
     * 
     * Listen will accept all incoming tcp connections and wait for data.
     * 
     * The first thing Connect does is to send out a packet in order to
     * establish a connection.
     * 
     * Communication between Listen and Connect is done through Send.
     * 
     * Both Listen and Connect use ProcessPacket in order to decide what to do
     * with the received packet.
     * For example a Pakcet with request type 1 is a chat message
     *      save it with HistoryModel 
     *      display it
     * 
     */

    public class ConnectionModel
    {
        private readonly UserModel _user;
        public HistoryModel _history;

        private Task _listenTask;
        private Task _connectTask;

        private TcpClient _client;
        private TcpListener _listener;
        private NetworkStream _stream;
        private bool connection_status = false;
        public static Action<string> MessageAdd { get; set; }
        public static Action<string, string> ShowMessageBox { get; set; }
        public static Action Buzz { get; set; }
        public static Action SwitchToChatView { get; set; }
        public static Action<string, string, string> RequestResponse { get; set; }


        public ConnectionModel(UserModel user)
        {
            _user = user;
            _history = new HistoryModel();
        }


        public void ExecuteListen(object parameter = null)
        {
            _listenTask = new Task(() => Listen());
            _listenTask.Start();
        }


        public void ExecuteConnect(object parameter = null)
        {
            _connectTask = new Task(() => Connect());
            _connectTask.Start();
        }


        private async void Listen()
        {
            IPAddress localAddr = IPAddress.Parse(_user.Addres);
            _listener = new TcpListener(localAddr, _user.Port);
            try
            {
                _listener.Start();

                byte[] bytes = new byte[256];
                string data = null;

                while (true)
                {
                    // accept all tcp conenctions
                    _client = await _listener.AcceptTcpClientAsync();
                    data = null;
                    _stream = _client.GetStream();

                    int i;
                    if (_stream != null)
                    {
                        while ((i = await _stream.ReadAsync(bytes, 0, bytes.Length)) != 0)
                        {
                            data = Encoding.ASCII.GetString(bytes, 0, i);
                            MessageModel responseMsg = JsonSerializer.Deserialize<MessageModel>(data);

                            // packet has been received
                            // process it by looking at the request type field to know what to do
                            ProcessPacket(responseMsg);

                            // break connection when user declines inivitation, case 3 for example
                            if (connection_status == false)
                            {
                                return;
                            }
                        }
                    }
                }
            }
            catch (SocketException)
            {
                ShowMessageBox?.Invoke("This port is already in use. Try another.", "");
            }

            catch (IOException)
            {
                ShowMessageBox?.Invoke("User disconnected", "");
            }
        }


        // Connect class is only used one time when we create the connection
        private async void Connect()
        {
            try
            {
                _client = new TcpClient(_user.Addres, _user.Port);
                _stream = _client.GetStream();
                // send a message that we want to establish a connection
                Send("", 2, _user.Username);

                int i;
                byte[] bytes = new byte[256];
                string responseData = string.Empty;

                // keep receiving packets 
                while (true)
                {
                    _stream = _client.GetStream();
                    if (_stream != null)
                    {
                        while ((i = await _stream.ReadAsync(bytes, 0, bytes.Length)) != 0)
                        {
                            responseData = Encoding.ASCII.GetString(bytes, 0, i);
                            MessageModel responseMsg = JsonSerializer.Deserialize<MessageModel>(responseData);

                            ProcessPacket(responseMsg);
                            // break connection when user declines inivitation
                            if (connection_status == false)
                            {
                                return;
                            }
                        }
                    }
                }
            }
            catch (SocketException)
            {
                ShowMessageBox?.Invoke("Wrong port or IP-adress.", "");
            }
            catch (IOException)
            {
                ShowMessageBox?.Invoke("User disconnected.", "listen loop");
            }
        }

        // used in MainViewModel.OnWindowClosing
        public void AnnounceDisconnect()
        {
            if (connection_status)
            {
                _history.SaveToFile();
                Send("", 3);
            }
        }

        // all communication with the other instance is done though this method
        public async void Send(string content, int type, string from = "undefinded")
        {
            MessageModel packet = new MessageModel(content, type, from);
            if (type.Equals(1))
            {
                _history.SaveMsg(packet);
            }
            string jsonMsg = JsonSerializer.Serialize(packet);
            byte[] data = Encoding.ASCII.GetBytes(jsonMsg);
            await _stream.WriteAsync(data, 0, data.Length);
        }

        public void DenyConnection()
        {
            connection_status = false;
            Send("", 21);
            _listener.Stop();
            _client.Close();
        }

        public void AcceptConnection(string currentUser)
        {
            connection_status = true;
            Send("", 20, currentUser);
        }


        private void ProcessPacket(MessageModel packet)
        {
            switch (packet.ReqType)
            {
                // Message received
                case 1:
                    string msg = $"[{packet.Date} {packet.FromUser}]: {packet.Content}";
                    MessageAdd?.Invoke(msg);
                    _history.SaveMsg(packet);
                    break;

                // Attempt connect
                case 2:
                    RequestResponse?.Invoke("Accept connection?", packet.FromUser, "");
                    break;

                // Connection request accepted
                case 20:
                    ShowMessageBox?.Invoke("Connection established, user accpeted connection.", "");
                    connection_status = true;
                    _history.Init(_user.Username, packet.FromUser);
                    SwitchToChatView?.Invoke();
                    break;

                // Connection request denied
                case 21:
                    _client.Close();
                    ShowMessageBox?.Invoke("Unable to create connection, user denied request.", "");
                    break;

                // Disconnected
                case 3:
                    ShowMessageBox?.Invoke("User disconnected.", "");
                    _history.SaveToFile();
                    connection_status = false;
                    Environment.Exit(0);
                    break;

                //Buzz button
                case 4:
                    Buzz?.Invoke();
                    break;
                default:
                    break;
            }
        }
    }
}