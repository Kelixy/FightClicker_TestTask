using DG.Tweening;
using UnityEngine;

namespace Controllers
{
    public class TutorialController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform instructionRectTransform;
        [SerializeField] private Vector3 pulsedTutorialTextScale;

        private Sequence _pulseSequence;

        private void StartPulse()
        {
            _pulseSequence = DOTween.Sequence()
                .Append(instructionRectTransform.DOScale(pulsedTutorialTextScale, 1).SetEase(Ease.InOutElastic))
                .Append(instructionRectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.Flash))
                .SetLoops(-1);
        }

        public void ShowTutorial()
        {
            DOTween.Sequence()
                .Append(canvasGroup.DOFade(1, 1))
                .AppendCallback(StartPulse);
        }

        public void StopTutorial()
        {
            DOTween.Sequence().Append(canvasGroup.DOFade(0, 1));
            _pulseSequence.Kill();
        }
    }
}
