import functools as ft


def verify_kwargs(*allowed_keys):
    """ Decorator

    Verify that all 'kwargs' keys are within the 'allowed_keys' tuple
    """

    def decorator(func):

        @ft.wraps(func)
        def wrapper(*args, **kwargs):

            if any(k not in allowed_keys for k in kwargs.keys()):
                raise KeyError(f"Key not allowed for function {func.__name__}")

            return func(*args, **kwargs)

        return wrapper

    return decorator
