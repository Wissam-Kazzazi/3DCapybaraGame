# Capybara Jumpscare Game — Project Plan

## Game Overview

A first-person horror/comedy game where the player has been tasked with reconning a secret underground facility. John Pork has sent you on a mission to gather intel, and report back. But be careful Tung Tung Tung Sahur is patroling the facility and if he catches you John Pork will be at risk. Manuever from room to room, while avoiding being seen to win, get caught and you have to start over.

**Engine:** Unity  
**Genre:** First-person horror / jumpscare  
**Theme:** Brainrot

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
- Tung Tung Tung Suhur AI that patrols a set path using `NavMeshAgent`
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
│   ├── TTTSAI.cs
│   ├── JumpscareController.cs
│   ├── UIManager.cs
│   └── AudioManager.cs
├── Prefabs/
│   ├── TTTS.prefab
│   └── Player.prefab
├── Audio/
│   ├── ambience.wav
│   ├── jumpscare.wav
│   └── death_sound.wav
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

- Push creative deisgn, through funny videos, popular characters, and use audio, and lighting elements to build suspense
  
