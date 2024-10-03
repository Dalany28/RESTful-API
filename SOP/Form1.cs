using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RestSharp;

namespace SOP
{
    public partial class Form1 : Form
    {
        String URL = "http://localhost:5000"; 
        String ROUTE = "/books"; 
        private string accessToken;
        public static Form1 instance;
        public Form1()
        {
            InitializeComponent();
            instance = this;
        }

       

        public string AccessToken
        {
            get { return accessToken; }
            set { accessToken = value; }
        }
        public void SetAccessToken(string accessToken)
        {
            // You can store the access token or use it as needed in your Form1 class
            this.accessToken = accessToken;
        }
        private void IncludeAuthToken(RestRequest request)
        {
            // Check if the access token is available
            if (!string.IsNullOrEmpty(AccessToken))
            {
                // Include the access token in the Authorization header
                request.AddHeader("Authorization", $"Bearer {AccessToken}");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            

            Data.Items.Clear();
            var client = new RestClient($"{URL}{ROUTE}");

            if (!string.IsNullOrEmpty(bookidValue.Text) && int.TryParse(bookidValue.Text, out int bookId))
            {
                var request = new RestRequest($"/{bookId}", Method.GET);

                // Include the access token in the request headers
                IncludeAuthToken(request);

                IRestResponse<List<Book>> response = client.Execute<List<Book>>(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    foreach (Book book in response.Data)
                    {
                        Data.Items.Add($"{book.Id} {book.title} {book.published}");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    MessageBox.Show($"Fetch failed. Book with specified ID not found.");
                }
                else
                {
                    MessageBox.Show($"Fetch failed. Server returned: {response.StatusDescription}");
                }

                bookidValue.Clear();
                titleValue.Clear();
                priceValue.Clear();
                publishedValue.Clear();
            }
            else
            {
                var request = new RestRequest("", Method.GET);

                // Include the access token in the request headers
                IncludeAuthToken(request);

                IRestResponse<List<Book>> response = client.Execute<List<Book>>(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    foreach (Book book in response.Data)
                    {
                        Data.Items.Add($"{book.Id} {book.title} {book.published}");
                    }
                }
                else
                {
                    MessageBox.Show($"Fetch failed. Server returned: {response.StatusDescription}");
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            var client = new RestClient($"{URL}{ROUTE}");
            var request = new RestRequest("", Method.POST);
            request.RequestFormat = DataFormat.Json;

            // Include the access token in the request headers
            IncludeAuthToken(request);

            string newTitle = titleValue.Text;
            string newPrice = priceValue.Text;
            string newPublished = publishedValue.Text;

            if (string.IsNullOrEmpty(newTitle) || string.IsNullOrEmpty(newPrice) || string.IsNullOrEmpty(newPublished))
            {
                MessageBox.Show("All input fields must be filled. Please enter values for all fields.");
                return;
            }

            if (!IsValidDateFormat(newPublished))
            {
                MessageBox.Show("Invalid date format. Please enter a valid date (YYYY-MM-DD).");
                return;
            }

            request.AddJsonBody(new Book
            {
                title = newTitle,
                price = newPrice,
                published = newPublished
            });

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show("Insertion successful!");
            }
            else
            {
                MessageBox.Show($"Insertion failed. Server returned: {response.StatusDescription}");
            }

            bookidValue.Clear();
            titleValue.Clear();
            priceValue.Clear();
            publishedValue.Clear();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            var client = new RestClient($"{URL}{ROUTE}");

            if (!string.IsNullOrEmpty(bookidValue.Text) && int.TryParse(bookidValue.Text, out int updateId))
            {
                var request = new RestRequest($"/{updateId}", Method.PUT);
                request.RequestFormat = DataFormat.Json;

                // Include the access token in the request headers
                IncludeAuthToken(request);

                string updatedTitle = titleValue.Text;
                string updatedPrice = priceValue.Text;
                string updatedPublished = publishedValue.Text;

                if (!IsValidDateFormat(updatedPublished))
                {
                    MessageBox.Show("Invalid date format. Please enter a valid date (YYYY-MM-DD).");
                    return;
                }

                request.AddJsonBody(new Book
                {
                    title = updatedTitle,
                    price = updatedPrice,
                    published = updatedPublished
                });

                IRestResponse response = client.Execute(request);
                MessageBox.Show(response.Content);
                bookidValue.Clear();
                titleValue.Clear();
                priceValue.Clear();
                publishedValue.Clear();
            }
            else
            {
                MessageBox.Show("Invalid book ID for update.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var client = new RestClient($"{URL}{ROUTE}");

            if (!string.IsNullOrEmpty(bookidValue.Text) && int.TryParse(bookidValue.Text, out int deleteId))
            {
                var request = new RestRequest($"/{deleteId}", Method.DELETE);

                // Include the access token in the request headers
                IncludeAuthToken(request);

                IRestResponse response = client.Execute(request);
                MessageBox.Show(response.Content);
                bookidValue.Clear();
                titleValue.Clear();
                priceValue.Clear();
                publishedValue.Clear();
            }
            else
            {
                MessageBox.Show("Invalid book ID for delete.");
            }
        }

        private bool IsValidDateFormat(string date)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(date, @"^\d{4}-\d{2}-\d{2}$"))
            {
                int year = int.Parse(date.Substring(0, 4));
                int month = int.Parse(date.Substring(5, 2));
                int day = int.Parse(date.Substring(8, 2));

                if (year <= 2023 && month >= 1 && month <= 12 && day >= 1 && day <= 31)
                {
                    return true;
                }
            }

            return false;
        }

        private void goLoginBtn_Click(object sender, EventArgs e)
        {
            Login login = new Login(instance, URL);
            login.Show();
        }
    }

    public class Book
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string price { get; set; }
        public string published { get; set; }
    }
}
