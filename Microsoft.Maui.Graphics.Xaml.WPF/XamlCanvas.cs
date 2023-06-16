using System;
using System.Collections.Generic;
using Microsoft.Maui.Graphics.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Numerics;
using SharpDX.DirectWrite;

namespace Microsoft.Maui.Graphics.Xaml
{
    public enum ItemType
    {
        DrawLine,
        DrawRectangle,
        DrawRoundedRectangle,
        DrawEllipse,
        DrawArc,
        DrawPath,
        DrawText,
        DrawTextInRect,
        DrawImage,
        FillRectangle,
        FillRoundedRectangle,
        FillEllipse,
        FillArc,
        FillPath
    }

    public class Item
    {
        public ItemType Type;
        public UIElement Element;
    }

    public class XamlCanvas : AbstractCanvas<XamlCanvasState>
    {
        public static global::SharpDX.DirectWrite.Factory FactoryDirectWrite = new global::SharpDX.DirectWrite.Factory(global::SharpDX.DirectWrite.FactoryType.Shared);

        private readonly List<Item> _items = new List<Item>();

        private Canvas _canvas;
        private int _index;
        private float _rectX;
        private float _rectY;
        private float _rectWidth;
        private float _rectHeight;
        private float _rectCornerRadius;

        public XamlCanvas()
            : base(CreateNewState, CreateStateCopy)
        {
        }

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

        private static XamlCanvasState CreateNewState(object context)
        {
            return new XamlCanvasState();
        }

        private static XamlCanvasState CreateStateCopy(XamlCanvasState prototype)
        {
            return new XamlCanvasState(prototype);
        }

        public XamlCanvasSession CreateSession()
        {
            return new XamlCanvasSession(this);
        }

        class StringSizeService : IStringSizeService
        {
            public SizeF GetStringSize(string value, IFont font, float textSize)
            {
                if (value == null) return new SizeF();

                float fontSize = textSize;
                float factor = 1;
                while (fontSize > 14)
                {
                    fontSize /= 14;
                    factor *= 14;
                }

                if (font == null)
                    font = Graphics.Font.Default;

                var size = new SizeF();

                var textFormat = new TextFormat(FactoryDirectWrite, font.Name, fontSize);
                textFormat.TextAlignment = TextAlignment.Leading;
                textFormat.ParagraphAlignment = ParagraphAlignment.Near;

                var textLayout = new TextLayout(FactoryDirectWrite, value, textFormat, 512, 512);
                size.Width = textLayout.Metrics.Width;
                size.Height = textLayout.Metrics.Height;

                size.Width *= factor;
                size.Height *= factor;

                return size;
            }

            public SizeF GetStringSize(
                string value,
                IFont font,
                float textSize,
                HorizontalAlignment horizontalAlignment,
                VerticalAlignment verticalAlignment)
            {
                if (value == null) return new SizeF();

                float fontSize = textSize;
                float factor = 1;
                while (fontSize > 14)
                {
                    fontSize /= 14;
                    factor *= 14;
                }

                var size = new SizeF();

                var textFormat = new TextFormat(FactoryDirectWrite, font.Name, font.Weight.ToFontWeight(), font.StyleType.ToFontStyle(), fontSize);
                if (horizontalAlignment == HorizontalAlignment.Left)
                {
                    textFormat.TextAlignment = TextAlignment.Leading;
                }
                else if (horizontalAlignment == HorizontalAlignment.Center)
                {
                    textFormat.TextAlignment = TextAlignment.Center;
                }
                else if (horizontalAlignment == HorizontalAlignment.Right)
                {
                    textFormat.TextAlignment = TextAlignment.Trailing;
                }
                else if (horizontalAlignment == HorizontalAlignment.Justified)
                {
                    textFormat.TextAlignment = TextAlignment.Justified;
                }

                if (verticalAlignment == VerticalAlignment.Top)
                {
                    textFormat.ParagraphAlignment = ParagraphAlignment.Near;
                }
                else if (verticalAlignment == VerticalAlignment.Center)
                {
                    textFormat.ParagraphAlignment = ParagraphAlignment.Center;
                }
                else if (verticalAlignment == VerticalAlignment.Bottom)
                {
                    textFormat.ParagraphAlignment = ParagraphAlignment.Far;
                }

                var textLayout = new TextLayout(FactoryDirectWrite, value, textFormat, 512f, 512f, Dip, false);
                size.Width = textLayout.Metrics.Width;
                size.Height = textLayout.Metrics.Height;


                size.Width *= factor;
                size.Height *= factor;

                return size;
            }
        }

