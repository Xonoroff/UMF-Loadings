using UnityEngine;

namespace Scripts.src.Feature.Views
{
    [CreateAssetMenu(fileName = "LoadingViewConfig", menuName = "ScriptableObjects/LoadingViewConfig", order = 999)]
    public class LoadingViewConfig : ScriptableObject
    {
        public Sprite Background;
        
        [Space]
        public Sprite ProgressBarFiller;
        public Sprite ProgressBarBackground;
        public Sprite ProgressBarForeground;
    }
}