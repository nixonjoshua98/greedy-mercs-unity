using UnityEngine;
using UnityEngine.UI;

namespace GM.UI.Layouts
{
    public class ExpandableGridLayout : GridLayoutGroup
    {
        public void UpdateCellSize()
        {
            if (constraint == Constraint.FixedColumnCount)
            {
                float cellWidth = (GM.UI_.ScreenSpace.Width / constraintCount) - (m_Spacing.x / constraintCount);

                m_CellSize = new Vector3(cellWidth, cellSize.y);

                CalculateLayoutInputHorizontal();
                CalculateLayoutInputVertical();
            }
        }
    }
}
