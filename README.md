# Snappy Safari

![Gameplay Image](https://cameronmain.com/wp-content/uploads/2023/10/Build-Screenshot-2023.10.21-01.03.29.45.png)

## Project Overview

"Snappy Safari" is a short, time-bound Unity game designed and developed as part of my University coursework. Players are tasked with capturing pictures of randomly specified animals before time runs out. By integrating Goal-Oriented Action Planning (GOAP), the game introduces a layer of strategic gameplay and dynamic AI behaviors, offering players a blend of exploration, strategy, and snap decisions.

This project is available to play in-browser on [itch.io.](https://main.itch.io/snappy-safari)

## Features

- **GOAP Agents**: Dive into an ecosystem powered by GOAP, where animals exhibit dynamic behaviors based on their goals and the environment.

- **Time-based Challenge**: Every second counts as you attempt to track and capture each animal through your camera lens.

- **Three Unique Levels**: Meet the inhabitants of the African Savannah, freezing Arctic and Boreal Forest.


## Scripts

The core game mechanics, especially the GOAP system, are constructed through several C# scripts:

- `GAgent`: Manages AI behaviors and decisions of animals based on current goals and beliefs.

- `GAction`: Defines possible actions an agent (animal) can take within the GOAP framework, detailing preconditions and effects for each.

- `GPlanner`: Integral to the GOAP system, this script calculates which series of actions an agent should take to achieve a given goal. It considers available actions, their preconditions, and their effects to formulate a plan of action.

- `WorldStates`: Manages the overarching states of the world, influencing the decisions and behaviors of GOAP agents. E.g. If a wateringhole is occupied by another agent or not.

## Installation

1. Clone the repository or download the zip file.
2. Open the project in Unity.
3. Build and run the project 

## Licensing Information

### Code

The source code of "Snappy Safari" is under the [MIT License](https://opensource.org/licenses/MIT). You can use, adapt, and share the software in line with the license's stipulations.

### Models and Assets

It's imperative to note that while the source code is freely available under the MIT License, the 3D models, textures, sounds, and other assets **are not**. They come with their individual licensing terms. Use or redistribution of these assets outside the purview of this game is stringently prohibited unless sanctioned by the asset creators or rights holders. Always refer to each asset's accompanying license for detailed terms. Any unauthorised use of these proprietary assets might result in legal consequences.
