#!/bin/bash

dotnet "./src/AnomalyDetection.Publish/AnomalyDetection.dll" "./log_input/batch_log.json" "./log_input/stream_log.json" "./log_output/flagged_purchases.json" --verbose
