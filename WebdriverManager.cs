﻿using System;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SaveTheWorldRewards
{
    class WebdriverManager
    {
        public static string GetAuthCode()
        {
            string link = "https://www.epicgames.com/id/api/redirect?clientId=ec684b8c687f479fadea3cb2ad83f5c6&responseType=code";

            var options = new ChromeOptions();
            options.AddArgument("user-data-dir=C:/Users/dhrum/AppData/Local/Google/Chrome/User Data");
            options.AddExcludedArgument("enable-automation");
            options.AddAdditionalCapability("useAutomationExtension", false);

            options.AddArgument("disable-gpu");
            options.AddArgument("disable-software-rasterizer");
            options.AddArgument("no-sandbox");

            options.AddArgument("headless");

            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            chromeDriverService.SuppressInitialDiagnosticInformation = true;

            ChromeDriver driver;

            try
            {
                driver = new ChromeDriver(chromeDriverService, options);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Please close your chrome window.");
                return string.Empty;
            }

            driver.Url = link;

            string text = new WebDriverWait(driver, TimeSpan.FromSeconds(30)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("/html/body/pre"))).Text;
            driver.Close();
            Console.WriteLine("driver closed");

            var codeDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
            if (codeDict.ContainsKey("redirectUrl"))
            {
                try
                {
                    return codeDict["redirectUrl"][43..];
                }
                catch
                {
                    return string.Empty;
                }
            }

            return string.Empty;
        }
    }
}