        internal void BeginDrawing()
        {
            ResetState();
            _index = 0;
        }

        internal void EndDrawing()
        {
            Trim();
        }

        private void Trim()
        {
            if (_index < _items.Count)
            {
                var n = _items.Count - _index;
                for (var i = 0; i < n; i++)
                {
                    _canvas.Children.Remove(_items[_index + i].Element);
                }

                _items.RemoveRange(_index, n);
            }
        }

        private Item GetOrCreateItem(ItemType type)
        {
            Item item;

            if (_index >= _items.Count)
            {
                System.Diagnostics.Debug.WriteLine("Creating item of type {0}", type);

                item = CreateItem(type);
                _items.Add(item);
                _canvas.Children.Add(item.Element);
            }
            else
            {
                item = _items[_index];

                if (item.Type != type)
                {
                    System.Diagnostics.Debug.WriteLine("Item types diverge at index {0}.  Wanted {1}, but found {2}", _index, type, _items[_index].Type);

                    Trim();
                    item = CreateItem(type);
                    _items.Add(item);
                    _canvas.Children.Add(item.Element);
                }
            }


            _index++;
            return item;
        }

        private Item CreateItem(ItemType type)
        {
            var element = CreateElement(type);
            var item = new Item
            {
                Type = type,
                Element = element
            };

            return item;
        }

        private UIElement CreateElement(ItemType type)
        {
            switch (type)
            {
                case ItemType.DrawLine:
                    return new Line();
                case ItemType.DrawRectangle:
                case ItemType.DrawRoundedRectangle:
                case ItemType.FillRectangle:
                case ItemType.FillRoundedRectangle:
                    return new global::System.Windows.Shapes.Rectangle();
                case ItemType.DrawEllipse:
                case ItemType.FillEllipse:
                    return new Ellipse();
                case ItemType.DrawArc:
                case ItemType.FillArc:
                    var figure = new PathFigure();
                    figure.Segments.Add(new ArcSegment());
                    var geometry = new PathGeometry();
                    geometry.Figures.Add(figure);
                    var path = new Path
                    {
                        Data = geometry
                    };
                    return path;
                case ItemType.DrawPath:
                case ItemType.FillPath:
                    return new Path();
                case ItemType.DrawText:
                    return new TextBlock();
                case ItemType.DrawTextInRect:
                    var textBlock = new TextBlock();
                    var border = new Border { Child = textBlock };
                    return border;
            }

            return new global::System.Windows.Shapes.Rectangle();
        }

        private void CreateFillRect(float x, float y, float width, float height)
        {
            _rectX = x;
            _rectY = y;
            _rectWidth = width;
            _rectHeight = height;
        }

        private void CreateStrokeRoundedRect(float x, float y, float width, float height, float cornerRadius)
        {
            CreateStrokeRect(x, y, width, height);

            if (cornerRadius <= 0)
            {
                _rectCornerRadius = 0;
                return;
            }

            _rectCornerRadius = cornerRadius;
        }

        private void CreateStrokeRect(float x, float y, float width, float height)
        {
            var strokeSize = CurrentState.StrokeSize;
            var halfStroke = strokeSize / 2;

            _rectX = x - halfStroke;
            _rectY = y - halfStroke;
            _rectWidth = width + strokeSize;
            _rectHeight = height + strokeSize;
        }

