using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mini_Game
{
    public class MiniGameNodeObject : MonoBehaviour, IPointerClickHandler
    {
        [Header("Reference")]
        [SerializeField] private Image _notifyImage;
        [SerializeField] private Image _successImage;
        [SerializeField] private Image _errorImage;
        [Space(5)] 
        [SerializeField] private Image _icon;
        [SerializeField] private ExplosionEffectUI _explosionEffect;
        
        [Header("Notify Tween Settings")]
        [SerializeField] private float _notifyPulseInDuration;
        [SerializeField] private float _notifyPulseOutDuration;
        [SerializeField] private float _notifyPulseDuration;
        
        [Header("Error Tween Settings")]
        [SerializeField] private float _errorPulseInDuration;
        [SerializeField] private float _errorPulseOutDuration;
        [SerializeField] private float _errorPulseDuration;
        
        [Header("Success Tween Settings")]
        [SerializeField] private float _successLoopInDuration;
        [SerializeField] private float _successLoopOutDuration;
        [SerializeField] private float _successPulseDuration;

        [Header("Current Icon Tween Settings")] 
        [SerializeField] private float _currentDuration;
        [SerializeField] private Vector3 _currentFinalScale;
        [SerializeField] private float _currentPunchStrength;
        [SerializeField] private float _currentPunchDuration;
        [SerializeField] private Ease _currentEase;
        
        [Header("Next Icon Tween Settings")] 
        [SerializeField] private float _nextDuration;
        [SerializeField] private Vector3 _nextFinalScale;
        [SerializeField] private float _nextPunchStrength;
        [SerializeField] private float _nextPunchDuration;
        [SerializeField] private Ease _nextEase;

        [Header("Explosion Tween Settings")] 
        [SerializeField] private float _explosionPunchStrength;
        [SerializeField] private float _explosionFadeDuration;
        [SerializeField] private Ease _explosionEase;

        private string _highlightTweenDefaultId => $"Highlight-Tween {gameObject.GetInstanceID()}";
        private string _iconTweenDefaultId => $"Icon-Tween {gameObject.GetInstanceID()}";

        #region Overlap Images Tweens

        public Tween Notify()
        {
            StopHighlightTween();
            
            var sequence = DOTween.Sequence();
            
            sequence.SetId(_highlightTweenDefaultId);

            sequence.Append(_notifyImage.DOFade(1.0f, _notifyPulseInDuration).SetEase(Ease.InExpo));
            sequence.AppendInterval(_notifyPulseDuration);
            sequence.Append(_notifyImage.DOFade(0.0f, _notifyPulseOutDuration).SetEase(Ease.OutExpo));

            sequence.OnStart(DisableNotify);
            sequence.OnComplete(DisableNotify);
            
            sequence.Play();

            return sequence;
        }

        private void DisableNotify()
        {
            _notifyImage.color = new Color(_notifyImage.color.r, _notifyImage.color.g, _notifyImage.color.b, 0.0f);
        }

        public Tween Success()
        {
            StopHighlightTween();
            
            var sequence = DOTween.Sequence();
            
            sequence.SetId(_highlightTweenDefaultId);

            sequence.Append(_successImage.DOFade(1.0f, _successLoopInDuration).SetEase(Ease.InExpo));
            sequence.AppendInterval(_successPulseDuration);
            sequence.Append(_successImage.DOFade(0.0f, _successLoopOutDuration).SetEase(Ease.OutExpo));

            sequence.OnStart(DisableSuccess);
            sequence.OnComplete(DisableSuccess);
            sequence.SetLoops(-1, LoopType.Yoyo);
            
            sequence.Play();

            return sequence;
        }

        private void DisableSuccess()
        {
            _successImage.color = new Color(_successImage.color.r, _successImage.color.g, _successImage.color.b, 0.0f);
        }

        public Tween Error()
        {
            StopHighlightTween();
            
            var sequence = DOTween.Sequence();
            
            sequence.SetId(_highlightTweenDefaultId);

            sequence.Append(_errorImage.DOFade(1.0f, _errorPulseInDuration).SetEase(Ease.InExpo));
            sequence.AppendInterval(_errorPulseDuration);
            sequence.Append(_errorImage.DOFade(0.0f, _errorPulseOutDuration).SetEase(Ease.OutExpo));

            sequence.OnStart(DisableError);
            sequence.OnComplete(DisableError);
            
            sequence.Play();

            return sequence;
        }

        private void DisableError()
        {
            _errorImage.color = new Color(_errorImage.color.r, _errorImage.color.g, _errorImage.color.b, 0.0f);
        }
        
        public void StopHighlightTween()
        {
            DOTween.Kill(_highlightTweenDefaultId, true);
            
            DisableError();
            DisableNotify();
            DisableSuccess();
        }

        #endregion

        #region Icon Images Tweens

        public Tween ShowCurrent(float delay = 0)
        {
            StopIconTween();
            
            var sequence = DOTween.Sequence();

            sequence.SetId(_iconTweenDefaultId);

            sequence.Append(_icon.transform.DOScale(_currentFinalScale, _currentDuration));
            sequence.Append(_icon.transform.DOPunchScale(Vector3.one * _currentPunchStrength, _currentPunchDuration, 5, 3));
            sequence.SetDelay(delay);
            
            sequence.SetEase(_currentEase);
            
            sequence.OnComplete(EnableIcon);

            sequence.Play();

            return sequence;
        }

        public Tween ShowNext(float delay = 0)
        {
            StopIconTween();
            
            var sequence = DOTween.Sequence();

            sequence.SetId(_iconTweenDefaultId);

            sequence.Append(_icon.transform.DOScale(_nextFinalScale, _nextDuration));
            sequence.Append(_icon.transform.DOPunchScale(Vector3.one * _nextPunchStrength, _nextPunchDuration, 5, 3));
            sequence.SetDelay(delay);
            
            sequence.SetEase(_nextEase);
            
            sequence.OnStart(DisableIcon);

            sequence.Play();

            return sequence;
        }

        public Tween ShowExplosion(float delay = 0)
        {
            StopIconTween();
            
            var sequence = DOTween.Sequence();

            sequence.SetId(_iconTweenDefaultId);

            sequence.Append(_icon.transform.DOScale(Vector3.one * _explosionPunchStrength, 0.25f).SetEase(Ease.InOutBack));
            sequence.Append(_icon.transform.DOScale(0.0f, _explosionFadeDuration).SetEase(_explosionEase));
            
            sequence.OnStart(() => _explosionEffect.Explode());

            sequence.Play();

            return sequence;
        }

        public void StopIconTween()
        {
            DOTween.Kill(_iconTweenDefaultId, true);
        }

        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }
        
        public void DisableIcon()
        {
            _icon.transform.localScale = Vector3.zero;
        }
        
        public void EnableIcon()
        {
            _icon.transform.localScale = Vector3.one;
        }

        #endregion
        
        [Header("Callbacks"), Space(10)]
        public UnityEvent clicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            clicked?.Invoke();
        }
    }
}