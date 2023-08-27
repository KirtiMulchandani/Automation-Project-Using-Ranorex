/*
 * Created by Ranorex
 * User: kirti.mulchandani
 * Date: 13-07-2023
 * Time: 15:41
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;

namespace AmazonProject4
{
    /// <summary>
    /// Description of UserCodeModule1.
    /// </summary>
    [TestModule("82E24633-C1D8-4F1A-94F6-A61BE944E01D", ModuleType.UserCode, 1)]
    public class UserCodeModule1 : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public UserCodeModule1()
        {
            // Do not delete - a parameterless constructor is required!
        }

string _SearchItem = "";
[TestVariable("bbcab168-7710-48bb-8aef-c43a8b4a25c2")]
public string SearchItem
{
	get { return _SearchItem; }
	set { _SearchItem = value; }
}

        /// <summary>
        /// Performs the playback of actions in this module.
        /// </summary>
        /// <remarks>You should not call this method directly, instead pass the module
        /// instance to the <see cref="TestModuleRunner.Run(ITestModule)"/> method
        /// that will in turn invoke this method.</remarks>
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
            
            Host.Local.OpenBrowser("https://www.amazon.in/"); // Launching the browser
            var repo = AmazonProject4Repository.Instance; // Creating instance of the repository 
            
            repo.ApplicationUnderTest.SelfInfo.WaitForExists(10000);  // Validating whether the browser has been launched or not
            
            repo.ApplicationUnderTest.Twotabsearchtextbox.PressKeys(SearchItem); // Passing the item to be searched through module variable called "SearchItem"
            
            repo.ApplicationUnderTest.NavSearchSubmitButton.Click(); // Tapping on the search button
			Thread.Sleep(3000); // Waiting for results to come
         	  
            IList<DivTag> listOfResults = Host.Local.Find<DivTag>("/dom[@domain='www.amazon.in']//div[@id='search']/div[1]/div[1]//div[@data-component-type='s-search-result']"); // Extracting the list of the results appeared, through XPath
            
            Thread.Sleep(3000); 


            Validate.IsTrue(listOfResults.Count > 0);  // Validating that Isn't the list empty
            Report.Log(ReportLevel.Success, "Length of the list of results appeared is: " + listOfResults.Count.ToString()); // Logging the length of the list in the report

            // Scrolling down the page in order to add the desired item in the cart
			Keyboard.Press("{PageDown}");
			Thread.Sleep(2000);
			Keyboard.Press("{PageDown}");
			Thread.Sleep(2000);
			
			Mouse.Click(listOfResults[2].FindSingle(".//a")); // Tapping on the product
	

            var addToCartButton = repo.ApplicationUnderTest.AddToCartButton;
            addToCartButton.Click();  // Adding it into the Cart
           

			var addedToCart = repo.ApplicationUnderTest.AddedToCart;
			Validate.IsTrue(addedToCart.InnerText.Equals("Added to Cart"), "Product has been added to cart successfully!!"); // Validating whether the item has been added in the cart or not
			
			
			var attachCloseSideSheetLink = repo.ApplicationUnderTest.AttachCloseSideSheetLink;
			attachCloseSideSheetLink.Click();
		

			Host.Local.CloseBrowser("chrome"); // Closing the browser


        }
    }
}
