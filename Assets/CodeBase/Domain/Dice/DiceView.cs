using CodeBase.Services.Interaction;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase.Domain.Dice
{
    public class DiceView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private DiceViewContent _diceViewContent;
        [SerializeField] private Image _stateImage;
        [SerializeField] private Image _pointImage;
        
        private DiceDragDropController _dragDropController;
        
        public void Bind(DiceDragDropController dragDropController)
        {
            _dragDropController = dragDropController;
        }

        public void Render(DiceStateType stateType, DicePointType pointType)
        {
            SetStateSprite(stateType);
            SetPointSprite(pointType);
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            //Debug.Log("BeginDrag");
    
            if (_dragDropController == null)
                return;

            _dragDropController.BeginDrag(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log("Dragging");

            if (_dragDropController == null)
                return;

            _dragDropController.Drag(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
           // Debug.Log("EndDrag");

            if (_dragDropController == null)
                return;

            _dragDropController.EndDrag(eventData.position);
        }

        private void SetStateSprite(DiceStateType state) =>
            _stateImage.sprite = _diceViewContent.GetSprite(state);

        private void SetPointSprite(DicePointType pointType) =>
            _pointImage.sprite = _diceViewContent.GetSprite(pointType);
    }
}