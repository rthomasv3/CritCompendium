using System.Windows;
using System.Windows.Controls;

namespace CritCompendium
{
	public class AttachedProperties
	{
		public static string GetBindableLineCount(DependencyObject obj)
		{
			return (string)obj.GetValue(BindableLineCountProperty);
		}

		public static void SetBindableLineCount(DependencyObject obj, string value)
		{
			obj.SetValue(BindableLineCountProperty, value);
		}

		public static readonly DependencyProperty BindableLineCountProperty =
			 DependencyProperty.RegisterAttached(
			 "BindableLineCount",
			 typeof(string),
			 typeof(AttachedProperties),
			 new UIPropertyMetadata("1"));

		public static bool GetHasBindableLineCount(DependencyObject obj)
		{
			return (bool)obj.GetValue(HasBindableLineCountProperty);
		}

		public static void SetHasBindableLineCount(DependencyObject obj, bool value)
		{
			obj.SetValue(HasBindableLineCountProperty, value);
		}

		public static readonly DependencyProperty HasBindableLineCountProperty =
			 DependencyProperty.RegisterAttached(
			 "HasBindableLineCount",
			 typeof(bool),
			 typeof(AttachedProperties),
			 new UIPropertyMetadata(
				  false,
				  new PropertyChangedCallback(OnHasBindableLineCountChanged)));

		private static void OnHasBindableLineCountChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var textBox = (TextBox)o;
			if ((e.NewValue as bool?) == true)
			{
				textBox.SizeChanged += new SizeChangedEventHandler(box_SizeChanged);
				textBox.TextChanged += TextBox_TextChanged;
				textBox.SetValue(BindableLineCountProperty, textBox.LineCount.ToString());
			}
			else
			{
				textBox.SizeChanged -= new SizeChangedEventHandler(box_SizeChanged);
				textBox.TextChanged -= TextBox_TextChanged;
			}
		}

		private static void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			UpdateSize(sender as TextBox);
		}

		private static void box_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			UpdateSize(sender as TextBox);
		}

		private static void UpdateSize(TextBox textBox)
		{
			string x = string.Empty;
			for (int i = 0; i < textBox.LineCount; i++)
			{
				x += i + 1 + "\n";
			}
			textBox.SetValue(BindableLineCountProperty, x);
		}
	}
}
