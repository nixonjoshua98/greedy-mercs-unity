using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace GM.UI
{
    [ExecuteInEditMode]
    [ExecuteAlways]
    [RequireComponent(typeof(GridLayoutGroup))]
    [RequireComponent(typeof(RectTransform))]
    public class AdjustGridLayoutCellSize : MonoBehaviour
    {
        private enum Axis { X, Y };

        private enum RatioMode { Free, Fixed };

        private RectTransform RectTrans;
        private GridLayoutGroup Grid;

        [SerializeField] private Axis ExpandDirection;
        [SerializeField] private RatioMode ratioMode;

        [ShowIf("ratioMode", RatioMode.Fixed)]
        [SerializeField] private float cellRatio = 1;

        private void Awake()
        {
            Grid = GetComponent<GridLayoutGroup>();
            RectTrans = GetComponent<RectTransform>();
        }

        private void OnRectTransformDimensionsChange()
        {
            if (Grid is not null)
                UpdateCellSize();
        }

#if UNITY_EDITOR
        private void FixedUpdate()
        {
            if (Grid is not null)
                UpdateCellSize();
        }
#endif

        private void UpdateCellSize()
        {
            var count = Grid.constraintCount;

            if (ExpandDirection == Axis.X)
            {
                float spacing = (count - 1) * Grid.spacing.x;
                float contentSize = RectTrans.rect.width - Grid.padding.left - Grid.padding.right - spacing;
                float sizePerCell = contentSize / count;
                Grid.cellSize = new Vector2(sizePerCell, ratioMode == RatioMode.Free ? Grid.cellSize.y : sizePerCell * cellRatio);

            }
            else if (ExpandDirection == Axis.Y)
            {
                float spacing = (count - 1) * Grid.spacing.y;
                float contentSize = RectTrans.rect.height - Grid.padding.top - Grid.padding.bottom - spacing;
                float sizePerCell = contentSize / count;
                Grid.cellSize = new Vector2(ratioMode == RatioMode.Free ? Grid.cellSize.x : sizePerCell * cellRatio, sizePerCell);
            }
        }
    }
}
