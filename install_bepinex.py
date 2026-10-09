import os
import requests
import zipfile
import shutil

# URL for BepInEx_x64_5.4.22.0.zip (Base for Valheim)
BEPINEX_URL = "https://github.com/BepInEx/BepInEx/releases/download/v5.4.22/BepInEx_x64_5.4.22.0.zip"

def install_bepinex(valheim_path):
    if not os.path.exists(valheim_path):
        print(f"Error: La ruta '{valheim_path}' no existe.")
        return

    # Check for existing BepInEx installation
    if os.path.exists(os.path.join(valheim_path, "BepInEx")):
        print("¡Advertencia! BepInEx parece estar ya instalado.")
        return

    # Download
    print("Descargando BepInEx...")
    response = requests.get(BEPINEX_URL, stream=True)
    zip_path = "bepinex_temp.zip"
    with open(zip_path, 'wb') as f:
        shutil.copyfileobj(response.raw, f)

    # Extract
    print("Extrayendo archivos...")
    with zipfile.ZipFile(zip_path, 'r') as zip_ref:
        zip_ref.extractall(valheim_path)
    
    os.remove(zip_path)
    print(f"BepInEx instalado exitosamente en: {valheim_path}")

if __name__ == "__main__":
    path = input("Introduce la ruta de instalación de Valheim: ")
    install_bepinex(path)
