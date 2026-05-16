using System.Collections.Generic;
using CodeBase.Domain.Field.Cell;
using UnityEngine;

namespace CodeBase.Domain.Field.View
{
    [CreateAssetMenu(fileName = "FieldViewFactory", menuName = "Field/FieldViewFactory", order = 1)]
    public class FieldColumnViewFactory : ScriptableObject
    {
        [SerializeField] private FieldColumnView _fieldColumnViewPrefab;
        [SerializeField] private CellView _cellViewPrefab;

        public FieldColumnView Create(
            RectTransform parent,
            int columnIndex,
            int rowsCount,
            ScoreTextPosition scoreTextPosition)
        {
            FieldColumnView columnView = Instantiate(_fieldColumnViewPrefab, parent);

            var cells = new List<CellView>(rowsCount);

            for (int row = 0; row < rowsCount; row++)
            {
                CellView cellView = Instantiate(_cellViewPrefab, columnView.CellRoot);
                cellView.Initialize(new CellPosition(columnIndex, row));
                cells.Add(cellView);
            }

            columnView.Build(cells, scoreTextPosition);
            return columnView;
        }
    }
}