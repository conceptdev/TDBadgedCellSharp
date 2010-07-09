using System;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;

namespace TDBadgedCellSharp
{
	/// <summary>
	/// Simple class to bind data to the UITableView
	/// </summary>
	public class Rss
	{
		public string Title;
		public string Detail;
		public string Badge;
	}

	public class TableViewSource : UITableViewSource
	{
		static NSString kCellIdentifier = new NSString ("Cell");
		List<Rss> contents;

		public TableViewSource (List<Rss> data)
		{
			contents = data;
		}
		public override int RowsInSection (UITableView tableview, int section)
		{
			return contents.Count;
		}
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			TDBadgedCell cell = new TDBadgedCell (UITableViewCellStyle.Subtitle, kCellIdentifier);
			cell.TextLabel.Text = contents[indexPath.Row].Title;
			cell.TextLabel.Font = UIFont.BoldSystemFontOfSize (14);
			
			cell.DetailTextLabel.Text = contents[indexPath.Row].Detail;
			cell.DetailTextLabel.Font = UIFont.SystemFontOfSize (13);
			
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			cell.BadgeNumber = Convert.ToInt32 (contents[indexPath.Row].Badge);
			
			if (indexPath.Row == 1)
				cell.BadgeColor = UIColor.FromRGBA (1.000f, 0.397f, 0.419f, 1.000f);
			if (indexPath.Row == 2)
				cell.BadgeColor = UIColor.FromWhiteAlpha (0.783f, 1.000f);
			
			return cell;
		}
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow (indexPath, true);
		}
	}
}