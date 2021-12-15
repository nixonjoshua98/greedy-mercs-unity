import abc


class LootObject(abc.ABC):
    def __init__(self, weight: int, unique: bool, always: bool):
        self.weight = weight
        self.unique = unique
        self.always = always