        private void CreateArcStrokeRect(float x, float y, float width, float height)
        {
            _rectX = x;
            _rectY = y;
            _rectWidth = width;
            _rectHeight = height;

        }

        public override void DrawImage(IImage image, float x, float y, float width, float height)
        {
        }

        public override void SetFillPaint(Paint paint, RectF rectangle)
        {
            if (paint is SolidPaint solidPaint)
                FillColor = solidPaint.Color;

            CurrentState.SetFillPaint(paint, rectangle);
        }

        public override void SetShadow(SizeF offset, float blur, Color color)
        {
            CurrentState.SetShadow(offset, blur, color);
        }

        public override void DrawString(
            string value,
            float x,
            float y,
            float width,
            float height,
            HorizontalAlignment horizontalAlignment,
            VerticalAlignment verticalAlignment,
            TextFlow textFlow = TextFlow.ClipBounds,
            float lineAdjustment = 0)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            CreateFillRect(x, y, width, height);

            var item = GetOrCreateItem(ItemType.DrawTextInRect);
            var element = (Border)item.Element;
            var block = (TextBlock)element.Child;

            block.Text = value;

            Canvas.SetLeft(element, _rectX);
            Canvas.SetTop(element, _rectY);
            element.Width = _rectWidth;
            element.Height = _rectHeight;

            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    block.TextAlignment = TextAlignment.Left;
                    break;
                case HorizontalAlignment.Center:
                    block.TextAlignment = TextAlignment.Center;
                    break;
                case HorizontalAlignment.Right:
                    block.TextAlignment = TextAlignment.Right;
                    break;
                case HorizontalAlignment.Justified:
                    block.TextAlignment = TextAlignment.Justify;
                    break;
            }

            switch (verticalAlignment)
            {
                case VerticalAlignment.Top:
                    block.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    break;
                case VerticalAlignment.Center:
                    block.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    break;
                case VerticalAlignment.Bottom:
                    block.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                    break;
            }

