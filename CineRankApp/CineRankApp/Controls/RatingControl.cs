using CineRankApp.Common;
using CineRankApp.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CineRankApp.Controls
{
    public class RatingControl : HorizontalStackLayout
    {
        private bool _isReadOnly = false;
        public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value),
                                                                                typeof(int),
                                                                                typeof(RatingControl));
        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color),
                                                                                typeof(Color),
                                                                                typeof(RatingControl),
                                                                                defaultValue:Colors.Gold);
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public static readonly BindableProperty RatingTappedCommandProperty = BindableProperty.Create(nameof(RatingTappedCommand),
                                                                                                      typeof(ICommand),
                                                                                                      typeof(RatingControl));

        public ICommand RatingTappedCommand
        {
            get=>(ICommand)GetValue(RatingTappedCommandProperty);
            set=>SetValue(RatingTappedCommandProperty, value);
        }

        [Obsolete]
        public RatingControl()
        {
            PropertyChanged += OnRatingControlPropertyChanged;
            MessagingCenter.Subscribe<object, int>(this, "UserRatingChanged", (sender, userRating) =>
            {
                Value = userRating;
            });

            UpdateStars(Value, Color);
        }


        public void OnStarTapped(int starValue)
        {
            if (_isReadOnly)
            {
                return;
            }
            UpdateStars(starValue,Color);
            _isReadOnly = true;

            RatingTappedCommand?.Execute(starValue);

        }

        private void UpdateStars(int starValue, Color color)
        {
            Children.Clear();
            for (int i = 0; i < 5; i++)
            {
                var star = new StarControl(this, i < starValue ? MaterialIconsFont.Star : MaterialIconsFont.Star_outline, color);
                star.Value = i + 1;
                Children.Add(star);

            }
        }

        private void OnRatingControlPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Value))
            {
                UpdateStars(Value, Color);
                _isReadOnly = true;
            }

            else if (e.PropertyName == nameof(Color))
            {
                UpdateStars(Value, Color);
            }
        }

    }
}
        

