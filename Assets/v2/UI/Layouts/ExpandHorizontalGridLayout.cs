using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

namespace GM.UI.Layouts
{
    public class ExpandHorizontalGridLayout : MonoBehaviour
    {
        [SerializeField] GridLayoutGroup Layout;

        public void UpdateCellSize()
        {
            if (Layout.constraint == Constraint.FixedColumnCount)
            {
                RectTransform rectTrans = GetComponent<RectTransform>();

                // X Spacing
                float cellWidth = (rectTrans.rect.width - (Layout.spacing.x * (Layout.constraintCount - 1))) / Layout.constraintCount;

                // Left + right padding
                cellWidth -= (Layout.padding.left + Layout.padding.right) / Layout.constraintCount;

                Layout.cellSize = new Vector3(cellWidth, Layout.cellSize.y);

                Layout.CalculateLayoutInputHorizontal();
            }
            else
            {
                Debug.LogWarning($"GridLayoutExpandable cannot be used with constraint '{Layout.constraint}'");
            }
        }
    }
}
