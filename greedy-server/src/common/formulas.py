import math

from src import utils
from src.common.types import BonusType
from src.mongo.artefacts import ArtefactModel
from src.static_models.artefacts import StaticArtefact


def calculate_artefact_upgrade_cost(static_artefact: StaticArtefact, user_artefact: ArtefactModel, levels: int) -> int:
    base_cost = calculate_base_artefact_upgrade_cost(static_artefact, user_artefact, levels)

    return base_cost


def calculate_base_artefact_upgrade_cost(s_art: StaticArtefact, u_art: ArtefactModel, buying: int) -> int:
    return math.ceil(s_art.cost_coeff * sum_non_int_power_seq(u_art.level, buying, s_art.cost_expo))


def create_bonus_dict(ls: list[tuple[int, float]]) -> dict[int, float]:
    d: dict[int, float] = {}

    for bonus, value in ls:
        current: float = d.get(bonus)

        if current is None:
            d[bonus] = value

        else:
            d[bonus] = {

            }.get(
                bonus,
                current * value
            )

    return d


def create_artefacts_bonus_list(
    user_artefacts: list[ArtefactModel],
    static_artefacts: dict[int, StaticArtefact]
) -> list[tuple[int, float]]:
    """

    """
    ls: list[tuple[int, float]] = []

    for user_artefact in user_artefacts:
        static_artefact = static_artefacts.get(user_artefact.artefact_id)

        if static_artefact:
            bonus = artefact_base_effect(user_artefact, static_artefact)

            ls.append((static_artefact.bonus_type, bonus))

    return ls


def artefacts_bonus_product(
    bonus_type: BonusType, u_arts: list[ArtefactModel], s_arts: list[StaticArtefact]
):
    value = 1

    for art in u_arts:
        if s_art := utils.get(s_arts, id=art.artefact_id):
            if s_art.bonus_type == bonus_type:
                value *= artefact_base_effect(art, s_art)

    return value


def artefact_base_effect(u_art: ArtefactModel, s_art: StaticArtefact) -> float:
    return s_art.base_effect + (s_art.level_effect * (u_art.level - 1))


def artefact_upgrade_cost(s_art: StaticArtefact, current: int, buying: int) -> int:
    return math.ceil(s_art.cost_coeff * sum_non_int_power_seq(current, buying, s_art.cost_expo))


def base_points_at_stage(stage: int):
    return math.ceil(math.pow(math.ceil((max(stage, 75) - 75) / 10.0), 2.2))


def sum_non_int_power_seq(start: int, buying: int, s: float):
    def pred(startval: int):
        x = pow(startval, s + 1) / (s + 1)
        y = pow(startval, s) / 2
        z = math.sqrt(pow(startval, s - 1))

        return x + y + z

    return pred(start + buying) - pred(start)
