/*
 * Copyright 2018 Jan Tschada
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoordinateFormatterApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MapViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = (MapViewModel)Grid.DataContext;
        }

        private void MapView_MouseMove(object sender, MouseEventArgs e)
        {
            if (null == FocusMapView.GetCurrentViewpoint(ViewpointType.BoundingGeometry))
            {
                return;
            }

            var screenLocation = e.GetPosition(FocusMapView);
            var location = FocusMapView.ScreenToLocation(screenLocation);
            if (FocusMapView.IsWrapAroundEnabled)
            {
                location = (MapPoint)GeometryEngine.NormalizeCentralMeridian(location);
            }

            const int precision = 8;
            _viewModel.Wgs84 = CoordinateFormatter.ToLatitudeLongitude(location, LatitudeLongitudeFormat.DecimalDegrees, precision);
            _viewModel.Mgrs = CoordinateFormatter.ToMgrs(location, MgrsConversionMode.Automatic, precision, true);
            _viewModel.Utm = CoordinateFormatter.ToUtm(location, UtmConversionMode.NorthSouthIndicators, true);
        }
    }
}
