using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UartUiApp.RJControl
{
    public class RJToggleButton : ToggleButton
    {
        public static readonly DependencyProperty OnBackgroundProperty =
            DependencyProperty.Register(
                nameof(OnBackground),
                typeof(Brush),
                typeof(RJToggleButton),
                new PropertyMetadata(Brushes.MediumSlateBlue));

        public static readonly DependencyProperty OffBackgroundProperty =
            DependencyProperty.Register(
                nameof(OffBackground),
                typeof(Brush),
                typeof(RJToggleButton),
                new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty OnToggleColorProperty =
            DependencyProperty.Register(
                nameof(OnToggleColor),
                typeof(Brush),
                typeof(RJToggleButton),
                new PropertyMetadata(Brushes.WhiteSmoke));

        public static readonly DependencyProperty OffToggleColorProperty =
            DependencyProperty.Register(
                nameof(OffToggleColor),
                typeof(Brush),
                typeof(RJToggleButton),
                new PropertyMetadata(Brushes.Gainsboro));
        public Brush OnBackground
        {
            get => (Brush)GetValue(OnBackgroundProperty);
            set => SetValue(OnBackgroundProperty, value);
        }

        public Brush OffBackground
        {
            get => (Brush)GetValue(OffBackgroundProperty);
            set => SetValue(OffBackgroundProperty, value);
        }

        public Brush OnToggleColor
        {
            get => (Brush)GetValue(OnToggleColorProperty);
            set => SetValue(OnToggleColorProperty, value);
        }

        public Brush OffToggleColor
        {
            get => (Brush)GetValue(OffToggleColorProperty);
            set => SetValue(OffToggleColorProperty, value);
        }

        protected override void OnRender(DrawingContext dc)
        {
            var toggleSize = Height - 5;
            dc.PushClip(new RectangleGeometry(new Rect(0, 0, Width, Height)));
            dc.DrawRoundedRectangle(
                IsChecked == true ? OnBackground : OffBackground,
                null, 
                new Rect(0, 0, Width, Height), 
                Height / 2, 
                Height / 2
                );

            dc.DrawEllipse(
                IsChecked == true ? OnToggleColor : OffToggleColor,
                null, 
                new Point(IsChecked == true ? Width - Height / 2 : Height / 2, Height / 2),
                toggleSize / 2, 
                toggleSize / 2
                );

            dc.Pop();
        }

    }
}
