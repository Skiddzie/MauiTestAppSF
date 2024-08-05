namespace MauiApp1;

public partial class AccountDisplayPage : ContentPage
{
	public AccountDisplayPage()
	{
		InitializeComponent();

        AccountId.Text = "ID: " + Preferences.Get("AccountId", string.Empty);
        AccountName.Text = Preferences.Get("AccountName", string.Empty);
        AccountBillingStreet.Text = Preferences.Get("AccountBillingStreet", string.Empty);
        AccountBillingCity.Text = Preferences.Get("AccountBillingCity", string.Empty);
        AccountBillingState.Text = Preferences.Get("AccountBillingState", string.Empty);
        AccountBillingPostalCode.Text = Preferences.Get("AccountBillingPostalCode", string.Empty);
        AccountBillingCountry.Text = Preferences.Get("AccountBillingCountry", string.Empty);
    }

    public async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new QueryPage());
    }
}