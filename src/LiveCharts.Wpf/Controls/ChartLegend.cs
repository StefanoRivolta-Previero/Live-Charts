﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.DataSeries;
using Orientation = LiveCharts.Core.Abstractions.Orientation;
using Point = LiveCharts.Core.Drawing.Point;
using Size = LiveCharts.Core.Drawing.Size;

namespace LiveCharts.Wpf.Controls
{
    /// <summary>
    /// Default legend class.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.WrapPanel" />
    /// <seealso cref="LiveCharts.Core.Abstractions.ILegend" />
    public class ChartLegend : ItemsControl, ILegend
    {
        /// <summary>
        /// Initializes the <see cref="ChartLegend"/> class.
        /// </summary>
        static ChartLegend()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChartLegend),
                new FrameworkPropertyMetadata(typeof(ChartLegend)));
        }

        #region Properties

        /// <summary>
        /// The orientation property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            nameof(Orientation), typeof(Orientation), typeof(ChartLegend), new PropertyMetadata(default(Orientation)));

        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        /// <value>
        /// The orientation.
        /// </value>
        public Orientation Orientation
        {
            get => (Orientation) GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        /// <summary>
        /// The geometry size property.
        /// </summary>
        public static readonly DependencyProperty GeometrySizeProperty = DependencyProperty.Register(
            nameof(GeometrySize), typeof(double), typeof(ChartLegend), new PropertyMetadata(15d));

        /// <summary>
        /// Gets or sets the size of the geometry.
        /// </summary>
        /// <value>
        /// The size of the geometry.
        /// </value>
        public double GeometrySize
        {
            get => (double) GetValue(GeometrySizeProperty);
            set => SetValue(GeometrySizeProperty, value);
        }

        /// <summary>
        /// The actual orientation property
        /// </summary>
        public static readonly DependencyProperty ActualOrientationProperty = DependencyProperty.Register(
            nameof(ActualOrientation), typeof(System.Windows.Controls.Orientation), typeof(ChartLegend), 
            new PropertyMetadata(default(System.Windows.Controls.Orientation)));

        /// <summary>
        /// Gets the actual orientation.
        /// </summary>
        /// <value>
        /// The actual orientation.
        /// </value>
        public System.Windows.Controls.Orientation ActualOrientation =>
            (System.Windows.Controls.Orientation) GetValue(ActualOrientationProperty);

        /// <summary>
        /// The default group style property
        /// </summary>
        public static readonly DependencyProperty DefaultGroupStyleProperty = DependencyProperty.Register(
            nameof(DefaultGroupStyle), typeof(GroupStyle), typeof(ChartLegend),
            new PropertyMetadata(default(GroupStyle), OnDefaultGroupStyleChanged));

        /// <summary>
        /// Gets or sets the default group style.
        /// </summary>
        /// <value>
        /// The default group style.
        /// </value>
        public GroupStyle DefaultGroupStyle
        {
            get => (GroupStyle)GetValue(DefaultGroupStyleProperty);
            set => SetValue(DefaultGroupStyleProperty, value);
        }

        #endregion

        private static void OnDefaultGroupStyleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (args.NewValue == null) return;

            var tooltip = (ChartToolTip)sender;

            if (tooltip.GroupStyle.Count == 0)
            {
                tooltip.GroupStyle.Add((GroupStyle)args.NewValue);
            }
        }

        Size ILegend.Measure(
            IEnumerable<Series> seriesCollection, 
            Orientation orientation,
            IChartView chart)
        {
            return Dispatcher.Invoke(() =>
            {
                if (Parent == null)
                {
                    var wpfChart = (Chart) chart;
                    wpfChart.Children.Add(this);
                }

                ItemsSource = seriesCollection;

                if (Orientation == Orientation.Auto)
                {
                    SetValue(ActualOrientationProperty,
                        orientation == Orientation.Horizontal
                            ? System.Windows.Controls.Orientation.Horizontal
                            : System.Windows.Controls.Orientation.Vertical);
                }
                else
                {
                    SetValue(ActualOrientationProperty, Orientation.AsWpfOrientation());
                }
                UpdateLayout();
                return new Size(DesiredSize.Width, DesiredSize.Width);
            });
        }

        void ILegend.Move(Point location, IChartView chart)
        {
            Dispatcher.Invoke(() =>
            {
                Canvas.SetLeft(this, location.X);
                Canvas.SetTop(this, location.Y);
            });
        }

        public event DisposingResource Disposed;

        object IResource.UpdateId { get; set; }

        /// <inheritdoc />
        void IResource.Dispose(IChartView view)
        {
            Dispatcher.Invoke(() =>
            {
                var wpfChart = (Chart) view;
                if (wpfChart.Children.Contains(this))
                {
                    wpfChart.Children.Remove(this);
                }
            });
            Disposed?.Invoke(view);
        }
    }
}