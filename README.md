Insight Coding Challenge - Anomaly detection
===============
---
### Purpose

This software submission is made for the Insight Coding Challenge 2017 – Anomaly Detection. The software calculates when a purchase made by a user is “out the ordinary” for the standards of his social network purchasing habits. 
The formal definition of the problem is available [here](https://github.com/InsightDataScience/anomaly_detection/blob/master/README.md).


### Formalization & Algorithmic approach
The approach followed is outlined below.

#### Log inputs
The `batch_log.json` and `stream_log.json` files are read *in memory*. This is done only for technicality (keep the lock on files for as little as possible). Besides, in a production scenario, files would have been arriving from a (streaming) API.

 The `batch_log.json` is used first to read the social network parameters (depth and maximum consecutive purchases). The rest of the data are used to "train" the social network. No anomaly detection is done at this point. This is done when the other log file is evaluated.

>It is given from the challenge description, that the purchase and relationship data in the input log files is "arriving" in the order from older to newer. Code that handles cases where newer records contain older data is considered tedious.

#### NetworkBuilder
It's the name of the component that builds the social network, adding persons and relationships according to the streaming data.

##### Social network traversal
It is comprised of a `NetworkTraverser` component responsible for finding the social group of a given person but for a given depth. It uses a simple but efficient BFS algorithm (with a twist in order to track the depth).

##### Customer purchase merging
Another important component is `PurchaseMerger`. Instead of using the naive approach of holding *all* the purchase records of every individual and scan on the entire array for the past M records of it's social group, a different approach is used:  Every person has it's own rolling list of M latest purchases (`RollingList` component, a.k.a. a simple LinkedList). In this manner, i.e. by scattering and then gathering back the data, we increase the locality of reference of the required data.

##### Statistics calculator
A simple component that calculates the statistical parameters: count, mean, variance, standard deviation of the *population* (it is debatable why the stdev of the population is used (1/N) and not the biased stdev of the sample (1/N-1) ).


### Technology
This software is --I assume-- different that most software submitted for this Challenge, in the sense that is uses the brand new technology from Microsoft, called .NET Core. 
.NET Core brings the richness, speed and maturity of the .NET Framework to Linux and Mac. Binaries compiled with .NET Core run **natively** on both Linux, Windows and Mac.
The code is written in C# 6 using [Visual Studio Code](https://code.visualstudio.com/) (on Linux) and [Visual Studio](https://www.visualstudio.com/) (on Windows).
The library used for JSON parsing and manipulation is the excellent [Json.NET](http://www.newtonsoft.com/json) from NewtonSoft.
For the unit testing project, [x-Unit.NET](https://xunit.github.io/) was chosen.
>#### Dependencies
>**The application requires .NET Core version 1.1.2 to be installed, in order to run. You can find detailed installation instructions for both Linux, Windows and Mac [here](https://www.microsoft.com/net/core#linuxubuntu). On all operating systems, administrative privileges are required for the installation.**


#### Why not a container?
Containers are popular these days, especially Docker. Why not submit the C# based software in it? The answer is performance impediments caused by running software via another layer.



#### Execution
The location of the application DLL is in the folder
`./src/AnomalyDetection.Publish` and it's execution is made using the following command syntax:

    dotnet <path_to_execution_dll_file> <path_to_batch_log_json> <path_to_stream_log_json> <path_to_output_log_json> [--verbose]

The `--verbose` switch displays detailed information about the  anomalies detected as soon as they are detected.

For example on Linux:

    dotnet "./src/AnomalyDetection.Publish/AnomalyDetection.dll" "./log_input/batch_log.json" "./log_input/stream_log.json" "./log_output/flagged_purchases.json" --verbose 


### Results
Due to an intolerance of the test_suite regarding the newline characters and spaces, it was unable to provide a screenshot of a successful test message. 
However running the first suite test (test1) using the `run.sh` script, yielded the same correct result.
![run.sh](http://i66.tinypic.com/2mrgktu.png)

Additionally, the 50MB sample file was used as well (can be seen by running `run-sample.sh`).
![run-sample.sh](http://i65.tinypic.com/15qp1e0.png)

The application has been tested successfully in Windows, Linux Mint 18.1 and Ubuntu Linux 16.10.

### Unit tests
There are unit tests for every component used in the application. Feel free to (you should) look around in the `./src/AnomalyDetection.Tests` folder.

### Final words
It was a nice challenge and a lovely weekend :)
