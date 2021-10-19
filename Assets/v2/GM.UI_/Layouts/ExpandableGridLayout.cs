using UnityEngine;
using UnityEngine.UI;

namespace GM.UI_.Layouts
{
    public class ExpandableGridLayout : GridLayoutGroup
    {
        public void UpdateCellSize()
        {
            if (constraint == Constraint.FixedColumnCount)
            {
                float cellWidth = (ScreenSpace.Width / constraintCount) - (m_Spacing.x / constraintCount);

                m_CellSize = new Vector3(cellWidth, cellSize.y);

                CalculateLayoutInputHorizontal();
                CalculateLayoutInputVertical();
            }
        }
    }
}