            block.FontSize = CurrentState.FontSize;
            block.Foreground = CurrentState.XamlFontBrush;
            block.TextTrimming = TextTrimming.None;
            block.TextWrapping = TextWrapping.Wrap;
            block.Padding = new Thickness();
            block.Opacity = CurrentState.Alpha;
            block.Effect = CurrentState.XamlEffect;
            block.RenderTransform = CurrentState.GetXamlTransform(_rectX, _rectY);
        }

        public override void DrawString(string value, float x, float y, HorizontalAlignment horizontalAlignment)
        {
            var item = GetOrCreateItem(ItemType.DrawText);
            var element = (TextBlock)item.Element;
            element.Text = value;

            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    element.TextAlignment = TextAlignment.Left;
                    break;
                case HorizontalAlignment.Center:
                    element.TextAlignment = TextAlignment.Center;
                    break;
                case HorizontalAlignment.Right:
                    element.TextAlignment = TextAlignment.Right;
                    break;
                case HorizontalAlignment.Justified:
                    element.TextAlignment = TextAlignment.Justify;
                    break;
            }

            Canvas.SetLeft(element, _rectX);
            Canvas.SetTop(element, _rectY);

            element.FontSize = CurrentState.FontSize;
            element.Foreground = CurrentState.XamlFontBrush;
            element.TextTrimming = TextTrimming.None;
            element.TextWrapping = TextWrapping.NoWrap;
            element.Opacity = CurrentState.Alpha;
            element.Effect = CurrentState.XamlEffect;
            element.RenderTransform = CurrentState.GetXamlTransform(_rectX, _rectY);
        }

        public override void DrawText(IAttributedText value, float x, float y, float width, float height)
        {
            System.Diagnostics.Debug.WriteLine("XamlCanvas.DrawText not yet implemented.");
            DrawString(value?.Text, x, y, width, height, HorizontalAlignment.Left, VerticalAlignment.Top);
        }

        public override void FillEllipse(float x, float y, float width, float height)
        {
            var item = GetOrCreateItem(ItemType.FillEllipse);
            var element = (Ellipse)item.Element;

            CreateFillRect(x, y, width, height);

            Canvas.SetLeft(element, _rectX);
            Canvas.SetTop(element, _rectY);
            element.Width = _rectWidth;
            element.Height = _rectHeight;

            element.Fill = CurrentState.XamlFillBrush;
            element.Opacity = CurrentState.Alpha;
            element.Effect = CurrentState.XamlEffect;
            element.RenderTransform = CurrentState.GetXamlTransform(_rectX, _rectY);
        }

        public override void FillRoundedRectangle(float x, float y, float width, float height, float cornerRadius)
        {
            var item = GetOrCreateItem(ItemType.FillRoundedRectangle);
            var element = (global::System.Windows.Shapes.Rectangle)item.Element;

            CreateFillRect(x, y, width, height);

            var halfHeight = Math.Abs(_rectHeight / 2);
            if (cornerRadius > halfHeight)
            {
                cornerRadius = halfHeight;
            }

            var halfWidth = Math.Abs(_rectWidth / 2);
            if (cornerRadius > halfWidth)
            {
                cornerRadius = halfWidth;
            }

            Canvas.SetLeft(element, _rectX);
            Canvas.SetTop(element, _rectY);
            element.Width = _rectWidth;
            element.Height = _rectHeight;
            element.RadiusX = cornerRadius;
            element.RadiusY = cornerRadius;

            element.Fill = CurrentState.XamlFillBrush;
            element.Opacity = CurrentState.Alpha;
            element.Effect = CurrentState.XamlEffect;
            element.RenderTransform = CurrentState.GetXamlTransform(_rectX, _rectY);
        }

        public override void FillRectangle(float x, float y, float width, float height)
        {
            var item = GetOrCreateItem(ItemType.FillRectangle);
            var element = (global::System.Windows.Shapes.Rectangle)item.Element;

            CreateFillRect(x, y, width, height);

            Canvas.SetLeft(element, _rectX);
            Canvas.SetTop(element, _rectY);
            element.Width = _rectWidth;
            element.Height = _rectHeight;
            element.RadiusX = 0;
            element.RadiusY = 0;

            element.Fill = CurrentState.XamlFillBrush;
            element.Opacity = CurrentState.Alpha;
            element.Effect = CurrentState.XamlEffect;
            element.RenderTransform = CurrentState.GetXamlTransform(_rectX, _rectY);
        }

        public override float MiterLimit
        {
            set => CurrentState.MiterLimit = value;
        }

        public override Color StrokeColor
        {
            set => CurrentState.StrokeColor = value;
        }

        public override LineCap StrokeLineCap
        {
            set => CurrentState.StrokeLineCap = value;
        }

        public override LineJoin StrokeLineJoin
        {
            set => CurrentState.StrokeLineJoin = value;
        }

        public override Color FillColor
        {
            set => CurrentState.FillColor = value;
        }

        public override Color FontColor
        {
            set => CurrentState.FontColor = value;
        }

        public override float FontSize
        {
            set => CurrentState.FontSize = value;
        }

        public override float Alpha
        {
            set => CurrentState.Alpha = value;
        }

        public override bool Antialias
        {
            set { }
        }

        public override BlendMode BlendMode
        {
            set { }
        }

        protected override float PlatformStrokeSize { set => throw new NotImplementedException(); }
        public override IFont Font { set => throw new NotImplementedException(); }

        public override void FillPath(PathF path, WindingMode windingMode)
        {
            var item = GetOrCreateItem(ItemType.FillPath);
            var element = (Path)item.Element;

            var transformedPath = path;
            if (!CurrentState.Transform.IsIdentity)
            {
                transformedPath = new PathF(path);
                transformedPath.Transform(CurrentState.Transform);
            }

            var geometry = transformedPath.AsPathGeometry();
            element.Data = geometry;

            element.Fill = CurrentState.XamlFillBrush;
            element.Opacity = CurrentState.Alpha;
            element.Effect = CurrentState.XamlEffect;
            geometry.FillRule = windingMode == WindingMode.NonZero ? FillRule.Nonzero : FillRule.EvenOdd;
        }

        public override void FillArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise)
        {
            while (startAngle < 0)
            {
                startAngle += 360;
            }

            while (endAngle < 0)
            {
                endAngle += 360;
            }

            var item = GetOrCreateItem(ItemType.FillArc);
            var element = (Path)item.Element;

            CreateFillRect(x, y, width, height);

            var geometry = (PathGeometry)element.Data;
            var figure = geometry.Figures[0];
            var arcSegment = (ArcSegment)figure.Segments[0];

            var sweep = GeometryUtil.GetSweep(startAngle, endAngle, clockwise);
            var absSweep = Math.Abs(sweep);
            var startPoint = GeometryUtil.EllipseAngleToPoint(_rectX, _rectY, _rectWidth, _rectHeight, -startAngle);
            var endPoint = GeometryUtil.EllipseAngleToPoint(_rectX, _rectY, _rectWidth, _rectHeight, -endAngle);

            figure.StartPoint = new global::System.Windows.Point(startPoint.X, startPoint.Y);
            arcSegment.Point = new global::System.Windows.Point(endPoint.X, endPoint.Y);
            arcSegment.Size = new global::System.Windows.Size(_rectWidth / 2, _rectHeight / 2);
            arcSegment.SweepDirection = clockwise ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;
            arcSegment.IsLargeArc = absSweep >= 180;

            element.Fill = CurrentState.XamlFillBrush;
            element.Opacity = CurrentState.Alpha;
            element.Effect = CurrentState.XamlEffect;

            var pathX = geometry.Bounds.Left;
            var pathY = geometry.Bounds.Top;
            element.RenderTransform = CurrentState.GetXamlTransform(pathX, pathY);
        }

        #region Not Implemented

        public override void SubtractFromClip(float x, float y, float width, float height)
        {
        }

        public override void ClipPath(PathF path, WindingMode windingMode = WindingMode.NonZero)
        {
        }

        public override void ClipRectangle(float x, float y, float width, float height)
        {
        }

        protected override void PlatformSetStrokeDashPattern(float[] strokePattern, float strokeDashOffset, float strokeSize)
        {
            throw new NotImplementedException();
        }

        protected override void PlatformDrawLine(float x1, float y1, float x2, float y2)
        {
            throw new NotImplementedException();
        }

        protected override void PlatformDrawArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise, bool closed)
        {
            throw new NotImplementedException();
        }

        protected override void PlatformDrawRectangle(float x, float y, float width, float height)
        {
            throw new NotImplementedException();
        }

        protected override void PlatformDrawRoundedRectangle(float x, float y, float width, float height, float cornerRadius)
        {
            throw new NotImplementedException();
        }

        protected override void PlatformDrawEllipse(float x, float y, float width, float height)
        {
            throw new NotImplementedException();
        }

        protected override void PlatformDrawPath(PathF path)
        {
            throw new NotImplementedException();
        }

        protected override void PlatformRotate(float degrees, float radians, float x, float y)
        {
            throw new NotImplementedException();
        }

        protected override void PlatformRotate(float degrees, float radians)
        {
            throw new NotImplementedException();
        }

        protected override void PlatformScale(float fx, float fy)
        {
            throw new NotImplementedException();
        }

        protected override void PlatformTranslate(float tx, float ty)
        {
            throw new NotImplementedException();
        }

        protected override void PlatformConcatenateTransform(Matrix3x2 transform)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
