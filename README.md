# TrafficLights
Code for an application that simulates a set of traffic lights at an intersection. The traffic lights are designated (N, S) and (E, W) like a compass.

# Requirements
- The lights will change automatically every 5 minutes.
- When switching from green to red, the yellow light must be displayed for 30 seconds prior to
it switching to red.

# Intro
- Create new intersection:
```csharp
Intersection intersection = new Intersection( IntervalOfDirection );
```
- Create needed traffic lights:
```csharp
TrafficLight nsLight = new TrafficLight( intersection, IntervalOfYellow, Direction.NS );
TrafficLight ewLight = new TrafficLight( intersection, IntervalOfYellow, Direction.EW );
```
- ~~Light the fire~~ Switch intersection on:
```csharp
intersection.Start();
```
- When you are done, switch it off like this:
```csharp
intersection.Stop();
```
- Constants to control time intervals for various events:
```csharp
// Use TimeCoefficient to manipulate time scale in application;        

// Use this for honest 30 mintues simulation
public const int TimeCoefficient = 1000;        

// Use this for speeded up simulation
//public const int TimeCoefficient = 10;

// Interval of switching directions NS -> EW -> NS and so on.
public const int IntervalOfDirection = 5 * 60 * TimeCoefficient;

// How long will yellow be shown, during switch Green -> Red.
public const int IntervalOfYellow = 30 * TimeCoefficient;

// How long to wait before stoping simulation
public const int TimeToSimulate = 30 * 60 * TimeCoefficient;
```
