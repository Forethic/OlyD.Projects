using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Lia.Extensions
{
    public static class UIElementEx
    {
        #region Animation - Fade In & Out

        public static Storyboard GetAttachedFadeStoryboard(DependencyObject d)
            => d.GetValue(AttachedFadeStoryboardProperty) as Storyboard;

        public static void SetAttachedFadeStoryboard(DependencyObject d, Storyboard storyboard)
            => d.SetValue(AttachedFadeStoryboardProperty, storyboard);

        public static readonly DependencyProperty AttachedFadeStoryboardProperty
            = DependencyProperty.RegisterAttached("AttachedFadeStoryboard", typeof(Storyboard), typeof(UIElementEx),
                new PropertyMetadata(null));

        public static async Task FadeIn(this UIElement element, TimeSpan? duration = null)
        {
            if (element == null) { return; }

            element.Visibility = Visibility.Visible;
            var fadeInStoryboard = new Storyboard();
            var fadeInAnimation = new FadeInThemeAnimation();

            if (duration.HasValue) { fadeInAnimation.Duration = duration.Value; }
            Storyboard.SetTarget(fadeInAnimation, element);
            fadeInStoryboard.Children.Add(fadeInAnimation);
            await fadeInStoryboard.BeginAsync();
        }

        public static async Task FadeOut(this UIElement element, TimeSpan? duration = null)
        {
            if (element == null) { return; }

            var fadeOutStoryboard = new Storyboard();
            var fadeOutAnimation = new FadeOutThemeAnimation();

            if (duration.HasValue) { fadeOutAnimation.Duration = duration.Value; }
            Storyboard.SetTarget(fadeOutAnimation, element);
            fadeOutStoryboard.Children.Add(fadeOutAnimation);
            await fadeOutStoryboard.BeginAsync();
        }


        public static async Task FadeIn(this UIElement element, TimeSpan? duration = null, EasingFunctionBase easingFunction = null, double targetOpacity = 1.0)
        {
            element.CleanUpPreviousFadeStoryboard();

            var fadeInAnimation = new DoubleAnimation();
            fadeInAnimation.Duration = duration ?? TimeSpan.FromSeconds(0.4);
            fadeInAnimation.To = targetOpacity;
            fadeInAnimation.EasingFunction = easingFunction;

            Storyboard.SetTarget(fadeInAnimation, element);
            Storyboard.SetTargetProperty(fadeInAnimation, "Opacity");

            var fadeInStoryboard = new Storyboard();
            fadeInStoryboard.Children.Add(fadeInAnimation);
            SetAttachedFadeStoryboard(element, fadeInStoryboard);
            await fadeInStoryboard.BeginAsync();

            element.Opacity = targetOpacity;
            fadeInStoryboard.Stop();
        }

        public static async Task FadeOut(this UIElement element, TimeSpan? duration = null, EasingFunctionBase easingFunction = null)
        {
            element.CleanUpPreviousFadeStoryboard();

            var fadeOutAnimation = new DoubleAnimation();
            fadeOutAnimation.Duration = duration ?? TimeSpan.FromSeconds(0.4);
            fadeOutAnimation.To = 0.0;
            fadeOutAnimation.EasingFunction = easingFunction;

            Storyboard.SetTarget(fadeOutAnimation, element);
            Storyboard.SetTargetProperty(fadeOutAnimation, "Opacity");

            var fadeOutStoryboard = new Storyboard();
            fadeOutStoryboard.Children.Add(fadeOutAnimation);
            SetAttachedFadeStoryboard(element, fadeOutStoryboard);
            await fadeOutStoryboard.BeginAsync();

            element.Opacity = 0.0;
            fadeOutStoryboard.Stop();
        }

        public static void CleanUpPreviousFadeStoryboard(this UIElement element)
        {
            var storyboard = GetAttachedFadeStoryboard(element);
            if (storyboard != null)
            {
                storyboard.Stop();
            }
        }

        #endregion
    }
}