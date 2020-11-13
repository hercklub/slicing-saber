# Saber slice
Simple demo game project created using **Unity 2019.4.13f1**,  **Strange IoC** and  **Framewerk** .

![Strage IoC](http://strangeioc.github.io/strangeioc/class-flow.png)
- [Strange](https://github.com/strangeioc/strangeioc) is a super-lightweight and highly extensible Inversion-of-Control (IoC) framework.
- Framewerk is set of extension for Strage taking care of things like UI, lists, coroutienes etc.
- Bunch of the code is also stolen from my [VR project](https://sidequestvr.com/app/1470/you-are-two) 

## Code overview

[Bootstrap.cs](https://github.com/hercklub/slicing-saber/blob/master/Assets/Scripts/Contexts/Bootstrap.cs) acts as a single point of entry for the whole application. 
Context is created and all bindings necessary for dependency injection are mapped.
The scene holds only ViewConfig that defines containers for later instancing.


### View/Mediator
Mediator is responsible for getting data from the model and passing it to the View.
### Data
- Static data are defined in Scriptable objects that are using ScriptableObjectDefintions
- Dynamic data are stored mostly in data models and retrieved by mediators/controllers
### Signal
 Events that are dispatched using event bus systems providing communication through the app.
### Commands
Commands are objects that are bind to specific signal calls and usually serve as a nice container for logic.
### Pools
Standard usage of object pool with IPoolable interface.
### Asset manager
Responsible for getting objects from game resources.
### FSM
FSM is used to control the game lifecycle.