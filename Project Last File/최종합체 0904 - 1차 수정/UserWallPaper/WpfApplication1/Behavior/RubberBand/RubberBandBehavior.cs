using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.ComponentModel;

namespace RubberBand
{
    [DescriptionAttribute("Enable Rubberband selection for a WPF listbox object.")]
    public class RubberBandBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += new System.Windows.RoutedEventHandler(AssociatedObject_Loaded);
            base.OnAttached();
        }

        void AssociatedObject_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            RubberBandAdorner band = new RubberBandAdorner(AssociatedObject);
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(AssociatedObject);
            if(adornerLayer != null)
            adornerLayer.Add(band);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= new System.Windows.RoutedEventHandler(AssociatedObject_Loaded);
            base.OnDetaching();
        }
    }
}
