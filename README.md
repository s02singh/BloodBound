# BloodBound #

## Summary ##

The objective of BloodBound is to survive in a gladiator pit-style arena. It’s a typical wave survival with various mobs being introduced as you progress. Your only weapon is a sword. The environment is a 3D world with a third-person controller for the player. Once you make it to wave 10, you unlock the boss fight, beat the final enemy, and win.

## Project Resources

[Web-playable version of your game.](https://itch.io/)  
[Trailor](https://youtube.com)  
[Press Kit](https://dopresskit.com/)  
[Proposal: make your own copy of the linked doc.](https://docs.google.com/document/d/1qwWCpMwKJGOLQ-rRJt8G8zisCa2XHFhv6zSWars0eWM/edit?usp=sharing)  

## Gameplay Explanation ##

You have unlimited time/waves. Defeat all enemies in the wave to start the next one. There are 10 waves.

Rage - As you inflict damage and take damage, you slowly build a rage meter. Once you are full, the character emits a red lightning aura.
Press V to trigger Rage Blast.

Current Controls:
Movement: WASD
Sprint: Hold shift
Look: Mouse
Jump: Space
Equip: R
Roll: Q
Dash Attack - Sprint + Left click
Light Attack: Left click
Combos: Light x 3
Block: Hold right click



# Main Roles #

- Producer + Character Mechanics: Sahilbir Singh

## Producer + Player Mechanics - [Sahilbir Singh](https://github.com/s02singh)

### Producer Role:

#### Leadership and Communication:

As the producer of BloodBound, my primary focus was coordinating and leading the development team. I used Discord to establish a way to connect and inform everyone of timelines as well as to stay in touch of updates. In Discord, I created dedicated channels for playtesting, bugs, and game ideas, each of which I had members document their findings. All Git repo pushes were also mandated to be reported before pushing. Everyone seemed to have a great understanding of their role, and I made sure to check on their systems, ensuring compatibility to the game as a whole. 

![DiscordLayoutGIF](https://github.com/s02singh/BloodBound/blob/main/READMEAssets/discordlayout.gif)
#### Project Management:

I organized and led all group meetings, using Discord as a central hub for scheduling and conducting discussions. My primary plan was to have each contributor create their components on their own branch in our GitHub repo. You can find that repo here https://github.com/s02singh/RollingWithThePunches. This allowed Nico to create enemies, Inseon to create the map, Khuyen to make the UI, and Darsh to learn vfx. My aim was to provide a "Playground" to each member, allowing them to learn and create their systems without having to deal with each other's bugs and problems. I was in charge of developing the entire player character - mechanics, look, feel and all. I will discuss this further in the Character Mechanics section. Once all systems were implemented, I had each member package their respective assignments. Finally, I put it all in one project as a base spawn point, which led to the creation of this GitHub repo. 

#### Engagement:
Aside from meetings during class and Discord calls every few days, I made sure to actively communicate daily. I reached out to team members, making sure timelines were being followed and help was provided when needed. The timeline defined in the initial progress report was great baseline. However, during our peer review report, I used it as an opportunity to also create an updated report/progress check in. You can find that here. https://docs.google.com/document/d/1Hy5fcxIj2UezN37R412JOx16OQY12U62Vvo3Qlx1_To/edit?usp=sharing

### Character Mechanics:

#### Animations:

My primary goal with character animations was to make the character feel alive. I wanted our protagonist to have a story to tell through his body language. I wanted his attacks to be desperate yet undying. The theme is Rolling with the Punches. The story to tell is of a gladiator who rises against all odds. I imported and configured a variety of animations for the player character, including combinations of combat maneuvers such as attacks, blocks, rolls, and dashes. These animations were sourced from https://www.mixamo.com/#/. Each animation is integrated into the gameplay with smooth transitions and feel. The character feels alive. He gives you a sense of immersion. All character animations were implemented by me. These include but are not limited to: 
- attack1, attack2, attack3
- block
- takedamage
- meteor ultimate
- block
- roll
- death


Animation Events:
![DiscordLayoutGIF](https://github.com/s02singh/BloodBound/blob/main/READMEAssets/animation_events.png)

DashAttack
![DashAttack](https://github.com/s02singh/BloodBound/blob/main/READMEAssets/dashattack.gif)

Attack Combo
![Combo](https://github.com/s02singh/BloodBound/blob/main/READMEAssets/combo.gif)

Dodging
![Dodge](https://github.com/s02singh/BloodBound/blob/main/READMEAssets/dodge.gif)

Each animation has key animation events that trigger core mechanics. For example, each attack has events to trigger the sound and send raycasts, making the animation feel smooth and connected to the game. The roll triggers invincibility (iframes). Meteor ultimate triggers a vfx effect.

#### Creative Implementation:

Once all the base mechanics were implemented, I felt the player lacked a sense of power. The player should feel excited, he should enjoy being the paladin. That's how I decided on a unique ultimate attack for the player character. With incredible visual effects that includes meteor strikes, time-slowing effects, and dramatic camera angles, I crafted a show-stopping ability that added depth and excitement to the gameplay. This not only elevated the game's combat dynamics, but it provided players with a memorable and exhilarating experience. The idea was that the ability charges as you take damage, hence, you Roll With The Punches.
![MeteorUlt](https://github.com/s02singh/BloodBound/blob/main/READMEAssets/meteorult.gif)
#### Technical Proficiency:

My contributions extended beyond animation. I developed the combat logic for the player character. Through the implementation of damage logic, including sword slashing mechanics, aim assist, raycasting, and particle effects, I ensured that combat interactions felt responsive, fluid, and engaging. My scripting was documented, concise and enabled all character mechanics, contributing to the overall polish and gameplay of BloodBound.
Check out my scripting here [PlayerController.cs](https://github.com/s02singh/BloodBound/blob/359bd4876b90052a129f0684d52f56c03f948f14/Assets/Scripts/PlayerController.cs#L106-L126)

**Describe the steps you took in your role as producer. Typical items include group scheduling mechanisms, links to meeting notes, descriptions of team logistics problems with their resolution, project organization tools (e.g., timelines, dependency/task tracking, Gantt charts, etc.), and repository management methodology.**



## User Interface and Input

**Describe your user interface and how it relates to gameplay. This can be done via the template.**
**Describe the default input configuration.**

**Add an entry for each platform or input style your project supports.**


## Enemy Design and AI

There are five main enemies in our game: 3 melee, 1 ranged, and 1 boss with many attacks.

Zombie: Weak enemy that paths at a medium speed towards the player and has a very short attack range. Appears on the first few waves.

![ZombieGIF](https://github.com/s02singh/BloodBound/blob/main/READMEAssets/zombie.gif)

Archer: Weak enemy that paths at a very slow speed towards the player but has a very long attack range, in which the archer shoots an arrow at the player. Appears on the first few waves.

![ArcherGIF](https://github.com/s02singh/BloodBound/blob/main/READMEAssets/archer.gif)

Warrok: Tanky enemy that paths at a slow speed towards the player and has a short-medium attack range. Begins appearing towards the middle waves of the game.

![WarrokGIF](https://github.com/s02singh/BloodBound/blob/main/READMEAssets/warrok.gif)

Reaper: Medium health enemy that is incredibly fast and dangerous, with a medium attack range. Only appears in the very late waves of the game.

![ReaperGIF](https://github.com/s02singh/BloodBound/blob/main/READMEAssets/reaper.gif)

Dragon: Boss enemy that has two stages of the fight.

Stage 1: Bites at the player from a fairly long distance, as the dragon is very large. Dragon walks towards the player until he is in bite range and keeps biting until he is low enough in health to enter Stage 2.

![DragonStage1](https://github.com/s02singh/BloodBound/blob/main/READMEAssets/dragonbasic.gif)

Stage 2: Dragon now cycles (based on a timer) between two modes: flying and grounded. In the flying mode, the dragon is immune to damage and shoots explosive fireballs at the player until his flying timer is done. Once the flying timer has expired, the dragon lands so the player can get attacks to land on the dragon once again, but the dragon is not docile during this window of attack. Once grounded in Stage 2, the dragon breathes flames on the player, inducing tick damage.

![DragonStage2](https://github.com/s02singh/BloodBound/blob/main/READMEAssets/dragonStageTwo.gif)

For the enemies in our game, I used many assets from the Unity Asset Store and mixamo.com to provide quick implementations of animated enemies without the heavy use of blender. Here is a list of all the sources used in this project for the enemies:

Archer, Warrok, and all humanoid animations:
https://www.mixamo.com/#/

Zombie:
https://assetstore.unity.com/packages/3d/characters/humanoids/zombie-30232

Dragon:
https://assetstore.unity.com/packages/3d/characters/creatures/dragon-pbr-94333

PyroParticles (for dragon fire attacks): 
https://assetstore.unity.com/packages/vfx/particles/fire-explosions/fire-spell-effects-36825

Arrow (for archer attack): https://assetstore.unity.com/packages/3d/props/weapons/low-poly-rpg-fantasy-weapons-lite-226554


With these prefabs and animations ready to implement, I used Unity’s AnimatorController to call certain animations based on boolean logic within my two main scripts: EnemyAI and DragonAI.

EnemyAI:
This script is used to control the four main enemies from the waves. It uses a NavMeshAgent placed on a NavMeshSurface that is baked into the scene to path towards the player at a speed decided within the inspector. The script provides implementations for all enemy decisions and pathing, using the transform position of the enemy and the position of the player to decide if the enemy is within range to attack. 

https://github.com/s02singh/BloodBound/blob/7f8ed21a6d4c5965921ec0de8d6d6d2cd3b470e9/Assets/Scripts/EnemyAI.cs#L48C9-L63C10

If it is within range, the enemy will enter the animation state of attacking and use animation events to trigger a Raycast (or for the Archer, another function ProjectileLaunch to launch an arrow) towards the player. Once completed, an animation event within each of the enemies’ attack animations will trigger the end of the attack, indicating to return the enemy back to the idle state where the enemy can decide whether to follow the player or wait for a cooldown for their next attack (if still within range of the target).

https://github.com/s02singh/BloodBound/blob/7f8ed21a6d4c5965921ec0de8d6d6d2cd3b470e9/Assets/Scripts/EnemyAI.cs#L90C5-L125C6

Finally, a TakeDamage() function is used to provide the player a way to hurt the enemies. When the enemy has taken damage and their health is at or below 0, they will die and be removed from the scene a few seconds later to prevent massive heaps of bodies filling the scene.

https://github.com/s02singh/BloodBound/blob/7f8ed21a6d4c5965921ec0de8d6d6d2cd3b470e9/Assets/Scripts/EnemyAI.cs#L128C5-L140C10

DragonAI:
This script uses very similar components to the EnemyAI script but implements two stages of fighting via boolean logic. Once the dragon has taken enough damage to reach a threshold health value, the dragon enters Stage 2 of the fight and begins attacking in new ways. Most of this is handled through boolean logic and calling events from the animation controller within the script.

https://github.com/s02singh/BloodBound/blob/7f8ed21a6d4c5965921ec0de8d6d6d2cd3b470e9/Assets/Scripts/DragonAI.cs#L53C9-L94C10

Enemy projectile attacks:
Much of the influence for projectile attacks came from the factory pattern extra credit exercise we completed previously in the course. Both the dragon and the archer use projectile-based attacks. For this, I made separate scripts, DragonFire.cs, LaunchProjectile.cs, and LaunchFireball.cs to instantiate projectiles aimed towards the player with differing physics mechanisms. For example, the LaunchProjectile.cs script uses torque to rotate the projectile mid air to simulate how an arrow should rotate in the air as it reaches its target.

https://github.com/s02singh/BloodBound/blob/7f8ed21a6d4c5965921ec0de8d6d6d2cd3b470e9/Assets/Scripts/LaunchProjectile.cs#L15C5-L39C6

Many of these projectiles use other scripts to actually deal damage to the player, such as the arrow using ProjectileDamage.cs to deal damage to the player if it hits him, then destroy the arrow GameObject. 

https://github.com/s02singh/BloodBound/blob/7f8ed21a6d4c5965921ec0de8d6d6d2cd3b470e9/Assets/Scripts/ProjectileDamage.cs#L5C5-L13C6

Some other scripts that deal with these projectiles for the dragon include FireBaseScript.cs and FlamethrowerCollider.cs. FireBaseScript.cs was an implemented script from the PyroParticles package, but I had to add code to damage the player with the fireball explosion. 

https://github.com/s02singh/BloodBound/blob/7f8ed21a6d4c5965921ec0de8d6d6d2cd3b470e9/Assets/Prefabs/PyroParticles/Prefab/Script/FireBaseScript.cs#L148C9-L180C10

I created FlamethrowerCollider.cs to deal with a damage over time (DoT) effect of the dragon’s flame breath attack. When the player is standing within the hitbox of the flame breath attack, he takes tick damage every 0.1 seconds for the duration of the attack. This encourages the player to not bathe in the flames of the dragon breath for too long.

https://github.com/s02singh/BloodBound/blob/7f8ed21a6d4c5965921ec0de8d6d6d2cd3b470e9/Assets/Prefabs/PyroParticles/Prefab/Script/FlamethrowerCollider.cs#L16C5-L29C6

Wave spawning:
For the main portion of the game, we needed a way of spawning enemies. I took heavy influence from the shield factory exercise, and simply added a WaveSpawner GameObject to the scene that has the WaveSpawner.cs script attached to it, as well as various transform positions for the enemies to spawn at and the enemy prefabs themselves (Zombie, Archer, Warrok, Reaper). With this in place, all of the logic is contained within the WaveSpawner.cs script. This script begins spawning waves of enemies once the player has entered the arena, increasing in difficulty for each wave beaten. At the end of the 10 specified waves within the script, the player is allowed to enter the boss fight.



## Terrains and Visuals




**List your assets, including their sources and licenses.**

### Health HUD
…

### Blood Effects on Enemies
…

### Lightning Strike VFX
…

### Meteor Strike VFX
…

### Sword VFX
…

## Game Logic

**Document the game states and game data you managed and the design patterns you used to complete your task.**

# Sub-Roles


## Audio - [Sahilbir Singh](https://github.com/s02singh)

### Assets and Sources:

-   Entire soundtrack created through AI by Sahilbir Singh
-   Sound effects (SFX) sourced from various online repositories and some created by Sahilbir Singh (the sourced sfx were free without licensing and full use without credit)
-   Music composition and generation tools used: [suno.ai](https://www.suno.ai/)

![DiscordLayoutGIF](https://github.com/s02singh/BloodBound/blob/main/READMEAssets/all_sfx.png)

### Implementation:

#### Soundtrack Composition:

I utilized AI music composition tools to create the entire soundtrack for BloodBound. Each song was orchestrated based on specific feelings and emotions, prompted and iterated upon by me to find the perfect fit for the game's atmosphere. The soundtrack follows a structured format, featuring an opening track, nine acts representing different stages throughout the game, and a final track of hope, which plays during the boss fight to inspire the player.

#### 3D Audio Environment:

To enhance immersion, I implemented a 3D audio environment within the game. Each enemy has its own audio source, creating a sense of depth and spatial awareness for players. This implementation ensures that players can discern the location and proximity of enemies based on sound cues, adding an extra layer of strategy to gameplay. This is especially important when trying to determine when an enemy attack or where an attack is coming from, for example knowing where an archer's arrow is sourced or discerning when the dragon boss will bite.

![DiscordLayoutGIF](https://github.com/s02singh/BloodBound/blob/main/READMEAssets/zombie_sfx.png)
![DiscordLayoutGIF](https://github.com/s02singh/BloodBound/blob/main/READMEAssets/dragon_sfx.png)
#### Sound Effects Integration:

I linked sound effects to animation events, so that gameplay actions are timed and fluid with appropriate audio feedback. For example, the sound of the sword swing is synchronized with the animation of the player character's attack, giving overall better responsiveness and realism of combat mechanics. I even created custom sound effects, like those for the zombie enemy. SFX was definitely a fun, experimental part of game design for me. It was awesome creating a fully original and unique soundtrack for our game.

https://github.com/s02singh/BloodBound/blob/64b451a0925f11f1261cb622c1cc10b2d0bb1fad/Assets/Scripts/PlayerController.cs#L212-217

![DiscordLayoutGIF](https://github.com/s02singh/BloodBound/blob/main/READMEAssets/sound_animation.png)

## Gameplay Testing

**Add a link to the full results of your gameplay tests.**

**Summarize the key findings from your gameplay tests.**

## Narrative Design

**Document how the narrative is present in the game via assets, gameplay systems, and gameplay.** 

## Press Kit and Trailer

Trailer: https://youtu.be/gBnaaRzAJCg
Press Kit: …

For the trailer, I wanted to showcase gameplay but also focus on the core theme of the game, survival. Since it’s a medieval gladiator-style game, I thought it would be appropriate to start the trailer with a background of the situation the player is in, especially by showing the enemies and mobs first rather than any real gameplay. Once the viewer knows what the player’s predicament is, I showed some gameplay to demonstrate that the player must fight to survive, and slash enemies to kill them (the Grim Reaper for example). That is when I decided to include more details, such as the dodge mechanic, while also showcasing that there’s going to be multiple waves. Since we wanted to keep the final boss fight suspenseful, I decided to not show it directly, but to first show the Pit gate opening and the player crossing it. That’s when the dragon reveal happens, with brief clips to not reveal the dragon a lot and keep it as a secret for the player.
The music for the trailer is one of the game’s soundtracks, Hope, but slightly edited around the end to fit the length of the trailer. The Font also matches the UI font. I used DaVinci Resolve to edit my video, and Unity Recorder to record in-game clips.
Most of the screenshots for the Press-Kit were also taken directly from the clips used for the Trailer. They were nice cinematic shots of the environment and enemies, which I believe are the main elements of the game.


## Game Feel and Polish

With our game being a wave survival game, I wanted the waves to be increasingly difficult but easy enough towards the beginning that even inexperienced players could get through the first few. Balancing the game was an enormous task because we have so many different attacks, enemies, and systems in place to worry about such as stamina and rage gauges. Since I primarily designed the enemies, I tweaked all of their numbers numerous times to match the experience desired for the 10-wave system I implemented. Not only that, but deciding how many enemies and what types of enemies to come out in certain waves determines how exciting the experience may be for the player.

[Player and Enemy Stats](https://docs.google.com/document/d/1a7omcd_x4HPyXmLFQ7AxQ1R93e6L3X09QiWZiPWggSY/edit?usp=sharing)  

Another huge aspect of game feel was ensuring that all animations played through smoothly. Many of the animations that I worked with had parts of the animation that transformed the enemy model’s position (root motion), which we did not want since we did our movement and physics calculations on our own in the scripts. Due to this reason, I had to trim many of the animations and reorder animation events called during those animations to perfectly fit the feel of the desired enemy.


Lastly, the game debugging was a large part of my role as I experimented heavily with everybody’s implementations in the project. Although there are still countless bugs within the game, the major gamebreaking ones have mostly been extinguished. Some of the things to note during this process was that I was the main game tester and bug finder for all systems, such as character movement and animation transitions, enemy AI routing and animation transitions, and even some elements of the scene transitions for the map and UI. The presence of bugs in a game brings down the feel enormously, as it makes the game feel unpolished and without effort. Fortunately, everyone in the group worked hard to ensure that the major bugs that I pointed out after each system update were prioritized and dealt with before the final submission of the project.


Overall, I think that game feel may be one of most important roles in a game in general, as it contributes to the players experience within the game. Even if you make an amazing game with complex structures that is not fun to play because of some imbalances or bugs, it’s unlikely that anyone will want to play your game.




