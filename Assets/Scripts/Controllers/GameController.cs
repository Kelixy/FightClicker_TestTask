using UnityEngine;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private TutorialController tutorialController;
        [SerializeField] private FightAnimationsController fightController;

        private bool _tutorialIsOn = true;

        private void Start()
        {
            tutorialController.ShowTutorial();
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (_tutorialIsOn)
                {
                    tutorialController.StopTutorial();
                    _tutorialIsOn = false;
                    return;
                }

                if (fightController.AnimationsArePlaying)
                {
                    return;
                }

                fightController.SwitchFighters();
                fightController.Hit();
            }
        }
    }
}
