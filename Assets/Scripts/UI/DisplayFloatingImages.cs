using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class DisplayFloatingImages : MonoBehaviour
    {
        [SerializeField] private Image _prefab;
        [SerializeField] private float _range;

        [SerializeField] private float _scaleInDuration;
        [SerializeField] private Ease _scaleInEase;
        
        [FormerlySerializedAs("_scaleOutDuration")] [SerializeField] private float _fadeOutDuration;
        [FormerlySerializedAs("_scaleOutEase")] [SerializeField] private Ease _fadeOutEase;

        public void Show(int count)
        {
            var rect = (transform as RectTransform).rect;
            var rndX = UnityEngine.Random.Range(rect.xMin, rect.xMax);
            var rndY = UnityEngine.Random.Range(rect.yMin, rect.yMax);
            
            ShowGroup(new Vector2(rndX, rndY) + (Vector2)transform.position, count);
        }

        private void ShowGroup(Vector2 position, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var offset = new Vector2(UnityEngine.Random.Range(0, _range), UnityEngine.Random.Range(0, _range));

                var image = Instantiate(_prefab, transform);

                image.transform.position = position + offset;
                AnimateImage(image);
            }
        }

        private void AnimateImage(Image image)
        {
            image.rectTransform.localScale = Vector3.zero;

            var sequence = DOTween.Sequence();

            sequence.Append(image.rectTransform.DOShakePosition(10.0f, 15f, 1).SetEase(Ease.InOutSine));
            sequence.Insert(0.0f, image.rectTransform.DOScale(Vector3.one, _scaleInDuration).SetEase(_scaleInEase));
            sequence.Append(image.DOFade(0.0f, _fadeOutDuration).SetEase(_fadeOutEase));

            sequence.OnComplete(async () =>
            {
                await Task.Yield();
                Destroy(image.gameObject);
            });
            
            sequence.Play();
        }
    }
}