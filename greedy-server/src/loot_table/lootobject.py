import abc


class LootObject(abc.ABC):
    weight: int
    unique: bool
    always: bool

    def update(self, weight: int, unique: bool, always: bool):
        self.weight = weight
        self.unique = unique
        self.always = always
