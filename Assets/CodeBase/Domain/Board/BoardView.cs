using CodeBase.Domain.Dice;
using CodeBase.Services.Interaction;
using UnityEngine;

namespace CodeBase.Domain.Board
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private RectTransform _diceRoot;
        [SerializeField] private DiceViewFactory _diceViewFactory;
        
        private DiceView _diceView;

        public void ShowDice(
            DiceStateType stateType,
            DicePointType pointType,
            DiceDragDropController dragDropController,
            bool isInteractable)
        {
            EnsureDiceView();

            _diceView.Render(stateType, pointType);
            _diceView.Bind(isInteractable ? dragDropController : null);
        }

        private void EnsureDiceView()
        {
            if (_diceView != null)
                return;

            _diceView = _diceViewFactory.CreateDice(_diceRoot);
        }
        
        public DiceView DetachDiceView()
        {
            DiceView result = _diceView;
            _diceView = null;
            return result;
        }
        

        public void ClearDiceView()
        {
            if (_diceView == null)
                return;

            Destroy(_diceView.gameObject);
            _diceView = null;
        }
    }
}