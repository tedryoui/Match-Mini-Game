using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ExplosionEffectUI : MonoBehaviour
    {
        [SerializeField] private Image _particlePrefab;
        [SerializeField] private int _particlesCount;

        [SerializeField] private Vector2Int _particleLifetime;
        [SerializeField] private float _distance;

        [SerializeField] private AnimationCurve _moveCurve;
        [SerializeField] private AnimationCurve _scaleCurve;
        [SerializeField] private AnimationCurve _fadeCurve;

        public void Explode()
        {
            // Instantiate and tween particles
            var particles = InstantiateParticles();
            TweenParticles(particles);
        }

        private void TweenParticles(List<Image> particle)
        {
            foreach (var image in particle)
            {
                // Reset particle visuals
                ResetParticle(image);
                
                // Compute main tween parameters
                var lifetime = UnityEngine.Random.Range(_particleLifetime.x, _particleLifetime.y);
                var direction = Quaternion.Euler(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360.0f)) * Vector3.right;
                
                var sequence = DOTween.Sequence();

                sequence.Append(image.rectTransform.DOLocalMove(direction * _distance, lifetime).SetEase(_moveCurve));
                sequence.Insert(0.0f, image.rectTransform.DOScale(Vector3.one, lifetime).SetEase(_scaleCurve));
                sequence.Insert(0.0f, image.DOFade(0.0f, lifetime).SetEase(_fadeCurve));
                
                DOVirtual.DelayedCall(lifetime, () => DestroyParticle(image));
            }
        }

        private void ResetParticle(Image particle)
        {
            particle.rectTransform.anchoredPosition = Vector2.zero;
            particle.rectTransform.localScale = Vector3.zero;
            particle.gameObject.SetActive(true);
        }

        private void DestroyParticle(Image particle)
        {
            Destroy(particle.gameObject);
        }

        private List<Image> InstantiateParticles()
        {
            var list = new List<Image>();

            for (int i = 0; i < _particlesCount; i++)
                list.Add(Instantiate(_particlePrefab, transform));
            
            return list;
        }
    }
}