using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Geo.View
{
    public partial class RedSetPivotPage : PhoneApplicationPage
    {
        public RedSetPivotPage()
        {
            InitializeComponent();

            ///////////init Colors Sources
            var colors = new List<ColorSelectModel>();
            var accentColor = (Color)Resources["PhoneAccentColor"];
            colors.Add(new ColorSelectModel("Accent Color", accentColor));
            colors.AddRange(
                    typeof(Colors).GetProperties(BindingFlags.Static | BindingFlags.Public)
                        .Where(p => p.PropertyType == typeof(Color))
                        .Where(p => p.Name != "Transperant")
                        .Select(p => new ColorSelectModel(p.Name, (Color)p.GetValue(typeof(Colors), null))));

            List<int> lstSizes = new List<int>() {2, 3, 4, 5, 7, 10, 12, 15, 17, 20, 25, 30, 35, 40,  45, 50 };
            List<int> lstThikness = new List<int>() { 2, 3, 4, 5, 7, 10, 12, 15, 17, 20, 25};
            List<string> lstPointStyle = new List<string>() { "Circle", "Cross", "Diamond", "Square", "Triangle", ""};
            List<string> lstLineStyle = new List<string>() { "Solid", "Dash", "DashDot", "DashDotDot", "Dot", "" };
            

            lstPkrPointColor.ItemsSource = lstPkrPointColor.ItemsSource ?? new ObservableCollection<ColorSelectModel>(colors);
            lstPkrPointColor.SelectedIndex = 12;
            lstPkrPointSize.ItemsSource = lstSizes;
            lstPkrPointSize.SelectedIndex = 7;
            lstPkrPointStyle.ItemsSource = lstPointStyle;

            lstPkrLineColor.ItemsSource = lstPkrPointColor.ItemsSource ?? new ObservableCollection<ColorSelectModel>(colors);
            lstPkrLineColor.SelectedIndex = 12;
            lstPkrLineWidth.ItemsSource = lstThikness;
            lstPkrLineWidth.SelectedIndex = 3;
            lstPkrLineStyle.ItemsSource = lstLineStyle;

            lstPkrPolygonBorderColor.ItemsSource = lstPkrPolygonBorderColor.ItemsSource ?? new ObservableCollection<ColorSelectModel>(colors);
            lstPkrPolygonBorderColor.SelectedIndex = 12;
            lstPkrPolygonBorderThikness.ItemsSource = lstThikness;
            lstPkrPolygonBorderThikness.SelectedIndex = 1;
            lstPkrPolygonFillColor.ItemsSource = lstPkrPolygonBorderColor.ItemsSource ?? new ObservableCollection<ColorSelectModel>(colors);
            lstPkrPolygonFillColor.SelectedIndex = 12;

            lstPkrTextSize.ItemsSource = lstSizes;
            lstPkrTextSize.SelectedIndex = 7;
            lstPkrTextColor.ItemsSource = lstPkrTextColor.ItemsSource ?? new ObservableCollection<ColorSelectModel>(colors);
            lstPkrTextColor.SelectedIndex = 1;
        }

        private void btnApplyRedSettings_Click_1(object sender, EventArgs e)
        {

        }

        private void btnApplyAllRedSettings_Click_1(object sender, EventArgs e)
        {

        }

       


    }

    public class ColorSelectModel
    {
        public ColorSelectModel(string text, Color color)
        {
            this.Text = text;
            this.Color = color;
            this.ColorBrush = new SolidColorBrush(color);
        }
        public string Text { get; set; }
        public Color Color { get; set; }
        public SolidColorBrush ColorBrush { get; set; }
    }
}