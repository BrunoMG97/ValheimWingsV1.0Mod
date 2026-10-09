import os
import requests
import zipfile
import shutil

def install_bepinex(valheim_path, download_url):
    if not os.path.exists(valheim_path):
        print(f"Error: La ruta '{valheim_path}' no existe.")
        return

    # Ruta temporal para el archivo descargado
    zip_path = os.path.join(os.getcwd(), "bepinex_temp.zip")
    
    print(f"Descargando BepInEx desde {download_url}...")
    try:
        response = requests.get(download_url, stream=True)
        response.raise_for_status()
        with open(zip_path, 'wb') as f:
            shutil.copyfileobj(response.raw, f)
        
        print("Extrayendo archivos en:", valheim_path)
        with zipfile.ZipFile(zip_path, 'r') as zip_ref:
            zip_ref.extractall(valheim_path)
        
        os.remove(zip_path)
        print("Instalación completada exitosamente.")
    except Exception as e:
        print(f"Error durante la instalación: {e}")

if __name__ == "__main__":
    # Preguntar ruta y URL
    valheim_path = input("Introduce la ruta de instalación de Valheim (ej. C:\\STEAMAPP\\steamapps\\common\\Valheim): ")
    url = input("Por favor, introduce la URL de descarga directa del .zip de BepInEx: ")
    install_bepinex(valheim_path, url)
