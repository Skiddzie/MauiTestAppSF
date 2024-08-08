namespace MauiApp1;

using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;
using MauiApp1.Services;
using Microsoft.Maui.Controls;
using System;
using System.Diagnostics;

public partial class AccountDisplayPage : ContentPage
{
    private ZebraPrinterService _printerService;

    public AccountDisplayPage()
    {
        InitializeComponent();
        _printerService = new ZebraPrinterService("192.168.1.122");

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

    public async void OnPrintButtonClicked(object sender, EventArgs e)
    {
        Debug.WriteLine("print button running");

        string accountId = Preferences.Get("AccountId", string.Empty);
        string accountName = Preferences.Get("AccountName", string.Empty);
        string billingStreet = Preferences.Get("AccountBillingStreet", string.Empty);
        string billingCity = Preferences.Get("AccountBillingCity", string.Empty);
        string billingState = Preferences.Get("AccountBillingState", string.Empty);
        string billingPostalCode = Preferences.Get("AccountBillingPostalCode", string.Empty);
        string billingCountry = Preferences.Get("AccountBillingCountry", string.Empty);

        // Construct ZPL string
        string zpl = "^XA" +
                     $"^FO0,50^ADN,36,20^FDID: {accountId}^FS" +
                     $"^FO0,100^ADN,36,20^FDName: {accountName}^FS" +
                     $"^FO0,150^ADN,36,20^FDStreet: {billingStreet}^FS" +
                     $"^FO0,200^ADN,36,20^FDCity: {billingCity}^FS" +
                     $"^FO0,250^ADN,36,20^FDState: {billingState}^FS" +
                     $"^FO0,300^ADN,36,20^FDPostal Code: {billingPostalCode}^FS" +
                     $"^FO0,350^ADN,36,20^FDCountry: {billingCountry}^FS" +
                     "^XZ";

        await _printerService.PrintZplAsync(zpl);
    }
}
