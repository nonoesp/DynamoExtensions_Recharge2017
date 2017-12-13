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

4) `IViewExtension` required implementations that must be included
    - `IViewExtension.Startup()`
    - `IViewExtension.Loaded()`
    - `IViewExtension.Shutdown()`
    - `IViewExtension.UniqueId`
    - `IViewExtension.Name`
    - `IDisposable.Dispose()`

    At this point your class should look something like this...

    ```C#
    namespace RechargeViewExtension
    {
        public class RechargeViewExtension : IViewExtension
        {
            public void Dispose() { }

            public void Startup(ViewStartupParams p) { }

            public void Loaded(ViewLoadedParams p) { }

            public void Shutdown() { }

            public string UniqueId
            {
                get
                {
                    return Guid.NewGuid().ToString();
                }
            }

            public string Name
            {
                get
                {
                    return "Recharge View Extension";
                }
            }

        }
    }
    ```

5) Define a `ViewExtensionDefinition.xml` </br>
Let's add an `xml` file that defines our viewExtension to the project.  This should include a reference to our assembly and the type name we defined in our class. So something like this... </br>

    `RechargeViewExtension_ViewExtensionDefinition.xml`
    ```xml
    <ViewExtensionDefinition>
        <AssemblyPath>..\RechargeViewExtension.dll</AssemblyPath>
        <TypeName>RechargeViewExtension.RechargeViewExtension</TypeName>
    </ViewExtensionDefinition>
    ```

6) Define a pop-up window

    The next step is to define what type of user interface we want to create. For this first example we are going to keep it very simple and implement a new pop-up window that can be launched from the Dynamo menu.  There are 3 steps to this section but we will start by defining the appearance of the window in an `xaml` file.  If you are new to `xaml` it is an XML-based markup language developed by Microsoft.  There is a ton of documentation online and it is very common when working with Wpf.

    There are multiple ways to develop a design using `xaml`.  You can textually define the aesthetic/behavior, use a `xaml` designer for a more physical approach, or a combination of both.

    ```xaml
    <Window x:Class="RechargeViewExtension.SampleWindow"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
             xmlns:local="clr-namespace:RechargeViewExtension"
             mc:Ignorable="d" 
             d:DesignHeight="300" d:DesignWidth="300"
            Width="500" Height="100">
        <Grid Name="MainGrid" 
            HorizontalAlignment="Stretch"
            VerticalAlignment="Stretch">
                <TextBlock HorizontalAlignment="Stretch" Text="{Binding SelectedNodesText}" FontWeight="Bold" FontSize="24"/>
        </Grid>
    </Window>
    ```
    

