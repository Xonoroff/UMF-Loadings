﻿using TMPro;
using UnityEngine;

namespace Scripts.src.Feature.Views
{
    public class LoadingView : MonoBehaviour, ILoadingView
    {
        [SerializeField]
        private TextMeshProUGUI headerLabel;
        
        [SerializeField]
        private TextMeshProUGUI descriptionLabel;
        
        [SerializeField]
        private TextMeshProUGUI progressLabel;
        
        public void SetPreloaderHeader(string header)
        {
            if (headerLabel != null)
            {
                headerLabel.text = header;
            }
        }

        public void SetDescriptionText(string totalText)
        {
            if (descriptionLabel != null)
            {
                descriptionLabel.text = totalText;
            }
        }

        public void SetProgressText(string text)
        {
            if (progressLabel != null)
            {
                progressLabel.text = text;
            }
        }

        public GameObject GameObject => this.gameObject;
    }
}