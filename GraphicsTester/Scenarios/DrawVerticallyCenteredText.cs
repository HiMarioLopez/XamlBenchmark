﻿using Microsoft.Maui.Graphics;

namespace GraphicsTester.Scenarios
{
    public class DrawVerticallyCenteredText : AbstractScenario
    {
        public DrawVerticallyCenteredText()
            : base(720, 1024)
        {
        }

        public override void Draw(ICanvas canvas)
        {
            canvas.StrokeSize = 1;
            canvas.StrokeColor = Colors.Blue;
            canvas.Font = new Font(name: "Arial", weight: FontWeights.Regular);
            canvas.FontSize = 12f;

            canvas.SaveState();

            var rectHeight = 20;
            canvas.Translate(0, 10);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Really Short 1", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Center);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 30);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Really Short 2", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Center);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 30);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Really Short 3", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Center);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 30);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Really Short 4", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Center);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            canvas.RestoreState();

            canvas.SaveState();
            canvas.Translate(150, 0);

            rectHeight = 20;
            canvas.Translate(0, 10);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Really Short 1", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Top);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 30);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Really Short 2", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Top);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 30);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Really Short 3", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Top);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 30);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Really Short 4", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Top);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            canvas.RestoreState();

            canvas.SaveState();
            canvas.Translate(300, 0);

            rectHeight = 20;
            canvas.Translate(0, 10);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Really Short 1", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Bottom);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 30);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Really Short 2", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Bottom);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 30);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Really Short 3", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Bottom);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 30);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Really Short 4", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Bottom);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            canvas.RestoreState();

            canvas.SaveState();
            canvas.Translate(0, 150);
            canvas.Font = new Font(name: "Arial", weight: FontWeights.Regular);

            rectHeight = 20;
            canvas.Translate(0, 10);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Sys Font R 1", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Center);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 30);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Sys Font R 2", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Center);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 30);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Sys Font R 3", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Center);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 30);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Sys Font R 4", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Center);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            canvas.RestoreState();

            canvas.SaveState();
            canvas.Translate(150, 150);
            canvas.Font = new Font(name: "Arial", weight: FontWeights.Regular);

            rectHeight = 20;
            canvas.Translate(0, 10);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Sys Font R 1", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Top);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 30);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Sys Font R 2", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Top);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 30);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Sys Font R 3", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Top);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 30);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Sys Font R 4", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Top);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            canvas.RestoreState();

            canvas.SaveState();
            canvas.Translate(300, 150);
            canvas.Font = new Font(name: "Arial", weight: FontWeights.Regular);

            rectHeight = 20;
            canvas.Translate(0, 10);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Sys Font R 1", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Bottom);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 30);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Sys Font R 2", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Bottom);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 30);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Sys Font R 3", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Bottom);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 30);
            canvas.DrawRectangle(10, 0, 100, rectHeight);
            canvas.DrawString("Sys Font R 4", 10, 0, 100, rectHeight, HorizontalAlignment.Center, VerticalAlignment.Bottom);
            canvas.DrawLine(10, rectHeight / 2, 100, rectHeight / 2);

            canvas.RestoreState();
        }
    }
}