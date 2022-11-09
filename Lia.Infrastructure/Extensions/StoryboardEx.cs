using Lia.Utils;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace Lia.Extensions
{
    public static class StoryboardEx
    {
        public static async Task BeginAsync(this Storyboard storyboard)
        {
            await EventAsync.FromEvent(delegate (EventHandler<object> completed) { storyboard.Completed += completed; },
                                       delegate (EventHandler<object> completed) { storyboard.Completed -= completed; },
                                       storyboard.Begin);
        }
    }
}