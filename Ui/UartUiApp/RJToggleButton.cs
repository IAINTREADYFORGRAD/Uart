﻿using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace UartUiApp.RJControl
{
    public class RJToggleButton : ToggleButton
    {


        private Brush OnBackground = (Brush)new BrushConverter().ConvertFromString("#99D9EA");
        private Brush OffBackground = (Brush)new BrushConverter().ConvertFromString("#7092BE");
        private Brush ToggleColor = (Brush)new BrushConverter().ConvertFromString("#E0E0E0");
        public RJToggleButton()
        {
            // Set default Width and Height
            this.Width = 20; 
            this.Height = 10;
            this.CornerRadius = 10;
        }
        protected override void OnRender(DrawingContext dc)
        {
            var toggleSize = Height - 5;
            var togglePosition = IsChecked == true ? Width - Height : 0; // Toggle moves to the right when checked

            dc.PushClip(new RectangleGeometry(new Rect(0, 0, Width, Height)));
            dc.DrawRoundedRectangle(
                OffBackground,
                null,
                new Rect(30, 30, Width, Height),
                Height / 2,
                Height / 2
            );

            //dc.DrawEllipse(
            //    ToggleColor,
            //    null,
            //   new Point(togglePosition + toggleSize / 2, Height / 2), // Move toggle to left or right
            //    toggleSize / 2,
            //    toggleSize / 2
            //    );

            dc.Pop();
        }

    }
}
