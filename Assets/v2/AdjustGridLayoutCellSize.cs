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
        enum Axis { X, Y };
        enum RatioMode { Free, Fixed };

        RectTransform RectTrans;
        GridLayoutGroup Grid;

        [SerializeField] Axis ExpandDirection;
        [SerializeField] RatioMode ratioMode;

        [ShowIf("ratioMode", RatioMode.Fixed)]
        [SerializeField] float cellRatio = 1;

        void Awake()
        {
            Grid = GetComponent<GridLayoutGroup>();
            RectTrans = GetComponent<RectTransform>();
        }

        void OnRectTransformDimensionsChange()
        {
            if (Grid is not null)
                UpdateCellSize();
        }

#if UNITY_EDITOR
        void FixedUpdate()
        {
            if (Grid is not null)
                UpdateCellSize();
        }
#endif

        void UpdateCellSize()
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
