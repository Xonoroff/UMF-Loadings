using PreloadingModule.src.Views;
using UnityEngine;

namespace Scripts.src.Feature.Entities
{
    public class PreloadingModuleConfig : ScriptableObject
    {
        public const string FILE_NAME = "PreloadingSystemModuleConfig";
        
        [SerializeField]
        private GameObject preloadingView;

        public IPreloadingView GetPreloadingView()
        {
            return preloadingView.GetComponent<PreloadingView>();
        }
    }
}
