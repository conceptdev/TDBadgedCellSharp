//
//  TDBadgedCell.m
//  TDBadgedTableCell
//	TDBageView
//
//	Any rereleasing of this code is prohibited.
//	Please attribute use of this code within your application
//
//	Any Queries should be directed to hi@tmdvs.me | http://www.tmdvs.me
//	
//  Created by Tim on [Dec 30].
//  Copyright 2009 Tim Davies. All rights reserved.
//
using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
/*
Github
http://github.com/tmdvs/TDBadgedCell/
Blog
http://www.tuaw.com/2010/01/07/iphone-devsugar-simple-table-badges/
*/
namespace TDBadgedCellSharp
{
	public class TDBadgeView : UIView
	{
		public int Width {get;set;}
		public int BadgeNumber {get;set;}

		SizeF numberSize;
		UIFont font;
		string countString;
		public UITableViewCell Parent {get;set;}

		public UIColor BadgeColor {get;set;}
		public UIColor BadgeColorHighlighted {get;set;}

		public TDBadgeView (RectangleF frame) : base (frame)
		{
			font = UIFont.BoldSystemFontOfSize(14f);
			this.BackgroundColor = UIColor.Clear;
		}
		public override void Draw (RectangleF rect)
		{
			countString = this.BadgeNumber.ToString();
			NSString ns = new NSString(countString);
			numberSize = ns.StringSize (font);

			Width = Convert.ToInt32(numberSize.Width + 16);
			
			RectangleF bounds = new RectangleF(0, 0, numberSize.Width + 16, 18);

			var context = UIGraphics.GetCurrentContext();
			
			float radius = bounds.Size.Height / 2.0f;

			context.SaveState();

			if (Parent.Highlighted || Parent.Selected)
			{
				UIColor col;

				if (this.BadgeColorHighlighted != null)
					col = this.BadgeColorHighlighted;
				else
					col = UIColor.FromRGBA (1.0f, 1.0f, 1.0f, 1.000f);

				context.SetFillColorWithColor (col.CGColor);
			}
			else
			{
				UIColor col;
				if (this.BadgeColor != null)
					col = this.BadgeColor;
				else 
					col = UIColor.FromRGBA(0.530f, 0.600f, 0.738f, 1.000f);

				context.SetFillColorWithColor (col.CGColor);
			}

			context.BeginPath();
			float a = Convert.ToSingle(Math.PI / 2f);
			float b = Convert.ToSingle(3f * Math.PI / 2f);
			context.AddArc(radius, radius, radius, a, b, false);
			context.AddArc(bounds.Size.Width - radius, radius, radius, b, a, false);
			context.ClosePath();
			context.FillPath();
			context.RestoreState();

			bounds.X = (bounds.Size.Width - numberSize.Width) / 2 + 0.5f;
			
			context.SetBlendMode( CGBlendMode.Clear);
			
			this.DrawString(countString, bounds, font);
		}
	}

	public class TDBadgedCell : UITableViewCell
	{
		public int BadgeNumber {get;set;}
		public TDBadgeView Badge {get;set;}
		public UIColor BadgeColor {get;set;}
		public UIColor BadgeColorHighlighted {get;set;}

		public TDBadgedCell (UITableViewCellStyle style, string reuseIdentifier) : base (style, reuseIdentifier)
		{
			Badge = new TDBadgeView(RectangleF.Empty);
			Badge.Parent = this;
			
			//redraw cells in accordance to accessory
			float version = float.Parse(UIDevice.CurrentDevice.SystemVersion);

			if (version <= 3.0)
				this.AddSubview(this.Badge);
			else
				this.ContentView.AddSubview(this.Badge);
		} 

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews();
			if (this.BadgeNumber > 0)
			{
				//force badges to hide on edit.
				if (this.Editing)
					this.Badge.Hidden = true;
				else
					this.Badge.Hidden = false;
	
				NSString ns = new NSString(BadgeNumber.ToString());
				SizeF badgeSize = ns.StringSize(UIFont.BoldSystemFontOfSize(14));
	
				float version = float.Parse(UIDevice.CurrentDevice.SystemVersion);
	
				RectangleF badgeFrame;

				if (version <= 3.0)
				{
					badgeFrame = new RectangleF(this.ContentView.Frame.Size.Width - (badgeSize.Width+16) - 10 // not sure why this wasn't -10 in the objective-c version...?
						, Convert.ToSingle(Math.Round((this.ContentView.Frame.Size.Height - 18) /2))
						, badgeSize.Width + 16f
						, 18f);
				}
				else
				{
					badgeFrame = new RectangleF(this.ContentView.Frame.Size.Width - (badgeSize.Width+16) - 10
						, Convert.ToSingle(Math.Round((this.ContentView.Frame.Size.Height - 18) /2))
						, badgeSize.Width + 16f
						, 18f);
				}
	
				this.Badge.Frame = badgeFrame;
				Badge.BadgeNumber = this.BadgeNumber;
				Badge.Parent = this;
	
				if (this.TextLabel.Frame.X + this.TextLabel.Frame.Size.Width >= badgeFrame.X)
				{
					float badgeWidth = Convert.ToSingle(this.TextLabel.Frame.Size.Width - badgeFrame.Size.Width - 10.0);
					
					this.TextLabel.Frame = new RectangleF(this.TextLabel.Frame.X
						, this.TextLabel.Frame.Y
						, badgeWidth
						, this.TextLabel.Frame.Size.Height);
				}
				
				// http://github.com/tmdvs/TDBadgedCell/commit/4c3c09e0adef7d90b6dbba82b5f35b91f16e8dd2
				if ((DetailTextLabel.Frame.X + DetailTextLabel.Frame.Size.Width) >= badgeFrame.X)
				{
					float badgeWidth = Convert.ToSingle(this.DetailTextLabel.Frame.Size.Width - badgeFrame.Size.Width - 10);
					DetailTextLabel.Frame = new RectangleF(this.DetailTextLabel.Frame.X
						, this.DetailTextLabel.Frame.Y
						, badgeWidth
						, this.DetailTextLabel.Frame.Size.Height);
				}

				//set badge hightlighed colours or use defaults
				if (this.BadgeColorHighlighted != null)
					Badge.BadgeColorHighlighted = this.BadgeColorHighlighted;
				else
					Badge.BadgeColorHighlighted = UIColor.FromRGBA(1.0f, 1.0f, 1.0f, 1.000f);
				
				if (this.BadgeColor != null)
					Badge.BadgeColor = this.BadgeColor;
				else
					Badge.BadgeColor = UIColor.FromRGBA(0.530f, 0.600f, 0.738f, 1.000f);
			}
			else
			{
				this.Badge.Hidden = true;
			}
		}
		public override void SetHighlighted (bool highlighted, bool animated)
		{
			base.SetHighlighted (highlighted, animated);
			Badge.SetNeedsDisplay();
		}
		public override void SetSelected (bool selected, bool animated)
		{
			base.SetSelected (selected, animated);
			Badge.SetNeedsDisplay();
		}
		public override void SetEditing (bool editing, bool animated)
		{
			base.SetEditing (editing, animated);
			if (editing)
			{
				Badge.Hidden = true;
				Badge.SetNeedsDisplay();
				this.SetNeedsDisplay();
			}
			else
			{
				Badge.Hidden = false;
				Badge.SetNeedsDisplay();
				this.SetNeedsDisplay();
			}
		}
	}
}