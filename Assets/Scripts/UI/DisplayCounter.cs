using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DisplayCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private int _lastInteger;

        private readonly string _counterTweenDefaultId = "Counter-Tween";
        
        public void UpdateCounter(int amount)
        {
            DOTween.Kill(_counterTweenDefaultId);

            var tween = DOVirtual.Int(_lastInteger, amount, 1.0f, (i) => _lastInteger = i);
            tween.SetId(_counterTweenDefaultId);
            tween.OnComplete(() => _lastInteger = amount);
        }

        private void Update()
        {
            _text.SetText(_lastInteger.ToString());
        }
    }
}