using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UserWallPaper.Xml;

namespace UserWallPaper.CustomControl
{
    class MyInkCanvas : InkCanvas
    {
        Service service;
        public Service Service
        {
            get { return this.service; }
            set { this.service = value; }
        }

        MainWindow parent;
        public new MainWindow Parent
        {
            get { return this.parent; }
            set { this.parent = value; }
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            // Track when objects are added and removed
            if (visualAdded != null)
            {
                // Do stuff with the added object
            }
            if (visualRemoved != null)
            {
                // Do stuff with the removed object

                TextControl text = visualRemoved as TextControl;

                if (text != null)
                {
                    string left = text.GetValue(InkCanvas.LeftProperty).ToString();
                    string top = text.GetValue(InkCanvas.TopProperty).ToString();

                    if (parent.Flag != 0)
                    {
                        DrawXml.Remove(parent.Page, text.ID);
                        NoteXml.Remove(text.ID);
                    }

                    if (service != null)
                        service.Erase("Rectangle", parent.Page, double.Parse(left), double.Parse(top), null);
                }
            }

            // Call base function
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }

        protected override void OnStrokeErasing(InkCanvasStrokeErasingEventArgs e)
        {
            DrawXml.Remove(parent.Page, this.Strokes.IndexOf(e.Stroke), this.Strokes.Count);

            if (service != null)
                service.Erase("Pen",parent.Page, 0, 0, (Point[])e.Stroke.StylusPoints);
        }
    }
}
