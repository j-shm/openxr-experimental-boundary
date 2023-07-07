
# Open XR Boundary Documentation

## Setup
### User Setup
Direct the user to install and [run the application](https://github.com/j-shm/openxr-experimental-boundary) that is used to designate the bounds they will through this process create a file ending in .json that contains all the information you need to use their boundary setup
### Developer Setup
Install the package and newtonsoft json.
Add the ```LoadObjects``` component to a object and then give it the corner and object prefab.
These can be custom however the object prefab should be a singular mesh.
Call ``LoadObjects.Load()`` from another script and this will load all the objects in.
You should pass in the file path as a paramater so the package knows where to find the .json

## Getting Object Information
Every Object has a component called ```ObjectData``` this contains information about the object.
```
bool placed
ObjectType.Object objectType
```
The objectType is the variable that defines what a object is in the real world. Using the Object enum in the ObjectType class. Use ObjectType.Object to get the types { Table, Chair, Screen, Drawers, Misc }

The default corner prefab has a component called ```ObjectHolder``` this is a component for easily getting the above information. User ```GetObjects()``` to get objects that are placed and pass in a type as a paramater to get only objects of that type. A more verbose option is ```GetObjectsOfType()``` but they are the same method.


## Further Information
This bases all objects off a corner point that the user defined when they create the project

```
         o
         ▲
         │      o
         │      ▲    o
         │      │    ▲
         │o     │    │
         │▲     │    │  o
         ││     │    │  ▲
         ││     │    │  │
   x─────┴┴─────┴────┴──┘


x=corner (pivot)
o=object
```

This means that if you move the corner all the other objects will move as well: This should be able to be moved by the player at the start in case the unity project is setup differently. The corner may vary slightly so it is advised to give the option to move.
