/*
    ===============
    IMPORTANT NOTES
    ===============

    GM.*.Models
        These classes act as (server) models as well as data structures. We should be very cautious when changing or adding properties
    
    Server armoury static data needs to be updated
    Evo -> Star Level

    Armoury Items
        - Level -> (level * (baseEffect * (1 + rating))) * grade?
        - Grade (B, C, D etc) cannot be changed - Gives base multiplier
        - Star Rating (Level) (1, 2, 3, 4, 5) can be improved - Improves per level improvement
 */