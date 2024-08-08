#if __ANDROID__ || __IOS__
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;
using Zebra.Sdk.Settings;
#endif
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MauiApp1.Services
{
    public class ZebraPrinterService
    {
        private string _printerAddressOrBluetoothMac;

        public ZebraPrinterService(string addressOrMac)
        {
            _printerAddressOrBluetoothMac = addressOrMac;
        }

        public async Task PrintZplAsync(string zplString)
        {
#if __ANDROID__ || __IOS__
            Connection connection = null;

            try
            {
                Debug.WriteLine($"Attempting to connect to {_printerAddressOrBluetoothMac}");

                // Check if the address is a Bluetooth MAC address
                if (IsBluetoothAddress(_printerAddressOrBluetoothMac))
                {
                    // Establish a Bluetooth connection
                    connection = ConnectionBuilder.Build("BT:" + _printerAddressOrBluetoothMac);
                }
                else
                {
                    // Establish a TCP connection
                    connection = ConnectionBuilder.Build("TCP:" + _printerAddressOrBluetoothMac);
                }

                connection.Open();

                ZebraPrinter printer = ZebraPrinterFactory.GetInstance(connection);

                printer.SendCommand(zplString);

                Debug.WriteLine("Print command sent successfully");
            }
            catch (ConnectionException ex)
            {
                Debug.WriteLine($"Connection Exception: {ex.Message}");
            }
            catch (ZebraPrinterLanguageUnknownException ex)
            {
                Debug.WriteLine($"Zebra Printer Language Unknown Exception: {ex.Message}");
            }
            finally
            {
                if (connection != null && connection.Connected)
                {
                    connection.Close();
                }
            }
#else
            throw new PlatformNotSupportedException("Bluetooth printing is not supported on this platform.");
#endif
        }

#if __ANDROID__ || __IOS__
        private bool IsBluetoothAddress(string address)
        {
            // Simple check for Bluetooth MAC address format (e.g., 00:11:22:33:44:55)
            return Regex.IsMatch(address, "^([0-9A-Fa-f]{2}:){5}[0-9A-Fa-f]{2}$");
        }
#endif
    }
}
