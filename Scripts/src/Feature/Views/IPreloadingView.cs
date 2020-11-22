using UnityEngine;

namespace PreloadingModule.src.Views
{
    public interface IPreloadingView
    {
        GameObject GameObject { get; }
        void SetProgressText(string text);
        void SetDescriptionText(string text);
    }
}