#!/bin/bash

dotnet "./src/AnomalyDetection.Publish/AnomalyDetection.dll" "./sample_dataset/batch_log.json" "./sample_dataset/stream_log.json" "./sample_dataset/flagged_purchases.json" --verbose