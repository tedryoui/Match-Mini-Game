using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mini_Game
{
    public class MiniGameReward : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _amount;

        [SerializeField] private Ease _fadeInEase;
        [SerializeField] private float _fadeInDuration;
        [SerializeField] private Ease _fadeOutEase;
        [SerializeField] private float _fadeOutDuration;

        public void Open(Sprite icon, string message)
        {
            _amount.SetText(message);
            
            SmoothFadeIn();
        }

        public void SmoothFadeIn()
        {
            _canvasGroup.alpha = 0.0f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            _canvasGroup.DOFade(1.0f, _fadeInDuration).SetEase(_fadeInEase);
        }

        public void SmoothFadeOut()
        {
            _canvasGroup.alpha = 1.0f;

            _canvasGroup.DOFade(0.0f, _fadeOutDuration).SetEase(_fadeOutEase).OnComplete(() =>
            {
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
            });
        }
    }
}