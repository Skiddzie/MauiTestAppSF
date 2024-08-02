using System.Diagnostics;
using System.Net.Http.Headers;

namespace MauiApp1;

public partial class QueryPage : ContentPage
{
	public QueryPage()
	{
		InitializeComponent();
	}
    private async void OnQueryButtonClicked(object sender, EventArgs e)
    {
        string lastName = NameEntryBox.Text;
        string soqlQuery = "SELECT Id, Name, (SELECT LastName FROM Contacts WHERE LastName = '" + lastName + "') FROM Account\r\n"
        try
        {
            

            Debug.WriteLine("f");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
        }
    }

    private async Task<List<Account>> QueryAccountsByLastNameAsync(string lastName, string accessToken)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Assuming you have a custom field LastName__c on Account
            string query = $"SELECT Id, Name FROM Account WHERE LastName__c = '{lastName}'";
            string requestUrl = $"{DOMAIN_NAME}/services/data/{API_VERSION}/query?q={Uri.EscapeDataString(query)}";

            var response = await client.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseBody);

            var records = jsonResponse["records"];
            var accounts = new List<Account>();

            foreach (var record in records)
            {
                accounts.Add(new Account
                {
                    Id = record["Id"].ToString(),
                    Name = record["Name"].ToString()
                });
            }

            return accounts;
        }
    }
}