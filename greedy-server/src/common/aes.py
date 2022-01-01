import base64

# noinspection PyPackageRequirements
from Crypto.Cipher import AES

_key = "1234567895645451".encode('ascii')
_key_len = len(_key)


def encrypt(text):
    aes = AES.new(_key, AES.MODE_CBC)

    padded_text = bytes(_pkcs7encode(text), encoding="UTF-8")

    cipher = aes.encrypt(padded_text)

    return base64.b64encode(aes.iv + cipher).decode("UTF-8")


def decrypt(ciphertext):
    decoded = base64.b64decode(ciphertext)

    iv, text = decoded[:_key_len], decoded[_key_len:]

    aes = AES.new(_key, AES.MODE_CBC, iv)

    return _pkcs7decode(aes.decrypt(text))


def _pkcs7decode(text):
    return text[:(len(text) - text[-1])].decode("UTF-8")


def _pkcs7encode(text):
    val = _key_len - ((len(text)) % _key_len)
    return text + bytearray([val] * val).decode("UTF-8")
