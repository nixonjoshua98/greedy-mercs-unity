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
                float cellWidth = (GM.UI.ScreenSpace.Width - (m_Spacing.x * (constraintCount - 1))) / constraintCount;

                m_CellSize = new Vector3(cellWidth, cellSize.y);

                CalculateLayoutInputHorizontal();
                CalculateLayoutInputVertical();
            }
        }
    }
}
