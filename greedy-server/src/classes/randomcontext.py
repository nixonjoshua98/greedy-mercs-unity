import random


class RandomContext:
    def __init__(self, seed):
        self._seed = seed

        self._prev_state = random.getstate()

    def __enter__(self):
        random.seed(f"{self._seed}")

    def __exit__(self, exc_type, exc_val, exc_tb):
        random.setstate(self._prev_state)
