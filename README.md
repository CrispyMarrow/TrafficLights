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
