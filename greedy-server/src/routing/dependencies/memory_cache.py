from fastapi import Request


def inject_memory_cache(request: Request):
    return request.app.state.memory_cache
