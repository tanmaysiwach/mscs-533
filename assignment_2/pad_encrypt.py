import os

def xor_bytes(data: bytes, key: bytes) -> bytes:
    return bytes([b ^ k for b, k in zip(data, key)])

def to_hex(b: bytes) -> str:
    return b.hex().upper()

plaintext = "MY NAME IS UNKNOWN"
plain_bytes = plaintext.encode('ascii')

key = os.urandom(len(plain_bytes))
ciphertext = xor_bytes(plain_bytes, key)
decrypted = xor_bytes(ciphertext, key).decode('ascii')

print(f"Plaintext: {plaintext}")
print(f"Key (hex): {to_hex(key)}")
print(f"Ciphertext (hex): {to_hex(ciphertext)}")
print(f"Decrypted: {decrypted}")