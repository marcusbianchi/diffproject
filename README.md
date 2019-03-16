# DiffProject
Project to calculate the diff between two files.

## Software Requirements
- .Net Core 2.2

## How to Run The Tests

On Command Line:

```
 dotnet restore -p .\DiffProject.WebAPI\
 
 dotnet test .\DiifProjectTest\
```

## How to Run the Project

On Command Line:

```
 dotnet restore -p .\DiffProject.WebAPI\
 
 dotnet run -p .\DiffProject.WebAPI\
```

## How to use the Solution

The solution is a Web solution to find the difference between two files. It has two endpoints one for receiving data and one for query data. 

### Sending Data

Any kind of text file can be sent to the endpoint:
- http://localhost:5000/v1/{id}/{left|right}

Where Id is the Id that you want to give the file and left and right are the directions of the file. The Processing requires on left and one right file.

The example below show how to do it with curl (Requires the instalation of curl on windows(https://curl.haxx.se/latest.cgi?curl=win64-ssl-sspi)):
```
 curl -H "Content-Type: application/json" -X POST http://localhost:5000/v1/test/left -d "{\"Name\":\"Test Value\"}"
```

### Queryng data

The data can be queried on:
- http://localhost:5000/v1/{id}

Where Id is the Id that was given to the process.

The data can also be acessed on the User interface accessing the following link on the browser.
- http://localhost:5000/swagger/index.html

## Future Improvments
- Implement MapReduce to increase the processing speed of the Diff process
- Create a Docker file to the solution