using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

namespace RubberBand
{
    public class RubberBandAdorner : Adorner
    {
        private Point startpoint;
        private Point currentpoint;
        private Brush brush;
        private bool flag;
        private ScrollViewer viewer;
        private ScrollBar scrollbar;

        public RubberBandAdorner(UIElement adornedElement)
            :base(adornedElement)
        {
            IsHitTestVisible = false;
            adornedElement.MouseMove += new MouseEventHandler(adornedElement_PreviewMouseMove);
            adornedElement.MouseLeftButtonDown += new MouseButtonEventHandler(adornedElement_PreviewMouseLeftButtonDown);
            adornedElement.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(adornedElement_PreviewMouseLeftButtonUp);
            brush = new SolidColorBrush(SystemColors.HighlightColor);
            brush.Opacity = 0.3;
        }

        void adornedElement_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DisposeRubberBand();
        }

        void adornedElement_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox _selector = AdornedElement as ListBox;
            if (_selector.SelectedItems != null && (_selector.SelectionMode == SelectionMode.Extended || _selector.SelectionMode == SelectionMode.Multiple))
            {
                _selector.SelectedItems.Clear();
            }
            startpoint = Mouse.GetPosition(this.AdornedElement);
            Mouse.Capture(_selector);
            flag = true;
        }

        public static childItem FindVisualChild<childItem>(DependencyObject obj)
   where childItem : DependencyObject
        {
            // Search immediate children first (breadth-first)
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child != null && child is childItem)
                    return (childItem)child;

                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);

                    if (childOfChild != null)
                        return childOfChild;
                }
            }

            return null;
        }

        void adornedElement_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && flag)
            {
                currentpoint = Mouse.GetPosition(AdornedElement);
                
                Selector _selector = AdornedElement as Selector;
                if (viewer == null)
                {
                    viewer = FindVisualChild<ScrollViewer>(_selector);
                }

                if (scrollbar == null)
                {
                    scrollbar = FindVisualChild<ScrollBar>(viewer);
                }

                if (_selector.Items.Count > 0)
                {
                    if (currentpoint.Y > ((FrameworkElement)AdornedElement).ActualHeight && viewer.VerticalOffset < _selector.ActualHeight && scrollbar.Visibility == System.Windows.Visibility.Visible)
                    {
                        startpoint.Y -= 50;
                    }
                    else if (currentpoint.Y < 0 && viewer.VerticalOffset > 0 && scrollbar.Visibility == System.Windows.Visibility.Visible)
                    {
                        startpoint.Y += 50;
                    }
                }

                InvalidateVisual();

                foreach (var obj in _selector.Items)
                {
                    ListBoxItem item = _selector.ItemContainerGenerator.ContainerFromItem(obj) as ListBoxItem;
                    if (item != null)
                    {
                        Point point = item.TransformToAncestor(AdornedElement).Transform(new Point(0, 0));
                        Rect bandrect = new Rect(startpoint, currentpoint);
                        Rect elementrect = new Rect(point.X, point.Y, item.ActualWidth, item.ActualHeight);
                        if (bandrect.IntersectsWith(elementrect))
                        {
                            item.IsSelected = true;
                        }
                        else
                        {
                            item.IsSelected = false;
                        }
                    }
                }
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect rect = new Rect(startpoint, currentpoint);
            drawingContext.DrawGeometry(brush, new Pen(SystemColors.HighlightBrush, 1), new RectangleGeometry(rect));
            base.OnRender(drawingContext);
        }

        private void DisposeRubberBand()
        {
            currentpoint = new Point(0, 0);
            startpoint = new Point(0, 0);
            AdornedElement.ReleaseMouseCapture();
            InvalidateVisual();
            flag = false;
        }
    }
}
