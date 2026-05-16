using System.Collections.Generic;
using CodeBase.Domain;
using CodeBase.Domain.Dice;
using CodeBase.Services.Turn;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeBase.Services.Interaction
{
    public class DiceDragDropController : MonoBehaviour
    {
        
        [SerializeField] private RectTransform _dragRoot;
        
        private LocalHumanPlayerController _localController;
        private DiceDragContext _dragContext;

        private readonly List<RaycastResult> _raycastResults = new();

        public void Init(LocalHumanPlayerController localController)
        {
            if (_localController != null)
            {
                _localController.PlacementRejected -= OnPlacementRejected;
                _localController.PlacementAccepted -= OnPlacementAccepted;
            }

            _localController = localController;

            if (_localController != null)
            {
                _localController.PlacementRejected += OnPlacementRejected;
                _localController.PlacementAccepted += OnPlacementAccepted;
            }
        }

        public void BeginDrag(DiceView view)
        {
            
            if (_localController == null)
            {
                Debug.LogError("DiceDragDropController is not initialized.");
                return;
            }

            if (!_localController.IsWaitingForPlacement)
                return;

            if (_dragContext != null)
                return;

            var rectTransform = view.transform as RectTransform;
            var canvasGroup = view.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = view.gameObject.AddComponent<CanvasGroup>();

            _dragContext = new DiceDragContext
            {
                
                DiceView = view,
                OriginalParent = view.transform.parent,
                OriginalSiblingIndex = view.transform.GetSiblingIndex(),
                SourcePosition = null,
                CanvasGroup = canvasGroup,
                OriginalAnchoredPosition = rectTransform != null ? rectTransform.anchoredPosition : Vector2.zero
            };

            canvasGroup.blocksRaycasts = false;
            view.transform.SetParent(_dragRoot, true);
            view.transform.SetAsLastSibling();
        }

        public void Drag(Vector2 screenPosition)
        {
            if (_dragContext == null)
                return;

            if (_dragContext.DiceView.transform is not RectTransform rectTransform)
                return;

            rectTransform.position = screenPosition;
        }

        public void EndDrag(Vector2 screenPosition)
        {
            if (_dragContext == null)
                return;

            if (TryGetDropCell(screenPosition, out var targetInfo) &&
                targetInfo.Slot == PlayerSlot.Local &&
                _localController.TrySubmitPlacement(targetInfo.CellPosition))
            {
                if (_dragContext.CanvasGroup != null)
                    _dragContext.CanvasGroup.blocksRaycasts = true;

                return;
            }

            RestoreDraggedView();
            ClearDrag();
        }

        private bool TryGetDropCell(Vector2 screenPosition, out DiceDropTargetInfo targetInfo)
        {
            targetInfo = default;

            if (EventSystem.current == null)
                return false;

            var eventData = new PointerEventData(EventSystem.current)
            {
                position = screenPosition
            };

            _raycastResults.Clear();
            EventSystem.current.RaycastAll(eventData, _raycastResults);

            foreach (var result in _raycastResults)
            {
                var dropTarget = result.gameObject.GetComponentInParent<IDiceDropCellTarget>();
                if (dropTarget == null)
                    continue;

                targetInfo = dropTarget.TargetInfo;
                return true;
            }

            return false;
        }

        private void RestoreDraggedView()
        {
            if (_dragContext == null)
                return;

            if (_dragContext.CanvasGroup != null)
                _dragContext.CanvasGroup.blocksRaycasts = true;

            _dragContext.DiceView.transform.SetParent(_dragContext.OriginalParent, false);
            _dragContext.DiceView.transform.SetSiblingIndex(_dragContext.OriginalSiblingIndex);

            if (_dragContext.DiceView.transform is RectTransform rectTransform)
                rectTransform.anchoredPosition = _dragContext.OriginalAnchoredPosition;
        }
        
        private void OnPlacementRejected()
        {
            RestoreDraggedView();
            ClearDrag();
        }

        private void OnPlacementAccepted()
        {
            if (_dragContext?.CanvasGroup != null)
                _dragContext.CanvasGroup.blocksRaycasts = true;

            ClearDrag();
        }

        private void ClearDrag() => _dragContext = null;
    }
}