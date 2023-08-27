using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;


namespace LoginTests
{   
    [TestFixture]
    public class Program
    {
        [Test]    
        static async Task Main(string[] args)
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            await page.GotoAsync("http://georgiapapadopoulou.eu.185-25-20-64.linux11.name-servers.gr/login.html");
            await LoginTestValidCredentials(page);
            await LoginTestWithEmptyEmailField(page);
            await LoginTestWithEmptyPasswordField(page);
            await browser.CloseAsync();
        }

        [Test]
        static async Task LoginTestValidCredentials(IPage page)
        {
            await page.FillAsync("#inputEmail", "your-email@example.com");
            await page.FillAsync("#inputPassword", "your-password");
            await page.ClickAsync("#signInButton");
            await Task.Delay(5000);
            await page.WaitForSelectorAsync("#successAlert");
            var successMessageAlert = await page.WaitForSelectorAsync("#successAlert");
            var h4Element = await successMessageAlert.QuerySelectorAsync("h4");
            if (h4Element != null)
            {
                var h4Text = await h4Element.InnerTextAsync();
                Console.WriteLine("Success Alert H4: " + h4Text);
            }

            var pElement = await successMessageAlert.QuerySelectorAsync("p");
            if (pElement != null)
            {
                var pText = await pElement.InnerTextAsync();
                Console.WriteLine("Success Alert P: " + pText);
            }
            await page.FillAsync("#inputEmail", "");
            await page.FillAsync("#inputPassword", "");
            await page.ReloadAsync();
        }

        [Test]
        static async Task LoginTestWithEmptyEmailField(IPage page)
        {
            await page.FillAsync("#inputPassword", "your-password");
            await page.ClickAsync("#signInButton");
            await page.WaitForSelectorAsync("#emailError");
            var validationErrorEmail = await page.QuerySelectorAsync("#emailError");
            if (validationErrorEmail != null)
            {
                var errorMessage = await validationErrorEmail.InnerTextAsync();
                Console.WriteLine("Validation Error: " + errorMessage);
            }
            await page.FillAsync("#inputEmail", "");
            await page.FillAsync("#inputPassword", "");
            await page.ReloadAsync();
        }

        [Test]
        static async Task LoginTestWithEmptyPasswordField(IPage page)
        {
          await page.FillAsync("#inputEmail", "email@example.com");
            await page.ClickAsync("#signInButton");
            await page.WaitForSelectorAsync("#passwordError");
            var validationErrorPassword = await page.QuerySelectorAsync("#passwordError");
            if (validationErrorPassword != null)
            {
                var errorMessage = await validationErrorPassword.InnerTextAsync();
                Console.WriteLine("Validation Error: " + errorMessage);
            }
            await page.FillAsync("#inputEmail", "");
            await page.FillAsync("#inputPassword", "");
            await page.ReloadAsync();  
        }
    }
}
