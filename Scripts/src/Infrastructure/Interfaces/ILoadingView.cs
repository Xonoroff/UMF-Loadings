using DefaultNamespace;
using UnityEngine;

namespace Scripts.src.Feature.Views
{
    public interface ILoadingView : IView<LoadingViewEntity>
    {
        void SetConfig(LoadingViewConfig loadingViewConfig);
    }
}