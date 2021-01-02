using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfTotalnik
{
    public static class MyGridView
    {
        public static GridView CreateGridView()
        {
            var factory = new FrameworkElementFactory(typeof(System.Windows.Controls.Image));
            factory.SetValue(System.Windows.Controls.Image.SourceProperty, new Binding(nameof(MyViewItem.MyIcon)));
            factory.SetValue(System.Windows.Controls.Image.HeightProperty, 30.0);
            var dataTemplate = new DataTemplate { VisualTree = factory };

            GridView grView = new GridView();
            grView.Columns.Add(new GridViewColumn { Header = "Icon", Width = 30, CellTemplate = dataTemplate });
            grView.Columns.Add(new GridViewColumn { Header = "Name", Width = 275, DisplayMemberBinding = new Binding(nameof(MyViewItem.Name)) });
            grView.Columns.Add(new GridViewColumn { Header = "Type", Width = 60, DisplayMemberBinding = new Binding(nameof(MyViewItem.Type)) });
            grView.Columns.Add(new GridViewColumn { Header = "Size", Width = 70, DisplayMemberBinding = new Binding(nameof(MyViewItem.Size)) });
            grView.Columns.Add(new GridViewColumn { Header = "Date", Width = 120, DisplayMemberBinding = new Binding(nameof(MyViewItem.Date)) });

            return grView;
        }
    }
}
