# EdgeSinglePageDownloader

EdgeSinglePageDownloader is a simple console application which downloads a set of webpages and saves them as single .MHTML files in a specified folder using Edge Selenium Webdriver (it should work also on ChromeDriver). It is a simple proof of concept which shows how to implement this feature using the Selenium Webdriver and allows also to download and save a set of webpages in batch.

# Background

The Edge Selenium Webdriver is a NuGet package which allows you to automate Microsoft Edge by simulating user interaction. The saving of the browsed web page happens by sending CTRL + S key to Edge in order to pop up the "Save As" dialog, specifying then a filename, selecting the file format (Webpage, single file .mhtml) and clicking the "Save" button. Unluckily the method SendKeys provided by Selenium Webdriver does not work (or at least I was not able to in Windows), so after several tries I switched to using VBScript SendKeys method which works flawlessly with the caveat of requiring Windows as operating system.

# CodeProject article

[Here](https://www.codeproject.com/Tips/5368776/How-to-Automate-Saving-Webpages-as-a-Single-MHTML)
