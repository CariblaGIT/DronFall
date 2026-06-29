# DronFall

Roguelike/dungeon-crawler 2D top-down hecho en Unity. Controlas un dron que se mueve y dispara en direcciones independientes mientras explora mazmorras generadas proceduralmente, elimina enemigos y recoge ítems que mejoran sus estadísticas.

## Características

- **Generación procedural de mazmorras**: cada nivel se construye a partir de un "crawler" que camina aleatoriamente por una grid, generando un número variable de habitaciones (configurable por nivel mediante un `DungeonGenerationData`).
- **Niveles temáticos independientes**: `EntranceHall`, `Reactor` y `Underground`, cada uno con sus propias salas de inicio, salas genéricas y sala final (boss room).
- **Habitaciones conectadas por puertas**: las puertas se abren o se ocultan automáticamente según si hay una sala vecina, y se bloquean mientras queden enemigos vivos en la sala.
- **Combate con doble dirección**: el movimiento y el disparo usan ejes independientes, permitiendo moverse y disparar en direcciones distintas al mismo tiempo (twin-stick).
- **Enemigos con IA por estados**: `Idle → Follow → Attack → Death`, con distintos tipos de dron (a distancia y cuerpo a cuerpo).
- **Ítems de mejora**: al recogerlos aumentan vida, velocidad de movimiento, alcance, cadencia o tamaño de disparo.
- **Interfaz**: barra de vida dinámica y menú principal con inicio de partida y salida del juego.

## Requisitos

- **Unity 2021.1.29f1** (misma versión exacta recomendada; instálala desde Unity Hub si no la tienes).
- Windows (proyecto configurado y probado para build `StandaloneWindows64`).

## Instalación

1. Clona el repositorio:
   ```
   git clone https://github.com/CariblaGIT/DronFall.git
   ```
2. Abre **Unity Hub** → `Add` → selecciona la carpeta del proyecto clonado.
3. Si no tienes la versión 2021.1.29f1 instalada, Unity Hub te ofrecerá instalarla automáticamente al abrir el proyecto.
4. Espera a que Unity importe los assets (primera apertura puede tardar varios minutos).

> Nota: carpetas como `Library/`, `Temp/`, `obj/` y los archivos `.sln`/`.csproj` no están versionados (se regeneran automáticamente al abrir el proyecto en Unity/Visual Studio, ver `.gitignore`).

## Cómo jugar

1. Abre la escena `Assets/Scenes/MainMenu.unity`.
2. Pulsa **Play** en el editor, o dale a "Jugar" en el menú principal para empezar en la mazmorra `EntranceHall`.

### Controles (teclado)

| Acción              | Teclas         |
|---------------------|----------------|
| Mover               | `W` `A` `S` `D` |
| Disparar (dirección)| Flechas `↑` `↓` `←` `→` |

El disparo apunta hacia la dirección de las flechas de forma independiente al movimiento con WASD.

## Estructura del proyecto

```
Assets/
├── GameManager.cs        # Estado global del jugador (vida, velocidad, disparo)
├── Scenes/
│   ├── MainMenu.unity
│   └── DronDungeonRooms/         # EntranceHall, Reactor, Underground
│       └── <Nivel>/<Nivel>{,Start,Empty,End}.unity
├── Scripts/
│   ├── PlayerController.cs, CameraController.cs, BulletController.cs
│   ├── EnemyController.cs, HealthPointUI.cs, ItemsController.cs, MainMenu.cs
│   ├── DronDungeon/               # Generación y gestión de la mazmorra
│   └── DronDungeonSpawners/       # Spawns de enemigos/ítems por habitación
├── Objects/               # Prefabs y ScriptableObjects de datos
└── Sprites/                # Arte 2D (enemigos, GUI, tiles)
```

## Tecnologías

- Unity 2021.1.29f1 (2D URP/Built-in, `Rigidbody2D`/`Collider2D`)
- C#
- TextMesh Pro para la UI de texto
