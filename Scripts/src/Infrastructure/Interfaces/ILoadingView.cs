using UnityEngine;

namespace Scripts.src.Feature.Views
{
    public interface ILoadingView
    {
        GameObject GameObject { get; }
        void SetViewEntity(LoadingViewEntity loadingViewEntity);
        void SetConfig(LoadingViewConfig loadingViewConfig);
    }
}