using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiApp1
{
    public partial class QueryPage : ContentPage
    {
        public QueryPage()
        {
            InitializeComponent();
        }

        public string RetrieveAccessToken()
        {
            Debug.WriteLine("revriveaccesstoken called");
            return Preferences.Get("AccessToken", string.Empty);
        }

        public string RetrieveURL() 
        {
            return Preferences.Get("URL", string.Empty);
        }

        private async void OnQueryButtonClicked(object sender, EventArgs e)
        {
            string lastName = NameEntryBox.Text;
            string accessToken = RetrieveAccessToken();

            await AccountQuery(lastName);

                        
        }
        //curl "https://empathetic-goat-7i1jmi-dev-ed.trailblaze.my.salesforce.com/services/data/v61.0/query/?q=SELECT+Id,+Name,+BillingStreet,+BillingCity,+BillingState,+BillingPostalCode,+BillingCountry+FROM+Account+WHERE+Name+LIKE+%27%25McHendry%25%27" -H "Authorization: Bearer 00Daj00000AYtbY!AQEAQNgp.zMPTzSBo0pO6F2uXeyoGQM12.upyBxAfeKUeorwjaeWFe_oI7ffUSagGYbNf5v7JT5wilkjCEqoQDoEMOr57gpY" -H "X-PrettyPrint:1"
        //amchendry@empathetic-goat-7i1jmi.com
        public async Task AccountQuery(string lastName)
        {
            using (var client = new HttpClient())
            {
                Debug.WriteLine(RetrieveAccessToken());

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", RetrieveAccessToken());
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                string query = $"SELECT+Id,+Name,+BillingStreet,+BillingCity,+BillingState,+BillingPostalCode,+BillingCountry+FROM+Account+WHERE+Name+LIKE+%27%25{lastName}%25%27";

                string requestUrl = $"{RetrieveURL()}/services/data/v61.0/query?q={query}";

                Debug.WriteLine(requestUrl);
                Debug.WriteLine(query);

                try
                {
                    var response = await client.GetAsync(requestUrl);
                    Debug.WriteLine("Response received");

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine($"Error: {response.StatusCode}");
                        Debug.WriteLine($"Details: {errorResponse}");
                    }
                    else
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var jsonResponse = JsonDocument.Parse(responseBody);

                        Debug.WriteLine("Response JSON: " + JsonSerializer.Serialize(jsonResponse, new JsonSerializerOptions { WriteIndented = true }));
                        var records = jsonResponse.RootElement.GetProperty("records");
                        if (records.GetArrayLength() > 0)
                        {
                            var firstRecord = records[0];
                            string id = firstRecord.GetProperty("Id").GetString();
                            Preferences.Set("AccountId", id);
                            string name = firstRecord.GetProperty("Name").GetString();
                            Preferences.Set("AccountName", name);
                            string billingStreet = firstRecord.GetProperty("BillingStreet").GetString();
                            Preferences.Set("AccountBillingStreet", billingStreet);
                            string billingCity = firstRecord.GetProperty("BillingCity").GetString();
                            Preferences.Set("AccountBillingCity", billingCity);
                            string billingState = firstRecord.GetProperty("BillingState").GetString();
                            Preferences.Set("AccountBillingState", billingState);
                            string billingPostalCode = firstRecord.GetProperty("BillingPostalCode").GetString();
                            Preferences.Set("AccountBillingPostalCode", billingPostalCode);
                            string billingCountry = firstRecord.GetProperty("BillingCountry").GetString();
                            Preferences.Set("AccountBillingCountry", billingCountry);

                            Debug.WriteLine($"Id: {id}, Name: {name}, BillingStreet: {billingStreet}, BillingCity: {billingCity}, BillingState: {billingState}, BillingPostalCode: {billingPostalCode}, BillingCountry: {billingCountry}");

                            await Navigation.PushAsync(new AccountDisplayPage());
                        }
                        else
                        {
                            Debug.WriteLine("No records found.");
                        };
                    }
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine($"Request error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Unexpected error: {ex.Message}");
                }
            }
        }

        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            Preferences.Set("AccessToken", null);
            await Navigation.PushAsync(new MainPage());
        }
    }
}
