using System.Diagnostics;
using Microsoft.Maui.Controls;
namespace MauiApp1
{

    public partial class MainPage : ContentPage
    {
        int count = 0;
        private static readonly string CONSUMER_KEY = "3MVG9XgkMlifdwVAyVXTr89KTncOGuqMZWc.WmpVEE_eDrxkW3VJnBJVmMjBlMz38LTySY2tEaw_UinU33_6a";
        private static readonly string CONSUMER_SECRET = "55D075BC603EAA2C5DB26215A571318B0E5220D3F4CFF97C2BAD8299BA94110F";
        private static readonly string DOMAIN_NAME = "https://login.salesforce.com/services/oauth2/token";

        public MainPage()
        {
            InitializeComponent();

            Debug.WriteLine("MainPage has been initialized.");
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;
            Debug.WriteLine("you clicked that shit");
            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";
            
            SemanticScreenReader.Announce(CounterBtn.Text);


        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var response = await GetAccessTokenAsync(
                    UserEntryBox.Text,
                    PasswordEntryBox.Text
                );

                Debug.WriteLine(response);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
        }
        private async Task<string> GetAccessTokenAsync(string username, string password)
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    //amchendry@empathetic-goat-7i1jmi.com
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_id", CONSUMER_KEY),
                    new KeyValuePair<string, string>("client_secret", CONSUMER_SECRET),
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password)
                });

                var response = await client.PostAsync(DOMAIN_NAME, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    InfoText.Text = "Invalid Login Details";
                    InfoText.TextColor = Colors.Red;
                    throw new Exception($"HTTP Status Code: {response.StatusCode}. Reason: {response.ReasonPhrase}. Response: {responseContent}");
                }
                InfoText.Text = "Success";
                InfoText.TextColor = Colors.Green;
                StoreAccessToken(ExtractAccessToken(responseContent));
                Debug.WriteLine("access token: " + RetrieveAccessToken() + " my access token");
                return responseContent;
            }
        }
        private string ExtractAccessToken(string jsonResponse)
        {
            // Assuming the jsonResponse is in the format {"access_token":"...", ...}
            var jsonObject = System.Text.Json.JsonDocument.Parse(jsonResponse).RootElement;
            return jsonObject.GetProperty("access_token").GetString();
        }

        private void StoreAccessToken(string accessToken)
        {
            Preferences.Set("AccessToken", accessToken);
        }

        private string RetrieveAccessToken()
        {
            Debug.WriteLine("revriveaccesstoken called");
            return Preferences.Get("AccessToken", string.Empty);
        }


    }

}



