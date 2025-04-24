🧮 OrderCalc

OrderCalc is a distributed system built with .NET 8, designed to receive, manage, and process customer orders with a focus on performance, scalability, and maintainability. It handles product calculations, pricing, and order logic, and integrates with RabbitMQ for asynchronous communication between microservices.
🚀 Technologies

    .NET 8

    ASP.NET Core

    Entity Framework Core

    RabbitMQ

    SQLite

    Docker

🏗️ Project Structure

    OrderCalc.API: API layer (in development)

    OrderCalc.Domain: Domain layer with interfaces and entities

    OrderCalc.Infrastructure: EF context, repositories, and external integrations

    OrderCalc.Consumer: Worker service responsible for consuming messages and calculating taxes

🔄 Migrations and Database Updates
Create a new migration

dotnet ef --startup-project ../OrderCalc.API/ migrations add create_table_order --verbose

    Note: Run this command from the OrderCalc.Infrastructure folder.

Update the database with the latest migration

dotnet ef --startup-project ../OrderCalc.API/ database update --verbose

🐇 Messaging Requirements (RabbitMQ)

This system depends on a running RabbitMQ instance. You can spin up a container using Docker with the following command:

docker run \
    --name rabbitmq-afm \
    -p 5672:5672 \
    -p 15672:15672 \
    --network afm-network \
    -e RABBITMQ_USERNAME=rabbitmq \
    -e RABBITMQ_PASSWORD=@Abc1234 \
    -e RABBITMQ_MANAGEMENT_ALLOW_WEB_ACCESS=true \
    -v bitnami:/bitnami/rabbitmq/mnesia \
    -d bitnami/rabbitmq:latest

    The management UI will be available at: http://localhost:15672
    Username: rabbitmq
    Password: @Abc1234

🧪 Database

The system currently uses SQLite for local persistence. The database file (ordercalc.db) is located inside the OrderCalc.API project.

You can explore the database using tools such as:

    SQLite Viewer extension in VS Code

    Applications like DB Browser for SQLite

📬 Message Flow

    A newly created order is published to the order.queue

    The Consumer (Worker) consumes the message, calculates the tax, updates the order, and sends it to the next stage

📌 TODOs

Implement calculations for the Tax Reform

Send the processed order to another queue for the "External Product B" microservice

    Finalize the OrderCalc.API and expose API endpoints

Let me know if you’d like this README to include badges (e.g., .NET version, build status, license), VS Code/Visual Studio run instructions, or deployment notes.