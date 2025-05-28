# 📝 StoryWriterCLI

StoryWriter es una aplicación de consola multiplataforma desarrollada en .NET Core para escribir, editar, organizar y exportar historias en formato Markdown (`.md`). Ideal para escritores que desean trabajar desde la terminal con una experiencia fluida, ordenada y productiva.

---

## ✨ Características

- 📂 Sistema de carpetas para organizar tus historias
- 🖊️ Editor en vivo con comandos (`--save`, `--exit`, etc.)
- 🧠 Metadatos por historia (título, fecha, etiquetas)
- 📖 Lector con paginación y resaltado básico de Markdown
- 🔎 Filtro de historias por título y etiquetas
- 📚 Índice automatizado (`index.json`) con navegación tipo biblioteca
- 🌐 Exportación a HTML y PDF (experimental)
- 💾 Publicación como ejecutable multiplataforma (`.exe`, `.bin`, etc.)

---

## 🚀 Instalación

### Ejecutar desde código fuente

Requisitos:

* [.NET SDK 8+](https://dotnet.microsoft.com/en-us/download)

```bash
git clone https://github.com/FeithNoir/StoryWriter.git
cd StoryWriter
dotnet run
```

---

## 🧪 Uso básico

### Menú principal

```text
1. Nueva Historia
2. Editar Historia
3. Cargar Historia
4. Crear Carpeta
5. Seleccionar Carpeta
6. Ver Biblioteca
7. Salir
```

### Comandos dentro del editor

* `--save` → Guardar historia
* `--exit` → Salir del editor sin guardar
* `--menu` → Volver al menú principal con confirmación

---

## 💡 Ejemplo de metadatos en `.md`

```markdown
---
Title: El Hada del Fuego
CreatedAt: 2025-05-28
Tags: fantasía, romance, conflicto
---

Aquí comienza tu historia...
```

Los metadatos se procesan automáticamente al leer o indexar historias.

---

## 📦 Empaquetado multiplataforma

Compila para tu sistema:

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

Plataformas disponibles:

* `win-x64`, `linux-x64`, `osx-x64`, `osx-arm64`

Instalador disponible para Windows con [Inno Setup](https://jrsoftware.org/isinfo.php).

---

## 📄 Licencia

Este proyecto se distribuye bajo la licencia [MIT](LICENSE).

---

## 👤 Autor

Creado con amor por **Feith Noir**
🔗 [github.com/FeithNoir](https://github.com/FeithNoir)

---

¡Feliz escritura, coleguis! 🌙✨

```
