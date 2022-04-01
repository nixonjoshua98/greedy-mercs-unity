using UnityEngine;
using UnityEngine.UI;

namespace GM.UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(GridLayoutGroup))]
    public class AdjustGridLayoutCellSize : MonoBehaviour
    {
        public enum Axis { X, Y };

        [SerializeField] Axis expand;

        [SerializeField] RectTransform RectTransform;
        [SerializeField] GridLayoutGroup Grid;

        void Start()
        {
            UpdateCellSize();
        }

        void OnRectTransformDimensionsChange()
        {
            UpdateCellSize();
        }

#if UNITY_EDITOR
        [ExecuteAlways]
        void Update()
        {
            UpdateCellSize();
        }
#endif

        void OnValidate()
        {
            UpdateCellSize();
        }

        void UpdateCellSize()
        {
            var count = Grid.constraintCount;

            if (expand == Axis.X)
            {
                float spacing = (count - 1) * Grid.spacing.x;
                float contentSize = RectTransform.rect.width - Grid.padding.left - Grid.padding.right - spacing;
                float sizePerCell = contentSize / count;
                Grid.cellSize = new Vector2(sizePerCell, Grid.cellSize.y);
            }

            else if (expand == Axis.Y)
            {
                float spacing = (count - 1) * Grid.spacing.y;
                float contentSize = RectTransform.rect.height - Grid.padding.top - Grid.padding.bottom - spacing;
                float sizePerCell = contentSize / count;
                Grid.cellSize = new Vector2(Grid.cellSize.x, sizePerCell);
            }
        }
    }
}
