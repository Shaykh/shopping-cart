# 🛒 Basket Microservice - .NET 9

[![.NET 9](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/9.0)
[![Redis](https://img.shields.io/badge/Cache-Redis-red.svg)](https://redis.io/)
[![RabbitMQ](https://img.shields.io/badge/Messaging-RabbitMQ-orange.svg)](https://www.rabbitmq.com/)

Ce microservice gère la logique métier du panier d'achat. Il est conçu pour être performant (latence < 15ms), scalable horizontalement et résilient aux pannes grâce à une architecture orientée événements.

## 🏗️ Architecture

Le service suit les principes de la **Clean Architecture** et du pattern **CQRS** (via MediatR) :

* **Basket.API** : Point d'entrée (Endpoints, Swagger, Middlewares).
* **Basket.Application** : Logique de gestion (Commands/Queries, MediatR, FluentValidation).
* **Basket.Domain** : Entités métier pures (ShoppingCart, CartItem).
* **Basket.Infrastructure** : Persistence Redis, Clients gRPC (Discount) et Communication EventBus.

## 🚀 Stack Technique

* **Framework :** ASP.NET Core 9
* **Cache :** Redis (StackExchange.Redis)
* **Bus de Messages :** MassTransit + RabbitMQ
* **Communication Synchrone :** gRPC (intégration avec le service Discount)
* **Validation :** FluentValidation
* **Mapping :** Mapster

## 🛠️ Installation & Démarrage

### Prérequis

* Docker Desktop
* .NET 9 SDK

### 1. Lancer l'infrastructure (Redis & RabbitMQ)

À la racine du projet, exécutez la commande suivante :

```bash
docker-compose up -d
```

### 2. Lancer le service

```bash
cd src/Basket.API
dotnet run
```

## 📋 Points de terminaison (API)

| Méthode | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/api/v1/basket/{userName}` | Récupère le panier d'un utilisateur |
| `POST` | `/api/v1/basket` | Crée ou met à jour un panier |
| `DELETE` | `/api/v1/basket/{userName}` | Supprime un panier |
| `POST` | `/api/v1/basket/checkout` | Valide le panier et déclenche la commande |

## ⚙️ Configuration

Modifiez le fichier `appsettings.json` pour pointer vers vos services :

```json
{
  "CacheSettings": {
    "ConnectionString": "localhost:6379"
  },
  "EventBusSettings": {
    "HostAddress": "rabbitmq://localhost"
  },
  "GrpcSettings": {
    "DiscountUrl": "http://localhost:5003"
  }
}
```

## 📡 Intégrations Event-Driven

Le service publie l'événement suivant lors du checkout :

* **`BasketCheckoutEvent`** : Consommé par le microservice `Ordering.API` pour transformer le panier en commande ferme.

## 🔍 Observabilité

* **Health Checks** : Accessible sur `/hc`.
* **Swagger UI** : Accessible sur `/swagger` en mode développement.
* **Logs** : Structurés avec Serilog pour une intégration ELK/Splunk.
