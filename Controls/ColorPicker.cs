using System;
using System.Drawing;
using System.Windows.Forms;

namespace Apps.Controls
{
    partial class ColorPicker : ComboBox
    {
        public ColorPicker()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;
            foreach (System.Reflection.PropertyInfo prop in typeof(Color).GetProperties())
            {
                if (prop.PropertyType.FullName == "System.Drawing.Color")
                {
                     Items.Add(prop.Name);
                }
            }
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index == -1)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromName("White")), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            }
            else
            {
                string text = Items[e.Index].ToString();
                e.Graphics.FillRectangle(new SolidBrush(Color.FromName(text)), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            }
        }
    }
}
