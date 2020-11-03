using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;
using OpenQA.Selenium.Support.UI;

namespace ozon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //на случай повторного запуска
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            //стартуем хром
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            IWebDriver driver = new ChromeDriver(driverService, new ChromeOptions());
            //идем на озон
            driver.Navigate().GoToUrl("https://www.ozon.ru/");

            driver.FindElement(By.XPath("//*[text()='Каталог']")).Click(); //кликаем на каталог

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement Element_Smartphones = wait.Until(d => driver.FindElement(By.XPath("//*[text()='Смартфоны']"))); //ждем пока все загрузится
            Element_Smartphones.Click(); //и нажмем на телефоны
            IWebElement Element_ShowAll = wait.Until(d => driver.FindElement(By.ClassName("show")));
            Element_ShowAll.Click(); //посмотреть все
            IWebElement Element_BrandsPanel = Element_ShowAll.FindElement(By.XPath("parent::*"));

            IWebElement Element_ModelsPanel = driver.FindElement(By.XPath("//div[contains(text(), 'Модель')]")).FindElement(By.XPath("parent::*"));
            Element_ModelsPanel.FindElement(By.ClassName("show")).Click(); //посмотреть все

            Thread.Sleep(2000);
            IReadOnlyList<IWebElement> Elements_Brands = Element_BrandsPanel.FindElements(By.TagName("span"));
            foreach (var item in Elements_Brands) //заполняем список брендов
            {
                if (item.Text != "Свернуть")
                    listBox1.Items.Add(item.Text);
            }

            
            IReadOnlyList<IWebElement> Elements_Models = Element_ModelsPanel.FindElements(By.TagName("span"));
            foreach (var item in Elements_Models) //заполняем список моделей
            {
                if (item.Text != "Свернуть" && item.FindElements(By.TagName("span")).Count != 0)
                    listBox2.Items.Add(item.FindElement(By.TagName("span")).Text);
            }
            
            driver.Close(); //все...
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
