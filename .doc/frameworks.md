# Descrição dos Serviços e Frameworks

Este projeto utiliza uma combinação de serviços e frameworks para garantir escalabilidade, resiliência e boas práticas de desenvolvimento. Abaixo está uma breve descrição do que cada um faz:

---

## **PostgreSQL**
- **Função**: Banco de dados relacional usado para armazenar dados de forma persistente.
- **Uso no Projeto**: Armazena os lançamentos (débitos e créditos) e os dados consolidados diários.
- **Benefícios**: Garante ACID (Atomicidade, Consistência, Isolamento, Durabilidade) e suporte a transações.

---

## **Redis**
- **Função**: Banco de dados em memória usado para cache e mensageria.
- **Uso no Projeto**: 
  - **Cache**: Armazena resultados de consultas frequentes (ex: saldo consolidado) para reduzir a carga no PostgreSQL.
  - **Mensageria**: Utiliza Redis Streams para comunicação assíncrona entre serviços (ex: eventos de lançamentos).
- **Benefícios**: Alta performance e baixa latência.

---


## **Entity Framework Core / Migrations**
- **Função**: ORM (Object-Relational Mapper) para acesso a banco de dados e gerenciamento de esquemas.
- **Uso no Projeto**: 
  - Facilita a interação com o PostgreSQL.
  - Migrations são usadas para criar e atualizar o esquema do banco de dados.
- **Benefícios**: Produtividade e manutenção simplificada do banco de dados.

---

## **FluentValidation**
- **Função**: Biblioteca para validação de dados de forma fluente e declarativa.
- **Uso no Projeto**: Valida entradas de API (ex: lançamentos) antes de processá-las.
- **Benefícios**: Código de validação limpo e fácil de manter.

---

## **MediatR**
- **Função**: Biblioteca que implementa o padrão Mediator para desacoplar comandos e consultas.
- **Uso no Projeto**: Gerencia a comunicação entre camadas, como a execução de casos de uso.
- **Benefícios**: Separação clara de responsabilidades e facilita testes unitários.

---

## **Microsoft.Extensions.Caching.StackExchangeRedis**
- **Função**: Integração do Redis com o .NET para caching.
- **Uso no Projeto**: Configura e gerencia o cache distribuído usando Redis.
- **Benefícios**: Facilita a implementação de cache em aplicações .NET.

---

## **StackExchange.Redis**
- **Função**: Biblioteca .NET para interação com o Redis.
- **Uso no Projeto**: Publica e consome mensagens via Redis Streams e gerencia o cache.
- **Benefícios**: Alta performance e suporte avançado para operações no Redis.

---

## **Swashbuckle.AspNetCore.SwaggerUI**
- **Função**: Gera documentação automática da API no formato Swagger/OpenAPI.
- **Uso no Projeto**: Disponibiliza uma interface interativa para testar e explorar os endpoints da API.
- **Benefícios**: Facilita a integração e o entendimento da API por desenvolvedores e consumidores.

---

## **APM Server**
 O APM Server coleta métricas e logs das aplicações e os envia para o Elasticsearch.

---

## **Elasticsearch**
2️⃣ O Elasticsearch armazena e indexa os dados recebidos.

---

## **Kibana**
3️⃣ O Kibana permite visualizar os logs e métricas em dashboards interativos.

---