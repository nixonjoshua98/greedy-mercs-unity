from random import Random

from lootitem import LootItem

from loottable import LootTable

t = LootTable()

for i in range(3):
    t.add_item(i)

ai = LootTable()
ai.add_item("Armoury Item 0")
ai.add_item("Armoury Item 1")
ai.add_item("Unique Armoury Item", unique=True)

t.add_item(ai, weight=10)

print(t.get_items(10, rnd=Random()))
