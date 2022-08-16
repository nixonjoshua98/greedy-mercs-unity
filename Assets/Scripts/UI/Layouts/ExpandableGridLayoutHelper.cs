using UnityEngine;
using UnityEngine.UI;

namespace SRC.UI
{
    [ExecuteInEditMode]
    [ExecuteAlways]
    [RequireComponent(typeof(GridLayoutGroup))]
    [RequireComponent(typeof(RectTransform))]
    public class ExpandableGridLayoutHelper : MonoBehaviour
    {
        private enum Axis { X, Y };
        private enum RatioMode { Free, Fixed };

        private RectTransform RectTrans;
        private GridLayoutGroup Grid;

        [SerializeField] private Axis ExpandDirection;
        [SerializeField] private RatioMode ratioMode;

        [Tooltip("Used for 'RatioMode.Fixed'")]
        [SerializeField] private float cellRatio = 1;

        void Awake()
        {
            Grid = GetComponent<GridLayoutGroup>();
            RectTrans = GetComponent<RectTransform>();
        }

        private void FixedUpdate()
        {
            UpdateCellSize();
        }

        void OnRectTransformDimensionsChange()
        {
            if (Grid is not null)
                UpdateCellSize();
        }

        public void UpdateCellSize()
        {
            var count = Grid.constraintCount;

            if (ExpandDirection == Axis.X)
            {
                float spacing = (count - 1) * Grid.spacing.x;
                float contentSize = RectTrans.rect.width - Grid.padding.left - Grid.padding.right - spacing;
                float sizePerCell = contentSize / count;
                Grid.cellSize = new(sizePerCell, ratioMode == RatioMode.Free ? Grid.cellSize.y : sizePerCell * cellRatio);

            }
            else if (ExpandDirection == Axis.Y)
            {
                float spacing = (count - 1) * Grid.spacing.y;
                float contentSize = RectTrans.rect.height - Grid.padding.top - Grid.padding.bottom - spacing;
                float sizePerCell = contentSize / count;
                Grid.cellSize = new(ratioMode == RatioMode.Free ? Grid.cellSize.x : sizePerCell * cellRatio, sizePerCell);
            }
        }
    }
}
