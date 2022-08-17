using SRC.Mercs.Controllers;
using SRC.Mercs.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SRC.Mercs
{
    public class MercSquadController : Core.GMMonoBehaviour
    {
        [SerializeField] private SRC.Scriptables.UnitFormation Formation;
        private float FormationOffsetX = 0;

        public GameManager GameManager;

        [Header("Events")]
        [HideInInspector] public UnityEvent<AbstractMercController> E_UnitSpawned = new();
        private readonly List<GameObject> UnitGameObjects = new();

        private void Awake()
        {
            AddMercToQueue(MercID.STONE_GOLEM);
        }

        public IEnumerator MoveUnitsToFormation(float duration, float offset = 0)
        {
            var initialPositions = Formation.RelativePositions.Select(pos => new Vector3(pos.x + FormationOffsetX, pos.y)).ToList();

            FormationOffsetX += offset;

            yield return this.Lerp(0, 1, duration, progress =>
            {
                for (int i = 0; i < UnitGameObjects.Count; i++)
                {
                    var unit = UnitGameObjects[i];
                    var relPos = Formation.RelativePositions[i];
                    var initialPos = initialPositions[i];

                    var controller = unit.GetComponent<AbstractMercController>();

                    controller.InControl = false;

                    Vector3 position = new(relPos.x + FormationOffsetX, relPos.y);

                    controller.Movement.LerpTowards(initialPos, position, progress);
                }
            });
        }

        public void SetControl(bool value)
        {
            UnitGameObjects.ForEach(unit =>
            {
                var controller = unit.GetComponent<AbstractMercController>();

                controller.InControl = value;
            });
        }

        private bool UnitExistsInQueue(MercID unit) => UnitGameObjects.Select(x => x.GetComponent<AbstractMercController>()).ToList().Exists(x => x.ID == unit);

        private void AddMercToQueue(MercID unitId)
        {
            AbstractMercController unit = InstantiateMerc(unitId);

            unit.Init(this, () => GameManager.EnemyUnits.FirstOrDefault(x => x));

            UnitGameObjects.Add(unit.gameObject);

            E_UnitSpawned.Invoke(unit);
        }

        private AbstractMercController InstantiateMerc(MercID unitId)
        {
            var data = App.Mercs.GetMerc(unitId);

            GameObject o = Instantiate(data.Prefab, Formation.RelativePositions[0] + new Vector2(FormationOffsetX, 0), Quaternion.identity);

            return o.GetComponent<AbstractMercController>();
        }
    }
}