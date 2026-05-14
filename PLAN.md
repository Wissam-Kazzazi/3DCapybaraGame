# Capybara Jumpscare Game — Project Plan

## Game Overview

A first-person horror/comedy game where the player explores a dark map and gets jumpscared by capybaras. The contrast between the cute animal and the horror setting is the core design hook.

**Engine:** Unity  
**Genre:** First-person horror / jumpscare  
**Theme:** Capybara

---

## Team Roles

| Role | Responsibility |
|------|---------------|
| Map / Environment | Build the level, place props, set up colliders, add lighting |
| Player Controller | First-person movement, mouse-look, camera setup |
| Capybara AI & Jumpscare | Patrol logic, trigger zones, jumpscare animation/sound |
| UI & Audio | Main menu, game-over screen, background ambience, audio manager |

> Everyone should commit their own work to the repo — even small commits (fixes, tweaks, README edits) count toward the commit history grade.

---

## Required Rubric Elements

### Lighting (2 types required)
- **Ambient light** — low-intensity global light to set the dark mood
- **Point lights** — placed at torches, lamps, or glowing objects around the map

### Camera Movement
- First-person camera with mouse-look (Unity `CinemachineVirtualCamera` or manual `Transform.Rotate`)
- WASD movement tied to player controller

### Animation
- Capybara idle / walk cycle animation (Animator component)
- Jumpscare animation — sudden scale/position pop toward the camera

### Enemies / Moving Obstacles
- Capybara AI that patrols a set path using `NavMeshAgent`
- Trigger zone (collider) that activates the jumpscare when the player gets close

### Menus
- **Main menu** — Start button, Quit button
- **Game-over screen** — shown after jumpscare triggers (with restart option)

---

## Scenes

```
Assets/
├── Scenes/
│   ├── MainMenu.unity
│   └── Game.unity
├── Scripts/
│   ├── PlayerController.cs
│   ├── MouseLook.cs
│   ├── CapybaraAI.cs
│   ├── JumpscareController.cs
│   ├── UIManager.cs
│   └── AudioManager.cs
├── Prefabs/
│   ├── Capybara.prefab
│   └── Player.prefab
├── Audio/
│   ├── ambience.wav
│   ├── jumpscare.wav
│   └── capybara_sound.wav
└── Materials/
```

---

## Milestone Plan

### Week 1 — Core Setup
- [ ] Create Unity project and push to GitHub
- [ ] Block out the map (basic geometry, no detail)
- [ ] Get player movement and mouse-look working
- [ ] Add basic ambient + point lighting

### Week 2 — Gameplay
- [ ] Add capybara model/prefab with NavMesh patrol
- [ ] Implement trigger zone for jumpscare
- [ ] Build jumpscare animation and sound effect
- [ ] Add capybara idle/walk animation

### Week 3 — Polish & Submission
- [ ] Build main menu scene
- [ ] Build game-over screen
- [ ] Add background ambience and capybara audio cues (for tension)
- [ ] Playtest and fix major bugs
- [ ] Record gameplay video walkthrough (~3 min)
- [ ] Final push to GitHub, confirm all members have commits

---

## Presentation Outline (~3 minutes)

1. **Intro** — Each team member says their name and what they built (30 sec)
2. **Game overview** — Explain the concept and theme (30 sec)
3. **Gameplay walkthrough** — Show the map, player movement, capybara AI, and trigger a jumpscare (90 sec)
4. **Wrap-up** — Briefly mention design decisions (lighting, tension-building audio) (30 sec)

---

## Design Notes

- The jumpscare works best with a tension build-up before it — use flickering lights, quiet capybara sounds in the distance, or slow footstep audio to create dread before the scare triggers.
- Keep the map small and navigable. A single floor with a few rooms is enough — complexity comes from lighting and sound, not map size.
- The comedic/horror contrast (cute capybara = scary) is a deliberate design choice worth mentioning in the presentation.
