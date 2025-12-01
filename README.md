# Cohesion Steering Behaviour

## Overview

This project aims to demonstrate the utilities of Steering Behaviors in Unity 6.2. These behaviors are essential to create believable autonomous NPCs that can move through the scene while reacting to the player and to other agents, becoming an important asset in any Game Developer’s toolkit.

## What are Steering Behaviors?

Steering Behaviors are movement patterns that control how an autonomous character accelerates, slows down, and changes direction in response to a target.
In this project, the NPCs use forces calculated from vectors (distance, direction and velocity) to:

- Seek and arrive smoothly at a target (the player)

- Evade a threat when it gets too close

- And, as an extension of the activity, stay together using the Cohesion movement pattern

## Cohesion (chosen extra behavior)

- Group Movement: a group of NPCs periodically calculate the average position of their neighbors and steer towards this center of mass.

- Visual Organization: by applying Cohesion together with Seek/Evade, the NPCs do not spread randomly across the level, instead, they tend to remain close to their group while still reacting to the player’s movement.

## References

[Prática em Laboratório UNITY – Comportamentos de Direção (Steering Behaviors) – Curso Tecnológico em Jogos Digitais](https://github.com/user-attachments/files/23847566/praticaSB-UNITY.pdf)

## Advisor

[Murilo Boratto](https://github.com/muriloboratto)
