# Microservices Example

This repository contains a collection of microservices built using ASP.NET Core, MassTransit, RabbitMQ, and Docker.

## Services

- **CatalogService**: Manages catalog items.
- **OrderService**: Handles order processing.
- **PaymentService**: Manages payment transactions.
- **ShippingService**: Manages shipping operations.
- **GatewayService**: Acts as an API gateway for external clients.

## Prerequisites

- Docker
- Docker Compose

## Setup

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/saharNofal/Microservices.git
   cd Microservices
Build and Run the Services:
      docker-compose up --build
