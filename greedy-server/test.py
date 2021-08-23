from dataclasses import dataclass


@dataclass(frozen=True)
class Item:
    weight: int


items = [
    Item(weight=5), Item(weight=5), Item(weight=5),
]

total_weight = sum(ele.weight for ele in items)

for ele in items:
    print(f"{ele} {round((ele.weight / total_weight) * 100, 2)}%")