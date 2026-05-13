# Map / Environment — Teammate Notes

The Game scene (Assets/Scenes/Game.unity) is built and ready to integrate.

## Scene structure

_Map (root, has NavMeshSurface — already baked)
  Floor (20x20)
  Ceiling (4m high)
  Wall_North/South/East/West (outer)
  Wall_Int_EW_Left/Right (interior, 3m doorway gap on x=0)
  Wall_Int_NS_North/South (interior, 3m doorway gap on z=0)
  Lamps/ (Lamp_NW, NE, SW, SE — instances of Assets/Prefabs/Lamp.prefab)
  Props/ (tables, crates, shelves, pillar — atmospheric clutter)
PlayerSpawn (tag: 'Respawn', position SW room facing NE)
_AI_Hooks
  PatrolPoint_NW / NE / SW / SE / Center

## Hooks for each role

PLAYER CONTROLLER
- Spawn at GameObject.FindWithTag("Respawn").transform.position
- Or expose a public Transform spawnPoint and drag PlayerSpawn into it.

CAPYBARA AI
- NavMesh already baked. Drop a NavMeshAgent on your capybara and it will pathfind immediately.
- Patrol points: _AI_Hooks has 5 empties. Expose a Transform[] patrolPoints field and drag the children in.
- If you change geometry: select _Map -> NavMeshSurface component -> Bake to rebuild.

UI / AUDIO
- Build settings: MainMenu is index 0, Game is index 1.
- SceneManager.LoadScene(1) to start the game from the menu.

## Lighting

URP, max 4 additional lights — currently at the limit (one lamp per room).
- Ambient: Flat, ~0.05 grey (very low for horror mood)
- Fog: exponential squared, dark blue-grey
- No directional sun (interior only)

If you need a flicker/extra-light moment, either disable a room lamp first
or bump 'Max Additional Lights' in Assets/Settings/PC_RPAsset.asset.

## Materials

In Assets/Materials/: M_Floor, M_Wall, M_Ceiling, M_Prop, M_LampGlow.
Swap a _BaseColorMap texture on any of these to add real textures later;
the geometry stays the same.
