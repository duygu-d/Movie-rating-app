using CineRankApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineRankApp.Controls
{
    public class StarControl : ContentView
    {
        private RatingControl _parentControl;
        private bool _isReadOnly = false;
        public int Value { get; set; }

        public StarControl(RatingControl parent, string icon, Color color)
        {
            _parentControl = parent;
            var starLabel = new Label
            {
                Text = icon,
                FontFamily = "MaterialIconsRegular",
                FontSize = 25,
                TextColor = color
            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (sender, e) => OnStarTapped();
            GestureRecognizers.Add(tapGestureRecognizer);

            Content = starLabel;
        }

        private void OnStarTapped()
        {
            if (_isReadOnly)
            {
                return;
            }
            _parentControl.OnStarTapped(Value);

            _isReadOnly = true;

        }
    }
}
