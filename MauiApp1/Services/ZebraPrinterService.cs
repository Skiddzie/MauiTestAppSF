using System;
using System.Threading.Tasks;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;

//doesn't fucking work for maccatalyst
//remove this from maccatalyst within your .csproj or else it'll fail to build and the build errors will tell you NOTHING

namespace MauiApp1.Services
{
    public class ZebraPrinterService
    {
        private string _printerAddress;

        public ZebraPrinterService(string printerAddress)
        {
            _printerAddress = printerAddress;
        }

        public async Task PrintZplAsync(string zplString)
        {
            Connection connection = null;

            try
            {
                connection = ConnectionBuilder.Build("TCP:" + _printerAddress);
                connection.Open();

                ZebraPrinter printer = ZebraPrinterFactory.GetInstance(connection);

                printer.SendCommand(zplString);
            }
            catch (ConnectionException ex)
            {
                Console.WriteLine("Connection Exception: " + ex.Message);
            }
            catch (ZebraPrinterLanguageUnknownException ex)
            {
                Console.WriteLine("Zebra Printer Language Unknown Exception: " + ex.Message);
            }
            finally
            {
                if (connection != null && connection.Connected)
                {
                    connection.Close();
                }
            }
        }
    }
}
