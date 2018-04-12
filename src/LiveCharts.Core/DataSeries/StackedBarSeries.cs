using System;
using System.Drawing;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries.Data;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.ViewModels;

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// The stacked bar series.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <seealso cref="CartesianSeries{TModel, StackedCoordinate, BarViewModel, Point}" />
    /// <seealso cref="IBarSeries" />
    public class StackedBarSeries<TModel> : BaseBarSeries<TModel, StackedCoordinate, Point<TModel, StackedCoordinate, BarViewModel>>, IBarSeries
    {
        private static ISeriesViewProvider<TModel, StackedCoordinate, BarViewModel> _provider;
        private int _stackIndex;

        /// <inheritdoc />
        int ISeries.GroupingIndex => StackIndex;

        /// <summary>
        /// Gets or sets the stack index, bars that shares the same indexes will be stacked together, 
        /// if set to -1 the series won't be stacked with any other series.
        /// </summary>
        /// <value>
        /// The index of the stack.
        /// </value>
        public int StackIndex
        {
            get => _stackIndex;
            set
            {
                _stackIndex = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        protected override ISeriesViewProvider<TModel, StackedCoordinate, BarViewModel>
            DefaultViewProvider =>
            _provider ?? (_provider = Charting.Current.UiProvider.StackedBarViewProvider<TModel>());

        /// <inheritdoc />
        protected override void BuildModel(
            Point<TModel, StackedCoordinate, BarViewModel> current, UpdateContext context, 
            ChartModel chart, Plane directionAxis, Plane scaleAxis, float cw, float columnStart, 
            float[] byBarOffset, float[] positionOffset, Orientation orientation, int h, int w)
        {
            var currentOffset = chart.ScaleToUi(current.Coordinate[0][0], directionAxis);
            var key = current.Coordinate.Key;
            var value = current.Coordinate.Value;

            float stack;

            unchecked
            {
                stack = context.GetStack((int) key, ScalesAt[1], value >= 0);
            }

            var columnCorner1 = new[]
            {
                currentOffset,
                chart.ScaleToUi(stack, scaleAxis)
            };

            var columnCorner2 = new[]
            {
                currentOffset + cw,
                columnStart
            };

            var difference = Perform.SubstractEach2D(columnCorner1, columnCorner2);

            var location = new[]
            {
                currentOffset,
                columnStart + (columnCorner1[1] < columnStart ? difference[1] : 0f) 
            };

            if (current.View.VisualElement == null)
            {
                var initialRectangle = chart.InvertXy
                    ? new RectangleF(
                        columnStart,
                        location[h] + byBarOffset[1] + positionOffset[1],
                        0f,
                        Math.Abs(difference[h]))
                    : new RectangleF(
                        location[w] + byBarOffset[0] + positionOffset[0],
                        columnStart,
                        Math.Abs(difference[w]),
                        0f);
                current.ViewModel = new BarViewModel(RectangleF.Empty, initialRectangle, orientation);
            }

            var y = location[h] + byBarOffset[1] + positionOffset[1];
            var l = columnCorner1[1] > columnCorner2[1] ? columnCorner1[1] : columnCorner2[1];

            current.ViewModel = new BarViewModel(
                current.ViewModel.To,
                new RectangleF(
                    location[w] + byBarOffset[0] + positionOffset[0],
                    y + (l - y) * current.Coordinate.From / stack,
                    Math.Abs(difference[w]),
                    Math.Abs(difference[h]) * value / stack),
                orientation);
        }
    }
}