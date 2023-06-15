using Microsoft.Maui.Graphics;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XamlBenchmarkUWP
{
    internal class GraphicsDrawable : IDrawable
    {
        private

        public Canvas Canvas
        {
            get => _canvas;
            set
            {
                _canvas = value;
                _index = 0;
                Trim();
            }
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (drawable != null)
            {
                using (canvas.CreateSession())
                {
                    drawable.Draw(canvas, new RectangleF(0, 0, (float)Canvas.Width, (float)Canvas.Height));
                }
            }
        }
    }
}
