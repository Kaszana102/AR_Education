# MSI AR Game

## Introduction

The aim of this project is to create an educational AR (Augmented Reality)  mobile game for the subject of Multimedia Interactive Systems. The game is to contain issues from a selected field of computer science, computer graphics was chosen. The task was completed by a team of three people during the first semester of master's studies. Each team member has to create a separate mini-game based on different issue of computer graphics. The final version contains 3 mini-games:
- `RGBMixer` - Michał Kuprianowicz
- `Lattice Deform` - Jakub Wierzba
- `Bézier Curves` - Krzysztof Pisarski
<br><br>


### RGBMixer

The aim of this mini-game is to combine fun gameplay with understanding how mixing RGB colors work. To start playing 4 vumarks (1-4) are needed. The game randomly selects 4 colors (except black) and sets them as `starting colors`. They can be seen appearing on vumarks when camera tracking works correctly. Each mark shows:
- its color and hex representation
- number of how many times it is used in mixing
-  `+` and `-` signs

#### UI

The top part of the screen has a simple UI containing:
- exit to the main menu button
- `current` representing color created from mixing `starting colors`. At the beginning of the level, it is set to white
- `target` is a color that the player has to create using `starting colors`. It is randomly chosen from starting colors and mixing them 5 - 9 times
- `similarity` which represents how close `current` is to `target`. The value is calculated by the distance of 2 colors in an RGB cube which is similar to the human eye comparing 2 colors. 
- `next level` button after completing the level
- `reset` button at the bottom of the screen to reset the whole level and start again

#### Gameplay

To complete a level it is needed to create `current` color with at least `96%` similarity to `target`. After this ability to go to the next level is given. Then new colors are chosen and the game repeats itself.

If some issues happen with vumarks tracking it is recommended to cover the phone camera for 3 seconds to make it lose tracking for all vumarks.

<br>

### Lattice deform

TODO Jakub Wierzba


<br>

### Bezier

TODO Krzysztof Pisarski

<br>

## Technologies

- Unity 2022.3.18f1
- Vuforia 10.22
- [Vumarks 1-4](https://developer.vuforia.com/sites/default/files/vuforia-library/docs/target_samples/unity/mars_vumarks.pdf)




