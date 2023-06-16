using System;
using System.Windows;
using System.Diagnostics;
using System.Windows.Media;

using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;

using GraphicsTester.Scenarios;

namespace XamlBenchmarkWPF
{
    public partial class MainWindow : Window
    {
        const int TestIterations = GlobalVariables.TestCount;
        int testIncrement = -1;
        Stopwatch timer = new Stopwatch();

        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {
            this.GraphicsView.BackgroundColor = Microsoft.Maui.Graphics.Colors.White;

            foreach (var scenario in ScenarioList.Scenarios)
            {
                List.Items.Add(scenario);
            }
            List.SelectionChanged += OnSelectionChanged;
            List.SelectedIndex = 0;

            this.SizeChanged += (source, args) => Draw();

            CompositionTarget.Rendering += OnRendering;
        }

        public IDrawable Drawable
        {
            set
            {
                this.GraphicsView.Drawable = value;
                Draw();
            }
        }

        private void Draw()
        {
            this.GraphicsView.Invalidate();
        }

        private void OnSelectionChanged(object source, System.Windows.Controls.SelectionChangedEventArgs args)
        {
            AbstractScenario scenario = (AbstractScenario)List.SelectedItem;
            Drawable = scenario;
            this.GraphicsView.Width = scenario.Width;
            this.GraphicsView.Height = scenario.Height;
            this.Title = $"WPF Maui Graphics Sample: {scenario}";
            Draw();
        }

        private void AddToClipboard()
        {
            string fullResult = Clipboard.GetText();
            string testResult = $"(  WPF  ) Elapsed: {Elapsed.Text}, Passes: {Passes.Text}";

            if (string.IsNullOrEmpty(fullResult))
                Clipboard.SetDataObject($"{testResult}\n");
            else
                Clipboard.SetDataObject($"{fullResult}\n{testResult}");
        }

        private void OnRendering(object sender, EventArgs e)
        {
            if (testIncrement != -1)
            {
                int step = testIncrement++ % 3;

                switch (step)
                {
                    case 0:
                        List.SelectedIndex = ScenarioList.Scenarios.Count - 1;
                        break;
                    case 1:
                        List.SelectedIndex = 9;
                        break;

                    case 2:
                        List.SelectedIndex = 27;
                        break;
                }

                Drawable = List.SelectedItem as IDrawable;

                if (testIncrement >= TestIterations)
                {
                    timer.Stop();
                    GlobalVariables.TotalElapsedMM = timer.ElapsedMilliseconds;

                    Elapsed.Text = $"{GlobalVariables.TotalElapsedMM} ms";
                    Passes.Text = $"{GlobalVariables.TotalPasses}";

                    testIncrement = -1;

                    AddToClipboard();
                }
            }
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            if (testIncrement == -1)
            {
                testIncrement = 0;
                GlobalVariables.TotalPasses = 0;
                GlobalVariables.TotalElapsedMM = 0;
                timer.Reset();
                timer.Start();
            }
            else
            {
                testIncrement = -1;
                timer.Stop();
            }
        }
    }
}
