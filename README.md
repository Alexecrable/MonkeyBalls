# MONKEY BALLS

Super jeu tres cool où on pourra jouer en mode MONKEY ou en mode marble it up

## TO-DO

- [Level](#level)
- [Collectables](#collectables)
- [Arrivée](#arrivée)
- [HUD](#hud)
- [Menu](#menu)
- [Bouton Niveau](#bouton-niveau)
- [JEU](#jeu)


### Level
```
Node3D :
    - * staticbody (Terrain)
    - Node3D (Spawn)
    - Arrivée

```

### Collectables

```
Tacos :  Node3D
    - Mesh3D + Text : V
    - Area (pour la collect)
    - Tweener (pour ptite anim)

Monnaie du jeu a collectionner dans les niveaux, permettra d'acheter des skins ?

Sig : 
    Tacos Collecté
```

### Arrivée

```
Goal : StaticBody3D
    - Mesh3D + Text : X
    - Area (pour entrer dans la porte et trigger la fin)

Batiment dans lequel rentrer pr finir le niveau

Sig : 
    Arrivée oe
```

### HUD

```
HUD : CanvasLayer / Control
    - TacosCounter : Label + Textrec
    - Timer : Label
    - LifeCounter : Label + TextRec
    - Timer ?
```

Sig : 
    -> Tacos

### Menu

```
Menu : CanvasLayer / Control
    - V/HBox
    - Boutons

L'idée sera d'abord d'avoir un layout vertical classique avec debut de jeu, controles, exit.

Puis de choisir entre le controle monkeyball ou marble

puis d'avoir un ecran de selection de niveau (deblocables, faire un systeme de save ?)

Sig : 
    Niveau selected (Level level)

```

### Bouton Niveau

```
Button
    -Image screenshot du niveau [Export]
    -Level [Export]

Faire une liste des niveaux qui créé automatiquement les boutons pour les mettre dans le menu
```

### JEU

Mettra en oeuvre les elements du jeu
```
- Jeu : 
    - Menu
--------------------------
    - Level
    - Joueur
    - HUD

Sig : 
    -> Level Selected
```