# vending-machine
This project has 4 microserivce 
1. Identity Server
2. User
3. Product
4. Transaction

All above microservice project are based on Clean architecture, MediatR and CQRS pattern along with Domain Driven Design.

# set up development environment

docker network create --driver bridge devnet-vending 

docker run -d -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Strong@passw0rd" -p 3000:1433 --name sqlvending-identity --network devnet-vending -h sql.database -d mcr.microsoft.com/mssql/server:2019-latest 

docker run -d -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Strong@passw0rd" -p 3100:1433 --name sqlvending-product --network devnet-vending -h sql.database -d mcr.microsoft.com/mssql/server:2019-latest 
