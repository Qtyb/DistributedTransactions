# DistributedTransactions
Project for university telecomunications conference

## Services
| Service       | Port          |
| ------------- |:-------------:|
| Product       | 6000          |
| Basket        | 6010          |
| Order         | 6020          |
| Shipping      | 6030          |
| User          | 6040          |
| Db            | 1433          |
| RabbitMq      | 5672          |
| RabbitMqGui   | 15672         |
| Jenkins       | 8080          |


## Docker configuration
For now RabbitMq Jenkins MsSql Server are configured in docker-compose.yml
Move to /docker and run them with
```
docker-compose up
```
Services has dockerfiles which are WRONG, becuase of name change of each project.
Basket service although has this fixed as a proof concept.
You can run him with docker typing in /src/Basket/Basket
```
docker build --tag basketwebapi:1.0 .
docker run --publish 6010:80 --detach --name basketwebapi basketwebapi:1.0
```
Then browse to [http://localhost:6010/swagger](http://localhost:6010/swagger)

## Database migrations
Clean database need migrations applied.
To do so you need first to install tools:
```
dotnet tool install --global dotnet-ef
```
After this operation you may need to restart system.

Database connection string is in every service in appsettings.json file.
You might need to change it to connect to database.
You can apply migrations for one service going to service solution directory and typing
```
dotnet ef database update
```
