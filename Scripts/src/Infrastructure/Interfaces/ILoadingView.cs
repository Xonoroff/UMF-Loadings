using UnityEngine;

namespace Scripts.src.Feature.Views
{
    public interface ILoadingView
    {
        GameObject GameObject { get; }
        void SetProgressText(string text);
        void SetDescriptionText(string text);
    }
}