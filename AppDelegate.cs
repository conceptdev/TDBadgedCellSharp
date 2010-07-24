using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace TDBadgedCellSharp
{
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;
		UINavigationController navigationController;
		UITableViewController tableViewController;

		List<Rss> contents = new List<Rss> ();

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			// Create some sample data
			contents.Add (new Rss { Title = "TUAW", Detail = "The Unofficial Apple Weblog", Badge = "17" });
			contents.Add (new Rss { Title = "High Caffine Content", Detail = "Steven Troughton Smith", Badge = "2" });
			contents.Add (new Rss { Title = "Smoking Apples", Detail = "Blog about Apple Software...", Badge = "145" });
			contents.Add (new Rss { Title = "Daring Fireball", Detail = "The musings of John Gruber", Badge = "0" });
			contents.Add (new Rss { Title = "tmdvs.me", Detail = "Long detail text to test update by tonymillion on github", Badge = "2345" });

			// Build a tableview
			tableViewController = new UITableViewController ();
			tableViewController.TableView.Frame = new RectangleF (0, 20, this.window.Frame.Width, this.window.Frame.Height - 20);
			tableViewController.TableView.Source = new TableViewSource (contents);		
			tableViewController.Title = "Tim's RSS Reader";
			tableViewController.NavigationItem.RightBarButtonItem = tableViewController.EditButtonItem;

			// Add to the navigation controller
			navigationController = new UINavigationController (tableViewController);

			// Add to the window, and show
			window.AddSubview (navigationController.View);
			window.MakeKeyAndVisible ();
			return true;
		}

		// This method is required in iPhoneOS 3.0
		public override void OnActivated (UIApplication application)
		{
		}
	}
}