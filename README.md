# League of Legends NetLog Reader
# Analysis Tool

The Analysis Tool is a .NET application that allows users to analyze network data from League of Legends netlog files and identify potential issues. It provides a user-friendly interface for loading netlog files, calculating average values, detecting anomalies, and generating an analysis report.

## Features

- Load network data from League of Legends netlog files by simply dragging and dropping the file onto the application.
- Calculate average values for different network metrics, including ping, incoming/outgoing packets, jitter, packet loss, and reconnect events.
- Highlight cells in a DataGridView based on predefined thresholds to identify abnormal values.
- Generate an analysis report summarizing the thresholds, maximum/minimum changes, and explanations of network issues.
- Analysis report in a flash.

## Usage
Launch the Analysis Tool.
Drag and drop your League of Legends netlog file onto the application window.
(files are usually located in Riot Games\League of Legends\Logs\GameLogs\{Date}\*****netlog.txt)
The tool will automatically load the netlog file and calculate average values for each metric.

## Bad Connection Example
![Network Analysis Tool Bad Connection Example](/Screenshots/BadConnectionExample.gif)

## Good Connection Example
![Network Analysis Tool Good Connection Example](/Screenshots/GoodConnectionExample.gif)
