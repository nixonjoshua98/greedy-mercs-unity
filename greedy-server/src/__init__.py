
import fastapi


def create_app():
    app = fastapi.FastAPI(openapi_url=None)

    return app
