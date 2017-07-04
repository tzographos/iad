dotnet restore
dotnet build --output ..\AnomalyDetection.Build
dotnet publish --output ..\AnomalyDetection.Publish --configuration Release