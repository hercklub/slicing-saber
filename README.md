# Saber slice
Simple demo game project created using **Unity 2019.4.13f1** and **Strange IoC**.

![Strage IoC](http://strangeioc.github.io/strangeioc/class-flow.png)
- [Strange](https://github.com/strangeioc/strangeioc) is a super-lightweight and highly extensible Inversion-of-Control (IoC) framework.
- Framewerk is the set of extension for Strage taking care of things like UI, lists, coroutienes etc.
- Bunch of the code is also stolen from my [VR project](https://sidequestvr.com/app/1470/you-are-two) 

## Code overview

[Bootstrap.cs](https://github.com/hercklub/slicing-saber/blob/master/Assets/Scripts/Contexts/Bootstrap.cs) acts as a single point of entry for the whole application. 
Context is created and all bindings necessary for dependency injection are mapped.
The scene holds only ViewConfig that defines containers for later instancing.


### View/Mediator
Mediator acts as an interface between View and Model. Processing all logic, getting data, preparing for the final render.
### Data
Static data are defined in Scriptable objects.
Dynamic data are stored mostly in data models and retrieved by mediators/controllers.
### Signal
 Events that are dispatched using an event bus that makes information flow easy and highly decoupled.
### Commands
Commands are objects that are bind to specific signal calls serve as a nice container for logic.
### FSM
FSM is used to control the game lifecycle.

## Credits
[Strange](https://github.com/strangeioc/strangeioc) - core framework used in this application\
Framewerk - extension co-developed with Matej Ošanec