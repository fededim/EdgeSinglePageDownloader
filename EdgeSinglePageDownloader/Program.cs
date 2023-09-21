using OpenQA.Selenium.Edge;
using IWshRuntimeLibrary;


namespace EdgeChromeSinglePageDownloader
{
    public class Program
    {
        const int MaxWaitForSaveMSec = 60 * 1000;  // 1 minute
        const string DefaultSaveFolder = "c:\\temp";

        static private EdgeDriver Driver { get; set; }
        static private WshShellClass WshShell { get; set; }


        static void SaveAsSingleFile(string filename, string url)
        {
        again:
            if (!Directory.Exists(Path.GetDirectoryName(filename)))
                Directory.CreateDirectory(Path.GetDirectoryName(filename));

            if (System.IO.File.Exists(filename))
            {
                // simple check that the existing file format is mhtml, otherwise delete and re-save it
                if (!System.IO.File.ReadAllText(filename).Contains($"Snapshot-Content-Location: {url}"))
                    System.IO.File.Delete(filename);
                else
                    return;
            }

            WshShell.SendKeys("^s");
            Thread.Sleep(1000);
            // send alt+n, enter filename
            WshShell.SendKeys($"%n{filename}");
            Thread.Sleep(20);
            // send alt+t, down arrow, up arrow (to select single mhtml), press enter twice
            WshShell.SendKeys($"%t");
            Thread.Sleep(20);
            WshShell.SendKeys($"{{DOWN}}");
            Thread.Sleep(20);
            WshShell.SendKeys($"{{UP}}");
            Thread.Sleep(20);
            WshShell.SendKeys($"~~");


            // waits up to MaxWaitForSaveMSec to check that the file is saved correctly
            var endtime = DateTime.Now.AddMilliseconds(MaxWaitForSaveMSec);

            while (DateTime.Now < endtime)
            {
                Thread.Sleep(1000);
                // simple check that the file is present and its format is mhtml, otherwise retry again to save
                if (System.IO.File.Exists(filename))
                {
                    if (!System.IO.File.ReadAllText(filename).Contains($"Snapshot-Content-Location: {url}"))
                        goto again;
                    else
                        break;
                }
            }
        }


        static void Main(string[] args)
        {
            var options = new EdgeOptions();

            var service = EdgeDriverService.CreateDefaultService();
            service.EnableVerboseLogging = true;

            WshShell = new WshShellClass();

            var urlsToSave = new List<string> { "https://www.theverge.com", "https://www.wired.com/" };

            var i = 1;
            foreach (var url in urlsToSave)
            {
                Driver = new EdgeDriver(service, options);

                Driver.Navigate().GoToUrl(url);

                SaveAsSingleFile(Path.Combine(DefaultSaveFolder, $"Page_{i++}.mhtml"),url);

                Driver.Close();
            }
        }
    }
}