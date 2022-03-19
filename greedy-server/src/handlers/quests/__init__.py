from .claim_daily_quest import (CompleteDailyQuestHandler,
                                CompleteDailyQuestRequestModel,
                                CompleteDailyQuestResponse)
from .claim_merc_quest import (CompleteMercQuestHandler,
                               CompleteMercQuestResponse)
from .get_user_quests import GetUserQuestsHandler, GetUserQuestsResponse

"""
    Group static and user quests together as they are very tighly coupled
    
    Create handler get_user_quests to return game quests (as models) and quest progress
"""