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
                // X Spacing
                float cellWidth = (ScreenSpace.Width - (m_Spacing.x * (constraintCount - 1))) / constraintCount;

                // Left + right padding
                cellWidth -= (m_Padding.left + m_Padding.right) / constraintCount;

                m_CellSize = new Vector3(cellWidth, cellSize.y);

                CalculateLayoutInputHorizontal();
                CalculateLayoutInputVertical();
            }
            else
            {
                Debug.LogWarning($"ExpandableGridLayout cannot be used with constraint '{constraint}'");
            }
        }
    }
}
