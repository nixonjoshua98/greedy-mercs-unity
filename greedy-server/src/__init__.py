from fastapi import FastAPI


def create_app():
    return FastAPI(openapi_url=None)
