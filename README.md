# League of Legends NetLog Reader
# Analysis Tool

The League of Legends NetLog Reader is a .NET application that allows users to analyze network data from League of Legends netlog files and identify potential issues. The tool provides a detailed breakdown of network performance, allowing users to visualize key metrics like ping, jitter, packet loss, and reconnect events over time. The tool now includes multiple data visualization options, export capabilities, and more advanced analysis features to help pinpoint network-related issues.

## Features

- Load network data from League of Legends netlog files by simply dragging and dropping the file onto the application.
- Calculate average values for different network metrics, including ping, incoming/outgoing packets, jitter, packet loss, and reconnect events.
- Highlight cells in a DataGridView based on predefined thresholds to identify abnormal values.
- Generate an analysis report summarizing the thresholds, maximum/minimum changes, and explanations of network issues.
- Analysis Summary: Generate an analysis report summarizing the maximum, minimum, and average values for each metric, along with an explanation of network issues detected.
- Data Visualization: Generate detailed line charts showing the evolution of network metrics over time, including multiple graph support for comparing metrics side-by-side.
- Export to Image: Save the entire analysis, including all charts and data, as an image file (PNG, JPEG, BMP), ensuring you can share your findings easily.
- Anomaly Detection: The tool highlights anomalies in the data by comparing them to predefined thresholds, providing alerts for potential connectivity issues.

## Usage
Launch the Analysis Tool.
Drag and drop your League of Legends netlog file onto the application window.
(files are usually located in Riot Games\League of Legends\Logs\GameLogs\{Date}\*****netlog.txt)
The tool will automatically load the netlog file and calculate average values for each metric.

## Recent Updates
Now added right click context menus for customization. Full analysis on the NetLog folder. So you can analyse how your connection has been over multiple days.
Screen shots updated.

## Contribute
Contribution and improvements to the LoL Log File Analyzer are warmly welcomed. If you have any suggestions, bug fixes, or new features to add, please feel free to submit a pull request or open an issue on the GitHub repository. Together, we can enhance and refine this tool for the benefit of the League of Legends community.

## Bad Connection Example
![Network Analysis Tool Bad Connection Example](/Screenshots/BadConnectionExample.gif)

## Good Connection Example
![Network Analysis Tool Good Connection Example](/Screenshots/GoodConnectionExample.gif)

## Full Log Folder Analysis
![Network Analysis Tool Good Connection Example](/Screenshots/LeagueNetLogAnalyserwithCharts.png)
