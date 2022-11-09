using Lia.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Lia.ControlHelpers
{
    public class FadeInOnLoadedHandler
    {
        private Image _image;
        private BitmapImage _source;
        private double _targetOpacity;

        public FadeInOnLoadedHandler(Image image)
        {
            Attach(image);
        }

        private void Attach(Image image)
        {
            _image = image;
            _source = image.Source as BitmapImage;

            if (_source != null)
            {
                // 已经加载好了
                if (_source.PixelWidth > 0)
                {
                    image.Opacity = 1.0;
                    _image = null;
                    _source = null;
                    return;
                }
                _source.ImageOpened += Source_ImageOpened;
                _source.ImageFailed += Source_ImageFailed;
            }
            image.Unloaded += Image_Unloaded;
            _targetOpacity = image.Opacity == 0.0 ? 1.0 : image.Opacity;
            image.Opacity = 0; // 先不可见
        }
        public void Detach()
        {
            if (_source != null)
            {
                _source.ImageFailed -= Source_ImageFailed;
                _source.ImageOpened -= Source_ImageOpened;
                _source = null;
            }

            if (_image != null)
            {
                _image.Unloaded -= Image_Unloaded;
                _image.CleanUpPreviousFadeStoryboard();
                _image.Opacity = _targetOpacity;
                _image = null;
            }
        }


        private void Image_Unloaded(object sender, RoutedEventArgs e) => Detach();

        private void Source_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (sender is BitmapImage source)
            {
                source.ImageOpened -= Source_ImageOpened;
                source.ImageFailed -= Source_ImageFailed;
            }
        }

        /// <summary>
        /// 当图片加载完成 - FadeIn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Source_ImageOpened(object sender, RoutedEventArgs e)
        {
            if (sender is BitmapImage source)
            {
                source.ImageOpened -= Source_ImageOpened;
                source.ImageFailed -= Source_ImageOpened;
                _image.FadeIn(TimeSpan.FromSeconds(1.0), null, _targetOpacity);
            }
        }
    }

    public static class ImageHelper
    {
        public static readonly DependencyProperty FadeInOnLoadedProperty
            = DependencyProperty.RegisterAttached("FadeInOnLoaded", typeof(bool), typeof(ImageHelper),
                new PropertyMetadata(false, OnFadeInOnLoadedChanged));

        public static readonly DependencyProperty FadeInOnLoadedHandlerProperty
            = DependencyProperty.RegisterAttached("FadeInOnLoadedHandler", typeof(FadeInOnLoadedHandler), typeof(ImageHelper),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SourceProperty
            = DependencyProperty.RegisterAttached("Source", typeof(object), typeof(ImageHelper),
                new PropertyMetadata(null, OnSourceChanged));

        public static bool GetFadeInOnLoaded(DependencyObject d) => (bool)d.GetValue(FadeInOnLoadedProperty);
        public static void SetFadeInOnLoaded(DependencyObject d, bool value) => d.SetValue(FadeInOnLoadedProperty, value);
        private static void OnFadeInOnLoadedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignMode.DesignModeEnabled) { return; }

            if (d is Image image)
            {
                DispatchedHandler dispatchedHandler = null;
                var fadeInOnLoaded = GetFadeInOnLoaded(d);
                if (fadeInOnLoaded)
                {
                    var fadeInOnLoadedHandler = new FadeInOnLoadedHandler(image);
                    SetFadeInOnLoadedHandler(d, fadeInOnLoadedHandler);
                    var dispatcher = image.Dispatcher;

                    if (dispatchedHandler == null)
                    {
                        dispatchedHandler = delegate
                        {
                            var binding = new Binding();
                            binding.Path = new PropertyPath("Source");
                            binding.Source = image;
                            image.SetBinding(SourceProperty, binding);
                        };
                    }
                    dispatcher.RunAsync(CoreDispatcherPriority.High, dispatchedHandler);
                }
                else
                {
                    var fadeInOnLoadedHandler = GetFadeInOnLoadedHandler(d);
                    SetFadeInOnLoadedHandler(d, null);
                    fadeInOnLoadedHandler.Detach();
                    image.SetValue(SourceProperty, null);
                }
            }
        }

        public static FadeInOnLoadedHandler GetFadeInOnLoadedHandler(DependencyObject d)
            => d.GetValue(FadeInOnLoadedHandlerProperty) as FadeInOnLoadedHandler;
        public static void SetFadeInOnLoadedHandler(DependencyObject d, FadeInOnLoadedHandler value)
            => d.SetValue(FadeInOnLoadedHandlerProperty, value);

        public static object GetSource(DependencyObject d) => d.GetValue(SourceProperty);
        public static void SetSource(DependencyObject d, object value) => d.SetValue(SourceProperty, value);
        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (GetFadeInOnLoaded(d))
            {
                GetFadeInOnLoadedHandler(d)?.Detach();
                SetFadeInOnLoadedHandler(d, new FadeInOnLoadedHandler(d as Image));
            }
        }
    }
}