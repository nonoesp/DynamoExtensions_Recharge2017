# Recharge 2017

# Day 1

## Background/Existing Resources

### What is the View Extension framework?
The View Extension framework for Dynamo allows you to extend the Dynamo UI by registering custom MenuItems. A ViewExtension has two components, an assembly containing a class that implements `IViewExtension`, and an `ViewExtensionDefintion` xml file used to instruct Dynamo where to find the class containing the `IViewExtension` implementation. The `ViewExtensionDefinition` xml file must be located in your `dynamo\viewExtensions` folder.

### Dynamo Samples
DynamoSampples contains a viewExtentsion sample that demonstrates an `IViewExtension` implementation which shows a modeless window when its `MenuItem` is clicked. The Window created tracks the number of nodes in the current workspace, by handling the workspace's `NodeAdded` and `NodeRemoved` events.

### Other Examples

- [DynamoSamples](https://github.com/DynamoDS/DynamoSamples/tree/master/src/SampleViewExtension)
- [Dynamo Package Manager](https://github.com/DynamoDS/Dynamo/tree/dec6240ded0c4369617775336b9af60c2aba4103/src/DynamoPackagesUI)
- [Dynamo Notifications](https://github.com/DynamoDS/Dynamo/tree/master/src/Notifications) 
- [Librarie.js](https://github.com/DynamoDS/Dynamo/tree/master/src/LibraryViewExtension)
- [DynamoShape](https://github.com/LongNguyenP/DynaShape)

## Setting up a New ViewExtension Project

1) Create a new Visual Studio Class Library project
    - This project will be called RechargeViewExtension

2) Include the following assemblies
    - `System`
    - `System.Windows`
    - `System.Windows.Controls`
    - `Dynamo.Wpf.Extension` (NuGet DynamoVisualProgramming.WpfUILibrary)

3) Extend the IViewExtension class </br>
    ```public class RechargeViewExtension : IViewExtension```