using System;
using System.Net;
using System.Windows.Forms;
using RestSharp;

namespace SOP
{
    public partial class Login : Form
    {
        private Form1 mainForm;
        private string URL;

        public Login(Form1 mainForm, string url)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.URL = url;

            loginBtn.Click += button1_Click;
        }

        public class LoginResponse
        {
            public string status { get; set; }
            public string access_token { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var client = new RestClient($"{URL}/login");
            var request = new RestRequest("", Method.POST);
            request.RequestFormat = DataFormat.Json;

            string username = usernameField.Text;
            string password = passwordField.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            request.AddJsonBody(new { username, password });

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = SimpleJson.DeserializeObject<LoginResponse>(response.Content);

                if (result.status == "1")
                {
                    // Login successful, store the access token
                    string accessToken = result.access_token;

                    // Display the token in the tokenDisplayField
                    tokenDisplayField.Items.Add($"Access Token: {accessToken}");

                    // Set up a timer to wait for 2 seconds
                    Timer timer = new Timer();
                    timer.Interval = 2000; // 2 seconds
                    timer.Tick += (s, args) =>
                    {
                       
                        mainForm.SetAccessToken(accessToken);

                        this.Close();

                      
                        mainForm.Show();

                     
                        timer.Stop();
                    };

                    // Start the timer
                    timer.Start();
                }
                else
                {
                    MessageBox.Show("Login failed. Invalid username or password.");
                }
            }
            else
            {
                MessageBox.Show($"Login failed. Server returned: {response.StatusDescription}");
            }
        }

    }
}
